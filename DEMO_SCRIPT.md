# Library Book Borrowing System - Demo Video Script

**Duration Target:** 10-15 minutes  
**Format:** Screen recording with optional voiceover narration  
**Tools:** OBS Studio, ScreenFlow, or equivalent screen recorder

---

## Pre-Recording Setup Checklist

- [ ] Start application: `dotnet run --project LibraryBorrowingSystem`
- [ ] Verify Swagger is running: `https://localhost:5001/swagger/index.html`
- [ ] Open VS Code with project files
- [ ] Open Postman or Browser DevTools (Network tab)
- [ ] Open terminal showing application logs
- [ ] Test all endpoints before recording
- [ ] Prepare sample data
- [ ] Hide sensitive information (API keys, credentials)

---

## Section 1: Project Structure Walkthrough (2 minutes)

**Objective:** Show the architecture and project organization

### 1.1 Directory Structure Tour (0:00-0:30)
```narration
"Let me start by walking through the project structure of the Library Book Borrowing System. 
This is an ASP.NET Core REST API that demonstrates enterprise-level architecture patterns."
```

**Show in IDE:**
```
LibraryBorrowingSystem/
├── Controllers/           (Show files: BooksController, MembersController, BorrowController)
├── Services/              (Show files: IBookService, BookService, IMemberService, MemberService, etc.)
├── Repositories/          (Show files: IBookRepository, BookRepository, IMemberRepository, etc.)
├── Dtos/
│   ├── Request/           (Show: CreateBookDto, BorrowRequestDto, etc.)
│   └── Response/          (Show: BookResponseDto, ErrorResponse, etc.)
├── Models/                (Show: Book, Member, BorrowRecord)
├── Middleware/            (Show: GlobalExceptionMiddleware)
├── Data/                  (Show: LibraryDbContext)
├── Program.cs             (Show: Dependency Injection setup)
└── appsettings.json       (Show: Configuration)
```

**Narration Points:**
- "The Controllers layer handles HTTP requests and routes them to services"
- "The Services layer contains all business logic"
- "The Repositories layer provides data access abstraction"
- "DTOs decouple internal models from API contracts"
- "Middleware handles cross-cutting concerns like error handling"

### 1.2 Code Tour (0:30-1:30)

**Show Program.cs:**
```csharp
// Dependency Injection Setup
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IMemberService, MemberService>();
builder.Services.AddScoped<IBorrowService, BorrowService>();

// In-memory database and caching
builder.Services.AddDbContext<LibraryDbContext>(options =>
    options.UseInMemoryDatabase("LibraryDb"));
builder.Services.AddMemoryCache();

// Middleware
app.UseMiddleware<GlobalExceptionMiddleware>();
```

**Narration:**
"The dependency injection container is configured here, registering all services and repositories. 
Notice we're using an in-memory database for this demo, but this can easily be swapped for SQL Server 
or PostgreSQL. We also register the memory cache for performance optimization."

**Show Controller Example (BooksController):**
```csharp
[HttpGet("{id:int}")]
public async Task<ActionResult<BookResponseDto>> GetById(int id)
{
    var book = await _bookService.GetByIdAsync(id);
    if (book is null) return NotFound(new ErrorResponse("Book not found."));
    return Ok(book);
}
```

**Narration:**
"Here's a typical controller endpoint. Notice how it relies on the service layer for business logic,
and returns standardized responses with proper HTTP status codes."

### 1.3 Domain Models (1:30-2:00)

**Show Models:**
```
Book Model:
- Id, Title, Author, ISBN, TotalCopies, AvailableCopies

Member Model:
- Id, FullName, Email, MembershipDate

BorrowRecord Model:
- Id, BookId, MemberId, BorrowDate, ReturnDate, Status
- Navigation properties to Book and Member
```

**Narration:**
"These are the core entities of our system. A Book can have multiple copies tracked separately.
Members can borrow books, and BorrowRecord tracks each transaction with dates and status."

---

