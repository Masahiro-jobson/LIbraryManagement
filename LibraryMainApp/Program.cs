using Microsoft.EntityFrameworkCore;
using LibraryMainApp.Data;

var builder = WebApplication.CreateBuilder(args);


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));


builder.Services.AddControllersWithViews();

builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Give initial data in case the table is emptyw
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    if (!db.Staffs.Any())
    {
        db.Staffs.Add(new LibraryMainApp.Models.Staff
        {
            FirstName = "Admin",
            LastName = "Librarian",
            Email = "admin@library.com",
            PasswordHash = "demo",
            Role = "Admin",
            HiredDate = new DateTime(2024, 1, 1)
        });
        db.SaveChanges();
    }

    if (!db.Members.Any())
    {
        db.Members.Add(new LibraryMainApp.Models.Member
        {
            FirstName = "Demo",
            LastName = "User",
            Email = "demo@library.com",
            PasswordHash = "demo",
            Address = "123 Library Street",
            PhoneNumber = "0400000000",
            RegistrationDate = new DateTime(2024, 1, 1)
        });
        db.SaveChanges();
    }

    if (!db.Books.Any())
    {
        db.Books.AddRange(
            new LibraryMainApp.Models.Book { Title = "The Great Gatsby",        Author = "F. Scott Fitzgerald", PublishedYear = 1925, Genre = "Fiction",     AvailabilityStatus = "Available", CoverImagePath = "https://covers.openlibrary.org/b/id/8231990-M.jpg" },
            new LibraryMainApp.Models.Book { Title = "Clean Code",              Author = "Robert C. Martin",    PublishedYear = 2008, Genre = "Technology",  AvailabilityStatus = "Available", CoverImagePath = "https://covers.openlibrary.org/b/id/8508894-M.jpg" },
            new LibraryMainApp.Models.Book { Title = "A Brief History of Time", Author = "Stephen Hawking",     PublishedYear = 1988, Genre = "Science",      AvailabilityStatus = "Available", CoverImagePath = "https://covers.openlibrary.org/b/id/8739161-M.jpg" },
            new LibraryMainApp.Models.Book { Title = "Sapiens",                 Author = "Yuval Noah Harari",   PublishedYear = 2011, Genre = "History",      AvailabilityStatus = "Available", CoverImagePath = "https://covers.openlibrary.org/b/id/8739415-M.jpg" },
            new LibraryMainApp.Models.Book { Title = "C# Programming",          Author = "John Sharp",          PublishedYear = 2020, Genre = "Technology",  AvailabilityStatus = "Available", CoverImagePath = "https://covers.openlibrary.org/b/id/8406786-M.jpg" },
            new LibraryMainApp.Models.Book { Title = "The Hobbit",              Author = "J.R.R. Tolkien",      PublishedYear = 1937, Genre = "Fiction",      AvailabilityStatus = "Available", CoverImagePath = "https://covers.openlibrary.org/b/id/8406786-M.jpg" },
            new LibraryMainApp.Models.Book { Title = "Design Patterns",         Author = "Gang of Four",        PublishedYear = 1994, Genre = "Technology",  AvailabilityStatus = "Available", CoverImagePath = "https://covers.openlibrary.org/b/id/8739382-M.jpg" },
            new LibraryMainApp.Models.Book { Title = "1984",                    Author = "George Orwell",       PublishedYear = 1949, Genre = "Fiction",      AvailabilityStatus = "Available", CoverImagePath = "https://covers.openlibrary.org/b/id/7222246-M.jpg" }
        );
        db.SaveChanges();
    }

    // Update cover images for existing books that have no cover
    var coverMap = new Dictionary<string, string>
    {
        { "The Great Gatsby",        "https://covers.openlibrary.org/b/id/8231990-M.jpg" },
        { "Clean Code",              "https://covers.openlibrary.org/b/id/8508894-M.jpg" },
        { "A Brief History of Time", "https://covers.openlibrary.org/b/id/8739161-M.jpg" },
        { "Sapiens",                 "https://covers.openlibrary.org/b/id/8739415-M.jpg" },
        { "C# Programming",          "https://covers.openlibrary.org/b/id/8406786-M.jpg" },
        { "The Hobbit",              "https://covers.openlibrary.org/b/id/8406786-M.jpg" },
        { "Design Patterns",         "https://covers.openlibrary.org/b/id/8739382-M.jpg" },
        { "1984",                    "https://covers.openlibrary.org/b/id/7222246-M.jpg" }
    };

    var booksWithoutCover = db.Books.Where(b => b.CoverImagePath == null).ToList();
    foreach (var book in booksWithoutCover)
    {
        if (coverMap.ContainsKey(book.Title))
        {
            book.CoverImagePath = coverMap[book.Title];
        }
    }
    if (booksWithoutCover.Any()) db.SaveChanges();
}



if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseStaticFiles();

app.UseRouting();


app.UseSession();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
