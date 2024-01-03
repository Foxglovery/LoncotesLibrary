﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace LoncotesLibrary.Migrations
{
    [DbContext(typeof(LoncotesLibraryDbContext))]
    [Migration("20240103155920_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("LoncotesLibrary.Models.Checkout", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CheckoutDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("MaterialId")
                        .HasColumnType("integer");

                    b.Property<int>("PatronId")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("ReturnDate")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("MaterialId");

                    b.HasIndex("PatronId");

                    b.ToTable("Checkouts");
                });

            modelBuilder.Entity("LoncotesLibrary.Models.Genre", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Genres");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Self-Help"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Sci-Fi"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Horror"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Non-Fiction"
                        },
                        new
                        {
                            Id = 5,
                            Name = "Fantasy"
                        });
                });

            modelBuilder.Entity("LoncotesLibrary.Models.Material", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("GenreId")
                        .HasColumnType("integer");

                    b.Property<string>("MaterialName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("MaterialTypeId")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("OutOfCirculationSince")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("GenreId");

                    b.ToTable("Materials");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            GenreId = 1,
                            MaterialName = "The Anarchist Cookbook",
                            MaterialTypeId = 1
                        },
                        new
                        {
                            Id = 2,
                            GenreId = 2,
                            MaterialName = "The Joy of Spice Mining",
                            MaterialTypeId = 1
                        },
                        new
                        {
                            Id = 3,
                            GenreId = 5,
                            MaterialName = "Highlights for Grown-Ass Kids Vol. II",
                            MaterialTypeId = 3
                        },
                        new
                        {
                            Id = 4,
                            GenreId = 4,
                            MaterialName = "Chickamauga Frontiersman",
                            MaterialTypeId = 2
                        },
                        new
                        {
                            Id = 5,
                            GenreId = 4,
                            MaterialName = "Our Neighbor Deb",
                            MaterialTypeId = 4
                        },
                        new
                        {
                            Id = 6,
                            GenreId = 5,
                            MaterialName = "Actually Be John Malkovich",
                            MaterialTypeId = 2
                        },
                        new
                        {
                            Id = 7,
                            GenreId = 1,
                            MaterialName = "How To Win Fries and Influence Potatoes",
                            MaterialTypeId = 1
                        },
                        new
                        {
                            Id = 8,
                            GenreId = 5,
                            MaterialName = "NYT Onion Articles Vol. XI",
                            MaterialTypeId = 3
                        },
                        new
                        {
                            Id = 9,
                            GenreId = 2,
                            MaterialName = "That One Guy Who Went To Space",
                            MaterialTypeId = 4
                        },
                        new
                        {
                            Id = 10,
                            GenreId = 3,
                            MaterialName = "The Mind of a Programmer",
                            MaterialTypeId = 2
                        });
                });

            modelBuilder.Entity("LoncotesLibrary.Models.MaterialType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CheckoutDays")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("MaterialTypes");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CheckoutDays = 14,
                            Name = "Book"
                        },
                        new
                        {
                            Id = 2,
                            CheckoutDays = 3,
                            Name = "Mind Prism"
                        },
                        new
                        {
                            Id = 3,
                            CheckoutDays = 14,
                            Name = "Periodical"
                        },
                        new
                        {
                            Id = 4,
                            CheckoutDays = 3,
                            Name = "Living Story"
                        });
                });

            modelBuilder.Entity("LoncotesLibrary.Models.Patron", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Patrons");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Address = "9749 Brayan Islands",
                            Email = "whoputmeinhere@neopets.com",
                            FirstName = "Groogery",
                            IsActive = true,
                            LastName = "Adlerburb"
                        },
                        new
                        {
                            Id = 2,
                            Address = "580 Antonia Landing",
                            Email = "alleyoop@farquest.com",
                            FirstName = "Schlebethany",
                            IsActive = true,
                            LastName = "Jerp"
                        },
                        new
                        {
                            Id = 3,
                            Address = "408 Ritchie Crest",
                            Email = "abjfrud@gmoil.com",
                            FirstName = "Jeef",
                            IsActive = false,
                            LastName = "Dagsroll"
                        },
                        new
                        {
                            Id = 4,
                            Address = "6106 Corkery Key",
                            Email = "dusterbuster@fathingtons.com",
                            FirstName = "Fabriel",
                            IsActive = true,
                            LastName = "Corlingping"
                        },
                        new
                        {
                            Id = 5,
                            Address = "4168 Hagenes Overpass",
                            Email = "Whesker@OysterCrackers.com",
                            FirstName = "Korg",
                            IsActive = true,
                            LastName = "Dumfries"
                        });
                });

            modelBuilder.Entity("LoncotesLibrary.Models.Checkout", b =>
                {
                    b.HasOne("LoncotesLibrary.Models.Material", "Material")
                        .WithMany()
                        .HasForeignKey("MaterialId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LoncotesLibrary.Models.Patron", "Patron")
                        .WithMany()
                        .HasForeignKey("PatronId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Material");

                    b.Navigation("Patron");
                });

            modelBuilder.Entity("LoncotesLibrary.Models.Material", b =>
                {
                    b.HasOne("LoncotesLibrary.Models.Genre", "Genre")
                        .WithMany()
                        .HasForeignKey("GenreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Genre");
                });
#pragma warning restore 612, 618
        }
    }
}
