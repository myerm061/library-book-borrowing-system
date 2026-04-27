# Library Book Borrowing System - Testing Guide

**Test Date:** To be executed after Issues 2, 3, 4 merge  
**Environment:** Local development server on `https://localhost:5001`  
**Tools Required:** Postman, Swagger UI, or equivalent HTTP client

---

## Setup Instructions

### Prerequisites
1. Merge Issues 2, 3, and 4 into develop branch
2. Start the application: `dotnet run --project LibraryBorrowingSystem`
3. Verify Swagger is accessible: `https://localhost:5001/swagger/index.html`
4. Import Postman collection or use Swagger UI directly

### Database Initialization
The application uses in-memory database that resets on each run. Before testing:
1. Create seed data using POST endpoints
2. Or use Swagger UI to populate database

---

## Test 1: All Endpoints Basic Functionality

### Test 1.1: Books Endpoints

**1.1.1 GET /api/books (List All Books)**

```
Request: GET https://localhost:5001/api/books
Expected Status: 200 OK
Expected Response:
[
  {
    "id": 1,
    "title": "Clean Code",
    "author": "Robert C. Martin",
    "isbn": "978-0132350884",
    "totalCopies": 5,
    "availableCopies": 5
  }
]
Test Result: ☐ PASS ☐ FAIL
Notes: _____________________
```

**1.1.2 GET /api/books/{id} (Get Single Book)**

```
Request: GET https://localhost:5001/api/books/1
Expected Status: 200 OK
Expected Response:
{
  "id": 1,
  "title": "Clean Code",
  "author": "Robert C. Martin",
  "isbn": "978-0132350884",
  "totalCopies": 5,
  "availableCopies": 5
}
Test Result: ☐ PASS ☐ FAIL
Notes: _____________________
```

**1.1.3 POST /api/books (Create Book)**

```
Request: POST https://localhost:5001/api/books
Body:
{
  "title": "Design Patterns",
  "author": "Gang of Four",
  "isbn": "978-0201633610",
  "totalCopies": 3
}

Expected Status: 201 Created
Expected Response:
{
  "id": 2,
  "title": "Design Patterns",
  "author": "Gang of Four",
  "isbn": "978-0201633610",
  "totalCopies": 3,
  "availableCopies": 3
}
Expected Header: Location: /api/books/2
Test Result: ☐ PASS ☐ FAIL
Notes: _____________________
```

**1.1.4 PUT /api/books/{id} (Update Book)**

```
Request: PUT https://localhost:5001/api/books/1
Body:
{
  "title": "Clean Code (2nd Edition)",
  "author": "Robert C. Martin",
  "isbn": "978-0132350884",
  "totalCopies": 10
}

Expected Status: 200 OK
Expected Response:
{
  "id": 1,
  "title": "Clean Code (2nd Edition)",
  "author": "Robert C. Martin",
  "isbn": "978-0132350884",
  "totalCopies": 10,
  "availableCopies": 10
}
Test Result: ☐ PASS ☐ FAIL
Notes: _____________________
```

**1.1.5 DELETE /api/books/{id} (Delete Book)**

```
Request: DELETE https://localhost:5001/api/books/2
Expected Status: 204 No Content
Expected Response: (empty body)
Test Result: ☐ PASS ☐ FAIL
Notes: _____________________
```

### Test 1.2: Members Endpoints

**1.2.1 GET /api/members (List All Members)**
```
Request: GET https://localhost:5001/api/members
Expected Status: 200 OK
Expected Response: [] or [{ member objects }]
Test Result: ☐ PASS ☐ FAIL
Notes: _____________________
```

**1.2.2 GET /api/members/{id} (Get Single Member)**
```
Request: GET https://localhost:5001/api/members/1
Expected Status: 200 OK (or 404 if not created)
Test Result: ☐ PASS ☐ FAIL
Notes: _____________________
```

**1.2.3 POST /api/members (Create Member)**
```
Request: POST https://localhost:5001/api/members
Body:
{
  "fullName": "John Doe",
  "email": "john@example.com"
}

Expected Status: 201 Created
Expected Response:
{
  "id": 1,
  "fullName": "John Doe",
  "email": "john@example.com",
  "membershipDate": "2026-04-26T..."
}
Expected Header: Location: /api/members/1
Test Result: ☐ PASS ☐ FAIL
Notes: _____________________
```

