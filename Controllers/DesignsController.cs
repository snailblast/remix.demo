using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Reflection;
using static remix.demo.AccessToken;

namespace remix.demo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DesignsController : ControllerBase
    {
        private readonly ILogger<DesignsController> _logger;
        private readonly AccessToken _accessToken;
        private readonly AppConfig _appConfig;

        public DesignsController(ILogger<DesignsController> logger, AccessToken accessToken, AppConfig appConfig)
        {
            _logger = logger;
            _accessToken = accessToken;
            _appConfig = appConfig;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string userId)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await _accessToken.GetAsync());
                var response = await client.GetAsync($"{_appConfig.RemixApiDomain}/designs?userId={userId}");
                if (response.IsSuccessStatusCode)
                {
                    var result = JsonConvert.DeserializeObject<List<DesignResponse>>(await response.Content.ReadAsStringAsync());
                    return Ok(result);
                }
            }
            return BadRequest("Something bad happened");
        }

        public class DesignResponse
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public DateTime CreatedDate { get; set; }
            public string ThumbnailUrl { get; set; }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromQuery] string userId, string id)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await _accessToken.GetAsync());
                var response = await client.DeleteAsync($"{_appConfig.RemixApiDomain}/designs/{id}?userId={userId}");
                if (response.IsSuccessStatusCode)
                {
                    return Ok();
                }
            }
            return BadRequest("Something bad happened");
        }

        [HttpGet("{id}/output/{type}")]
        public async Task<IActionResult> GetOutput([FromQuery] string userId, string id, string type)
        {
            var error = "";
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await _accessToken.GetAsync());
                var response = await client.GetAsync($"{_appConfig.RemixApiDomain}/designs/{id}/output?userId={userId}&pdfType={type}");
                if (response.IsSuccessStatusCode)
                {
                    var result = JsonConvert.DeserializeObject<OutputResponse>(await response.Content.ReadAsStringAsync());
                    return Ok(result);
                }
                else
                {
                    error = await response.Content.ReadAsStringAsync();
                }
            }
            return BadRequest($"Something bad happened - {error}");
        }

        public class OutputResponse
        {
            public string Url { get; set; }
        }
    }
}