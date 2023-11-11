using AutoMapper;
using DealRept.Data;
using DealRept.Models;
using DealRept.Models.ViewModel;
using DealRept.Services.AdminEmailsService;
using DealRept.Services.EmailService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;


namespace DealRept.Controllers
{
    public class AccountController : Controller
    {
        private readonly IdentityContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<AccountController> _logger;
        private readonly IConfiguration _configuration;

        private readonly IEmailsGetter _adminEmailGetter;

        public AccountController(IMapper mapper, UserManager<User> userManager,
            SignInManager<User> signInManager,
            IEmailSender emailSender, ILogger<AccountController> logger, IConfiguration configuration,
            IdentityContext context, IEmailsGetter adminEmailGetter)
        {
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
            _configuration = configuration;
            _context = context;
            _adminEmailGetter = adminEmailGetter;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserRegistrationViewModel userViewModel)
        {
            if (userViewModel == null)
            {
                throw new ProccesingException("Something went wrong during User registration,",
                    new ArgumentNullException($"{nameof(userViewModel)}"));
            }

            if (!ModelState.IsValid)
            {
                return View(userViewModel);
            }

            var user = _mapper.Map<User>(userViewModel);

            if (_userManager.Users.Any(u => u.EmployeeNumber == user.EmployeeNumber))
            {
                ModelState.AddModelError(nameof(UserRegistrationViewModel.EmployeeNumber), $"User with Employee Number {user.EmployeeNumber} already exists.");
            }

            if (!ModelState.IsValid)
            {
                return View(userViewModel);
            }

            user.RegistrationDateUTC = DateTime.Now.ToUniversalTime();

            var result = await _userManager.CreateAsync(user, userViewModel.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.TryAddModelError("", error.Description);
                }
                return View(userViewModel);
            }

            string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            string confirmationLink = Url.Action(nameof(ConfirmEmail), "Account", new { token, email = user.Email }, Request.Scheme);

            Message message = new Message(new string[] { user.Email }, "Confirmation email link", $"To confirm your email please click or copy the link: {confirmationLink}", null, true);
            await _emailSender.SendEmailAsync(message);

            await _userManager.AddToRoleAsync(user, "JustRegistered");

            return RedirectToAction(nameof(SuccessRegistration));
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            User userToConfirm = await _userManager.FindByEmailAsync(email);

            if (userToConfirm == null)
            {
                throw new ProccesingException("Something went wrong during email confirmation,",
                    new ArgumentNullException($"{nameof(userToConfirm)}"));
            }

            IdentityResult result = await _userManager.ConfirmEmailAsync(userToConfirm, token);
            if (result.Succeeded)
            {
                string userLink = Url.Action(nameof(Details), "Users", new { id = userToConfirm.Id }, Request.Scheme);

                Message message = new Message(await _adminEmailGetter.GetEmailAddressesAsync("Administrator"), "New User to approve", userLink, null, true);

                await _emailSender.SendEmailAsync(message);


                return View(nameof(ConfirmEmail));
            }

            _logger.LogError(new ProccesingException(), "Error during email:  {Email} confirmation", email);
            throw new ProccesingException("Something went wrong during email confirmation,");
        }

        [HttpGet]
        public IActionResult SuccessRegistration()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            UserLoginViewModel userLoginViewModel = new UserLoginViewModel() { ReturnUrl = returnUrl };
            return View(userLoginViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserLoginViewModel userLoginViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(userLoginViewModel);
            }

            var result = await _signInManager.PasswordSignInAsync(userLoginViewModel.Email, userLoginViewModel.Password,
                userLoginViewModel.RememberMe, true);

            if (result.Succeeded)
            {
                return RedirectToLocal(userLoginViewModel.ReturnUrl ?? "/");
            }

