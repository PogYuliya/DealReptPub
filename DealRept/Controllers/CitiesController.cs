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
    public class CitiesController : Controller
    {
        private const string Pattern = @"^[a-zA-Z][a-zA-Z\s-,]+[a-zA-Z]$";
        private readonly DealDbContext _context;
        private readonly ILogger<CitiesController> _logger;
        private readonly IConfiguration _configuration;

        public CitiesController(DealDbContext ctx,
            ILogger<CitiesController> logger, IConfiguration configuration)
        {
            _context = ctx;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index(int? pageNumber, SortModel sortModel, string sortOrder, SearchModel searchModel,
            string currentFilterName, string currentFilterLastAdded)
        {
            sortModel.CurrentSort = sortOrder;
            sortModel.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "Name_desc" : "";

            if (searchModel.NameCitySearch != null || searchModel.LastAdded != default)
            {
                pageNumber = 1;
            }
            else
            {
                searchModel.NameCitySearch = currentFilterName;
                searchModel.LastAdded = (currentFilterLastAdded != default) ? Int32.Parse(currentFilterLastAdded) : default;
            }

            IQueryable<City> cities = _context.Cities
                .AsNoTracking().Select(c => new City
                {
                    Name = c.Name,
                    ID = c.ID,
                    BranchesCount = (from b in _context.Branches where b.CityID == c.ID select b.ID).Count(),
                    SuppliersCount = (from s in _context.Suppliers where s.CityID == c.ID select s.ID).Count()
                });

            if (searchModel != null)
            {
                if (searchModel.NameCitySearch != null)
                {
                    cities = cities.Where(c => c.Name.Contains(searchModel.NameCitySearch));
                }
                if (searchModel.LastAdded != default)
                {
                    cities = cities.OrderByDescending(c => c.ID).Take(searchModel.LastAdded);
                }
            }

            switch (sortOrder)
            {
                case "Name_desc":
                    cities = cities.OrderByDescending(c => c.Name); break;
                default:
                    cities = cities.OrderBy(c => c.Name); break;
            }

            int pageSize = 10;

            return View(await PaginatedList<City>.CreateAsync(cities.AsNoTracking(), pageNumber ?? 1, pageSize, searchModel, sortModel));

        }

        [Authorize(Roles = "ContractStaff")]
        public IActionResult Create()
        {
            return View(new City());
        }

        [Authorize(Roles = "ContractStaff")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Create([Bind("Name")] City cityToCreate)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    _context.Cities.Add(cityToCreate);
                    await _context.SaveChangesAsync();

                    City newCity = await _context.Cities.FirstOrDefaultAsync(c => c.Name == cityToCreate.Name);

                    if (newCity == null)
                    {
                        throw new ElementNotFoundException("Something went wrong during City create,"
                            , new ArgumentNullException($"{nameof(newCity)}"));
                    }

                    return RedirectToAction(nameof(Index), new { currentFilterName = newCity.Name });
                }
            }
            catch (DbUpdateException dBUpEx)
            {
                _logger.LogError(dBUpEx, "Unable to save changes to DB");
                ModelState.AddModelError("", "Unable to save changes." +
                                "Try again, and if the problem persists " +
                                "see your system administrator.");
            }

            return View(cityToCreate);
        }

        [Authorize(Roles = "ContractStaff")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                throw new ElementNotFoundException("Something went wrong during City edit,",
                    new ArgumentNullException($"{nameof(id)}"));
            }

            City cityToEdit = await _context.Cities.FirstOrDefaultAsync(c => c.ID == id);

            if (cityToEdit == null)
            {
                throw new ElementNotFoundException("Something went wrong during City edit,",
                                        new ArgumentNullException($"{nameof(cityToEdit)}"));
            }

            return View(cityToEdit);
        }

        [Authorize(Roles = "ContractStaff")]
        [ValidateAntiForgeryToken]
        [HttpPost, ActionName("Edit")]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                throw new ElementNotFoundException("Something went wrong during City edit,",
                    new ArgumentNullException($"{nameof(id)}"));
            }

            City cityToUpdate = await _context.Cities.FirstOrDefaultAsync(c => c.ID == id);

            if (cityToUpdate == null)
            {
                throw new ElementNotFoundException("Something went wrong during City edit,",
                                            new ArgumentNullException($"{nameof(cityToUpdate)}"));
            }

            if (await TryUpdateModelAsync<City>(cityToUpdate, "", c => c.Name))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index), new { currentFilterName = cityToUpdate.Name });
                }
                catch (DbUpdateException dbUpEx)
                {
                    _logger.LogError(dbUpEx, "Unable to save changes to DB.");
                    ModelState.AddModelError("", "Unable to save changes. " +
                            "Try again, and if the problem persists " +
                            "see your system administrator.");
                }
            }

            return View(cityToUpdate);
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

            City cityToDelete = await _context.Cities.FirstOrDefaultAsync(c => c.ID == id);

            if (cityToDelete == null)
            {
                _logger.LogError(new DbUpdateConcurrencyException("CityToDelete was not found during a city delete."), "CityToDelete was not found.");
                return true;
            }
            try
            {
                _context.Cities.Remove(cityToDelete);
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

            List<string> cities = new List<string>();
            const int colCityName = 3;
            int batchLimit = _configuration.GetSection("ImportData").GetValue<int>("BatchLimit");

            try
            {
                using Stream stream = importFileVM.FileToUpload.FormFile.OpenReadStream();
                using XLWorkbook workBook = new XLWorkbook(stream);
                IXLWorksheet workSheet = workBook.Worksheet(1);

                if (workSheet.LastRowUsed().RowNumber() > (batchLimit + 2)
                || workSheet.FirstRowUsed().CellsUsed().Count() > 2
                || workSheet.FirstRowUsed().Cell(colCityName).GetString() != "City Name"
                 || workSheet.FirstRowUsed().RowBelow().Cell(colCityName).IsEmpty())
                {
                    throw new ProccesingException("Template was changed or is empty.");
                }

                IXLAddress firstPossibleAddress = workSheet.FirstRowUsed().FirstCell().Address;
                IXLAddress lastPossibleAddress = workSheet.LastCellUsed().Address;
                IXLRange cityRange = workSheet.Range(firstPossibleAddress, lastPossibleAddress).RangeUsed();

                IXLTable cityTable = cityRange.AsTable();

                if (cityTable.RowCount() > (batchLimit + 1) || cityTable.ColumnCount() != 2)
                {
                    throw new ProccesingException("Template was changed.");
                }

                cities = cityTable.DataRange.Rows()
              .Select(cityRow => cityRow.Field("City Name").GetString())
              .ToList();

                cities.RemoveAll(c => String.IsNullOrEmpty(c));

                if (cities.Count == 0)
                {
                    throw new ProccesingException("Cities list is empty.");
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

            Regex pattern = new Regex(Pattern);

            List<string> validatedCities = new List<string>();

            foreach (string city in cities)
            {
                if (pattern.IsMatch(city))
                {
                    validatedCities.Add(city);
                }
            }

            string values = "";
            for (int i = 0; i < validatedCities.Count; i++)
            {
                values += $"('{validatedCities[i]}')";
                if (i != validatedCities.Count - 1)
                {
                    values += ",";
                }
            }

            successCount = (validatedCities.Count == default) ? 0 : await _context.Database.ExecuteSqlRawAsync($"INSERT IGNORE Cities (Name) VALUES {values}");

            return RedirectToAction(nameof(ImportDataReport), new
            {
                successCount,
                unsuccessCount = cities.Count - successCount
            });

        }

        public FileStreamResult DownloadTemplate()
        {
            string pathToTemplate = _configuration.GetSection("ImportData").GetValue<string>("PathToCityTemplate");
            if (System.IO.File.Exists(pathToTemplate))
            {

                FileStream fs = new FileStream(pathToTemplate, FileMode.Open, FileAccess.Read);

                if (fs == null)
                {
                    throw new ProccesingException("Something went wrong during cities template downloading,",
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
