using System;
using System.Collections.Generic;
using KhoaNoiVuCNTT.Models;
using Microsoft.EntityFrameworkCore;

namespace KhoaNoiVuCNTT.Data;

public partial class ThongTinNoiBoContext : DbContext
{
    public ThongTinNoiBoContext()
    {
    }

    public ThongTinNoiBoContext(DbContextOptions<ThongTinNoiBoContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BoMon> BoMon { get; set; }

    public virtual DbSet<CanBo> CanBo { get; set; }

    public virtual DbSet<GiangVien> GiangVien { get; set; }

    public virtual DbSet<LichCongTac> LichCongTac { get; set; }

    public virtual DbSet<QuanTriVienGiaoVu> QuanTriVienGiaoVu { get; set; }

    public virtual DbSet<TaiLieu> TaiLieu { get; set; }

    public virtual DbSet<ThongBao> ThongBao { get; set; }

    public virtual DbSet<VaiTro> VaiTro { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.;Database=ThongTinNoiBo_CNTT;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BoMon>(entity =>
        {
            entity.HasKey(e => e.MaBoMon).HasName("PK__BoMon__B783EFA61745CE29");

            entity.ToTable("BoMon");

            entity.Property(e => e.TenBoMon).HasMaxLength(100);
        });

        modelBuilder.Entity<CanBo>(entity =>
        {
            entity.HasKey(e => e.MaCanBo).HasName("PK__CanBo__4003E215CEA609C5");

            entity.ToTable("CanBo");

            entity.HasIndex(e => e.Email, "UQ__CanBo__A9D10534051C04CB").IsUnique();

            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.HoTen).HasMaxLength(100);
            entity.Property(e => e.MatKhau)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.MaBoMonNavigation).WithMany(p => p.CanBos)
                .HasForeignKey(d => d.MaBoMon)
                .HasConstraintName("FK__CanBo__MaBoMon__3C69FB99");

            entity.HasOne(d => d.MaVaiTroNavigation).WithMany(p => p.CanBo)
                .HasForeignKey(d => d.MaVaiTro)
                .HasConstraintName("FK__CanBo__MaVaiTro__3D5E1FD2");
        });

        modelBuilder.Entity<GiangVien>(entity =>
        {
            entity.HasKey(e => e.MaGiangVien).HasName("PK__GiangVie__C03BEEBAAE52707D");

            entity.ToTable("GiangVien");

            entity.Property(e => e.GioiTinh).HasMaxLength(10);
            entity.Property(e => e.MonDay).HasMaxLength(100);
            entity.Property(e => e.TenGiangVien).HasMaxLength(100);
        });

        modelBuilder.Entity<LichCongTac>(entity =>
        {
            entity.HasKey(e => e.MaLich).HasName("PK__LichCong__728A9AE9C0B0610F");

            entity.ToTable("LichCongTac");

            entity.Property(e => e.DiaDiem).HasMaxLength(255);
            entity.Property(e => e.ThoiGianBatDau).HasColumnType("datetime");
            entity.Property(e => e.ThoiGianKetThuc).HasColumnType("datetime");
            entity.Property(e => e.TieuDe).HasMaxLength(255);

            entity.HasOne(d => d.MaNguoiToChucNavigation).WithMany(p => p.LichCongTacs)
                .HasForeignKey(d => d.MaNguoiToChuc)
                .HasConstraintName("FK__LichCongT__MaNgu__47DBAE45");
        });

        modelBuilder.Entity<QuanTriVienGiaoVu>(entity =>
        {
            entity.HasKey(e => e.MaNhanVien).HasName("PK__QuanTriV__77B2CA47D2D1E118");

            entity.ToTable("QuanTriVienGiaoVu");

            entity.Property(e => e.ChucVu).HasMaxLength(100);
            entity.Property(e => e.GioiTinh).HasMaxLength(10);
            entity.Property(e => e.TenNhanVien).HasMaxLength(100);
        });

        modelBuilder.Entity<TaiLieu>(entity =>
        {
            entity.HasKey(e => e.MaTaiLieu).HasName("PK__TaiLieu__FD18A657C807D83B");

            entity.ToTable("TaiLieu");

            entity.Property(e => e.DuongDanFile)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.NgayTaiLen)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PhanLoai).HasMaxLength(100);
            entity.Property(e => e.TenTaiLieu).HasMaxLength(255);

            entity.HasOne(d => d.MaNguoiTaiLenNavigation).WithMany(p => p.TaiLieus)
                .HasForeignKey(d => d.MaNguoiTaiLen)
                .HasConstraintName("FK__TaiLieu__MaNguoi__440B1D61");
        });

        modelBuilder.Entity<ThongBao>(entity =>
        {
            entity.HasKey(e => e.MaThongBao).HasName("PK__ThongBao__04DEB54E784554CB");

            entity.ToTable("ThongBao");

            entity.Property(e => e.NgayDang)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TieuDe).HasMaxLength(255);

            entity.HasOne(d => d.MaNguoiDangNavigation).WithMany(p => p.ThongBaos)
                .HasForeignKey(d => d.MaNguoiDang)
                .HasConstraintName("FK__ThongBao__MaNguo__412EB0B6");
        });

        modelBuilder.Entity<VaiTro>(entity =>
        {
            entity.HasKey(e => e.MaVaiTro).HasName("PK__VaiTro__C24C41CF209D58FC");

            entity.ToTable("VaiTro");

            entity.Property(e => e.TenVaiTro).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
