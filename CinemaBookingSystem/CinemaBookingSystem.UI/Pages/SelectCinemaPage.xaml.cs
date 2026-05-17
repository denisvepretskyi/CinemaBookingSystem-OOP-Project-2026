using CinemaBookingSystem.UI.Windows;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CinemaBookingSystem.UI.Pages
{
    /// <summary>
    /// Логика взаимодействия для SelectCinemaPage.xaml
    /// </summary>
    public partial class SelectCinemaPage : Page
    {
        public SelectCinemaPage()
        {
            InitializeComponent();            
            if(Window.GetWindow(this) is MainWindow mainWindow)
                mainWindow.CinemaPlaceholder.Visibility = Visibility.Visible;
        }
    }
}
