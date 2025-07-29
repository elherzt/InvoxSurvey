using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Enums
{
    public enum TypeOfResponse
    {
        OK = 0, 
        FailedResponse = 1, // error was handled, logic, flow or business validation
        Exception = 2, //system error, unhandled 
        TimeOut = 3,
        NotFound = 4,
    }
}
