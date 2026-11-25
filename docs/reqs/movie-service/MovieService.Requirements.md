# MovieService – Software Requirements (SWE.1)

Source: `CinemaApp.Services.Core/MovieService.cs`
Dependencies: `IMovieRepository` (mockable), `ILogger<MovieService>` (mockable), `BasicModelValidator` (created internally, not injectable).
Response status enum: `MovieServiceResponseStatus` { Success, InvalidRequest, NotFound, GeneralError }.

Notation: REQ-MS-<function>-<nn>. "Test hint" gives the suggested unit test condition.

---

## 1. GetAllMoviesAsync() (no parameters)

- REQ-MS-GETALL-01: Shall read movies from `movieRepository.AllAsNoTracking()`.
  Test hint: verify repository call; no tracking.
- REQ-MS-GETALL-02: Shall return movies ordered ascending by `Title`, then ascending by `Genre`.
- REQ-MS-GETALL-03: Shall map each movie to `GetAllMoviesResponseModel` with fields Id, Title, Genre, Director, ReleaseDate, ImageUrl only.
- REQ-MS-GETALL-04: Shall return an empty collection when no movies exist (never null).
- REQ-MS-GETALL-05: Shall not apply paging or filtering.

## 2. GetAllMoviesAsync(GetAllMoviesRequestData)

- REQ-MS-GETALLF-01: If `requestData` fails `modelValidator.IsValid`, shall return an empty list and shall not query the repository.
- REQ-MS-GETALLF-02: If `SearchQuery` is not null/whitespace, shall filter movies where `Title` matches LIKE `%SearchQuery%`.
- REQ-MS-GETALLF-03: If `GenreQuery` is not null/whitespace, shall filter movies where `Genre` matches LIKE `%GenreQuery%`.
- REQ-MS-GETALLF-04: If `ReleaseYearQuery` has a value, shall filter movies whose `ReleaseDate.Year` equals it.
- REQ-MS-GETALLF-05: Null/empty/whitespace filters shall not restrict results.
- REQ-MS-GETALLF-06: Filters shall be combinable (logical AND).
- REQ-MS-GETALLF-07: Results shall be ordered by `Title` then `Genre` ascending.
- REQ-MS-GETALLF-08: Shall apply `Skip(SkipEntries)` then `Take(TakeEntries)` after ordering.
- REQ-MS-GETALLF-09: Shall map to `GetAllMoviesResponseModel` (Id, Title, Genre, Director, ReleaseDate, ImageUrl).
- REQ-MS-GETALLF-10: Shall return an empty collection when nothing matches.
- Note: EF.Functions.Like requires a relational provider; the in-memory provider does not support it (use SQLite/SQL Server test DB or a repository returning a testable provider).

## 3. CreateMovieAsync(CreateMovieRequestData)

- REQ-MS-CREATE-01: If the request is invalid, shall return Status=`InvalidRequest`, with `ErrorMessages` equal to the validation error messages, and shall not call `AddAsync` or `SaveChangesAsync`.
- REQ-MS-CREATE-02: If valid, shall create a `Movie` with Title, Genre, ReleaseDate, Duration, Director, Description, ImageUrl copied from the request.
- REQ-MS-CREATE-03: Shall call `AddAsync(movie)` and then `SaveChangesAsync()` once each.
- REQ-MS-CREATE-04: On success shall return Status=`Success`.
- REQ-MS-CREATE-05: If `AddAsync`/`SaveChangesAsync` throws `DbUpdateException`, shall return Status=`GeneralError` with `ErrorMessages` containing exactly the exception message.
- REQ-MS-CREATE-06: Exceptions other than `DbUpdateException` are not handled (shall propagate).

## 4. GetMovieBasicDataByIdAsync(Guid id)

- REQ-MS-BASIC-01: Shall look up the movie via `FindByIdAsync(id)`.
- REQ-MS-BASIC-02: If not found, shall return null.
- REQ-MS-BASIC-03: If found, shall return `MovieBasicDataResponseModel` with Id, Title, Genre, ImageUrl.

## 5. GetMovieExtendedDataByIdAsync(Guid id)

- REQ-MS-EXT-01: Shall look up the movie via `FindByIdAsync(id)`.
- REQ-MS-EXT-02: If not found, shall return null.
- REQ-MS-EXT-03: If found, shall return `MovieExtendedDataResponseModel` with Id, Title, Genre, ReleaseDate, Duration, Director, Description, ImageUrl.

