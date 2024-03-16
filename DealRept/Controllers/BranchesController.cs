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
    public class BranchesController : Controller
    {
        private readonly DealDbContext _context;
        private readonly ILogger<BranchesController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IRazorRendererHelper _razorRendererHelper;



        public BranchesController(DealDbContext ctx, ILogger<BranchesController> logger, 
            IConfiguration config, IRazorRendererHelper razorRendererHelper)
        {
            _context = ctx;
            _logger = logger;
            _configuration = config;
            _razorRendererHelper = razorRendererHelper;
        }

        public async Task<IActionResult> Index(int? pageNumber, SortModel sortModel, string sortOrder, SearchModel searchModel,
            string currentFilterName, string currentFilterPostalAddress, string currentFilterHeadName)
        {

            sortModel.CurrentSort = sortOrder;
            sortModel.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "Name_desc" : "";
            sortModel.PostalAddressSortParm = sortOrder == "PostalAddress" ? "PostalAddress_desc" : "PostalAddress";
            sortModel.HeadFullNameSortParm = sortOrder == "HeadFullName" ? "HeadFullName_desc" : "HeadFullName";

            if (searchModel.NameBranchSearch != null
                || searchModel.PostalAddressSearch != null || searchModel.NameHead != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchModel.NameBranchSearch = currentFilterName;
                searchModel.PostalAddressSearch = currentFilterPostalAddress;
                searchModel.NameHead = currentFilterHeadName;
            }

            IQueryable<Branch> branches = _context.Branches.Include(b => b.City)
                .AsNoTracking().Select(b=>
                new Branch { 
                ID=b.ID,
                Name=b.Name,
                Code=b.Code,
                HeadFirstName=b.HeadFirstName,
                HeadLastName=b.HeadLastName,
                HeadMiddleName=b.HeadMiddleName,
                HeadEmail=b.HeadEmail,
                CurrentContractCount=(from cc in _context.CurrentContracts where cc.BranchID == b.ID select cc.ID).Count(),
                PastContractCount= (from pc in _context.PastContracts where pc.BranchID == b.ID select pc.ID).Count(),
                City =b.City,
                StreetBuilding=b.StreetBuilding,
                PostalIndex = b.PostalIndex,
                });


            if (searchModel != null)
            {
                if (!String.IsNullOrEmpty(searchModel.NameBranchSearch))
                {
                    branches = branches.Where(b => b.Name.Contains(searchModel.NameBranchSearch)
                    ||b.Code.Contains(searchModel.NameBranchSearch.ToUpper()));
                }

                if (!String.IsNullOrEmpty(searchModel.PostalAddressSearch))
                {
                    branches = branches.Where(b => b.City.Name.Contains(searchModel.PostalAddressSearch) ||
                    b.StreetBuilding.Contains(searchModel.PostalAddressSearch) ||
                    b.PostalIndex.Contains(searchModel.PostalAddressSearch));
                }
                if (!String.IsNullOrEmpty(searchModel.NameHead))
                {
                    branches = branches.Where(b => b.HeadFirstName.Contains(searchModel.NameHead)
                    || b.HeadLastName.Contains(searchModel.NameHead) || b.HeadMiddleName.Contains(searchModel.NameHead));
                }
            }

            switch (sortOrder)
            {
                case "Name_desc":
                    branches = branches.OrderByDescending(b => b.Name); break;
                case "PostalAddress_desc":
                    branches = branches.OrderByDescending(c => c.PostalIndex).ThenByDescending(b => b.City.Name).ThenByDescending(b => b.StreetBuilding); break;
                case "PostalAddress":
                    branches = branches.OrderBy(c => c.PostalIndex).ThenBy(b => b.City.Name).ThenBy(b => b.StreetBuilding); break;
                case "HeadFullName_desc":
                    branches = branches.OrderByDescending(b => b.HeadLastName).ThenByDescending(b => b.HeadFirstName).ThenByDescending(b => b.HeadMiddleName); break;
                case "HeadFullName":
                    branches = branches.OrderBy(c => c.HeadLastName).ThenBy(b => b.HeadFirstName).ThenBy(b => b.HeadMiddleName); break;
                default:
                    branches = branches.OrderBy(b => b.Name); break;
            }

            int pageSize = 4;
            
            return View(await PaginatedList<Branch>.CreateAsync(branches.AsNoTracking(), pageNumber ?? 1, pageSize, searchModel, sortModel));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                throw new ElementNotFoundException("The Branch cannot be found,",
                    new ArgumentNullException($"{nameof(id)}"));
            }

            Branch branch = await _context.Branches.Include(b => b.City)
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.ID == id);
            
            if (branch == null)
            {
                throw new ElementNotFoundException("The Branch cannot be found,",
                                        new ArgumentNullException($"{nameof(branch)}"));
            }

            branch.PastContractCount = (from pc in _context.PastContracts where pc.BranchID == branch.ID select pc).Count();

            branch.CurrentContractCount = (from cc in _context.CurrentContracts where cc.BranchID == branch.ID select cc).Count();

            return View(branch);
        }

        [Authorize(Roles = "ContractStaff")]
        public async Task<IActionResult> Create(int? cityID)
        {
            SelectList cities = await GetCitiesForSelect();

            if (cityID != null)
            {
                City city = await _context.Cities.FirstOrDefaultAsync(c => c.ID == cityID);
                if (city == null)
                {
                    throw new ElementNotFoundException("Something went wrong during branch creation,",
                     new ArgumentNullException($"{nameof(city)}"));
                }
                return View(ViewModelBranchFactory.Create(new Branch() {CityID=city.ID}, cities));
            }

            return View(ViewModelBranchFactory.Create(new Branch(),cities));
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

        [Authorize(Roles = "ContractStaff")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Create(BranchViewModel branchViewModel)
        {
            
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Branches.Add(branchViewModel.Branch);
                    await _context.SaveChangesAsync();

                    Branch newBranch = await _context.Branches.FirstOrDefaultAsync(s => s.Code == branchViewModel.Branch.Code);

                    if (newBranch == null)
                    {
                        throw new ElementNotFoundException("Something went wrong during a branch create,"
                            , new ArgumentNullException($"{nameof(newBranch)}"));
                    }

                    return RedirectToAction(nameof(Details), new { id = newBranch.ID });
                }
            }
            catch (DbUpdateException dBUpEx)
            {
                _logger.LogError(dBUpEx, "Unable to save changes to DB");
                ModelState.AddModelError("", "Unable to save changes." +
                                "Try again, and if the problem persists " +
                                "see your system administrator.");
            }

            SelectList cities = await GetCitiesForSelect();
            return View(ViewModelBranchFactory.Create(branchViewModel.Branch, cities));
        }

        [Authorize(Roles = "ContractStaff")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                throw new ElementNotFoundException("Something went wrong during a branch edit,",
                    new ArgumentNullException($"{nameof(id)}"));
            }

            Branch branchToEdit = await _context.Branches.Include(b => b.City).FirstOrDefaultAsync(b => b.ID == id);
            
            if (branchToEdit == null)
            {
                throw new ElementNotFoundException("The Branch cannot be found,",
                                        new ArgumentNullException($"{nameof(branchToEdit)}"));
            }

            SelectList cities = await GetCitiesForSelect();

            BranchViewModel model = ViewModelBranchFactory.Edit(branchToEdit, cities);
            return View(model);
        }

        [Authorize(Roles = "ContractStaff")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Edit(BranchViewModel branchViewModel)
        {

            if (ModelState.IsValid)
            {
                if (branchViewModel.Branch == null)
                {
                    throw new ElementNotFoundException("Something went wrong during a branch edit,",
                        new ArgumentNullException($"{nameof(branchViewModel.Branch)}"));
                }

                Branch branchToUpdate = await _context.Branches.FirstOrDefaultAsync(b => b.ID == branchViewModel.Branch.ID);

                if (branchToUpdate == null)
                {
                    throw new ElementNotFoundException("Something went wrong during a branch edit,",
                                                new ArgumentNullException($"{nameof(branchToUpdate)}"));
                }

                branchToUpdate.Name = branchViewModel.Branch.Name;
                branchToUpdate.BranchTelephone = branchViewModel.Branch.BranchTelephone;
                branchToUpdate.BranchEmail = branchViewModel.Branch.BranchEmail;
                branchToUpdate.PostalIndex = branchViewModel.Branch.PostalIndex;
                branchToUpdate.CityID = branchViewModel.Branch.CityID;
                branchToUpdate.StreetBuilding = branchViewModel.Branch.StreetBuilding;
                branchToUpdate.HeadFirstName = branchViewModel.Branch.HeadFirstName;
                branchToUpdate.HeadMiddleName = branchViewModel.Branch.HeadMiddleName;
                branchToUpdate.HeadLastName = branchViewModel.Branch.HeadLastName;
                branchToUpdate.HeadTelephone = branchViewModel.Branch.HeadTelephone;
                branchToUpdate.HeadEmail = branchViewModel.Branch.HeadEmail;

                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Details), new { branchViewModel.Branch.ID });
                }
                catch (DbUpdateException dbUpEx)
                {
                    _logger.LogError(dbUpEx, "Unable to save changes to DB.");
                    ModelState.AddModelError("", "Unable to save changes. " +
                            "Try again, and if the problem persists " +
                            "see your system administrator.");
                }
            }

            SelectList cities = await GetCitiesForSelect();

            BranchViewModel model = ViewModelBranchFactory.Edit(branchViewModel.Branch, cities);
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

            Branch branchToDelete = await _context.Branches.FirstOrDefaultAsync(b => b.ID == id);
            if (branchToDelete == null)
            {
                _logger.LogError(new DbUpdateConcurrencyException($"{nameof(branchToDelete)} was not found during a branch delete."), "BranchToDelete was not found");
                return true;
            }
            try
            {
                _context.Branches.Remove(branchToDelete);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException dBUpEx)
            {
                _logger.LogError(dBUpEx, "Delete failed");
                
                return false;
            }
        }

        public IActionResult Export(SearchModel searchModel)
        {
            IQueryable<Branch> branches = _context.Branches.Include(b => b.City)
                .AsNoTracking().Select(b =>
                new Branch
                {
                    ID = b.ID,
                    Name = b.Name,
                    Code = b.Code,
                    HeadFirstName = b.HeadFirstName,
                    HeadLastName = b.HeadLastName,
                    HeadMiddleName = b.HeadMiddleName,
                    HeadEmail = b.HeadEmail,
                    CurrentContractCount = (from cc in _context.CurrentContracts where cc.BranchID == b.ID select cc.ID).Count(),
                    PastContractCount = (from pc in _context.PastContracts where pc.BranchID == b.ID select pc.ID).Count(),
                    City = b.City,
                    StreetBuilding = b.StreetBuilding,
                    PostalIndex = b.PostalIndex,
                    BranchTelephone=b.BranchTelephone,
                    BranchEmail=b.BranchEmail,
                    HeadTelephone=b.HeadTelephone
                });
            
                if (!String.IsNullOrEmpty(searchModel.NameBranchSearch))
                {
                    branches = branches.Where(s => s.Name.Contains(searchModel.NameBranchSearch));
                }

                if (!String.IsNullOrEmpty(searchModel.PostalAddressSearch))
                {
                    branches = branches.Where(s => s.City.Name.Contains(searchModel.PostalAddressSearch) ||
                    s.StreetBuilding.Contains(searchModel.PostalAddressSearch) ||
                    s.PostalIndex.Contains(searchModel.PostalAddressSearch));
                }

                if (!String.IsNullOrEmpty(searchModel.NameHead))
                {
                    branches = branches.Where(s => s.HeadFirstName.Contains(searchModel.NameHead)
                    || s.HeadLastName.Contains(searchModel.NameHead) || s.HeadMiddleName.Contains(searchModel.NameHead));
                }

            branches = branches.OrderBy(b => b.Name).ThenBy(b => b.HeadLastName);

            /*create table*/
            DataTable dtBranches = new DataTable("Branches");
            dtBranches.Columns.AddRange(new DataColumn[10] {
                                        new DataColumn("Branch Name", typeof(string)),
                                        new DataColumn("Branch Code", typeof(string)),
                                        new DataColumn("Branch Telephone Number", typeof(string)),
                                        new DataColumn("Branch Email Address", typeof(string)),
                                        new DataColumn("Branch Postal Address",typeof(string)),
                                        new DataColumn("Head Name",typeof(string)),
                                        new DataColumn("Head Telephone Number", typeof(string)),
                                        new DataColumn("Head Email Address", typeof(string)),
                                        new DataColumn("Current Contracts", typeof(int)),
                                        new DataColumn("Past Contracts", typeof(int))
            });
            foreach (Branch branch in branches)
            {
                dtBranches.Rows.Add(
                    branch.Name,
                    branch.Code,
                    branch.BranchTelephone,
                    branch.BranchEmail,
                    branch.PostalAddress,
                    branch.HeadFullName,
                    branch.HeadTelephone,
                    branch.HeadEmail,
                    branch.CurrentContractCount,
                    branch.PastContractCount
                    );
            }

            /*export table*/
            try
            {
                using (XLWorkbook wbBranches = new XLWorkbook())
                {
                    IXLWorksheet ws = wbBranches.Worksheets.Add(dtBranches);

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

                    ws.Column(3).Width = 20;
                    ws.Column(3).Style.Alignment.WrapText = true;

                    ws.Column(4).Width = 30;
                    ws.Column(4).Style.Alignment.WrapText = true;

                    ws.Column(5).Width = 45;
                    ws.Column(5).Style.Alignment.WrapText = true;

                    ws.Column(7).Width = 20;
                    ws.Column(7).Style.Alignment.WrapText = true;

                    ws.Column(8).Width = 30;
                    ws.Column(8).Style.Alignment.WrapText = true;

                    ws.Column(9).Width = 11;
                    ws.Column(9).Style.Alignment.WrapText = true;

                    ws.Column(10).Width = 11;
                    ws.Column(10).Style.Alignment.WrapText = true;

                    CreateEmailHyperlinks(ws, 4, 3);

                    CreateEmailHyperlinks(ws, 8, 3);

                    ws.RangeUsed().Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    ws.RangeUsed().Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    ws.RangeUsed().Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                    ws.RangeUsed().Style.Border.RightBorder = XLBorderStyleValues.Thin;

                    // Report Heading
                    ws.Cell(1, 1).Value = "Branches Report";
                    ws.Row(1).Style.Font.FontSize = 16;
                    ws.Row(1).Height = 25;
                    ws.Row(1).Style.Font.Bold = true;
                    ws.Range(1, 1, 1, 5).Merge();
                    ws.Range(1, 1, 1, 5).Style.Border.OutsideBorder = XLBorderStyleValues.None;
                    ws.Range(1, 1, 1, 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    ws.Range(1, 1, 1, 5).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

                    using (MemoryStream stream = new MemoryStream())
                    {
                        wbBranches.SaveAs(stream);
                        string fileName = $"Branches_{DateTime.Now:ddMMyyyy_hhmmss}.xlsx";
                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ProccesingException("Something went wrong during Report Export,", ex);
            }
        }

        private void CreateEmailHyperlinks(IXLWorksheet workSheet, int emailColumnNumber, int headingRowNum)
        {
            if (workSheet == null)
            {
                throw new ProccesingException("Something went wrong during report export,",
                    new ArgumentNullException($"{nameof(workSheet)}"));
            }
            if (emailColumnNumber == default)
            {
                throw new ProccesingException("Something went wrong during report export,",
                                        new ArgumentNullException($"{nameof(emailColumnNumber)}"));
            }
            if (headingRowNum == default)
            {
                throw new ProccesingException("Something went wrong during report export,",
                                        new ArgumentNullException($"{nameof(headingRowNum)}"));
            }
            var emailList =
                        workSheet.Column(emailColumnNumber).CellsUsed().Select(c =>
                        new { RowNum = c.Address.RowNumber, Value = c.Value.ToString() }).ToList();
           
            if (emailList == null)
            {
                throw new ProccesingException("Something went wrong during report export,",
                           new ArgumentNullException($"{nameof(emailList)}"));
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

            IQueryable<Branch> branches = _context.Branches
                .Include(s => s.CurrentContracts).Include(s => s.PastContracts);

            List<ChartShortSupBranchVM> shortBranches = await (from branch in branches
                                        select new ChartShortSupBranchVM
                                        {
                                            Code = branch.Code,
                                            CurrentContractsTotalAmt =
                                            branch.CurrentContracts.Select(c => c.ContractAmount).Sum(),
                                            ContractsTotalAmt =
                                            branch.CurrentContracts.Select(c => c.ContractAmount).Sum() +
                                            branch.PastContracts.Select(c => c.ContractAmount).Sum(),
                                            CurrentContractsQuantity = branch.CurrentContracts.Count,
                                            ContractsQuantity = branch.PastContracts.Count + branch.CurrentContracts.Count
                                        }).ToListAsync();


            if (shortBranches == null)
            {
                throw new ElementNotFoundException("The shortBranches cannot be null.",
                        new ArgumentNullException($"{nameof(shortBranches)}"));
            }


            #region Get data for quantity charts
            IEnumerable<IGrouping<int, ChartShortSupBranchVM>> groupedShortBranchesByQuantity;

            groupedShortBranchesByQuantity = isPastContractIncluded ?
            shortBranches.GroupBy(b => b.ContractsQuantity) :
            shortBranches.GroupBy(b => b.CurrentContractsQuantity);

            if (groupedShortBranchesByQuantity == null)
            {
                throw new ElementNotFoundException("Something went wrong during charts showing.",
                         new ArgumentNullException($"{nameof(groupedShortBranchesByQuantity)}"));
            }

            List<ChartSupBranchViewModel> branchesByQuantity = new List<ChartSupBranchViewModel>();

            int lessThanMinTresholdQty = 0;
            int moreThanMinTresholdQty = 0;
            int moreThanMaxTresholdQty = 0;

            int minTresholdQty = Int32.Parse(_configuration["ChartConfiguration:MinQtyTreshold"]);
            int maxTresholdQty = Int32.Parse(_configuration["ChartConfiguration:MaxQtyTreshold"]);

            foreach (var groupingShortBranches in groupedShortBranchesByQuantity)
            {
                if (groupingShortBranches.Key <= minTresholdQty)
                {
                    lessThanMinTresholdQty += groupingShortBranches.Count();
                }
                else if (groupingShortBranches.Key >minTresholdQty
                    && groupingShortBranches.Key <= maxTresholdQty)
                {
                    moreThanMinTresholdQty += groupingShortBranches.Count();

                }
                else if (groupingShortBranches.Key > maxTresholdQty)
                {
                    moreThanMaxTresholdQty += groupingShortBranches.Count();
                }
            }

            branchesByQuantity.Add(new ChartSupBranchViewModel
            {
                Name = $"contracts qty <= {minTresholdQty}",
                SupBranchQuantity = lessThanMinTresholdQty
            });
            branchesByQuantity.Add(new ChartSupBranchViewModel
            {
                Name = $"{minTresholdQty} < contracts qty <= {maxTresholdQty}",
                SupBranchQuantity = moreThanMinTresholdQty
            });
            branchesByQuantity.Add(new ChartSupBranchViewModel
            {
                Name = $"contracts qty > {maxTresholdQty}",
                SupBranchQuantity = moreThanMaxTresholdQty
            });
            #endregion

            #region Get data for amount charts

            List<ChartSupBranchViewModel> branchesByAmmount = new List<ChartSupBranchViewModel>();

            int lessThanMinTresholdCount = 0;
            int moreThanMinTresholdCount = 0;
            int moreThanMaxTresholdCount = 0;
            
            decimal minAmtTreshold = decimal.Parse(_configuration["ChartConfiguration:MinAmtTreshold"]);
            decimal maxAmtTreshold = decimal.Parse(_configuration["ChartConfiguration:MaxAmtTreshold"]);

            if (isPastContractIncluded)
            {
                foreach (var branch in shortBranches)
                {
                    if (branch.ContractsTotalAmt <= minAmtTreshold)
                    {
                        lessThanMinTresholdCount++;
                    }
                    else if (branch.ContractsTotalAmt > minAmtTreshold &&
                        branch.ContractsTotalAmt <= maxAmtTreshold)
                    {
                        moreThanMinTresholdCount++;
                    }
                    else if (branch.ContractsTotalAmt > maxAmtTreshold)
                    {
                        moreThanMaxTresholdCount++;
                    }
                }
            }
            else
            {
                foreach (var branch in shortBranches)
                {
                    if (branch.CurrentContractsTotalAmt <= minAmtTreshold)
                    {
                        lessThanMinTresholdCount++;
                    }
                    else if (branch.CurrentContractsTotalAmt > minAmtTreshold
                        && branch.CurrentContractsTotalAmt <= maxAmtTreshold)
                    {
                        moreThanMinTresholdCount++;
                    }
                    else if (branch.CurrentContractsTotalAmt > maxAmtTreshold)
                    {
                        moreThanMaxTresholdCount++;
                    }
                }
            }

            branchesByAmmount.Add(
                new ChartSupBranchViewModel
                {
                    Name = $"contracts amt <= {minAmtTreshold/1000}T",
                    SupBranchQuantity = lessThanMinTresholdCount
                });

            branchesByAmmount.Add(
                new ChartSupBranchViewModel
                {
                    Name = $"{minAmtTreshold / 1000}T < contracts amt <= {maxAmtTreshold / 1000}T",
                    SupBranchQuantity = moreThanMinTresholdCount
                });

            branchesByAmmount.Add(
                new ChartSupBranchViewModel
                {
                    Name = $"contracts amt > {maxAmtTreshold/1000}T",
                    SupBranchQuantity = moreThanMaxTresholdCount
                });

            #endregion

            List<string> labelsQuantity = branchesByQuantity.Select(b => b.Name).ToList<string>();
            data.Add("labelsQuantity", labelsQuantity);

            List<int> branchesByContractQty = branchesByQuantity.Select(b => b.SupBranchQuantity)
                .ToList<int>();
            data.Add("branchesByContractQty", branchesByContractQty);

            List<string> labelsAmount = branchesByAmmount.Select(b => b.Name).ToList();
            data.Add("labelsAmount", labelsAmount);

            List<int> branchesByContractAmt = branchesByAmmount.Select(b => b.SupBranchQuantity).ToList();
            data.Add("branchesByContractAmt", branchesByContractAmt);

            data.Add("isPastContractIncluded", isPastContractIncluded);

            return data;
        }

        [HttpPost]
        public async Task<string> GetPDFData(int? id)
        {
            if (id == null)
            {
                throw new ElementNotFoundException("The Branch cannot be found,", new ArgumentNullException($"{nameof(id)}"));
            }


            Branch branch = await _context.Branches.Include(b => b.City)
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.ID == id);

            if (branch == null)
            {
                throw new ElementNotFoundException("The Branch cannot be found,",
                                        new ArgumentNullException($"{nameof(branch)}"));
            }

            branch.PastContractCount = (from pc in _context.PastContracts where pc.BranchID == branch.ID select pc).Count();

            branch.CurrentContractCount = (from cc in _context.CurrentContracts where cc.BranchID == branch.ID select cc).Count();

            string pdfContent = _razorRendererHelper.RenderPartialToString("Views/Branches/_PrintPDFPartial.cshtml", branch);
            return pdfContent;
        }
    }
}

