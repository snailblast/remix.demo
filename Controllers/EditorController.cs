using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace remix.demo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EditorController : ControllerBase
    {
        private readonly ILogger<EditorController> _logger;
        private readonly AccessToken _accessToken;
        private readonly AppConfig _appConfig;

        public EditorController(ILogger<EditorController> logger, AccessToken accessToken, AppConfig appConfig)
        {
            _logger = logger;
            _accessToken = accessToken;
            _appConfig = appConfig;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string userId, [FromQuery] string designId = "", [FromQuery] string templateId = "")
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", await _accessToken.GetAsync());
                var param = "";
                if (!string.IsNullOrEmpty(designId) && designId != "none")
                {
                    param += $"&designId={designId}";
                }
                if (!string.IsNullOrEmpty(templateId) && templateId != "none")
                {
                    param += $"&templateId={templateId}";
                }
                var response = await client.GetAsync($"{_appConfig.RemixApiDomain}/editor?userId={userId}{param}");
                if (response.IsSuccessStatusCode)
                {
                    var result = JsonConvert.DeserializeObject<EditorResponse>(await response.Content.ReadAsStringAsync());
                    return Ok(result);
                }
            }
            return BadRequest("Something bad happened");
        }

        public class EditorResponse
        {
            public string url { get; set; }
        }
    }
}