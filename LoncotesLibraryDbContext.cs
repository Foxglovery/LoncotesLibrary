using Microsoft.EntityFrameworkCore;
using LoncotesLibrary.Models;

public class LoncotesLibraryDbContext : DbContext
{

    public DbSet<Checkout> Checkouts { get; set; }
    public DbSet<Material> Materials { get; set; }
    public DbSet<Patron> Patrons { get; set; }
    public DbSet<MaterialType> MaterialTypes { get; set; }
    public DbSet<Genre> Genres { get; set; }

    public LoncotesLibraryDbContext(DbContextOptions<LoncotesLibraryDbContext> context) : base(context)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    // seed data with campsite types
    modelBuilder.Entity<Patron>().HasData(new Patron[]
    {
        
        new Patron {Id = 1, FirstName = "Groogery", LastName = "Adlerburb", Address = "9749 Brayan Islands", Email = "whoputmeinhere@neopets.com", IsActive = true},
        new Patron {Id = 2, FirstName = "Schlebethany", LastName = "Jerp", Address = "580 Antonia Landing", Email = "alleyoop@farquest.com", IsActive = true},
        new Patron {Id = 3, FirstName = "Jeef", LastName = "Dagsroll", Address = "408 Ritchie Crest", Email = "abjfrud@gmoil.com", IsActive = false},
        new Patron {Id = 4, FirstName = "Fabriel", LastName = "Corlingping", Address = "6106 Corkery Key", Email = "dusterbuster@fathingtons.com", IsActive = true},
        new Patron {Id = 5, FirstName = "Korg", LastName = "Dumfries", Address = "4168 Hagenes Overpass", Email = "Whesker@OysterCrackers.com", IsActive = true},
        
    });

    modelBuilder.Entity<MaterialType>().HasData(new MaterialType[]
    {
        new MaterialType {Id = 1, Name = "Book", CheckoutDays = 14},
        new MaterialType {Id = 2, Name = "Mind Prism", CheckoutDays = 3},
        new MaterialType {Id = 3, Name = "Periodical", CheckoutDays = 14},
        new MaterialType {Id = 4, Name = "Living Story", CheckoutDays = 3}
    });

    modelBuilder.Entity<Material>().HasData(new Material[]
    {
        new Material {Id = 1, MaterialName = "The Anarchist Cookbook", MaterialTypeId = 1, GenreId = 1, OutOfCirculationSince = null},
        new Material {Id = 2, MaterialName = "The Joy of Spice Mining", MaterialTypeId = 1, GenreId = 2, OutOfCirculationSince = null},
        new Material {Id = 3, MaterialName = "Highlights for Grown-Ass Kids Vol. II", MaterialTypeId = 3, GenreId = 5, OutOfCirculationSince = null},
        new Material {Id = 4, MaterialName = "Chickamauga Frontiersman", MaterialTypeId = 2, GenreId = 4, OutOfCirculationSince = null},
        new Material {Id = 5, MaterialName = "Our Neighbor Deb", MaterialTypeId = 4, GenreId = 4, OutOfCirculationSince = null},
        new Material {Id = 6, MaterialName = "Actually Be John Malkovich", MaterialTypeId = 2, GenreId = 5, OutOfCirculationSince = null},
        new Material {Id = 7, MaterialName = "How To Win Fries and Influence Potatoes", MaterialTypeId = 1, GenreId = 1, OutOfCirculationSince = null},
        new Material {Id = 8, MaterialName = "NYT Onion Articles Vol. XI", MaterialTypeId = 3, GenreId = 5, OutOfCirculationSince = null},
        new Material {Id = 9, MaterialName = "That One Guy Who Went To Space", MaterialTypeId = 4, GenreId = 2, OutOfCirculationSince = null},
        new Material {Id = 10, MaterialName = "The Mind of a Programmer", MaterialTypeId = 2, GenreId = 3, OutOfCirculationSince = null},
    });

    modelBuilder.Entity<Genre>().HasData(new Genre[]
    {
        new Genre {Id = 1, Name = "Self-Help"},
        new Genre {Id = 2, Name = "Sci-Fi"},
        new Genre {Id = 3, Name = "Horror"},
        new Genre {Id = 4, Name = "Non-Fiction"},
        new Genre {Id = 5, Name = "Fantasy"},
    });

    modelBuilder.Entity<Checkout>().HasData(new Checkout[]
    {

    });
}
}