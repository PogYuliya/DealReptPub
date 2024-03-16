using ClosedXML.Excel;
using DealRept.Models;
using DealRept.Models.ViewModel;
using DealRept.Services.RazorRenderService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DealRept.Controllers
{
    [Authorize(Roles = "ContractStaff, BranchStaff, Administrator")]
    public class SuppliersController : Controller
    {
        private readonly DealDbContext _context;
        private readonly ILogger<SuppliersController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IRazorRendererHelper _razorRendererHelper;


        public SuppliersController(DealDbContext ctx, ILogger<SuppliersController> logger,
            IConfiguration config, IRazorRendererHelper razorRendererHelper)
        {
            _context = ctx;
            _logger = logger;
            _configuration = config;
            _razorRendererHelper = razorRendererHelper;
        }

        public async Task<IActionResult> Index(int? pageNumber, string sortOrder,
            SortModel sortModel, SearchModel searchModel, string currentFilterSupplierNameCode,
            string currentFilterLegalAddress, string currentFilterBankNameCode,
            string currentFilterSpecialization, string currentFilterNegativeRemarks)
        {
            sortModel.CurrentSort = sortOrder;
            sortModel.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "Name_desc" : "";
            sortModel.LegalAddressSortParm = sortOrder == "LegalAddress" ? "LegalAddress_desc" : "LegalAddress";
            sortModel.SpecializationSortParm = sortOrder == "Specialization" ? "Specialization_desc" : "Specialization";

            if (searchModel.NameCodeSupplierSearch != null || searchModel.LegalAddressSearch != null
                || searchModel.SpecializationSearch != null ||
                searchModel.NegativeRemarksSearch != null || searchModel.NameCodeBankSearch != null)
            {
                pageNumber = 1;
            }

            else
            {
                searchModel.NameCodeSupplierSearch = currentFilterSupplierNameCode;
                searchModel.LegalAddressSearch = currentFilterLegalAddress;
                searchModel.NegativeRemarksSearch = currentFilterNegativeRemarks;
                searchModel.SpecializationSearch = currentFilterSpecialization;
                searchModel.NameCodeBankSearch = currentFilterBankNameCode;
                if (searchModel.NegativeRemarksSearch == null)
                {
                    searchModel.NegativeRemarksSearch = "includeNegativeRemarks";
                }
            }

            IQueryable<Supplier> suppliers = _context.Suppliers.Include(s => s.Specialization)
                .Include(s => s.City).Include(s => s.Bank)
                .AsNoTracking().Select(s => new Supplier
                {
                    ID = s.ID,
                    Name = s.Name,
                    LegalCode = s.LegalCode,
                    City = s.City,
                    Bank = s.Bank,
                    StreetBuilding = s.StreetBuilding,
                    PostalIndex = s.PostalIndex,
                    PastContractCount = (from pc in _context.PastContracts where pc.SupplierID == s.ID select pc).Count(),
                    CurrentContractCount = (from cc in _context.CurrentContracts where cc.SupplierID == s.ID select cc).Count(),
                    NegativeRemarks = s.NegativeRemarks,
                    Specialization = s.Specialization
                });

            if (!String.IsNullOrEmpty(searchModel.NameCodeSupplierSearch))
            {
                suppliers = suppliers.Where(s => s.Name.Contains(searchModel.NameCodeSupplierSearch)
                || s.LegalCode.Contains(searchModel.NameCodeSupplierSearch));
            }

            if (!String.IsNullOrEmpty(searchModel.LegalAddressSearch))
            {
                suppliers = suppliers.Where(s => s.City.Name.Contains(searchModel.LegalAddressSearch)
                || s.PostalIndex.Contains(searchModel.LegalAddressSearch)
                || s.StreetBuilding.Contains(searchModel.LegalAddressSearch));

            }

            if (!String.IsNullOrEmpty(searchModel.SpecializationSearch))
            {
                suppliers = suppliers.Where(s => s.Specialization.Name.Contains(searchModel.SpecializationSearch));
            }

            if (!String.IsNullOrEmpty(searchModel.NameCodeBankSearch))
            {
                suppliers = suppliers.Where(s => s.Bank.Name.Contains(searchModel.NameCodeBankSearch)
                  || s.Bank.Code.Contains(searchModel.NameCodeBankSearch));
            }

            switch (searchModel.NegativeRemarksSearch)
            {
                case ("onlyWithNegativeRemarks"):
                    suppliers = suppliers.Where(s => s.NegativeRemarks != null); break;
                case ("excludeNegativeRemarks"):
                    suppliers = suppliers.Where(s => s.NegativeRemarks == null); break;
                default:

                    break;
            }

            switch (sortOrder)
            {
                case "Name_desc":
                    suppliers = suppliers.OrderByDescending(s => s.Name); break;
                case "LegalAddress":
                    suppliers = suppliers.OrderBy(s => s.PostalIndex)
                        .ThenBy(s => s.City.Name)
                        .ThenBy(s => s.StreetBuilding); break;
                case "LegalAddress_desc":
                    suppliers = suppliers.OrderByDescending(s => s.PostalIndex).ThenByDescending(s => s.City.Name)
                        .ThenByDescending(s => s.StreetBuilding); break;
                case "Specialization":
                    suppliers = suppliers.OrderBy(s => s.Specialization.Name); break;
                case "Specialization_desc":
                    suppliers = suppliers.OrderByDescending(s => s.Specialization.Name); break;
                default:
                    suppliers = suppliers.OrderBy(s => s.Name);
                    break;
            }

            int pageSize = 5;
            return View(await PaginatedList<Supplier>.CreateAsync(suppliers.AsNoTracking(), pageNumber ?? 1, pageSize, searchModel, sortModel));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                throw new ElementNotFoundException("Something went wrong during a supplier edit,", new ArgumentNullException($"{nameof(id)}"));
            }

            Supplier supplier = await _context.Suppliers.Include(s => s.Bank).Include(s => s.City)
                .Include(s => s.Specialization)
               .AsNoTracking().FirstOrDefaultAsync(s => s.ID == id);
            if (supplier == null)
            {
                throw new ElementNotFoundException("The Supplier cannot be found", new ArgumentNullException($"{nameof(supplier)}"));
            }

            supplier.PastContractCount = (from pc in _context.PastContracts where pc.SupplierID == supplier.ID select pc).Count();
            supplier.CurrentContractCount = (from cc in _context.CurrentContracts where cc.SupplierID == supplier.ID select cc).Count();

            return View(supplier);
        }

        [Authorize(Roles = "ContractStaff")]
        public async Task<IActionResult> Create(int? cityID, int? bankID)
        {
            SelectList supplierSpecializations = await GetSpecializationsForSelect();
            SelectList cities = await GetCitiesForSelect();
            SelectList banks = await GetBanksForSelect();

            if (cityID != null)
            {
                City city = await _context.Cities.FirstOrDefaultAsync(c => c.ID == cityID);
                if (city == null)
                {
                    throw new ElementNotFoundException("Something went wrong during supplier creation,",
                     new ArgumentNullException($"{nameof(city)}"));
                }
                return View(ViewModelSupplierFactory.Create(new Supplier() { CityID = city.ID }, supplierSpecializations, cities, banks));
            }

            if (bankID != null)
            {
                Bank bank = await _context.Banks.FirstOrDefaultAsync(b => b.ID == bankID);
                if (bank == null)
                {
                    throw new ElementNotFoundException("Something went wrong during supplier creation,",
                     new ArgumentNullException($"{nameof(bank)}"));
                }
                return View(ViewModelSupplierFactory.Create(new Supplier() { BankID = bank.ID }, supplierSpecializations, cities, banks));
            }

            return View(ViewModelSupplierFactory.Create(new Supplier(), supplierSpecializations, cities, banks));
        }

        private async Task<SelectList> GetSpecializationsForSelect()
        {
            List<ModelForSelect2> specializationsForSelect = await (from specialization in _context.Specializations
                                                                    select new ModelForSelect2
                                                                    {
                                                                        ID = specialization.ID,
                                                                        NameLegalCode = specialization.Name
                                                                    }).ToListAsync();
            if (specializationsForSelect == null)
            {
                throw new ElementNotFoundException("Something went wrong during specialization getting,", new ArgumentNullException($"{nameof(specializationsForSelect)}"));
            }

            SelectList specializationsSelect = new SelectList(specializationsForSelect, "ID", "NameLegalCode");

            if (specializationsSelect == null)
            {
                throw new ElementNotFoundException("Something went wrong during specialization getting,", new ArgumentNullException($"{nameof(specializationsSelect)}"));
            }

            return specializationsSelect;
        }

        private async Task<SelectList> GetCitiesForSelect()
        {
            List<ModelForSelect2> citiesForSelect = await (from city in _context.Cities
                                                           select new ModelForSelect2
                                                           {
                                                               ID = city.ID,
                                                               NameLegalCode = city.Name
                                                           }).ToListAsync();
            if (citiesForSelect == null)
            {
                throw new ElementNotFoundException("Something went wrong during cities getting,", new ArgumentNullException($"{nameof(citiesForSelect)}"));
            }

            SelectList citiesSelect = new SelectList(citiesForSelect, "ID", "NameLegalCode");

            if (citiesSelect == null)
            {
                throw new ElementNotFoundException("Something went wrong during cities getting,", new ArgumentNullException($"{nameof(citiesSelect)}"));
            }

            return citiesSelect;
        }

        private async Task<SelectList> GetBanksForSelect()
        {
            List<ModelForSelect2> banksForSelect = await (from bank in _context.Banks
                                                          select new ModelForSelect2
                                                          {
                                                              ID = bank.ID,
                                                              NameLegalCode = $"{bank.Name} - {bank.Code}"
                                                          }).ToListAsync();
            if (banksForSelect == null)
            {
                throw new ElementNotFoundException("Something went wrong during banks getting,", new ArgumentNullException($"{nameof(banksForSelect)}"));
            }

            SelectList banksSelect = new SelectList(banksForSelect, "ID", "NameLegalCode");

            if (banksSelect == null)
            {
                throw new ElementNotFoundException("Something went wrong during banks getting,", new ArgumentNullException($"{nameof(banksSelect)}"));
            }

            return banksSelect;
        }

        [Authorize(Roles = "ContractStaff")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Create(SupplierViewModel supplierViewModel)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Suppliers.Add(supplierViewModel.Supplier);
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateException dBUEx)
                {
                    _logger.LogError(dBUEx, "Unable to save changes.");
                    ModelState.AddModelError("", "Unable to save changes. " +
                                    "Try again, and if the problem persists " +
                                    "see your system administrator. ");
                }

                Supplier newSupplier = await _context.Suppliers.FirstOrDefaultAsync(s => s.LegalCode == supplierViewModel.Supplier.LegalCode);

                if (newSupplier == null)
                {
                    throw new ProccesingException("Something went wrong during Supplier creation,", new ArgumentNullException($"{nameof(newSupplier)}"));
                }

                return RedirectToAction(nameof(Details), new { id = newSupplier.ID });
            }


            SelectList specializations = await GetSpecializationsForSelect();
            SelectList cities = await GetCitiesForSelect();
            SelectList banks = await GetBanksForSelect();

            return View(ViewModelSupplierFactory.Create(supplierViewModel.Supplier, specializations, cities, banks));
        }

        [Authorize(Roles = "ContractStaff")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                throw new ElementNotFoundException("Something went wrong during a supplier edit,",
                    new ArgumentNullException($"{nameof(id)}"));
            }

            Supplier supplierToEdit = await _context.Suppliers.Include(s => s.Bank)
                .Include(s => s.City).Include(s => s.Specialization).AsNoTracking().FirstOrDefaultAsync(s => s.ID == id);

            if (supplierToEdit == null)
            {
                throw new ElementNotFoundException("Something went wrong during a supplier edit,",
                    new ArgumentNullException($"{nameof(supplierToEdit)}"));
            }

            SelectList specializations = await GetSpecializationsForSelect();
            SelectList cities = await GetCitiesForSelect();
            SelectList banks = await GetBanksForSelect();

            SupplierViewModel model = ViewModelSupplierFactory.Edit(supplierToEdit, specializations,
               cities, banks);

            return View(model);
        }

        [Authorize(Roles = "ContractStaff")]
        [AutoValidateAntiforgeryToken]
        [HttpPost]
        public async Task<IActionResult> Edit(SupplierViewModel supplierViewModel)
        {
            if (ModelState.IsValid)
            {
                if (supplierViewModel.Supplier == null)
                {
                    throw new ElementNotFoundException("Something went wrong during a supplier edit,",
                        new ArgumentNullException($"{nameof(supplierViewModel.Supplier)}"));
                }

                Supplier supplierToUpdate = await _context.Suppliers.FirstOrDefaultAsync(s => s.ID == supplierViewModel.Supplier.ID);

                if (supplierToUpdate == null)
                {
                    throw new ElementNotFoundException("The Supplier cannot be found", new ArgumentNullException($"{nameof(supplierToUpdate)}"));
                }

                supplierToUpdate.Name = supplierViewModel.Supplier.Name;
                supplierToUpdate.StreetBuilding = supplierViewModel.Supplier.StreetBuilding;
                supplierToUpdate.PostalIndex = supplierViewModel.Supplier.PostalIndex;
                supplierToUpdate.BankAccount = supplierViewModel.Supplier.BankAccount.ToUpper();
                supplierToUpdate.CompanyTelephone = supplierViewModel.Supplier.CompanyTelephone;
                supplierToUpdate.CompanyEmail = supplierViewModel.Supplier.CompanyEmail;
                supplierToUpdate.ContactFirstName = supplierViewModel.Supplier.ContactFirstName;
                supplierToUpdate.ContactLastName = supplierViewModel.Supplier.ContactLastName;
                supplierToUpdate.ContactMiddleName = supplierViewModel.Supplier.ContactMiddleName;
                supplierToUpdate.ContactTelephone = supplierViewModel.Supplier.ContactTelephone;
                supplierToUpdate.ContactEmail = supplierViewModel.Supplier.ContactEmail;
                supplierToUpdate.DirectorFirstName = supplierViewModel.Supplier.DirectorFirstName;
                supplierToUpdate.DirectorLastName = supplierViewModel.Supplier.DirectorLastName;
                supplierToUpdate.DirectorMiddleName = supplierViewModel.Supplier.DirectorMiddleName;
                supplierToUpdate.NegativeRemarks = supplierViewModel.Supplier.NegativeRemarks;

                supplierToUpdate.SpecializationID = supplierViewModel.Supplier.SpecializationID;
                supplierToUpdate.BankID = supplierViewModel.Supplier.BankID;
                supplierToUpdate.CityID = supplierViewModel.Supplier.CityID;

                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Details), new { id = supplierToUpdate.ID });
                }
                catch (DbUpdateException dBUEx)
                {
                    _logger.LogError(dBUEx, "Unable to save changes");
                    ModelState.AddModelError("", "Unable to save changes. " +
                            "Try again, and if the problem persists " +
                            "see your system administrator. ");
                }
            }

            SelectList specializations = await GetSpecializationsForSelect();
            SelectList cities = await GetCitiesForSelect();
            SelectList banks = await GetBanksForSelect();

            SupplierViewModel model = ViewModelSupplierFactory.Edit(supplierViewModel.Supplier, specializations, cities, banks);
            return View(model);
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

            Supplier supplierToDelete = await _context.Suppliers.FirstOrDefaultAsync(s => s.ID == id);

            if (supplierToDelete == null)
            {
                _logger.LogError(new DbUpdateConcurrencyException($"{nameof(supplierToDelete)} was not found during a supplier delete."), "SupplierToDelete was not found");
                return true;
            }
            try
            {
                _context.Suppliers.Remove(supplierToDelete);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException eUp)
            {
                _logger.LogError(eUp, "Delete failed");
                return false;
            }
        }

        public IActionResult Export(SearchModel searchModel)
        {
            IQueryable<Supplier> suppliers = _context.Suppliers.Include(s => s.Specialization)
                .Include(s => s.City)
                .Include(s => s.Bank)
                .AsNoTracking().Select(s => new Supplier
                {
                    ID = s.ID,
                    Name = s.Name,
                    LegalCode = s.LegalCode,
                    City = s.City,
                    StreetBuilding = s.StreetBuilding,
                    PostalIndex = s.PostalIndex,
                    PastContractCount = (from pc in _context.PastContracts where pc.SupplierID == s.ID select pc).Count(),
                    CurrentContractCount = (from cc in _context.CurrentContracts where cc.SupplierID == s.ID select cc).Count(),
                    NegativeRemarks = s.NegativeRemarks,
                    Specialization = s.Specialization,
                    DirectorFirstName = s.DirectorFirstName,
                    DirectorLastName = s.DirectorLastName,
                    DirectorMiddleName = s.DirectorMiddleName,
                    CompanyTelephone = s.CompanyTelephone,
                    CompanyEmail = s.CompanyEmail,
                    Bank = s.Bank,
                    ContactFirstName = s.ContactFirstName,
                    ContactLastName = s.ContactLastName,
                    ContactMiddleName = s.ContactMiddleName,
                    ContactEmail = s.ContactEmail,
                    ContactTelephone = s.ContactTelephone,
                    BankAccount = s.BankAccount
                });

            if (!String.IsNullOrEmpty(searchModel.NameCodeSupplierSearch))
            {
                suppliers = suppliers.Where(s => s.Name.Contains(searchModel.NameCodeSupplierSearch)
                || s.LegalCode.Contains(searchModel.NameCodeSupplierSearch));
            }

            if (!String.IsNullOrEmpty(searchModel.LegalAddressSearch))
            {
                suppliers = suppliers.Where(s => s.City.Name.Contains(searchModel.LegalAddressSearch)
                || s.PostalIndex.Contains(searchModel.LegalAddressSearch)
                || s.StreetBuilding.Contains(searchModel.LegalAddressSearch));

            }

            if (!String.IsNullOrEmpty(searchModel.SpecializationSearch))
            {
                suppliers = suppliers.Where(s => s.Specialization.Name.Contains(searchModel.SpecializationSearch));

            }

            if (!string.IsNullOrEmpty(searchModel.NameCodeBankSearch))
            {
                suppliers = suppliers.Where(s => s.Bank.Name.Contains(searchModel.NameCodeBankSearch)
                  || s.Bank.Code.Contains(searchModel.NameCodeBankSearch));
            }

            switch (searchModel.NegativeRemarksSearch)
            {
                case ("onlyWithNegativeRemarks"):
                    suppliers = suppliers.Where(s => s.NegativeRemarks != null); break;
                case ("excludeNegativeRemarks"):
                    suppliers = suppliers.Where(s => s.NegativeRemarks == null); break;
                default:

                    break;
            }

            suppliers = suppliers.OrderBy(s => s.Name).ThenBy(s => s.LegalCode);

            /*create table*/
            DataTable dtSuppliers = new DataTable("Suppliers");
            dtSuppliers.Columns.AddRange(new DataColumn[14] {
                                        new DataColumn("Supplier Name", typeof(string)),
                                        new DataColumn("Legal Code", typeof(string)),
                                        new DataColumn("Legal Address",typeof(string)),
                                        new DataColumn("Specialization",typeof(string)),
                                        new DataColumn("Director Name", typeof(string)),
                                        new DataColumn("Telephone Number", typeof(string)),
                                        new DataColumn("Email Address", typeof(string)),
                                        new DataColumn("Negative Remarks", typeof(string)),
                                        new DataColumn("Banking Details", typeof(string)),
                                        new DataColumn("Contact Person", typeof(string)),
                                        new DataColumn("Contact Person Telephone Number", typeof(string)),
                                        new DataColumn("Contact Person Email Address", typeof(string)),
                                        new DataColumn("Current Contracts", typeof(int)),
                                        new DataColumn("Past Contracts", typeof(int))
            });
            foreach (Supplier supplier in suppliers)
            {
                dtSuppliers.Rows.Add(
                    supplier.Name,
                    supplier.LegalCode,
                    supplier.LegalAddress,
                    supplier.Specialization.Name,
                    supplier.DirectorFullName,
                    supplier.CompanyTelephone,
                    supplier.CompanyEmail,
                    supplier.NegativeRemarks,
                    supplier.BankingDetails,
                    supplier.ContactFullName,
                    supplier.ContactTelephone,
                    supplier.ContactEmail,
                    supplier.CurrentContractCount,
                    supplier.PastContractCount
                    );
            }

            /*export table*/
            try
            {
                using (XLWorkbook wbP = new XLWorkbook())
                {
                    IXLWorksheet ws = wbP.Worksheets.Add(dtSuppliers);

                    ws.Row(1).InsertRowsAbove(2);

                    //Styling dataTable
                    ws.Rows().AdjustToContents();
                    ws.Columns().AdjustToContents();

                    //Styling heading
                    ws.Row(3).Height = 40;
                    ws.Row(3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    ws.Row(3).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                    ws.Row(3).Style.Fill.BackgroundColor = XLColor.FromArgb(0xf8f9fa);
                    ws.Row(3).Style.Font.Bold = true;


                    ws.Column(1).Width = 30;
                    ws.Column(1).Style.Alignment.WrapText = true;

                    ws.Column(3).Width = 45;
                    ws.Column(3).Style.Alignment.WrapText = true;

                    ws.Column(4).Width = 30;
                    ws.Column(4).Style.Alignment.WrapText = true;

                    ws.Column(5).Width = 35;
                    ws.Column(5).Style.Alignment.WrapText = true;

                    ws.Column(7).Width = 25;
                    ws.Column(7).Style.Alignment.WrapText = true;

                    ws.Column(8).Width = 35;
                    ws.Column(8).Style.Alignment.WrapText = true;

                    ws.Column(9).Width = 35;
                    ws.Column(9).Style.Alignment.WrapText = true;

                    ws.Column(10).Width = 35;
                    ws.Column(10).Style.Alignment.WrapText = true;

                    ws.Column(11).Width = 20;
                    ws.Column(11).Style.Alignment.WrapText = true;

                    ws.Column(12).Width = 25;
                    ws.Column(12).Style.Alignment.WrapText = true;

                    ws.Column(13).Width = 11;
                    ws.Column(13).Style.Alignment.WrapText = true;

                    ws.Column(14).Width = 11;
                    ws.Column(14).Style.Alignment.WrapText = true;

                    CreateEmailHyperlinks(ws, 7, 3);

                    CreateEmailHyperlinks(ws, 12, 3);

                    ws.RangeUsed().Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    ws.RangeUsed().Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    ws.RangeUsed().Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    ws.RangeUsed().Style.Border.RightBorder = XLBorderStyleValues.Thin;

                    // Report Heading
                    ws.Cell(1, 1).Value = "Suppliers Report";
                    ws.Row(1).Style.Font.FontSize = 16;
                    ws.Row(1).Height = 25;
                    ws.Row(1).Style.Font.Bold = true;
                    ws.Range(1, 1, 1, 5).Merge();
                    ws.Range(1, 1, 1, 5).Style.Border.OutsideBorder = XLBorderStyleValues.None;
                    ws.Range(1, 1, 1, 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    ws.Range(1, 1, 1, 5).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                    using (MemoryStream stream = new MemoryStream())
                    {
                        wbP.SaveAs(stream);
                        string fileName = $"Suppliers_{DateTime.Now:ddMMyyyy_hhmmss}.xlsx";
                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ProccesingException("Something went wrong during report export,", ex);
            }
        }

        private void CreateEmailHyperlinks(IXLWorksheet workSheet, int emailColumnNumber, int headingRowNum)
        {
            if (workSheet == null)
            {
                throw new ProccesingException("Something went wrong during a report export,"
                    , new ArgumentNullException($"{nameof(workSheet)}"));
            }
            if (emailColumnNumber == default)
            {
                throw new ProccesingException("Something went wrong during report export,"
                                        , new ArgumentNullException($"{nameof(emailColumnNumber)}"));

            }
            if (headingRowNum == default)
            {
                throw new ProccesingException("Something went wrong during a report export,"
                                        , new ArgumentNullException($"{nameof(headingRowNum)}"));
            }
            var emailList =
                        workSheet.Column(emailColumnNumber).CellsUsed().Select(c =>
                        new { RowNum = c.Address.RowNumber, Value = c.Value.ToString() }).ToList();

            if (emailList == null)
            {
                throw new ProccesingException("Something went wrong during a report export,", new ArgumentNullException($"{nameof(emailList)}"));
            }
            foreach (var email in emailList)
            {
                if (email.RowNum != headingRowNum)
                {
                    workSheet.Cell(email.RowNum, emailColumnNumber).CreateHyperlink().ExternalAddress = new Uri($"mailto:{email.Value}");
                }
            }
        }

        public IActionResult ShowCharts()
        {
            return View();
        }

        [HttpPost]
        public async Task<IDictionary<string, Object>> GetChartData(bool isPastContractIncluded)
        {
            IDictionary<string, Object> data = new Dictionary<string, object>();

            IQueryable<Supplier> suppliers = _context.Suppliers.AsNoTracking()
                .Include(s => s.CurrentContracts).Include(s => s.PastContracts);

            var shortSuppliers = await (from supplier in suppliers
                                        select new ChartShortSupBranchVM
                                        {
                                            Code = supplier.LegalCode,
                                            CurrentContractsTotalAmt =
                                            supplier.CurrentContracts.Select(c => c.ContractAmount).Sum(),
                                            ContractsTotalAmt =
                                            supplier.CurrentContracts.Select(c => c.ContractAmount).Sum() +
                                            supplier.PastContracts.Select(c => c.ContractAmount).Sum(),
                                            CurrentContractsQuantity = supplier.CurrentContracts.Count,
                                            ContractsQuantity = supplier.PastContracts.Count + supplier.CurrentContracts.Count
                                        }).ToListAsync();


            if (shortSuppliers == null)
            {
                throw new ElementNotFoundException("The shortSuppliers cannot be null.",
                        new ArgumentNullException($"{nameof(shortSuppliers)}"));
            }


            #region Get data for quantity charts
            IEnumerable<IGrouping<int, ChartShortSupBranchVM>> groupedShortSupsByQuantity;

            groupedShortSupsByQuantity = isPastContractIncluded ?
            shortSuppliers.GroupBy(s => s.ContractsQuantity) :
            shortSuppliers.GroupBy(s => s.CurrentContractsQuantity);

            if (groupedShortSupsByQuantity == null)
            {
                throw new ElementNotFoundException("The groupedShortSupsByQuantity cannot be null.",
                         new ArgumentNullException($"{nameof(groupedShortSupsByQuantity)}"));
            }

            List<ChartSupBranchViewModel> suppliersByQuantity = new List<ChartSupBranchViewModel>();

            int lessThanMinTresholdQtyCount = 0;
            int moreThanMinTresholdQtyCount = 0;
            int moreThanMaxTresholdQtyCount = 0;

            int minQtyTreshold = Int32.Parse(_configuration["ChartConfiguration:MinQtyTreshold"]);
            int maxQtyTreshold = Int32.Parse(_configuration["ChartConfiguration:MaxQtyTreshold"]);

            foreach (var groupingShortSuppliers in groupedShortSupsByQuantity)
            {
                if (groupingShortSuppliers.Key <= minQtyTreshold)
                {
                    lessThanMinTresholdQtyCount += groupingShortSuppliers.Count();
                }
                else if (groupingShortSuppliers.Key > minQtyTreshold
                    && groupingShortSuppliers.Key <= maxQtyTreshold)
                {
                    moreThanMinTresholdQtyCount += groupingShortSuppliers.Count();

                }
                else if (groupingShortSuppliers.Key > maxQtyTreshold)
                {
                    moreThanMaxTresholdQtyCount += groupingShortSuppliers.Count();

                }
            }

            suppliersByQuantity.Add(new ChartSupBranchViewModel
            {
                Name = $"contracts qty <= {minQtyTreshold}",
                SupBranchQuantity = lessThanMinTresholdQtyCount
            });
            suppliersByQuantity.Add(new ChartSupBranchViewModel
            {
                Name = $"{minQtyTreshold} < contracts qty <= {maxQtyTreshold}",
                SupBranchQuantity = moreThanMinTresholdQtyCount
            });
            suppliersByQuantity.Add(new ChartSupBranchViewModel
            {
                Name = $"contracts qty > {maxQtyTreshold}",
                SupBranchQuantity = moreThanMaxTresholdQtyCount
            });
            #endregion

            #region Get data for amount charts

            List<ChartSupBranchViewModel> suppliersByAmmount = new List<ChartSupBranchViewModel>();

            int lessThanMinTresholdCount = 0;
            int moreThanMinTresholdCount = 0;
            int moreThanMaxTresholdCount = 0;

            decimal minAmtTreshold = decimal.Parse(_configuration["ChartConfiguration:MinAmtTreshold"]);
            decimal maxAmtTreshold = decimal.Parse(_configuration["ChartConfiguration:MaxAmtTreshold"]);

            if (isPastContractIncluded)
            {
                foreach (var supplier in shortSuppliers)
                {
                    if (supplier.ContractsTotalAmt <= minAmtTreshold)
                    {
                        lessThanMinTresholdCount++;
                    }
                    else if (supplier.ContractsTotalAmt > minAmtTreshold
                        && supplier.ContractsTotalAmt <= maxAmtTreshold)
                    {
                        moreThanMinTresholdCount++;
                    }
                    else if (supplier.ContractsTotalAmt > maxAmtTreshold)
                    {
                        moreThanMaxTresholdCount++;
                    }
                }
            }
            else
            {
                foreach (var supplier in shortSuppliers)
                {
                    if (supplier.CurrentContractsTotalAmt <= minAmtTreshold)
                    {
                        lessThanMinTresholdCount++;
                    }
                    else if (supplier.CurrentContractsTotalAmt > minAmtTreshold
                        && supplier.CurrentContractsTotalAmt <= maxAmtTreshold)
                    {
                        moreThanMinTresholdCount++;
                    }
                    else if (supplier.CurrentContractsTotalAmt > maxAmtTreshold)
                    {
                        moreThanMaxTresholdCount++;
                    }
                }
            }

            suppliersByAmmount.Add(
                new ChartSupBranchViewModel
                {
                    Name = $"contracts amt <= {minAmtTreshold / 1000}T",
                    SupBranchQuantity = lessThanMinTresholdCount
                });

            suppliersByAmmount.Add(
                new ChartSupBranchViewModel
                {
                    Name = $"{minAmtTreshold / 1000}T < contracts amt <= {maxAmtTreshold / 1000}T",
                    SupBranchQuantity = moreThanMinTresholdCount
                });

            suppliersByAmmount.Add(
                new ChartSupBranchViewModel
                {
                    Name = $"contracts amt > {maxAmtTreshold / 1000}T",
                    SupBranchQuantity = moreThanMaxTresholdCount
                });

            #endregion

            List<string> labelsQuantity = suppliersByQuantity.Select(s => s.Name).ToList<string>();
            data.Add("labelsQuantity", labelsQuantity);

            List<int> suppliersByContractQty = suppliersByQuantity.Select(s => s.SupBranchQuantity)
                .ToList<int>();
            data.Add("suppliersByContractQty", suppliersByContractQty);

            List<string> labelsAmount = suppliersByAmmount.Select(s => s.Name).ToList();
            data.Add("labelsAmount", labelsAmount);

            List<int> suppliersByContractAmt = suppliersByAmmount.Select(s => s.SupBranchQuantity).ToList();
            data.Add("suppliersByContractAmt", suppliersByContractAmt);

            data.Add("isPastContractIncluded", isPastContractIncluded);

            return data;
        }

        [HttpPost]
        public async Task<string> GetPDFData(int? id)
        {
            if (id == null)
            {
                throw new ElementNotFoundException("The Supplier cannot be found,", new ArgumentNullException($"{nameof(id)}"));
            }

            Supplier supplier = await _context.Suppliers.Include(s => s.Bank).Include(s => s.City)
                .Include(s => s.Specialization)
               .AsNoTracking().FirstOrDefaultAsync(s => s.ID == id);
            
            if (supplier == null)
            {
                throw new ElementNotFoundException("The Supplier cannot be found", new ArgumentNullException($"{nameof(supplier)}"));
            }

            supplier.PastContractCount = (from pc in _context.PastContracts where pc.SupplierID == supplier.ID select pc).Count();
            supplier.CurrentContractCount = (from cc in _context.CurrentContracts where cc.SupplierID == supplier.ID select cc).Count();
            
            string pdfContent = _razorRendererHelper.RenderPartialToString("Views/Suppliers/_PrintPDFPartial.cshtml", supplier);
            return pdfContent;
        }
    }
}
