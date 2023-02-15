using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace remix.demo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TemplatesController : ControllerBase
    {
        private readonly ILogger<TemplatesController> _logger;
        private readonly AccessToken _accessToken;
        private readonly AppConfig _appConfig;

        public TemplatesController(ILogger<TemplatesController> logger, AccessToken accessToken, AppConfig appConfig)
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
                var json = JsonConvert.SerializeObject(new { type = "templates", page = 1 });
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"{_appConfig.RemixApiDomain}/assets?userId={userId}", new StringContent(json, Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    var result = JsonConvert.DeserializeObject<TemplateResponse>(await response.Content.ReadAsStringAsync());
                    return Ok(result);
                }
            }
            return BadRequest("Something bad happened");
        }

        public class TemplateResponse
        {
            public List<object> Folders { get; set; }
            public List<object> Assets { get; set; }
            public int Page { get; set; }
            public int NumberOfPages { get; set; }
            public string Type { get; set; }
            public Guid? CurrentFolderId { get; set; }
            public string CurrentFolderName { get; set; }
            public Guid? ParentId { get; set; }
            public string ParentName { get; set; }
        }
    }
}