## 6. EditMovieAsync(EditMovieRequestData)

- REQ-MS-EDIT-01: Shall look up the movie via `FindByIdAsync(requestData.Id)`.
- REQ-MS-EDIT-02: If the movie is not found and the request is valid, shall return Status=`NotFound`, shall not call `SaveChangesAsync`.
- REQ-MS-EDIT-03: If the request is invalid, Status shall be `InvalidRequest` and `ErrorMessages` shall be the validation messages.
- REQ-MS-EDIT-04 (observed behavior): If the movie exists, the update is applied even when the request is invalid (validation result does not prevent the update); on a successful save Status is overwritten to `Success`. If the movie is not found and the request is invalid, Status=`InvalidRequest`.
- REQ-MS-EDIT-05: If the movie exists, shall set Title, Description, ImageUrl, Director, Genre, ReleaseDate, Duration from the request and call `SaveChangesAsync()` once.
- REQ-MS-EDIT-06: On successful save shall return Status=`Success`.
- REQ-MS-EDIT-07: If `SaveChangesAsync` throws any exception, shall return Status=`GeneralError` with `ErrorMessages` containing exactly the exception message.
- REQ-MS-EDIT-08: The initial default response is Status=`GeneralError` with empty `ErrorMessages`.

## 7. SoftDeleteMovieAsync(Guid id)

- REQ-MS-SDEL-01: If the movie is not found, shall return Status=`NotFound` and not call `SoftDelete`/`SaveChangesAsync`.
- REQ-MS-SDEL-02: If the movie is already deleted (`IsDeleted == true`), shall log a warning, return Status=`Success`, and not call `SoftDelete` or `SaveChangesAsync`.
- REQ-MS-SDEL-03: If found and not deleted, shall call `SoftDelete(movie)` then `SaveChangesAsync()` and return Status=`Success`.
- REQ-MS-SDEL-04: If `SoftDelete`/`SaveChangesAsync` throws, shall return Status=`GeneralError` with `ErrorMessages` containing exactly the exception message.

## 8. HardDeleteMovieAsync(Guid id)

- REQ-MS-HDEL-01: If the movie is not found, shall return Status=`NotFound` and not call `Remove`/`SaveChangesAsync`.
- REQ-MS-HDEL-02: If the movie is soft-deleted, shall log a warning and still proceed with removal.
- REQ-MS-HDEL-03: Shall call `Remove(movie)` then `SaveChangesAsync()` and return Status=`Success`.
- REQ-MS-HDEL-04: If `Remove`/`SaveChangesAsync` throws, shall return Status=`GeneralError` with `ErrorMessages` containing exactly the exception message.
- REQ-MS-HDEL-05: No warning shall be logged for non-deleted movies.

## 9. GetMovieCountAsync()

- REQ-MS-COUNT-01: Shall return the total number of movies from `AllAsNoTracking()`.
- REQ-MS-COUNT-02: Shall return 0 when there are no movies.

## 10. GetMovieCountAsync(string? searchQuery, string? genreQuery, int? releaseYearQuery)

- REQ-MS-COUNTF-01: Shall apply Title LIKE `%searchQuery%` when searchQuery is not null/whitespace.
- REQ-MS-COUNTF-02: Shall apply Genre LIKE `%genreQuery%` when genreQuery is not null/whitespace.
- REQ-MS-COUNTF-03: Shall apply `ReleaseDate.Year == releaseYearQuery` when it has a value.
- REQ-MS-COUNTF-04: Filters are combined with AND; with all null filters it equals the total count.
- REQ-MS-COUNTF-05: Shall return 0 when nothing matches.
- REQ-MS-COUNTF-06: Shall not validate input and shall not apply paging.

---

## Non-functional / Testability Notes

- NFR-MS-01: All methods are asynchronous and return `Task`.
- NFR-MS-02: Constructor shall store the injected repository and logger; the validator is created internally.
- NFR-MS-03: Read methods shall use `AllAsNoTracking()` (no change tracking).
- NFR-MS-04: Logging occurs only in SoftDelete (already deleted) and HardDelete (soft-deleted) scenarios, at Warning level, with message containing the entity Id.
- NFR-MS-05: Repository mocks must return `IQueryable<Movie>` supporting async (e.g. `MockQueryable`) or use a relational test DB.
