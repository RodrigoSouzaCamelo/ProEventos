using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProEventos.API.Extensions;
using ProEventos.Application.DTOs;
using ProEventos.Application.Interfaces;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ProEventos.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService, ITokenService tokenService)
        {
            _tokenService = tokenService;
            _accountService = accountService;
        }

        [HttpGet("user")]
        public async Task<IActionResult> GetUser()
        {
            try
            {
                var userName = User.GetUserName();
                var user = await _accountService.GetUserByUserNameAsync(userName);

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao tentar recuperar evento! Erro: {ex.Message}");
            }
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDTO userDTO)
        {
            try
            {
                if (await _accountService.UserExists(userDTO.UserName)) 
                    return BadRequest("Usuário já existe");

                UserDTO user = await _accountService.CreateAccountAsync(userDTO);
                if (user != null) return Ok(user);

                return BadRequest("Não foi possível cadastrar o usuário.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao tentar recuperar evento! Erro: {ex.Message}");
            }
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO userLoginDTO)
        {
            try
            {
                var user = await _accountService.GetUserByUserNameAsync(userLoginDTO.UserName);
                if (user == null) return Unauthorized("Usuário ou senha está incoreto.");

                var result = await _accountService.CheckUserPasswordAsync(user, userLoginDTO.Password);

                return Ok(new
                {
                    user.UserName,
                    user.PrimeiroNome,
                    Token = await _tokenService.CreateToken(user)
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao tentar recuperar evento! Erro: {ex.Message}");
            }
        }

        [HttpPut("user")]
        public async Task<IActionResult> UpdateUser([FromBody] UserUpdateDTO userUpdateDTO)
        {
            try
            {
                var user = await _accountService.GetUserByUserNameAsync(User.GetUserName());
                if (user == null) return Unauthorized("Usuário inválido.");

                userUpdateDTO.Id = user.Id;

                var result = await _accountService.UpdateAccount(userUpdateDTO);
                if (result == null) return NoContent();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao tentar atualizar usuário! Erro: {ex.Message}");
            }
        }
    }
}
