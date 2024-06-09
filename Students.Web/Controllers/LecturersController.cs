using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Students.Common.Data;
using Students.Common.Models;
using Students.Interfaces;

namespace Students.Web.Controllers
{
    public class LecturersController : Controller
    {

        #region CTOR

        private readonly ILogger _logger;
        private readonly IDatabaseService _databaseService;
        private readonly ISharedResourcesService _sharedResourcesService;

        public LecturersController(
           IDatabaseService databaseService,
           ILogger<StudentsController> logger,
           ISharedResourcesService sharedResourcesService)
        {
            _logger = logger;
            _databaseService = databaseService;
            _sharedResourcesService = sharedResourcesService;
        }

        #endregion

        // GET: LecturersController
        public async Task<IActionResult> Index()
        {
            return View(await _databaseService.GetOllLecturersAsync());
        }

        // GET: LecturersController/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lecturer = await _databaseService.GetLecturerWithSubjectsAsync(id);

            if (lecturer == null)
            {
                return NotFound();
            }

            return View(lecturer);
        }

        // GET: LecturersController/Create
        public async Task<IActionResult> Create()
        {
            var listOfSubjects = await _databaseService.GetOllSubjectsAsync();

            var newLecturer = new Lecturer();
            newLecturer.AvailableSubjects = listOfSubjects;

            return View(newLecturer);
        }

        // POST: LecturersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Lecturer lecturer, int[] subjectIdDst)
        {
            if (ModelState.IsValid)
            {
                await _databaseService.CreateLecturerAsync(lecturer, subjectIdDst);

                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        // GET: LecturersController/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lecture = await _databaseService.GetLecturerWithSubjectsAsync(id);

            if (lecture == null)
            {
                return NotFound();
            }

            return View(lecture);
        }

        // POST: LecturersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Lecturer lecturer, int[] subjectIdDst)
        {
            if (id != lecturer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _databaseService.UpdateLecturerAsync(lecturer, subjectIdDst);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_databaseService.LecturerExists(lecturer.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            return View(lecturer);
        }

        // GET: LecturersController/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lectureHall = await _databaseService.GetLecturerWithSubjectsAsync(id);

            if (lectureHall == null)
            {
                return NotFound();
            }

            return View(lectureHall);
        }

        // POST: LecturersController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            await _databaseService.DeleteLecturerAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
