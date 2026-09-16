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
    public class RoleService : IRoleService
    {
        private readonly RoleManager<Domain.Entities.Role> _roleManager;

        public RoleService(RoleManager<Domain.Entities.Role> roleManager)
        {
            _roleManager = roleManager;
        }

        public RoleResponse Add(RoleRequest request)
        {
            var response = new RoleResponse
            {
                Entity = new Domain.Entities.Role(),
                Error = new DTO.Base.Error()
            };

            try
            {
                request.Entity.CreatedDate = DateTime.Now;

                var result = _roleManager.CreateAsync(request.Entity).Result;

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

        public async Task<RoleResponse> AddAsync(RoleRequest request)
        {
            var response = new RoleResponse
            {
                Entity = new Domain.Entities.Role(),
                Error = new DTO.Base.Error()
            };

            try
            {
                request.Entity.CreatedDate = DateTime.Now;

                var result = await _roleManager.CreateAsync(request.Entity);

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

        public void Delete(RoleRequest request)
        {
            request.Entity.UpdatedDate = DateTime.Now;

            _roleManager.DeleteAsync(request.Entity).Wait();
        }

        public async Task DeleteAsync(RoleRequest request)
        {
            request.Entity.UpdatedDate = DateTime.Now;

            await _roleManager.DeleteAsync(request.Entity);
        }

        public RoleResponse GetByFilter(RoleRequest request)
        {
            var response = new RoleResponse
            {
                Entity = new Domain.Entities.Role(),
                Error = new DTO.Base.Error()
            };

            try
            {
                response.Entity = _roleManager.Roles
                    .FirstOrDefault(x =>
                        x.Name == request.Entity.Name);
            }
            catch (Exception ex)
            {
                response.Error.HasException = true;
                response.Error.Message = ex.Message;
            }

            return response;
        }

        public RoleResponse GetById(int id)
        {
            var response = new RoleResponse
            {
                Entity = new Domain.Entities.Role(),
                Error = new DTO.Base.Error()
            };

            try
            {
                response.Entity = _roleManager.Roles
                    .FirstOrDefault(x => x.Id == id);
            }
            catch (Exception ex)
            {
                response.Error.HasException = true;
                response.Error.Message = ex.Message;
            }

            return response;
        }

        public RoleResponse GetList()
        {
            var response = new RoleResponse
            {
                EntityList = new List<Domain.Entities.Role>(),
                Error = new DTO.Base.Error()
            };

            try
            {
                response.EntityList = _roleManager.Roles.ToList();
            }
            catch (Exception ex)
            {
                response.Error.HasException = true;
                response.Error.Message = ex.Message;
            }

            return response;
        }

        public RoleResponse Update(RoleRequest request)
        {
            var response = new RoleResponse
            {
                Entity = new Domain.Entities.Role(),
                Error = new DTO.Base.Error()
            };

            try
            {
                request.Entity.UpdatedDate = DateTime.Now;

                var result = _roleManager.UpdateAsync(request.Entity).Result;

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

        public async Task<RoleResponse> UpdateAsync(RoleRequest request)
        {
            var response = new RoleResponse
            {
                Entity = new Domain.Entities.Role(),
                Error = new DTO.Base.Error()
            };

            try
            {
                request.Entity.UpdatedDate = DateTime.Now;

                var result = await _roleManager.UpdateAsync(request.Entity);

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
