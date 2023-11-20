using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProEventos.API.Extensions;
using ProEventos.API.Helpers;
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
        private const string DESTINO = "Perfil";

        private readonly IUtil _util;
        private readonly ITokenService _tokenService;
        private readonly IAccountService _accountService;

        public AccountController(IUtil util, ITokenService tokenService, IAccountService accountService)
        {
            _util = util;
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

                UserUpdateDTO user = await _accountService.CreateAccountAsync(userDTO);
                if (user != null)
                    return Ok(new
                    {
                        user.UserName,
                        user.PrimeiroNome,
                        Token = await _tokenService.CreateToken(user)
                    }); 

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
                if (!result.Succeeded) return Unauthorized();

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
                if (user == null || userUpdateDTO.UserName != User.GetUserName()) 
                    return Unauthorized("Usuário inválido.");

                userUpdateDTO.Id = user.Id;

                var usuarioAtualizado = await _accountService.Update(userUpdateDTO);
                if (usuarioAtualizado == null) return NoContent();

                return Ok(new
                {
                    usuarioAtualizado.UserName,
                    usuarioAtualizado.PrimeiroNome,
                    Token = await _tokenService.CreateToken(usuarioAtualizado)
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao tentar atualizar usuário! Erro: {ex.Message}");
            }
        }

        [HttpPost("upload-image")]
        [ProducesResponseType(typeof(EventoDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UploadImage()
        {
            try
            {
                var user = await _accountService.GetUserByUserNameAsync(User.GetUserName());

                if (user == null) return NoContent();

                var file = Request.Form.Files[0];
                if (file.Length > 0)
                {
                    _util.DeleteImage(user.ImagemURL, DESTINO);
                    user.ImagemURL = await _util.SaveImage(file, DESTINO);
                }

                var retorno = await _accountService.Update(user);

                return Ok(retorno);
            }
            catch (Exception)
            {
                return StatusCode(500, "Erro ao tentar realizar upload da foto do usuário!");
            }
        }
    }
}
