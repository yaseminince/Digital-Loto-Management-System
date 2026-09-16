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
    public class RightService : IRightService
    {
        private readonly IRightRepository _repository;
        public RightService(IRightRepository repository)
        {
            _repository = repository;
        }
        public RightResponse Add(RightRequest request)
        {
            var response = new RightResponse
            {
                Entity = new Domain.Entities.Right(),
                Error = new DTO.Base.Error()
            };

            try
            {
                request.Entity.CreatedDate = DateTime.Now;
                response.Entity = _repository.Add(request.Entity);
            }
            catch (Exception ex)
            {
                response.Error.HasException = true;
                response.Error.Message = ex.Message;
            }

            return response;
        }

        public async Task<RightResponse> AddAsync(RightRequest request)
        {
            var response = new RightResponse
            {
                Entity = new Domain.Entities.Right(),
                Error = new DTO.Base.Error()
            };

            try
            {
                request.Entity.CreatedDate = DateTime.Now;
                response.Entity = await _repository.AddAsync(request.Entity);
            }
            catch (Exception ex)
            {
                response.Error.HasException = true;
                response.Error.Message = ex.Message;
            }

            return response;
        }

        public void Delete(RightRequest request)
        {
            request.Entity.UpdatedDate = DateTime.Now;
            _repository.Update(request.Entity);
        }

        public async Task DeleteAsync(RightRequest request)
        {
            request.Entity.UpdatedDate = DateTime.Now;
            await _repository.UpdateAsync(request.Entity);
        }

        public RightResponse GetByFilter(RightRequest request)
        {
            var response = new RightResponse
            {
                Entity = new Domain.Entities.Right(),
                Error = new DTO.Base.Error()
            };

            try
            {
                response.Entity = _repository.Get(x => x.Id == request.Entity.Id);
            }
            catch (Exception ex)
            {
                response.Error.HasException = true;
                response.Error.Message = ex.Message;
            }

            return response;
        }

        public RightResponse GetById(int id)
        {
            var response = new RightResponse
            {
                Entity = new Domain.Entities.Right(),
                Error = new DTO.Base.Error()
            };

            try
            {
                response.Entity = _repository.Get(x => x.Id == id);
            }
            catch (Exception ex)
            {
                response.Error.HasException = true;
                response.Error.Message = ex.Message;
            }

            return response;
        }

        public RightResponse GetList()
        {
            var response = new RightResponse
            {
                EntityList = new List<Domain.Entities.Right>(),
                Error = new DTO.Base.Error()
            };

            try
            {
                response.EntityList = _repository.GetList();
            }
            catch (Exception ex)
            {
                response.Error.HasException = true;
                response.Error.Message = ex.Message;
            }

            return response;
        }

        public RightResponse Update(RightRequest request)
        {
            var response = new RightResponse
            {
                Entity = new Domain.Entities.Right(),
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

        public async Task<RightResponse> UpdateAsync(RightRequest request)
        {
            var response = new RightResponse
            {
                Entity = new Domain.Entities.Right(),
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
