using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPWebApplicationDataAccess
{
    interface IEntityStatusBase
    {
        bool IsActive { get; set; }
        bool IsDelete { get; set; }
    }
}
