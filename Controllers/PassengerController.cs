using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CodingChallenge.Controllers
{
    public class PassengerController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public PassengerController(HttpClient httpClient, IConfiguration configuration) 
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("/candidate")]
        public IActionResult GetCandidate()
        {
            var candidate = new
            {
                name = "test",
                phone = "test"
            };

            return Ok(candidate);
        }

        [HttpGet]
        [Route("/Location")]
        public async Task<ActionResult> GetLocationAsync([FromQuery] string ipAddress)
        {
            if (String.IsNullOrEmpty(ipAddress))
            {
                return BadRequest("No IP Address provided");
            }

            string accessKey = _configuration["IpStackAccessKey"];

            HttpResponseMessage response =  await _httpClient.GetAsync(new Uri($@"http://api.ipstack.com/{ipAddress}?access_key={accessKey}&fields=city"));

            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await (response.Content.ReadAsStringAsync());
                return Ok(jsonResponse);
            }
            else
            {
                return StatusCode((int)response.StatusCode);
            }
        }

        [HttpGet]
        [Route("/Listings")]
        public async Task<ActionResult> GetListingsAsync([FromQuery] string numberOfPassengers)
        {
            if (String.IsNullOrEmpty(numberOfPassengers) || !int.TryParse(numberOfPassengers, out _))
            {
                return BadRequest("Invalid number of passengers input");
            }


            HttpResponseMessage response = await _httpClient.GetAsync(new Uri($@"https://jayridechallengeapi.azurewebsites.net/api/QuoteRequest"));

            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                JObject data = JObject.Parse(jsonResponse);

                var filteredListings = data["listings"]
                    .Where(l => l["vehicleType"]["maxPassengers"].Value<int>() >= int.Parse(numberOfPassengers))
                    .Select(l => new
                    {
                        Name = (string)l["name"],
                        PricePerPassenger = (double)l["pricePerPassenger"]
                    })
                    .ToList();

                var totalPriceListings = filteredListings.Select(l => new
                {
                    Name = l.Name,
                    TotalPrice = l.PricePerPassenger * int.Parse(numberOfPassengers)
                });

                var sortedListings = totalPriceListings.OrderBy(l => l.TotalPrice);

                return Ok(sortedListings);
            }
            else
            {
                return StatusCode((int)response.StatusCode);    
            }
        }
    }
}
