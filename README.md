# .NET Project

Hello. This README file contains the details required for the tasks given. I will describe the architecture of the backend part, what it does and how it works.


# Setup

For the setup everything should be ok. Just to make sure just build the project again and then launch it. Project uses .NET 9.0. All the endpoints are HTTP and the project runs on localhost port 5143. If the port is already in use or for some reason needs to be changed, just replace the values in the launchSettings.json file from the properties folder.

# Additional Feature
Since the main user of the application is a library administrator let's enhance the data and statistics that he can gather from the book lendings. I added an additional field for the book model called categories. The Categories field represents a predefined list of categories that describe the characteristics of a book. With this information we can get relevant information such as: most wanted categories and filter the books by the categories they have. Since having the lendings table we can now mark which books are to be returned, most frequent borrowed books, which user borrowed the most books and limit the number of simultaneously borrowings of the same book by a certain user to 1.

# Architecture

The architecture chosen is a standard multilayer architecture containing the: Controller, Service, Repository, Model layers. As for the storage of the data I chose to store it in .JSON files in the Data package. Each layer has its own package.
<img src="https://github.com/user-attachments/assets/881eb6e5-dd15-4d15-8edd-8862897a599e" width="400"/>

<img src="https://github.com/user-attachments/assets/e8c8da99-1bf6-43b2-ad1b-b140c4edd2ff" width="400"/>


## Database

The database contains two tables: books and lendings. The lending is an additional entity created by me to store the records of the books that were borrowed and returned, at what time and date, and by which user.  
<img src="https://github.com/user-attachments/assets/060d8a52-4e45-46a6-a06e-a7e2cfe74790" width="400"/>

## Endpoints
### 📖 Book Management

#### `GET /api/books`
- **Description:** Get all books
- **Returns:** `List<Book>`

#### `GET /api/books/{id}`
- **Description:** Get a book by its ID
- **Returns:** `Book` or `404 NotFound`

#### `POST /api/books`
- **Description:** Add a new book
- **Body:** `Book`
- **Returns:** `201 Created` with the created book

#### `PUT /api/books/{id}`
- **Description:** Update an existing book by ID
- **Body:** `Book`
- **Returns:** `200 OK` with updated book or `404 NotFound`

#### `DELETE /api/books/{id}`
- **Description:** Delete a book by ID
- **Returns:** `204 No Content`

---

### 📗 Lending Actions

#### `POST /api/books/{bookId}/{userId}/borrow`
- **Description:** Borrow a book for a user
- **Returns:** `200 OK` on success, `400 Bad Request` if already borrowed or unavailable

#### `POST /api/books/{bookId}/{userId}/return`
- **Description:** Return a borrowed book
- **Returns:** `200 OK` on success, `400 Bad Request` if invalid return

---

### 🔍 Filtering

#### `GET /api/books/{author}/findAuthor`
- **Description:** Get books by author name
- **Returns:** `List<Book>`

#### `GET /api/books/{title}/findTitle`
- **Description:** Get books by title
- **Returns:** `List<Book>`

#### `GET /api/books/{category}/findCategory`
- **Description:** Get books by category (enum)
- **Returns:** `List<Book>` or `400 Bad Request` if category is invalid

---

### 🏆 Leaderboards

#### `GET /api/books/{date}/bookLeaderboard`
- **Description:** Get most borrowed books since a given date
- **Returns:** `List<Book>`, sorted by borrow count

#### `GET /userLeaderboard`
- **Description:** Get user IDs of the most active borrowers since a given date
- **Body:** `DateTime`
- **Returns:** `List<long>` (user IDs)

#### `GET /api/books/categoryLeaderboard`
- **Description:** Get most borrowed categories since a given date
- **Body:** `DateTime`
- **Returns:** `List<string>` (e.g. `"Fantasy (3 times)"`)

---

### 🚫 Missing Books

#### `GET /api/books/missing`
- **Description:** Get books that are currently borrowed and not returned
- **Returns:** `List<object>` with `Title`, `BookId`, `MissingQuantity`

