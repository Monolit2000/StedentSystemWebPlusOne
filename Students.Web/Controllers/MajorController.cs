using Students.Interfaces;
using Students.Common.Data;
using Students.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Students.Web.Controllers
{
    public class MajorController : Controller
    {

        private readonly IDatabaseService _databaseService;

        public MajorController(StudentsContext context, IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        // GET: ResearchWorkers
        public async Task<IActionResult> Index()
        {
            return View(await _databaseService.GetOllMajorsAsync());
        }

        // GET: ResearchWorkers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var major = await _databaseService.GetMajorWithStudentsAsync(id);

            if (major == null)
            {
                return NotFound();
            }

            return View(major);
        }

        public async Task<IActionResult> Create()
        {

            var listOfStudents = await _databaseService.GetOllStudentsAsync();

            var newMajor = new Major();
            newMajor.AvailableStudents = listOfStudents.ToList();

            return View(newMajor);

        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id, string name, int[] subjectIdDst)
        {
            var major = new Major();

            if (ModelState.IsValid)
            {

                major = await _databaseService.CreateMajorAsync(id, name, subjectIdDst);

                return RedirectToAction(nameof(Index));
            }

            return View(major);
        }



        // GET: ResearchWorkers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var major = await _databaseService.GetMajorWithStudentsAsync(id);

            if (major == null)
            {
                return NotFound();
            }
            return View(major);
        }

        // POST: ResearchWorkers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Major major, int[] subjectIdDst)
        {
            if (id != major.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _databaseService.UpdateMajorAsync(major, subjectIdDst);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_databaseService.MajorExists(major.Id))
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
            return View(major);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var major = await _databaseService.GetMajorWithStudentsAsync(id);

            if (major == null)
            {
                return NotFound();
            }

            return View(major);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _databaseService.DeleteMajor(id);

            return RedirectToAction(nameof(Index));
        }

    
    }
}
