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
    public class LocationService : ILocationService
    {
        private readonly ILocationRepository _repository;
        public LocationService(ILocationRepository repository)
        {
            _repository = repository;
        }
        public LocationResponse Add(LocationRequest request)
        {
            var response = new LocationResponse
            {
                Entity = new Domain.Entities.Location(),
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

        public async Task<LocationResponse> AddAsync(LocationRequest request)
        {
            var response = new LocationResponse
            {
                Entity = new Domain.Entities.Location(),
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

        public void Delete(LocationRequest request)
        {
            request.Entity.IsActive = false;
            request.Entity.UpdatedDate = DateTime.Now;

            _repository.Update(request.Entity);
        }

        public async Task DeleteAsync(LocationRequest request)
        {
            request.Entity.IsActive = false;
            request.Entity.UpdatedDate = DateTime.Now;

            await _repository.UpdateAsync(request.Entity);
        }

        public LocationResponse GetByFilter(LocationRequest request)
        {
            var response = new LocationResponse
            {
                Entity = new Domain.Entities.Location(),
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

        public LocationResponse GetById(int id)
        {
            var response = new LocationResponse
            {
                Entity = new Domain.Entities.Location(),
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

        public LocationResponse GetList()
        {
            var response = new LocationResponse
            {
                EntityList = new List<Domain.Entities.Location>(),
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

        public LocationResponse Update(LocationRequest request)
        {
            var response = new LocationResponse
            {
                Entity = new Domain.Entities.Location(),
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

        public async Task<LocationResponse> UpdateAsync(LocationRequest request)
        {
            var response = new LocationResponse
            {
                Entity = new Domain.Entities.Location(),
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
