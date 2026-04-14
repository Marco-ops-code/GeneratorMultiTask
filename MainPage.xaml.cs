using GeneratorMultiTask.Models;
using GeneratorMultiTask.Services;

namespace GeneratorMultiTask;

public partial class MainPage : ContentPage
{
	readonly HashSet<SubjectId> _selectedSubjects = new();
	readonly Dictionary<SubjectId, Border> _chipBorders = new();

	public MainPage()
	{
		InitializeComponent();
		LevelPicker.ItemsSource = new[]
		{
			"Primaire",
			"Collège",
			"Lycée",
			"Supérieur"
		};
		LevelPicker.SelectedIndex = 1;

		DifficultyPicker.ItemsSource = new[]
		{
			"Facile",
			"Modéré",
			"Difficile"
		};
		DifficultyPicker.SelectedIndex = 1;

		CountPicker.ItemsSource = new[] { "5", "10", "15", "20", "30" };
		CountPicker.SelectedIndex = 1;

		ModePicker.ItemsSource = new[]
		{
			"Questions variées (aléatoire)",
			"Même épreuve pour tous (graine fixe)"
		};
		ModePicker.SelectedIndex = 0;
		ModePicker.SelectedIndexChanged += (_, _) => UpdateSessionPanel();

		BuildSubjectChips();
		UpdateSessionPanel();
	}

	void BuildSubjectChips()
	{
		SubjectChips.Children.Clear();
		_chipBorders.Clear();

		foreach (var id in SubjectLabels.All)
		{
			var border = new Border
			{
				StrokeThickness = 1,
				Padding = new Thickness(12, 8),
				Margin = new Thickness(0, 0, 8, 8),
				StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(20) },
				Content = new Label
				{
					Text = SubjectLabels.Fr(id),
					FontSize = 13,
					VerticalTextAlignment = TextAlignment.Center
				}
			};

			ApplyChipStyle(border, selected: false);
			var tap = new TapGestureRecognizer();
			var sid = id;
			tap.Tapped += (_, _) => ToggleSubject(sid, border);
			border.GestureRecognizers.Add(tap);

			_chipBorders[id] = border;
			SubjectChips.Children.Add(border);
		}
	}

	void ToggleSubject(SubjectId id, Border border)
	{
		if (_selectedSubjects.Contains(id))
			_selectedSubjects.Remove(id);
		else
			_selectedSubjects.Add(id);

		ApplyChipStyle(border, _selectedSubjects.Contains(id));
	}

	void ApplyChipStyle(Border border, bool selected)
	{
		var isDark = Application.Current?.RequestedTheme == AppTheme.Dark;
		if (selected)
		{
			border.BackgroundColor = Color.FromArgb("#1e3a5f");
			border.Stroke = Color.FromArgb("#c9a227");
			if (border.Content is Label l)
			{
				l.TextColor = Colors.White;
				l.FontFamily = "OpenSansSemibold";
			}
		}
		else
		{
			border.BackgroundColor = isDark ? Color.FromArgb("#1a2332") : Color.FromArgb("#f4f6f9");
			border.Stroke = isDark ? Color.FromArgb("#2d3d52") : Color.FromArgb("#d7dde8");
			if (border.Content is Label l)
			{
				l.TextColor = isDark ? Color.FromArgb("#e8eef7") : Color.FromArgb("#1a1f2b");
				l.FontFamily = "OpenSansRegular";
			}
		}
	}

	void UpdateSessionPanel()
	{
		var uniform = ModePicker.SelectedIndex == 1;
		SessionCodePanel.IsVisible = uniform;
	}

	SchoolLevelBand MapLevel(int index) => index switch
	{
		0 => SchoolLevelBand.Primaire,
		1 => SchoolLevelBand.College,
		2 => SchoolLevelBand.Lycee,
		_ => SchoolLevelBand.Superieur
	};

	Difficulty MapDifficulty(int index) => index switch
	{
		0 => Difficulty.Facile,
		1 => Difficulty.Modere,
		_ => Difficulty.Difficile
	};

	async void OnStartClicked(object? sender, EventArgs e)
	{
		ErrorLabel.IsVisible = false;

		if (_selectedSubjects.Count == 0)
		{
			ShowError("Choisissez au moins une matière.");
			return;
		}

		if (CountPicker.SelectedItem is not string countStr || !int.TryParse(countStr, out var count) || count < 1)
		{
			ShowError("Nombre de questions invalide.");
			return;
		}

		var mode = ModePicker.SelectedIndex == 0
			? GenerationMode.Aleatoire
			: GenerationMode.SessionUniforme;

		if (mode == GenerationMode.SessionUniforme &&
		    string.IsNullOrWhiteSpace(SessionCodeEntry.Text))
		{
			ShowError("Saisissez un code séance pour le mode « même épreuve ».");
			return;
		}

		try
		{
			var config = new ExamConfig
			{
				Subjects = _selectedSubjects.OrderBy(x => x).ToList(),
				Level = MapLevel(LevelPicker.SelectedIndex < 0 ? 1 : LevelPicker.SelectedIndex),
				Difficulty = MapDifficulty(DifficultyPicker.SelectedIndex < 0 ? 1 : DifficultyPicker.SelectedIndex),
				QuestionCount = count,
				Mode = mode,
				SessionCode = mode == GenerationMode.SessionUniforme ? SessionCodeEntry.Text : null
			};

			var exam = ExamGeneratorService.Generate(config);
			ExamAppState.CurrentExam = exam;
			ExamAppState.LastAttempt = null;

			await Shell.Current.GoToAsync(nameof(ExamPage));
		}
		catch (Exception ex)
		{
			ShowError(ex.Message);
		}
	}

	void ShowError(string message)
	{
		ErrorLabel.Text = message;
		ErrorLabel.IsVisible = true;
	}
}
