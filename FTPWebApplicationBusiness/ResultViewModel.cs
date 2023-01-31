using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPWebApplicationBusiness
{
    [Serializable]
    public class ResultViewModel
    {
        public ResultViewModel()
        {
            RECORD = new List<FTPResultRecordViewModel>();
        }
        public string MsgName { get; set; }
        public string ResultStatus { get; set; }
        public string ResultDesc { get; set; }
        public string ResultDateTime { get; set; }
        public List<FTPResultRecordViewModel> RECORD { get; set; }
    }

    [Serializable]
    public class FTPResultRecordViewModel
    {
        public string FileName { get; set; }
        public string DownloadUrl { get; set; }
    }
}
