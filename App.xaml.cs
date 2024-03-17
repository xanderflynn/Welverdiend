using Welverdiend7.Pages;

namespace Welverdiend7
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new Login();
        }
    }
}
