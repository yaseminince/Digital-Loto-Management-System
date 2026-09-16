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
    public class PlantService : IPlantService
    {
        private readonly IPlantRepository _repository;
        public PlantService(IPlantRepository repository)
        {
            _repository = repository;
        }
        public PlantResponse Add(PlantRequest request)
        {
            var response = new PlantResponse
            {
                Entity = new Domain.Entities.Plant(),
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

        public async Task<PlantResponse> AddAsync(PlantRequest request)
        {
            var response = new PlantResponse
            {
                Entity = new Domain.Entities.Plant(),
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

        public void Delete(PlantRequest request)
        {
            request.Entity.IsActive = false;
            request.Entity.UpdatedDate = DateTime.Now;

            _repository.Update(request.Entity);
        }

        public async Task DeleteAsync(PlantRequest request)
        {
            request.Entity.IsActive = false;
            request.Entity.UpdatedDate = DateTime.Now;

            await _repository.UpdateAsync(request.Entity);
        }

        public PlantResponse GetByFilter(PlantRequest request)
        {
            var response = new PlantResponse
            {
                Entity = new Domain.Entities.Plant(),
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

        public PlantResponse GetById(int id)
        {
            var response = new PlantResponse
            {
                Entity = new Domain.Entities.Plant(),
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

        public PlantResponse GetList()
        {
            var response = new PlantResponse
            {
                EntityList = new List<Domain.Entities.Plant>(),
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

        public PlantResponse Update(PlantRequest request)
        {
            var response = new PlantResponse
            {
                Entity = new Domain.Entities.Plant(),
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

        public async Task<PlantResponse> UpdateAsync(PlantRequest request)
        {
            var response = new PlantResponse
            {
                Entity = new Domain.Entities.Plant(),
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