## Section 2: Borrow Flow (2 minutes)

**Objective:** Demonstrate the complete borrowing process with happy path

### 2.1 Create Sample Data (0:00-0:45)

**Show in Swagger/Postman:**

```
Step 1: Create Book
POST /api/books
{
  "title": "Clean Code",
  "author": "Robert C. Martin",
  "isbn": "978-0132350884",
  "totalCopies": 3
}

Response: 201 Created
{
  "id": 1,
  "title": "Clean Code",
  "availableCopies": 3
}
```

**Narration:**
"First, we create a book in the system with 3 available copies."

```
Step 2: Create Member
POST /api/members
{
  "fullName": "Alice Johnson",
  "email": "alice@example.com"
}

Response: 201 Created
{
  "id": 1,
  "fullName": "Alice Johnson",
  "membershipDate": "2026-04-26T..."
}
```

**Narration:**
"Next, we register a library member."

### 2.2 Perform Borrow (0:45-1:30)

```
Step 3: Borrow Book
POST /api/borrow
{
  "bookId": 1,
  "memberId": 1
}

Response: 201 Created
{
  "id": 1,
  "bookId": 1,
  "memberId": 1,
  "borrowDate": "2026-04-26T10:30:00Z",
  "returnDate": null,
  "status": "Borrowed"
}
```

**Show Terminal/Logs:**
"Notice the application logs showing the transaction and database state changes."

### 2.3 Verify State Change (1:30-2:00)

```
Step 4: Check Book Availability
GET /api/books/1

Response:
{
  "id": 1,
  "title": "Clean Code",
  "totalCopies": 3,
  "availableCopies": 2  ← Decreased from 3 to 2
}
```

**Narration:**
"After borrowing, the available copies automatically decreased from 3 to 2. 
This demonstrates the business logic correctly updating inventory."

```
Step 5: Check Member's Borrow History
GET /api/borrow/member/1

Response: [
  {
    "id": 1,
    "bookId": 1,
    "memberId": 1,
    "borrowDate": "2026-04-26T...",
    "returnDate": null,
    "status": "Borrowed"
  }
]
```

**Narration:**
"We can query the member's borrow history to see all books they currently have borrowed."

---

## Section 3: Return Flow (1.5 minutes)

**Objective:** Demonstrate the return process and state cleanup

### 3.1 Return Book (0:00-0:45)

```
Step 1: Return Borrowed Book
POST /api/borrow/return
{
  "borrowRecordId": 1
}

Response: 200 OK
{
  "id": 1,
  "bookId": 1,
  "memberId": 1,
  "borrowDate": "2026-04-26T10:30:00Z",
  "returnDate": "2026-04-26T11:45:00Z",
  "status": "Returned"
}
```

**Narration:**
"When a member returns a book, we record the return date and update the status to 'Returned'."

### 3.2 Verify Inventory Restored (0:45-1:30)

```
Step 2: Check Book Availability Again
GET /api/books/1

Response:
{
  "id": 1,
  "title": "Clean Code",
  "totalCopies": 3,
  "availableCopies": 3  ← Back to 3!
}
```

**Narration:**
"The available copies have been restored to 3. The system correctly incremented 
the available count when the book was returned."

**Show in Borrow History:**
```
GET /api/borrow/member/1

Response: [
  {
    "id": 1,
    "status": "Returned",
    "returnDate": "2026-04-26T11:45:00Z"  ← Now has a return date
  }
]
```

**Narration:**
"The borrow record is still maintained for audit purposes, showing when the book was returned."

---

## Section 4: Validation Example (1.5 minutes)

**Objective:** Show validation errors for invalid requests

### 4.1 Missing Required Field (0:00-0:45)

```
Request: Create Book without Title
POST /api/books
{
  "author": "John Smith",
  "isbn": "978-0132350884",
  "totalCopies": 5
}

Response: 400 Bad Request
{
  "error": "The Title field is required."
}
```

