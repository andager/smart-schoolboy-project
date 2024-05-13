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
    /// Логика взаимодействия для AddEditSchoolSubjectView.xaml
    /// </summary>
    public partial class AddEditSchoolSubjectView : Window
    {
        public AddEditSchoolSubjectView(SchoolSubject school)
        {
            InitializeComponent();
            DataContext = new AddEditSchoolSubjectViewModel(school);
        }
    }
}
