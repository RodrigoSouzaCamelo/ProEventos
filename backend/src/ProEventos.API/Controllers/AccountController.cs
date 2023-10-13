using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProEventos.Application.DTOs;
using ProEventos.Application.Interfaces;
using System;
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

        [AllowAnonymous]
        [HttpGet("user/{userName}")]
        public async Task<IActionResult> GetUser(string userName)
        {
            try
            {
                var user = _accountService.GetUserByUserNameAsync(userName);
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
    }
}
