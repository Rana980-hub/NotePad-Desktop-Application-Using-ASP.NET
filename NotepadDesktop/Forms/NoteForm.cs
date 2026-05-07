using System.ComponentModel;
using NotepadDesktop.Models;

namespace NotepadDesktop.Forms
{
    public class NoteForm : Form
    {
        private TextBox _txtTitle = null!;
        private RichTextBox _txtContent = null!;
        private ComboBox _cboCategory = null!;
        private CheckBox _chkPinned = null!;
        private Button _btnSave = null!, _btnCancel = null!;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Note Note { get; private set; }

        public NoteForm(Note? note = null)
        {
            Note = note != null
                ? new Note { Id = note.Id, Title = note.Title, Content = note.Content, Category = note.Category, IsPinned = note.IsPinned, CreatedAt = note.CreatedAt }
                : new Note();

            Text = note == null ? "New Note" : "Edit Note";
            Size = new Size(700, 550);
            MinimumSize = new Size(500, 400);
            StartPosition = FormStartPosition.CenterParent;
            BackColor = Color.FromArgb(30, 30, 30);
            ForeColor = Color.White;

            BuildUI();
            PopulateFields();
        }

        private void BuildUI()
        {
            var lblTitle = CreateLabel("Title:", 12, 15);
            _txtTitle = new TextBox
            {
                Location = new Point(80, 12),
                Width = 580,
                BackColor = Color.FromArgb(45, 45, 45),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Segoe UI", 11)
            };

            var lblCat = CreateLabel("Category:", 12, 50);
            _cboCategory = new ComboBox
            {
                Location = new Point(80, 47),
                Width = 200,
                BackColor = Color.FromArgb(45, 45, 45),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10)
            };
            _cboCategory.Items.AddRange(new[] { "General", "Work", "Personal", "Ideas", "Todo" });

            _chkPinned = new CheckBox
            {
                Text = "📌 Pin this note",
                Location = new Point(300, 50),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10),
                AutoSize = true
            };

            var lblContent = CreateLabel("Content:", 12, 85);
            _txtContent = new RichTextBox
            {
                Location = new Point(12, 105),
                Size = new Size(660, 360),
                BackColor = Color.FromArgb(45, 45, 45),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Consolas", 11),
                ScrollBars = RichTextBoxScrollBars.Vertical,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };

            _btnSave = new Button
            {
                Text = "💾 Save",
                Location = new Point(12, 475),
                Size = new Size(100, 35),
                BackColor = Color.FromArgb(0, 120, 215),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left,
                Cursor = Cursors.Hand
            };
            _btnSave.FlatAppearance.BorderSize = 0;
            _btnSave.Click += BtnSave_Click;

            _btnCancel = new Button
            {
                Text = "Cancel",
                Location = new Point(120, 475),
                Size = new Size(90, 35),
                BackColor = Color.FromArgb(60, 60, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left,
                Cursor = Cursors.Hand,
                DialogResult = DialogResult.Cancel
            };
            _btnCancel.FlatAppearance.BorderSize = 0;

            Controls.AddRange(new Control[] { lblTitle, _txtTitle, lblCat, _cboCategory, _chkPinned, lblContent, _txtContent, _btnSave, _btnCancel });
        }

        private Label CreateLabel(string text, int x, int y) => new Label
        {
            Text = text,
            Location = new Point(x, y),
            ForeColor = Color.LightGray,
            Font = new Font("Segoe UI", 9),
            AutoSize = true
        };

        private void PopulateFields()
        {
            _txtTitle.Text = Note.Title;
            _txtContent.Text = Note.Content;
            _cboCategory.Text = Note.Category;
            _chkPinned.Checked = Note.IsPinned;
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_txtTitle.Text))
            {
                MessageBox.Show("Title is required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            Note.Title = _txtTitle.Text.Trim();
            Note.Content = _txtContent.Text;
            Note.Category = string.IsNullOrWhiteSpace(_cboCategory.Text) ? "General" : _cboCategory.Text.Trim();
            Note.IsPinned = _chkPinned.Checked;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
