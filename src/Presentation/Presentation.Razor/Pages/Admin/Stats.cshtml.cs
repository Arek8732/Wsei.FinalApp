using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace Presentation.Razor.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class StatsModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _config;

        public StatsModel(IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            _httpClientFactory = httpClientFactory;
            _config = config;
        }

        [BindProperty]
        public string? SoapResult { get; set; }

        public void OnGet()
        {
        }

        // POST /Admin/Stats?handler=CallSoap
        public async Task<IActionResult> OnPostCallSoapAsync()
        {
            var endpoint = _config["Soap:Endpoint"] ?? "http://localhost:5095/UserService.asmx";

            var envelope = @"<?xml version=""1.0"" encoding=""utf-8""?>
<soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
               xmlns:xsd=""http://www.w3.org/2001/XMLSchema""
               xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
  <soap:Body>
    <RegisterUser xmlns=""http://tempuri.org/"">
      <user xmlns:q1=""http://schemas.datacontract.org/2004/07/SoapService.DataContract""
            xmlns:i=""http://www.w3.org/2001/XMLSchema-instance"">
        <q1:Age>25</q1:Age>
        <q1:EmailAddress>admin@wsei.edu.pl</q1:EmailAddress>
        <q1:FirstName>Admin</q1:FirstName>
        <q1:LastName>Test</q1:LastName>
        <q1:MarketingConsent>true</q1:MarketingConsent>
      </user>
    </RegisterUser>
  </soap:Body>
</soap:Envelope>";

            var client = _httpClientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Post, endpoint);
            request.Content = new StringContent(envelope, Encoding.UTF8, "text/xml");
            request.Headers.Add("SOAPAction", "\"http://tempuri.org/IUserService/RegisterUser\"");

            var response = await client.SendAsync(request);
            SoapResult = await response.Content.ReadAsStringAsync();

            return Page();
        }
    }
}
