using LoncotesLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;
using LoncotesLibrary.Models.DTOs;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Mvc;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
builder.Services.AddNpgsql<LoncotesLibraryDbContext>(builder.Configuration["LoncotesLibraryDbConnectionString"]);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/api/Materials", (LoncotesLibraryDbContext db, int? GenreId, int? MaterialTypeId) =>
{
    var query = db.Materials.AsQueryable();
    if (GenreId.HasValue && MaterialTypeId.HasValue)
    {

        query = query.Where(m => m.GenreId == GenreId).Where(m => m.MaterialTypeId == MaterialTypeId);
    }
    else if (GenreId.HasValue && !MaterialTypeId.HasValue)
    {
        query = query.Where(m => m.GenreId == GenreId);
    }
    else if (!GenreId.HasValue && MaterialTypeId.HasValue)
    {
        query = query.Where(m => m.MaterialTypeId == MaterialTypeId);
    }

    return query
    .Where(m => m.OutOfCirculationSince == null)
    .Include(m => m.MaterialType)
    .Include(m => m.Genre)
    .OrderBy(m => m.Id)
    .Select(m => new MaterialDTO
    {
        Id = m.Id,
        MaterialName = m.MaterialName,
        MaterialTypeId = m.MaterialTypeId,
        MaterialType = new MaterialTypeDTO
        {
            Id = m.MaterialType.Id,
            Name = m.MaterialType.Name,
            CheckoutDays = m.MaterialType.CheckoutDays
        },
        GenreId = m.GenreId,
        Genre = new GenreDTO
        {
            Id = m.Genre.Id,
            Name = m.Genre.Name
        },
        OutOfCirculationSince = m.OutOfCirculationSince
    }).ToList();
});

app.MapGet("/api/materials/{id}", (LoncotesLibraryDbContext db, int id) =>
{
    return db.Materials
    .Include(m => m.MaterialType)
    .Include(m => m.Genre)
    .Include(m => m.Checkouts)
    .ThenInclude(c => c.Patron)
    .Select(m => new MaterialDTO
    {
        Id = m.Id,
        MaterialName = m.MaterialName,
        MaterialTypeId = m.MaterialTypeId,
        MaterialType = new MaterialTypeDTO
        {
            Id = m.MaterialType.Id,
            Name = m.MaterialType.Name,
            CheckoutDays = m.MaterialType.CheckoutDays
        },
        GenreId = m.GenreId,
        Genre = new GenreDTO
        {
            Id = m.Genre.Id,
            Name = m.Genre.Name
        },
        OutOfCirculationSince = m.OutOfCirculationSince,
        Checkouts = m.Checkouts != null ? m.Checkouts.Select(c => new CheckoutWithLateFeeDTO 
        {
            Id = c.Id,
            MaterialId = c.MaterialId,
            Material = c.Material != null ? new MaterialDTO
            {
                Id = c.Material.Id,
                MaterialName = c.Material.MaterialName,
                MaterialTypeId = c.Material.MaterialTypeId,
                MaterialType = c.Material.MaterialType != null ? new MaterialTypeDTO
                {
                    Id = c.Material.MaterialType.Id,
                    Name = c.Material.MaterialType.Name,
                    CheckoutDays = c.Material.MaterialType.CheckoutDays
                } : null,
                GenreId = c.Material.GenreId,
                
            } : null,
            PatronId = c.PatronId,
            Patron = c.Patron != null ? new PatronWithBalanceDTO
            {
                Id = c.Patron.Id,
                FirstName = c.Patron.FirstName,
                LastName = c.Patron.LastName,
                Address = c.Patron.Address,
                Email = c.Patron.Email,
                IsActive = c.Patron.IsActive,
                
                
            } : null,
            CheckoutDate = c.CheckoutDate,
            ReturnDate = c.ReturnDate,
            Paid = c.Paid
        }).ToList() : null
    }).Single(m => m.Id == id);
});

