using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace SmartSchoolboyApp.Classes
{
    public class ApiConnection
    {
        //Сконфигурированный клиент для отправки Http запросов
        private readonly HttpClient _httpClient;
        public ApiConnection(HttpClient client)
        {
            _httpClient = client;
        }

        public async Task<List<Teacher>> SearchAsync()
        {
            var response = await _httpClient.GetAsync(_httpClient.BaseAddress + "Teachers" + "/баранова");
            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException($"Запрос завершился с кодом {response.StatusCode}");
            return JsonConvert.DeserializeObject<List<Teacher>>(await response.Content.ReadAsStringAsync());
        }
        public async Task<T> GetTAsync<T>(string url)
        {
            var response = await _httpClient.GetAsync(_httpClient.BaseAddress + url);
            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException($"Запрос завершился с кодом {response.StatusCode}");
            return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
        }
        public async Task<T> PostTAsync<T>(T obj, string url)
        {
            var response = await _httpClient.PostAsync(_httpClient.BaseAddress + url,
                new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json"));
            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException($"Запрос завершился с кодом {response.StatusCode}");
            return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
        }
        public async Task<T> PutTAsync<T>(T obj, string url, int objId)
        {
            var response = await _httpClient.PutAsync(_httpClient.BaseAddress + url + $"/{objId}",
                new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json"));
            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException($"Запрос завершился с кодом {response.StatusCode}");
            return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
        }
        public async Task<HttpStatusCode> DeleteAsync(string url, int objId) => (await _httpClient.DeleteAsync(_httpClient.BaseAddress + url + $"/{objId}")).StatusCode;
    }
}
