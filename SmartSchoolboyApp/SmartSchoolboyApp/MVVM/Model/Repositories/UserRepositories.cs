using SmartSchoolboyApp.Classes;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;

namespace SmartSchoolboyApp.MVVM.Model.Repositories
{
    public class UserRepositories : Teacher
    {
        public async Task<bool> Authenticateuser(NetworkCredential credential)
        {
            try
            {
                Teacher teacher = new Teacher()
                {
                    numberPhone = credential.UserName,
                    password = credential.Password
                };
                App.currentUser = await App.ApiConnector.PostTAsync(teacher, "TeacherAutches");
                return true;
            }
            catch (HttpRequestException)
            {
                return false;
            }

        }
    }
}
