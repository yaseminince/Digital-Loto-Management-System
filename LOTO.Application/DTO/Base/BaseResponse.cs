using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOTO.Application.DTO.Base
{
    public class BaseResponse<T>
    {
        public T Entity { get; set; }
        public List<T> EntityList { get; set; }

        public Error Error { get; set; }
    }
}
