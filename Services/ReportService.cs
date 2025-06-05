using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using VehicleReservationSystem.Models;
using VehicleReservationSystem.ViewModels;

namespace VehicleReservationSystem.Services
{
    public interface IReportService
    {
        Task<List<ReservationReportItemViewModel>> GenerateReservationReport(DateTime startDate, DateTime endDate, int? locationId, int? vehicleTypeId);
        Task<ExcelFileViewModel> ExportReservationReportToExcel(DateTime startDate, DateTime endDate, int? locationId, int? vehicleTypeId);
    }

    public class ReportService : IReportService
    {
        private readonly AppDbContext _context;

        public ReportService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ReservationReportItemViewModel>> GenerateReservationReport(DateTime startDate, DateTime endDate, int? locationId, int? vehicleTypeId)
        {
            // Ensure the date range includes the full day
            startDate = startDate.Date;
            endDate = endDate.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
            
            var query = _context.Reservations
                .Include(r => r.Vehicle)
                    .ThenInclude(v => v.VehicleType)
                .Include(r => r.Vehicle)
                    .ThenInclude(v => v.Location)
                .Include(r => r.Requester)
                .Include(r => r.Driver)
                .Where(r => r.StartDate >= startDate && r.StartDate <= endDate);

            if (locationId.HasValue)
            {
                query = query.Where(r => r.Vehicle.LocationId == locationId);
            }

            if (vehicleTypeId.HasValue)
            {
                query = query.Where(r => r.Vehicle.VehicleTypeId == vehicleTypeId);
            }

            var reservations = await query.ToListAsync();

            var reportItems = reservations.Select(r => new ReservationReportItemViewModel
            {
                ReservationId = r.Id,
                RequestDate = r.RequestDate,
                StartDate = r.StartDate,
                EndDate = r.EndDate,
                Duration = Math.Round((r.EndDate - r.StartDate).TotalHours, 1),
                VehicleRegistration = r.Vehicle.RegistrationNumber,
                VehicleType = r.Vehicle.VehicleType.Name,
                VehicleCategory = r.Vehicle.VehicleType.Category,
                Location = r.Vehicle.Location.Name,
                Region = r.Vehicle.Location.Region,
                RequesterName = r.Requester.FullName,
                RequesterDepartment = r.Requester.Department,
                DriverName = r.Driver?.Name,
                Purpose = r.Purpose,
                Destination = r.Destination,
                Status = r.Status
            }).ToList();

            return reportItems;
        }

