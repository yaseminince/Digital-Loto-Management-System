using LOTO.Application.DTO.Request;
using LOTO.Application.Interfaces;
using LOTO.Application.Interfaces.Repository;
using LOTO.Domain.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LOTO.Application.Services
{
    public class AuthService : IAuthService // kim?
    {
        private readonly UserManager<User> _userManager; // kullanıcı bulma,oluşturma ve güncelleme
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<Role> _roleManager;

        public AuthService(UserManager<User> userManager,SignInManager<User> signInManager,RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task<bool> LoginAsync(LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email); // emaile göre buluyor

            if (user == null)
                return false;

            if (!user.IsActive)
                return false;

            var result = await _signInManager.PasswordSignInAsync(user,request.Password,
                false, // beni hatırla özelliği kapalı
                false); // başarısız girişte hesabı kilitlemiyor
            return result.Succeeded;
        }

        public async Task<bool> RegisterAsync(RegisterRequest request)
        {
            if (request.Password != request.ConfirmPassword)
                return false;

            if (!request.Email.EndsWith("@loto.com"))
                return false;

            var role = await _roleManager.FindByNameAsync("User");

            if (role == null)
                return false;

            var user = new User
            {
                Name = request.Name,
                UserName = request.Email,
                Email = request.Email,
                IsActive = true,
                RoleId = role.Id,
                CreatedBy = 1,
                CreatedDate = DateTime.Now
            };

            var result = await _userManager.CreateAsync(user,request.Password);
            if (!result.Succeeded)
                return false;
            var roleResult = await _userManager.AddToRoleAsync(user,"User");

            if (!roleResult.Succeeded)
                return false;
            return true;
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
