using System.Text.Json;
using NotepadApp.Models;

namespace NotepadApp.Services
{
    public class NoteService : INoteService
    {
        private readonly string _filePath;
        private List<Note> _notes;

        public NoteService(IWebHostEnvironment env)
        {
            _filePath = Path.Combine(env.ContentRootPath, "Data", "notes.json");
            _notes = Load();
        }

        private List<Note> Load()
        {
            if (!File.Exists(_filePath)) return new List<Note>();
            var json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<Note>>(json) ?? new List<Note>();
        }

        private void Save() =>
            File.WriteAllText(_filePath, JsonSerializer.Serialize(_notes, new JsonSerializerOptions { WriteIndented = true }));

        public List<Note> GetAll() => _notes.OrderByDescending(n => n.IsPinned).ThenByDescending(n => n.UpdatedAt).ToList();

        public Note? GetById(int id) => _notes.FirstOrDefault(n => n.Id == id);

        public void Add(Note note)
        {
            note.Id = _notes.Count > 0 ? _notes.Max(n => n.Id) + 1 : 1;
            note.CreatedAt = DateTime.Now;
            note.UpdatedAt = DateTime.Now;
            _notes.Add(note);
            Save();
        }

        public void Update(Note note)
        {
            var existing = _notes.FirstOrDefault(n => n.Id == note.Id);
            if (existing == null) return;
            existing.Title = note.Title;
            existing.Content = note.Content;
            existing.Category = note.Category;
            existing.IsPinned = note.IsPinned;
            existing.UpdatedAt = DateTime.Now;
            Save();
        }

        public void Delete(int id)
        {
            _notes.RemoveAll(n => n.Id == id);
            Save();
        }

        public List<Note> Search(string query) =>
            _notes.Where(n => n.Title.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                               n.Content.Contains(query, StringComparison.OrdinalIgnoreCase))
                  .OrderByDescending(n => n.IsPinned).ThenByDescending(n => n.UpdatedAt).ToList();

        public List<string> GetCategories() =>
            _notes.Select(n => n.Category).Distinct().OrderBy(c => c).ToList();
    }
}
