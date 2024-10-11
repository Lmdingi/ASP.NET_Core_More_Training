using CRUDExample.Filters.ActionFilters;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace CRUDExample.Controllers
{
    [Route("[controller]")]
    public class PersonsController : Controller
    {
        //private fields
        private readonly IPersonsService _personsService;
        private readonly ICountriesService _countriesService;
        private readonly ILogger<PersonsController> _logger;


        //constructor
        public PersonsController(IPersonsService personsService, ICountriesService countriesService,
        ILogger<PersonsController> logger)
        {
            _personsService = personsService;
            _countriesService = countriesService;
            _logger = logger;
        }

        [Route("[action]")]
        [Route("/")]
        [TypeFilter(typeof(PersonsListActionFilter))]
        public IActionResult Index(string searchBy, string searchString,
        string sortBy = nameof(PersonResponse.PersonName), SortOrderOptions sortOrder = SortOrderOptions.ASC)
        {
            _logger.LogInformation("Index action method of PersonsController");

            _logger.LogDebug($"searchBy: {searchBy}, searchString: {searchString}, sortBy: {sortBy}, sortOrder: {sortOrder}");

            //search
            ViewBag.SearchFields = new Dictionary<string, string>()
            {
                {nameof(PersonResponse.PersonName), "Person Name"},
                {nameof(PersonResponse.Email), "Email"},
                {nameof(PersonResponse.DateOfBirth), "Date of Birth"},
                {nameof(PersonResponse.Gender), "Gender"},
                {nameof(PersonResponse.CountryID), "Country"},
                {nameof(PersonResponse.Address), "Address"}
            };

            List<PersonResponse> persons = _personsService.GetFilteredPerson(searchBy, searchString);
            
            //sort
            List<PersonResponse> sortedPersons = _personsService.GetSortedPersons(persons, sortBy, sortOrder);
            
            return View(sortedPersons);
        }

        //Execute when the user clicks on "Create Person" hyperlink (while opening the create view);
        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            List<CountryResponse> countries = await _countriesService.GetAllCountries();
            ViewBag.Countries = countries;

            return View();
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> Create(PersonAddRequest personAddRequest)
        {
            if (!ModelState.IsValid)
            {
                List<CountryResponse> countries = await _countriesService.GetAllCountries();
                ViewBag.Countries = countries;

                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage).ToList();

                return View();
            }
            PersonResponse personResponse = await _personsService.AddPerson(personAddRequest);

            return RedirectToAction("Index", "Persons");
        }

        [Route("PersonsPdf")]
        public async Task<IActionResult> PersonsPdf()
        {
            //Get list of persons
            List<PersonResponse> persons = _personsService.GetAllPersons();
            // Return views as pdf
            return new ViewAsPdf("PersonsPdf", persons, ViewData)
            {
                PageMargins = new Rotativa.AspNetCore.Options.Margins()
                {
                    Top = 20,
                    Right = 20,
                    Bottom = 2,
                    Left = 20
                },
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
            };
        }

        [Route("PersonsCSV")]
        public async Task<IActionResult> PersonsCSV()
        {
            MemoryStream memoryStream = await _personsService.GetPersonsCSV();
            return File(memoryStream, "application/octet-stream", "persons.csv");
        }

        [Route("PersonsExcel")]
        public async Task<IActionResult> PersonsExcel()
        {
            MemoryStream memoryStream = await _personsService.GetPersonsExcel();
            return File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
             "persons.xlsx");
        }
    }
}
