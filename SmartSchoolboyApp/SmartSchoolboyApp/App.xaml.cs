﻿using SmartSchoolboyApp.Classes;
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
using SmartSchoolboyApp.Stores;
using System.Security.Cryptography.X509Certificates;

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
        private readonly NavigationStore _navigationStore;
        static App()
        {
            //Инициализация коннектора сконфигурированным HttpClient
            ApiConnector = new ApiConnection(new HttpClient() { BaseAddress = BaseUri });
        }
        public App()
        {
            _navigationStore = new NavigationStore();
        }
        /// <summary>
        /// Метод загрузки окон приложения
        /// </summary>
        protected void Application_Startup(object sender, StartupEventArgs e)
        {
            LoginView loginView = new LoginView();
            loginView.Show();

            //_navigationStore.CurrentViewModel = new CourseViewModel();
            //HomeView homeView = new HomeView()
            //{
            //    DataContext = new HomeViewModel(_navigationStore)
            //};
            //homeView.Show();
            


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
