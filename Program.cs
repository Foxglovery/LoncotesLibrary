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
    .Include(m => m.Checkout)
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
        Checkout = m.Checkout != null ? new CheckoutDTO
        {
            Id = m.Checkout.Id,
            MaterialId = m.Checkout.MaterialId,
            PatronId = m.Checkout.PatronId,
            Patron = m.Checkout.Patron != null ? new PatronDTO
            {
                Id = m.Checkout.Patron.Id,
                FirstName = m.Checkout.Patron.FirstName,
                LastName = m.Checkout.Patron.LastName,
                Address = m.Checkout.Patron.Address,
                Email = m.Checkout.Patron.Email,
                IsActive = m.Checkout.Patron.IsActive
            } : null,
            CheckoutDate = m.Checkout.CheckoutDate,
            ReturnDate = m.Checkout.ReturnDate
        } : null
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
{   checkout.CheckoutDate = DateTime.Now;
    db.Checkouts.Add(checkout);
    db.SaveChanges();
    return Results.Created($"/api/checkouts/{checkout.Id}", checkout);
});

app.MapGet("/api/patrons/{id}", (LoncotesLibraryDbContext db, int id) =>
{
    return db.Patrons
        .Include(p => p.Checkout)
        .ThenInclude(c => c.Material)
        .ThenInclude(c => c.MaterialType)
        .Select(p => new PatronDTO
        {
            Id = p.Id,
            FirstName = p.FirstName,
            LastName = p.LastName,
            Address = p.Address,
            Email = p.Email,
            IsActive = p.IsActive,
            Checkout = p.Checkout != null ? new CheckoutDTO
            {
                Id = p.Checkout.Id,
                MaterialId = p.Checkout.MaterialId,
                Material = p.Checkout.Material != null ? new MaterialDTO
                {
                    Id = p.Checkout.Material.Id,
                    MaterialName = p.Checkout.Material.MaterialName,
                    MaterialTypeId = p.Checkout.Material.MaterialTypeId,
                    MaterialType = new MaterialTypeDTO
                        {
                            Id = p.Checkout.Material.MaterialType.Id,
                            Name = p.Checkout.Material.MaterialType.Name,
                            CheckoutDays = p.Checkout.Material.MaterialType.CheckoutDays
                        },
                    GenreId = p.Checkout.Material.GenreId,
                    OutOfCirculationSince = p.Checkout.Material.OutOfCirculationSince
                } : null,
                PatronId = p.Checkout.PatronId,
                CheckoutDate = p.Checkout.CheckoutDate,
                ReturnDate = p.Checkout.ReturnDate
            } : null
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

app.Run();

