using SmartSchoolboyApp.Classes;
using SmartSchoolboyApp.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using SmartSchoolboyApp.MVVM.View;

namespace SmartSchoolboyApp
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        // Авторизовавшийся пользователь
        public static Teacher currentUser;
        //Экземпляр класса коннектора для реализации Http запросов
        public static readonly ApiConnection ApiConnector;
        //Строка подключения к Api
        private static readonly Uri BaseUri = new Uri("http://172.20.10.9:5063/api/");
        static App()
        {
            //Инициализация коннектора сконфигурированным HttpClient
            ApiConnector = new ApiConnection(new HttpClient() { BaseAddress = BaseUri });
        }

        /// <summary>
        /// Метод загрузки окон приложения
        /// </summary>
        protected void Application_Startup(object sender, StartupEventArgs e)
        {
            var homeView = new HomeView();
            homeView.Show();

            //var addteacher = new AddEditTeacherView();
            //addteacher.Show();

            //окно авторизации
            //var loginView = new LoginView();
            //loginView.Show();
            //loginView.IsVisibleChanged += (s, ev) =>
            //{
            //    if (loginView.IsVisible == false && loginView.IsLoaded)
            //    {
            //        // главное окно
            //        var homeView = new HomeView();
            //        homeView.Show();
            //        loginView.Close();
            //    }
            //};
        }
    }
}
