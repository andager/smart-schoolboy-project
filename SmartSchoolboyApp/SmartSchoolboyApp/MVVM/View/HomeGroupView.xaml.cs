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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SmartSchoolboyApp.MVVM.View
{
    /// <summary>
    /// Логика взаимодействия для HomeGroupView.xaml
    /// </summary>
    public partial class HomeGroupView : UserControl
    {
        public HomeGroupView(Group group)
        {
            InitializeComponent();
            DataContext = new HomeGroupViewModel(group);
        }
    }
}
