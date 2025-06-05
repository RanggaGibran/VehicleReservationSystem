using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VehicleReservationSystem.Models;

namespace VehicleReservationSystem.Data
{
    public static class DbInitializer
    {        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<AppDbContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            
            // Ensure database exists but don't migrate in production initialization
            context.Database.EnsureCreated();
            
            string[] roleNames = { "Admin", "Approver", "User" };
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Create admin user if it doesn't exist
            var adminUser = new ApplicationUser
            {
                UserName = "admin@example.com",
                Email = "admin@example.com",
                EmailConfirmed = true,
                FullName = "System Administrator",
                Position = "Administrator",
                Department = "IT",
                SupervisorId = null
            };

            if (await userManager.FindByEmailAsync(adminUser.Email) == null)
            {
                var result = await userManager.CreateAsync(adminUser, "Admin123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
            var approver1 = new ApplicationUser
            {
                UserName = "approver1@example.com",
                Email = "approver1@example.com",
                EmailConfirmed = true,
                FullName = "Approver Level 1",
                Position = "Supervisor",
                Department = "Operations"
            };

            if (await userManager.FindByEmailAsync(approver1.Email) == null)
            {
                var result = await userManager.CreateAsync(approver1, "Approver123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(approver1, "Approver");
                }
            }

            var approver2 = new ApplicationUser
            {
                UserName = "approver2@example.com",
                Email = "approver2@example.com",
                EmailConfirmed = true,
                FullName = "Approver Level 2",
                Position = "Manager",
                Department = "Operations"
            };

            if (await userManager.FindByEmailAsync(approver2.Email) == null)
            {
                var result = await userManager.CreateAsync(approver2, "Approver123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(approver2, "Approver");
                }
            }            // Seed Locations if not already seeded
            if (!context.Locations.Any())
            {
                var locations = new List<Location>
                {
                    // Kantor Pusat
                    new Location 
                    { 
                        Name = "Kantor Pusat Jakarta", 
                        Type = "HeadOffice", 
                        Address = "Jl. Sudirman No. 123, Jakarta Pusat", 
                        Region = "DKI Jakarta",
                        ContactPerson = "Ir. Budi Santoso",
                        PhoneNumber = "+62-21-1234567",
                        IsActive = true
                    },
                    
                    // Kantor Cabang
                    new Location 
                    { 
                        Name = "Kantor Cabang Surabaya", 
                        Type = "BranchOffice", 
                        Address = "Jl. Basuki Rahmat No. 456, Surabaya", 
                        Region = "Jawa Timur",
                        ContactPerson = "Dra. Siti Aminah",
                        PhoneNumber = "+62-31-2345678",
                        IsActive = true
                    },
                    
                    // 6 Lokasi Tambang
                    new Location 
                    { 
                        Name = "Tambang Nikel Sulawesi Tengah - Site A", 
                        Type = "MiningLocation", 
                        Address = "Desa Tambang Raya, Kec. Petasia, Morowali Utara", 
                        Region = "Sulawesi Tengah",
                        MineCode = "NI-SULUT-A",
                        ContactPerson = "Ir. Ahmad Zulkarnain",
                        PhoneNumber = "+62-464-123456",
                        IsActive = true
                    },
                    new Location 
                    { 
                        Name = "Tambang Nikel Sulawesi Tengah - Site B", 
                        Type = "MiningLocation", 
                        Address = "Desa Bahodopi, Kec. Bungku Timur, Morowali", 
                        Region = "Sulawesi Tengah",
                        MineCode = "NI-SULUT-B",
                        ContactPerson = "Ir. Maya Sari",
                        PhoneNumber = "+62-464-234567",
                        IsActive = true
                    },
                    new Location 
                    { 
                        Name = "Tambang Nikel Sulawesi Tenggara - Site C", 
                        Type = "MiningLocation", 
                        Address = "Desa Pomalaa, Kec. Pomalaa, Kolaka", 
                        Region = "Sulawesi Tenggara",
                        MineCode = "NI-SULTRA-C",
                        ContactPerson = "Ir. Hendra Wijaya",
                        PhoneNumber = "+62-405-345678",
                        IsActive = true
                    },
                    new Location 
                    { 
                        Name = "Tambang Nikel Maluku Utara - Site D", 
                        Type = "MiningLocation", 
                        Address = "Desa Weda, Kec. Weda Tengah, Halmahera Tengah", 
                        Region = "Maluku Utara",
                        MineCode = "NI-MALUT-D",
                        ContactPerson = "Ir. Fatimah Zahra",
                        PhoneNumber = "+62-924-456789",
                        IsActive = true
                    },
                    new Location 
                    { 
                        Name = "Tambang Nikel Papua - Site E", 
                        Type = "MiningLocation", 
                        Address = "Desa Timika, Kec. Mimika Baru, Mimika", 
                        Region = "Papua Tengah",
                        MineCode = "NI-PAPUA-E",
                        ContactPerson = "Ir. Yohanes Kogoya",
                        PhoneNumber = "+62-901-567890",
                        IsActive = true
                    },
                    new Location 
                    { 
                        Name = "Tambang Nikel Kalimantan Timur - Site F", 
                        Type = "MiningLocation", 
                        Address = "Desa Sangkulirang, Kec. Sangkulirang, Kutai Timur", 
                        Region = "Kalimantan Timur",
                        MineCode = "NI-KALTIM-F",
                        ContactPerson = "Ir. Bambang Sutrisno",
                        PhoneNumber = "+62-549-678901",
                        IsActive = true
                    }
                };

                context.Locations.AddRange(locations);
                await context.SaveChangesAsync();
            }

            // Seed VehicleTypes for mining company if not already seeded
            if (!context.VehicleTypes.Any())
            {
                var vehicleTypes = new List<VehicleType>
                {
                    // Kendaraan Angkutan Orang
                    new VehicleType 
                    { 
                        Name = "Sedan Executive", 
                        Description = "Kendaraan sedan untuk executive dan tamu", 
                        Category = "Passenger", 
                        Capacity = 5,
                        LoadCapacity = 0,
                        RequiresSpecialLicense = false,
                        FuelType = "Bensin",
                        AverageFuelConsumption = 12.5m
                    },
                    new VehicleType 
                    { 
                        Name = "SUV", 
                        Description = "Sport Utility Vehicle untuk medan berat", 
                        Category = "Passenger", 
                        Capacity = 7,
                        LoadCapacity = 0.5m,
                        RequiresSpecialLicense = false,
                        FuelType = "Solar",
                        AverageFuelConsumption = 15.0m
                    },
                    new VehicleType 
                    { 
                        Name = "Minibus", 
                        Description = "Minibus untuk transportasi karyawan", 
                        Category = "Passenger", 
                        Capacity = 15,
                        LoadCapacity = 0,
                        RequiresSpecialLicense = false,
                        FuelType = "Solar",
                        AverageFuelConsumption = 18.0m
                    },
                    new VehicleType 
                    { 
                        Name = "Bus Medium", 
                        Description = "Bus medium untuk transportasi karyawan ke tambang", 
                        Category = "Passenger", 
                        Capacity = 30,
                        LoadCapacity = 0,
                        RequiresSpecialLicense = true,
                        FuelType = "Solar",
                        AverageFuelConsumption = 25.0m
                    },
                    new VehicleType 
                    { 
                        Name = "Bus Besar", 
                        Description = "Bus besar untuk transportasi massal karyawan", 
                        Category = "Passenger", 
                        Capacity = 50,
                        LoadCapacity = 0,
                        RequiresSpecialLicense = true,
                        FuelType = "Solar",
                        AverageFuelConsumption = 35.0m
                    },
                    
                    // Kendaraan Angkutan Barang
                    new VehicleType 
                    { 
                        Name = "Pickup Single Cabin", 
                        Description = "Pickup untuk transportasi barang ringan", 
                        Category = "Cargo", 
                        Capacity = 3,
                        LoadCapacity = 1.0m,
                        RequiresSpecialLicense = false,
                        FuelType = "Solar",
                        AverageFuelConsumption = 14.0m
                    },
                    new VehicleType 
                    { 
                        Name = "Pickup Double Cabin", 
                        Description = "Pickup double cabin untuk personel dan barang", 
                        Category = "Cargo", 
                        Capacity = 5,
                        LoadCapacity = 1.5m,
                        RequiresSpecialLicense = false,
                        FuelType = "Solar",
                        AverageFuelConsumption = 16.0m
                    },
                    new VehicleType 
                    { 
                        Name = "Truk Colt Diesel", 
                        Description = "Truk ringan untuk distribusi supplies", 
                        Category = "Cargo", 
                        Capacity = 3,
                        LoadCapacity = 3.0m,
                        RequiresSpecialLicense = false,
                        FuelType = "Solar",
                        AverageFuelConsumption = 20.0m
                    },
                    new VehicleType 
                    { 
                        Name = "Truk Fuso", 
                        Description = "Truk medium untuk transportasi equipment", 
                        Category = "Cargo", 
                        Capacity = 3,
                        LoadCapacity = 8.0m,
                        RequiresSpecialLicense = true,
                        FuelType = "Solar",
                        AverageFuelConsumption = 30.0m
                    },
                    new VehicleType 
                    { 
                        Name = "Truk Trailer", 
                        Description = "Truk berat untuk transportasi heavy equipment", 
                        Category = "Cargo", 
                        Capacity = 2,
                        LoadCapacity = 25.0m,
                        RequiresSpecialLicense = true,
                        FuelType = "Solar",
                        AverageFuelConsumption = 45.0m
                    },
                    
                    // Kendaraan Khusus Tambang
                    new VehicleType 
                    { 
                        Name = "Ambulance", 
                        Description = "Kendaraan ambulance untuk emergency", 
                        Category = "Emergency", 
                        Capacity = 2,
                        LoadCapacity = 0,
                        RequiresSpecialLicense = false,
                        FuelType = "Solar",
                        AverageFuelConsumption = 18.0m
                    },
                    new VehicleType 
                    { 
                        Name = "Fire Truck", 
                        Description = "Mobil pemadam kebakaran", 
                        Category = "Emergency", 
                        Capacity = 6,
                        LoadCapacity = 5.0m,
                        RequiresSpecialLicense = true,
                        FuelType = "Solar",
                        AverageFuelConsumption = 40.0m
                    }
                };

                context.VehicleTypes.AddRange(vehicleTypes);
                await context.SaveChangesAsync();
            }            // Seed Drivers if not already seeded
            if (!context.Drivers.Any())
            {
                var locations = await context.Locations.ToListAsync();
                var drivers = new List<Driver>
                {
                    new Driver {
                        Name = "Budi Hartono",
                        LicenseNumber = "DRV-001-JKT",
                        LicenseExpiry = DateTime.Today.AddYears(2),
                        PhoneNumber = "081234567890",
                        Email = "budi.hartono@ptnikel.com",
                        Address = "Jl. Kebon Jeruk No. 45, Jakarta Barat",
                        LocationId = locations.First(l => l.Type == "HeadOffice").Id,
                        IsAvailable = true
                    },
                    new Driver {
                        Name = "Siti Nurhaliza",
                        LicenseNumber = "DRV-002-JKT",
                        LicenseExpiry = DateTime.Today.AddYears(3),
                        PhoneNumber = "082345678901",
                        Email = "siti.nurhaliza@ptnikel.com",
                        Address = "Jl. Mangga Dua No. 78, Jakarta Utara",
                        LocationId = locations.First(l => l.Type == "HeadOffice").Id,
                        IsAvailable = true
                    },
                    new Driver {
                        Name = "Ahmad Fauzi",
                        LicenseNumber = "DRV-003-SBY",
                        LicenseExpiry = DateTime.Today.AddYears(1),
                        PhoneNumber = "083456789012",
                        Email = "ahmad.fauzi@ptnikel.com",
                        Address = "Jl. Diponegoro No. 123, Surabaya",
                        LocationId = locations.First(l => l.Type == "BranchOffice").Id,
                        IsAvailable = true
                    },
                    new Driver {
                        Name = "Rizky Pratama",
                        LicenseNumber = "DRV-004-MLT",
                        LicenseExpiry = DateTime.Today.AddYears(2),
                        PhoneNumber = "084567890123",
                        Email = "rizky.pratama@ptnikel.com",
                        Address = "Perumahan Tambang Block A No. 12, Morowali",
                        LocationId = locations.First(l => l.MineCode == "NI-SULUT-A").Id,
                        IsAvailable = true
                    },
                    new Driver {
                        Name = "Maya Indira",
                        LicenseNumber = "DRV-005-MLT",
                        LicenseExpiry = DateTime.Today.AddYears(4),
                        PhoneNumber = "085678901234",
                        Email = "maya.indira@ptnikel.com",
                        Address = "Perumahan Tambang Block B No. 8, Morowali",
                        LocationId = locations.First(l => l.MineCode == "NI-SULUT-B").Id,
                        IsAvailable = true
                    },
                    new Driver {
                        Name = "Eko Widodo",
                        LicenseNumber = "DRV-006-KLK",
                        LicenseExpiry = DateTime.Today.AddYears(3),
                        PhoneNumber = "086789012345",
                        Email = "eko.widodo@ptnikel.com",
                        Address = "Desa Pomalaa RT 03 RW 02, Kolaka",
                        LocationId = locations.First(l => l.MineCode == "NI-SULTRA-C").Id,
                        IsAvailable = true
                    },
                    new Driver {
                        Name = "Dewi Sartika",
                        LicenseNumber = "DRV-007-MLU",
                        LicenseExpiry = DateTime.Today.AddYears(2),
                        PhoneNumber = "087890123456",
                        Email = "dewi.sartika@ptnikel.com",
                        Address = "Komplek Tambang Weda Bay No. 15, Halmahera",
                        LocationId = locations.First(l => l.MineCode == "NI-MALUT-D").Id,
                        IsAvailable = true
                    },
                    new Driver {
                        Name = "Joko Susilo",
                        LicenseNumber = "DRV-008-PNG",
                        LicenseExpiry = DateTime.Today.AddYears(1),
                        PhoneNumber = "088901234567",
                        Email = "joko.susilo@ptnikel.com",
                        Address = "Perumahan Timika Indah No. 22, Papua",
                        LocationId = locations.First(l => l.MineCode == "NI-PAPUA-E").Id,
                        IsAvailable = true
                    },
                    new Driver {
                        Name = "Ratna Sari",
                        LicenseNumber = "DRV-009-KLT",
                        LicenseExpiry = DateTime.Today.AddYears(3),
                        PhoneNumber = "089012345678",
                        Email = "ratna.sari@ptnikel.com",
                        Address = "Desa Sangkulirang RT 01 RW 03, Kutai Timur",
                        LocationId = locations.First(l => l.MineCode == "NI-KALTIM-F").Id,
                        IsAvailable = true
                    },
                    new Driver {
                        Name = "Hendra Gunawan",
                        LicenseNumber = "DRV-010-JKT",
                        LicenseExpiry = DateTime.Today.AddYears(2),
                        PhoneNumber = "090123456789",
                        Email = "hendra.gunawan@ptnikel.com",
                        Address = "Jl. Cilandak Raya No. 67, Jakarta Selatan",
                        LocationId = locations.First(l => l.Type == "HeadOffice").Id,
                        IsAvailable = true
                    }
                };

                context.Drivers.AddRange(drivers);
                await context.SaveChangesAsync();
            }            if (!context.Vehicles.Any())
            {
                var locations = await context.Locations.ToListAsync();
                var vehicleTypes = await context.VehicleTypes.ToListAsync();
                
                var vehicles = new List<Vehicle>
                {
                    // Vehicles for Head Office Jakarta
                    new Vehicle {
                        RegistrationNumber = "B 1234 NNM",
                        Brand = "Toyota",
                        Model = "Camry Hybrid",
                        Year = 2023,
                        VehicleTypeId = vehicleTypes.First(vt => vt.Name == "Sedan Executive").Id,
                        LocationId = locations.First(l => l.Type == "HeadOffice").Id,
                        IsCompanyOwned = true,
                        RentalCompany = "",
                        IsAvailable = true,
                        Status = "Active",
                        Mileage = 15000,
                        FuelType = "Bensin",
                        FuelCapacity = 60,
                        Notes = "Kendaraan eksekutif untuk direksi"
                    },
                    new Vehicle {
                        RegistrationNumber = "B 2345 NNM",
                        Brand = "Mitsubishi",
                        Model = "Pajero Sport",
                        Year = 2022,
                        VehicleTypeId = vehicleTypes.First(vt => vt.Name == "SUV").Id,
                        LocationId = locations.First(l => l.Type == "HeadOffice").Id,
                        IsCompanyOwned = true,
                        RentalCompany = "",
                        IsAvailable = true,
                        Status = "Active",
                        Mileage = 25000,
                        FuelType = "Solar",
                        FuelCapacity = 68,
                        Notes = "SUV untuk keperluan operasional kantor pusat"
                    },
                    new Vehicle {
                        RegistrationNumber = "B 3456 NNM",
                        Brand = "Toyota",
                        Model = "Hiace Commuter",
                        Year = 2021,
                        VehicleTypeId = vehicleTypes.First(vt => vt.Name == "Minibus").Id,
                        LocationId = locations.First(l => l.Type == "HeadOffice").Id,
                        IsCompanyOwned = true,
                        RentalCompany = "",
                        IsAvailable = true,
                        Status = "Active",
                        Mileage = 45000,
                        FuelType = "Solar",
                        FuelCapacity = 70,
                        Notes = "Minibus untuk transportasi karyawan"
                    },
                    
                    // Vehicles for Branch Office Surabaya
                    new Vehicle {
                        RegistrationNumber = "L 4567 NNM",
                        Brand = "Honda",
                        Model = "CR-V",
                        Year = 2022,
                        VehicleTypeId = vehicleTypes.First(vt => vt.Name == "SUV").Id,
                        LocationId = locations.First(l => l.Type == "BranchOffice").Id,
                        IsCompanyOwned = true,
                        RentalCompany = "",
                        IsAvailable = true,
                        Status = "Active",
                        Mileage = 30000,
                        FuelType = "Bensin",
                        FuelCapacity = 58,
                        Notes = "SUV untuk kebutuhan operasional cabang Surabaya"
                    },
                    new Vehicle {
                        RegistrationNumber = "L 5678 NNM",
                        Brand = "Toyota",
                        Model = "Hilux Double Cabin",
                        Year = 2023,
                        VehicleTypeId = vehicleTypes.First(vt => vt.Name == "Pickup Double Cabin").Id,
                        LocationId = locations.First(l => l.Type == "BranchOffice").Id,
                        IsCompanyOwned = true,
                        RentalCompany = "",
                        IsAvailable = true,
                        Status = "Active",
                        Mileage = 18000,
                        FuelType = "Solar",
                        FuelCapacity = 80,
                        Notes = "Pickup untuk transportasi barang dan personel"
                    },
                    
                    // Mining Site Vehicles - Morowali (Site A)
                    new Vehicle {
                        RegistrationNumber = "DT 6789 NNM",
                        Brand = "Mitsubishi",
                        Model = "Canter",
                        Year = 2021,
                        VehicleTypeId = vehicleTypes.First(vt => vt.Name == "Truk Colt Diesel").Id,
                        LocationId = locations.First(l => l.MineCode == "NI-SULUT-A").Id,
                        IsCompanyOwned = true,
                        RentalCompany = "",
                        IsAvailable = true,
                        Status = "Active",
                        Mileage = 75000,
                        FuelType = "Solar",
                        FuelCapacity = 100,
                        Notes = "Truk ringan untuk distribusi supplies tambang"
                    },
                    new Vehicle {
                        RegistrationNumber = "DT 7890 NNM",
                        Brand = "Toyota",
                        Model = "Coaster",
                        Year = 2020,
                        VehicleTypeId = vehicleTypes.First(vt => vt.Name == "Bus Medium").Id,
                        LocationId = locations.First(l => l.MineCode == "NI-SULUT-A").Id,
                        IsCompanyOwned = true,
                        RentalCompany = "",
                        IsAvailable = true,
                        Status = "Active",
                        Mileage = 120000,
                        FuelType = "Solar",
                        FuelCapacity = 90,
                        Notes = "Bus medium untuk transportasi karyawan ke lokasi tambang"
                    },
                    
                    // Mining Site Vehicles - Morowali (Site B)
                    new Vehicle {
                        RegistrationNumber = "DT 8901 NNM",
                        Brand = "Hino",
                        Model = "Dutro",
                        Year = 2022,
                        VehicleTypeId = vehicleTypes.First(vt => vt.Name == "Truk Colt Diesel").Id,
                        LocationId = locations.First(l => l.MineCode == "NI-SULUT-B").Id,
                        IsCompanyOwned = false,
                        RentalCompany = "PT Sewa Kendaraan Tambang",
                        RentalStartDate = DateTime.Today.AddMonths(-6),
                        RentalEndDate = DateTime.Today.AddMonths(6),
                        RentalCostPerDay = 850000,
                        IsAvailable = true,
                        Status = "Active",
                        Mileage = 55000,
                        FuelType = "Solar",
                        FuelCapacity = 100,
                        Notes = "Truk rental untuk operasional tambang"
                    },
                    new Vehicle {
                        RegistrationNumber = "DT 9012 NNM",
                        Brand = "Mercedes-Benz",
                        Model = "Sprinter",
                        Year = 2021,
                        VehicleTypeId = vehicleTypes.First(vt => vt.Name == "Ambulance").Id,
                        LocationId = locations.First(l => l.MineCode == "NI-SULUT-B").Id,
                        IsCompanyOwned = true,
                        RentalCompany = "",
                        IsAvailable = true,
                        Status = "Active",
                        Mileage = 35000,
                        FuelType = "Solar",
                        FuelCapacity = 75,
                        Notes = "Ambulance untuk keadaan darurat di lokasi tambang"
                    },
                    
                    // Mining Site Vehicles - Kolaka (Site C)
                    new Vehicle {
                        RegistrationNumber = "DT 0123 NNM",
                        Brand = "Mitsubishi",
                        Model = "Fuso Fighter",
                        Year = 2020,
                        VehicleTypeId = vehicleTypes.First(vt => vt.Name == "Truk Fuso").Id,
                        LocationId = locations.First(l => l.MineCode == "NI-SULTRA-C").Id,
                        IsCompanyOwned = true,
                        RentalCompany = "",
                        IsAvailable = true,
                        Status = "Active",
                        Mileage = 95000,
                        FuelType = "Solar",
                        FuelCapacity = 200,
                        Notes = "Truk medium untuk transportasi equipment tambang"
                    },
                    new Vehicle {
                        RegistrationNumber = "DT 1234 NNM",
                        Brand = "Toyota",
                        Model = "Fortuner",
                        Year = 2023,
                        VehicleTypeId = vehicleTypes.First(vt => vt.Name == "SUV").Id,
                        LocationId = locations.First(l => l.MineCode == "NI-SULTRA-C").Id,
                        IsCompanyOwned = true,
                        RentalCompany = "",
                        IsAvailable = true,
                        Status = "Active",
                        Mileage = 12000,
                        FuelType = "Solar",
                        FuelCapacity = 80,
                        Notes = "SUV untuk supervisor dan manajer tambang"
                    },
                    
                    // Mining Site Vehicles - Halmahera (Site D)
                    new Vehicle {
                        RegistrationNumber = "DE 2345 NNM",
                        Brand = "Isuzu",
                        Model = "Giga",
                        Year = 2019,
                        VehicleTypeId = vehicleTypes.First(vt => vt.Name == "Truk Trailer").Id,
                        LocationId = locations.First(l => l.MineCode == "NI-MALUT-D").Id,
                        IsCompanyOwned = true,
                        RentalCompany = "",
                        IsAvailable = true,
                        Status = "Active",
                        Mileage = 180000,
                        FuelType = "Solar",
                        FuelCapacity = 300,
                        Notes = "Truk trailer untuk transportasi heavy equipment"
                    },
                    new Vehicle {
                        RegistrationNumber = "DE 3456 NNM",
                        Brand = "Toyota",
                        Model = "Dyna",
                        Year = 2021,
                        VehicleTypeId = vehicleTypes.First(vt => vt.Name == "Pickup Single Cabin").Id,
                        LocationId = locations.First(l => l.MineCode == "NI-MALUT-D").Id,
                        IsCompanyOwned = true,
                        RentalCompany = "",
                        IsAvailable = true,
                        Status = "Active",
                        Mileage = 48000,
                        FuelType = "Solar",
                        FuelCapacity = 60,
                        Notes = "Pickup untuk transportasi barang ringan"
                    },
                    
                    // Mining Site Vehicles - Papua (Site E)
                    new Vehicle {
                        RegistrationNumber = "PA 4567 NNM",
                        Brand = "Mercedes-Benz",
                        Model = "Atego",
                        Year = 2022,
                        VehicleTypeId = vehicleTypes.First(vt => vt.Name == "Fire Truck").Id,
                        LocationId = locations.First(l => l.MineCode == "NI-PAPUA-E").Id,
                        IsCompanyOwned = true,
                        RentalCompany = "",
                        IsAvailable = true,
                        Status = "Active",
                        Mileage = 22000,
                        FuelType = "Solar",
                        FuelCapacity = 150,
                        Notes = "Fire truck untuk pemadam kebakaran di lokasi tambang"
                    },
                    new Vehicle {
                        RegistrationNumber = "PA 5678 NNM",
                        Brand = "Toyota",
                        Model = "Land Cruiser",
                        Year = 2021,
                        VehicleTypeId = vehicleTypes.First(vt => vt.Name == "SUV").Id,
                        LocationId = locations.First(l => l.MineCode == "NI-PAPUA-E").Id,
                        IsCompanyOwned = true,
                        RentalCompany = "",
                        IsAvailable = true,
                        Status = "Active",
                        Mileage = 68000,
                        FuelType = "Solar",
                        FuelCapacity = 93,
                        Notes = "Land Cruiser untuk medan berat Papua"
                    },
                    
                    // Mining Site Vehicles - Kalimantan (Site F)
                    new Vehicle {
                        RegistrationNumber = "KT 6789 NNM",
                        Brand = "Hino",
                        Model = "Ranger",
                        Year = 2020,
                        VehicleTypeId = vehicleTypes.First(vt => vt.Name == "Bus Besar").Id,
                        LocationId = locations.First(l => l.MineCode == "NI-KALTIM-F").Id,
                        IsCompanyOwned = false,
                        RentalCompany = "PT Trans Kalimantan",
                        RentalStartDate = DateTime.Today.AddMonths(-12),
                        RentalEndDate = DateTime.Today.AddMonths(12),
                        RentalCostPerDay = 1200000,
                        IsAvailable = true,
                        Status = "Active",
                        Mileage = 145000,
                        FuelType = "Solar",
                        FuelCapacity = 250,
                        Notes = "Bus besar rental untuk transportasi massal karyawan"
                    },
                    new Vehicle {
                        RegistrationNumber = "KT 7890 NNM",
                        Brand = "Mitsubishi",
                        Model = "Outlander",
                        Year = 2023,
                        VehicleTypeId = vehicleTypes.First(vt => vt.Name == "SUV").Id,
                        LocationId = locations.First(l => l.MineCode == "NI-KALTIM-F").Id,
                        IsCompanyOwned = true,
                        RentalCompany = "",
                        IsAvailable = true,
                        Status = "Active",
                        Mileage = 8500,
                        FuelType = "Bensin",
                        FuelCapacity = 63,
                        Notes = "SUV untuk manajemen site Kalimantan"
                    }
                };

                context.Vehicles.AddRange(vehicles);
                await context.SaveChangesAsync();
            }
        }
    }
}