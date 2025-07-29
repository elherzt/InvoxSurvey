using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Common
{
    public class CustomResponse
    {

        public CustomResponse()
        {
            this.TypeOfResponse = TypeOfResponse.OK;
        }

        public CustomResponse(TypeOfResponse typeOfResponse, string message = "")
        {
            this.TypeOfResponse = typeOfResponse;
            this.Message = message;
            this.Data = null;
        }

        public TypeOfResponse TypeOfResponse { get; set; }
        public string Message { get; set; } = string.Empty;
        public object? Data { get; set; }
    }
}
