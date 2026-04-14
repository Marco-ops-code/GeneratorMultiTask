namespace GeneratorMultiTask.Models;

/// <summary>Niveau scolaire regroupé (couverture large du primaire au supérieur).</summary>
public enum SchoolLevelBand
{
	Primaire,
	College,
	Lycee,
	Superieur
}

public enum Difficulty
{
	Facile,
	Modere,
	Difficile
}

/// <summary>Mode de tirage : épreuve différente à chaque fois, ou reproductible (même jeu pour toute la classe).</summary>
public enum GenerationMode
{
	/// <summary>Graine non fixée — questions et ordre changent à chaque session.</summary>
	Aleatoire,
	/// <summary>Même graine — mêmes questions et même ordre si les paramètres sont identiques.</summary>
	SessionUniforme
}

public enum SubjectId
{
	Francais,
	Mathematiques,
	HistoireGeo,
	Sciences,
	Langues,
	Philosophie,
	PhysiqueChimie,
	Svt,
	SesEconomie,
	Arts,
	Eps,
	Informatique,
	Lettres,
	Methodologie
}

public static class SubjectLabels
{
	public static string Fr(SubjectId s) => s switch
	{
		SubjectId.Francais => "Français",
		SubjectId.Mathematiques => "Mathématiques",
		SubjectId.HistoireGeo => "Histoire-Géographie",
		SubjectId.Sciences => "Sciences & technologie",
		SubjectId.Langues => "Langues vivantes",
		SubjectId.Philosophie => "Philosophie",
		SubjectId.PhysiqueChimie => "Physique-Chimie",
		SubjectId.Svt => "SVT",
		SubjectId.SesEconomie => "SES / Économie",
		SubjectId.Arts => "Arts & culture",
		SubjectId.Eps => "EPS & santé",
		SubjectId.Informatique => "Numérique & informatique",
		SubjectId.Lettres => "Lettres & littérature",
		SubjectId.Methodologie => "Méthodologie & raisonnement",
		_ => s.ToString()
	};

	public static IReadOnlyList<SubjectId> All { get; } = Enum.GetValues<SubjectId>();
}

[Flags]
public enum LevelMask
{
	Primaire = 1,
	College = 2,
	Lycee = 4,
	Superieur = 8,
	All = Primaire | College | Lycee | Superieur
}

public sealed class QuestionTemplate
{
	public required string Id { get; init; }
	public required SubjectId Subject { get; init; }
	public required LevelMask Levels { get; init; }
	public required Difficulty Difficulty { get; init; }
	public required string Prompt { get; init; }
	public required IReadOnlyList<string> Options { get; init; }
	public required int CorrectIndex { get; init; }
}

public sealed class ExamQuestion
{
	public required string Id { get; init; }
	public required SubjectId Subject { get; init; }
	public required string SubjectLabel { get; init; }
	public required Difficulty Difficulty { get; init; }
	public required string Prompt { get; init; }
	public required IReadOnlyList<string> Options { get; init; }
	public required int CorrectIndex { get; init; }

	public ExamQuestion CloneWithNewId(string newId) => new()
	{
		Id = newId,
		Subject = Subject,
		SubjectLabel = SubjectLabel,
		Difficulty = Difficulty,
		Prompt = Prompt,
		Options = Options,
		CorrectIndex = CorrectIndex
	};
}

public sealed class ExamConfig
{
	public required IReadOnlyList<SubjectId> Subjects { get; init; }
	public required SchoolLevelBand Level { get; init; }
	public required Difficulty Difficulty { get; init; }
	public required int QuestionCount { get; init; }
	public required GenerationMode Mode { get; init; }
	/// <summary>Code partagé en classe (ex. salle + date) pour le mode uniforme.</summary>
	public string? SessionCode { get; init; }
}

public sealed class GeneratedExam
{
	public required int Seed { get; init; }
	public required ExamConfig Config { get; init; }
	public required IReadOnlyList<ExamQuestion> Questions { get; init; }
}

public sealed class QuestionResult
{
	public required ExamQuestion Question { get; init; }
	public int? SelectedIndex { get; init; }
	public bool IsCorrect => SelectedIndex == Question.CorrectIndex;
}

public sealed class ExamAttempt
{
	public required GeneratedExam Exam { get; init; }
	public required IReadOnlyList<QuestionResult> Results { get; init; }
	public int CorrectCount => Results.Count(r => r.IsCorrect);
}
