using ClosedXML.Excel;
using DealRept.Models;
using DealRept.Models.ViewModel;
using DealRept.Services.EmailService;
using DealRept.Services.RazorRenderService;
using DealRept.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DealRept.Controllers
{
    [Authorize(Roles = "ContractStaff, BranchStaff, Administrator")]
    public class ContractsController : Controller
    {
        private readonly DealDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ContractsController> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IRazorRendererHelper _razorRendererHelper;

        public ContractsController(DealDbContext ctx, IConfiguration config,
            ILogger<ContractsController> logger, IEmailSender emailSender,
            IRazorRendererHelper razorRendererHelper)
        {
            _context = ctx;
            _configuration = config;
            _logger = logger;
            _emailSender = emailSender;
            _razorRendererHelper = razorRendererHelper;
        }

        public async Task<IActionResult> Index(int? pageNumber, SortModel sortModel, string sortOrder,
            SearchModel searchModel, string currentFilterType,
            string currentFilterAmountFrom, string currentFilterAmountTo,
            DateTime? currentFilterConclusionDateFrom, DateTime? currentFilterConclusionDateTo,
            DateTime? currentFilterExpirationDateFrom, DateTime? currentFilterExpirationDateTo,
            int currentFilterContractStatusID, string currentFilterSupplier,
            string currentFilterBranch, string currentFilterCourtDispute, string currentFilterNumber)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            sortModel.CurrentSort = sortOrder;
            sortModel.ConclusionDateSortParm =
                String.IsNullOrEmpty(sortOrder) ? "ConclusionDate" : "";
            sortModel.ContractAmountSortParm = sortOrder == "ContractAmount" ? "ContractAmount_desc" : "ContractAmount";
            sortModel.SupplierNameSortParm = sortOrder == "SupplierName" ? "SupplierName_desc" : "SupplierName";
            sortModel.BranchNameSortParm = sortOrder == "BranchName" ? "BranchName_desc" : "BranchName";

            searchModel.PastContractStatuses = await _context.ContractStatuses.Where(s => s.Name != "current").AsNoTracking().ToListAsync();

            IQueryable<CurrentContract> currentContracts = _context.CurrentContracts
                .Include(c => c.Supplier).Include(c => c.Branch).Include(c => c.ContractStatus).AsNoTracking();
            IQueryable<PastContract> pastContracts = _context.PastContracts.Include(c => c.Supplier)
                .Include(c => c.Branch)
                            .Include(c => c.ContractStatus).AsNoTracking();

            if (searchModel.ContractNumber != null
                || searchModel.ConclusionDateFrom != default || searchModel.ConclusionDateTo != default
                || searchModel.ContractAmountFrom != default || searchModel.ContractAmountTo != default
                || searchModel.ExpirationDateFrom != default || searchModel.ExpirationDateTo != default
                || searchModel.PastContractStatusID != default
                || searchModel.NameCodeSupplierSearch != null
                || searchModel.NameBranchSearch != null
                || searchModel.ContractType != null
                || searchModel.CourtDisputeSearch != null)
            {
                pageNumber = 1;
                if (searchModel.CourtDisputeSearch == null)
                {
                    searchModel.CourtDisputeSearch = "includeCourtDispute";
                }
                if (searchModel.ContractType == "currentContracts")
                {
                    searchModel.PastContractStatusID = default;
                }
            }
            else
            {
                searchModel.ContractAmountFrom = currentFilterAmountFrom;
                searchModel.ContractAmountTo = currentFilterAmountTo;
                searchModel.ContractNumber = currentFilterNumber;
                searchModel.ContractType = currentFilterType;
                searchModel.ConclusionDateFrom = currentFilterConclusionDateFrom;
                searchModel.ConclusionDateTo = currentFilterConclusionDateTo;
                searchModel.ExpirationDateFrom = currentFilterExpirationDateFrom;
                searchModel.ExpirationDateTo = currentFilterExpirationDateTo;
                searchModel.PastContractStatusID = currentFilterContractStatusID;
                searchModel.NameCodeSupplierSearch = currentFilterSupplier;
                searchModel.NameBranchSearch = currentFilterBranch;
                searchModel.CourtDisputeSearch = currentFilterCourtDispute;
                if (searchModel.CourtDisputeSearch == null)
                {
                    searchModel.CourtDisputeSearch = "includeCourtDispute";
                }
                if (searchModel.ContractType == null)
                {
                    searchModel.ContractType = "currentContracts";
                }
                if (searchModel.ContractType == "currentContracts")
                {
                    searchModel.PastContractStatusID = default;
                }
            }

            int pageSize = 5;

            if (searchModel.ContractType == "currentContracts")
            {
                switch (sortOrder)
                {
                    case "ConclusionDate":
                        currentContracts = currentContracts.OrderBy(c => c.ConclusionDate); break;
                    case "ContractAmount":
                        currentContracts = currentContracts.OrderBy(c => c.ContractAmount); break;
                    case "ContractAmount_desc":
                        currentContracts = currentContracts.OrderByDescending(c => c.ContractAmount); break;
                    case "SupplierName":
                        currentContracts = currentContracts.OrderBy(c => c.Supplier.Name); break;
                    case "SupplierName_desc":
                        currentContracts = currentContracts.OrderByDescending(c => c.Supplier.Name); break;
                    case "BranchName":
                        currentContracts = currentContracts.OrderBy(c => c.Branch.Name); break;
                    case "BranchName_desc":
                        currentContracts = currentContracts.OrderByDescending(c => c.Branch.Name); break;
                    default:
                        currentContracts = currentContracts.OrderByDescending(c => c.ConclusionDate);
                        break;
                }

                switch (searchModel.CourtDisputeSearch)
                {
                    case ("onlyWithCourtDisputes"):
                        currentContracts = currentContracts.Where(c => c.IsCourtDispute == true); break;
                    case ("excludeCourtDisputes"):
                        currentContracts = currentContracts.Where(c => c.IsCourtDispute == false); break;
                    default:

                        break;
                }

                if (!string.IsNullOrEmpty(searchModel.ContractNumber))
                {
                    currentContracts = currentContracts.Where(c => c.ContractNumber.Contains(searchModel.ContractNumber));
                }
                if (searchModel.ContractAmountFrom != default)
                {
                    if (decimal.TryParse(searchModel.ContractAmountFrom, out decimal ContractAmountFromDec))
                    {
                        currentContracts = currentContracts.Where(c => (c.ContractAmount >= ContractAmountFromDec));
                    }
                    else
                    {
                        _logger.LogError(new FormatException(), "Something went wrong during {ContractAmountFrom} parsing", searchModel.ContractAmountFrom);
                        throw new ProccesingException("Something went wrong during Contracts filtering,",
                            new FormatException());
                    }
                }
                if (searchModel.ContractAmountTo != default)
                {
                    if (decimal.TryParse(searchModel.ContractAmountTo, out decimal ContractAmountToDec))
                    {
                        currentContracts = currentContracts.Where(c => (c.ContractAmount <= ContractAmountToDec));
                    }
                    else
                    {
                        _logger.LogError(new FormatException(), "Something went wrong during {ContractAmountTo} parsing", searchModel.ContractAmountTo);
                        throw new ProccesingException("Something went wrong during Contracts filtering,",
                            new FormatException());
                    }
                }
                if (searchModel.ConclusionDateFrom != default)
                {
                    currentContracts = currentContracts.Where(c => (c.ConclusionDate >= searchModel.ConclusionDateFrom));
                }
                if (searchModel.ConclusionDateTo != default)
                {
                    currentContracts = currentContracts.Where(c => (c.ConclusionDate <= searchModel.ConclusionDateTo));
                }
                if (searchModel.ExpirationDateFrom != default)
                {
                    currentContracts = currentContracts.Where(c => (c.ExpirationDate >= searchModel.ExpirationDateFrom));
                }
                if (searchModel.ExpirationDateTo != default)
                {
                    currentContracts = currentContracts.Where(c => (c.ExpirationDate <= searchModel.ExpirationDateTo));
                }
                if (searchModel.PastContractStatusID != default)
                {
                    searchModel.PastContractStatusID = default;
                }
                if (!String.IsNullOrEmpty(searchModel.NameCodeSupplierSearch))
                {
                    currentContracts = currentContracts.Where(c => c.Supplier.Name.Contains(searchModel.NameCodeSupplierSearch) || c.Supplier.LegalCode.Contains(searchModel.NameCodeSupplierSearch));
                }
                if (!String.IsNullOrEmpty(searchModel.NameBranchSearch))
                {
                    currentContracts = currentContracts.Where(c => c.Branch.Name.Contains(searchModel.NameBranchSearch) || c.Branch.Code.Contains(searchModel.NameBranchSearch.ToUpper()));
                }

                return View(await PaginatedList<Contract>.CreateAsync(currentContracts.AsNoTracking(), pageNumber ?? 1, pageSize, searchModel, sortModel));
            }

            searchModel.CourtDisputeSearch = default;
            switch (sortOrder)
            {
                case "ConclusionDate":
                    pastContracts = pastContracts.OrderBy(c => c.ConclusionDate); break;
                case "ContractAmount":
                    pastContracts = pastContracts.OrderBy(c => c.ContractAmount); break;
                case "ContractAmount_desc":
                    pastContracts = pastContracts.OrderByDescending(c => c.ContractAmount); break;
                case "SupplierName":
                    pastContracts = pastContracts.OrderBy(c => c.Supplier.Name); break;
                case "SupplierName_desc":
                    pastContracts = pastContracts.OrderByDescending(c => c.Supplier.Name); break;
                case "BranchName":
                    pastContracts = pastContracts.OrderBy(c => c.Branch.Name); break;
                case "BranchName_desc":
                    pastContracts = pastContracts.OrderByDescending(c => c.Branch.Name); break;
                default:
                    pastContracts = pastContracts.OrderByDescending(c => c.ConclusionDate);
                    break;
            }
            if (!string.IsNullOrEmpty(searchModel.ContractNumber))
            {
                pastContracts = pastContracts.Where(c => c.ContractNumber.Contains(searchModel.ContractNumber));
            }
            if (searchModel.ContractAmountFrom != default)
            {
                if (decimal.TryParse(searchModel.ContractAmountFrom, out decimal ContractAmountFromDec))
                {
                    pastContracts = pastContracts.Where(c => (c.ContractAmount >= ContractAmountFromDec));
                }
                else
                {
                    _logger.LogError(new FormatException(), "Something went wrong during {ContractAmountFrom} parsing", searchModel.ContractAmountFrom);
                    throw new ProccesingException("Something went wrong during Contracts filtering,",
                        new FormatException());
                }
            }
            if (searchModel.ContractAmountTo != default)
            {
                if (decimal.TryParse(searchModel.ContractAmountTo, out decimal ContractAmountToDec))
                {
                    pastContracts = pastContracts.Where(c => (c.ContractAmount <= ContractAmountToDec));
                }
                else
                {
                    _logger.LogError(new FormatException(), "Something went wrong during {ContractAmountTo} parsing", searchModel.ContractAmountTo);
                    throw new ProccesingException("Something went wrong during Contracts filtering,",
                        new FormatException());
                }
            }
            if (searchModel.ConclusionDateFrom != default)
            {
                pastContracts = pastContracts.Where(c => (c.ConclusionDate >= searchModel.ConclusionDateFrom));
            }
            if (searchModel.ConclusionDateTo != default)
            {
                pastContracts = pastContracts.Where(c => (c.ConclusionDate <= searchModel.ConclusionDateTo));
            }
            if (searchModel.ExpirationDateFrom != default)
            {
                pastContracts = pastContracts.Where(c => (c.ExpirationDate >= searchModel.ExpirationDateFrom));
            }
            if (searchModel.ExpirationDateTo != default)
            {
                pastContracts = pastContracts.Where(c => (c.ExpirationDate <= searchModel.ExpirationDateTo));
            }
            if (searchModel.PastContractStatusID != default)
            {
                pastContracts = pastContracts.Where(c => c.ContractStatus.ID == searchModel.PastContractStatusID);
            }
            if (!String.IsNullOrEmpty(searchModel.NameCodeSupplierSearch))
            {
                pastContracts = pastContracts.Where(c => c.Supplier.Name.Contains(searchModel.NameCodeSupplierSearch) || c.Supplier.LegalCode.Contains(searchModel.NameCodeSupplierSearch));
            }
            if (!String.IsNullOrEmpty(searchModel.NameBranchSearch))
            {
                pastContracts = pastContracts.Where(c => c.Branch.Name.Contains(searchModel.NameBranchSearch) || c.Branch.Code.Contains(searchModel.NameBranchSearch));
            }
            return View(await PaginatedList<Contract>.CreateAsync(pastContracts.AsNoTracking(), pageNumber ?? 1, pageSize, searchModel, sortModel));
        }


        public async Task<IActionResult> Details(int? id, string contractStatus)
        {
            if (id == null)
            {
                throw new ElementNotFoundException("The Contract cannot be found,", new ArgumentNullException($"{nameof(id)}"));
            }

            Contract contract;

            if (contractStatus == "Current")
            {
                contract = await _context.CurrentContracts.Include(c => c.Branch)
                    .Include(c => c.Supplier).Include(c => c.ContractStatus)
                    .Include(c => c.CurrentDocuments).AsNoTracking().FirstOrDefaultAsync(c => c.ID == id);
            }
            else
            {
                contract = await _context.PastContracts.Include(c => c.Branch)
                    .Include(c => c.Supplier).Include(c => c.ContractStatus)
                    .Include(c => c.PastDocuments).AsNoTracking().FirstOrDefaultAsync(c => c.ID == id);
            }

            if (contract == null)
            {
                throw new ElementNotFoundException("The Contract cannot be found,", new ArgumentNullException($"{nameof(contract)}"));
            }

            ContractViewModel model = ViewModelContractFactory.Details(contract);
            return View(model);
        }

        [Authorize(Roles = "ContractStaff")]
        public async Task<IActionResult> Create(int? supplierID, int? branchID)
        {
            ContractStatus contractStatus = await _context.ContractStatuses.FirstOrDefaultAsync(c => c.Name == "Current");

            if (contractStatus == null)
            {
                throw new ElementNotFoundException("Something went wrong during Contract creation,", new ArgumentNullException($"{nameof(contractStatus)}"));
            }

            SelectList suppliersSelect = await GetSuppliersForSelect();
            SelectList brancesSelect = await GetBranchesForSelect();

            if (supplierID != null)
            {
                Supplier supplier = await _context.Suppliers.FirstOrDefaultAsync(s => s.ID == supplierID);
                if (supplier == null)
                {
                    throw new ElementNotFoundException("Something went wrong during a Contract creation,",
                     new ArgumentNullException($"{nameof(supplier)}"));
                }
                return View(ViewModelContractFactory.Create(new Contract()
                {
                    ContractStatusID = contractStatus.ID,
                    SupplierID = supplier.ID
                }, suppliersSelect, brancesSelect));
            }
            if (branchID != null)
            {
                Branch branch = await _context.Branches.FirstOrDefaultAsync(b => b.ID == branchID);
                if (branch == null)
                {
                    throw new ElementNotFoundException("Something went wrong during a Contract creation,",
                     new ArgumentNullException($"{nameof(branch)}"));
                }
                return View(ViewModelContractFactory.Create(new Contract()
                {
                    ContractStatusID = contractStatus.ID,
                    BranchID = branch.ID
                },
                    suppliersSelect, brancesSelect));
            }

            return View(ViewModelContractFactory.Create(new Contract() { ContractStatusID = contractStatus.ID }, suppliersSelect, brancesSelect));
        }

        [Authorize(Roles = "ContractStaff")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Create(ContractViewModel contractViewModel)
        {
            if (contractViewModel == null)
            {
                throw new ElementNotFoundException("Something went wrong during a Contract creation,", new ArgumentNullException($"{nameof(contractViewModel)}"));
            }

            if (ModelState.IsValid)
            {
                CurrentContract currentContract = new CurrentContract
                {
                    ContractNumber = contractViewModel.Contract.ContractNumber,
                    ContractAmount = contractViewModel.Contract.ContractAmount,
                    ConclusionDate = contractViewModel.Contract.ConclusionDate,
                    ExpirationDate = contractViewModel.Contract.ExpirationDate,
                    Subject = contractViewModel.Contract.Subject,
                    Remarks = contractViewModel.Contract.Remarks,
                    SupplierID = contractViewModel.Contract.SupplierID,
                    BranchID = contractViewModel.Contract.BranchID,
                    IsGoods = contractViewModel.Contract.IsGoods,
                    IsProlonged = false,
                    ContractStatusID = contractViewModel.Contract.ContractStatusID
                };
                try
                {
                    _context.CurrentContracts.Add(currentContract);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {
                    _logger.LogError(ex, "Unable to save changes.");
                    ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator. ");
                }

                CurrentContract newCurrentContract = await _context.CurrentContracts.Include(c => c.ContractStatus).Include(c => c.Branch)
                            .Include(c => c.Supplier).AsNoTracking().FirstOrDefaultAsync(c => c.ContractNumber == currentContract.ContractNumber && c.ConclusionDate == currentContract.ConclusionDate);

                if (newCurrentContract == null)
                {
                    throw new ElementNotFoundException("Something went wrong during Contract creation,", new ArgumentNullException($"{nameof(newCurrentContract)}"));
                }

                string contractLink = Url.Action(nameof(Details), "Contracts", new { id = newCurrentContract.ID, contractStatus = "Current" }, Request.Scheme);

                Message message = new Message(new string[] { newCurrentContract.Branch.HeadEmail }, "Your branch contract has been added", $"Contract #{newCurrentContract.ContractNumber} has been added, to view it, please click or copy the link: {contractLink}", null, true);

                await _emailSender.SendEmailAsync(message);

                return RedirectToAction(nameof(CreateCurrentDocument), new { id = newCurrentContract.ID });
            }

            SelectList suppliersSelect = await GetSuppliersForSelect();
            SelectList branchesSelect = await GetBranchesForSelect();

            return View(ViewModelContractFactory.Create(contractViewModel.Contract, suppliersSelect, branchesSelect));
        }

        private async Task<SelectList> GetSuppliersForSelect()
        {
            List<ModelForSelect2> suppliersForSelect = await (from supplier in _context.Suppliers
                                                              select new ModelForSelect2
                                                              {
                                                                  ID = supplier.ID,
                                                                  NameLegalCode = $"{supplier.Name} - {supplier.LegalCode}"
                                                              }).ToListAsync();
            if (suppliersForSelect == null)
            {
                throw new ElementNotFoundException("Something went wrong during suppliers getting,", new ArgumentNullException($"{nameof(suppliersForSelect)}"));
            }

            SelectList suppliersSelect = new SelectList(suppliersForSelect, "ID", "NameLegalCode");

            if (suppliersSelect == null)
            {
                throw new ElementNotFoundException("Something went wrong during suppliers getting,", new ArgumentNullException($"{nameof(suppliersSelect)}"));
            }

            return suppliersSelect;
        }

        private async Task<SelectList> GetBranchesForSelect()
        {
            List<ModelForSelect2> branchesForSelect = await (from branch in _context.Branches
                                                             select new ModelForSelect2
                                                             {
                                                                 ID = branch.ID,
                                                                 NameLegalCode = $"{branch.Name} - {branch.Code}"
                                                             }).ToListAsync();
            if (branchesForSelect == null)
            {
                throw new ElementNotFoundException("Something went wrong during branches getting,", new ArgumentNullException($"{nameof(branchesForSelect)}"));
            }

            SelectList branchesSelect = new SelectList(branchesForSelect, "ID", "NameLegalCode");

            if (branchesSelect == null)
            {
                throw new ElementNotFoundException("Something went wrong during branches getting,", new ArgumentNullException($"{nameof(branchesSelect)}"));
            }

            return branchesSelect;
        }

        private async Task<SelectList> GetContractStatusesForSelect()
        {
            List<ModelForSelect2> contractStatusesForSelect = await (from contractStatus in _context.ContractStatuses
                                                                     select new ModelForSelect2
                                                                     {
                                                                         ID = contractStatus.ID,
                                                                         NameLegalCode = contractStatus.Name
                                                                     }).ToListAsync();
            if (contractStatusesForSelect == null)
            {
                throw new ElementNotFoundException("Something went wrong during contract statuses getting,", new ArgumentNullException($"{nameof(contractStatusesForSelect)}"));
            }

            SelectList contractStatusesSelect = new SelectList(contractStatusesForSelect, "ID", "NameLegalCode");

            if (contractStatusesSelect == null)
            {
                throw new ElementNotFoundException("Something went wrong during contract statuses getting,", new ArgumentNullException($"{nameof(contractStatusesSelect)}"));
            }

            return contractStatusesSelect;
        }


        [Authorize(Roles = "ContractStaff")]
        public async Task<IActionResult> CreateCurrentDocument(int id)
        {
            if (id == default)
            {
                throw new ElementNotFoundException("Something went wrong during document creation,", new ArgumentNullException($"{nameof(id)}"));
            }

            CurrentContract currentContract = await _context.CurrentContracts.AsNoTracking().FirstOrDefaultAsync(c => c.ID == id);

            if (currentContract == null)
            {
                throw new ElementNotFoundException("Something went wrong during document creation,", new ArgumentNullException($"{nameof(currentContract)}"));
            }

            return View(new DocumentViewModel() { CurrentContractID = id, CurrentContract = currentContract });
        }

        [Authorize(Roles = "ContractStaff")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> CreateCurrentDocument(DocumentViewModel currentDocument)
        {
            long _fileSizeLimit = _configuration.GetValue<long>("FileSizeLimit");
            string[] _permittedExtensions = { ".pdf" };
            string _targetFilePath = _configuration.GetValue<string>("StoredFilesPath");

            if (!ModelState.IsValid)
            {
                if (currentDocument == null)
                {
                    throw new ProccesingException("Something went wrong during Document creation,", new ArgumentNullException($"{nameof(currentDocument)}"));
                }
                currentDocument.CurrentContract = await _context.CurrentContracts.AsNoTracking().FirstOrDefaultAsync(c => c.ID == currentDocument.CurrentContractID);

                if (currentDocument.CurrentContract == null)
                {
                    throw new ProccesingException("Something went wrong during Document creation,", new ArgumentNullException($"{nameof(currentDocument.CurrentContract)}"));
                }

                return View(currentDocument);
            }

            var formFileContent =
                await FileHelpers.ProcessFormFile<FileUpload>(
                    currentDocument.FileToUpload.FormFile, ModelState, _permittedExtensions,
                    _fileSizeLimit, _logger);

            if (!ModelState.IsValid)
            {
                if (currentDocument == null)
                {
                    throw new ProccesingException("Something went wrong during Document creation,", new ArgumentNullException($"{nameof(currentDocument)}"));
                }

                currentDocument.CurrentContract = await _context.CurrentContracts.AsNoTracking().FirstOrDefaultAsync(c => c.ID == currentDocument.CurrentContractID);
                if (currentDocument.CurrentContract == null)
                {
                    throw new ProccesingException("Something went wrong during Document creation,", new ArgumentNullException($"{nameof(currentDocument.CurrentContract)}"));
                }

                return View(currentDocument);
            }
            try
            {
                var trustedFileNameForDisplay = WebUtility.HtmlEncode(
                currentDocument.FileToUpload.FormFile.FileName);
                var trustedFileNameForFileStorage = Path.GetRandomFileName();
                var filePath = Path.Combine(
                    _targetFilePath, trustedFileNameForFileStorage);

                using (var fileStream = System.IO.File.Create(filePath))
                {
                    await fileStream.WriteAsync(formFileContent);
                }

                CurrentDocument newCurrentDocument = new CurrentDocument
                {
                    Name = trustedFileNameForDisplay,
                    DateOfUploading = DateTime.Now.Date,
                    PathToDocument = filePath,
                    CurrentContractID = currentDocument.CurrentContractID
                };

                _context.CurrentDocuments.Add(newCurrentDocument);
                await _context.SaveChangesAsync();

                CurrentContract currentContractUpdated = await _context.CurrentContracts.Include(c => c.ContractStatus).Include(c => c.Branch)
                    .Include(c => c.Supplier).AsNoTracking().FirstOrDefaultAsync(c => c.ID == newCurrentDocument.CurrentContractID);

                if (currentContractUpdated == null)
                {
                    throw new ElementNotFoundException("Contract cannot be found,", new ArgumentNullException($"{nameof(currentContractUpdated)}"));
                }

                return RedirectToAction(nameof(Details), new { id = currentContractUpdated.ID, contractStatus = "Current" });
            }
            catch (DbUpdateException DbUEx)
            {
                _logger.LogError(DbUEx, "Unable to save changes.");
                ModelState.AddModelError("", "Unable to save changes. " +
                "Try again, and if the problem persists " +
                "see your system administrator.");
            }
            catch (IOException IOEx)
            {
                _logger.LogError(IOEx, "Unable to upload file.");
                ModelState.AddModelError("", "Unable to upload file. " +
               "Try again, and if the problem persists " +
               "see your system administrator.");
            }

            currentDocument.CurrentContract = await _context.CurrentContracts.AsNoTracking().FirstOrDefaultAsync(c => c.ID == currentDocument.CurrentContractID);
            if (currentDocument.CurrentContract == null)
            {
                throw new ElementNotFoundException("Something went wrong during document creation,", new ArgumentNullException($"{nameof(currentDocument.CurrentContract)}"));
            }

            return View(currentDocument);
        }

        public async Task<IActionResult> OpenDocument(int? id, string contractStatus)
        {
            if (id == null)
            {
                throw new ElementNotFoundException("The Document cannot be found,", new ArgumentNullException($"{nameof(id)}"));
            }

            if (contractStatus == null)
            {
                throw new ElementNotFoundException("The Document cannot be found,", new ArgumentNullException($"{nameof(contractStatus)}"));
            }

            if (contractStatus == "Current")
            {
                CurrentDocument currentDocument = await _context.CurrentDocuments.AsNoTracking().FirstOrDefaultAsync(d => d.ID == id);

                if (currentDocument == null)
                {
                    throw new ElementNotFoundException("The Document cannot be found", new ArgumentNullException($"{nameof(currentDocument)}"));
                }

                if (System.IO.File.Exists(currentDocument.PathToDocument))
                {
                    string absolutePath = Path.GetFullPath(currentDocument.PathToDocument);
                    return new PhysicalFileResult(absolutePath, "application/pdf");
                }
                else
                {
                    throw new ElementNotFoundException("The Document cannot be found,",
                        new FileNotFoundException("Document File cannot be found.", $"{currentDocument.Name}"));
                }
            }

            PastDocument pastDocument = await _context.PastDocuments.AsNoTracking().FirstOrDefaultAsync(d => d.ID == id);

            if (pastDocument == null)
            {
                throw new ElementNotFoundException("The Document cannot be found,", new ArgumentNullException($"{nameof(pastDocument)}"));
            }

            if (System.IO.File.Exists(pastDocument.PathToDocument))
            {
                string absolutePath = Path.GetFullPath(pastDocument.PathToDocument);
                return new PhysicalFileResult(absolutePath, "application/pdf");
            }
            else
            {
                throw new ElementNotFoundException("The Document cannot be found,",
                     new FileNotFoundException("Document File cannot be found.", $"{pastDocument.Name}"));
            }

        }

        public async Task<IActionResult> DownloadDocument(int? id, string contractStatus)
        {
            if (id == null)
            {
                throw new ElementNotFoundException("The Document cannot be found,", new ArgumentNullException($"{nameof(id)}"));
            }

            if (contractStatus == null)
            {
                throw new ElementNotFoundException("The Document cannot be found,", new ArgumentNullException($"{nameof(contractStatus)}"));
            }

            if (contractStatus == "Current")
            {
                CurrentDocument currentDocument = await _context.CurrentDocuments.AsNoTracking().FirstOrDefaultAsync(d => d.ID == id);

                if (currentDocument == null)
                {
                    throw new ElementNotFoundException("The Document cannot be found,",
                        new ArgumentNullException($"{nameof(currentDocument)}"));
                }
                if (System.IO.File.Exists(currentDocument.PathToDocument))
                {
                    string fileName = currentDocument.Name;

                    FileStream fs = new FileStream(currentDocument.PathToDocument, FileMode.Open, FileAccess.Read);
                    if (fs == null)
                    {
                        throw new ProccesingException("Something went wrong during downloading the Document,",
                            new ArgumentNullException($"{nameof(fs)}"));
                    }

                    FileStreamResult fsResult = new FileStreamResult(fs, System.Net.Mime.MediaTypeNames.Application.Octet)
                    {
                        FileDownloadName = fileName
                    };

                    if (fsResult == null)
                    {
                        throw new ProccesingException("Something went wrong during downloading the Document,",
                            new ArgumentNullException($"{nameof(fsResult)}"));
                    }

                    return fsResult;
                }
                else
                {
                    throw new ElementNotFoundException("The Document cannot be found,",
                        new FileNotFoundException($"{nameof(currentDocument.Name)}"));
                }
            }

            PastDocument pastDocument = await _context.PastDocuments.AsNoTracking().FirstOrDefaultAsync(d => d.ID == id);
            if (pastDocument == null)
            {
                throw new ElementNotFoundException("The Document cannot be found,",
                    new ArgumentNullException($"{nameof(pastDocument)}"));
            }

            if (System.IO.File.Exists(pastDocument.PathToDocument))
            {
                string fileName = pastDocument.Name;

                FileStream fs = new FileStream(pastDocument.PathToDocument, FileMode.Open, FileAccess.Read);

                if (fs == null)
                {
                    throw new ProccesingException("Something went wrong during downloading the Document,",
                        new ArgumentNullException($"{nameof(fs)}"));
                }

                FileStreamResult fsResult = new FileStreamResult(fs, System.Net.Mime.MediaTypeNames.Application.Octet)
                {
                    FileDownloadName = fileName
                };

                if (fsResult == null)
                {
                    throw new ProccesingException("Something went wrong during downloading the Document,",
                        new ArgumentNullException($"{nameof(fsResult)}"));
                }

                return fsResult;
            }
            else
            {
                throw new ElementNotFoundException("The Document cannot be found,",
                    new FileNotFoundException($"{pastDocument.Name}"));
            }
        }

        [Authorize(Roles = "ContractStaff")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                throw new ElementNotFoundException("Something went wrong during a contract edit,",
                    new ArgumentNullException($"{nameof(id)}"));
            }

            Contract contractToEdit = await _context.CurrentContracts.Include(c => c.Branch)
           .Include(c => c.Supplier).Include(c => c.ContractStatus)
           .Include(c => c.CurrentDocuments).FirstOrDefaultAsync(c => c.ID == id);

            if (contractToEdit == null)
            {
                throw new ElementNotFoundException("Something went wrong during a contract edit,",
                    new ArgumentNullException($"{nameof(contractToEdit)}"));
            }

            List<CurrentDocumentVM> currentDocumentsToEdit = new List<CurrentDocumentVM>();

            if (contractToEdit.CurrentDocuments.Any())
            {
                currentDocumentsToEdit = PopulateCurrentDocumentsToEdit(contractToEdit);

                if (!currentDocumentsToEdit.Any())
                {
                    throw new ProccesingException("Something went wrong during Contract edit,",
                        new ArgumentNullException($"{nameof(currentDocumentsToEdit)}", "Empty IEnumerable"));
                }
            }

            SelectList contractStatusesSelect = await GetContractStatusesForSelect();
            SelectList suppliersSelect = await GetSuppliersForSelect();
            SelectList branchesSelect = await GetBranchesForSelect();

            ContractViewModel model = ViewModelContractFactory.Edit(contractToEdit, currentDocumentsToEdit,
                suppliersSelect, branchesSelect, contractStatusesSelect);
            return View(model);
        }

        [Authorize(Roles = "ContractStaff")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Edit(ContractViewModel contractViewModel)
        {
            IQueryable<ContractStatus> contractStatuses = _context.ContractStatuses;

            if (contractViewModel.Contract == null)
            {
                throw new ElementNotFoundException("Something went wrong during a contract edit,",
                    new ArgumentNullException($"{nameof(contractViewModel.Contract)}"));
            }

            ContractStatus status = await contractStatuses.FirstOrDefaultAsync(s => s.ID == contractViewModel.Contract.ContractStatusID);

            if (status == null)
            {
                throw new ElementNotFoundException("Something went wrong during a contract edit,",
                                           new ArgumentNullException($"{nameof(status)}"));
            }

            if (status.Name != "Current" && contractViewModel.Contract.IsCourtDispute == true)
            {
                ModelState.AddModelError("Contract.ContractStatusID", "Court Dispute contract must have only Current status.");
            }

            if (ModelState.IsValid)
            {
                CurrentContract currentContractToUpdate =
                await _context.CurrentContracts.Include(c=>c.CurrentEvents).Include(c => c.ContractStatus).FirstOrDefaultAsync(c => c.ID == contractViewModel.Contract.ID);

                if (currentContractToUpdate == null)
                {
                    throw new ElementNotFoundException("Something went wrong during a contract edit,",
                        new ArgumentNullException($"{nameof(currentContractToUpdate)}"));
                }
                if (status.Name == "Current")
                {
                    if (contractViewModel.Contract.ExpirationDate > currentContractToUpdate.ExpirationDate)
                    {
                        currentContractToUpdate.IsProlonged = true;
                    }

                    currentContractToUpdate.ContractAmount = contractViewModel.Contract.ContractAmount;
                    currentContractToUpdate.ExpirationDate = contractViewModel.Contract.ExpirationDate;
                    currentContractToUpdate.Subject = contractViewModel.Contract.Subject;
                    currentContractToUpdate.IsGoods = contractViewModel.Contract.IsGoods;
                    currentContractToUpdate.IsCourtDispute = contractViewModel.Contract.IsCourtDispute;
                    currentContractToUpdate.ContractStatusID = contractViewModel.Contract.ContractStatusID;
                    currentContractToUpdate.Remarks = contractViewModel.Contract.Remarks;
                    currentContractToUpdate.BranchID = contractViewModel.Contract.BranchID;
                    currentContractToUpdate.SupplierID = contractViewModel.Contract.SupplierID;

                    if (contractViewModel.CurrentDocumentsToEdit != null && contractViewModel.CurrentDocumentsToEdit.Any())
                    {
                        await DeleteCurrentDocuments(contractViewModel.CurrentDocumentsToEdit,
                            false, null);
                    }

                    try
                    {
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Details), new { id = currentContractToUpdate.ID, contractStatus = "Current" });
                    }
                    catch (DbUpdateException DbUEx)
                    {
                        _logger.LogError(DbUEx, "Unable to save changes to DB");
                        ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.");
                    }
                }
                else
                {
                    if (contractViewModel.Contract.ExpirationDate > currentContractToUpdate.ExpirationDate)
                    {
                        contractViewModel.Contract.IsProlonged = true;
                    }
                    PastContract newPastContract = new PastContract
                    {
                        ContractNumber = contractViewModel.Contract.ContractNumber,
                        ContractAmount = contractViewModel.Contract.ContractAmount,
                        ConclusionDate = contractViewModel.Contract.ConclusionDate,
                        ExpirationDate = contractViewModel.Contract.ExpirationDate,
                        TransitionDate = DateTime.Now.Date,
                        Subject = contractViewModel.Contract.Subject,
                        Remarks = contractViewModel.Contract.Remarks,
                        IsGoods = contractViewModel.Contract.IsGoods,
                        IsCourtDispute = false,
                        IsProlonged = contractViewModel.Contract.IsProlonged,
                        BranchID = contractViewModel.Contract.BranchID,
                        SupplierID = contractViewModel.Contract.SupplierID,
                        ContractStatusID = status.ID
                    };
                    try
                    {
                        _context.PastContracts.Add(newPastContract);
                        await _context.SaveChangesAsync();

                    }
                    catch (DbUpdateException DbUExPastContract)
                    {
                        _logger.LogError(DbUExPastContract, "Unable to save changes");
                        ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists " +
                        "see your system administrator. ");
                    }

                    if (ModelState.IsValid)
                    {
                        PastContract createdPastContract = await _context.PastContracts
                                            .Include(c => c.Branch)
                                            .FirstOrDefaultAsync(c => c.ContractNumber == newPastContract.ContractNumber
                                        && c.ConclusionDate == newPastContract.ConclusionDate);
                        if (createdPastContract == null)
                        {
                            throw new ElementNotFoundException("Something went wrong during a contract edit,",
                                new ArgumentNullException($"{nameof(createdPastContract)}"));
                        }

                        if (currentContractToUpdate.CurrentEvents.Count != 0)
                        {
                            try
                            {
                                DeleteCurrentEvents(currentContractToUpdate.CurrentEvents, createdPastContract);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, $"Contract edit failed");
                                _context.PastContracts.Remove(createdPastContract);
                                await _context.SaveChangesAsync();
                                throw ex;
                            }
                        }

                        if (contractViewModel.CurrentDocumentsToEdit != null && contractViewModel.CurrentDocumentsToEdit.Any())
                        {
                            try
                            {
                                await DeleteCurrentDocuments(contractViewModel.CurrentDocumentsToEdit,
                                                        true, createdPastContract);
                                 
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, $"Contract edit failed");
                                _context.PastContracts.Remove(createdPastContract);
                                await _context.SaveChangesAsync();
                                throw ex;
                            }
                        }

                        _context.CurrentContracts.Remove(currentContractToUpdate);

                        try
                        {
                            await _context.SaveChangesAsync();
                            try
                            {
                                string contractLink = Url.Action(nameof(Details), "Contracts", new { id = createdPastContract.ID, contractStatus = status.Name }, Request.Scheme);
                                Message message = new Message(new string[] { createdPastContract.Branch.HeadEmail }, "Your branch contract status has been changed", $"Contract #{createdPastContract.ContractNumber} status has been changed, to view changes, please click or copy the link: {contractLink}", null, true);
                                await _emailSender.SendEmailAsync(message);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, $"A message has not been sent to {createdPastContract.Branch.HeadEmail}");
                                throw ex;
                            }
                            return RedirectToAction(nameof(Details), new { id = createdPastContract.ID, contractStatus = status.Name });
                        }
                        catch (DbUpdateException DbUExPastContract)
                        {
                            _context.PastContracts.Remove(createdPastContract);
                            await _context.SaveChangesAsync();
                            _logger.LogError(DbUExPastContract, "Unable to save changes");

                            ModelState.AddModelError("", "Unable to save changes. " +
                            "Try again, and if the problem persists " +
                            "see your system administrator. ");
                        }
                    }
                }
            }

            SelectList suppliersSelect = await GetSuppliersForSelect();
            SelectList branchesSelect = await GetBranchesForSelect();
            SelectList contractStatusesSelect = await GetContractStatusesForSelect();

            ContractViewModel model = ViewModelContractFactory.Edit(contractViewModel.Contract,
                contractViewModel.CurrentDocumentsToEdit, suppliersSelect,
                branchesSelect, contractStatusesSelect);
            return View(model);
        }

        [Authorize(Roles = "ContractStaff")]
        private List<CurrentDocumentVM> PopulateCurrentDocumentsToEdit(Contract contract)
        {
            if (contract == null)
            {
                throw new ElementNotFoundException("Something went wrong during a contract edit,", new ArgumentNullException($"{nameof(contract)}"));
            }

            List<CurrentDocumentVM> currentDocumentsToEdit = new List<CurrentDocumentVM>();
            foreach (CurrentDocument document in contract.CurrentDocuments)
            {
                currentDocumentsToEdit.Add(new CurrentDocumentVM
                {
                    ID = document.ID,
                    Name = document.Name,
                    PathToDocument = document.PathToDocument,
                    DateOfUploading = document.DateOfUploading,
                    IsDeleted = document.IsDeleted
                });
            }
            return currentDocumentsToEdit;
        }

        private void DeleteCurrentEvents(ICollection<CurrentEvent> currentEvents, PastContract createdPastContract)
        {
            foreach (var currentEvent in currentEvents)
            {
                _context.PastEvents.Add(new PastEvent
                {
                    Title = currentEvent.Title,
                    Description = currentEvent.Description,
                    AllDay=currentEvent.AllDay,
                    Start=currentEvent.Start,
                    End=currentEvent.End,
                    PastContractID=createdPastContract.ID
                });
            }
        }

        [Authorize(Roles = "ContractStaff")]
        private async Task DeleteCurrentDocuments(List<CurrentDocumentVM> currentDocuments,
            bool isPastContract, PastContract createdPastContract)
        {
            if (!isPastContract)
            {
                foreach (CurrentDocumentVM currentDocumentVM in currentDocuments)
                {
                    if (currentDocumentVM.IsMarkedToDelete)
                    {
                        CurrentDocument documentToDelete = await _context.CurrentDocuments
                        .FirstOrDefaultAsync(d => d.ID == currentDocumentVM.ID);

                        if (documentToDelete == null)
                        {
                            throw new ElementNotFoundException("Something went wrong during a contract edit,",
                                new ArgumentNullException($"{nameof(documentToDelete)}"));
                        }

                        documentToDelete.IsDeleted = currentDocumentVM.IsMarkedToDelete;
                    }

                }
            }
            else
            {
                foreach (CurrentDocumentVM currentDocumentVM in currentDocuments)
                {
                    _context.PastDocuments.Add(new PastDocument
                    {
                        Name = currentDocumentVM.Name,
                        PathToDocument = currentDocumentVM.PathToDocument,
                        DateOfUploading = currentDocumentVM.DateOfUploading,
                        PastContractID = createdPastContract.ID,
                        IsDeleted = currentDocumentVM.IsDeleted || currentDocumentVM.IsMarkedToDelete
                    });
                }
            }
        }

        //post
        [Authorize(Roles = "ContractStaff")]
        [HttpPost]
        public async Task<bool> Delete(int? id)
        {
            if (id == null)
            {
                _logger.LogError(new ArgumentNullException(nameof(id)), "Delete failed");
                return false;
            }

            CurrentContract currentContractToDelete = await _context.CurrentContracts.Include(c => c.CurrentDocuments).Include(c => c.Branch).FirstOrDefaultAsync(c => c.ID == id);

            if (currentContractToDelete == null)
            {
                _logger.LogError(new DbUpdateConcurrencyException("CurrentContractToDelete during delete was not found"), "CurrentContractToDelete was not found");
                return true;
            }

            try
            {
                _context.CurrentContracts.Remove(currentContractToDelete);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (DbUpdateException eUp)
            {
                _logger.LogError(eUp, "Delete failed.");
                return false;
            }
        }

        public IActionResult Export(SearchModel searchModel)
        {
            IQueryable<CurrentContract> currentContracts = _context.CurrentContracts
                .Include(c => c.Supplier).Include(c => c.Branch)
                .Include(c => c.ContractStatus).AsNoTracking();
            IQueryable<PastContract> pastContracts = _context.PastContracts.Include(c => c.Supplier)
                .Include(c => c.Branch).Include(c => c.ContractStatus).AsNoTracking();

            if (searchModel.ContractType == "currentContracts")
            {
                if (searchModel.CourtDisputeSearch == null)
                {
                    searchModel.CourtDisputeSearch = "includeCourtDispute";
                }

                switch (searchModel.CourtDisputeSearch)
                {
                    case ("onlyWithCourtDisputes"):
                        currentContracts = currentContracts.Where(c => c.IsCourtDispute == true); break;
                    case ("excludeCourtDisputes"):
                        currentContracts = currentContracts.Where(c => c.IsCourtDispute == false); break;
                    default:

                        break;
                }

                if (!string.IsNullOrEmpty(searchModel.ContractNumber))
                {
                    currentContracts = currentContracts.Where(c => c.ContractNumber.Contains(searchModel.ContractNumber));
                }
                if (searchModel.ContractAmountFrom != default)
                {
                    if (decimal.TryParse(searchModel.ContractAmountFrom, out decimal ContractAmountFromDec))
                    {
                        currentContracts = currentContracts.Where(c => (c.ContractAmount >= ContractAmountFromDec));
                    }
                    else
                    {
                        _logger.LogError(new FormatException(), "Something went wrong during {ContractAmountFrom} parsing", searchModel.ContractAmountFrom);
                        throw new ProccesingException("Something went wrong during Contracts filtering,",
                            new FormatException());
                    }

                }
                if (searchModel.ContractAmountTo != default)
                {
                    if (decimal.TryParse(searchModel.ContractAmountTo, out decimal ContractAmountToDec))
                    {
                        currentContracts = currentContracts.Where(c => (c.ContractAmount <= ContractAmountToDec));
                    }
                    else
                    {
                        _logger.LogError(new FormatException(), "Something went wrong during {ContractAmountTo} parsing", searchModel.ContractAmountTo);
                        throw new ProccesingException("Something went wrong during Contracts filtering,",
                            new FormatException());
                    }
                }
                if (searchModel.ConclusionDateFrom != default)
                {
                    currentContracts = currentContracts.Where(c => (c.ConclusionDate >= searchModel.ConclusionDateFrom));
                }
                if (searchModel.ConclusionDateTo != default)
                {
                    currentContracts = currentContracts.Where(c => (c.ConclusionDate <= searchModel.ConclusionDateTo));
                }
                if (searchModel.ExpirationDateFrom != default)
                {
                    currentContracts = currentContracts.Where(c => (c.ExpirationDate >= searchModel.ExpirationDateFrom));
                }
                if (searchModel.ExpirationDateTo != default)
                {
                    currentContracts = currentContracts.Where(c => (c.ExpirationDate <= searchModel.ExpirationDateTo));
                }
                if (searchModel.PastContractStatusID != default)
                {
                    searchModel.PastContractStatusID = default;
                }
                if (!String.IsNullOrEmpty(searchModel.NameCodeSupplierSearch))
                {
                    currentContracts = currentContracts.Where(c => c.Supplier.Name.Contains(searchModel.NameCodeSupplierSearch) || c.Supplier.LegalCode.Contains(searchModel.NameCodeSupplierSearch));
                }
                if (!String.IsNullOrEmpty(searchModel.NameBranchSearch))
                {
                    currentContracts = currentContracts.Where(c => c.Branch.Name.Contains(searchModel.NameBranchSearch) || c.Branch.Code.Contains(searchModel.NameBranchSearch.ToUpper()));
                }

                currentContracts = currentContracts.OrderBy(c => c.ConclusionDate).ThenBy(c => c.ContractNumber);

                /*create table*/
                DataTable dtCurrent = CreateContractTable(currentContracts, false);

                /*export table*/
                try
                {
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        AddContractWorkSheet(dtCurrent, wb);

                        using (MemoryStream stream = new MemoryStream())
                        {
                            wb.SaveAs(stream);
                            string fileName = $"Contracts_{DateTime.Now:ddMMyyyy_hhmmss}.xlsx";
                            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new ProccesingException("Something went wrong during report exporting,", ex);
                }
            }

            searchModel.CourtDisputeSearch = default;

            if (!string.IsNullOrEmpty(searchModel.ContractNumber))
            {
                pastContracts = pastContracts.Where(c => c.ContractNumber.Contains(searchModel.ContractNumber));
            }

            if (searchModel.ContractAmountFrom != default)
            {
                if (decimal.TryParse(searchModel.ContractAmountFrom, out decimal ContractAmountFromDec))
                {
                    currentContracts = currentContracts.Where(c => (c.ContractAmount >= ContractAmountFromDec));
                }
                else
                {
                    _logger.LogError(new FormatException(), "Something went wrong during {ContractAmountFrom} parsing", searchModel.ContractAmountFrom);
                    throw new ProccesingException("Something went wrong during Contracts filtering,",
                        new FormatException());
                }
            }

            if (searchModel.ContractAmountTo != default)
            {
                if (decimal.TryParse(searchModel.ContractAmountTo, out decimal ContractAmountToDec))
                {
                    currentContracts = currentContracts.Where(c => (c.ContractAmount <= ContractAmountToDec));
                }
                else
                {
                    _logger.LogError(new FormatException(), "Something went wrong during {ContractAmountTo} parsing", searchModel.ContractAmountTo);
                    throw new ProccesingException("Something went wrong during Contracts filtering,",
                        new FormatException());
                }
            }

            if (searchModel.ConclusionDateFrom != default)
            {
                pastContracts = pastContracts.Where(c => (c.ConclusionDate >= searchModel.ConclusionDateFrom));
            }

            if (searchModel.ConclusionDateTo != default)
            {
                pastContracts = pastContracts.Where(c => (c.ConclusionDate <= searchModel.ConclusionDateTo));
            }

            if (searchModel.ExpirationDateFrom != default)
            {
                pastContracts = pastContracts.Where(c => (c.ExpirationDate >= searchModel.ExpirationDateFrom));
            }

            if (searchModel.ExpirationDateTo != default)
            {
                pastContracts = pastContracts.Where(c => (c.ExpirationDate <= searchModel.ExpirationDateTo));
            }

            if (searchModel.PastContractStatusID != default)
            {
                pastContracts = pastContracts.Where(c => c.ContractStatus.ID == searchModel.PastContractStatusID);
            }

            if (!String.IsNullOrEmpty(searchModel.NameCodeSupplierSearch))
            {
                pastContracts = pastContracts.Where(c => c.Supplier.Name.Contains(searchModel.NameCodeSupplierSearch) || c.Supplier.LegalCode.Contains(searchModel.NameCodeSupplierSearch));
            }
            if (!String.IsNullOrEmpty(searchModel.NameBranchSearch))
            {
                pastContracts = pastContracts.Where(c => c.Branch.Name.Contains(searchModel.NameBranchSearch) || c.Branch.Code.Contains(searchModel.NameBranchSearch.ToUpper()));
            }

            pastContracts = pastContracts.OrderBy(c => c.ConclusionDate).ThenBy(c => c.ContractNumber);

            /*create table*/
            DataTable dtPast = CreateContractTable(pastContracts, true);

            /*export table*/
            try
            {
                using (XLWorkbook wbP = new XLWorkbook())
                {
                    AddContractWorkSheet(dtPast, wbP);

                    using (MemoryStream stream = new MemoryStream())
                    {
                        wbP.SaveAs(stream);
                        string fileName = "Contracts" + DateTime.Now.ToString("ddMMyyyy_hhmmss") + ".xlsx";
                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ProccesingException("Something went wrong during report exporting,", ex);
            }
        }

        public IActionResult ExportHome()
        {
            IQueryable<CurrentContract> currentContracts = _context.CurrentContracts
                .Include(c => c.Supplier).Include(c => c.Branch)
                .Include(c => c.ContractStatus).AsNoTracking().OrderBy(c => c.ConclusionDate).ThenBy(c => c.ContractNumber);
            IQueryable<PastContract> pastContracts = _context.PastContracts.Include(c => c.Supplier)
                .Include(c => c.Branch).Include(c => c.ContractStatus).AsNoTracking().OrderBy(c => c.ConclusionDate).ThenBy(c => c.ContractNumber);

            /*create table current*/
            DataTable dtCurrent = CreateContractTable(currentContracts, false);

            /*create table past*/
            DataTable dtPast = CreateContractTable(pastContracts, true);

            /*export tables*/

            XLWorkbook wb = new XLWorkbook();

            try
            {
                using (wb)
                {
                    AddContractWorkSheet(dtCurrent, wb);
                    AddContractWorkSheet(dtPast, wb);

                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        string fileName = "Contracts" + DateTime.Now.ToString("ddMMyyyy_hhmmss") + ".xlsx";
                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ProccesingException("Something went wrong during report exporting,", ex);
            }
        }

        private DataTable CreateContractTable(IQueryable<Contract> contracts, bool isPastContracts)
        {
            DataTable dataTable = new DataTable(isPastContracts ? "Past_Contracts" : "Current_Contracts");

            dataTable.Columns.AddRange(new DataColumn[11] { new DataColumn("Contract Number", typeof(string)),
                                        new DataColumn("Conclusion Date",typeof(DateTime)),
                                        new DataColumn("Expiration Date",typeof(DateTime)),
                                        new DataColumn("Amount (UAH)", typeof(decimal)),
                                        new DataColumn("Type of Subject",typeof(string)),
                                        new DataColumn("Subject", typeof(string)),
                                        new DataColumn("Contract Status", typeof(string)),
                                        new DataColumn("Supplier", typeof(string)),
                                        new DataColumn("Branch", typeof(string)),
                                        new DataColumn("Prolonged",typeof(string)),
                                        new DataColumn("Remarks", typeof(string))
            });
            if (!isPastContracts)
            {
                dataTable.Columns.Add(new DataColumn("Court Dispute", typeof(string)));
            }
            else
            {
                dataTable.Columns.Add(new DataColumn("Transition Date", typeof(DateTime)));
            }
            foreach (Contract contract in contracts)
            {
                dataTable.Rows.Add(
                    contract.ContractNumber,
                    contract.ConclusionDate.Date,
                    contract.ExpirationDate.Date.ToString("d"),
                    contract.ContractAmount,
                    contract.IsGoods ? "Goods" : "Services",
                    contract.Subject,
                    contract.ContractStatus.Name,
                    contract.Supplier.Name,
                    contract.Branch.Name,
                    contract.IsProlonged ? "Prolonged" : "",
                    contract.Remarks,
                    isPastContracts ? contract.TransitionDate.ToString("d") : (contract.IsCourtDispute ? "Court dispute" : ""));
            }
            return dataTable;
        }

        private void AddContractWorkSheet(DataTable contractTable, IXLWorkbook contractBook)
        {
            IXLWorksheet ws = contractBook.Worksheets.Add(contractTable);

            ws.Row(1).InsertRowsAbove(2);

            //Styling dataTable
            ws.Rows().AdjustToContents();
            ws.Columns().AdjustToContents();

            ws.Row(3).Height = 20;
            ws.Row(3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Row(3).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            ws.Row(3).Style.Fill.BackgroundColor = XLColor.FromArgb(0xf8f9fa);
            ws.Row(3).Style.Font.Bold = true;


            ws.RangeUsed().Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            ws.RangeUsed().Style.Border.TopBorder = XLBorderStyleValues.Thin;
            ws.RangeUsed().Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            ws.RangeUsed().Style.Border.RightBorder = XLBorderStyleValues.Thin;
            ws.Column(4).Style.NumberFormat.Format = "#,##0.00";
            ws.Columns(2, 3).Style.NumberFormat.Format = "dd/MM/yyyy";
            // Table Heading
            ws.Cell(1, 1).Value = "Current Contracts Report";
            ws.Row(1).Style.Font.FontSize = 16;
            ws.Row(1).Height = 25;
            ws.Row(1).Style.Font.Bold = true;
            ws.Range(1, 1, 1, 5).Merge();
            ws.Range(1, 1, 1, 5).Style.Border.OutsideBorder = XLBorderStyleValues.None;
            ws.Range(1, 1, 1, 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
            ws.Range(1, 1, 1, 5).Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);

            ws.Column(6).Width = 35;
            ws.Column(6).Style.Alignment.WrapText = true;

            ws.Column(8).Width = 35;
            ws.Column(8).Style.Alignment.WrapText = true;

            ws.Column(9).Width = 35;
            ws.Column(9).Style.Alignment.WrapText = true;

            ws.Column(11).Width = 35;
            ws.Column(11).Style.Alignment.WrapText = true;
        }

        public IActionResult ShowCharts()
        {
            return View();
        }

        [HttpPost]
        public async Task<IDictionary<string, Object>> GetChartData(int monthesAgo)
        {
            if (monthesAgo == 0)
            {
                throw new ElementNotFoundException("The monthesAgo cannot be 0.",
                    new ArgumentNullException($"{nameof(monthesAgo)}"));
            }
            IDictionary<string, Object> data = new Dictionary<string, object>();

            DateTime startDate = DateTime.Now.AddMonths(-monthesAgo).Date;
            DateTime endDate = DateTime.Now.Date;

            List<CurrentContract> lastCurrentContrats =
            await _context.CurrentContracts.Where(c => c.ConclusionDate >= startDate).ToListAsync();

            List<PastContract> lastPastContracts =
            await _context.PastContracts.Where(c => c.ConclusionDate >= startDate).ToListAsync();

            if (lastCurrentContrats == null)
            {
                throw new ElementNotFoundException("The lastCurrentContrats cannot be null.",
                        new ArgumentNullException($"{nameof(lastCurrentContrats)}"));
            }

            if (lastPastContracts == null)
            {
                throw new ElementNotFoundException("The lastCurrentContrats cannot be null.",
                         new ArgumentNullException($"{nameof(lastPastContracts)}"));
            }

            var groupedCurrentContracts = lastCurrentContrats.Select(c => c).GroupBy(c => new { c.ConclusionDate.Year, c.ConclusionDate.Month });

            var groupedPastContracts = lastPastContracts.Select(c => c).GroupBy(c => new { c.ConclusionDate.Year, c.ConclusionDate.Month });

            if (groupedCurrentContracts == null)
            {
                throw new ElementNotFoundException("The groupedCurrentContracts cannot be null.",
                         new ArgumentNullException($"{nameof(groupedCurrentContracts)}"));
            }

            if (groupedPastContracts == null)
            {
                throw new ElementNotFoundException("The groupedPastContracts cannot be null.",
                          new ArgumentNullException($"{nameof(groupedPastContracts)}"));
            }

            List<ContractsInMonth> conractsInMonthes = new List<ContractsInMonth>();

            CultureInfo culture = new CultureInfo("en-UK");

            for (int i = 0; i < monthesAgo + 1; i++)
            {
                conractsInMonthes.Add(new ContractsInMonth
                {
                    DateInOneMonth = startDate.AddMonths(i),
                    MonthName = startDate.AddMonths(i).ToString("MMM yy", culture)
                });
            }

            foreach (var contractsInMonth in conractsInMonthes)
            {
                foreach (var groupingCurrentContracts in groupedCurrentContracts)
                {
                    if (groupingCurrentContracts.Key.Month == contractsInMonth.DateInOneMonth.Month
                        && groupingCurrentContracts.Key.Year == contractsInMonth.DateInOneMonth.Year)
                    {
                        contractsInMonth.MonthlyTotalContractQuantity += groupingCurrentContracts.Count();
                        contractsInMonth.MonthlyCurrentContractQuantity += groupingCurrentContracts.Count();
                        foreach (var contract in groupingCurrentContracts)
                        {
                            contractsInMonth.MonthlyTotalAmmount += contract.ContractAmount;
                            contractsInMonth.MonthlyCurrentAmmount += contract.ContractAmount;
                        }
                    }

                }

                foreach (var groupingPastContracts in groupedPastContracts)
                {
                    if (groupingPastContracts.Key.Month == contractsInMonth.DateInOneMonth.Month
                        && groupingPastContracts.Key.Year == contractsInMonth.DateInOneMonth.Year)
                    {
                        contractsInMonth.MonthlyTotalContractQuantity += groupingPastContracts.Count();
                        foreach (var contract in groupingPastContracts)
                        {
                            contractsInMonth.MonthlyTotalAmmount += contract.ContractAmount;
                        }
                    }
                }

            }

            //0
            List<string> labels = conractsInMonthes.Select(cinm => cinm.MonthName).ToList<string>();
            data.Add("labels", labels);

            //1
            List<int> totalContractsQuantity = conractsInMonthes.Select(cinm => cinm.MonthlyTotalContractQuantity)
                .ToList<int>();
            data.Add("totalContractsQuantity", totalContractsQuantity);

            //2
            data.Add("startDate", startDate.ToString("dd-MM-yyyy"));
            //3
            data.Add("endDate", endDate.ToString("dd-MM-yyyy"));

            //4
            List<decimal> totalContractAmmounts = conractsInMonthes.Select(cinm => cinm.MonthlyTotalAmmount).ToList<decimal>();
            data.Add("totalContractAmmounts", totalContractAmmounts);

            //5
            List<int> currentContractQuantities = conractsInMonthes.Select(cinm => cinm.MonthlyCurrentContractQuantity).ToList<int>();
            data.Add("currentContractQuantities", currentContractQuantities);

            //6
            List<decimal> currentContractAmmounts = conractsInMonthes.Select(cinm => cinm.MonthlyCurrentAmmount).ToList<decimal>();
            data.Add("currentContractAmmounts", currentContractAmmounts);

            return data;
        }

        [HttpPost]
        public async Task<string> GetPDFData(int? id, string contractType)
        {
            if (id == null)
            {
                throw new ElementNotFoundException("The Contract cannot be found,", new ArgumentNullException($"{nameof(id)}"));
            }

            Contract contract;

            if (contractType == "Current")
            {
                contract = await _context.CurrentContracts.Include(c => c.Branch)
                    .Include(c => c.Supplier).Include(c => c.ContractStatus)
                    .Include(c => c.CurrentDocuments).AsNoTracking().FirstOrDefaultAsync(c => c.ID == id);
            }
            else
            {
                contract = await _context.PastContracts.Include(c => c.Branch)
                    .Include(c => c.Supplier).Include(c => c.ContractStatus)
                    .Include(c => c.PastDocuments).AsNoTracking().FirstOrDefaultAsync(c => c.ID == id);
            }

            if (contract == null)
            {
                throw new ElementNotFoundException("The Contract cannot be found,", new ArgumentNullException($"{nameof(contract)}"));
            }

            ContractViewModel model = ViewModelContractFactory.Details(contract);
            string pdfContent = _razorRendererHelper.RenderPartialToString("Views/Contracts/_PrintPDFPartial.cshtml", model);
            return pdfContent;
        }

        public async Task<IActionResult> ShowContractEvents(int? id, string contractStatus)
        {
            if (id == null)
            {
                throw new ElementNotFoundException("The Contract cannot be found,", new ArgumentNullException($"{nameof(id)}"));
            }

            Contract contract;
            if (contractStatus == "Current")
            {
                contract = await _context.CurrentContracts.Include(c => c.ContractStatus).AsNoTracking().FirstOrDefaultAsync(c => c.ID == id);
            }
            else
            {
                contract= await _context.PastContracts.Include(c => c.ContractStatus).AsNoTracking().FirstOrDefaultAsync(c => c.ID == id);
            }


            if (contract == null)
            {
                throw new ElementNotFoundException("The Contract cannot be found,", new ArgumentNullException($"{nameof(contract)}"));
            }

            DateTime startDate = new DateTime(contract.ConclusionDate.Year, contract.ConclusionDate.Month, 1);
            string startStr = startDate.ToString("yyyy-MM-dd");

            DateTime endDate = new DateTime(contract.ExpirationDate.AddMonths(1).Year, contract.ExpirationDate.AddMonths(1).Month, 1);
            string endStr = endDate.ToString("yyyy-MM-dd");

            ContractViewModel model = ViewModelContractFactory.Events(contract,startStr,endStr);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> GetEvents(int? id, string contractStatus)
        {
            if (id == null)
            {
                throw new ElementNotFoundException("The Contract cannot be found,", new ArgumentNullException($"{nameof(id)}"));
            }

            if (contractStatus=="Current")
            {
                CurrentContract contract = await _context.CurrentContracts
                    .Include(c => c.CurrentEvents).AsNoTracking().FirstOrDefaultAsync(c => c.ID == id);

                if (contract == null)
                {
                    throw new ElementNotFoundException("The Contract cannot be found,", new ArgumentNullException($"{nameof(contract)}"));
                }

                var events = contract.CurrentEvents.Select(e => new
                {
                    id = e.ID,
                    title = e.Title,
                    start = e.Start,
                    end = e.End,
                    description = e.Description,
                    allDay = e.AllDay,
                }).ToList();
                return new JsonResult(events);
            }
            else
            {
                PastContract contract = await _context.PastContracts
                     .Include(c => c.PastEvents).AsNoTracking().FirstOrDefaultAsync(c => c.ID == id);

                if (contract == null)
                {
                    throw new ElementNotFoundException("The Contract cannot be found,", new ArgumentNullException($"{nameof(contract)}"));
                }

                var events = contract.PastEvents.Select(e => new
                {
                    id = e.ID,
                    title = e.Title,
                    start = e.Start,
                    end = e.End,
                    description = e.Description,
                    allDay = e.AllDay,
                }).ToList();
                return new JsonResult(events);
            }
        }

        [HttpPost]
        public async Task<bool> CreateEvent(int? contractID,
                                    string title, DateTime end, DateTime start,
                                    string description, bool allDay)
        {
            if (contractID == null)
            {
                _logger.LogError(new ArgumentNullException(nameof(contractID)), "Create Event failed");
                return false;
            }

            CurrentContract contract = await _context.CurrentContracts.AsNoTracking().FirstOrDefaultAsync(c => c.ID == contractID);

            if (contract == null)
            {
                _logger.LogError(new ArgumentNullException(nameof(contract)), "Create Event failed");
                return false;
            }

            if (contract.ExpirationDate<DateTime.Now.Date)
            {
                _logger.LogError(new ProccesingException("This contract has expired; you can\'t add its events!"), "Create Event failed");
                return false;
            }

            if (end > contract.ExpirationDate.AddDays(1) || start > contract.ExpirationDate.AddHours(23))
            {
                _logger.LogError(new ProccesingException("Start or End date exceeds expiration contract date."), "Create Event failed");
                return false;
            }

            if (!allDay)
            {
                allDay = CheckAllDay(start, end);
            }

            if (start.Date<DateTime.Now.Date)
            {
                _logger.LogError(new ProccesingException($"Start date is less than today's date. {start}, { DateTime.Now}"),"Create Event failed");
                return false;
            }

            CurrentEvent contractEvent = new CurrentEvent
            {
                Title = title,
                Start = start,
                End = end,
                Description = description,
                CurrentContractID = (int)contractID,
                AllDay = allDay
            };

            try
            {
                _context.CurrentEvents.Add(contractEvent);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (DbUpdateException eUp)
            {
                _logger.LogError(eUp, "Event Create failed.");
                return false;
            }
        }

        private bool CheckAllDay(DateTime start, DateTime end)
        {
            if (start.Hour == 0 && start.Minute == 0 && end.Hour == 0 && end.Minute == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        [HttpPost]
        public async Task<bool> EditEvent(int? eventID, string title, DateTime end, DateTime start,
                                    string description, bool allDay)
        {
            if (eventID == null)
            {
                _logger.LogError(new ArgumentNullException(nameof(eventID)), "Edit Event failed");
                return false;
            }

            CurrentEvent eventToEdit=await _context.CurrentEvents.Include(e=>e.CurrentContract).FirstOrDefaultAsync(e=>e.ID==eventID);
            if (eventToEdit == null)
            {
                _logger.LogError(new ArgumentNullException(nameof(eventToEdit)), "Edit Event failed");
                return false;
            }

            if (eventToEdit.CurrentContract == null)
            {
                _logger.LogError(new ArgumentNullException(nameof(eventToEdit.CurrentContract)), "Create Event failed");
                return false;
            }

            if (eventToEdit.CurrentContract.ExpirationDate < DateTime.Now.Date)
            {
                _logger.LogError(new ProccesingException("This contract has expired; you can\'t edit its events!"), "Edit Event failed");
                return false;
            }

            if (end > eventToEdit.CurrentContract.ExpirationDate.AddDays(1) || start > eventToEdit.CurrentContract.ExpirationDate.AddHours(23))
            {
                _logger.LogError(new ProccesingException("Start or End date exceeds expiration contract date."), "Create Event failed");
                return false;
            }

            if (!allDay)
            {
                allDay = CheckAllDay(start, end);
            }

            if (start.Date < DateTime.Now.Date)
            {
                _logger.LogError(new ProccesingException($"Start date is less than today's date. {start}, {DateTime.Now}"), "Create Event failed");
                return false;
            }

            eventToEdit.Title = title;
            eventToEdit.Start = start;
            eventToEdit.End = end;
            eventToEdit.Description = description;
            eventToEdit.AllDay = allDay;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException eUp)
            {
                _logger.LogError(eUp, "Event Edit failed.");
                return false;
            }
        }

        [HttpPost]
        public async Task<bool> DeleteEvent(int? id)
        {
            if (id == null)
            {
                _logger.LogError(new ArgumentNullException(nameof(id)), "Delete failed");
                return false;
            }

            CurrentEvent eventToDelete = await _context.CurrentEvents.FirstOrDefaultAsync(e => e.ID == id);

            if (eventToDelete == null)
            {
                _logger.LogError(new DbUpdateConcurrencyException("EventToDelete during delete was not found"), "EventToDelete was not found");
                return true;
            }

            try
            {
                _context.CurrentEvents.Remove(eventToDelete);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (DbUpdateException eUp)
            {
                _logger.LogError(eUp, "Delete failed.");
                return false;
            }
        }
    }
}



