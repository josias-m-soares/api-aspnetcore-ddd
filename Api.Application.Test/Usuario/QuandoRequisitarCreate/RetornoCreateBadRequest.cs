﻿using System;
using System.Threading.Tasks;
using Application.Controllers;
using Domain.DTOs.User;
using Domain.Interfaces.Services.Users;
using Faker;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Api.Application.Test.Usuario.QuandoRequisitarCreate
{
    public class RetornoCreateBadRequest
    {
        private UsersController _controller;

        [Fact(DisplayName = "É possivel realizar o Created na UsersController e retornar BadRequest")]
        public async Task E_Possivel_Invocar_a_UsersController_Create_Retornando_BadRequest()
        {
            var serviceMock = new Mock<IUserService>();
            var name = Name.FullName();
            var email = Internet.Email();

            serviceMock.Setup(m => m.Post(It.IsAny<UserDtoCreate>())).ReturnsAsync(
                new UserDtoCreateResult
                {
                    Id = Guid.NewGuid(),
                    Name = name,
                    Email = email,
                    CreateAt = DateTime.UtcNow
                });
            
            _controller = new UsersController(serviceMock.Object);
            _controller.ModelState.AddModelError("Email", "É um campo Obrigatório");
            
            Mock<IUrlHelper> url = new Mock<IUrlHelper>();
            url.Setup(x => x.Link(It.IsAny<string>(), It.IsAny<Object>())).Returns("http://localhost:5000");
            _controller.Url = url.Object;

            var userDtoCreated = new UserDtoCreate
            {
                Name = name,
                Email = email,
            };

            var result = await _controller.Post(userDtoCreated);
            Assert.True(result is BadRequestObjectResult);
        }
        
    }
}