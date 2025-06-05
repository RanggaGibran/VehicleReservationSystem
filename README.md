# Vehicle Reservation System

Sistem pemesanan kendaraan untuk perusahaan tambang nikel dengan fitur persetujuan berjenjang, monitoring penggunaan kendaraan, dan laporan periodik.

## Teknologi yang Digunakan

- **.NET Version**: 9.0
- **Framework**: ASP.NET Core MVC 9.0
- **Database**: SQLite (Entity Framework Core 9.0.5)
- **UI Framework**: Bootstrap 5
- **Chart Library**: Chart.js (untuk visualisasi data)
- **Excel Export**: ClosedXML 0.105.0
- **Validation**: jQuery dan jQuery Validation
- **Authentication**: ASP.NET Core Identity

## Informasi Login

### Admin
- Username: admin@example.com
- Password: Admin123!

### Approver Level 1
- Username: approver1@example.com
- Password: Approver123!

### Approver Level 2
- Username: approver2@example.com
- Password: Approver123!

## Panduan Penggunaan

### Untuk Admin:

1. Login menggunakan kredensial admin
2. Di dashboard, Anda dapat melihat:
   - Statistik kendaraan (total, tersedia, reservasi pending, reservasi aktif)
   - Grafik penggunaan kendaraan
   - Grafik status reservasi
   - Grafik konsumsi BBM 6 bulan terakhir
   - Reservasi terbaru
   - Jadwal service kendaraan yang akan datang

3. Untuk membuat reservasi baru:
   - Klik "Reservations" di menu
   - Klik tombol "Create New Reservation"
   - Isi form dengan informasi yang diperlukan:
     - Pemohon (requester)
     - Kendaraan
     - Pengemudi (opsional)
     - Jumlah penumpang
     - Tanggal dan waktu mulai/selesai
     - Tujuan dan keperluan
     - Minimal 2 approver (berjenjang) untuk proses persetujuan
   - Klik "Create Reservation"

4. Untuk melihat dan mengekspor laporan:
   - Klik "Reports" di menu
   - Pilih rentang tanggal dan filter lokasi/tipe kendaraan (opsional)
   - Klik "Generate Report"
   - Untuk mengekspor ke Excel, klik tombol "Export to Excel"

### Untuk Approver:

1. Login menggunakan kredensial approver
2. Klik "Approvals" di menu untuk melihat daftar reservasi yang menunggu persetujuan
3. Klik tombol "Review" pada reservasi yang ingin disetujui/ditolak
4. Pada halaman detail:
   - Lihat informasi lengkap reservasi
   - Isi komentar pada form persetujuan
   - Klik "Approve" untuk menyetujui atau "Reject" untuk menolak

## Fitur Utama

- Manajemen kendaraan (perusahaan dan sewa)
- Pemesanan kendaraan dengan persetujuan berjenjang
- Dashboard dengan visualisasi data:
  - Statistik kendaraan dan reservasi
  - Grafik penggunaan kendaraan
  - Grafik status reservasi
  - Grafik konsumsi BBM
- Monitoring pemakaian kendaraan
- Jadwal service kendaraan
- Laporan pemesanan kendaraan dengan filter tanggal, lokasi, dan tipe kendaraan
- Export laporan ke Excel

## Struktur Database

Aplikasi ini menggunakan **SQLite Database Version 3** dengan struktur tabel utama:
- **Vehicles**: Data kendaraan (registrasi, merek, model, tahun, status)
- **VehicleTypes**: Jenis kendaraan (sedan, SUV, truk, dll)
- **Locations**: Lokasi/region (pusat, cabang, tambang)
- **Drivers**: Data pengemudi
- **Reservations**: Pemesanan kendaraan
- **Approvals**: Persetujuan pemesanan berjenjang
- **FuelConsumption**: Konsumsi BBM
- **ServiceSchedule**: Jadwal service kendaraan
- **AspNetUsers**: Data pengguna sistem
- **AspNetRoles**: Peran pengguna (Admin, Approver, User)

## Cara Menjalankan Aplikasi

1. Pastikan .NET 9.0 SDK sudah terinstall
2. Buka terminal/command prompt
3. Navigasi ke folder project: `cd "e:\Net Developer\VehicleReservationSystem"`
4. Restore dependencies: `dotnet restore`
5. Build aplikasi: `dotnet build`
6. Jalankan aplikasi: `dotnet run`
7. Buka browser dan akses: `https://localhost:5001` atau `http://localhost:5000`

## Struktur Proyek

- Controllers: Mengatur logika aplikasi dan interaksi dengan view
- Models: Mendefinisikan entitas database
- ViewModels: Model khusus untuk view
- Views: Tampilan antarmuka pengguna
- Services: Layanan business logic
- Data: Konfigurasi database dan inisialisasi data