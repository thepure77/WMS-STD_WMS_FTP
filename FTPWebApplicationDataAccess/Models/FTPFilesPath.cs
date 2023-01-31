using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPWebApplicationDataAccess.Models
{
    public class FTPFilesPath : EntityBaseModel
    {
        public string FileName { get; set; }
        public string RelativePath { get; set; }
    }
}
