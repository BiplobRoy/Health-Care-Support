﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HealthCareSupport02.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class dbHCS02Entities : DbContext
    {
        public dbHCS02Entities()
            : base("name=dbHCS02Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<Ambulance> Ambulances { get; set; }
        public virtual DbSet<Apointment> Apointments { get; set; }
        public virtual DbSet<ApointmentFixed> ApointmentFixeds { get; set; }
        public virtual DbSet<BloodDonare> BloodDonares { get; set; }
        public virtual DbSet<BloodGroup> BloodGroups { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<DataRecord> DataRecords { get; set; }
        public virtual DbSet<Doctor> Doctors { get; set; }
        public virtual DbSet<Gender> Genders { get; set; }
        public virtual DbSet<HealthTip> HealthTips { get; set; }
        public virtual DbSet<Hospital> Hospitals { get; set; }
        public virtual DbSet<Medicine> Medicines { get; set; }
        public virtual DbSet<MedicineGroup> MedicineGroups { get; set; }
        public virtual DbSet<Messege> Messeges { get; set; }
        public virtual DbSet<Patient> Patients { get; set; }
        public virtual DbSet<Solution> Solutions { get; set; }
        public virtual DbSet<Specialization> Specializations { get; set; }
        public virtual DbSet<SubCity> SubCities { get; set; }
    }
}
