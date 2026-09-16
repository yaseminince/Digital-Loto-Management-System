using LOTO.Domain.Entities;

namespace LOTO.Application.DTO.Response
{
    public class LotoFilterResponse
    {
        public List<Loto> Items { get; set; } = new();
        public int TotalCount { get; set; }
    }
}