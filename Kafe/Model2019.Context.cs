﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Kafe
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Database2019EntitiesRevision : DbContext
    {
        public Database2019EntitiesRevision()
            : base("name=Database2019EntitiesRevision")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Material> Materials { get; set; }
        public virtual DbSet<Member> Members { get; set; }
        public virtual DbSet<Menu> Menus { get; set; }
        public virtual DbSet<Recipe> Recipes { get; set; }
        public virtual DbSet<RoleUser> RoleUsers { get; set; }
        public virtual DbSet<UserView> UserViews { get; set; }
    }
}
