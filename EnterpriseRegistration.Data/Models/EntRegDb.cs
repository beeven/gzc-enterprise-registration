namespace EnterpriseRegistration.Data.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using System.Linq;

    public class EntRegDb : DbContext
    {
        // Your context has been configured to use a 'Models' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'EnterpriseRegistration.Data.Models.Models' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'Models' 
        // connection string in the application configuration file.
        public EntRegDb()
            : base("name=EnterpriseRegistration")
        {
            //Database.SetInitializer<EntRegDb>(null);
        }



        // public virtual DbSet<MyEntity> MyEntities { get; set; }

        public virtual DbSet<Attachment> Attachments { get; set; }
        public virtual DbSet<RegInfo> RegInfoes { get; set; }
        public virtual DbSet<Registration> Registrations { get; set; }
        public virtual DbSet<SystemLog> SystemLogs { get; set; }
        public virtual DbSet<Customs> Customs { get; set; }
        public virtual DbSet<CustomRights> CustomRights { get; set; }
        public virtual DbSet<CustomMap> CustomMap { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }
    }
}