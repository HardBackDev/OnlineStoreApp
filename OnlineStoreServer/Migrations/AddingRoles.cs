using AspNetCore.Identity.Dapper.Models;
using FluentMigrator;
using System.Collections.Generic;

namespace OnlineStoreServer.Migrations
{
    [Migration(3)]
    public class AddingRoles : Migration
    {   
        public override void Down()
        {
            Delete.FromTable("AspNetRoles")
            .Row(new ApplicationRole
            {
                Name = "User",
                NormalizedName = "USER"
            })
            .Row(new ApplicationRole
            {
                Name = "Admin",
                NormalizedName = "ADMIN"
            });
        }
        public override void Up()
        {
            Insert.IntoTable("AspNetRoles")
            .Row(new ApplicationRole
            {
                Name = "User",
                NormalizedName = "USER"
            })
            .Row(new ApplicationRole
            {
                Name = "Admin",
                NormalizedName = "ADMIN"
            });
        }
    }
}