app.MapPost("/api/materials", (LoncotesLibraryDbContext db, Material material) => 
{
    db.Materials.Add(material);
    db.SaveChanges();
    return Results.Created($"/api/materials/{material.Id}", material);
});

app.MapPut("/api/materials", (LoncotesLibraryDbContext db, int id, Material material) =>
{
    Material materialToUncirculate = db.Materials.SingleOrDefault(material => material.Id == id);
    if (materialToUncirculate == null)
    {
        return Results.NotFound();
    }
    materialToUncirculate.MaterialName = material.MaterialName;
    materialToUncirculate.MaterialTypeId = material.MaterialTypeId;
    materialToUncirculate.GenreId = material.GenreId;
    materialToUncirculate.OutOfCirculationSince = DateTime.Now;

    db.SaveChanges();
    return Results.NoContent();
});

app.MapGet("/api/materialtypes", (LoncotesLibraryDbContext db) =>
{
    return db.MaterialTypes
        .Select(mt => new MaterialTypeDTO
        {
            Id = mt.Id,
            Name = mt.Name,
            CheckoutDays = mt.CheckoutDays
        }).ToList();
});

app.MapGet("/api/genres", (LoncotesLibraryDbContext db) =>
{
    return db.Genres
        .Select(g => new GenreDTO
        {
            Id = g.Id,
            Name = g.Name
        }).ToList();
});

app.MapGet("/api/patrons", (LoncotesLibraryDbContext db) =>
{
    return db.Patrons
        .Select(p => new PatronDTO
        {
            Id = p.Id,
            FirstName = p.FirstName,
            LastName = p.LastName,
            Address = p.Address,
            Email = p.Email,
            IsActive = p.IsActive
        }).ToList();
});

app.MapPost("/api/checkouts", (LoncotesLibraryDbContext db, Checkout checkout) =>
{   
    // checkout.CheckoutDate = DateTime.Now;
    // checkout.ReturnDate = null;
    db.Checkouts.Add(checkout);
    db.SaveChanges();
    return Results.Created($"/api/checkouts/{checkout.Id}", checkout);
});

app.MapGet("/api/patrons/{id}", (LoncotesLibraryDbContext db, int id) =>
{
    return db.Patrons
        .Include(p => p.Checkouts)
        .ThenInclude(c => c.Material)
        .ThenInclude(c => c.MaterialType)
        .Select(p => new PatronWithBalanceDTO
        {
            Id = p.Id,
            FirstName = p.FirstName,
            LastName = p.LastName,
            Address = p.Address,
            Email = p.Email,
            IsActive = p.IsActive,
            //if this breaks, change back to checkoutdto
            Checkouts = p.Checkouts.Select(c => new CheckoutWithLateFeeDTO
            {
                Id = c.Id,
                MaterialId = c.MaterialId,
                Material = c.Material != null ? new MaterialDTO
                {
                    Id = c.Material.Id,
                    MaterialName = c.Material.MaterialName,
                    MaterialTypeId = c.Material.MaterialTypeId,
                    MaterialType = new MaterialTypeDTO
                        {
                            Id = c.Material.MaterialType.Id,
                            Name = c.Material.MaterialType.Name,
                            CheckoutDays = c.Material.MaterialType.CheckoutDays
                        },
                    GenreId = c.Material.GenreId,
                    OutOfCirculationSince = c.Material.OutOfCirculationSince
                } : null,
                PatronId = c.PatronId,
                CheckoutDate = c.CheckoutDate,
                ReturnDate = c.ReturnDate
            }).ToList()
        }).Single(p => p.Id == id);
});

app.MapPut("/api/patrons/{id}", (LoncotesLibraryDbContext db, int id, Patron patron) =>
{
    Patron patronToUpdate = db.Patrons.SingleOrDefault(p => p.Id == id);
    if (patronToUpdate == null)
    {
        return Results.NotFound();
    }
    patronToUpdate.Address = patron.Address;
    patronToUpdate.Email = patron.Email;
    db.SaveChanges();
    return Results.NoContent();
});

