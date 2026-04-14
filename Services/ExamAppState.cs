using GeneratorMultiTask.Models;

namespace GeneratorMultiTask.Services;

/// <summary>État minimal partagé entre les pages Shell (sans conteneur DI complexe).</summary>
public static class ExamAppState
{
	public static GeneratedExam? CurrentExam { get; set; }
	public static ExamAttempt? LastAttempt { get; set; }
}
