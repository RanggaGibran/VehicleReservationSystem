using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VehicleReservationSystem.Models;

namespace VehicleReservationSystem.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<AppDbContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            context.Database.Migrate();
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
            }

            // Seed Drivers if not already seeded
            if (!context.Drivers.Any())
            {
                var locations = await context.Locations.ToListAsync();
                var drivers = new List<Driver>
                {
                    new Driver {
                        Name = "John Smith",
                        LicenseNumber = "DRV-123456",
                        LicenseExpiry = DateTime.Today.AddYears(2),
                        PhoneNumber = "081234567890",
                        Email = "john.smith@example.com",
                        Address = "123 Driver Street",
                        LocationId = locations.First(l => l.Type == "Headquarters").Id,
                        IsAvailable = true
                    },
                    new Driver {
                        Name = "Alice Johnson",
                        LicenseNumber = "DRV-234567",
                        LicenseExpiry = DateTime.Today.AddYears(3),
                        PhoneNumber = "082345678901",
                        Email = "alice.johnson@example.com",
                        Address = "456 Driver Avenue",
                        LocationId = locations.First(l => l.Type == "Branch").Id,
                        IsAvailable = true
                    },
                    new Driver {
                        Name = "Robert Chen",
                        LicenseNumber = "DRV-345678",
                        LicenseExpiry = DateTime.Today.AddYears(1),
                        PhoneNumber = "083456789012",
                        Email = "robert.chen@example.com",
                        Address = "789 Driver Boulevard",
                        LocationId = locations.First(l => l.Type == "Mine").Id,
                        IsAvailable = true
                    }
                };

                context.Drivers.AddRange(drivers);
                await context.SaveChangesAsync();
            }
            if (!context.Vehicles.Any())
            {
                var locations = await context.Locations.ToListAsync();
                var vehicleTypes = await context.VehicleTypes.ToListAsync();
                
                var vehicles = new List<Vehicle>
                {
                    new Vehicle {
                        RegistrationNumber = "B 1234 ABC",
                        Brand = "Toyota",
                        Model = "Avanza",
                        Year = 2022,
                        VehicleTypeId = vehicleTypes.First(vt => vt.Name == "SUV").Id,
                        LocationId = locations.First(l => l.Type == "Headquarters").Id,
                        IsCompanyOwned = true,
                        RentalCompany = "",
                        IsAvailable = true,
                        Status = "Operational"
                    },
                    new Vehicle {
                        RegistrationNumber = "B 2345 BCD",
                        Brand = "Mitsubishi",
                        Model = "Pajero",
                        Year = 2021,
                        VehicleTypeId = vehicleTypes.First(vt => vt.Name == "SUV").Id,
                        LocationId = locations.First(l => l.Type == "Branch").Id,
                        IsCompanyOwned = true,
                        RentalCompany = "",
                        IsAvailable = true,
                        Status = "Operational"
                    },
                    new Vehicle {
                        RegistrationNumber = "B 3456 CDE",
                        Brand = "Toyota",
                        Model = "Hilux",
                        Year = 2023,
                        VehicleTypeId = vehicleTypes.First(vt => vt.Name == "Pickup").Id,
                        LocationId = locations.First(l => l.Name == "Mine Site 1").Id,
                        IsCompanyOwned = true,
                        RentalCompany = "",
                        IsAvailable = true,
                        Status = "Operational"
                    },
                    new Vehicle {
                        RegistrationNumber = "B 4567 DEF",
                        Brand = "Hino",
                        Model = "Dutro",
                        Year = 2020,
                        VehicleTypeId = vehicleTypes.First(vt => vt.Name == "Truck").Id,
                        LocationId = locations.First(l => l.Name == "Mine Site 2").Id,
                        IsCompanyOwned = false,
                        RentalCompany = "PT Rental Kendaraan",
                        RentalStartDate = DateTime.Today.AddMonths(-6),
                        RentalEndDate = DateTime.Today.AddMonths(6),
                        IsAvailable = true,
                        Status = "Operational"
                    },
                    new Vehicle {
                        RegistrationNumber = "B 5678 EFG",
                        Brand = "Isuzu",
                        Model = "Elf",
                        Year = 2021,
                        VehicleTypeId = vehicleTypes.First(vt => vt.Name == "Van").Id,
                        LocationId = locations.First(l => l.Name == "Mine Site 3").Id,
                        IsCompanyOwned = true,
                        RentalCompany = "",
                        IsAvailable = true,
                        Status = "Operational"
                    }
                };

                context.Vehicles.AddRange(vehicles);
                await context.SaveChangesAsync();
            }
        }
    }
}