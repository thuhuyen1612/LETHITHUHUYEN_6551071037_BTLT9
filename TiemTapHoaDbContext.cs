using System;
using Microsoft.EntityFrameworkCore;

namespace Cau1_TiemTapHoa.Models
{
    public class TiemTapHoaDbContext : DbContext
    {
        public DbSet<HangHoa> HangHoas { get; set; } = null!;

        public static string ConnectionString = "Server=.\\SQLEXPRESS;Database=TiemTapHoa;User Id=sa;Password=1532006Quang;TrustServerCertificate=True;";

        public TiemTapHoaDbContext()
        {
        }

        public TiemTapHoaDbContext(DbContextOptions<TiemTapHoaDbContext> options)
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
            modelBuilder.Entity<HangHoa>(entity =>
            {
                entity.ToTable("HangHoa");
                entity.HasKey(e => e.MaHang);
                entity.Property(e => e.TenHang).IsRequired().HasMaxLength(100);
                entity.Property(e => e.LoaiHang).HasMaxLength(50);
                entity.Property(e => e.DonVi).HasMaxLength(20);
                entity.Property(e => e.GiaBan).HasColumnType("decimal(18, 0)");
                entity.Property(e => e.NgayNhap).HasColumnType("datetime");
                entity.Property(e => e.ConKinhDoanh).HasDefaultValue(true);
            });
        }
    }
}
