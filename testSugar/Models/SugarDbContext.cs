using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace testSuger.Models;

public partial class SugarDbContext : DbContext
{
    public SugarDbContext()
    {
    }

    public SugarDbContext(DbContextOptions<SugarDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Doctor> Doctors { get; set; }

    public virtual DbSet<Dose> Doses { get; set; }

    public virtual DbSet<Medicine> Medicines { get; set; }

    public virtual DbSet<Patient> Patients { get; set; }

    public virtual DbSet<PatientDoctor> PatientDoctors { get; set; }

    public virtual DbSet<Read> Reads { get; set; }

    public virtual DbSet<Time> Times { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Visit> Visits { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=MARWA;Database=SugarDB;Integrated Security=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.ToTable("Doctor");

            entity.Property(e => e.Id)
                .HasColumnType("numeric(10, 0)")
                .HasColumnName("ID");
            entity.Property(e => e.Age)
                .HasColumnType("numeric(2, 0)")
                .HasColumnName("age");
            entity.Property(e => e.Dob)
                .HasColumnType("date")
                .HasColumnName("DOB");
            entity.Property(e => e.Gender).HasColumnName("gender");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(20)
                .HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.Doctor)
                .HasForeignKey<Doctor>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Doctor_user");
        });

        modelBuilder.Entity<Dose>(entity =>
        {
            entity.Property(e => e.DoseId).HasColumnName("dose_id");
            entity.Property(e => e.Dose1).HasColumnName("dose");
            entity.Property(e => e.PatientId)
                .HasColumnType("numeric(10, 0)")
                .HasColumnName("patient_id");
            entity.Property(e => e.R1)
                .HasColumnType("numeric(3, 0)")
                .HasColumnName("r1");
            entity.Property(e => e.R2)
                .HasColumnType("numeric(3, 0)")
                .HasColumnName("r2");

            entity.HasOne(d => d.Patient).WithMany(p => p.Doses)
                .HasForeignKey(d => d.PatientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("patient_doses");
        });

        modelBuilder.Entity<Medicine>(entity =>
        {
            entity.ToTable("Medicine");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.ToTable("Patient");

            entity.Property(e => e.Id)
                .HasColumnType("numeric(10, 0)")
                .HasColumnName("ID");
            entity.Property(e => e.Age)
                .HasColumnType("numeric(2, 0)")
                .HasColumnName("age");
            entity.Property(e => e.Dob)
                .HasColumnType("date")
                .HasColumnName("DOB");
            entity.Property(e => e.Gender).HasColumnName("gender");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");
            entity.Property(e => e.Wt).HasColumnName("WT");

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.Patient)
                .HasForeignKey<Patient>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Patient_user");
        });

        modelBuilder.Entity<PatientDoctor>(entity =>
        {
            entity.HasKey(e => e.DrPtId);

            entity.ToTable("Patient_Doctor");

            entity.Property(e => e.DrPtId).HasColumnName("dr_pt_id");
            entity.Property(e => e.DoctorId)
                .HasColumnType("numeric(10, 0)")
                .HasColumnName("doctor_id");
            entity.Property(e => e.PatientId)
                .HasColumnType("numeric(10, 0)")
                .HasColumnName("patient_id");

            entity.HasOne(d => d.Doctor).WithMany(p => p.PatientDoctors)
                .HasForeignKey(d => d.DoctorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Patient_Doctor_Doctor");

            entity.HasOne(d => d.Patient).WithMany(p => p.PatientDoctors)
                .HasForeignKey(d => d.PatientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Patient_Doctor_Patient");
        });

        modelBuilder.Entity<Read>(entity =>
        {
            entity.Property(e => e.ReadId).HasColumnName("read_id");
            entity.Property(e => e.Date)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("date");
            entity.Property(e => e.PatientId)
                .HasColumnType("numeric(10, 0)")
                .HasColumnName("patient_id");
            entity.Property(e => e.Value)
                .HasColumnType("numeric(3, 0)")
                .HasColumnName("value");

            entity.HasOne(d => d.Patient).WithMany(p => p.Reads)
                .HasForeignKey(d => d.PatientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("patient_reads");
        });

        modelBuilder.Entity<Time>(entity =>
        {
            entity.HasKey(e => e.PatientId);

            entity.Property(e => e.PatientId)
                .HasColumnType("numeric(10, 0)")
                .HasColumnName("patient_id");
            entity.Property(e => e.Breakfast).HasColumnName("breakfast");
            entity.Property(e => e.Dinner).HasColumnName("dinner");
            entity.Property(e => e.Lunch).HasColumnName("lunch");

            entity.HasOne(d => d.Patient).WithOne(p => p.Time)
                .HasForeignKey<Time>(d => d.PatientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("patient_times");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.Property(e => e.Id)
                .HasColumnType("numeric(10, 0)")
                .HasColumnName("ID");
            entity.Property(e => e.Password)
                .HasMaxLength(20)
                .HasColumnName("password");
            entity.Property(e => e.Role)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("role");
        });

        modelBuilder.Entity<Visit>(entity =>
        {
            entity.HasKey(e => new { e.PatientId, e.VisitNo });

            entity.Property(e => e.PatientId)
                .HasColumnType("numeric(10, 0)")
                .HasColumnName("patient_id");
            entity.Property(e => e.VisitNo).HasColumnName("visit_no");
            entity.Property(e => e.CorrectMed)
                .HasMaxLength(50)
                .HasColumnName("correct_med");
            entity.Property(e => e.CorrectVal).HasColumnName("correct_val");
            entity.Property(e => e.Examination).HasColumnName("examination");
            entity.Property(e => e.History).HasColumnName("history");
            entity.Property(e => e.LongMed)
                .HasMaxLength(50)
                .HasColumnName("long_med");
            entity.Property(e => e.LongVal).HasColumnName("long_val");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.ShortMed)
                .HasMaxLength(50)
                .HasColumnName("short_med");
            entity.Property(e => e.Wt).HasColumnName("WT");

            entity.HasOne(d => d.Patient).WithMany(p => p.Visits)
                .HasForeignKey(d => d.PatientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Visits_Patient");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
