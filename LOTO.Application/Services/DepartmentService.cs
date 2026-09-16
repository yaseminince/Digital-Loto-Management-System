using LOTO.Application.DTO.Request;
using LOTO.Application.DTO.Response;
using LOTO.Application.Interfaces;
using LOTO.Application.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOTO.Application.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _repository;
        public DepartmentService(IDepartmentRepository repository)
        {
            _repository = repository;
        }
        public DepartmentResponse Add(DepartmentRequest request)
        {
            var response = new DepartmentResponse
            {
                Entity = new Domain.Entities.Department(),
                Error = new DTO.Base.Error()
            };

            try
            {
                request.Entity.CreatedDate = DateTime.Now;
                request.Entity.IsActive = true;

                response.Entity = _repository.Add(request.Entity);
            }
            catch (Exception ex)
            {
                response.Error.HasException = true;
                response.Error.Message = ex.Message;
            }

            return response;
        }

        public async Task<DepartmentResponse> AddAsync(DepartmentRequest request)
        {
            var response = new DepartmentResponse
            {
                Entity = new Domain.Entities.Department(),
                Error = new DTO.Base.Error()
            };

            try
            {
                request.Entity.CreatedDate = DateTime.Now;
                request.Entity.IsActive = true;

                response.Entity = await _repository.AddAsync(request.Entity);
            }
            catch (Exception ex)
            {
                response.Error.HasException = true;
                response.Error.Message = ex.Message;
            }

            return response;
        }

        public void Delete(DepartmentRequest request)
        {
            request.Entity.IsActive = false;
            request.Entity.UpdatedDate = DateTime.Now;

            _repository.Update(request.Entity);
        }

        public async Task DeleteAsync(DepartmentRequest request)
        {
            request.Entity.IsActive = false;
            request.Entity.UpdatedDate = DateTime.Now;

            await _repository.UpdateAsync(request.Entity);
        }

        public DepartmentResponse GetByFilter(DepartmentRequest request)
        {
            var response = new DepartmentResponse
            {
                Entity = new Domain.Entities.Department(),
                Error = new DTO.Base.Error()
            };

            try
            {
                response.Entity = _repository.Get(x =>
                    x.Name == request.Entity.Name && x.IsActive);
            }
            catch (Exception ex)
            {
                response.Error.HasException = true;
                response.Error.Message = ex.Message;
            }

            return response;
        }

        public DepartmentResponse GetById(int id)
        {
            var response = new DepartmentResponse
            {
                Entity = new Domain.Entities.Department(),
                Error = new DTO.Base.Error()
            };

            try
            {
                response.Entity = _repository.Get(x => x.Id == id && x.IsActive);
            }
            catch (Exception ex)
            {
                response.Error.HasException = true;
                response.Error.Message = ex.Message;
            }

            return response;
        }

        public DepartmentResponse GetList()
        {
            var response = new DepartmentResponse
            {
                EntityList = new List<Domain.Entities.Department>(),
                Error = new DTO.Base.Error()
            };

            try
            {
                response.EntityList = _repository.GetList(x => x.IsActive);
            }
            catch (Exception ex)
            {
                response.Error.HasException = true;
                response.Error.Message = ex.Message;
            }

            return response;
        }

        public DepartmentResponse Update(DepartmentRequest request)
        {
            var response = new DepartmentResponse
            {
                Entity = new Domain.Entities.Department(),
                Error = new DTO.Base.Error()
            };

            try
            {
                request.Entity.UpdatedDate = DateTime.Now;

                response.Entity = _repository.Update(request.Entity);
            }
            catch (Exception ex)
            {
                response.Error.HasException = true;
                response.Error.Message = ex.Message;
            }

            return response;
        }

        public async Task<DepartmentResponse> UpdateAsync(DepartmentRequest request)
        {
            var response = new DepartmentResponse
            {
                Entity = new Domain.Entities.Department(),
                Error = new DTO.Base.Error()
            };

            try
            {
                request.Entity.UpdatedDate = DateTime.Now;

                response.Entity = await _repository.UpdateAsync(request.Entity);
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
