# 📓 Notepad Application

A professional Notepad application built with **ASP.NET Core MVC** and **WinForms Desktop**.

## 🌐 Live Demo

👉 **[https://notepad-desktop-application-using-aspnet-production.up.railway.app/](https://notepad-desktop-application-using-aspnet-production.up.railway.app/)**

---

## 🚀 Projects

| Project | Description |
|---|---|
| `NotepadApp` | ASP.NET Core MVC Web Application |
| `NotepadDesktop` | Windows Desktop App (WinForms) |

---

## ✨ Features

- ✅ Create, Edit, Delete Notes
- ✅ Pin / Unpin important notes
- ✅ Search notes by title or content
- ✅ Categories (Work, Personal, Ideas, Todo, General)
- ✅ Live preview panel (Desktop)
- ✅ Dark theme UI (Desktop)
- ✅ JSON file-based data persistence
- ✅ Responsive UI with Bootstrap 5 (Web)

---

## 🛠️ Tech Stack

- **Language:** C# (.NET 9)
- **Web:** ASP.NET Core MVC, Razor Views, Bootstrap 5
- **Desktop:** WinForms
- **Storage:** JSON file
- **Deployment:** Railway (Docker)

---

## 💻 Run Locally

### Web App
```bash
cd NotepadApp
dotnet run
```
Open: `http://localhost:5278`

### Desktop App
```bash
cd NotepadDesktop
dotnet run
```

### Using Docker
```bash
docker build -t notepad .
docker run -p 8080:8080 notepad
```
Open: `http://localhost:8080`

---

## 📁 Project Structure

```
Notepad/
├── NotepadApp/              # ASP.NET MVC Web App
│   ├── Controllers/
│   ├── Models/
│   ├── Views/
│   ├── Services/
│   └── wwwroot/
├── NotepadDesktop/          # WinForms Desktop App
│   ├── Forms/
│   ├── Models/
│   └── Services/
└── Dockerfile
```

---

## 👨‍💻 Developer

**Rana980-hub** — [GitHub](https://github.com/Rana980-hub)
