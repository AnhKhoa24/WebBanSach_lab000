using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace HuynhKom_lab00_bansach.Models.Entities;

public partial class QuanLySachContext : DbContext
{
    public QuanLySachContext()
    {
    }

    public QuanLySachContext(DbContextOptions<QuanLySachContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Chitietdondathang> Chitietdondathangs { get; set; }

    public virtual DbSet<Chude> Chudes { get; set; }

    public virtual DbSet<Dondathang> Dondathangs { get; set; }

    public virtual DbSet<Khachhang> Khachhangs { get; set; }

    public virtual DbSet<Nhaxuatban> Nhaxuatbans { get; set; }

    public virtual DbSet<Sach> Saches { get; set; }

    public virtual DbSet<Tacgium> Tacgia { get; set; }

    public virtual DbSet<Vietsac> Vietsacs { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
 => optionsBuilder.UseSqlServer("Name=ConnectionStrings:Connection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Chitietdondathang>(entity =>
        {
            entity.HasKey(e => new { e.MaDonHang, e.Masach }).HasName("PK__CHITIETD__8B6CA7654745E4D5");

            entity.ToTable("CHITIETDONDATHANG");

            entity.Property(e => e.Dongia).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.MaDonHangNavigation).WithMany(p => p.Chitietdondathangs)
                .HasForeignKey(d => d.MaDonHang)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CHITIETDO__MaDon__47DBAE45");

            entity.HasOne(d => d.MasachNavigation).WithMany(p => p.Chitietdondathangs)
                .HasForeignKey(d => d.Masach)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CHITIETDO__Masac__48CFD27E");
        });

        modelBuilder.Entity<Chude>(entity =>
        {
            entity.HasKey(e => e.MaCd).HasName("PK__CHUDE__27258E04B7197516");

            entity.ToTable("CHUDE");

            entity.Property(e => e.MaCd)
                .ValueGeneratedNever()
                .HasColumnName("MaCD");
            entity.Property(e => e.TenChuDe)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Dondathang>(entity =>
        {
            entity.HasKey(e => e.MaDonHang).HasName("PK__DONDATHA__129584AD04A43E01");

            entity.ToTable("DONDATHANG");

            entity.Property(e => e.MaDonHang).ValueGeneratedNever();
            entity.Property(e => e.MaKh).HasColumnName("MaKH");
            entity.Property(e => e.Tinhtranggiaohang)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.MaKhNavigation).WithMany(p => p.Dondathangs)
                .HasForeignKey(d => d.MaKh)
                .HasConstraintName("FK__DONDATHANG__MaKH__44FF419A");
        });

        modelBuilder.Entity<Khachhang>(entity =>
        {
            entity.HasKey(e => e.MaKh).HasName("PK__KHACHHAN__2725CF1E8E1EFC66");

            entity.ToTable("KHACHHANG");

            entity.Property(e => e.MaKh)
                .ValueGeneratedNever()
                .HasColumnName("MaKH");
            entity.Property(e => e.DiachiKh)
                .HasColumnType("text")
                .HasColumnName("DiachiKH");
            entity.Property(e => e.DienthoaiKh)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("DienthoaiKH");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.HoTen)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Matkhau)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Taikhoan)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Nhaxuatban>(entity =>
        {
            entity.HasKey(e => e.MaNxb).HasName("PK__NHAXUATB__3A19482C562E3E30");

            entity.ToTable("NHAXUATBAN");

            entity.Property(e => e.MaNxb)
                .ValueGeneratedNever()
                .HasColumnName("MaNXB");
            entity.Property(e => e.Diachi).HasColumnType("text");
            entity.Property(e => e.Dienthoai)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.TenNxb)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("TenNXB");
        });

        modelBuilder.Entity<Sach>(entity =>
        {
            entity.HasKey(e => e.Masach).HasName("PK__SACH__9F923C880EE2BEAE");

            entity.ToTable("SACH");

            entity.Property(e => e.Masach).ValueGeneratedNever();
            entity.Property(e => e.Anhbia)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Giaban).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.MaCd).HasColumnName("MaCD");
            entity.Property(e => e.MaNxb).HasColumnName("MaNXB");
            entity.Property(e => e.Mota).HasColumnType("text");
            entity.Property(e => e.Tensach)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.MaCdNavigation).WithMany(p => p.Saches)
                .HasForeignKey(d => d.MaCd)
                .HasConstraintName("FK_SACH_CHUDE");

            entity.HasOne(d => d.MaNxbNavigation).WithMany(p => p.Saches)
                .HasForeignKey(d => d.MaNxb)
                .HasConstraintName("FK_SACH_NHAXUATBAN");
        });

        modelBuilder.Entity<Tacgium>(entity =>
        {
            entity.HasKey(e => e.MaTg).HasName("PK__TACGIA__2725007492246A16");

            entity.ToTable("TACGIA");

            entity.Property(e => e.MaTg)
                .ValueGeneratedNever()
                .HasColumnName("MaTG");
            entity.Property(e => e.Diachi).HasColumnType("text");
            entity.Property(e => e.Dienthoai)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.TenTg)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("TenTG");
            entity.Property(e => e.Tieusu).HasColumnType("text");
        });

        modelBuilder.Entity<Vietsac>(entity =>
        {
            entity.HasKey(e => new { e.MaTg, e.Masach }).HasName("PK__VIETSAC__BEDC23BC5A7F1D33");

            entity.ToTable("VIETSAC");

            entity.Property(e => e.MaTg).HasColumnName("MaTG");
            entity.Property(e => e.Vaitro)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Vitri)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.MaTgNavigation).WithMany(p => p.Vietsacs)
                .HasForeignKey(d => d.MaTg)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__VIETSAC__MaTG__3F466844");

            entity.HasOne(d => d.MasachNavigation).WithMany(p => p.Vietsacs)
                .HasForeignKey(d => d.Masach)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__VIETSAC__Masach__403A8C7D");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
