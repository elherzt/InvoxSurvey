using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Common
{
    public class BasicSessionModel
    {
        public string Provider { get; set; } //custom property to identify provider, not implemented in this version, but can be used for future enhancements
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
