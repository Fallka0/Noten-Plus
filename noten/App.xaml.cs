namespace noten
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

             UserAppTheme = AppTheme.Light;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            // Start the app with the LoginPage; after a successful login the app sets the MainPage to the AppShell.
            return new Window(new Pages.LoginPage());
        }
    }
}