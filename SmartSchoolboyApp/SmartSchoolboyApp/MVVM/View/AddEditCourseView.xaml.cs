using SmartSchoolboyApp.Classes;
using SmartSchoolboyApp.MVVM.ViewModel;
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
using System.Windows.Shapes;

namespace SmartSchoolboyApp.MVVM.View
{
    /// <summary>
    /// Логика взаимодействия для AddEditCourseView.xaml
    /// </summary>
    public partial class AddEditCourseView : Window
    {
        public AddEditCourseView(Course course)
        {
            InitializeComponent();
            DataContext = new AddEditCourseViewModel(course);
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
        private void btnMinimaze_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
