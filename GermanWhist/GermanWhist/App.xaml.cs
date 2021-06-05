using Xamarin.Forms;

namespace GermanWhist
{
    public partial class App : Application
    {
        public static int AnimationDuration { get; set; }
        public App()
        {
            InitializeComponent();
            AnimationDuration = 900;
            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