        public async Task<ExcelFileViewModel> ExportReservationReportToExcel(DateTime startDate, DateTime endDate, int? locationId, int? vehicleTypeId)
        {
            var reportData = await GenerateReservationReport(startDate, endDate, locationId, vehicleTypeId);

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Reservation Report");

                // Add report title and information
                worksheet.Cell(1, 1).Value = "Vehicle Reservation Report";
                worksheet.Cell(1, 1).Style.Font.Bold = true;
                worksheet.Cell(1, 1).Style.Font.FontSize = 16;
                worksheet.Range(1, 1, 1, 5).Merge();
                
                worksheet.Cell(2, 1).Value = $"Period: {startDate:yyyy-MM-dd} to {endDate:yyyy-MM-dd}";
                worksheet.Range(2, 1, 2, 5).Merge();
                
                worksheet.Cell(3, 1).Value = $"Generated on: {DateTime.Now:yyyy-MM-dd HH:mm}";
                worksheet.Range(3, 1, 3, 5).Merge();
                
                // Add filter information
                int row = 4;
                if (locationId.HasValue)
                {
                    var location = await _context.Locations.FindAsync(locationId.Value);
                    worksheet.Cell(row, 1).Value = $"Location: {location?.Name}";
                    worksheet.Range(row, 1, row, 5).Merge();
                    row++;
                }
                
                if (vehicleTypeId.HasValue)
                {
                    var vehicleType = await _context.VehicleTypes.FindAsync(vehicleTypeId.Value);
                    worksheet.Cell(row, 1).Value = $"Vehicle Type: {vehicleType?.Name}";
                    worksheet.Range(row, 1, row, 5).Merge();
                    row++;
                }
                
                row++; // Empty row before headers

                // Add header
                var headerRow = row;
                worksheet.Cell(headerRow, 1).Value = "Reservation ID";
                worksheet.Cell(headerRow, 2).Value = "Request Date";
                worksheet.Cell(headerRow, 3).Value = "Start Date";
                worksheet.Cell(headerRow, 4).Value = "End Date";
                worksheet.Cell(headerRow, 5).Value = "Duration (Hours)";
                worksheet.Cell(headerRow, 6).Value = "Vehicle Registration";
                worksheet.Cell(headerRow, 7).Value = "Vehicle Type";
                worksheet.Cell(headerRow, 8).Value = "Vehicle Category";
                worksheet.Cell(headerRow, 9).Value = "Location";
                worksheet.Cell(headerRow, 10).Value = "Region";
                worksheet.Cell(headerRow, 11).Value = "Requester";
                worksheet.Cell(headerRow, 12).Value = "Department";
                worksheet.Cell(headerRow, 13).Value = "Driver";
                worksheet.Cell(headerRow, 14).Value = "Purpose";
                worksheet.Cell(headerRow, 15).Value = "Destination";
                worksheet.Cell(headerRow, 16).Value = "Status";

                // Format header
                var headerRange = worksheet.Range(headerRow, 1, headerRow, 16);
                headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                headerRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                // Add data
                for (int i = 0; i < reportData.Count; i++)
                {
                    var item = reportData[i];
                    int dataRow = headerRow + i + 1;

                    worksheet.Cell(dataRow, 1).Value = item.ReservationId;
                    worksheet.Cell(dataRow, 2).Value = item.RequestDate;
                    worksheet.Cell(dataRow, 3).Value = item.StartDate;
                    worksheet.Cell(dataRow, 4).Value = item.EndDate;
                    worksheet.Cell(dataRow, 5).Value = item.Duration;
                    worksheet.Cell(dataRow, 6).Value = item.VehicleRegistration;
                    worksheet.Cell(dataRow, 7).Value = item.VehicleType;
                    worksheet.Cell(dataRow, 8).Value = item.VehicleCategory;
                    worksheet.Cell(dataRow, 9).Value = item.Location;
                    worksheet.Cell(dataRow, 10).Value = item.Region;
                    worksheet.Cell(dataRow, 11).Value = item.RequesterName;
                    worksheet.Cell(dataRow, 12).Value = item.RequesterDepartment;
                    worksheet.Cell(dataRow, 13).Value = item.DriverName;
                    worksheet.Cell(dataRow, 14).Value = item.Purpose;
                    worksheet.Cell(dataRow, 15).Value = item.Destination;
                    worksheet.Cell(dataRow, 16).Value = item.Status;
                    
                    // Format date cells
                    worksheet.Cell(dataRow, 2).Style.DateFormat.Format = "yyyy-MM-dd HH:mm";
                    worksheet.Cell(dataRow, 3).Style.DateFormat.Format = "yyyy-MM-dd HH:mm";
                    worksheet.Cell(dataRow, 4).Style.DateFormat.Format = "yyyy-MM-dd HH:mm";
                    
                    // Format duration cell
                    worksheet.Cell(dataRow, 5).Style.NumberFormat.Format = "0.0";
                    
                    // Style for status column
                    var statusCell = worksheet.Cell(dataRow, 16);
                    if (item.Status == "Approved")
                        statusCell.Style.Fill.BackgroundColor = XLColor.LightGreen;
                    else if (item.Status == "Pending")
                        statusCell.Style.Fill.BackgroundColor = XLColor.LightYellow;
                    else if (item.Status == "Rejected")
                        statusCell.Style.Fill.BackgroundColor = XLColor.LightPink;
                    else if (item.Status == "Completed")
                        statusCell.Style.Fill.BackgroundColor = XLColor.LightBlue;
                }

                // Add summary section
                int summaryRow = headerRow + reportData.Count + 2;
                
                worksheet.Cell(summaryRow, 1).Value = "Summary";
                worksheet.Cell(summaryRow, 1).Style.Font.Bold = true;
                worksheet.Range(summaryRow, 1, summaryRow, 16).Merge();
                
                worksheet.Cell(summaryRow + 1, 1).Value = "Total Reservations:";
                worksheet.Cell(summaryRow + 1, 2).Value = reportData.Count;
                
                worksheet.Cell(summaryRow + 2, 1).Value = "Total Duration (Hours):";
                worksheet.Cell(summaryRow + 2, 2).Value = reportData.Sum(r => r.Duration);
                worksheet.Cell(summaryRow + 2, 2).Style.NumberFormat.Format = "0.0";
                
                // Add status breakdown
                int statusRow = summaryRow + 4;
                worksheet.Cell(statusRow, 1).Value = "Status Breakdown";
                worksheet.Cell(statusRow, 1).Style.Font.Bold = true;
                worksheet.Range(statusRow, 1, statusRow, 3).Merge();
                
                statusRow++;
                worksheet.Cell(statusRow, 1).Value = "Status";
                worksheet.Cell(statusRow, 2).Value = "Count";
                worksheet.Cell(statusRow, 3).Value = "Percentage";
                
                var statusCells = worksheet.Range(statusRow, 1, statusRow, 3);
                statusCells.Style.Fill.BackgroundColor = XLColor.LightGray;
                statusCells.Style.Font.Bold = true;
                
                var statusGroups = reportData.GroupBy(r => r.Status)
                    .Select(g => new { Status = g.Key, Count = g.Count() })
                    .OrderByDescending(x => x.Count)
                    .ToList();
                
                for (int i = 0; i < statusGroups.Count; i++)
                {
                    var status = statusGroups[i];
                    int currentRow = statusRow + i + 1;
                    
                    worksheet.Cell(currentRow, 1).Value = status.Status;
                    worksheet.Cell(currentRow, 2).Value = status.Count;
                    worksheet.Cell(currentRow, 3).Value = (double)status.Count / reportData.Count;
                    worksheet.Cell(currentRow, 3).Style.NumberFormat.Format = "0.0%";
                    
                    // Color-code status rows
                    if (status.Status == "Approved")
                        worksheet.Cell(currentRow, 1).Style.Fill.BackgroundColor = XLColor.LightGreen;
                    else if (status.Status == "Pending")
                        worksheet.Cell(currentRow, 1).Style.Fill.BackgroundColor = XLColor.LightYellow;
                    else if (status.Status == "Rejected")
                        worksheet.Cell(currentRow, 1).Style.Fill.BackgroundColor = XLColor.LightPink;
                    else if (status.Status == "Completed")
                        worksheet.Cell(currentRow, 1).Style.Fill.BackgroundColor = XLColor.LightBlue;
                }

                // Format the worksheet
                worksheet.Columns().AdjustToContents();
                
                // Add border to data
                var dataRange = worksheet.Range(headerRow, 1, headerRow + reportData.Count, 16);
                dataRange.Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
                dataRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                // Convert to byte array
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Position = 0;

                    return new ExcelFileViewModel
                    {
                        Content = stream.ToArray(),
                        ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        FileName = $"VehicleReservationReport_{startDate:yyyyMMdd}_to_{endDate:yyyyMMdd}.xlsx"
                    };
                }
            }
        }
    }
}