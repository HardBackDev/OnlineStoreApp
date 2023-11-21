using AspNetCore.Identity.Dapper.Models;
using Entities.Models;
using FluentMigrator;
using Microsoft.AspNetCore.Identity;
using OnlineStoreServer.Presentation.Cotrollers;
using Shared.Dto.UserDtos;
using Shared.utilities;

namespace OnlineStoreServer.Migrations
{
    [Migration(4)]
    public class AddingUsers : Migration
    {
        AuthenticationController _controller;
        private readonly UserManager<ApplicationUser> _userManager;

        public AddingUsers(AuthenticationController controller, UserManager<ApplicationUser> userManager)
        {
            _controller = controller;
            _userManager = userManager;
        }

        public override void Down()
        {
        }

        public override async void Up()
        {
            var user = new ApplicationUser
            {
                Email = "admin@gmail.com",
                UserName = "Admin",
                PhoneNumber = "5555"
            };

            var adminCreating = _userManager.CreateAsync(user, "Password1000")
                .ContinueWith(r => _userManager.AddToRoleAsync(user, "Admin"));

            var userCreating = _controller.RegisterUser(new UserForRegistrationDto()
            {
                UserName = "User",
                Password = "Password1000",
                PhoneNumber = "1234",
                Email = "User@gmail.com",
                Roles = new[] { "User" }
            });

            Task.WaitAll(adminCreating, userCreating);

        }
    }
}
