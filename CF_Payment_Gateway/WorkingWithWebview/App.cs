using WorkingWithWebview.MyPages;
using Xamarin.Forms;

namespace WorkingWithWebview
{
    public class App : Application
    {
        public App()
        {
            //var tabs = new TabbedPage();
            //tabs.Children.Add(new Cashfree { Title = "Cashfree" });

            //MainPage = new Cashfree { Title = "Cashfree" };

            MainPage = new NavigationPage(new CheckoutPage());
        }
    }
}

