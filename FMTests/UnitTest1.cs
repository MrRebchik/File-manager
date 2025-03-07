using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using FileManagerAPI.Controllers;
using FileManagerLibrary.Models;
using Microsoft.EntityFrameworkCore;
using FileManagerAPI.Models;

namespace FMTests
{
    public class UnitTest1
    {
        private readonly DbContextOptions<PeopleContext> _options;

        public UnitTest1()
        {
            _options = new DbContextOptionsBuilder<PeopleContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
        }

        [Fact]
        public void Index_ValidCredentials_ReturnsOkWithToken()
        {
            using (var context = new PeopleContext(_options))
            {
                var person = new Person("test@example.com", "password");
                context.People.Add(person);
                context.SaveChanges();

                var controller = new AuthController();

                var result = controller.Index("test@example.com", "password", context);

                var okResult = Assert.IsType<JsonResult>(result);
                Assert.NotNull(okResult.Value);
            }
        }

        [Fact]
        public void Index_InvalidCredentials_ReturnsUnauthorized()
        {
            using (var context = new PeopleContext(_options))
            {
                var person = new Person("test@example.com", "password");
                context.People.Add(person);
                context.SaveChanges();

                var controller = new AuthController();

                var result = controller.Index("wrong@example.com", "wrongpassword", context);

                Assert.IsType<UnauthorizedResult>(result);
            }
        }

        [Fact]
        public void Register_NewUser_ReturnsOkWithToken()
        {
            using (var context = new PeopleContext(_options))
            {
                var controller = new AuthController();

                var result = controller.Register("newuser@example.com", "newpassword", context);

                var okResult = Assert.IsType<JsonResult>(result);
                Assert.NotNull(okResult.Value);

                var user = context.People.SingleOrDefault(p => p.Email == "newuser@example.com");
                Assert.NotNull(user);
            }
        }

        [Fact]
        public void Register_ExistingUser_ReturnsBadRequest()
        {
            using (var context = new PeopleContext(_options))
            {
                var person = new Person("existing@example.com", "password");
                context.People.Add(person);
                context.SaveChanges();

                var controller = new AuthController();

                var result = controller.Register("existing@example.com", "password", context);

                Assert.IsType<BadRequestResult>(result);
            }
        }
    }
}