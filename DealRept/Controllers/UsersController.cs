using DealRept.Data;
using DealRept.Models;
using DealRept.Models.ViewModel;
using DealRept.Services.AdminEmailsService;
using DealRept.Services.EmailService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DealRept.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class UsersController : Controller
    {

        private readonly IdentityContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<UsersController> _logger;

        private readonly string[] _permittedExtensions = { ".txt" };

        private readonly IEmailsGetter _adminEmailGetter;

        public UsersController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager,
            IdentityContext dbx, IConfiguration configuration, IEmailSender emailSender,
            ILogger<UsersController> logger, IEmailsGetter adminEmailsGetter)
        {
            _context = dbx;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _emailSender = emailSender;
            _logger = logger;
            _adminEmailGetter = adminEmailsGetter;
        }

        public async Task<IActionResult>Index(int? pageNumber, SortModel sortModel, string sortOrder,
            SearchModel searchModel, string currentFilterUserFullName, int currentFilterEmployeeNumber,
            string currentFilterRoleID, string currentFilterAccountApproval,
            DateTime? currentFilterRegistrationDateFrom, DateTime? currentFilterRegistrationDateTo)
        {
            sortModel.CurrentSort = sortOrder;
            sortModel.RegistrationDateSortParam =
                String.IsNullOrEmpty(sortOrder) ? "RegistrationDate" : "";
            sortModel.EmployeeNumberSortParam = sortOrder == "EmployeeNumber" ? "EmployeeNumber_desc" : "EmployeeNumber";
            sortModel.UserFullNameSortParam = sortOrder == "UserFullName" ? "UserFullName_desc" : "UserFullName";

            IEnumerable<User> users = from user in _context.Users.AsEnumerable<User>()
                                      select new User
                                      {
                                          Id = user.Id,
                                          Email = user.Email,
                                          FirstName = user.FirstName,
                                          MiddleName = user.MiddleName,
                                          LastName = user.LastName,
                                          EmployeeNumber = user.EmployeeNumber,
                                          LockoutEnd = user.LockoutEnd,
                                          IsApproved=user.IsApproved,
                                          RegistrationDateUTC=user.RegistrationDateUTC,
                                          Roles = from role in _context.Roles from index in (from userRole in _context.UserRoles where userRole.UserId == user.Id select userRole.RoleId) where role.Id == index select role.Name
                                      };

            searchModel.Roles = _context.Roles;

            var userRoles = await _context.UserRoles.ToListAsync();

            if (searchModel.UserFullName != null
                || searchModel.EmployeeNumber != default 
                || searchModel.RoleID != null
                ||searchModel.AccountApprovalSearch!=null
                || searchModel.RegistrationDateFrom != null || searchModel.RegistrationDateTo != null)
            {
                pageNumber = 1;
                if (searchModel.AccountApprovalSearch == null)
                {
                    searchModel.AccountApprovalSearch = "includeAllAccounts";
                }
            }

            else
            {
                searchModel.UserFullName = currentFilterUserFullName;
                searchModel.EmployeeNumber = currentFilterEmployeeNumber;
                searchModel.RoleID = currentFilterRoleID;
                searchModel.AccountApprovalSearch = currentFilterAccountApproval;
                searchModel.RegistrationDateFrom = currentFilterRegistrationDateFrom;
                searchModel.RegistrationDateTo=currentFilterRegistrationDateTo;
                if (searchModel.AccountApprovalSearch == null)
                {
                    searchModel.AccountApprovalSearch = "includeAllAccounts";
                }
            }

            switch (sortOrder)
            {
                case "RegistrationDate":
                    users = users.OrderBy(u => u.RegistrationDateUTC);break;
                case "EmployeeNumber":
                    users = users.OrderBy(u => u.EmployeeNumber); break;
                case "EmployeeNumber_desc":
                    users = users.OrderByDescending(u => u.EmployeeNumber); break;
                case "UserFullName":
                    users = users.OrderBy(u => u.UserFullName); break;
                case "UserFullName_desc":
                    users = users.OrderByDescending(u => u.UserFullName); break;
                default:
                    users = users.OrderByDescending(u => u.RegistrationDateUTC); break;
            }

            if (searchModel != null)
            {
                switch (searchModel.AccountApprovalSearch)
                {
                    case ("onlyApproved"):
                        users = users.Where(u => u.IsApproved == true); break;
                    case ("onlyNotApproved"):
                        users = users.Where(u => u.IsApproved == false); break;
                    default:

                        break;
                }

                if (!String.IsNullOrEmpty(searchModel.UserFullName))
                {
                    users = users.Where(u => u.UserFullName.ToLower().Contains(searchModel.UserFullName.ToLower()));
                }

                if (searchModel.EmployeeNumber != default)
                {
                    users = users.Where(u => u.EmployeeNumber == searchModel.EmployeeNumber);
                }
                if (!String.IsNullOrEmpty(searchModel.RoleID))
                {
                    users = from user in users
                            join userRole
                            in (from userRole in userRoles where userRole.RoleId == searchModel.RoleID select userRole)
                            on user.Id equals userRole.UserId
                            select user;
                }
                if (searchModel.RegistrationDateFrom != null)
                {
                    users = users.Where(u => (u.RegistrationDateUTC.Date >= searchModel.RegistrationDateFrom));
                }
                if (searchModel.RegistrationDateTo != null)
                {
                    users = users.Where(u => (u.RegistrationDateUTC.Date <= searchModel.RegistrationDateTo));
                }
            }

            int pageSize = 3;

            return View(PaginatedList<User>.Create(users, pageNumber ?? 1, pageSize, searchModel, sortModel));
        }

        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                throw new ElementNotFoundException("Something went wrong during a user edit,"
                    ,new ArgumentNullException($"{nameof(id)}"));
            }

            User user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                throw new ElementNotFoundException("Something went wrong during a user edit,",
                    new ArgumentNullException($"{nameof(user)}"));
            }

            user.Roles = await _userManager.GetRolesAsync(user);
            if (user.LockoutEnd != null)
            {
                user.LockoutEnd = user.LockoutEnd.Value.UtcDateTime.AddHours(_configuration.GetValue<int>("TimeZoneUTCDifference")); ;
            }

            UserViewModel model = ViewModelUserFactory.Details(user, _configuration.GetValue<int>("TimeZoneUTCDifference"));

            return View(model);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                throw new ElementNotFoundException("Something went wrong during a user edit,",
                    new ArgumentNullException($"{nameof(id)}"));
            }

            User user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                throw new ElementNotFoundException("Something went wrong during a user edit,",
                                        new ArgumentNullException($"{nameof(user)}"));
            }

            user.Roles = await _userManager.GetRolesAsync(user);
            List<AssignedRoleViewModel> allRolesWithAssignment = PopulateAssignedRoles(user);

            if (user.LockoutEnd != null)
            {
                user.LockoutEnd = user.LockoutEnd.Value.UtcDateTime.AddHours(_configuration.GetValue<int>("TimeZoneUTCDifference"));
            }

            LockoutEndDate lockOutEndDate = new LockoutEndDate(user.LockoutEnd);
            
            int.TryParse(_configuration["TimeZoneUTCDifference"], out int timeZoneUTCDifference);
            var model = ViewModelUserFactory.Edit(user, allRolesWithAssignment, lockOutEndDate,timeZoneUTCDifference, user.IsApproved);
            return View(model);
        }

        private List<AssignedRoleViewModel> PopulateAssignedRoles(User user)
        {
            IQueryable<IdentityRole> allRoles;
            if (user.IsApproved)
            {
                allRoles = _context.Roles.Where(r => r.NormalizedName != "JUSTREGISTERED");
            }
            else
            {
                allRoles = _context.Roles.Where(r => r.NormalizedName != "SUSPENDED");
            }
            var userRoles = new HashSet<string>(from roleUser in _context.UserRoles
                                                where roleUser.UserId == user.Id
                                                select roleUser.RoleId);
            if (userRoles == null)
            {
                throw new ProccesingException("Something went wrong during User editting,",
                    new ArgumentNullException($"{nameof(userRoles)}"));
            }

            var viewModel = new List<AssignedRoleViewModel>();
            foreach (var role in allRoles)
            {
                viewModel.Add(new AssignedRoleViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name,
                    Assigned = userRoles.Contains(role.Id)
                });
            }
            return viewModel;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserViewModel userViewModel)
        {
            int.TryParse(_configuration["TimeZoneUTCDifference"], out int timeZoneUTCDifference);

            if (ModelState.IsValid)
            {
                if (userViewModel == null)
                {
                    throw new ElementNotFoundException("Something went wrong during a user edit,",
                        new ArgumentNullException($"{nameof(userViewModel)}"));
                }
                User userToUpdate = await _userManager.FindByIdAsync(userViewModel.User.Id);

                if (userToUpdate == null)
                {
                    throw new ElementNotFoundException("Something went wrong during a user edit,",
                                               new ArgumentNullException($"{nameof(userToUpdate)}"));
                }

                if (userToUpdate.Email==_configuration["AdminConfiguration:Email"])
                {
                    ModelState.AddModelError("", "You can't edit root admin.");
                    return View(ViewModelUserFactory.Edit(userViewModel.User, userViewModel.AllRolesWithAssignment, userViewModel.LockoutEndDate,timeZoneUTCDifference, userViewModel.OldApproved));
                }

                userToUpdate.FirstName = userViewModel.User.FirstName;
                userToUpdate.LastName = userViewModel.User.LastName;
                userToUpdate.MiddleName = userViewModel.User.MiddleName;
                userToUpdate.LockoutEnd = userViewModel.LockoutEndDate.GetDateTimeOffset();

                if (userViewModel.AllRolesWithAssignment.Any())
                {
                    foreach (var role in userViewModel.AllRolesWithAssignment)
                    {
                        if (role.Assigned)
                        {
                            if (!await _userManager.IsInRoleAsync(userToUpdate, role.RoleName))
                            {
                                await _userManager.AddToRoleAsync(userToUpdate, role.RoleName);
                            }
                        }
                        else
                        {
                            if (await _userManager.IsInRoleAsync(userToUpdate, role.RoleName))
                            {
                                await _userManager.RemoveFromRoleAsync(userToUpdate, role.RoleName);
                            }
                        }
                    }
                }

                bool isApprovedChanged = false;

                if (userToUpdate.IsApproved==false&&userViewModel.User.IsApproved==true)
                {
                    userToUpdate.IsApproved = userViewModel.User.IsApproved;
                    isApprovedChanged = true;
                }

                try
                {
                    await _context.SaveChangesAsync();
                    if (isApprovedChanged)
                    {
                        string link = Url.ActionLink(nameof(Index), "Home"); 
                        Message message = new Message(new string[] { userToUpdate.Email }, "Your account is approved", $"Your account has been approved by our Admin, now you can work with Deal#Rept, click or copy the link: " +
                            $"{link}", null,true);
                        await _emailSender.SendEmailAsync(message);
                    }
                    return RedirectToAction(nameof(Details), new { id = userToUpdate.Id });

                }
                catch (DbUpdateException dBUEx)
                {
                    _logger.LogError(dBUEx, "Unable to save changes");
                    ModelState.AddModelError("", "Unable to save changes. " +
                            "Try again, and if the problem persists " +
                            "see your system administrator. ");
                }
            }
            return View(ViewModelUserFactory.Edit(userViewModel.User, userViewModel.AllRolesWithAssignment, userViewModel.LockoutEndDate,timeZoneUTCDifference, userViewModel.OldApproved));
        }

        [HttpPost]
        public async Task<bool> Delete(string id)
        {
            if (id == null)
            {
                _logger.LogError(new ArgumentNullException(nameof(id)), "Delete failed");
                return false;
            }

            User userToDelete = await _userManager.FindByIdAsync(id);
            if (userToDelete == null)
            {
                _logger.LogError(new DbUpdateConcurrencyException($"{nameof(userToDelete)} was not found."), "UserToDelete was not found");
                return true;
            }
            if (userToDelete.Email == _configuration["AdminConfiguration:Email"])
            {
                return false;
            }

            try
            {
                await _userManager.DeleteAsync(userToDelete);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Delete failed");
                return false;
            }
        }

        [HttpGet]
        public async Task<IActionResult> SendAdminMessage()
        {
            List<RoleWithSendTo> rolesWithSendTo = await(
                from roles in _context.Roles
                select new RoleWithSendTo { RoleName=roles.Name, IsSendTo=false}).ToListAsync<RoleWithSendTo>();

            return View(new AdminMessageViewModel { Roles = rolesWithSendTo });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendAdminMessage(AdminMessageViewModel adminMessage)
        {
            if (adminMessage.Roles.Where(m=>m.IsSendTo==true).Count()==0&& String.IsNullOrEmpty(adminMessage.EmployeeNumbersTo))
            {
                ModelState.AddModelError("", "Choose atleast one recipient.");
            }

            if (adminMessage.Attachment?.Length > _configuration.GetValue<long>("EmailAttachmentSizeLimit"))
            {
                ModelState.AddModelError(nameof(AdminMessageViewModel.Attachment), "Attachment file size can't be more than 2 MB.");
            }

            if (ModelState.IsValid)
            {
                if (adminMessage == null)
                {
                    throw new ElementNotFoundException("The Message cannot be found,",
                    new ArgumentNullException($"{nameof(adminMessage)}"));
                }

                List<string> emailAddress = new List<string>();

                for (int i = 0; i < adminMessage.Roles.Count; i++)
                {
                    if (adminMessage.Roles[i].IsSendTo)
                    {
                        List<string> emailList = await _adminEmailGetter.GetEmailAddressesAsync(adminMessage.Roles[i].RoleName);
                        emailAddress.AddRange(emailList);
                    }

                }

                emailAddress=emailAddress.Distinct().ToList();
                List<string> emailAddressesPersons;

                if (!String.IsNullOrEmpty(adminMessage.EmployeeNumbersTo))
                {
                    char[] delimiterChars = {' ', ','};
                    string[] numbersStr = adminMessage.EmployeeNumbersTo.Split(delimiterChars, System.StringSplitOptions.RemoveEmptyEntries);
                    int[] numbersInt = new int[numbersStr.Length];
                    for (int i = 0; i < numbersStr.Length; i++)
                    {
                         int.TryParse(numbersStr[i], out numbersInt[i]);
                    }
                   
                    emailAddressesPersons =  (from user in _context.Users.AsEnumerable()
                                              join number in numbersInt
                                        on user.EmployeeNumber equals number
                                        select user.Email).ToList();
                    emailAddress.AddRange(emailAddressesPersons);
                    emailAddress=emailAddress.Distinct().ToList();
                }

                IFormFileCollection files = Request.Form.Files.Any()?Request.Form.Files:new FormFileCollection();
                
                Message message = new Message(emailAddress, adminMessage.Subject, adminMessage.Content, files,true);
                
                try
                {
                    await _emailSender.SendEmailAsync(message);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Unable to send message");
                    ModelState.AddModelError("", "Unable to send message. Try again, and if the problem persists " +
                            "see application logs.");
                }
            }

            return View(adminMessage);
        }
    }
}
