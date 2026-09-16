using LOTO.Application.DTO.Request;
using LOTO.Application.DTO.Response;
using LOTO.Application.Interfaces;
using LOTO.Application.Interfaces.Repository;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOTO.Application.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<Domain.Entities.User> _userManager;

        public UserService(UserManager<Domain.Entities.User> userManager)
        {
            _userManager = userManager;
        }

        public UserResponse Add(UserRequest request)
        {
            var response = new UserResponse
            {
                Entity = new Domain.Entities.User(),
                Error = new DTO.Base.Error()
            };

            try
            {
                request.Entity.CreatedDate = DateTime.Now;
                request.Entity.IsActive = true;

                var result = _userManager.CreateAsync(request.Entity).Result;

                if (!result.Succeeded)
                {
                    response.Error.HasException = true;
                    response.Error.Message = string.Join(", ",
                        result.Errors.Select(x => x.Description));
                }
                else
                {
                    response.Entity = request.Entity;
                }
            }
            catch (Exception ex)
            {
                response.Error.HasException = true;
                response.Error.Message = ex.Message;
            }

            return response;
        }

        public async Task<UserResponse> AddAsync(UserRequest request)
        {
            var response = new UserResponse
            {
                Entity = new Domain.Entities.User(),
                Error = new DTO.Base.Error()
            };

            try
            {
                request.Entity.CreatedDate = DateTime.Now;
                request.Entity.IsActive = true;

                var result = await _userManager.CreateAsync(request.Entity);

                if (!result.Succeeded)
                {
                    response.Error.HasException = true;
                    response.Error.Message = string.Join(", ",
                        result.Errors.Select(x => x.Description));
                }
                else
                {
                    response.Entity = request.Entity;
                }
            }
            catch (Exception ex)
            {
                response.Error.HasException = true;
                response.Error.Message = ex.Message;
            }

            return response;
        }

        public void Delete(UserRequest request)
        {
            request.Entity.IsActive = false;
            request.Entity.UpdatedDate = DateTime.Now;

            _userManager.UpdateAsync(request.Entity).Wait();
        }

        public async Task DeleteAsync(UserRequest request)
        {
            request.Entity.IsActive = false;
            request.Entity.UpdatedDate = DateTime.Now;

            await _userManager.UpdateAsync(request.Entity);
        }

        public UserResponse GetByFilter(UserRequest request)
        {
            var response = new UserResponse
            {
                Entity = new Domain.Entities.User(),
                Error = new DTO.Base.Error()
            };

            try
            {
                response.Entity = _userManager.Users
                    .FirstOrDefault(x =>
                        x.Name == request.Entity.Name &&
                        x.IsActive);
            }
            catch (Exception ex)
            {
                response.Error.HasException = true;
                response.Error.Message = ex.Message;
            }

            return response;
        }

        public UserResponse GetById(int id)
        {
            var response = new UserResponse
            {
                Entity = new Domain.Entities.User(),
                Error = new DTO.Base.Error()
            };

            try
            {
                response.Entity = _userManager.Users
                    .FirstOrDefault(x =>
                        x.Id == id &&
                        x.IsActive);
            }
            catch (Exception ex)
            {
                response.Error.HasException = true;
                response.Error.Message = ex.Message;
            }

            return response;
        }

        public UserResponse GetList()
        {
            var response = new UserResponse
            {
                EntityList = new List<Domain.Entities.User>(),
                Error = new DTO.Base.Error()
            };

            try
            {
                response.EntityList = _userManager.Users
                    .Where(x => x.IsActive)
                    .ToList();
            }
            catch (Exception ex)
            {
                response.Error.HasException = true;
                response.Error.Message = ex.Message;
            }

            return response;
        }

        public UserResponse Update(UserRequest request)
        {
            var response = new UserResponse
            {
                Entity = new Domain.Entities.User(),
                Error = new DTO.Base.Error()
            };

            try
            {
                request.Entity.UpdatedDate = DateTime.Now;

                var result = _userManager.UpdateAsync(request.Entity).Result;

                if (!result.Succeeded)
                {
                    response.Error.HasException = true;
                    response.Error.Message = string.Join(", ",
                        result.Errors.Select(x => x.Description));
                }
                else
                {
                    response.Entity = request.Entity;
                }
            }
            catch (Exception ex)
            {
                response.Error.HasException = true;
                response.Error.Message = ex.Message;
            }

            return response;
        }

        public async Task<UserResponse> UpdateAsync(UserRequest request)
        {
            var response = new UserResponse
            {
                Entity = new Domain.Entities.User(),
                Error = new DTO.Base.Error()
            };

            try
            {
                request.Entity.UpdatedDate = DateTime.Now;

                var result = await _userManager.UpdateAsync(request.Entity);

                if (!result.Succeeded)
                {
                    response.Error.HasException = true;
                    response.Error.Message = string.Join(", ",
                        result.Errors.Select(x => x.Description));
                }
                else
                {
                    response.Entity = request.Entity;
                }
            }
            catch (Exception ex)
            {
                response.Error.HasException = true;
                response.Error.Message = ex.Message;
            }

            return response;
        }
    }
}