**Narration:**
"When required fields are missing, the API returns a 400 Bad Request with a clear error message."

### 4.2 Invalid Data Format (0:45-1:30)

```
Request: Invalid ISBN Format
POST /api/books
{
  "title": "Clean Code",
  "author": "Robert Martin",
  "isbn": "INVALID-ISBN",
  "totalCopies": 5
}

Response: 400 Bad Request
{
  "error": "The ISBN field is invalid. Expected format: 10 or 13 digit ISBN."
}
```

**Narration:**
"Data format validation ensures only valid data enters the system. Here, an invalid ISBN is rejected."

### 4.3 Business Logic Validation (1:30-1:50)

```
Scenario: Try to borrow when no copies available
Precondition: Book has 0 available copies

Request: Borrow Book
POST /api/borrow
{
  "bookId": 1,
  "memberId": 2
}

Response: 400 Bad Request
{
  "error": "Book is not available. Only 0 copies remaining."
}
```

**Narration:**
"Business logic validation prevents invalid operations like borrowing unavailable books."

---

## Section 5: Error Handling Example (1.5 minutes)

**Objective:** Show error handling for different scenarios

### 5.1 Resource Not Found (0:00-0:45)

```
Request: Get Non-Existent Book
GET /api/books/99999

Response: 404 Not Found
{
  "error": "Book not found."
}
```

**Narration:**
"When a resource doesn't exist, the API returns a 404 Not Found status with a clear message."

```
Request: Update Non-Existent Member
PUT /api/members/99999
{ ... }

Response: 404 Not Found
{
  "error": "Member not found."
}
```

### 5.2 Server Error Handling (0:45-1:30)

**Show GlobalExceptionMiddleware Code:**
```csharp
public async Task InvokeAsync(HttpContext context)
{
    try
    {
        await _next(context);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Unhandled exception occurred.");
        await HandleExceptionAsync(context);
    }
}
```

**Narration:**
"The global exception middleware catches any unhandled exceptions and returns a standardized 
500 Internal Server Error response, preventing sensitive error details from leaking to clients."

```
Response: 500 Internal Server Error
{
  "error": "An unexpected error occurred."
}
```

**Show Terminal Logs:**
"In the server logs, we can see the full exception details for debugging."

---

## Section 6: Concurrency Scenario (2 minutes)

**Objective:** Demonstrate safe handling of simultaneous requests

### 6.1 Setup (0:00-0:30)

**Create Sample Data:**
```
Book: "Concurrent Testing Book", TotalCopies: 1, AvailableCopies: 1
Member 1: "Bob Davis"
Member 2: "Charlie Evans"
```

**Narration:**
"We have one copy of a book and two members who want to borrow it simultaneously. 
This tests our concurrency handling."

### 6.2 Send Simultaneous Requests (0:30-1:30)

**Method 1: Using Postman Runner**
- Open Postman collection
- Select both "Borrow" requests
- Set delay to 0ms
- Run collection

**Method 2: Browser Developer Tools**
- Open two tabs with Swagger
- Fill in request bodies
- Submit both simultaneously

**Show Timeline:**
```
Time    Member 1 (Bob)          Member 2 (Charlie)       Database
─────────────────────────────────────────────────────────────
T1      Borrow Request →
                                 Borrow Request →        Available: 1
T2      Acquire Lock ✓
                                 Wait for Lock...
T3      Check: 1 > 0 ✓
T4      Decrement to 0
T5      Release Lock                                      Available: 0
T6      Return 201 ✓
T7                              Acquire Lock ✓
T8                              Check: 0 > 0 ✗
T9                              Return 400 ✗
```

### 6.3 Verify Results (1:30-2:00)

```
Response 1: 201 Created
{
  "id": 1,
  "bookId": 1,
  "memberId": 1,
  "status": "Borrowed"
}

Response 2: 400 Bad Request
{
  "error": "Book is not available. Only 0 copies remaining."
}

Final State: GET /api/books/1
{
  "availableCopies": 0  ← Correctly 0, not -1
}
```

