using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace FTPWebApplication.Controllers
{
    public class FTPFileRequestController : ApiController
    {
        [HttpGet]
        public IHttpActionResult RequestFiles()
        {
            try
            {
                var service = new FTPLibrary();
                var result = service.Download();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
