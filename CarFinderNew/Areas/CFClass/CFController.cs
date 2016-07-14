using CarFinderNew.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;



namespace CarFinderNew.Areas.CFClass
{

    [RoutePrefix("api/CF")]
    public class CFController : ApiController
    {

        private ApplicationDbContext db = new ApplicationDbContext();

        [Route("all")]
        [HttpGet]
        public async Task<List<string>> Testing()
        {
            return await db.GetPersons();
        }
    }
}