using GeneratorMultiTask.Models;

namespace GeneratorMultiTask.Services;

/// <summary>Banque de modèles de questions par matière, niveau et difficulté (extensible).</summary>
public static class QuestionCatalog
{
	private static readonly List<QuestionTemplate> Templates = BuildTemplates();

	private static List<QuestionTemplate> BuildTemplates()
	{
		var list = new List<QuestionTemplate>();
		void Add(QuestionTemplate q) => list.Add(q);

		// ——— Français ———
		Add(new QuestionTemplate
		{
			Id = "fr-orth-1", Subject = SubjectId.Francais,
			Levels = LevelMask.Primaire | LevelMask.College,
			Difficulty = Difficulty.Facile,
			Prompt = "Quelle forme est correcte ? « Il ___ à l'école. »",
			Options = new[] { "va", "vas", "vont", "allons" },
			CorrectIndex = 0
		});
		Add(new QuestionTemplate
		{
			Id = "fr-gram-2", Subject = SubjectId.Francais,
			Levels = LevelMask.College | LevelMask.Lycee,
			Difficulty = Difficulty.Modere,
			Prompt = "Identifiez la nature de « dont » dans : « Le livre dont je parle. »",
			Options = new[] { "Pronom relatif", "Conjonction", "Adverbe", "Déterminant" },
			CorrectIndex = 0
		});
		Add(new QuestionTemplate
		{
			Id = "fr-lit-3", Subject = SubjectId.Lettres,
			Levels = LevelMask.Lycee | LevelMask.Superieur,
			Difficulty = Difficulty.Difficile,
			Prompt = "Dans le registre épique, l'amplification sert surtout à :",
			Options = new[] { "Magnifier le héros et l'enjeu", "Créer du comique", "Raccourcir le récit", "Marquer l'oralité" },
			CorrectIndex = 0
		});

		// ——— Mathématiques ———
		Add(new QuestionTemplate
		{
			Id = "math-arith-1", Subject = SubjectId.Mathematiques,
			Levels = LevelMask.Primaire | LevelMask.College,
			Difficulty = Difficulty.Facile,
			Prompt = "Résultat de 48 ÷ 6 :",
			Options = new[] { "6", "7", "8", "9" },
			CorrectIndex = 2
		});
		Add(new QuestionTemplate
		{
			Id = "math-geo-2", Subject = SubjectId.Mathematiques,
			Levels = LevelMask.College | LevelMask.Lycee,
			Difficulty = Difficulty.Modere,
			Prompt = "Théorème de Pythagore : dans un triangle rectangle, si les côtés de l'angle droit mesurent 3 et 4, l'hypoténuse mesure :",
			Options = new[] { "5", "6", "7", "12" },
			CorrectIndex = 0
		});
		Add(new QuestionTemplate
		{
			Id = "math-ana-3", Subject = SubjectId.Mathematiques,
			Levels = LevelMask.Lycee | LevelMask.Superieur,
			Difficulty = Difficulty.Difficile,
			Prompt = "Dérivée de x ↦ ln(3x+1) :",
			Options = new[] { "3/(3x+1)", "1/(3x+1)", "1/x", "3ln(3x+1)" },
			CorrectIndex = 0
		});

		// ——— Histoire-Géo ———
		Add(new QuestionTemplate
		{
			Id = "hg-temps-1", Subject = SubjectId.HistoireGeo,
			Levels = LevelMask.Primaire | LevelMask.College,
			Difficulty = Difficulty.Facile,
			Prompt = "La Révolution française commence en :",
			Options = new[] { "1787", "1789", "1792", "1804" },
			CorrectIndex = 1
		});
		Add(new QuestionTemplate
		{
			Id = "hg-geo-2", Subject = SubjectId.HistoireGeo,
			Levels = LevelMask.College | LevelMask.Lycee,
			Difficulty = Difficulty.Modere,
			Prompt = "Un état côtier méditerranéen parmi :",
			Options = new[] { "Autriche", "Serbie", "Hongrie", "Slovaquie" },
			CorrectIndex = 1
		});
		Add(new QuestionTemplate
		{
			Id = "hg-hist-3", Subject = SubjectId.HistoireGeo,
			Levels = LevelMask.Lycee | LevelMask.Superieur,
			Difficulty = Difficulty.Difficile,
			Prompt = "Traités de Westphalie (1648) : principalement",
			Options = new[] { "Fin de guerres religieuses en Europe centrale", "Unification allemande", "Révolution industrielle", "Congrès de Vienne" },
			CorrectIndex = 0
		});

		// ——— Sciences (collège) ———
		Add(new QuestionTemplate
		{
			Id = "sci-cell-1", Subject = SubjectId.Sciences,
			Levels = LevelMask.Primaire | LevelMask.College,
			Difficulty = Difficulty.Facile,
			Prompt = "L'eau bout à environ (au niveau de la mer) :",
			Options = new[] { "90 °C", "100 °C", "120 °C", "0 °C" },
			CorrectIndex = 1
		});
		Add(new QuestionTemplate
		{
			Id = "sci-energy-2", Subject = SubjectId.Sciences,
			Levels = LevelMask.College | LevelMask.Lycee,
			Difficulty = Difficulty.Modere,
			Prompt = "Photosynthèse : principaux réactifs absorbés par la feuille",
			Options = new[] { "CO₂ et eau", "O₂ et azote", "CH₄ et eau", "CO et hydrogène" },
			CorrectIndex = 0
		});

		// ——— Langues ———
		Add(new QuestionTemplate
		{
			Id = "lv-en-1", Subject = SubjectId.Langues,
			Levels = LevelMask.Primaire | LevelMask.College,
			Difficulty = Difficulty.Facile,
			Prompt = "Choisissez la traduction correcte : « Bibliothèque »",
			Options = new[] { "Library", "Bookshop", "Stationery", "Laboratory" },
			CorrectIndex = 0
		});
		Add(new QuestionTemplate
		{
			Id = "lv-en-2", Subject = SubjectId.Langues,
			Levels = LevelMask.Lycee | LevelMask.Superieur,
			Difficulty = Difficulty.Difficile,
			Prompt = "« Had I known » correspond plutôt à :",
			Options = new[] { "Si j'avais su", "Si je savais", "Quand je savais", "Après avoir su" },
			CorrectIndex = 0
		});

		// ——— Philosophie ———
		Add(new QuestionTemplate
		{
			Id = "philo-1", Subject = SubjectId.Philosophie,
			Levels = LevelMask.Lycee | LevelMask.Superieur,
			Difficulty = Difficulty.Modere,
			Prompt = "Pour Kant, l'impératif catégorique relève du domaine :",
			Options = new[] { "De la morale pratique", "De l'esthétique", "De la physique", "De l'histoire" },
			CorrectIndex = 0
		});

		// ——— Physique-Chimie ———
		Add(new QuestionTemplate
		{
			Id = "pc-1", Subject = SubjectId.PhysiqueChimie,
			Levels = LevelMask.College | LevelMask.Lycee,
			Difficulty = Difficulty.Modere,
			Prompt = "Formule de la densité :",
			Options = new[] { "ρ = m/V", "ρ = m×V", "ρ = V/m", "ρ = m+g" },
			CorrectIndex = 0
		});
		Add(new QuestionTemplate
		{
			Id = "pc-2", Subject = SubjectId.PhysiqueChimie,
			Levels = LevelMask.Lycee | LevelMask.Superieur,
			Difficulty = Difficulty.Difficile,
			Prompt = "Énergie cinétique d'un point matériel :",
			Options = new[] { "½mv²", "mv", "mgh", "½kx²" },
			CorrectIndex = 0
		});

		// ——— SVT ———
		Add(new QuestionTemplate
		{
			Id = "svt-1", Subject = SubjectId.Svt,
			Levels = LevelMask.College | LevelMask.Lycee,
			Difficulty = Difficulty.Facile,
			Prompt = "Mitochondrie : rôle principal",
			Options = new[] { "Production d'ATP (énergie)", "Synthèse des protéines", "Stockage de l'ADN", "Digestion intracellulaire" },
			CorrectIndex = 0
		});

		// ——— SES ———
		Add(new QuestionTemplate
		{
			Id = "ses-1", Subject = SubjectId.SesEconomie,
			Levels = LevelMask.Lycee | LevelMask.Superieur,
			Difficulty = Difficulty.Modere,
			Prompt = "PIB :",
			Options = new[] { "Valeur des biens et services finaux produits", "Somme des salaires uniquement", "Dette publique", "Masse monétaire" },
			CorrectIndex = 0
		});

		// ——— Arts ———
		Add(new QuestionTemplate
		{
			Id = "art-1", Subject = SubjectId.Arts,
			Levels = LevelMask.Primaire | LevelMask.College | LevelMask.Lycee,
			Difficulty = Difficulty.Facile,
			Prompt = "Couleurs primaires en synthèse soustractive (peinture) :",
			Options = new[] { "Cyan, magenta, jaune", "Rouge, vert, bleu", "Noir, blanc, gris", "Orange, violet, marron" },
			CorrectIndex = 0
		});

		// ——— EPS ———
		Add(new QuestionTemplate
		{
			Id = "eps-1", Subject = SubjectId.Eps,
			Levels = LevelMask.All,
			Difficulty = Difficulty.Facile,
			Prompt = "Échauffement : objectif principal",
			Options = new[] { "Préparer cardio-musculairement le corps", "Maximiser la fatigue", "Éviter l'hydratation", "Remplacer l'entraînement" },
			CorrectIndex = 0
		});

		// ——— Informatique ———
		Add(new QuestionTemplate
		{
			Id = "info-1", Subject = SubjectId.Informatique,
			Levels = LevelMask.College | LevelMask.Lycee | LevelMask.Superieur,
			Difficulty = Difficulty.Modere,
			Prompt = "Complexité typique de la recherche dichotomique dans un tableau trié (n éléments) :",
			Options = new[] { "O(log n)", "O(n)", "O(n²)", "O(1)" },
			CorrectIndex = 0
		});

		// ——— Méthodologie ———
		Add(new QuestionTemplate
		{
			Id = "meta-1", Subject = SubjectId.Methodologie,
			Levels = LevelMask.All,
			Difficulty = Difficulty.Facile,
			Prompt = "Une consigne demande « argumenter » : il faut surtout",
			Options = new[] { "Articuler raisons et exemples", "Lister des synonymes", "Copier le cours", "Écrire sans plan" },
			CorrectIndex = 0
		});

		// Compléments : variantes par niveau (même matière, autres items)
		Add(new QuestionTemplate
		{
			Id = "fr-red-4", Subject = SubjectId.Francais,
			Levels = LevelMask.Lycee,
			Difficulty = Difficulty.Modere,
			Prompt = "La liaison en « les‿amis » se fait parce que « les » se termine par :",
			Options = new[] { "Une consonne", "Une voyelle", "Un h aspiré", "Une pause" },
			CorrectIndex = 1
		});
		Add(new QuestionTemplate
		{
			Id = "math-proba-4", Subject = SubjectId.Mathematiques,
			Levels = LevelMask.Lycee | LevelMask.Superieur,
			Difficulty = Difficulty.Modere,
			Prompt = "On lance un dé équilibré à 6 faces. Probabilité d'obtenir un nombre pair :",
			Options = new[] { "1/6", "1/3", "1/2", "2/3" },
			CorrectIndex = 2
		});
		Add(new QuestionTemplate
		{
			Id = "hg-geo-4", Subject = SubjectId.HistoireGeo,
			Levels = LevelMask.Primaire,
			Difficulty = Difficulty.Facile,
			Prompt = "Capitale de la France :",
			Options = new[] { "Lyon", "Marseille", "Paris", "Toulouse" },
			CorrectIndex = 2
		});

		return list;
	}

