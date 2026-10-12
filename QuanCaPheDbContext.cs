using Microsoft.EntityFrameworkCore;

namespace Cau2_QuanCaPhe.Models
{
    public class QuanCaPheDbContext : DbContext
    {
        public DbSet<Mon> Mons { get; set; } = null!;
        public DbSet<HoaDon> HoaDons { get; set; } = null!;
        public DbSet<ChiTietHoaDon> ChiTietHoaDons { get; set; } = null!;

        public static string ConnectionString = "Server=.\\SQLEXPRESS;Database=QuanCaPhe;User Id=sa;Password=1532006Quang;TrustServerCertificate=True;";

        public QuanCaPheDbContext()
        {
        }

        public QuanCaPheDbContext(DbContextOptions<QuanCaPheDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Mon>(entity =>
            {
                entity.ToTable("Mon");
                entity.HasKey(e => e.MaMon);
                entity.Property(e => e.MaMon).ValueGeneratedNever();
                entity.Property(e => e.TenMon).IsRequired().HasMaxLength(100);
                entity.Property(e => e.NhomMon).HasMaxLength(30);
                entity.Property(e => e.DonGia).HasColumnType("decimal(18, 0)");
            });

            modelBuilder.Entity<HoaDon>(entity =>
            {
                entity.ToTable("HoaDon");
                entity.HasKey(e => e.MaHD);
                entity.Property(e => e.NgayLap).HasColumnType("datetime");
                entity.Property(e => e.HinhThucTT).IsRequired().HasMaxLength(20);
                entity.Property(e => e.GhiChu).HasMaxLength(200);
            });

            modelBuilder.Entity<ChiTietHoaDon>(entity =>
            {
                entity.ToTable("ChiTietHoaDon");
                entity.HasKey(e => e.MaCT);
                entity.Property(e => e.DonGia).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.HoaDon)
                    .WithMany(p => p.ChiTietHoaDons)
                    .HasForeignKey(d => d.MaHD)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.Mon)
                    .WithMany(p => p.ChiTietHoaDons)
                    .HasForeignKey(d => d.MaMon)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });
        }
    }
}
