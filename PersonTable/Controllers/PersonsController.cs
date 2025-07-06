using Microsoft.AspNetCore.Mvc;
using PersonTable.Data.Entities;
using PersonTable.Extensions;
using PersonTable.Models;
using PersonTable.Repositories;

namespace PersonTable.Controllers
{
    public class PersonsController : Controller
    {
        private readonly ILogger<PersonsController> _logger;
        private readonly IPersonRepository _personRepository;

        public PersonsController(ILogger<PersonsController> logger, IPersonRepository personRepository)
        {
            _logger = logger;
            _personRepository = personRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> LoadTable(string search, string sortOrder, int page = 1, int pageSize = 5)
        {
            var persons = await _personRepository.GetFilteredAsync(search, sortOrder, page, pageSize);
            var count = await _personRepository.CountFilteredAsync(search);

            var totalPages = (int)Math.Ceiling(count / (double)pageSize);

            var model = new PersonListModel
            {
                Search = search,
                SortOrder = sortOrder,
                CurrentPage = page,
                TotalPages = totalPages,
                PageSize = pageSize,
                Persons = persons.Select(p => p.MapObjectToModel())
            };

            return PartialView("_PersonTable", model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View("CreateEditPerson", new PersonModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(PersonModel model)
        {
            if (!ModelState.IsValid)
                return View("CreateEditPerson", model);

            var person = model.MapModelToObject();

            await _personRepository.AddAsync(person);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var person = await _personRepository.GetByIdAsync(id);

            if (person == null)
                throw new ArgumentException($"Could not find person with id: {id}");

            var model = person.MapObjectToModel();

            return View("CreateEditPerson", model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(PersonModel model)
        {
            if (!ModelState.IsValid)
                return View("CreateEditPerson", model);

            var person = await _personRepository.GetByIdAsync(model.Id ?? 0);

            if (person == null)
                throw new ArgumentException($"Could not find person with id: {model.Id}");

            person.FirstName = model.FirstName;
            person.LastName = model.LastName;
            person.Description = model.Description;
            person.Emails = model.Emails.Where(e => !string.IsNullOrWhiteSpace(e.Address))
                                      .Select(e => new Email { Address = e.Address, PersonId = person.Id })
                                      .ToList();

            await _personRepository.UpdateAsync(person);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _personRepository.DeleteAsync(id);

                return Json(new { success = true, message = "Person has been deleted." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while deleting person with Id: {id}");
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