//Deactivate patron
app.MapPut("/api/patrons/{id}/deactivate", (LoncotesLibraryDbContext db, int id, Patron patron) =>
{
    Patron patronToUpdate = db.Patrons.SingleOrDefault(p => p.Id == id);
    if (patronToUpdate == null)
    {
        return Results.NotFound();
    }
    patronToUpdate.IsActive = false;
    db.SaveChanges();
    return Results.NoContent();
});

//Put will take material Id and Select checkout with matching Id and return date of null
//then update returndate, save changes, return no content
app.MapPut("/api/checkouts/materials/{materialId}", (LoncotesLibraryDbContext db, int materialId) =>
{   //THIS CAN ALSO BE PUT INTO A VAR IF NEEDED
    var checkoutToReturn = db.Checkouts.FirstOrDefault(c => c.MaterialId == materialId && c.ReturnDate == null);
    if (checkoutToReturn == null)
    {
        return Results.NotFound();
    }
    checkoutToReturn.ReturnDate = DateTime.Now;
    db.SaveChanges();
    return Results.NoContent();

});

app.MapPut("/api/checkouts/{id}", (LoncotesLibraryDbContext db, int id) =>
{
    var checkoutToUpdate = db.Checkouts.FirstOrDefault(c => c.Id == id);
    if (checkoutToUpdate == null)
    {
        return Results.NotFound();
    }
    checkoutToUpdate.ReturnDate = DateTime.Now;
    db.SaveChanges();
    return Results.NoContent();
});

app.MapGet("/api/materials/available", (LoncotesLibraryDbContext db) =>
{
    return db.Materials
    .Where(m => m.OutOfCirculationSince == null)
    //material WHERE ALL checkouts have null returnDate
    .Where(m => m.Checkouts.All(co => co.ReturnDate != null))
    .Select(material => new MaterialDTO
    {
        Id = material.Id,
        MaterialName = material.MaterialName,
        MaterialTypeId = material.MaterialTypeId,
        GenreId = material.GenreId,
        OutOfCirculationSince = material.OutOfCirculationSince
    })
    .ToList();
});

app.MapGet("/api/checkouts/overdue", (LoncotesLibraryDbContext db) =>
{
    return db.Checkouts
    .Include(p => p.Patron)
    .Include(co => co.Material)
    .ThenInclude(m => m.MaterialType)
    .Where(co =>
        (DateTime.Today - co.CheckoutDate).Days >
        co.Material.MaterialType.CheckoutDays &&
        co.ReturnDate == null)
        .Select(co => new CheckoutWithLateFeeDTO
        {
            Id = co.Id,
            MaterialId = co.MaterialId,
            Material = new MaterialDTO
            {
                Id = co.Material.Id,
                MaterialName = co.Material.MaterialName,
                MaterialTypeId = co.Material.MaterialTypeId,
                MaterialType = new MaterialTypeDTO
                {
                    Id = co.Material.MaterialTypeId,
                    Name = co.Material.MaterialType.Name,
                    CheckoutDays = co.Material.MaterialType.CheckoutDays
                },
                GenreId = co.Material.GenreId,
                OutOfCirculationSince = co.Material.OutOfCirculationSince
            },
            PatronId = co.PatronId,
            Patron = new PatronWithBalanceDTO
            {
                Id = co.Patron.Id,
                FirstName = co.Patron.FirstName,
                LastName = co.Patron.LastName,
                Address = co.Patron.Address,
                Email = co.Patron.Email,
                IsActive = co.Patron.IsActive
            },
            CheckoutDate = co.CheckoutDate,
            ReturnDate = co.ReturnDate
        })
    .ToList();
    
});

//days checked out > allowedCheckOutDays
//days checked out > checkout.Material.MaterialType.CheckoutDays
//(DateTime.Today - checkout.checkoutdate).Days > ^



app.Run();

