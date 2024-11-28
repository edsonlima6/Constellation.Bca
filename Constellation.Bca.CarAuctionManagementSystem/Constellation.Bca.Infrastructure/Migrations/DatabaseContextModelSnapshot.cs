﻿// <auto-generated />
using Constellation.Bca.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Constellation.Bca.Infrastructure.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    partial class DatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.0");

            modelBuilder.Entity("Constellation.Bca.Domain.Entites.Vehicle", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double>("LoadCapacity")
                        .HasColumnType("REAL");

                    b.Property<string>("Manufacturer")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("NumberOfDoors")
                        .HasColumnType("INTEGER");

                    b.Property<int>("NumberOfSeats")
                        .HasColumnType("INTEGER");

                    b.Property<int>("RegistrationYear")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("StartingBid")
                        .HasColumnType("TEXT");

                    b.Property<string>("UniqueIdentifier")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("VehicleType")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasAlternateKey("Id", "UniqueIdentifier");

                    b.ToTable("Vehicle");
                });
#pragma warning restore 612, 618
        }
    }
}
