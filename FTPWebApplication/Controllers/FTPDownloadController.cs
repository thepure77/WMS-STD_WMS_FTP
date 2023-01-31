using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace FTPWebApplication.Controllers
{
    public class FTPDownloadController : ApiController
    {
        [HttpGet]
        public IHttpActionResult DownloadFiles(string billing_id)
        {
            try
            {
                var service = new FTPLibrary();
                var result = service.DownloadFTP(billing_id);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
