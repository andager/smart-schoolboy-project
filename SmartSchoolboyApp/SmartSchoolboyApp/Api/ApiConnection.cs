using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

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

        /// <summary>
        /// GET: api/T
        /// <para/>
        /// Асинхронный метод Http GET
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url">Параметр ссылки на контроллер API</param>
        /// <returns></returns>
        /// <exception cref="HttpRequestException"></exception>
        public async Task<T> GetTAsync<T>(string url)
        {
                var response = await _httpClient.GetAsync(_httpClient.BaseAddress + url);
                if (response.IsSuccessStatusCode)
                    return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
                else throw new HttpRequestException($"Запрос завершился с кодом: {response.StatusCode}");
        }

        /// <summary>
        /// GET: api/T/search/5
        /// <para/>
        /// Асинхронный метод Http GET/search
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url">Параметр ссылки на контроллер API</param>
        /// <param name="search">Параметр для поиска и фильтрации данных</param>
        /// <returns></returns>
        /// <exception cref="HttpRequestException"></exception>
        public async Task<T> SearchAsync<T>(string url, string search)
        {
            var response = await _httpClient.GetAsync(_httpClient.BaseAddress + url + "/search/" + search);
            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
            else throw new HttpRequestException($"Запрос завершился с кодом: {response.StatusCode}");
        }

        /// <summary>
        /// POST: api/T
        /// <para/>
        /// Асинхронный метод Http POST
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">Параметр добавляемого обьекта модели</param>
        /// <param name="url">Параметр ссылки на контроллер API</param>
        /// <returns></returns>
        /// <exception cref="HttpRequestException"></exception>
        public async Task<T> PostTAsync<T>(T obj, string url)
        {
            var response = await _httpClient.PostAsync(_httpClient.BaseAddress + url,
                new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json"));
            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
            else throw new HttpRequestException($"Запрос завершился с кодом: {response.StatusCode}");
        }

        /// <summary>
        /// PUT: api/T/5
        /// <para/>
        /// Асинхронный метод Http PUT
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">Параметр изменяемого обьекта модели</param>
        /// <param name="url">Параметр ссылки на контроллер API</param>
        /// <param name="objId">Параметр индификатора обьекта</param>
        /// <returns></returns>
        /// <exception cref="HttpRequestException"></exception>
        public async Task<T> PutTAsync<T>(T obj, string url, int objId)
        {
            var response = await _httpClient.PutAsync(_httpClient.BaseAddress + url + $"/{objId}",
                new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json"));
            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
            else throw new HttpRequestException($"Запрос завершился с кодом: {response.StatusCode}");
        }

        /// <summary>
        /// DELETE: api/T/5
        /// <para/>
        /// Асинхронный метод Http DELETE
        /// </summary>
        /// <param name="url">Параметр ссылки на контроллер API</param>
        /// <param name="objId">Параметр индификатора обьекта</param>
        /// <returns></returns>
        public async Task<HttpStatusCode> DeleteAsync(string url, int objId) => (await _httpClient.DeleteAsync(_httpClient.BaseAddress + url + $"/{objId}")).StatusCode;
    }
}
