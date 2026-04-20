# 📚 Library Book Borrowing System

**Team:** Backend Devs
**Final Submission Due:** April 26

## Overview
A RESTful backend API for a Library Book Borrowing System built with ASP.NET Core.

## Project Structure
```
LibraryBorrowingSystem/
├── Controllers/
├── Services/
├── Repositories/
├── DTOs/
│   ├── Request/
│   └── Response/
├── Models/
├── Middleware/
└── Program.cs
```

## API Endpoints

### Books
- GET    /api/books
- GET    /api/books/{id}
- POST   /api/books
- PUT    /api/books/{id}
- DELETE /api/books/{id}

### Members
- GET    /api/members
- GET    /api/members/{id}
- POST   /api/members
- PUT    /api/members/{id}
- DELETE /api/members/{id}

### Borrowing
- POST   /api/borrow
- POST   /api/borrow/return
- GET    /api/borrow
- GET    /api/borrow/member/{memberId}

## Getting Started
```bash
git clone https://github.com/myerm061/library-book-borrowing-system.git
cd library-book-borrowing-system
dotnet restore
dotnet run --project LibraryBorrowingSystem
```

## Error Format
```json
{ "error": "Message here" }
```