**Narration:**
"Notice that exactly one request succeeded while the other was safely rejected. 
The database never reached an inconsistent state. This is because our service layer 
uses database-level locking to ensure atomic operations."

---

## Section 7: Caching Behavior (1.5 minutes)

**Objective:** Demonstrate performance improvement from caching

### 7.1 First Request (Cache Miss) (0:00-0:45)

**Show Network Tab / Timings:**
```
Request: GET /api/books/1

Timing: 125ms (includes database query + serialization)
Size: 450 bytes JSON response
```

**Narration:**
"The first request takes about 125 milliseconds because it needs to query the database, 
retrieve the book, and serialize it to JSON."

### 7.2 Subsequent Requests (Cache Hit) (0:45-1:30)

```
Request: GET /api/books/1 (again)

Timing: 3ms (memory cache access)
Size: 450 bytes JSON response (same data)
```

**Narration:**
"The second request completes in just 3 milliseconds! That's about 40 times faster. 
The response comes from the in-memory cache instead of the database."

**Show Cache Invalidation:**
```
Step 1: Modify the book
PUT /api/books/1 { "title": "Updated Title", ... }
Response: 200 OK
Cache Action: Cache entry invalidated

Step 2: Get the book (Cache Miss)
GET /api/books/1
Timing: 125ms (database hit again)
Data: Contains "Updated Title"

Step 3: Get the book again (Cache Hit)
GET /api/books/1
Timing: 3ms (new data cached)
```

**Narration:**
"When we update a book, the cache is automatically invalidated. The next request 
queries the database to get fresh data, and subsequent requests benefit from 
the cache again."

### 7.3 Performance Summary (1:30-1:50)

**Show Metrics:**
```
First Request:  125ms (Database)
Cached Request: 3ms   (Memory)
Improvement:    ~4100% faster (41x)
Cache TTL:      5 minutes
```

**Narration:**
"For a heavily-used library system, this caching strategy dramatically improves 
performance and reduces database load during peak usage times."

---

## Section 8: Summary & Key Takeaways (1 minute)

**Narration:**
"Let's summarize what we've demonstrated today:

1. **Architecture**: We showed the clean 3-layer architecture with separated concerns: 
   Controllers, Services, and Repositories.

2. **Borrow & Return Flows**: Members can safely borrow and return books with 
   automatic inventory management.

3. **Validation**: The system validates both data format and business rules, 
   returning clear error messages.

4. **Error Handling**: Centralized exception handling ensures consistent, 
   user-friendly error responses.

5. **Concurrency**: Even with simultaneous requests for the last copy, 
   the system maintains data integrity.

6. **Caching**: Strategic in-memory caching provides a 40x performance improvement 
   for read-heavy workloads.

This demonstrates how enterprise-level systems balance functionality, reliability, 
performance, and maintainability. Thank you for watching!"

---

## Post-Recording Checklist

- [ ] Review entire recording for clarity and audio quality
- [ ] Trim any long pauses or mistakes
- [ ] Add title/end cards
- [ ] Add annotations or graphics if desired
- [ ] Export in high quality (1080p minimum)
- [ ] Upload to shared platform (YouTube, Teams, etc.)
- [ ] Get team review and feedback

---

## Recording Tips

1. **Clear Voice:** Speak slowly and clearly, explaining each step
2. **Pacing:** Don't rush - give viewers time to read the code/responses
3. **Zoom Level:** Make sure code and responses are readable on smaller screens
4. **Mouse Movement:** Move deliberately to draw attention to relevant areas
5. **Response Time:** Allow API responses to appear naturally on screen
6. **Narration:** Explain the "why" behind each feature, not just the "what"
7. **Troubleshooting:** Record without mistakes if possible, or edit them out

---

**Total Target Duration:** 10-15 minutes  
**Estimated Recording Time:** 20-25 minutes (with retakes)  
**Editing Time:** 10-15 minutes
