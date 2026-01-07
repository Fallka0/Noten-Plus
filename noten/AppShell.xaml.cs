using noten.Pages;


namespace noten

{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
                    
                    Routing.RegisterRoute(nameof(DashboardPage), typeof(DashboardPage));
                    Routing.RegisterRoute(nameof(AddGradePage), typeof(AddGradePage));
                    Routing.RegisterRoute(nameof(AnalyticsPage), typeof(AnalyticsPage));
                    Routing.RegisterRoute(nameof(FAQPage), typeof(FAQPage));
                    Routing.RegisterRoute(nameof(SummaryPage), typeof(SummaryPage));
        }
    }
}
