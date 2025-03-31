using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using BookManager.Data;
using BookManager.Dtos;
using BookManager.Entities;

namespace BookManager.Endpoints
{
    public static class BookEndpoint
    {
        public static RouteGroupBuilder UseBookEndpoint(this WebApplication app)
        {
            RouteGroupBuilder group = app.MapGroup("books");

            group.MapGet("/{isbn}", (BookManagerContext dbContext, string? isbn) =>
            {
                if (isbn == null)
                {
                    return Results.BadRequest("isbn is required");
                }

                Book? book = dbContext.Find<Book>(isbn);

                if (book == null)
                {
                    return Results.BadRequest(string.Format("book isbn {0} doesn't exist", isbn));
                }

                return Results.Ok(book.ToBookDto());
            }).WithName("GetBook");

            group.MapGet("/search/{key}", (BookManagerContext dbContext, string? key) =>
            {
                if (key == null)
                {
                    return Results.BadRequest("search is required");
                }

                if (key.Length <= 2)
                {
                    return Results.BadRequest("need at least 2 characters");
                }

                var books = dbContext.Books
                    .Where(b => b.SearchVector!.Matches(key))
                    .OrderBy(b => b.ISBN)
                    .Take(10)
                    .Select(b => b.ToBookDto())
                    .ToList();

                return Results.Ok(books);
            }).WithName("SearchBook");

            group.MapGet("/list", async (
                BookManagerContext dbContext,
                [FromQuery] string page = "1",
                [FromQuery] string limit = "10"
            ) =>
            {
                bool successPage = int.TryParse(page, out int pageInt);
                if (!successPage)
                {
                    return Results.BadRequest("page must be integer");
                }

                bool successLimit = int.TryParse(limit, out int limitInt);
                if (!successLimit)
                {
                    return Results.BadRequest("limit must be integer");
                }

                IQueryable<BookDto> result = dbContext.Books
                    .OrderBy(b => b.ISBN)
                    .Skip(pageInt * limitInt)
                    .Take(limitInt)
                    .Select(b => b.ToBookDto());

                return Results.Ok(await result.ToArrayAsync());
            }).WithName("ListBook");


            return group;
        }
    }
}