            if (result.IsLockedOut)
            {
                string forgotPassLink = Url.Action(nameof(ForgotPassword), "Account", new { }, Request.Scheme);
                string content = string.Format("Your account is locked out, to reset your password, please click or copy this link: {0}", forgotPassLink);

                Message message = new Message(new string[] { userLoginViewModel.Email }, "Locked out account information", content, null, true);
                await _emailSender.SendEmailAsync(message);


                ModelState.AddModelError("", "The account is locked out.");
                return View();
            }
            else
            {
                ModelState.AddModelError("", "You have entered an invalid Email Address or Password.");
                return View();
            }
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        public IActionResult Details()
        {
            var claims = User.Claims;
            User user = new User
            {
                Roles = claims?.Where(x => x.Type.Equals("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", StringComparison.OrdinalIgnoreCase)).Select(x => x.Value).ToList(),
                FirstName = claims?.FirstOrDefault(x => x.Type.Equals("firstname", StringComparison.OrdinalIgnoreCase))?.Value,
                LastName = claims?.FirstOrDefault(x => x.Type.Equals("lastname", StringComparison.OrdinalIgnoreCase))?.Value,
                MiddleName = claims?.FirstOrDefault(x => x.Type.Equals("middlename", StringComparison.OrdinalIgnoreCase))?.Value,
                EmployeeNumber = int.Parse(claims?.FirstOrDefault(x => x.Type.Equals("employeeNumber", StringComparison.OrdinalIgnoreCase))?.Value),

                Email = claims?.FirstOrDefault(x => x.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", StringComparison.OrdinalIgnoreCase))?.Value,

                IsApproved = Boolean.Parse(claims?.FirstOrDefault(x => x.Type.Equals("isApproved", StringComparison.OrdinalIgnoreCase))?.Value),

                RegistrationDateUTC = DateTime.Parse(claims?.FirstOrDefault(x => x.Type.Equals("registrationDate", StringComparison.OrdinalIgnoreCase))?.Value)
            };

            user.MiddleName = user.MiddleName == "noMiddleName" ? null : user.MiddleName;

            return View(user);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel forgotPasswordViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(forgotPasswordViewModel);
            }

            User user = await _userManager.FindByEmailAsync(forgotPasswordViewModel.Email);

            if (user == null)
            {
                return RedirectToAction(nameof(ForgotPasswordConfirmation));
            }

            string token = await _userManager.GeneratePasswordResetTokenAsync(user);
            string callback = Url.Action(nameof(ResetPassword), "Account", new { token, email = user.Email }, Request.Scheme);

            Message message = new Message(new string[] { user.Email }, "Reset password token", $"To reset your password," +
                $" please click or copy the link: {callback}", null, true);
            await _emailSender.SendEmailAsync(message);

            return RedirectToAction(nameof(ForgotPasswordConfirmation));
        }

        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            ResetPasswordViewModel model =
                new ResetPasswordViewModel { Token = token, Email = email };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPasswordViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(resetPasswordViewModel);
            }

            User user = await _userManager.FindByEmailAsync(resetPasswordViewModel.Email);
            if (user == null)
            {
                RedirectToAction(nameof(ResetPasswordConfirmation));
            }

            var resetPassResult = await _userManager.ResetPasswordAsync(user, resetPasswordViewModel.Token, resetPasswordViewModel.Password);
            if (!resetPassResult.Succeeded)
            {
                foreach (var error in resetPassResult.Errors)
                {
                    ModelState.TryAddModelError("", error.Description);
                }
                return View();
            }

            await _userManager.SetLockoutEndDateAsync(user, new DateTime(2000, 1, 1));
            return RedirectToAction(nameof(ResetPasswordConfirmation));
        }

        [HttpGet]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "ContractStaff, BranchStaff, Administrator")]
        public IActionResult SendMessageToAdmins(string email)
        {
            return View(new MessageToAdminViewModel() { UserEmail = email });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendMessageToAdmins(MessageToAdminViewModel messageToAdmin)
        {
            if (messageToAdmin.Attachment?.Length > _configuration.GetValue<long>("EmailAttachmentSizeLimit"))
            {
                ModelState.AddModelError(nameof(MessageToAdminViewModel.Attachment), "Attachment file size can't be more than 2 MB.");
            }

            if (ModelState.IsValid)
            {
                if (messageToAdmin == null)
                {
                    throw new ElementNotFoundException("The Message cannot be found,",
                     new ArgumentNullException($"{nameof(messageToAdmin)}"));
                }

                User userFrom = await _userManager.FindByEmailAsync(messageToAdmin.UserEmail);

                if (userFrom == null)
                {
                    throw new ElementNotFoundException("The User cannot be found,",
                     new ArgumentNullException($"{nameof(userFrom)}"));
                }

                string userLink = Url.Action(nameof(Details), "Users", new { id = userFrom.Id }, Request.Scheme);

                IFormFileCollection files = Request.Form.Files.Any() ? Request.Form.Files : new FormFileCollection();

                string messageSubject = $"From {messageToAdmin.UserEmail}: {messageToAdmin.Subject}";

                string messageContent = String.Format("<p style='color:#002d77;'>{0}</p><p style='color:#ff6825;'>This message is from: <span style='color:#002d77;'>{1}</span></p>", messageToAdmin.Content, userLink);

                Message message = new Message(await _adminEmailGetter.GetEmailAddressesAsync("Administrator"), messageSubject, messageContent, files, false);

                try
                {
                    await _emailSender.SendEmailAsync(message);
                    return RedirectToAction(nameof(Index), "Home");
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Unable to send message");
                    ModelState.AddModelError("", "Unable to send message. Try again, and if the problem persists " +
                            "see your system administrator.");
                }
            }

            return View(messageToAdmin);
        }
    }
}
