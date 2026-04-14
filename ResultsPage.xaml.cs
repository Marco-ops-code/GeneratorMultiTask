using GeneratorMultiTask.Models;
using GeneratorMultiTask.Services;

namespace GeneratorMultiTask;

public partial class ResultsPage : ContentPage
{
	public ResultsPage()
	{
		InitializeComponent();
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		var attempt = ExamAppState.LastAttempt;
		if (attempt == null)
		{
			MainThread.BeginInvokeOnMainThread(async () =>
			{
				await Shell.Current.GoToAsync("//MainPage");
			});
			return;
		}

		var total = attempt.Results.Count;
		var ok = attempt.CorrectCount;
		ScoreTitle.Text = $"{ok} / {total} bonnes réponses";
		ScoreDetail.Text =
			$"Graine {attempt.Exam.Seed} · {FormatMode(attempt.Exam.Config.Mode)} · {FormatLevel(attempt.Exam.Config.Level)} · {FormatDiff(attempt.Exam.Config.Difficulty)}";

		DetailStack.Children.Clear();
		foreach (var r in attempt.Results)
		{
			var border = new Border
			{
				StrokeThickness = 1,
				Padding = new Thickness(14),
				StrokeShape = new RoundRectangle { CornerRadius = 12 },
				Content = new VerticalStackLayout
				{
					Spacing = 6,
					Children =
					{
						new Label
						{
							Text = $"{r.Question.SubjectLabel} · {(r.IsCorrect ? "Correct" : "À revoir")}",
							FontFamily = "OpenSansSemibold",
							TextColor = r.IsCorrect ? Color.FromArgb("#1e7a4f") : Color.FromArgb("#c0392b")
						},
						new Label
						{
							Text = r.Question.Prompt,
							FontSize = 13,
							LineBreakMode = LineBreakMode.WordWrap
						}
					}
				}
			};

			var isDark = Application.Current?.RequestedTheme == AppTheme.Dark;
			border.BackgroundColor = isDark ? Color.FromArgb("#1a2332") : Color.FromArgb("#f4f6f9");
			border.Stroke = isDark ? Color.FromArgb("#2d3d52") : Color.FromArgb("#d7dde8");

			if (!r.IsCorrect && r.SelectedIndex is int si && si >= 0 && si < r.Question.Options.Count)
			{
				((VerticalStackLayout)border.Content!).Children.Add(new Label
				{
					Text = $"Votre réponse : {r.Question.Options[si]}",
					FontSize = 12,
					TextColor = Color.FromArgb("#c0392b")
				});
				((VerticalStackLayout)border.Content!).Children.Add(new Label
				{
					Text = $"Réponse attendue : {r.Question.Options[r.Question.CorrectIndex]}",
					FontSize = 12,
					TextColor = Color.FromArgb("#1e7a4f")
				});
			}

			DetailStack.Children.Add(border);
		}
	}

	static string FormatMode(GenerationMode mode) => mode switch
	{
		GenerationMode.Aleatoire => "Mode aléatoire",
		GenerationMode.SessionUniforme => "Mode même épreuve",
		_ => mode.ToString()
	};

	static string FormatLevel(SchoolLevelBand l) => l switch
	{
		SchoolLevelBand.Primaire => "Primaire",
		SchoolLevelBand.College => "Collège",
		SchoolLevelBand.Lycee => "Lycée",
		SchoolLevelBand.Superieur => "Supérieur",
		_ => l.ToString()
	};

	static string FormatDiff(Difficulty d) => d switch
	{
		Difficulty.Facile => "Difficulté : facile",
		Difficulty.Modere => "Difficulté : modérée",
		Difficulty.Difficile => "Difficulté : difficile",
		_ => d.ToString()
	};

	async void OnHomeClicked(object? sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("//MainPage");
	}
}
