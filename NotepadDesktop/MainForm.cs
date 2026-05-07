using NotepadDesktop.Forms;
using NotepadDesktop.Models;
using NotepadDesktop.Services;

namespace NotepadDesktop
{
    public class MainForm : Form
    {
        private readonly NoteService _service = new();
        private ListView _listView = null!;
        private TextBox _txtSearch = null!;
        private Label _lblCount = null!;
        private RichTextBox _txtPreview = null!;
        private Label _lblPreviewTitle = null!;
        private SplitContainer _split = null!;

        public MainForm()
        {
            Text = "Notepad";
            Size = new Size(1100, 680);
            MinimumSize = new Size(800, 500);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.FromArgb(25, 25, 25);
            ForeColor = Color.White;
            Font = new Font("Segoe UI", 10);

            BuildUI();
            LoadNotes();

            Shown += (s, e) => _split.SplitterDistance = (int)(_split.Width * 0.58);
        }

        private void BuildUI()
        {
            var toolbar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 55,
                BackColor = Color.FromArgb(35, 35, 35)
            };

            var btnNew    = MakeButton("+ New Note",   Color.FromArgb(0, 120, 215), 8);
            var btnEdit   = MakeButton("Edit",         Color.FromArgb(80, 80, 80),  130);
            var btnDelete = MakeButton("Delete",       Color.FromArgb(180, 40, 40), 220);
            var btnPin    = MakeButton("Pin / Unpin",  Color.FromArgb(160, 120, 0), 315);

            btnNew.Click    += (s, e) => OpenNoteForm(null);
            btnEdit.Click   += (s, e) => EditSelected();
            btnDelete.Click += (s, e) => DeleteSelected();
            btnPin.Click    += (s, e) => PinSelected();

            _txtSearch = new TextBox
            {
                PlaceholderText = "Search notes...",
                Location = new Point(450, 13),
                Width = 240,
                BackColor = Color.FromArgb(50, 50, 50),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Segoe UI", 10)
            };
            _txtSearch.TextChanged += (s, e) => LoadNotes(_txtSearch.Text);

            _lblCount = new Label
            {
                Location = new Point(700, 18),
                AutoSize = true,
                ForeColor = Color.Gray,
                Font = new Font("Segoe UI", 9)
            };

            toolbar.Controls.AddRange(new Control[] { btnNew, btnEdit, btnDelete, btnPin, _txtSearch, _lblCount });

            _split = new SplitContainer
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(25, 25, 25)
            };

            // Left: ListView
            _listView = new ListView
            {
                Dock = DockStyle.Fill,
                View = View.Details,
                FullRowSelect = true,
                GridLines = false,
                BackColor = Color.FromArgb(30, 30, 30),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.None,
                Font = new Font("Segoe UI", 10),
                MultiSelect = false
            };
            _listView.Columns.Add("", 28);
            _listView.Columns.Add("Title", 210);
            _listView.Columns.Add("Category", 95);
            _listView.Columns.Add("Updated", 120);
            _listView.Columns.Add("Preview", 190);
            _listView.SelectedIndexChanged += ListView_SelectionChanged;
            _listView.DoubleClick += (s, e) => EditSelected();
            _split.Panel1.Controls.Add(_listView);

            // Right: Preview
            var previewPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(35, 35, 35),
                Padding = new Padding(12)
            };

            _lblPreviewTitle = new Label
            {
                Dock = DockStyle.Top,
                Height = 38,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Text = "Select a note to preview"
            };

            _txtPreview = new RichTextBox
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(35, 35, 35),
                ForeColor = Color.LightGray,
                BorderStyle = BorderStyle.None,
                Font = new Font("Consolas", 11),
                ReadOnly = true,
                ScrollBars = RichTextBoxScrollBars.Vertical
            };

            previewPanel.Controls.Add(_txtPreview);
            previewPanel.Controls.Add(_lblPreviewTitle);
            _split.Panel2.Controls.Add(previewPanel);

            Controls.Add(_split);
            Controls.Add(toolbar);
        }

        private Button MakeButton(string text, Color color, int x) => new Button
        {
            Text = text,
            Location = new Point(x, 10),
            Size = new Size(110, 34),
            BackColor = color,
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 9),
            Cursor = Cursors.Hand,
            FlatAppearance = { BorderSize = 0 }
        };

        private void LoadNotes(string search = "")
        {
            var notes = string.IsNullOrWhiteSpace(search) ? _service.GetAll() : _service.Search(search);
            _listView.Items.Clear();
            foreach (var note in notes)
            {
                var item = new ListViewItem(note.IsPinned ? "P" : "");
                item.SubItems.Add(note.Title);
                item.SubItems.Add(note.Category);
                item.SubItems.Add(note.UpdatedAt.ToString("MMM dd, yyyy"));
                item.SubItems.Add(note.Content.Length > 60 ? note.Content[..60] + "..." : note.Content);
                item.Tag = note.Id;
                item.ForeColor = note.IsPinned ? Color.Gold : Color.White;
                _listView.Items.Add(item);
            }
            _lblCount.Text = $"{notes.Count} note(s)";
            ClearPreview();
        }

        private void ListView_SelectionChanged(object? sender, EventArgs e)
        {
            if (_listView.SelectedItems.Count == 0) { ClearPreview(); return; }
            var id = (int)_listView.SelectedItems[0].Tag!;
            var note = _service.GetById(id);
            if (note == null) return;
            _lblPreviewTitle.Text = note.Title;
            _txtPreview.Text = note.Content;
        }

        private void ClearPreview()
        {
            _lblPreviewTitle.Text = "Select a note to preview";
            _txtPreview.Text = string.Empty;
        }

        private void OpenNoteForm(Note? note)
        {
            using var form = new NoteForm(note);
            if (form.ShowDialog() != DialogResult.OK) return;
            if (note == null) _service.Add(form.Note);
            else _service.Update(form.Note);
            LoadNotes(_txtSearch.Text);
        }

        private void EditSelected()
        {
            if (_listView.SelectedItems.Count == 0) return;
            var id = (int)_listView.SelectedItems[0].Tag!;
            OpenNoteForm(_service.GetById(id));
        }

        private void DeleteSelected()
        {
            if (_listView.SelectedItems.Count == 0) return;
            var id = (int)_listView.SelectedItems[0].Tag!;
            var note = _service.GetById(id);
            if (note == null) return;
            if (MessageBox.Show($"Delete \"{note.Title}\"?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            { _service.Delete(id); LoadNotes(_txtSearch.Text); }
        }

        private void PinSelected()
        {
            if (_listView.SelectedItems.Count == 0) return;
            _service.TogglePin((int)_listView.SelectedItems[0].Tag!);
            LoadNotes(_txtSearch.Text);
        }
    }
}
