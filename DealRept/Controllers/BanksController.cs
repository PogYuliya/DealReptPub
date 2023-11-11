using DealRept.Models;
using DealRept.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DealRept.Controllers
{
    [Authorize(Roles = "ContractStaff, BranchStaff, Administrator")]
    public class BanksController : Controller
    {
        private readonly DealDbContext _context;
        private readonly ILogger<BanksController> _logger;

        public BanksController(DealDbContext ctx, ILogger<BanksController> logger)
        {
            _context = ctx;
            _logger = logger;
        }

        public async Task<IActionResult> Index(int? pageNumber, SortModel sortModel, string sortOrder, SearchModel searchModel,
           string currentFilterNameCode)
        {
            sortModel.CurrentSort = sortOrder;
            sortModel.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "Name_desc" : "";

            if (searchModel.NameCodeBankSearch != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchModel.NameCodeBankSearch = currentFilterNameCode;
            }

            IQueryable<Bank> banks = _context.Banks.AsNoTracking()
                .Select(b => new Bank
                {
                    Name = b.Name,
                    Code = b.Code,
                    ID = b.ID,
                    SuppliersCount = (from s in _context.Suppliers where s.BankID == b.ID select s.ID).Count()
                });

            if (searchModel != null)
            {
                if (searchModel.NameCodeBankSearch != null)
                {
                    banks = banks.Where(b => b.Name.Contains(searchModel.NameCodeBankSearch)
                    || b.Code.Contains(searchModel.NameCodeBankSearch));
                }
            }

            switch (sortOrder)
            {
                case "Name_desc":
                    banks = banks.OrderByDescending(b => b.Name); break;
                default:
                    banks = banks.OrderBy(b => b.Name); break;
            }

            int pageSize = 4;

            return View(await PaginatedList<Bank>.CreateAsync(banks.AsNoTracking(), pageNumber ?? 1, pageSize, searchModel, sortModel));
        }

        [Authorize(Roles = "ContractStaff")]
        public IActionResult Create()
        {
            return View(new Bank());
        }

        [Authorize(Roles = "ContractStaff")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Create([Bind("Name", "Code")] Bank bankToCreate)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Banks.Add(bankToCreate);
                    await _context.SaveChangesAsync();

                    Bank newBank = await _context.Banks.FirstOrDefaultAsync(b => b.Code == bankToCreate.Code);

                    if (newBank == null)
                    {
                        throw new ElementNotFoundException("Something went wrong during Bank creation,"
                            , new ArgumentNullException($"{nameof(newBank)}"));
                    }

                    return RedirectToAction(nameof(Index), new { currentFilterNameCode = newBank.Code });
                }
            }
            catch (DbUpdateException dBUpEx)
            {
                _logger.LogError(dBUpEx, "Unable to save changes to DB");
                ModelState.AddModelError("", "Unable to save changes. " +
                                "Try again, and if the problem persists " +
                                "see your system administrator.");
            }

            return View(bankToCreate);
        }

        [Authorize(Roles = "ContractStaff")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                throw new ElementNotFoundException("Something went wrong during Bank edit,",
                    new ArgumentNullException($"{nameof(id)}"));
            }

            Bank bankToEdit = await _context.Banks.FirstOrDefaultAsync(b => b.ID == id);

            if (bankToEdit == null)
            {
                throw new ElementNotFoundException("Something went wrong during Bank edit,",
                                        new ArgumentNullException($"{nameof(bankToEdit)}"));
            }

            return View(bankToEdit);
        }

        [Authorize(Roles = "ContractStaff")]
        [ValidateAntiForgeryToken]
        [HttpPost, ActionName("Edit")]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                throw new ElementNotFoundException("Something went wrong during a Bank edit,",
                    new ArgumentNullException($"{nameof(id)}"));
            }

            Bank bankToUpdate = await _context.Banks.FirstOrDefaultAsync(b => b.ID == id);

            if (bankToUpdate == null)
            {
                throw new ElementNotFoundException("Something went wrong during a Bank edit,",
                                            new ArgumentNullException($"{nameof(bankToUpdate)}"));
            }

            if (await TryUpdateModelAsync<Bank>(bankToUpdate, "", b => b.Name))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index), new { currentFilterNameCode = bankToUpdate.Code });
                }
                catch (DbUpdateException dbUpEx)
                {
                    _logger.LogError(dbUpEx, "Unable to save changes to DB.");
                    ModelState.AddModelError("", "Unable to save changes. " +
                            "Try again, and if the problem persists " +
                            "see your system administrator.");
                }
            }

            return View(bankToUpdate);
        }

        [Authorize(Roles = "ContractStaff")]
        [HttpPost]
        public async Task<bool> Delete(int? id)
        {
            if (id == null)
            {
                _logger.LogError(new ArgumentNullException(nameof(id)), "Delete failed");
                return false;
            }

            Bank bankToDelete = await _context.Banks.FirstOrDefaultAsync(b => b.ID == id);
            if (bankToDelete == null)
            {
                _logger.LogError(new ArgumentNullException(nameof(bankToDelete)), "BankToDelete was not found");
                return true;
            }
            try
            {
                _context.Banks.Remove(bankToDelete);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException dBUpEx)
            {
                _logger.LogError(dBUpEx, "Delete failed");
                return false;
            }
        }

    }
}