**1.2.4 PUT /api/members/{id} (Update Member)**
```
Request: PUT https://localhost:5001/api/members/1
Body:
{
  "fullName": "John Doe Smith",
  "email": "john.smith@example.com"
}

Expected Status: 200 OK
Test Result: ☐ PASS ☐ FAIL
Notes: _____________________
```

**1.2.5 DELETE /api/members/{id} (Delete Member)**
```
Request: DELETE https://localhost:5001/api/members/1
Expected Status: 204 No Content
Test Result: ☐ PASS ☐ FAIL
Notes: _____________________
```

### Test 1.3: Borrowing Endpoints

**1.3.1 POST /api/borrow (Borrow Book)**
```
Prerequisite: Create Book (ID: 1) and Member (ID: 1)

Request: POST https://localhost:5001/api/borrow
Body:
{
  "bookId": 1,
  "memberId": 1
}

Expected Status: 201 Created
Expected Response:
{
  "id": 1,
  "bookId": 1,
  "memberId": 1,
  "borrowDate": "2026-04-26T...",
  "returnDate": null,
  "status": "Borrowed"
}
Test Result: ☐ PASS ☐ FAIL
Notes: _____________________
```

**1.3.2 GET /api/borrow (List All Borrow Records)**
```
Request: GET https://localhost:5001/api/borrow
Expected Status: 200 OK
Expected Response: [{ borrow record objects }]
Test Result: ☐ PASS ☐ FAIL
Notes: _____________________
```

**1.3.3 GET /api/borrow/member/{memberId} (Get Member Borrow History)**
```
Request: GET https://localhost:5001/api/borrow/member/1
Expected Status: 200 OK
Expected Response: [{ borrow records for member 1 }]
Test Result: ☐ PASS ☐ FAIL
Notes: _____________________
```

**1.3.4 POST /api/borrow/return (Return Book)**
```
Prerequisite: Active borrow record (ID: 1)

Request: POST https://localhost:5001/api/borrow/return
Body:
{
  "borrowRecordId": 1
}

Expected Status: 200 OK
Expected Response:
{
  "id": 1,
  "bookId": 1,
  "memberId": 1,
  "borrowDate": "2026-04-26T...",
  "returnDate": "2026-04-26T...",
  "status": "Returned"
}
Test Result: ☐ PASS ☐ FAIL
Notes: _____________________
```

---

## Test 2: Validation Error Response Format

### Test 2.1: Missing Required Fields

**2.1.1 Create Book with Missing Title**
```
Request: POST https://localhost:5001/api/books
Body:
{
  "author": "John Smith",
  "isbn": "978-0132350884",
  "totalCopies": 5
}

Expected Status: 400 Bad Request
Expected Response Format:
{
  "error": "The Title field is required."
}
or
{
  "errors": {
    "Title": ["The Title field is required."]
  }
}

Test Result: ☐ PASS ☐ FAIL
Notes: _____________________
```

**2.1.2 Create Member with Missing Email**
```
Request: POST https://localhost:5001/api/members
Body:
{
  "fullName": "Jane Doe"
}

Expected Status: 400 Bad Request
Expected Response: { "error": "..." } with email requirement message
Test Result: ☐ PASS ☐ FAIL
Notes: _____________________
```

### Test 2.2: Invalid Data Format

**2.2.1 Invalid ISBN Format**
```
Request: POST https://localhost:5001/api/books
Body:
{
  "title": "Clean Code",
  "author": "Robert Martin",
  "isbn": "INVALID",
  "totalCopies": 5
}

Expected Status: 400 Bad Request
Expected Response: { "error": "ISBN format is invalid" }
Test Result: ☐ PASS ☐ FAIL
Notes: _____________________
```

**2.2.2 Invalid Email Format**
```
Request: POST https://localhost:5001/api/members
Body:
{
  "fullName": "John Doe",
  "email": "not-an-email"
}

Expected Status: 400 Bad Request
Expected Response: { "error": "Email format is invalid" }
Test Result: ☐ PASS ☐ FAIL
Notes: _____________________
```

### Test 2.3: Business Logic Validation

