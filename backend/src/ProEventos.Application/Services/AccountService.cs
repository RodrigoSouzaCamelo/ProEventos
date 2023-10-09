using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProEventos.Application.DTOs;
using ProEventos.Application.Interfaces;
using ProEventos.Domain.Identity;
using ProEventos.Domain.Interfaces.Repositories;
using System;
using System.Threading.Tasks;

namespace ProEventos.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountService(UserManager<User> userManager, 
                                SignInManager<User> signInManager, 
                                IMapper mapper,
                                IUserRepository userRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _userRepository = userRepository;
        }
        public async Task<SignInResult> CheckUserPasswordAsync(UserUpdateDTO userUpdateDto, string password)
        {
            try
            {
                var user = await _userManager.Users
                                             .SingleOrDefaultAsync(user => user.UserName == userUpdateDto.UserName.ToLower());

                return await _signInManager.CheckPasswordSignInAsync(user, password, false);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao tentar verificar password. Erro: {ex.Message}");
            }
        }

        public async Task<UserDTO> CreateAccountAsync(UserDTO userDto)
        {
            try
            {
                var user = _mapper.Map<User>(userDto);
                var result = await _userManager.CreateAsync(user, userDto.Password);

                if (result.Succeeded)
                {
                    var userToReturn = _mapper.Map<UserDTO>(user);
                    return userToReturn;
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao tentar Criar Usuário. Erro: {ex.Message}");
            }
        }

        public async Task<UserUpdateDTO> GetUserByUserNameAsync(string userName)
        {
            try
            {
                var user = await _userRepository.GetUserByNameAsync(userName);
                if (user == null) return null;

                var userUpdateDto = _mapper.Map<UserUpdateDTO>(user);
                return userUpdateDto;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao tentar pegar Usuário por Username. Erro: {ex.Message}");
            }
        }

        public async Task<UserUpdateDTO> UpdateAccount(UserUpdateDTO userUpdateDto)
        {
            try
            {
                var user = await _userRepository.GetUserByNameAsync(userUpdateDto.UserName);
                if (user == null) return null;

                userUpdateDto.Id = user.Id;

                _mapper.Map(userUpdateDto, user);

                if (userUpdateDto.Password != null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    await _userManager.ResetPasswordAsync(user, token, userUpdateDto.Password);
                }

                _userRepository.Update(user);

                if (await _userRepository.SaveChangesAsync())
                {
                    var userRetorno = await _userRepository.GetUserByNameAsync(user.UserName);

                    return _mapper.Map<UserUpdateDTO>(userRetorno);
                }

                return null;
            }
            catch (System.Exception ex)
            {
                throw new Exception($"Erro ao tentar atualizar usuário. Erro: {ex.Message}");
            }
        }

        public async Task<bool> UserExists(string userName)
        {
            try
            {
                return await _userManager.Users
                                         .AnyAsync(user => user.UserName == userName.ToLower());
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao verificar se usuário existe. Erro: {ex.Message}");
            }
        }
    }
}

