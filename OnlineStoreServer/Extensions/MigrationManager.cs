﻿using CompanyEmployees.Migrations;
using FluentMigrator.Runner;
using LoggerService;
using OnlineStoreServer.Migrations;

namespace OnlineStoreServer.Extensions
{
    public static class MigrationManager
    {
        public static WebApplication MigrateDatabase(this WebApplication app, ILoggerManager logger)
        {
            using (var scope = app.Services.CreateScope())
            {
                var databaseService = scope.ServiceProvider.GetRequiredService<Database>();
                var migrationService = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();

                try
                {
                    //migrationService.MigrateDown(-1);
                    databaseService.CreateDatabase("products");
                    migrationService.ListMigrations();
                    migrationService.MigrateUp();
                }
                catch (Exception ex)
                {
                    logger.LogError($"Exception occurred during the database creation: {ex}");
                    throw;
                }
            }

            return app;
        }
    }
}
