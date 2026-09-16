using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOTO.Application.DTO.Base
{
    public class Error
    {
        public bool HasException { get; set; } = false;

        public string Message { get; set; }
    }
}
