﻿using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using Microsoft.Azure.Mobile.Server;
using Microsoft.Azure.Mobile.Server.Tables;
using ITW_MobileAppService.DataObjects;

namespace ITW_MobileAppService.Models
{
    public class ITW_MobileAppContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to alter your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx

        private const string connectionStringName = "ITW-EventDatabase";

        public ITW_MobileAppContext() : base(connectionStringName)
        {
        } 

        //public DbSet<TodoItem> TodoItems { get; set; }
        public DbSet<EmployeeItem> EmployeeItems { get; set; }
        public DbSet<EventItem> EventItems { get; set; }
        public DbSet<RecipientListItem> RecipientListItems { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            
            modelBuilder.Conventions.Add(
                new AttributeToColumnAnnotationConvention<TableColumnAttribute, string>(
                    "ServiceTableColumn", (property, attributes) => attributes.Single().ColumnType.ToString()));

        }
    }

}
