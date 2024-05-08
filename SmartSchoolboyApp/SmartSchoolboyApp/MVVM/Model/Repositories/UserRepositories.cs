using SmartSchoolboyApp.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

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
