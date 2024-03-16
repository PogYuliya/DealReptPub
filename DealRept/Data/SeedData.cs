using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace DealRept.Models
{
    public static class SeedData
    {
        public static void EnsurePopulated(IApplicationBuilder app)
        {
            DealDbContext context = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<DealDbContext>();

            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }

            if (!context.Cities.Any())
            {
                context.Cities.AddRange(
                    new City
                    {
                        Name = "Mariupol"
                    },
                    new City
                    {
                        Name = "Kramatorsk"
                    },
                    new City
                    {
                        Name = "Donetsk"
                    },
                    new City
                    {
                        Name = "Kharkov"
                    },
                    new City
                    {
                        Name = "Makeevka"
                    },
                    //6
                    new City
                    {
                        Name = "Chernigov"
                    },
                    //7
                    new City
                    {
                        Name = "Lvov"
                    },
                    //8
                    new City
                    {
                        Name = "Zhitomir"
                    },
                    //9
                    new City
                    {
                        Name = "Ivanofrankovsk"
                    },
                    //10
                    new City
                    {
                        Name = "Dnepr"
                    }
                    );
                context.SaveChanges();

                context.Banks.AddRange(
                    new Bank
                    {
                        Name = "PrivatBank",
                        Code = "305299"
                    },
                    new Bank
                    {
                        Name = "Oschadbank",
                        Code = "300465"
                    },
                    new Bank
                    {
                        Name = "Ukreximbank",
                        Code = "322313"
                    },
                    new Bank
                    {
                        Name = "Alfa-Bank",
                        Code = "300346"
                    },
                    new Bank
                    {
                        Name = "Raiffeisen Bank",
                        Code = "300335"
                    }
                    );
                context.SaveChanges();

                context.Specializations.AddRange(
                    new Specialization
                    {
                        Name = "construction materials"
                    },
                    new Specialization
                    {
                        Name = "food"
                    },
                    new Specialization
                    {
                        Name = "household cleaning products household cleaning products household cleaning products"
                    },
                    new Specialization
                    {
                        Name = "chemicals"
                    },
                    new Specialization
                    {
                        Name = "solid fuel"
                    },
                    new Specialization
                    {
                        Name = "liquid fuel",
                    }

                    );
                context.SaveChanges();

                context.Suppliers.AddRange(
                    new Supplier
                    {
                        Name = "LLC Cascade",
                        StreetBuilding = "Torgovaya st., 125",
                        PostalIndex = "87500",
                        LegalCode = "12356722",
                        BankAccount = "UA493351060000000260083001675",
                        CompanyTelephone = "+380671234534",
                        CompanyEmail = "cascade@test.com",
                        ContactFirstName = "Olga",
                        ContactLastName = "Ivanova",
                        ContactMiddleName = "Petrovna",
                        ContactEmail = "ivanova@test.com",
                        ContactTelephone = "+380671234537",
                        DirectorFirstName = "Kiril",
                        DirectorLastName = "Dotov",
                        DirectorMiddleName = "Ivanovich",
                        NegativeRemarks = "Partial delivery and bad character, Contract #1235",
                        CityID = 1,
                        BankID = 2,
                        SpecializationID = 1
                    },
                    new Supplier
                    {
                        Name = "LLC Food",
                        StreetBuilding = " Mira av., 12",
                        PostalIndex = "84312",
                        LegalCode = "19936503",
                        BankAccount = "UA091300465000002603930071801",
                        CompanyTelephone = "+380961234456",
                        CompanyEmail = "food@test.com",
                        ContactFirstName = "Petr",
                        ContactLastName = "Petrov",
                        ContactMiddleName = "Alekseevich",
                        ContactEmail = "petrov@test.com",
                        ContactTelephone = "+380671234538",
                        DirectorFirstName = "Anton",
                        DirectorLastName = "Mironov",
                        DirectorMiddleName = "Petrovich",
                        CityID = 4,
                        BankID = 1,
                        SpecializationID = 3
                    },
                    new Supplier
                    {
                        Name = "LLC Syntez",
                        StreetBuilding = "Metallurgiv av., 17",
                        PostalIndex = "87539",
                        LegalCode = "16729673",
                        BankAccount = "UA403005280000026002301339243",
                        CompanyTelephone = "+380961234456",
                        CompanyEmail = "syntez@test.com",
                        ContactFirstName = "Igor",
                        ContactMiddleName = "Petrovich",
                        ContactEmail = "igorev@test.com",
                        DirectorFirstName = "Ivan",
                        DirectorLastName = "Dobrinin",
                        DirectorMiddleName = "Petrovich",
                        CityID = 2,
                        BankID = 3,
                        SpecializationID = 5
                    },
                    new Supplier
                    {
                        Name = "LLC Epicenter",
                        StreetBuilding = "Centralnaya av., 54",
                        PostalIndex = "85440",
                        LegalCode = "23156891",
                        BankAccount = "UA883355480000026005053614097",
                        CompanyTelephone = "+380991234456",
                        CompanyEmail = "epicenter@test.com",
                        ContactFirstName = "Maksim",
                        ContactLastName = "Maksimov",
                        ContactMiddleName = "Maksimovich",
                        ContactEmail = "maksimov@test.com",
                        DirectorFirstName = "Helena",
                        DirectorLastName = "Kamenskaya",
                        DirectorMiddleName = "Vicrorovna",
                        NegativeRemarks = "Partial delivery and bad character, Contract #1235",
                        CityID = 5,
                        BankID = 4,
                        SpecializationID = 4
                    },
                    new Supplier
                    {
                        Name = "IE Vasichkin Vasiliy Vasilievich",
                        StreetBuilding = "Svobodi st., 123",
                        PostalIndex = "12355",
                        LegalCode = "5322866982",
                        BankAccount = "UA243223130000026009000000161",
                        CompanyTelephone = "+380961234452",
                        CompanyEmail = "vasichkin@test.com",
                        //ContactFirstName = "Maksim",
                        //ContactLastName = "Maksimov",
                        //ContactMiddleName = "Maksimovich",
                        //ContactEmail = "maksimov@test.com",
                        DirectorFirstName = "Vasiliy",
                        DirectorLastName = "Vasichkin",
                        DirectorMiddleName = "Vasilievich",
                        //NegativeRemarksSearch = "Partial delivery and bad character, Contract #1235",
                        CityID = 3,
                        BankID = 5,
                        SpecializationID = 2
                    },
                    new Supplier
                    {
                        Name = "LLC OKKO",
                        StreetBuilding = "Zelinskaya st, 67",
                        PostalIndex = "54321",
                        LegalCode = "12356120",
                        BankAccount = "UA443223130000026005000000422",
                        CompanyTelephone = "+380962234456",
                        CompanyEmail = "okko@test.com",
                        ContactFirstName = "Oksana",
                        ContactLastName = "Zotova",
                        ContactMiddleName = "Aleksandrovna",
                        ContactEmail = "zotova@test.com",
                        ContactTelephone = "+380671254534",
                        DirectorFirstName = "Ludmila",
                        DirectorLastName = "Zikina",
                        DirectorMiddleName = "Vladimirovna",
                        //NegativeRemarksSearch = "Partial delivery and bad character, Contract #1235",
                        CityID = 4,
                        BankID = 2,
                        SpecializationID = 1
                    },
                    new Supplier
                    {
                        Name = "LLC Mix",
                        StreetBuilding = "Lunina st., 125",
                        PostalIndex = "87510",
                        LegalCode = "19356722",
                        BankAccount = "UA493351060000000260083056751",
                        CompanyTelephone = "+380771234534",
                        CompanyEmail = "mix@test.com",
                        ContactFirstName = "Marina",
                        ContactLastName = "Petrova",
                        ContactMiddleName = "Petrovna",
                        ContactEmail = "petova@test.com",
                        ContactTelephone = "+380671234537",
                        DirectorFirstName = "Fedor",
                        DirectorLastName = "Dotov",
                        DirectorMiddleName = "Ivanovich",
                        //NegativeRemarksSearch = "Partial delivery and bad character, Contract #1235",
                        CityID = 1,
                        BankID = 2,
                        SpecializationID = 1
                    },
                    new Supplier
                    {
                        Name = "LLC FoodMix with long long long long long long long long long long long long long long long name",
                        StreetBuilding = "Sobornaya av., 12",
                        PostalIndex = "84312",
                        LegalCode = "18936504",
                        BankAccount = "UA091300465000002603930071301",
                        CompanyTelephone = "+380961235456",
                        CompanyEmail = "foodmix@test.com",
                        ContactFirstName = "Ivan",
                        ContactLastName = "Todorov",
                        ContactMiddleName = "Alekseevich",
                        ContactEmail = "todorov@test.com",
                        ContactTelephone = "+380631234538",
                        DirectorFirstName = "Anton",
                        DirectorLastName = "Mashkov",
                        DirectorMiddleName = "Petrovich",
                        //NegativeRemarksSearch = "Partial delivery and bad character, Contract #1235",
                        CityID = 4,
                        BankID = 1,
                        SpecializationID = 3
                    },
                    new Supplier
                    {
                        Name = "LLC Soyuz",
                        StreetBuilding = "Italyanscaya av., 17",
                        PostalIndex = "12353",
                        LegalCode = "16723673",
                        BankAccount = "UA403005280000026002301332143",
                        CompanyTelephone = "+380961214456",
                        CompanyEmail = "soyuz@test.com",
                        ContactFirstName = "Ivan",
                        ContactLastName = "Durov",
                        ContactMiddleName = "Petrovich",
                        ContactEmail = "durov@test.com",
                        //ContactTelephone = "+380631234538",
                        DirectorFirstName = "Nikita",
                        DirectorLastName = "Timoshenko",
                        DirectorMiddleName = "Aleksandrovich",
                        //NegativeRemarksSearch = "Partial delivery and bad character, Contract #1235",
                        CityID = 2,
                        BankID = 3,
                        SpecializationID = 5
                    },
                    new Supplier
                    {
                        Name = "LLC Lavanda",
                        StreetBuilding = "Kuybisheva av., 54",
                        PostalIndex = "86340",
                        LegalCode = "22156891",
                        BankAccount = "UA813355480000026005053614097",
                        CompanyTelephone = "+380991234456",
                        CompanyEmail = "lavanda@test.com",
                        ContactFirstName = "Ivan",
                        ContactLastName = "Maksimov",
                        ContactMiddleName = "Vladimirovich",
                        ContactEmail = "maksimovivan@test.com",
                        //ContactTelephone = "+380631234538",
                        DirectorFirstName = "Helena",
                        DirectorLastName = "Marinina",
                        DirectorMiddleName = "Victorovna",
                        NegativeRemarks = "partial delivery and bad character, contract #1335",
                        CityID = 5,
                        BankID = 4,
                        SpecializationID = 4
                    },
                    new Supplier
                    {
                        Name = "IE Motova Natalia Vasilievna",
                        StreetBuilding = "Moskovskaya st., 123",
                        PostalIndex = "44755",
                        LegalCode = "5322366980",
                        BankAccount = "UA243223130000026009000000261",
                        CompanyTelephone = "+380961214452",
                        CompanyEmail = "motova@test.com",
                        //ContactFirstName = "Ivan",
                        //ContactLastName = "Maksimov",
                        //ContactMiddleName = "Vladimirovich",
                        //ContactEmail = "maksimovivan@test.com",
                        //ContactTelephone = "+380631234538",
                        DirectorFirstName = "Nataliya",
                        DirectorLastName = "Motova",
                        DirectorMiddleName = "Vasiliyevna",
                        //NegativeRemarksSearch = "partial delivery and bad character, contract #1335",
                        CityID = 3,
                        BankID = 5,
                        SpecializationID = 2
                    },
                    new Supplier
                    {
                        Name = "LLC PARALELL",
                        StreetBuilding = "Zelinskaya st, 12",
                        PostalIndex = "54321",
                        LegalCode = "22356120",
                        BankAccount = "UA443223130000026005000000522",
                        CompanyTelephone = "+380952234456",
                        CompanyEmail = "paralell@test.com",
                        ContactFirstName = "Tatyana",
                        ContactLastName = "Yotova",
                        ContactMiddleName = "Aleksandrovna",
                        ContactEmail = "yotova@test.com",
                        ContactTelephone = "+380951254534",
                        DirectorFirstName = "Valeria",
                        DirectorLastName = "Shulga",
                        DirectorMiddleName = "Vladimirovna",
                        //NegativeRemarksSearch = "partial delivery and bad character, contract #1335",
                        CityID = 4,
                        BankID = 2,
                        SpecializationID = 1
                    }
                    );
                context.SaveChanges();

                context.Branches.AddRange(
                    new Branch
                    {
                        Name = "Mariupolskiy branch with long long long long long long long long long long long long long name",
                        StreetBuilding = "Latsheva st,12",
                        Code = "MAR",
                        PostalIndex = "87500",
                        BranchEmail = "mariupol@test.com",
                        BranchTelephone = "+380671234531",
                        HeadFirstName = "Helena",
                        HeadLastName = "Khorkina",
                        HeadMiddleName = "Petrovna",
                        HeadTelephone = "+380671234531",
                        HeadEmail = "khorkina@test.com",
                        CityID = 1
                    },
                    new Branch
                    {
                        Name = "Donetskiy",
                        Code = "DON",
                        StreetBuilding = "Gurova st,12",
                        BranchEmail = "donetsk@test.com",
                        BranchTelephone = "+380671234531",
                        PostalIndex = "86300",
                        HeadFirstName = "Irina",
                        HeadLastName = "Sidorova",
                        HeadMiddleName = "Ivanovna",
                        HeadTelephone = "+380961234534",
                        HeadEmail = "sidorova@test.com",
                        CityID = 3
                    },
                    new Branch
                    {
                        Code = "KRAM",
                        Name = "Kramatorskiy",
                        StreetBuilding = "1 Maya st, 1",
                        PostalIndex = "84300",
                        BranchEmail = "kramatorsk@test.com",
                        BranchTelephone = "+380671234531",
                        HeadFirstName = "Viktoriya",
                        HeadLastName = "Mokina",
                        HeadMiddleName = "Alekseevna",
                        HeadTelephone = "+380671284536",
                        HeadEmail = "mokina@test.com",
                        CityID = 1
                    },
                    new Branch
                    {
                        Name = "Kharkovskiy",
                        Code = "KHAR",
                        StreetBuilding = "Letniy av, 34",
                        PostalIndex = "61000",
                        BranchEmail = "kharkiv@test.com",
                        BranchTelephone = "+380671234531",
                        HeadFirstName = "Miron",
                        HeadLastName = "Shahov",
                        HeadMiddleName = "Stepanovich",
                        HeadTelephone = "+380681234538",
                        HeadEmail = "shahov@test.com",
                        CityID = 4
                    },
                    new Branch
                    {
                        Name = "Makeevskiy",
                        Code = "MAK",
                        StreetBuilding = "Mirniy av., 12",
                        PostalIndex = "86100",
                        BranchEmail = "makeevka@test.com",
                        BranchTelephone = "+380671234531",
                        HeadFirstName = "Nikolay",
                        HeadLastName = "Titov",
                        HeadMiddleName = "Arnoldovich",
                        HeadTelephone = "+380671934537",
                        HeadEmail = "titov@test.com",
                        CityID = 5
                    },
                    new Branch
                    {
                        Name = "Chernigovskiy",
                        Code = "CHER",
                        StreetBuilding = "Mirniy av., 12",
                        PostalIndex = "14000",
                        BranchEmail = "chernigov@test.com",
                        BranchTelephone = "+380671234531",
                        HeadFirstName = "Nikolay",
                        HeadLastName = "Nikolayev",
                        HeadMiddleName = "Arnoldovich",
                        HeadTelephone = "+380671994530",
                        HeadEmail = "nikolaev@test.com",
                        CityID = 6
                    },
                    new Branch
                    {
                        Name = "Lvovskiy",
                        Code = "LVOV",
                        StreetBuilding = "Shecchenko av., 12",
                        PostalIndex = "79000",
                        BranchEmail = "lvov@test.com",
                        BranchTelephone = "+380671234531",
                        HeadFirstName = "Evgeniy",
                        HeadLastName = "Kulikov",
                        HeadMiddleName = "Arnoldovich",
                        HeadTelephone = "+380711994532",
                        HeadEmail = "kulikov@test.com",
                        CityID = 7
                    },
                    new Branch
                    {
                        Name = "Zhitomirskiy",
                        Code = "ZHIT",
                        StreetBuilding = "Franko av., 12",
                        PostalIndex = "10001",
                        BranchEmail = "zhitomir@test.com",
                        BranchTelephone = "+380671234531",
                        HeadFirstName = "Olga",
                        HeadLastName = "Gromova",
                        HeadMiddleName = "Stepanovna",
                        HeadTelephone = "+380712994533",
                        HeadEmail = "gromova@test.com",
                        CityID = 8
                    },
                     new Branch
                     {
                         Name = "Ivanofrankovskiy",
                         Code = "IVAN",
                         StreetBuilding = "Pushkina av., 12",
                         PostalIndex = "76000",
                         BranchEmail = "ivanofrankovsk@test.com",
                         BranchTelephone = "+380671234531",
                         HeadFirstName = "Katerina",
                         HeadLastName = "Semenova",
                         HeadMiddleName = "Semenovna",
                         HeadTelephone = "+380713994543",
                         HeadEmail = "semenova@test.com",
                         CityID = 9
                     },
                     new Branch
                     {
                         Name = "Dneprovskiy",
                         Code = "DNEP",
                         StreetBuilding = "Dostoevskogo av., 46",
                         PostalIndex = "76000",
                         BranchEmail = "dnepr@test.com",
                         BranchTelephone = "+380671234531",
                         HeadFirstName = "Grogoriy",
                         HeadLastName = "Saveliev",
                         HeadMiddleName = "Petrovich",
                         HeadTelephone = "+380714995453",
                         HeadEmail = "saveliev@test.com",
                         CityID = 10
                     }

                    );
                context.SaveChanges();

                context.ContractStatuses.AddRange(
                    new ContractStatus
                    {
                        Name = "Current"
                    },
                    new ContractStatus
                    {
                        Name = "Fulfilled"
                    },
                    new ContractStatus
                    {
                        Name = "Invalid"
                    },
                    new ContractStatus
                    {
                        Name = "Terminated"
                    })
                    ;
                context.SaveChanges();

                context.CurrentContracts.AddRange(
                    new CurrentContract
                    {
                        ContractNumber = "2022/MAR/1",
                        ConclusionDate = DateTime.Parse("2022-12-12"),
                        ExpirationDate = DateTime.Parse("2023-12-31"),
                        Subject = "eniti quos quisquam recusandae id harum",
                        ContractAmount = 100000,
                        IsProlonged = false,
                        IsCourtDispute = false,
                        Remarks = "eniti quos quisquam recusandae id harum incidunt facere, vero nobis? eniti quos quisquam recusandae id harum incidunt facere, vero nobis?",
                        IsGoods = true,
                        ContractStatusID = 4,
                        SupplierID = 1,
                        BranchID = 1
                    },
                    new CurrentContract
                    {
                        ContractNumber = "2023/MAK/1",
                        ConclusionDate = DateTime.Parse("2023-01-02"),
                        ExpirationDate = DateTime.Parse("2023-12-31"),
                        Subject = "eniti quos quisquam recusandae id harum incidunt facere, vero nobis? eniti quos quisquam recusandae id harum incidunt facere, vero nobis?",
                        ContractAmount = 200000,
                        IsProlonged = false,
                        //Remarks = "eniti quos quisquam recusandae id harum incidunt facere, vero nobis? eniti quos quisquam recusandae id harum incidunt facere, vero nobis?",
                        IsCourtDispute = false,
                        IsGoods = false,
                        ContractStatusID = 4,
                        SupplierID = 2,
                        BranchID = 5
                    },
                    new CurrentContract
                    {
                        ContractNumber = "2022/MAR/2",
                        ConclusionDate = DateTime.Parse("2022-11-12"),
                        ExpirationDate = DateTime.Parse("2023-12-31"),
                        Subject = "eniti quos quisquam recusandae id harum incidunt facere",
                        ContractAmount = 2344,
                        IsProlonged = true,
                        //Remarks = "eniti quos quisquam recusandae id harum incidunt facere, vero nobis? eniti quos quisquam recusandae id harum incidunt facere, vero nobis?",
                        IsCourtDispute = true,
                        IsGoods = false,
                        ContractStatusID = 4,
                        SupplierID = 3,
                        BranchID = 1
                    },
                    new CurrentContract
                    {
                        ContractNumber = "2023/DON/1",
                        ConclusionDate = DateTime.Parse("2023-01-02"),
                        ExpirationDate = DateTime.Parse("2023-12-31"),
                        Subject = "eniti quos quisquam",
                        ContractAmount = 2356.98m,
                        IsProlonged = false,
                        IsCourtDispute = false,
                        Remarks = "eniti quos quisquam recusandae id harum incidunt facere, vero nobis? eniti quos quisquam recusandae id harum incidunt facere, vero nobis?",
                        IsGoods = true,
                        ContractStatusID = 4,
                        SupplierID = 4,
                        BranchID = 2
                    },
                    new CurrentContract
                    {
                        ContractNumber = "2022/KRAM/1",
                        ConclusionDate = DateTime.Parse("2022-10-12"),
                        ExpirationDate = DateTime.Parse("2023-12-31"),
                        Subject = "eniti quos quisquam recusandae id harum incidunt facere, vero nobis? eniti quos quisquam recusandae id harum incidunt facere, vero nobis?",
                        ContractAmount = 10000,
                        IsProlonged = true,
                        //Remarks = "eniti quos quisquam recusandae id harum incidunt facere, vero nobis? eniti quos quisquam recusandae id harum incidunt facere, vero nobis?",
                        IsCourtDispute = false,
                        IsGoods = false,
                        ContractStatusID = 4,
                        SupplierID = 4,
                        BranchID = 3
                    },
                    new CurrentContract
                    {
                        ContractNumber = "2021/DON/3",
                        ConclusionDate = DateTime.Parse("2021-01-02"),
                        ExpirationDate = DateTime.Parse("2023-12-31"),
                        Subject = "eniti",
                        ContractAmount = 500123.5m,
                        IsProlonged = true,
                        Remarks = "eniti quos quisquam recusandae id harum incidunt facere, vero nobis? eniti quos quisquam recusandae id harum incidunt facere, vero nobis?",
                        IsCourtDispute = true,
                        IsGoods = false,
                        ContractStatusID = 4,
                        SupplierID = 1,
                        BranchID = 2
                    },
                    new CurrentContract
                    {
                        ContractNumber = "2022/DON/4",
                        ConclusionDate = DateTime.Parse("2022-03-11"),
                        ExpirationDate = DateTime.Parse("2022-12-31"),
                        Subject = "eniti quos quisquam recusandae id harum incidunt facere, vero nobis? eniti quos quisquam recusandae",
                        ContractAmount = 12356.34m,
                        IsProlonged = false,
                        //Remarks = "eniti quos quisquam recusandae id harum incidunt facere, vero nobis? eniti quos quisquam recusandae id harum incidunt facere, vero nobis?",
                        IsCourtDispute = false,
                        IsGoods = true,
                        ContractStatusID = 4,
                        SupplierID = 2,
                        BranchID = 2
                    },
                    new CurrentContract
                    {
                        ContractNumber = "2022/KRAM/2",
                        ConclusionDate = DateTime.Parse("2022-01-12"),
                        ExpirationDate = DateTime.Parse("2022-12-31"),
                        Subject = "eniti quos quisquam recusandae id harum incidunt facere, vero nobis? eniti quos quisquam recusandae id harum incidunt facere, vero nobis?",
                        ContractAmount = 1234444.9m,
                        IsProlonged = false,
                        //Remarks = "eniti quos quisquam recusandae id harum incidunt facere, vero nobis? eniti quos quisquam recusandae id harum incidunt facere, vero nobis?",
                        IsCourtDispute = false,
                        IsGoods = false,
                        ContractStatusID = 4,
                        SupplierID = 3,
                        BranchID = 3
                    },
                    new CurrentContract
                    {
                        ContractNumber = "2022/MAR/10",
                        ConclusionDate = DateTime.Parse("2022-10-12"),
                        ExpirationDate = DateTime.Parse("2023-12-31"),
                        Subject = "eniti quos quisquam recusandae id harum incidunt facere, vero nobis? eniti quos quisquam recusandae id harum incidunt facere, vero nobis?",
                        ContractAmount = 12222.99m,
                        IsProlonged = false,
                        Remarks = "eniti quos quisquam recusandae id harum incidunt facere, vero nobis? eniti quos quisquam recusandae id harum incidunt facere, vero nobis?",
                        IsCourtDispute = true,
                        IsGoods = true,
                        ContractStatusID = 4,
                        SupplierID = 4,
                        BranchID = 1
                    },
                    new CurrentContract
                    {
                        ContractNumber = "2022/MAR/11",
                        ConclusionDate = DateTime.Parse("2022-02-12"),
                        ExpirationDate = DateTime.Parse("2023-12-31"),
                        Subject = "eniti quos quisquam",
                        ContractAmount = 123333.34m,
                        IsProlonged = true,
                        //Remarks = "eniti quos quisquam recusandae id harum incidunt facere, vero nobis? eniti quos quisquam recusandae id harum incidunt facere, vero nobis?",
                        IsCourtDispute = false,
                        IsGoods = false,
                        ContractStatusID = 4,
                        SupplierID = 5,
                        BranchID = 1
                    },
                    new CurrentContract
                    {
                        ContractNumber = "2022/DON/11",
                        ConclusionDate = DateTime.Parse("2022-01-12"),
                        ExpirationDate = DateTime.Parse("2023-12-31"),
                        Subject = "eniti quos quisquam recusandae id harum incidunt facere, vero nobis? eniti quos quisquam recusandae id harum incidunt facere, vero nobis?",
                        ContractAmount = 12222.99m,
                        IsProlonged = true,
                        //Remarks = "eniti quos quisquam recusandae id harum incidunt facere, vero nobis? eniti quos quisquam recusandae id harum incidunt facere, vero nobis?",
                        IsCourtDispute = false,
                        IsGoods = false,
                        ContractStatusID = 4,
                        SupplierID = 6,
                        BranchID = 2
                    },
                    new CurrentContract
                    {
                        ContractNumber = "2022/DON/20",
                        ConclusionDate = DateTime.Parse("2022-01-12"),
                        ExpirationDate = DateTime.Parse("2023-12-31"),
                        Subject = "eniti quos quisquam recusandae id harum incidunt facere, vero nobis? eniti quos quisquam recusandae id harum incidunt facere, vero nobis?",
                        ContractAmount = 12356.34m,
                        IsProlonged = false,
                        Remarks = "eniti quos quisquam recusandae id harum incidunt facere, vero nobis? eniti quos quisquam recusandae id harum incidunt facere, vero nobis?",
                        IsCourtDispute = false,
                        IsGoods = true,
                        ContractStatusID = 4,
                        SupplierID = 6,
                        BranchID = 2
                    },
                    new CurrentContract
                    {
                        ContractNumber = "2022/MAR/5",
                        ConclusionDate = DateTime.Parse("2022-05-12"),
                        ExpirationDate = DateTime.Parse("2023-12-31"),
                        Subject = "eniti quos quisquam recusandae id harum incidunt",
                        ContractAmount = 200000,
                        IsCourtDispute = false,
                        IsProlonged = false,
                        //Remarks = "eniti quos quisquam recusandae id harum incidunt facere, vero nobis? eniti quos quisquam recusandae id harum incidunt facere, vero nobis?",
                        IsGoods = false,
                        ContractStatusID = 4,
                        SupplierID = 2,
                        BranchID = 1
                    },
                    new CurrentContract
                    {
                        ContractNumber = "2020/KHAR/1",
                        ConclusionDate = DateTime.Parse("2020-01-02"),
                        ExpirationDate = DateTime.Parse("2023-12-31"),
                        Subject = "eniti quos quisquam recusandae id harum incidunt facere, vero nobis? eniti quos quisquam recusandae id harum incidunt facere, vero nobis?",
                        ContractAmount = 1234444.9m,
                        IsProlonged = true,
                        Remarks = "eniti quos quisquam recusandae id harum incidunt facere, vero nobis? eniti quos quisquam recusandae id harum incidunt facere, vero nobis?",
                        IsCourtDispute = false,
                        IsGoods = false,
                        ContractStatusID = 4,
                        SupplierID = 3,
                        BranchID = 1
                    },
                    new CurrentContract
                    {
                        ContractNumber = "2022/KHAR/2",
                        ConclusionDate = DateTime.Parse("2022-03-12"),
                        ExpirationDate = DateTime.Parse("2023-12-31"),
                        Subject = "eniti quos quisquam recusandae id harum incidunt facere, vero nobis? eniti quos quisquam recusandae id harum incidunt facere, vero nobis?",
                        ContractAmount = 2356.98m,
                        IsProlonged = false,
                        //Remarks = "eniti quos quisquam recusandae id harum incidunt facere, vero nobis? eniti quos quisquam recusandae id harum incidunt facere, vero nobis?",
                        IsCourtDispute = false,
                        IsGoods = true,
                        ContractStatusID = 4,
                        SupplierID = 4,
                        BranchID = 4
                    },
                    new CurrentContract
                    {
                        ContractNumber = "2021/KHAR/5",
                        ConclusionDate = DateTime.Parse("2021-01-02"),
                        ExpirationDate = DateTime.Parse("2023-12-31"),
                        Subject = "eniti quos quisquam recusandae",
                        ContractAmount = 12356.34m,
                        IsProlonged = true,
                        Remarks = "eniti quos quisquam recusandae id harum incidunt facere, vero nobis? eniti quos quisquam recusandae id harum incidunt facere, vero nobis?",
                        IsCourtDispute = false,
                        IsGoods = false,
                        ContractStatusID = 4,
                        SupplierID = 5,
                        BranchID = 4
                    },
                    new CurrentContract
                    {
                        ContractNumber = "2022/MAK/3",
                        ConclusionDate = DateTime.Parse("2022-06-12"),
                        ExpirationDate = DateTime.Parse("2023-12-31"),
                        Subject = "eniti quos quisquam recusandae id harum incidunt facere, vero nobis? eniti quos quisquam recusandae id harum incidunt facere, vero nobis?",
                        ContractAmount = 12222.99m,
                        IsProlonged = false,
                        Remarks = "eniti quos quisquam recusandae id harum incidunt facere, vero nobis? eniti quos quisquam recusandae id harum incidunt facere, vero nobis?",
                        IsCourtDispute = false,
                        IsGoods = true,
                        ContractStatusID = 4,
                        SupplierID = 1,
                        BranchID = 5
                    },
                    new CurrentContract
                    {
                        ContractNumber = "2022/MAK/10",
                        ConclusionDate = DateTime.Parse("2022-04-12"),
                        ExpirationDate = DateTime.Parse("2023-12-31"),
                        Subject = "eniti quos quisquam recusandae id harum incidunt facere, vero nobis? eniti quos quisquam recusandae id harum incidunt facere, vero nobis?",
                        ContractAmount = 200000,
                        IsProlonged = false,
                        //Remarks = "eniti quos quisquam recusandae id harum incidunt facere, vero nobis? eniti quos quisquam recusandae id harum incidunt facere, vero nobis?",
                        IsCourtDispute = true,
                        IsGoods = true,
                        ContractStatusID = 4,
                        SupplierID = 2,
                        BranchID = 5
                    },
                    new CurrentContract
                    {
                        ContractNumber = "2022/MAR/45",
                        ConclusionDate = DateTime.Parse("2022-01-12"),
                        ExpirationDate = DateTime.Parse("2023-12-31"),
                        Subject = "eniti quos quisquam recusandae id harum incidunt facere, vero nobis?",
                        ContractAmount = 12356.34m,
                        IsProlonged = false,
                        //Remarks = "eniti quos quisquam recusandae id harum incidunt facere, vero nobis? eniti quos quisquam recusandae id harum incidunt facere, vero nobis?",
                        IsCourtDispute = false,
                        IsGoods = false,
                        ContractStatusID = 4,
                        SupplierID = 2,
                        BranchID = 1
                    },
                    new CurrentContract
                    {
                        ContractNumber = "2022/DON/110",
                        ConclusionDate = DateTime.Parse("2022-01-02"),
                        ExpirationDate = DateTime.Parse("2023-12-31"),
                        Subject = "eniti quos quisquam recusandae id harum incidunt facere, vero nobis? eniti quos quisquam recusandae id harum incidunt facere, vero nobis?",
                        ContractAmount = 12222.99m,
                        IsProlonged = false,
                        //Remarks = "eniti quos quisquam recusandae id harum incidunt facere, vero nobis? eniti quos quisquam recusandae id harum incidunt facere, vero nobis?",
                        IsCourtDispute = false,
                        IsGoods = true,
                        ContractStatusID = 4,
                        SupplierID = 4,
                        BranchID = 2
                    }
                    );
                context.SaveChanges();

                #region Current Events
                context.CurrentEvents.AddRange(
                    new CurrentEvent
                    {
                        Title = "Meeting",
                        Start = DateTime.Parse("2023-01-25 11:05:14"),
                        End = DateTime.Parse("2023-01-25 12:05:14"),
                        Description = "Meeting with a supplier",
                        AllDay = false,
                        CurrentContractID = 1,
                    },
                    new CurrentEvent
                    {
                        Title = "Payment",
                        Start = DateTime.Parse("2023-02-09 00:00:00"),
                        End = DateTime.Parse("2023-02-10 00:00:00"),
                        Description = "Payment under the contract",
                        AllDay = true,
                        CurrentContractID = 1,
                    },
                    new CurrentEvent
                    {
                        Title = "Payment",
                        Start = DateTime.Parse("2023-02-20 00:00:00"),
                        End = DateTime.Parse("2023-02-21 00:00:00"),
                        Description = "Payment under the contract",
                        AllDay = true,
                        CurrentContractID = 1,
                    },
                    new CurrentEvent
                    {
                        Title = "Delivery",
                        Start = DateTime.Parse("2023-02-25 00:00:00"),
                        End = DateTime.Parse("2023-02-26 00:00:00"),
                        Description = "Delivery of goods",
                        AllDay = true,
                        CurrentContractID = 1
                    },
                     new CurrentEvent
                     {
                         Title = "Meeting",
                         Start = DateTime.Parse("2023-01-26 11:05:14"),
                         End = DateTime.Parse("2023-01-26 12:05:14"),
                         Description = "Meeting with a supplier",
                         AllDay = false,
                         CurrentContractID = 2,
                     },
                      new CurrentEvent
                      {
                          Title = "Online meeting",
                          Start = DateTime.Parse("2023-01-25 11:05:14"),
                          End = DateTime.Parse("2023-01-26 12:05:14"),
                          Description = "Meeting with a branch staff (Zoom)",
                          AllDay = false,
                          CurrentContractID = 1,
                      },
                      new CurrentEvent
                      {
                          Title = "Report",
                          Start = DateTime.Parse("2023-01-29 10:05:14"),
                          End = DateTime.Parse("2023-01-29 12:05:14"),
                          Description = "Purchase report meeting",
                          AllDay = false,
                          CurrentContractID = 1,
                      },
                      new CurrentEvent
                      {
                          Title = "Meeting",
                          Start = DateTime.Parse("2023-02-25 11:05:14"),
                          End = DateTime.Parse("2023-02-25 12:05:14"),
                          Description = "Meeting with a supplier",
                          AllDay = false,
                          CurrentContractID = 1,
                      },
                      new CurrentEvent
                      {
                          Title = "Meeting",
                          Start = DateTime.Parse("2023-02-05 11:05:14"),
                          End = DateTime.Parse("2023-02-06 12:05:14"),
                          Description = "Meeting with a supplier",
                          AllDay = false,
                          CurrentContractID = 1,
                      },
                      new CurrentEvent
                      {
                          Title = "Online meeting",
                          Start = DateTime.Parse("2023-02-03 19:05:14"),
                          End = DateTime.Parse("2023-02-03 20:05:14"),
                          Description = "Online staff meeting (Skype)",
                          AllDay = false,
                          CurrentContractID = 1,
                      },
                      new CurrentEvent
                      {
                          Title = "Meeting",
                          Start = DateTime.Parse("2023-02-03 11:05:14"),
                          End = DateTime.Parse("2023-02-03 12:05:14"),
                          Description = "Meeting with a supplier",
                          AllDay = false,
                          CurrentContractID = 1,
                      });
                #endregion

                #region Past Events
                context.PastEvents.AddRange(
                    new PastEvent
                    {
                        Title = "Meeting",
                        Start = DateTime.Parse("2023-01-25 11:05:14"),
                        End = DateTime.Parse("2023-01-25 12:05:14"),
                        Description = "Meeting with a supplier",
                        AllDay = false,
                        PastContractID = 1,
                    },
                    new PastEvent
                    {
                        Title = "Delivery",
                        Start = DateTime.Parse("2023-02-09 00:00:00"),
                        End = DateTime.Parse("2023-02-10 00:00:00"),
                        Description = "Partial delivery (20%)",
                        AllDay = true,
                        PastContractID = 1,
                    },
                    new PastEvent
                    {
                        Title = "Trip",
                        Start = DateTime.Parse("2022-12-20 00:00:00"),
                        End = DateTime.Parse("2022-12-21 00:00:00"),
                        Description = "Trip to a supplier factory",
                        AllDay = true,
                        PastContractID = 1,
                    },
                    new PastEvent
                    {
                        Title = "Delivery",
                        Start = DateTime.Parse("2022-12-25 00:00:00"),
                        End = DateTime.Parse("2022-12-26 00:00:00"),
                        Description = "Partial delivery",
                        AllDay = true,
                        PastContractID = 1
                    },
                     new PastEvent
                     {
                         Title = "Meeting",
                         Start = DateTime.Parse("2022-01-26 11:05:14"),
                         End = DateTime.Parse("2022-01-26 12:05:14"),
                         Description = "Meeting with a supplier",
                         AllDay = false,
                         PastContractID = 2,
                     },
                      new PastEvent
                      {
                          Title = "Delivery",
                          Start = DateTime.Parse("2022-12-25 11:05:14"),
                          End = DateTime.Parse("2022-12-26 12:05:14"),
                          Description = "Partial delivery (50%)",
                          AllDay = false,
                          PastContractID = 1,
                      },
                      new PastEvent
                      {
                          Title = "Meeting",
                          Start = DateTime.Parse("2022-12-29 10:05:14"),
                          End = DateTime.Parse("2022-12-29 12:05:14"),
                          Description = "Meeting with a supplier",
                          AllDay = false,
                          PastContractID = 3,
                      },
                      new PastEvent
                      {
                          Title = "Staff meeting",
                          Start = DateTime.Parse("2022-12-25 11:05:14"),
                          End = DateTime.Parse("2022-12-25 12:05:14"),
                          Description = "Online staff meeting (Zoom)",
                          AllDay = false,
                          PastContractID = 1,
                      },
                      new PastEvent
                      {
                          Title = "Delivery",
                          Start = DateTime.Parse("2022-12-05 11:05:14"),
                          End = DateTime.Parse("2022-12-06 12:05:14"),
                          Description = "Partial delivery (50%)",
                          AllDay = false,
                          PastContractID = 1,
                      },
                      new PastEvent
                      {
                          Title = "Meeting",
                          Start = DateTime.Parse("2022-11-25 11:05:14"),
                          End = DateTime.Parse("2022-11-25 12:05:14"),
                          Description = "Purchase report meeting",
                          AllDay = false,
                          PastContractID = 1,
                      },
                      new PastEvent
                      {
                          Title = "Trip",
                          Start = DateTime.Parse("2022-11-13 00:00:00"),
                          End = DateTime.Parse("2022-11-14 00:00:00"),
                          Description = "Trip to supplier factory",
                          AllDay = true,
                          PastContractID = 1,
                      });
                #endregion

                #region CurrentDocuments


                //context.CurrentDocuments.AddRange(
                //    new CurrentDocument
                //    {
                //        Name = "Contact.pdf",
                //        PathToDocument = "path/path",
                //        DateOfUploading = DateTime.Parse("2022-12-12"),
                //        CurrentContractID = 1
                //    },
                //    new CurrentDocument
                //    {
                //        Name = "Contact1.pdf",
                //        PathToDocument = "path/path",
                //        DateOfUploading = DateTime.Parse("2023-01-02"),
                //        CurrentContractID = 2
                //    },
                //    new CurrentDocument
                //    {
                //        Name = "Contact2.pdf",
                //        PathToDocument = "path/path",
                //        DateOfUploading = DateTime.Parse("2022-11-12"),
                //        CurrentContractID = 3
                //    },
                //    new CurrentDocument
                //    {
                //        Name = "Contact3.pdf",
                //        PathToDocument = "path/path",
                //        DateOfUploading = DateTime.Parse("2023-01-02"),
                //        CurrentContractID = 4
                //    },
                //    new CurrentDocument
                //    {
                //        Name = "Contact4.pdf",
                //        PathToDocument = "path/path",
                //        DateOfUploading = DateTime.Parse("2022-10-12"),
                //        CurrentContractID = 5
                //    },
                //    new CurrentDocument
                //    {
                //        Name = "Contact5.pdf",
                //        PathToDocument = "path/path",
                //        DateOfUploading = DateTime.Parse("2021-01-02"),
                //        CurrentContractID = 6
                //    },
                //    new CurrentDocument
                //    {
                //        Name = "Contact6.pdf",
                //        PathToDocument = "path/path",
                //        DateOfUploading = DateTime.Parse("2022-03-11"),
                //        CurrentContractID = 7
                //    },
                //    new CurrentDocument
                //    {
                //        Name = "Contact7.pdf",
                //        PathToDocument = "path/path",
                //        DateOfUploading = DateTime.Parse("2022-01-12"),
                //        CurrentContractID = 8
                //    },
                //    new CurrentDocument
                //    {
                //        Name = "Contact8.pdf",
                //        PathToDocument = "path/path",
                //        DateOfUploading = DateTime.Parse("2022-10-12"),
                //        CurrentContractID = 9
                //    },
                //    new CurrentDocument
                //    {
                //        Name = "Contact9.pdf",
                //        PathToDocument = "path/path",
                //        DateOfUploading = DateTime.Parse("2022-02-12"),
                //        CurrentContractID = 10
                //    },
                //    new CurrentDocument
                //    {
                //        Name = "Contact10.pdf",
                //        PathToDocument = "path/path",
                //        DateOfUploading = DateTime.Parse("2022-01-12"),
                //        CurrentContractID = 11
                //    },
                //    new CurrentDocument
                //    {
                //        Name = "Contact11.pdf",
                //        PathToDocument = "path/path",
                //        DateOfUploading = DateTime.Parse("2022-01-12"),
                //        CurrentContractID = 12
                //    },
                //    new CurrentDocument
                //    {
                //        Name = "Contact12.pdf",
                //        PathToDocument = "path/path",
                //        DateOfUploading = DateTime.Parse("2022-05-12"),
                //        CurrentContractID = 13
                //    },
                //    new CurrentDocument
                //    {
                //        Name = "Contact13.pdf",
                //        PathToDocument = "path/path",
                //        DateOfUploading = DateTime.Parse("2020-01-02"),
                //        CurrentContractID = 14
                //    },
                //    new CurrentDocument
                //    {
                //        Name = "Contact14.pdf",
                //        PathToDocument = "path/path",
                //        DateOfUploading = DateTime.Parse("2022-03-12"),
                //        CurrentContractID = 15
                //    },
                //    new CurrentDocument
                //    {
                //        Name = "Contact15.pdf",
                //        PathToDocument = "path/path",
                //        DateOfUploading = DateTime.Parse("2021-01-02"),
                //        CurrentContractID = 16
                //    },
                //    new CurrentDocument
                //    {
                //        Name = "Contact16.pdf",
                //        PathToDocument = "path/path",
                //        DateOfUploading = DateTime.Parse("2022-06-12"),
                //        CurrentContractID = 17
                //    },
                //    new CurrentDocument
                //    {
                //        Name = "Contact17.pdf",
                //        PathToDocument = "path/path",
                //        DateOfUploading = DateTime.Parse("2022-04-12"),
                //        CurrentContractID = 18
                //    },
                //    new CurrentDocument
                //    {
                //        Name = "Contact18.pdf",
                //        PathToDocument = "path/path",
                //        DateOfUploading = DateTime.Parse("2022-01-12"),
                //        CurrentContractID = 19
                //    },
                //    new CurrentDocument
                //    {
                //        Name = "Contact19.pdf",
                //        PathToDocument = "path/path",
                //        DateOfUploading = DateTime.Parse("2022-01-02"),
                //        CurrentContractID = 20
                //    },
                //    new CurrentDocument
                //    {
                //        Name = "Amed1.pdf",
                //        PathToDocument = "path/path",
                //        DateOfUploading = DateTime.Parse("2022-12-31"),
                //        CurrentContractID = 3
                //    },
                //    new CurrentDocument
                //    {
                //        Name = "Amed2.pdf",
                //        PathToDocument = "path/path",
                //        DateOfUploading = DateTime.Parse("2022-12-31"),
                //        CurrentContractID = 5
                //    },
                //    new CurrentDocument
                //    {
                //        Name = "Amed3.pdf",
                //        PathToDocument = "path/path",
                //        DateOfUploading = DateTime.Parse("2022-12-31"),
                //        CurrentContractID = 6
                //    },
                //    new CurrentDocument
                //    {
                //        Name = "Amed4.pdf",
                //        PathToDocument = "path/path",
                //        DateOfUploading = DateTime.Parse("2022-12-31"),
                //        CurrentContractID = 10
                //    }
                //    );
                //context.SaveChanges(); 
                #endregion

                context.PastContracts.AddRange(
                    new PastContract
                    {
                        ContractNumber = "2021/MAR/101",
                        ConclusionDate = DateTime.Parse("2021-12-12"),
                        ExpirationDate = DateTime.Parse("2022-12-31"),
                        Subject = "eniti quos quisquam recusandae id harum incidunt facere, vero nobis? eniti quos quisquam recusandae id harum incidunt facere, vero nobis?",
                        ContractAmount = 100000,
                        IsProlonged = true,

                        Remarks = "eniti quos quisquam recusandae id harum incidunt facere, vero nobis? eniti quos quisquam recusandae id harum incidunt facere, vero nobis?",
                        IsGoods = true,
                        TransitionDate = DateTime.Parse("2022-12-31"),
                        SupplierID = 1,
                        BranchID = 1,
                        ContractStatusID = 2
                    },
                    new PastContract
                    {
                        ContractNumber = "2020/MAK/101",
                        ConclusionDate = DateTime.Parse("2020-01-02"),
                        ExpirationDate = DateTime.Parse("2021-12-31"),
                        Subject = "eniti quos quisquam recusandae id harum incidunt facere, vero nobis? eniti quos quisquam recusandae id harum incidunt facere, vero nobis?",
                        ContractAmount = 200000,
                        IsProlonged = false,
                        //Remarks = "eniti quos quisquam recusandae id harum incidunt facere, vero nobis? eniti quos quisquam recusandae id harum incidunt facere, vero nobis?",
                        IsGoods = false,
                        TransitionDate = DateTime.Parse("2022-12-31"),
                        SupplierID = 1,
                        BranchID = 2,
                        ContractStatusID = 2
                    },
                    new PastContract
                    {
                        ContractNumber = "2022/MAR/202",
                        ConclusionDate = DateTime.Parse("2022-11-12"),
                        ExpirationDate = DateTime.Parse("2023-12-31"),
                        Subject = "eniti quos quisquam recusandae id harum incidunt facere, vero nobis? eniti quos quisquam recusandae id harum incidunt facere, vero nobis?",
                        ContractAmount = 2344,
                        IsProlonged = true,
                        //Remarks = "eniti quos quisquam recusandae id harum incidunt facere, vero nobis? eniti quos quisquam recusandae id harum incidunt facere, vero nobis?",
                        IsGoods = false,
                        TransitionDate = DateTime.Parse("2023-01-01"),
                        SupplierID = 1,
                        BranchID = 3,
                        ContractStatusID = 3
                    },
                    new PastContract
                    {
                        ContractNumber = "2021/DON/101",
                        ConclusionDate = DateTime.Parse("2021-01-02"),
                        ExpirationDate = DateTime.Parse("2022-12-31"),
                        Subject = "eniti quos quisquam recusandae id harum incidunt facere, vero nobis? eniti quos quisquam recusandae id harum incidunt facere, vero nobis?",
                        ContractAmount = 2356.98m,
                        IsProlonged = false,
                        Remarks = "eniti quos quisquam recusandae id harum incidunt facere, vero nobis? eniti quos quisquam recusandae id harum incidunt facere, vero nobis?",
                        IsGoods = true,
                        TransitionDate = DateTime.Parse("2022-06-01"),
                        SupplierID = 1,
                        BranchID = 4,
                        ContractStatusID = 3
                    },
                    new PastContract
                    {
                        ContractNumber = "2022/KRAM/101",
                        ConclusionDate = DateTime.Parse("2020-10-12"),
                        ExpirationDate = DateTime.Parse("2021-12-31"),
                        Subject = "eniti quos quisquam recusandae id harum incidunt facere, vero nobis? eniti quos quisquam recusandae id harum incidunt facere, vero nobis?",
                        ContractAmount = 100000,
                        IsProlonged = true,
                        Remarks = "eniti quos quisquam recusandae id harum incidunt facere, vero nobis? eniti quos quisquam recusandae id harum incidunt facere, vero nobis?",
                        IsGoods = false,
                        TransitionDate = DateTime.Parse("2021-01-01"),
                        SupplierID = 1,
                        BranchID = 4,
                        ContractStatusID = 1
                    }
                    );
                context.SaveChanges();
                #region PastDocuments
                //context.PastDocuments.AddRange(
                //             new PastDocument
                //             {
                //                 Name = "Contact.pdf",
                //                 PathToDocument = "path/path",
                //                 DateOfUploading = DateTime.Parse("2021-12-12"),
                //                 PastContractID = 1
                //             },
                //             new PastDocument
                //             {
                //                 Name = "Contact.pdf",
                //                 PathToDocument = "path/path",
                //                 DateOfUploading = DateTime.Parse("2020-01-02"),
                //                 PastContractID = 2
                //             },
                //             new PastDocument
                //             {
                //                 Name = "Contact.pdf",
                //                 PathToDocument = "path/path",
                //                 DateOfUploading = DateTime.Parse("2022-11-12"),
                //                 PastContractID = 3
                //             },
                //             new PastDocument
                //             {
                //                 Name = "Contact.pdf",
                //                 PathToDocument = "path/path",
                //                 DateOfUploading = DateTime.Parse("2021-01-02"),
                //                 PastContractID = 4
                //             },
                //             new PastDocument
                //             {
                //                 Name = "Contact.pdf",
                //                 PathToDocument = "path/path",
                //                 DateOfUploading = DateTime.Parse("2020-10-12"),
                //                 PastContractID = 5
                //             },
                //             new PastDocument
                //             {
                //                 Name = "Amed1.pdf",
                //                 PathToDocument = "path/path",
                //                 DateOfUploading = DateTime.Parse("2022-01-01"),
                //                 PastContractID = 1
                //             },
                //            new PastDocument
                //            {
                //                Name = "Amed1.pdf",
                //                PathToDocument = "path/path",
                //                DateOfUploading = DateTime.Parse("2022-12-12"),
                //                PastContractID = 3
                //            },
                //            new PastDocument
                //            {
                //                Name = "Amed1.pdf",
                //                PathToDocument = "path/path",
                //                DateOfUploading = DateTime.Parse("2022-12-12"),
                //                PastContractID = 5
                //            }
                //           );
                //context.SaveChanges(); 
                #endregion
            }
        }
    }
}
