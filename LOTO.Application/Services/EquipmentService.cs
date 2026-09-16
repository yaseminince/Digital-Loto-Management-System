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
    public class EquipmentService : IEquipmentService
    {
        private readonly IEquipmentRepository _repository;
        public EquipmentService(IEquipmentRepository repository)
        {
            _repository = repository;
        }
        public EquipmentResponse Add(EquipmentRequest request)
        {
            var response = new EquipmentResponse
            {
                Entity = new Domain.Entities.Equipment(),
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

        public async Task<EquipmentResponse> AddAsync(EquipmentRequest request)
        {
            var response = new EquipmentResponse
            {
                Entity = new Domain.Entities.Equipment(),
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

        public void Delete(EquipmentRequest request)
        {
            request.Entity.IsActive = false;
            request.Entity.UpdatedDate = DateTime.Now;

            _repository.Update(request.Entity);
        }

        public async Task DeleteAsync(EquipmentRequest request)
        {
            request.Entity.IsActive = false;
            request.Entity.UpdatedDate = DateTime.Now;

            await _repository.UpdateAsync(request.Entity);
        }

        public EquipmentResponse GetByFilter(EquipmentRequest request)
        {
            var response = new EquipmentResponse
            {
                Entity = new Domain.Entities.Equipment(),
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

        public EquipmentResponse GetById(int id)
        {
            var response = new EquipmentResponse
            {
                Entity = new Domain.Entities.Equipment(),
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

        public EquipmentResponse GetList()
        {
            var response = new EquipmentResponse
            {
                EntityList = new List<Domain.Entities.Equipment>(),
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

        public EquipmentResponse Update(EquipmentRequest request)
        {
            var response = new EquipmentResponse
            {
                Entity = new Domain.Entities.Equipment(),
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

        public async Task<EquipmentResponse> UpdateAsync(EquipmentRequest request)
        {
            var response = new EquipmentResponse
            {
                Entity = new Domain.Entities.Equipment(),
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
