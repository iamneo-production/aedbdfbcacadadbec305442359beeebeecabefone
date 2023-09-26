using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using BookStoreApp.Models;

namespace BookStoreApp.Services
{
    public interface IEnquiryService
    {
        bool AddEnquiry(Enquiry enquiry);
        List<Enquiry> GetAllEnquirys();
        bool DeleteEnquiry(int id);
    }
    public class EnquiryService : IEnquiryService
    {
        private readonly HttpClient _httpClient;
        public EnquiryService(HttpClient httpClient, IConfiguration configuration)
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            _httpClient = new HttpClient(clientHandler);
            var apiSettings = configuration.GetSection("ApiSettings").Get<ApiSettings>();
            _httpClient.BaseAddress = new Uri(apiSettings.BaseUrl);
        }

        public bool AddEnquiry(Enquiry enquiry)
        {
            try
            {
                var json = JsonConvert.SerializeObject(enquiry);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + $"/Enquiry", content).Result;

                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException)
            {
                return false;
            }
        }

        public List<Enquiry> GetAllEnquirys()
        {
            try
            {
                HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/Enquiry").Result;

                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<List<Enquiry>>(data);
                }

                return new List<Enquiry>();
            }
            catch (HttpRequestException)
            {
                return new List<Enquiry>();
            }
        }

        public bool DeleteEnquiry(int id)
        {
            try
            {
                HttpResponseMessage response = _httpClient.DeleteAsync(_httpClient.BaseAddress + $"/Enquiry/{id}").Result;

                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException)
            {
                return false;
            }
        }
    }
}
