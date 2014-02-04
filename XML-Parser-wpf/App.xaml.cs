using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using Test1.View;
using Test1.ViewModel;

namespace Test1
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            MainWindow window = new MainWindow();
            MaterialViewModel materialViewModel = new MaterialViewModel();
            window.DataContext = materialViewModel;
            window.Show();
        }
    }
}
