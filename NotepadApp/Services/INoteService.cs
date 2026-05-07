using NotepadApp.Models;

namespace NotepadApp.Services
{
    public interface INoteService
    {
        List<Note> GetAll();
        Note? GetById(int id);
        void Add(Note note);
        void Update(Note note);
        void Delete(int id);
        List<Note> Search(string query);
        List<string> GetCategories();
    }
}
