﻿// <auto-generated />
using System;
using DealRept.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DealRept.Migrations
{
    [DbContext(typeof(DealDbContext))]
    [Migration("20231106154838_checkBeforePublishing1")]
    partial class checkBeforePublishing1
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.17");

            modelBuilder.Entity("DealRept.Models.Bank", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(6)
                        .HasColumnType("char(6)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.HasKey("ID");

                    b.HasAlternateKey("Code");

                    b.HasAlternateKey("Name");

                    b.ToTable("Banks");
                });

            modelBuilder.Entity("DealRept.Models.Branch", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("BranchEmail")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("BranchTelephone")
                        .IsRequired()
                        .HasMaxLength(13)
                        .HasColumnType("varchar(13)");

                    b.Property<int>("CityID")
                        .HasColumnType("int");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(5)
                        .HasColumnType("varchar(5)");

                    b.Property<string>("HeadEmail")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("HeadFirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("HeadLastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("HeadMiddleName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("HeadTelephone")
                        .IsRequired()
                        .HasMaxLength(13)
                        .HasColumnType("varchar(13)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.Property<string>("PostalIndex")
                        .IsRequired()
                        .HasMaxLength(5)
                        .HasColumnType("char(5)");

                    b.Property<string>("StreetBuilding")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.HasKey("ID");

                    b.HasAlternateKey("Code");

                    b.HasIndex("CityID");

                    b.ToTable("Branches");
                });

            modelBuilder.Entity("DealRept.Models.City", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("varchar(300)");

                    b.HasKey("ID");

                    b.HasAlternateKey("Name");

                    b.ToTable("Cities");
                });

            modelBuilder.Entity("DealRept.Models.ContractStatus", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)");

                    b.HasKey("ID");

                    b.ToTable("ContractStatuses");
                });

            modelBuilder.Entity("DealRept.Models.CurrentContract", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("BranchID")
                        .HasColumnType("int");

                    b.Property<DateTime>("ConclusionDate")
                        .HasColumnType("Date");

                    b.Property<decimal>("ContractAmount")
                        .HasColumnType("Decimal(10,2)");

                    b.Property<string>("ContractNumber")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)");

                    b.Property<int>("ContractStatusID")
                        .HasColumnType("int");

                    b.Property<DateTime>("ExpirationDate")
                        .HasColumnType("Date");

                    b.Property<bool>("IsCourtDispute")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsGoods")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsProlonged")
                        .HasColumnType("tinyint(1)")
                        .HasDefaultValue(false);

                    b.Property<string>("Remarks")
                        .HasColumnType("text");

                    b.Property<string>("Subject")
                        .IsRequired()
                        .HasMaxLength(2147483647)
                        .HasColumnType("Text");

                    b.Property<int>("SupplierID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasAlternateKey("ContractNumber", "ConclusionDate");

                    b.HasIndex("BranchID");

                    b.HasIndex("ContractStatusID");

                    b.HasIndex("SupplierID");

                    b.ToTable("CurrentContracts");

                    b.HasCheckConstraint("CHK_TermDate", "ExpirationDate > ConclusionDate");
                });

            modelBuilder.Entity("DealRept.Models.CurrentDocument", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("CurrentContractID")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateOfUploading")
                        .HasColumnType("Date");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.Property<string>("PathToDocument")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.HasKey("ID");

                    b.HasAlternateKey("Name", "CurrentContractID");

                    b.HasIndex("CurrentContractID");

                    b.ToTable("CurrentDocuments");
                });

            modelBuilder.Entity("DealRept.Models.PastContract", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("BranchID")
                        .HasColumnType("int");

                    b.Property<DateTime>("ConclusionDate")
                        .HasColumnType("Date");

                    b.Property<decimal>("ContractAmount")
                        .HasColumnType("Decimal(10,2)");

                    b.Property<string>("ContractNumber")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)");

                    b.Property<int>("ContractStatusID")
                        .HasColumnType("int");

                    b.Property<DateTime>("ExpirationDate")
                        .HasColumnType("Date");

                    b.Property<bool>("IsCourtDispute")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsGoods")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsProlonged")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Remarks")
                        .HasColumnType("text");

                    b.Property<string>("Subject")
                        .IsRequired()
                        .HasMaxLength(2147483647)
                        .HasColumnType("Text");

                    b.Property<int>("SupplierID")
                        .HasColumnType("int");

                    b.Property<DateTime>("TransitionDate")
                        .HasColumnType("Date");

                    b.HasKey("ID");

                    b.HasAlternateKey("ContractNumber", "ConclusionDate");

                    b.HasIndex("BranchID");

                    b.HasIndex("ContractStatusID");

                    b.HasIndex("SupplierID");

                    b.ToTable("PastContracts");

                    b.HasCheckConstraint("CHK_TransDate", "TransitionDate >= ConclusionDate");
                });

            modelBuilder.Entity("DealRept.Models.PastDocument", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("DateOfUploading")
                        .HasColumnType("Date");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.Property<int>("PastContractID")
                        .HasColumnType("int");

                    b.Property<string>("PathToDocument")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.HasKey("ID");

                    b.HasIndex("PastContractID");

                    b.ToTable("PastDocuments");
                });

            modelBuilder.Entity("DealRept.Models.Specialization", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.HasKey("ID");

                    b.ToTable("Specializations");
                });

            modelBuilder.Entity("DealRept.Models.Supplier", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("BankAccount")
                        .IsRequired()
                        .HasMaxLength(29)
                        .HasColumnType("char(29)");

                    b.Property<int>("BankID")
                        .HasColumnType("int");

                    b.Property<int>("CityID")
                        .HasColumnType("int");

                    b.Property<string>("CompanyEmail")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("CompanyTelephone")
                        .IsRequired()
                        .HasMaxLength(13)
                        .HasColumnType("varchar(13)");

                    b.Property<string>("ContactEmail")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("ContactFirstName")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("ContactLastName")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("ContactMiddleName")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("ContactTelephone")
                        .HasMaxLength(13)
                        .HasColumnType("varchar(13)");

                    b.Property<string>("DirectorFirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("DirectorLastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("DirectorMiddleName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("LegalCode")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("varchar(10)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.Property<string>("NegativeRemarks")
                        .HasColumnType("text");

                    b.Property<string>("PostalIndex")
                        .IsRequired()
                        .HasMaxLength(5)
                        .HasColumnType("char(5)");

                    b.Property<int>("SpecializationID")
                        .HasColumnType("int");

                    b.Property<string>("StreetBuilding")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.HasKey("ID");

                    b.HasAlternateKey("LegalCode");

                    b.HasIndex("BankID");

                    b.HasIndex("CityID");

                    b.HasIndex("SpecializationID");

                    b.ToTable("Suppliers");
                });

            modelBuilder.Entity("DealRept.Models.Branch", b =>
                {
                    b.HasOne("DealRept.Models.City", "City")
                        .WithMany("Branches")
                        .HasForeignKey("CityID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("City");
                });

            modelBuilder.Entity("DealRept.Models.CurrentContract", b =>
                {
                    b.HasOne("DealRept.Models.Branch", "Branch")
                        .WithMany("CurrentContracts")
                        .HasForeignKey("BranchID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("DealRept.Models.ContractStatus", "ContractStatus")
                        .WithMany("CurrentContracts")
                        .HasForeignKey("ContractStatusID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("DealRept.Models.Supplier", "Supplier")
                        .WithMany("CurrentContracts")
                        .HasForeignKey("SupplierID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Branch");

                    b.Navigation("ContractStatus");

                    b.Navigation("Supplier");
                });

            modelBuilder.Entity("DealRept.Models.CurrentDocument", b =>
                {
                    b.HasOne("DealRept.Models.CurrentContract", "CurrentContract")
                        .WithMany("CurrentDocuments")
                        .HasForeignKey("CurrentContractID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CurrentContract");
                });

            modelBuilder.Entity("DealRept.Models.PastContract", b =>
                {
                    b.HasOne("DealRept.Models.Branch", "Branch")
                        .WithMany("PastContracts")
                        .HasForeignKey("BranchID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("DealRept.Models.ContractStatus", "ContractStatus")
                        .WithMany("PastContracts")
                        .HasForeignKey("ContractStatusID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("DealRept.Models.Supplier", "Supplier")
                        .WithMany("PastContracts")
                        .HasForeignKey("SupplierID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Branch");

                    b.Navigation("ContractStatus");

                    b.Navigation("Supplier");
                });

            modelBuilder.Entity("DealRept.Models.PastDocument", b =>
                {
                    b.HasOne("DealRept.Models.PastContract", "PastContract")
                        .WithMany("PastDocuments")
                        .HasForeignKey("PastContractID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PastContract");
                });

            modelBuilder.Entity("DealRept.Models.Supplier", b =>
                {
                    b.HasOne("DealRept.Models.Bank", "Bank")
                        .WithMany("Suppliers")
                        .HasForeignKey("BankID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("DealRept.Models.City", "City")
                        .WithMany("Suppliers")
                        .HasForeignKey("CityID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("DealRept.Models.Specialization", "Specialization")
                        .WithMany("Suppliers")
                        .HasForeignKey("SpecializationID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Bank");

                    b.Navigation("City");

                    b.Navigation("Specialization");
                });

            modelBuilder.Entity("DealRept.Models.Bank", b =>
                {
                    b.Navigation("Suppliers");
                });

            modelBuilder.Entity("DealRept.Models.Branch", b =>
                {
                    b.Navigation("CurrentContracts");

                    b.Navigation("PastContracts");
                });

            modelBuilder.Entity("DealRept.Models.City", b =>
                {
                    b.Navigation("Branches");

                    b.Navigation("Suppliers");
                });

            modelBuilder.Entity("DealRept.Models.ContractStatus", b =>
                {
                    b.Navigation("CurrentContracts");

                    b.Navigation("PastContracts");
                });

            modelBuilder.Entity("DealRept.Models.CurrentContract", b =>
                {
                    b.Navigation("CurrentDocuments");
                });

            modelBuilder.Entity("DealRept.Models.PastContract", b =>
                {
                    b.Navigation("PastDocuments");
                });

            modelBuilder.Entity("DealRept.Models.Specialization", b =>
                {
                    b.Navigation("Suppliers");
                });

            modelBuilder.Entity("DealRept.Models.Supplier", b =>
                {
                    b.Navigation("CurrentContracts");

                    b.Navigation("PastContracts");
                });
#pragma warning restore 612, 618
        }
    }
}
