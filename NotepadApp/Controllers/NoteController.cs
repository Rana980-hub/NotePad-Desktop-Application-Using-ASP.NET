using Microsoft.AspNetCore.Mvc;
using NotepadApp.Models;
using NotepadApp.Services;

namespace NotepadApp.Controllers
{
    public class NoteController : Controller
    {
        private readonly INoteService _noteService;

        public NoteController(INoteService noteService) => _noteService = noteService;

        public IActionResult Index(string? search, string? category)
        {
            var notes = string.IsNullOrWhiteSpace(search)
                ? _noteService.GetAll()
                : _noteService.Search(search);

            if (!string.IsNullOrWhiteSpace(category))
                notes = notes.Where(n => n.Category == category).ToList();

            ViewBag.Search = search;
            ViewBag.Category = category;
            ViewBag.Categories = _noteService.GetCategories();
            return View(notes);
        }

        public IActionResult Details(int id)
        {
            var note = _noteService.GetById(id);
            if (note == null) return NotFound();
            return View(note);
        }

        public IActionResult Create() => View(new Note());

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(Note note)
        {
            if (!ModelState.IsValid) return View(note);
            _noteService.Add(note);
            TempData["Success"] = "Note created successfully!";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var note = _noteService.GetById(id);
            if (note == null) return NotFound();
            return View(note);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(Note note)
        {
            if (!ModelState.IsValid) return View(note);
            _noteService.Update(note);
            TempData["Success"] = "Note updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var note = _noteService.GetById(id);
            if (note == null) return NotFound();
            return View(note);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _noteService.Delete(id);
            TempData["Success"] = "Note deleted successfully!";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult TogglePin(int id)
        {
            var note = _noteService.GetById(id);
            if (note != null)
            {
                note.IsPinned = !note.IsPinned;
                _noteService.Update(note);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
