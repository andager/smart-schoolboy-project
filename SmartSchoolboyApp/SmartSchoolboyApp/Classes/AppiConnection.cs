using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SmartSchoolboyApp.Classes
{
    public static class AppiConnection
    {
        private static string BaseUrl = "http://172.20.10.9:5063/api/";

        #region 

        public async static Task<object> AuthAsyns(UserAutch user)
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.PostAsync(BaseUrl + "TeacherAutches", new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json"));
                return response;
            }
        }

        #endregion
        public async static Task<List<Teacher>> GetSchoolSubjectsAsync()
        {
            using (HttpClient httpClient = new HttpClient())
            {
                var response = await httpClient.GetStringAsync(BaseUrl + "SchoolSubjects");
                return JsonConvert.DeserializeObject<List<Teacher>>(response);
            }
        }

    }
}
