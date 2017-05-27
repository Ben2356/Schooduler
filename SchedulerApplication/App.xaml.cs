using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SchedulerApplication
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            //when the main window is closed, in this case the login window, it will close the entire application => need to set the shutdown mode to override this behavior
            Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            Login loginWindow = new Login();
            if(loginWindow.ShowDialog() == true)
            {
                MainWindow wnd = new MainWindow();
                Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
                Current.MainWindow = wnd;
                wnd.Show();
            }
        }
    }
}
