﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Web.Http;
using Microsoft.Azure.Mobile.Server;
using Microsoft.Azure.Mobile.Server.Authentication;
using Microsoft.Azure.Mobile.Server.Config;
using ITW_MobileAppService.DataObjects;
using ITW_MobileAppService.Models;
using Owin;

namespace ITW_MobileAppService
{
    public partial class Startup
    {
        public static void ConfigureMobileApp(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();

            //For more information on Web API tracing, see http://go.microsoft.com/fwlink/?LinkId=620686 
            config.EnableSystemDiagnosticsTracing();

            new MobileAppConfiguration()
                .UseDefaultConfiguration()
                .ApplyTo(config);

            // Use Entity Framework Code First to create database tables based on your DbContext
            Database.SetInitializer(new ITW_MobileAppInitializer());

            // To prevent Entity Framework from modifying your database schema, use a null database initializer
            // Database.SetInitializer<ITW_MobileAppContext>(null);

            MobileAppSettingsDictionary settings = config.GetMobileAppSettingsProvider().GetMobileAppSettings();

            if (string.IsNullOrEmpty(settings.HostName))
            {
                // This middleware is intended to be used locally for debugging. By default, HostName will
                // only have a value when running in an App Service application.
                app.UseAppServiceAuthentication(new AppServiceAuthenticationOptions
                {
                    SigningKey = ConfigurationManager.AppSettings["SigningKey"],
                    ValidAudiences = new[] { ConfigurationManager.AppSettings["ValidAudience"] },
                    ValidIssuers = new[] { ConfigurationManager.AppSettings["ValidIssuer"] },
                    TokenHandler = config.GetAppServiceTokenHandler()
                });
            }
            app.UseWebApi(config);
        }
    }

    public class ITW_MobileAppInitializer : CreateDatabaseIfNotExists<ITW_MobileAppContext>
    {
        protected override void Seed(ITW_MobileAppContext context)
        {
            List<EmployeeItem> employeeItems = new List<EmployeeItem>
            
            {
                new EmployeeItem { Id = Guid.NewGuid().ToString(), EmployeeID = 2, Name = "Employee One", Department = "Test", PrivledgeLevel ="User", Email = "test@gmail.com" },
                new EmployeeItem { Id = Guid.NewGuid().ToString(), EmployeeID = 2, Name = "Employee One", Department = "Test", PrivledgeLevel ="User", Email = "test@gmail.com" },
            };

            foreach (EmployeeItem employeeItem in employeeItems)
            {
                context.Set<EmployeeItem>().Add(employeeItem);
            }

            base.Seed(context);
        }
    }
}

