﻿using SmartSchoolboyApp.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SmartSchoolboyApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для AutchPage.xaml
    /// </summary>
    public partial class AutchPage : Page
    {
        public AutchPage()
        {
            InitializeComponent();
        }

        private async void bTnAutch_Click(object sender, RoutedEventArgs e)
        {
            UserAutch userAutch = new UserAutch()
            {
                numberPhone = tBoxLogin.Text,
                password = pBoxPassword.Password
            };
            var user = await AppiConnection.AuthAsyns(userAutch);
            var teacher = await AppiConnection.GetSchoolSubjectsAsync();
            teacher.Where(p => p.numberPhone == userAutch.numberPhone);
        }
    }
}
