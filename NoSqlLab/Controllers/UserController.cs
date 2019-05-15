using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NoSqlLab.Models.Persistance;
using NoSqlLab.Services.Repositories;
using NoSqlLab.ViewModels;

namespace NoSqlLab.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserRepository _userRepository;
        public UserController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost]
        public IActionResult Post(UserApiModel model)
        {
            var dbUser = _userRepository.Insert(new User
            {
                UserName = model.UserName,
                Password = model.Password
            });
            return Ok(new UserResponseModel { Id = dbUser.Id, UserName = dbUser.UserName });
        }

        [HttpPut]
        public IActionResult Update(User user)
        {
            _userRepository.Update(user);

            return NoContent();
        }

        [HttpGet]
        public IActionResult Get()
        {
            var users = _userRepository.GetAll().Select(
            x => new UserResponseModel
            {
                Id = x.Id,
                UserName = x.UserName
            });
            return Ok(users);
        }
    }
}