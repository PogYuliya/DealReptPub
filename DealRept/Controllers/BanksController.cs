using ClosedXML.Excel;
using DealRept.Models;
using DealRept.Models.ViewModel;
using DealRept.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DealRept.Controllers
{
    [Authorize(Roles = "ContractStaff, BranchStaff, Administrator")]
    public class BanksController : Controller
    {
        private const string PatternName = @"^[a-zA-Z][a-zA-Z\s-,]+[a-zA-Z]$";
        private const string PatternCode = @"[0-9]{6}";
        private readonly DealDbContext _context;
        private readonly ILogger<BanksController> _logger;
        private readonly IConfiguration _configuration;

        public BanksController(DealDbContext ctx, ILogger<BanksController> logger,
            IConfiguration configuration)
        {
            _context = ctx;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index(int? pageNumber, SortModel sortModel, string sortOrder, SearchModel searchModel,
           string currentFilterNameCode, string currentFilterLastAdded)
        {
            sortModel.CurrentSort = sortOrder;
            sortModel.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "Name_desc" : "";

            if (searchModel.NameCodeBankSearch != null || searchModel.LastAdded != default)
            {
                pageNumber = 1;
            }
            else
            {
                searchModel.LastAdded = (currentFilterLastAdded != default) ? Int32.Parse(currentFilterLastAdded) : default;
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
                if (searchModel.LastAdded != default)
                {
                    banks = banks.OrderByDescending(b => b.ID).Take(searchModel.LastAdded);
                }
            }

            switch (sortOrder)
            {
                case "Name_desc":
                    banks = banks.OrderByDescending(b => b.Name); break;
                default:
                    banks = banks.OrderBy(b => b.Name); break;
            }

            int pageSize = 10;

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

        [Authorize(Roles = "ContractStaff")]
        public IActionResult ImportData()
        {
            ImportFileViewModel fileVM = new ImportFileViewModel();
            return View(fileVM);
        }

        [Authorize(Roles = "ContractStaff")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> ImportData(ImportFileViewModel importFileVM)
        {
            if (importFileVM == null)
            {
                throw new ElementNotFoundException("Something went wrong during City import data,",
                    new ArgumentNullException($"{nameof(importFileVM)}"));
            }

            long _fileSizeLimit = _configuration.GetSection("ImportData").GetValue<long>("ImportFileSizeLimit");
            string[] _permittedExtensions = { ".xlsx" };

            if (!ModelState.IsValid)
            {
                return View(importFileVM);
            }

            await FileHelpers.ProcessFormFile<FileUpload>(
                importFileVM.FileToUpload.FormFile, ModelState, _permittedExtensions,
                _fileSizeLimit, _logger);

            if (!ModelState.IsValid)
            {
                return View(importFileVM);
            }

            List<BankViewModel> banks = new List<BankViewModel>();
            const int colBankCode = 3;
            const int colBankName = 4;
            int batchLimit = _configuration.GetSection("ImportData").GetValue<int>("BatchLimit");

            try
            {
                using Stream stream = importFileVM.FileToUpload.FormFile.OpenReadStream();
                using XLWorkbook workBook = new XLWorkbook(stream);
                IXLWorksheet workSheet = workBook.Worksheet(1);

                if (workSheet.LastRowUsed().RowNumber() > (batchLimit + 2)
                || workSheet.FirstRowUsed().CellsUsed().Count() > 3
                || workSheet.FirstRowUsed().Cell(colBankName).GetString() != "Bank Name"
                || workSheet.FirstRowUsed().Cell(colBankCode).GetString() != "Bank Code"
                 || workSheet.FirstRowUsed().RowBelow().Cell(colBankName).IsEmpty())
                {
                    throw new ProccesingException("Template was changed or is empty.");
                }

                IXLAddress firstPossibleAddress = workSheet.FirstRowUsed().FirstCell().Address;
                IXLAddress lastPossibleAddress = workSheet.LastCellUsed().Address;
                IXLRange bankRange = workSheet.Range(firstPossibleAddress, lastPossibleAddress).RangeUsed();

                IXLTable bankTable = bankRange.AsTable();

                if (bankTable.RowCount() > (batchLimit + 1) || bankTable.ColumnCount() != 3)
                {
                    throw new ProccesingException("Template was changed.");
                }

                banks = (from bankRow in bankTable.DataRange.Rows()
                         select new BankViewModel
                         {
                             Name = bankRow.Field("Bank Name").GetString(),
                             Code = bankRow.Field("Bank Code").GetString()
                         }).ToList();

                banks.RemoveAll(b => String.IsNullOrEmpty(b.Code) && String.IsNullOrEmpty(b.Name));

                if (banks.Count == 0)
                {
                    throw new ProccesingException("Banks list is empty.");
                }

            }
            catch (ProccesingException procEx)
            {
                _logger.LogError(procEx, "Unable to upload file.");
                ModelState.AddModelError("", "Unable to upload file. Make sure you are using a downloaded template. " +
               "Try again, and if the problem persists " +
               "see your system administrator.");
            }
            catch (FileFormatException fFEX)
            {
                _logger.LogError(fFEX, "Unable to upload file.");
                ModelState.AddModelError("", "Unable to upload file. Make sure you are using a downloaded template. " +
               "Try again, and if the problem persists " +
               "see your system administrator.");
            }
            catch (IOException IOEx)
            {
                _logger.LogError(IOEx, "Unable to upload file.");
                ModelState.AddModelError("", "Unable to upload file. Make sure you are using a downloaded template. " +
               "Try again, and if the problem persists " +
               "see your system administrator.");
            }

            if (!ModelState.IsValid)
            {
                return View(importFileVM);
            }

            int successCount = 0;

            Regex patternName = new Regex(PatternName);
            Regex patternCode = new Regex(PatternCode);

            List<BankViewModel> validatedBanks = new List<BankViewModel>();

            foreach (BankViewModel bank in banks)
            {
                if (patternName.IsMatch(bank.Name) && patternCode.IsMatch(bank.Code))
                {
                    validatedBanks.Add(bank);
                }
            }

            string values = "";
            for (int i = 0; i < validatedBanks.Count; i++)
            {
                values += $"('{validatedBanks[i].Code}', '{validatedBanks[i].Name}')";
                if (i != validatedBanks.Count - 1)
                {
                    values += ",";
                }
            }

            successCount = (validatedBanks.Count == default) ? 0 : await _context.Database.ExecuteSqlRawAsync($"INSERT IGNORE Banks (Code, Name) VALUES {values}");

            return RedirectToAction(nameof(ImportDataReport), new
            {
                successCount,
                unsuccessCount = banks.Count - successCount
            });

        }

        public FileStreamResult DownloadTemplate()
        {
            string pathToTemplate = _configuration.GetSection("ImportData").GetValue<string>("PathToBankTemplate");
            if (System.IO.File.Exists(pathToTemplate))
            {

                FileStream fs = new FileStream(pathToTemplate, FileMode.Open, FileAccess.Read);

                if (fs == null)
                {
                    throw new ProccesingException("Something went wrong during banks template downloading,",
                        new ArgumentNullException($"{nameof(fs)}"));
                }

                FileStreamResult fsResult = new FileStreamResult(fs, System.Net.Mime.MediaTypeNames.Application.Octet)
                {
                    FileDownloadName = Path.GetFileName(pathToTemplate)
                };

                if (fsResult == null)
                {
                    throw new ProccesingException("Something went wrong during downloading a template,",
                        new ArgumentNullException($"{nameof(fsResult)}"));
                }

                return fsResult;
            }
            else
            {
                throw new ElementNotFoundException("The template cannot be found,",
                    new FileNotFoundException($"{nameof(pathToTemplate)}"));
            }
        }

        [Authorize(Roles = "ContractStaff")]
        public IActionResult ImportDataReport(int successCount, int unsuccessCount)
        {
            ImportReportViewModel importReportViewModel = new ImportReportViewModel()
            {
                SuccessImportCount = successCount,
                UnSuccessImportCount = unsuccessCount
            };

            return View(importReportViewModel);
        }
    }
}