**2.3.1 Borrow When No Copies Available**
```
Prerequisite: 
- Create Book (ID: 1) with totalCopies: 1
- Create 2 Members
- Member 1 borrows Book 1 (now availableCopies = 0)

Request: POST https://localhost:5001/api/borrow
Body:
{
  "bookId": 1,
  "memberId": 2
}

Expected Status: 400 Bad Request
Expected Response:
{
  "error": "Book is not available. Only 0 copies remaining."
}
Test Result: ☐ PASS ☐ FAIL
Notes: _____________________
```

**2.3.2 Borrow Non-Existent Book**
```
Request: POST https://localhost:5001/api/borrow
Body:
{
  "bookId": 99999,
  "memberId": 1
}

Expected Status: 400 Bad Request
Expected Response:
{
  "error": "Book not found."
}
Test Result: ☐ PASS ☐ FAIL
Notes: _____________________
```

---

## Test 3: 404 Not Found Responses

### Test 3.1: GET Non-Existent Resources

**3.1.1 GET Non-Existent Book**
```
Request: GET https://localhost:5001/api/books/99999
Expected Status: 404 Not Found
Expected Response:
{
  "error": "Book not found."
}
Test Result: ☐ PASS ☐ FAIL
Notes: _____________________
```

**3.1.2 GET Non-Existent Member**
```
Request: GET https://localhost:5001/api/members/99999
Expected Status: 404 Not Found
Expected Response:
{
  "error": "Member not found."
}
Test Result: ☐ PASS ☐ FAIL
Notes: _____________________
```

### Test 3.2: Update/Delete Non-Existent Resources

**3.2.1 PUT Non-Existent Book**
```
Request: PUT https://localhost:5001/api/books/99999
Body: { "title": "...", "author": "...", "isbn": "...", "totalCopies": 5 }
Expected Status: 404 Not Found
Expected Response:
{
  "error": "Book not found."
}
Test Result: ☐ PASS ☐ FAIL
Notes: _____________________
```

**3.2.2 DELETE Non-Existent Book**
```
Request: DELETE https://localhost:5001/api/books/99999
Expected Status: 404 Not Found
Expected Response:
{
  "error": "Book not found."
}
Test Result: ☐ PASS ☐ FAIL
Notes: _____________________
```

---

## Test 4: Concurrency Scenario (Two Simultaneous Borrow Requests)

### Test Setup
1. Create Book: ID=1, Title="Concurrent Test Book", TotalCopies=1, AvailableCopies=1
2. Create Member 1: ID=1
3. Create Member 2: ID=2

### Test 4.1: Simultaneous Last-Copy Borrow

**Scenario:** Two members simultaneously request to borrow the last copy of a book.

**Expected Behavior:**
- Request A (Member 1): Succeeds with 201 Created
- Request B (Member 2): Fails with 400 Bad Request (book not available)

**Test Execution:**

```
Step 1: Use Postman to send two simultaneous requests
  - Request A: Borrow BookId=1, MemberId=1
  - Request B: Borrow BookId=1, MemberId=2

Step 2: Send both requests at exact same time (within 100ms)
  To do this in Postman:
  a) Create two identical requests
  b) Use "Send" on both tabs simultaneously
  OR
  c) Use Postman's "Runner" to send collection with 0ms delay

Step 3: Observe Responses

Request A Response (Expected):
Status: 201 Created
Body:
{
  "id": 1,
  "bookId": 1,
  "memberId": 1,
  "borrowDate": "2026-04-26T...",
  "returnDate": null,
  "status": "Borrowed"
}

Request B Response (Expected):
Status: 400 Bad Request
Body:
{
  "error": "Book is not available. Only 0 copies remaining."
}

Step 4: Verify Book State
Request: GET https://localhost:5001/api/books/1
Expected Response:
{
  "id": 1,
  "title": "Concurrent Test Book",
  "totalCopies": 1,
  "availableCopies": 0  ← Should be 0, not -1
}

Test Result: ☐ PASS ☐ FAIL
Notes: _____________________
```

### Test 4.2: Concurrency with Multiple Copies

**Scenario:** Three members simultaneously request books when 2 copies available.

