using LOTO.Application.DTO.Request;
using LOTO.Application.DTO.Response;
using LOTO.Application.Interfaces;
using LOTO.Application.Interfaces.Repository;
using LOTO.Domain.Entities;
using LOTO.Domain.Enums;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace LOTO.Application.Services
{
    public class LotoService : ILotoService
    {
        private readonly ILotoRepository _repository;
        private readonly IUserAuthorizationService _authorizationService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly INotificationService _notificationService;

        public LotoService(ILotoRepository repository,IUserAuthorizationService authorizationService,
            IHttpContextAccessor httpContextAccessor,INotificationService notificationService)
        {
            _repository = repository;
            _authorizationService = authorizationService;
            _httpContextAccessor = httpContextAccessor;
            _notificationService = notificationService;
        }

        // LOTO oluşturma
        public LotoResponse Add(LotoRequest request)
        {
            var response = new LotoResponse
            {
                Entity = new Loto(),
                Error = new DTO.Base.Error()
            };

            try
            {
                var userIdString = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!int.TryParse(userIdString, out int userId))
                {
                    response.Error.HasException = true;
                    response.Error.Message = "User not found.";
                    return response;
                }

                var hasRight = _authorizationService.HasRightAsync(userId, "CreateLoto").GetAwaiter().GetResult();

                if (!hasRight)
                {
                    response.Error.HasException = true;
                    response.Error.Message ="You do not have permission to create a LOTO.";
                    return response;
                }



                request.Entity.LotoNumber =_repository.GetNextLotoNumber();

                request.Entity.UserId = userId;
                request.Entity.CreatedBy = userId;
                request.Entity.CreatedDate = DateTime.Now;
                request.Entity.Status = LotoStatus.Active;

                response.Entity = _repository.Add(request.Entity);

                _notificationService.CreateAsync(
                    userId,
                    response.Entity.Id,
                    NotificationType.LotoCreated)
                    .GetAwaiter()
                    .GetResult();
            }
            catch (Exception ex)
            {
                response.Error.HasException = true;
                response.Error.Message = ex.Message;
            }

            return response;
        }

        public async Task<LotoResponse> AddAsync(LotoRequest request)
        {
            var response = new LotoResponse
            {
                Entity = new Loto(),
                Error = new DTO.Base.Error()
            };

            try
            {
                //httpcontextaccessorla giriş yapan kullanıcıdan id alınıyo direkt
                var userIdString = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!int.TryParse(userIdString, out int userId))
                {
                    response.Error.HasException = true;
                    response.Error.Message = "User not found.";
                    return response;
                }

                var hasRight = await _authorizationService.HasRightAsync(userId, "CreateLoto");

                if (!hasRight)
                {
                    response.Error.HasException = true;
                    response.Error.Message ="You do not have permission to create a LOTO.";
                    return response;
                }

                var existingLoto = await _repository.GetListAsync(x => x.PlantId == request.Entity.PlantId &&x.DepartmentId == request.Entity.DepartmentId &&
                    x.LocationId == request.Entity.LocationId &&(x.Status == LotoStatus.Active || x.Status == LotoStatus.Overdue));

                if (existingLoto.Any())
                {
                    response.Error.HasException = true;
                    response.Error.Message =
                        "This Plant, Department and Location is currently being used by another LOTO.";

                    return response;
                }

                request.Entity.LotoNumber = _repository.GetNextLotoNumber();

                request.Entity.UserId = userId;
                request.Entity.CreatedBy = userId;
                request.Entity.CreatedDate = DateTime.Now;
                request.Entity.Status = LotoStatus.Active;

                response.Entity =await _repository.AddAsync(request.Entity);

                await _notificationService.CreateAsync(
                    userId,
                    response.Entity.Id,
                    NotificationType.LotoCreated);

            }
            catch (Exception ex)
            {
                response.Error.HasException = true;
                response.Error.Message = ex.Message;
            }

            return response;
        }

        public LotoResponse GetById(int id)
        {
            var response = new LotoResponse
            {
                Entity = new Loto(),
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
        public LotoResponse GetByFilter(LotoRequest request)
        {
            var response = new LotoResponse
            {
                Entity = new Loto(),
                Error = new DTO.Base.Error()
            };

            try
            {
                response.Entity = _repository.Get(x =>x.Id == request.Entity.Id);
            }
            catch (Exception ex)
            {
                response.Error.HasException = true;
                response.Error.Message = ex.Message;
            }

            return response;
        }

        // LOTO listesini getirir. süresi geçen varsa overdue yapar
        public async Task<LotoResponse> GetListAsync()
        {
            var response = new LotoResponse
            {
                EntityList = new List<Loto>(),
                Error = new DTO.Base.Error()
            };

            try
            {
                await UpdateOverdueLotosAsync();

                var userIdString = _httpContextAccessor.HttpContext?
                    .User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!int.TryParse(userIdString, out int userId))
                {
                    response.Error.HasException = true;
                    response.Error.Message = "User not found.";
                    return response;
                }

                var canViewAll =
                    await _authorizationService.HasRightAsync(userId, "ViewAllLoto");

                var lotos = await _repository.GetListWithDetailsAsync();

                if (canViewAll)
                {
                    response.EntityList = lotos;
                    return response;
                }

                var canViewOwn =
                    await _authorizationService.HasRightAsync(userId, "ViewOwnLoto");

                if (!canViewOwn)
                {
                    response.Error.HasException = true;
                    response.Error.Message =
                        "You do not have permission to view LOTO records.";

                    return response;
                }

                response.EntityList = lotos
                    .Where(x => x.UserId == userId)
                    .ToList();
            }
            catch (Exception ex)
            {
                response.Error.HasException = true;
                response.Error.Message = ex.Message;
            }

            return response;
        }

        // Süresi geçen Active LOTO'ları otomatik olarak Overdue yapar.
        private async Task UpdateOverdueLotosAsync()
        {
            var lotos = await _repository.GetListAsync(x =>x.Status == LotoStatus.Active && x.ExpectedEndDate < DateTime.Now);

            foreach (var loto in lotos)
            {
                loto.Status = LotoStatus.Overdue;
                loto.UpdatedDate = DateTime.Now;

                await _repository.UpdateAsync(loto);

                await _notificationService.CreateAsync(
                    loto.UserId,
                    loto.Id,
                    NotificationType.LotoOverdue);
            }
        }

        // loto kapatma
        public async Task<LotoResponse> CloseAsync(int lotoId)
        {
            var response = new LotoResponse
            {
                Entity = new Loto(),
                Error = new DTO.Base.Error()
            };

            try
            {
                var userIdString = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!int.TryParse(userIdString, out int userId))
                {
                    response.Error.HasException = true;
                    response.Error.Message = "User not found.";
                    return response;
                }

                var hasRight = await _authorizationService.HasRightAsync(userId, "CloseOwnLoto");

                if (!hasRight)
                {
                    response.Error.HasException = true;
                    response.Error.Message ="You do not have permission to close a LOTO.";
                    return response;
                }

                var loto = _repository.Get(x => x.Id == lotoId);

                if (loto == null)
                {
                    response.Error.HasException = true;
                    response.Error.Message = "LOTO not found.";
                    return response;
                }

                if (loto.UserId != userId)
                {
                    response.Error.HasException = true;
                    response.Error.Message ="You can only close your own LOTO.";
                    return response;
                }

                if (loto.Status == LotoStatus.Closed)
                {
                    response.Error.HasException = true;
                    response.Error.Message ="LOTO is already closed.";
                    return response;
                }

                loto.Status = LotoStatus.Closed;
                loto.ClosedDate = DateTime.Now;
                loto.UpdatedDate = DateTime.Now;
                loto.UpdatedBy = userId;

                response.Entity =
                    await _repository.UpdateAsync(loto);

                await _notificationService.CreateAsync(
                    userId,
                    loto.Id,
                    NotificationType.LotoClosed);
            }
            catch (Exception ex)
            {
                response.Error.HasException = true;
                response.Error.Message = ex.Message;
            }

            return response;
        }

        // admin harddelete
        public void Delete(LotoRequest request)
        {
            var userIdString = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdString, out int userId))
                return;

            var hasRight = _authorizationService.HasRightAsync(userId, "DeleteLoto").GetAwaiter().GetResult();

            if (!hasRight)
                return;

            _repository.Hardelete(request.Entity)
                .GetAwaiter()
                .GetResult();
        }

        // admin hard delete yapar
        public async Task DeleteAsync(LotoRequest request)
        {
            var userIdString = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdString, out int userId))
                return;

            var hasRight = await _authorizationService.HasRightAsync(userId, "DeleteLoto");

            if (!hasRight)
                return;

            await _repository.Hardelete(request.Entity);
        }

        public async Task<LotoFilterResponse> GetFilteredAsync(List<LotoStatus> statuses,List<int> userIds,List<LotoType> lotoTypes,
               DateTime? startDateFrom,DateTime? startDateTo,string searchValue,string sortColumn,string sortDirection,int page,int pageSize)
        {
            await UpdateOverdueLotosAsync();
            return await _repository.GetFilteredAsync(statuses,userIds,lotoTypes,startDateFrom,startDateTo,searchValue,sortColumn,sortDirection,page,pageSize);
        }
    }
}