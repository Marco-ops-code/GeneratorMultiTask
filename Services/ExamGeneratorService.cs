using GeneratorMultiTask.Models;

namespace GeneratorMultiTask.Services;

public static class ExamGeneratorService
{
	public static GeneratedExam Generate(ExamConfig config)
	{
		if (config.Subjects.Count == 0)
			throw new ArgumentException("Sélectionnez au moins une matière.");

		var seed = ResolveSeed(config);
		var rng = new Random(seed);

		var perSubject = Distribute(config.QuestionCount, config.Subjects.Count);
		var all = new List<ExamQuestion>();

		for (var i = 0; i < config.Subjects.Count; i++)
		{
			var subject = config.Subjects[i];
			var take = perSubject[i];
			var pool = QuestionCatalog.Query(subject, config.Level, config.Difficulty)
				.Select(ToExam)
				.ToList();

			all.AddRange(PickWithReplacement(rng, pool, take, subject));
		}

		Shuffle(all, rng);

		return new GeneratedExam
		{
			Seed = seed,
			Config = config,
			Questions = all
		};
	}

	private static int[] Distribute(int total, int buckets)
	{
		var n = Math.Max(1, buckets);
		var baseCount = total / n;
		var rem = total % n;
		var arr = new int[n];
		for (var i = 0; i < n; i++)
			arr[i] = baseCount + (i < rem ? 1 : 0);
		return arr;
	}

	private static int ResolveSeed(ExamConfig config)
	{
		if (config.Mode == GenerationMode.Aleatoire)
			return Random.Shared.Next();

		var code = string.IsNullOrWhiteSpace(config.SessionCode)
			? "DEFAULT-SESSION"
			: config.SessionCode.Trim();

		unchecked
		{
			var hash = 17;
			foreach (var c in code.ToUpperInvariant())
				hash = hash * 31 + c;

			hash = hash * 31 + (int)config.Level;
			hash = hash * 31 + (int)config.Difficulty;
			hash = hash * 31 + config.QuestionCount;

			foreach (var s in config.Subjects.OrderBy(x => x))
				hash = hash * 31 + (int)s;

			return hash;
		}
	}

	private static ExamQuestion ToExam(QuestionTemplate t) => new()
	{
		Id = t.Id,
		Subject = t.Subject,
		SubjectLabel = SubjectLabels.Fr(t.Subject),
		Difficulty = t.Difficulty,
		Prompt = t.Prompt,
		Options = t.Options,
		CorrectIndex = t.CorrectIndex
	};

	private static List<ExamQuestion> PickWithReplacement(Random rng, List<ExamQuestion> pool, int count, SubjectId subject)
	{
		var result = new List<ExamQuestion>();
		if (count <= 0)
			return result;

		if (pool.Count == 0)
		{
			// Aucun modèle : question de secours (ne devrait pas arriver si le catalogue couvre la matière)
			for (var i = 0; i < count; i++)
			{
				result.Add(new ExamQuestion
				{
					Id = $"fallback-{subject}-{i}",
					Subject = subject,
					SubjectLabel = SubjectLabels.Fr(subject),
					Difficulty = Difficulty.Facile,
					Prompt = $"[Banque en cours d'enrichissement] Placeholder — {SubjectLabels.Fr(subject)}",
					Options = new[] { "Réponse A", "Réponse B", "Réponse C", "Réponse D" },
					CorrectIndex = 0
				});
			}
			return result;
		}

		var work = pool.ToList();
		Shuffle(work, rng);

		for (var i = 0; i < count; i++)
		{
			var src = work[i % work.Count];
			result.Add(src.CloneWithNewId($"{src.Id}#t{i}"));
		}

		return result;
	}

	private static void Shuffle<T>(IList<T> list, Random rng)
	{
		for (var i = list.Count - 1; i > 0; i--)
		{
			var j = rng.Next(i + 1);
			(list[i], list[j]) = (list[j], list[i]);
		}
	}
}
