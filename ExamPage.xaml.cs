using GeneratorMultiTask.Models;
using GeneratorMultiTask.Services;

namespace GeneratorMultiTask;

public partial class ExamPage : ContentPage
{
	GeneratedExam? _exam;
	readonly List<QuestionResult> _results = new();
	int _index;
	int? _selectedOption;

	public ExamPage()
	{
		InitializeComponent();
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		_exam = ExamAppState.CurrentExam;
		if (_exam == null || _exam.Questions.Count == 0)
		{
			MainThread.BeginInvokeOnMainThread(async () =>
			{
				await DisplayAlert("Session", "Aucune épreuve active. Retour à l'accueil.", "OK");
				await Shell.Current.GoToAsync("//MainPage");
			});
			return;
		}

		_index = 0;
		_results.Clear();
		MetaLabel.Text = $"Graine : {_exam.Seed} · {FormatMode(_exam.Config.Mode)}";
		ShowQuestion();
	}

	void ShowQuestion()
	{
		if (_exam == null)
			return;

		_selectedOption = null;
		NextButton.IsEnabled = false;

		var total = _exam.Questions.Count;
		var q = _exam.Questions[_index];

		ProgressLabel.Text = $"{_index + 1} / {total}";
		ExamProgress.Progress = total == 0 ? 0 : (double)(_index + 1) / total;

		SubjectBadge.Text = q.SubjectLabel.ToUpperInvariant();
		PromptLabel.Text = q.Prompt;

		OptionsStack.Children.Clear();
		for (var i = 0; i < q.Options.Count; i++)
		{
			var idx = i;
			var border = new Border
			{
				StrokeThickness = 1,
				Padding = new Thickness(14, 12),
				StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(12) },
				Content = new Label
				{
					Text = q.Options[i],
					FontSize = 15,
					LineBreakMode = LineBreakMode.WordWrap
				}
			};
			StyleOption(border, selected: false);

			var tap = new TapGestureRecognizer();
			tap.Tapped += (_, _) => SelectOption(idx, border);
			border.GestureRecognizers.Add(tap);

			OptionsStack.Children.Add(border);
		}

		NextButton.Text = _index == total - 1 ? "Terminer et voir les résultats" : "Valider et suivant";
	}

	void SelectOption(int optionIndex, Border border)
	{
		_selectedOption = optionIndex;

		for (var i = 0; i < OptionsStack.Children.Count; i++)
		{
			if (OptionsStack.Children[i] is Border b)
				StyleOption(b, ReferenceEquals(b, border));
		}

		NextButton.IsEnabled = true;
	}

	void StyleOption(Border border, bool selected)
	{
		var isDark = Application.Current?.RequestedTheme == AppTheme.Dark;
		if (selected)
		{
			border.BackgroundColor = Color.FromArgb("#1e3a5f");
			border.Stroke = Color.FromArgb("#c9a227");
			if (border.Content is Label l)
				l.TextColor = Colors.White;
		}
		else
		{
			border.BackgroundColor = isDark ? Color.FromArgb("#1a2332") : Color.FromArgb("#f4f6f9");
			border.Stroke = isDark ? Color.FromArgb("#2d3d52") : Color.FromArgb("#d7dde8");
			if (border.Content is Label l)
				l.TextColor = isDark ? Color.FromArgb("#e8eef7") : Color.FromArgb("#1a1f2b");
		}
	}

	static string FormatMode(GenerationMode mode) => mode switch
	{
		GenerationMode.Aleatoire => "Questions variées",
		GenerationMode.SessionUniforme => "Même épreuve (graine)",
		_ => mode.ToString()
	};

	async void OnNextClicked(object? sender, EventArgs e)
	{
		if (_exam == null || _selectedOption is not int sel)
			return;

		var q = _exam.Questions[_index];
		_results.Add(new QuestionResult
		{
			Question = q,
			SelectedIndex = sel
		});

		if (_index >= _exam.Questions.Count - 1)
		{
			ExamAppState.LastAttempt = new ExamAttempt
			{
				Exam = _exam,
				Results = _results.ToList()
			};
			await Shell.Current.GoToAsync(nameof(ResultsPage));
			return;
		}

		_index++;
		ShowQuestion();
	}
}
