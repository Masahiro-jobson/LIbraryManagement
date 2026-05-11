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
            new LibraryMainApp.Models.Book { Title = "The Great Gatsby",        Author = "F. Scott Fitzgerald", PublishedYear = 1925, Genre = "Fiction",     AvailabilityStatus = "Available" },
            new LibraryMainApp.Models.Book { Title = "Clean Code",              Author = "Robert C. Martin",    PublishedYear = 2008, Genre = "Technology", AvailabilityStatus = "Available" },
            new LibraryMainApp.Models.Book { Title = "A Brief History of Time", Author = "Stephen Hawking",     PublishedYear = 1988, Genre = "Science",    AvailabilityStatus = "Available" },
            new LibraryMainApp.Models.Book { Title = "Sapiens",                 Author = "Yuval Noah Harari",   PublishedYear = 2011, Genre = "History",    AvailabilityStatus = "Available" },
            new LibraryMainApp.Models.Book { Title = "C# Programming",          Author = "John Sharp",          PublishedYear = 2020, Genre = "Technology", AvailabilityStatus = "Available" },
            new LibraryMainApp.Models.Book { Title = "The Hobbit",              Author = "J.R.R. Tolkien",      PublishedYear = 1937, Genre = "Fiction",    AvailabilityStatus = "Available" },
            new LibraryMainApp.Models.Book { Title = "Design Patterns",         Author = "Gang of Four",        PublishedYear = 1994, Genre = "Technology", AvailabilityStatus = "Available" },
            new LibraryMainApp.Models.Book { Title = "1984",                    Author = "George Orwell",       PublishedYear = 1949, Genre = "Fiction",    AvailabilityStatus = "Available" }
        );
        db.SaveChanges();
    }
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
