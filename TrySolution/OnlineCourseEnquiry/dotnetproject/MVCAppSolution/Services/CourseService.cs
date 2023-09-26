using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using BookStoreApp.Models;

namespace BookStoreApp.Services
{
    public interface ICourseService
    {
        bool AddCourse(Course course);
        List<Course> GetAllCourses();
        bool DeleteCourse(int id);
        Task<IEnumerable<string>> GetCourseTitles();
    }
    public class CourseService : ICourseService
    {
        private readonly HttpClient _httpClient;
        public CourseService(HttpClient httpClient, IConfiguration configuration)
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            _httpClient = new HttpClient(clientHandler);
            var apiSettings = configuration.GetSection("ApiSettings").Get<ApiSettings>();
            _httpClient.BaseAddress = new Uri(apiSettings.BaseUrl);
        }

        public bool AddCourse(Course course)
        {
            try
            {
                var json = JsonConvert.SerializeObject(course);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + $"/Course", content).Result;

                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException)
            {
                return false;
            }
        }
       public async Task<IEnumerable<string>> GetCourseTitles()
    {
        try
        {
            HttpResponseMessage response = await _httpClient.GetAsync(_httpClient.BaseAddress + "/Course/CourseName");

            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<string>>(data);
            }

            return new List<string>();
        }
        catch (HttpRequestException)
        {
            return new List<string>();
        }
    }
        public List<Course> GetAllCourses()
        {
            try
            {
                HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/Course").Result;

                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<List<Course>>(data);
                }

                return new List<Course>();
            }
            catch (HttpRequestException)
            {
                return new List<Course>();
            }
        }


        public bool DeleteCourse(int id)
        {
            try
            {
                HttpResponseMessage response = _httpClient.DeleteAsync(_httpClient.BaseAddress + $"/Course/{id}").Result;

                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException)
            {
                return false;
            }
        }
    }
}
