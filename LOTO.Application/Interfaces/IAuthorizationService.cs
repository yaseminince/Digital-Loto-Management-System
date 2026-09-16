using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOTO.Application.Interfaces
{
    public interface IUserAuthorizationService
    {
        Task<bool> HasRightAsync(int userId, string rightName);
        // kullanıcının bu righta hakkı var mı true false döndürüyor (hangi kullanıcı,hangi yetki)
    }
}
