using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FTPWebApplication.Controllers
{
    public class FileDownloadController : ApiController
    {
        [HttpGet]
        public IHttpActionResult DownloadFile()
        {
            try
            {
                return Json(new
                {
                    RES_Key = "FTPRequest",
                    RES_Status = "Success",
                    RES_Desc = ""
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
