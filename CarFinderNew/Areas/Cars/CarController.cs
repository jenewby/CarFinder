using CarFinderNew.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using CarFinderNew.Areas.Cars.Models;

namespace CarFinderNew.Areas.Cars
{
    [RoutePrefix("api/Cars")]
    public class CarController : ApiController
    {

        private ApplicationDbContext db = new ApplicationDbContext();

        [Route("years")]
        [HttpGet]
        public async Task<List<string>> GetYear()
        {
        return await db.GetYears();
        }

        [Route("make")]
        [HttpGet]
        public async Task<List<string>> GetAllMakes(int year)
        {
            return await db.GetMake(year);
        }

        [Route("model")]
        [HttpGet]
        public async Task<List<string>> GetModel(int year, string make)
        {
            return await db.GetModel(year, make);
        }

        [Route("trim")]
        [HttpGet]
        public async Task<List<string>> GetTrim(int year, string make, string model)
        {
            return await db.GetTrim(year, make, model);
        }

        [Route("getCars")]
        [HttpGet]
        public async Task<List<CarFinderNew.Areas.Cars.Models.Cars>> GetCars(int year, string make, string model, string trim)
        {
            return await db.GetCars(year, make, model, trim);
        }

        [Route("SingleCar")]
        public CarFinderNew.Areas.Cars.Models.Cars SingleCar(int year, string make = "", string model = "", string trim = "") {
            return db.SingleCar(year, make, model, trim).Result;
        }

        [Route("GetInfo")]
        public async Task<IHttpActionResult> getCarData(int year, string make = "", string model = "", string trim = "")

        {
            HttpResponseMessage response;
            string content = "";
            CarFinderNew.Areas.Cars.Models.Cars sCar = new Models.Cars();
            try
            {
                sCar = SingleCar(year, make, model, trim);
            }
            catch (Exception e) {
                 e.Message.ToString();
            }
            var car = new carViewModel
            {
                Car = sCar,
                Recalls = content,
                Image = ""
            };
            // Same as above//

            //var car2 = new carViewModel();
            //car2.Car = SingleCar(year, make, model, trim);
            //car2.Recalls = content;
            //car2.Image = "";

            //Get recall Data

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://www.nhtsa.gov");
                try
                {
                    response = await client.GetAsync("webapi/api/Recalls/vehicle/modelyear/" + year.ToString() + "/make/" + make + "/model/" + model + "?format=json");
                    content = await response.Content.ReadAsStringAsync();
                }
                catch (Exception e) {
                    return InternalServerError(e);
                }
            }

            car.Recalls = content;
            //  Bing Search //

            string query = year + " " + make + " " + model + " " + trim;

            string rootUri = "https://api.datamarket.azure.com/Bing/Search";

            var bingContainer = new Bing.BingSearchContainer(new Uri(rootUri));

            var accountKey = "5u/0CzVmYrTKDOjlxPepfPkh/G8llMIfVJ7QC/oNEvQ";

            bingContainer.Credentials = new NetworkCredential(accountKey, accountKey);

            var imageQuery = bingContainer.Image(query, null, null, null, null, null, null);

            var imageResults = imageQuery.Execute().ToList();

            car.Image = imageResults.First().MediaUrl;

            //////

            return Ok(car);   
        }
        
    }
}