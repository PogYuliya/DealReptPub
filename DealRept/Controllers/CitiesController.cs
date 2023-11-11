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
    public class CitiesController : Controller
    {
        private readonly DealDbContext _context;
        private readonly ILogger<CitiesController> _logger;

        public CitiesController(DealDbContext ctx, ILogger<CitiesController> logger)
        {
            _context = ctx;
            _logger = logger;
        }

        public async Task<IActionResult> Index(int? pageNumber, SortModel sortModel, string sortOrder, SearchModel searchModel,
            string currentFilterName)
        {
            sortModel.CurrentSort = sortOrder;
            sortModel.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "Name_desc" : "";

            if (searchModel.NameCitySearch != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchModel.NameCitySearch = currentFilterName;
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
            }

            switch (sortOrder)
            {
                case "Name_desc":
                    cities = cities.OrderByDescending(c => c.Name); break;
                default:
                    cities = cities.OrderBy(c => c.Name); break;
            }

            int pageSize = 4;

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

    }
}