```
Setup:
- Book: TotalCopies=2, AvailableCopies=2
- Members: 1, 2, 3

Requests (send simultaneously):
- Request A: Borrow BookId=1, MemberId=1
- Request B: Borrow BookId=1, MemberId=2
- Request C: Borrow BookId=1, MemberId=3

Expected Results:
- Two requests succeed (201 Created) - any combination acceptable
- One request fails (400 Bad Request)
- Final availableCopies = 0

Test Result: ☐ PASS ☐ FAIL
Notes: _____________________
```

---

## Test 5: Caching Behavior (First Request Hits DB, Subsequent Requests Return Cache)

### Test Setup
1. Restart application (clears cache)
2. Create Book: ID=1, Title="Caching Test Book", Author="Cache Author"
3. Add performance monitoring (browser DevTools, Postman timings, or application logs)

### Test 5.1: Cache Behavior on GET

**Test Execution:**

```
Request 1: GET https://localhost:5001/api/books/1 (FIRST REQUEST - Cache Miss)
Expected:
- Response Time: ~50-150ms (database hit)
- Response Code: 200 OK
- Data returned from database
- Entry stored in memory cache with 5-minute TTL

Observed Time: _______ ms
Test Result: ☐ PASS ☐ FAIL

---

Request 2: GET https://localhost:5001/api/books/1 (Cache Hit - within 5 min)
Expected:
- Response Time: ~1-10ms (memory hit, no DB access)
- Response Code: 200 OK
- Exact same data as Request 1

Observed Time: _______ ms
Performance Improvement: ~90% faster than first request
Test Result: ☐ PASS ☐ FAIL

---

Request 3: Wait 5+ minutes, then GET https://localhost:5001/api/books/1
Expected:
- Response Time: ~50-150ms (cache expired, database hit again)
- Cache re-populated

Observed Time: _______ ms
Test Result: ☐ PASS ☐ FAIL
```

### Test 5.2: Cache Invalidation on Write

```
Initial State: GET /api/books/1 (cached, 100ms response)

Step 1: Modify Book
Request: PUT https://localhost:5001/api/books/1
Body: { "title": "Updated Title", ... }
Response: 200 OK
Cache Action: "book_1" and "books_all" invalidated

Step 2: Retrieve Updated Book (Cache Miss)
Request: GET https://localhost:5001/api/books/1
Expected:
- Response Time: ~50-150ms (database hit - cache was cleared)
- Response contains updated title: "Updated Title"

Observed Time: _______ ms
Test Result: ☐ PASS ☐ FAIL

Step 3: Retrieve Again (Cache Hit)
Request: GET https://localhost:5001/api/books/1
Expected:
- Response Time: ~1-10ms (cache hit with new data)

Observed Time: _______ ms
Test Result: ☐ PASS ☐ FAIL
```

### Test 5.3: List Cache Behavior

```
Request 1: GET https://localhost:5001/api/books (List All - Cache Miss)
Expected Time: ~50-150ms (database scan + serialize all results)
Observed Time: _______ ms

Request 2: GET https://localhost:5001/api/books (Cache Hit)
Expected Time: ~1-10ms
Observed Time: _______ ms

After creating new book:
Request 3: POST https://localhost:5001/api/books
Body: { "title": "New Book", ... }
Response: 201 Created
Cache Action: "books_all" invalidated

Request 4: GET https://localhost:5001/api/books (Cache Miss - needs rebuild)
Expected Time: ~50-150ms (includes new book)
Observed Time: _______ ms

Test Result: ☐ PASS ☐ FAIL
Notes: _____________________
```

---

## Test Summary

### Test Results Summary
| Test # | Name | Result | Notes |
|--------|------|--------|-------|
| 1.1 | Books Endpoints | ☐ | |
| 1.2 | Members Endpoints | ☐ | |
| 1.3 | Borrowing Endpoints | ☐ | |
| 2 | Validation Errors | ☐ | |
| 3 | 404 Responses | ☐ | |
| 4 | Concurrency | ☐ | |
| 5 | Caching | ☐ | |

### Overall Test Status
- **Total Tests:** 7 suites + 30+ individual test cases
- **Tests Passed:** ☐ 
- **Tests Failed:** ☐
- **Date Tested:** ___________
- **Tested By:** ___________

---

**Ready for Testing:** Upon merge of Issues 2, 3, 4 into develop branch
