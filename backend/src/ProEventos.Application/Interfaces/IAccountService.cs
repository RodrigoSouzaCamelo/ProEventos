﻿using Microsoft.AspNetCore.Identity;
using ProEventos.Application.DTOs;
using System.Threading.Tasks;

namespace ProEventos.Application.Interfaces
{
    public interface IAccountService
    {
        Task<bool> UserExists(string userName);
        Task<UserUpdateDTO> GetUserByUserNameAsync(string userName);
        Task<SignInResult> CheckUserPasswordAsync(UserUpdateDTO userUpdateDTO, string password);
        Task<UserUpdateDTO> CreateAccountAsync(UserDTO userDTO);
        Task<UserUpdateDTO> Update(UserUpdateDTO userUpdateDTO);
    }
}
