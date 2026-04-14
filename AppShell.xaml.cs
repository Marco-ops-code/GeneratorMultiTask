namespace GeneratorMultiTask;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		Routing.RegisterRoute(nameof(ExamPage), typeof(ExamPage));
		Routing.RegisterRoute(nameof(ResultsPage), typeof(ResultsPage));
	}
}
