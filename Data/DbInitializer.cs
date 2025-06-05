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
            }
            if (!context.Locations.Any())
            {
                var locations = new List<Location>
                {
                    new Location { Name = "Headquarters", Type = "Headquarters", Address = "Main Street 123", Region = "Central" },
                    new Location { Name = "Branch Office", Type = "Branch", Address = "Business Park 456", Region = "West" },
                    new Location { Name = "Mine Site 1", Type = "Mine", Address = "Mining Area 1", Region = "North" },
                    new Location { Name = "Mine Site 2", Type = "Mine", Address = "Mining Area 2", Region = "South" },
                    new Location { Name = "Mine Site 3", Type = "Mine", Address = "Mining Area 3", Region = "East" },
                    new Location { Name = "Mine Site 4", Type = "Mine", Address = "Mining Area 4", Region = "West" },
                    new Location { Name = "Mine Site 5", Type = "Mine", Address = "Mining Area 5", Region = "Central" },
                    new Location { Name = "Mine Site 6", Type = "Mine", Address = "Mining Area 6", Region = "North" }
                };

                context.Locations.AddRange(locations);
                await context.SaveChangesAsync();
            }
            if (!context.VehicleTypes.Any())
            {
                var vehicleTypes = new List<VehicleType>
                {
                    new VehicleType { Name = "Sedan", Description = "4-door car", Category = "People", Capacity = 5 },
                    new VehicleType { Name = "SUV", Description = "Sport Utility Vehicle", Category = "People", Capacity = 7 },
                    new VehicleType { Name = "Van", Description = "Passenger van", Category = "People", Capacity = 12 },
                    new VehicleType { Name = "Bus", Description = "Large passenger bus", Category = "People", Capacity = 30 },
                    new VehicleType { Name = "Pickup", Description = "Pickup truck", Category = "Goods", Capacity = 1000 },
                    new VehicleType { Name = "Truck", Description = "Medium truck", Category = "Goods", Capacity = 5000 },
                    new VehicleType { Name = "Heavy Truck", Description = "Heavy duty truck", Category = "Goods", Capacity = 10000 }
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