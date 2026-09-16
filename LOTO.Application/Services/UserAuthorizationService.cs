using LOTO.Application.Interfaces;
using LOTO.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOTO.Application.Services
{
    public class UserAuthorizationService : IUserAuthorizationService // ne yapabilir?
    {
        private readonly UserManager<User> _userManager;

        public UserAuthorizationService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<bool> HasRightAsync(int userId, string rightName)
        {
            var user = await _userManager.Users // user kayıtlarına erişiyor
                .Include(x => x.Role) // rol bilgisini de getir
                .ThenInclude(x => x.Rights) // bir de right bilgisini getir
                .FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null)
                return false;

            // userın rolündeki rightlarda istenen right bulunuyor mu kontrol eder
            return user.Role.Rights
                .Any(x => x.Name == rightName);
        }
    }
}