	private static LevelMask BandToMask(SchoolLevelBand band) => band switch
	{
		SchoolLevelBand.Primaire => LevelMask.Primaire,
		SchoolLevelBand.College => LevelMask.College,
		SchoolLevelBand.Lycee => LevelMask.Lycee,
		SchoolLevelBand.Superieur => LevelMask.Superieur,
		_ => LevelMask.All
	};

	/// <summary>Filtre les modèles compatibles niveau + difficulté demandée (±1 si pool trop petit).</summary>
	public static IReadOnlyList<QuestionTemplate> Query(SubjectId subject, SchoolLevelBand level, Difficulty difficulty)
	{
		var mask = BandToMask(level);
		var primary = Templates.Where(t =>
			t.Subject == subject &&
			(t.Levels & mask) != 0 &&
			t.Difficulty == difficulty).ToList();

		if (primary.Count >= 3)
			return primary;

		var relaxed = Templates.Where(t =>
			t.Subject == subject &&
			(t.Levels & mask) != 0 &&
			Math.Abs((int)t.Difficulty - (int)difficulty) <= 1).ToList();

		if (relaxed.Count > 0)
			return relaxed;

		var anyLevel = Templates.Where(t => t.Subject == subject && (t.Levels & mask) != 0).ToList();
		if (anyLevel.Count > 0)
			return anyLevel;

		// Dernier recours : toute la matière (élargit le niveau si la banque est encore partielle)
		return Templates.Where(t => t.Subject == subject).ToList();
	}
}
