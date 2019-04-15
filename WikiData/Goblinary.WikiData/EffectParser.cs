namespace Goblinary.WikiData
{
	using System;
	using System.Collections.Generic;
	using System.Text.RegularExpressions;

	using Goblinary.Common;

	internal static class EffectParser
	{
		static EffectParser()
		{
			ElementNames = new Dictionary<string, string>();
			var patternParser = new Regex(elementPattern);
			foreach (Match m in patternParser.Matches(entryPattern))
			{
				var elementName = m.Value.XRegexReplaceSimple(elementPattern, "${ElementName}");
				ElementNames[elementName] = "${" + elementName + "}";
			}
		}

		private const string elementPattern = @"\(\?\<(?<ElementName>[^\>]*)\>";
		private const string textPattern = @"^(?<Entry>[^,\(]*(\([^\)]*\)[^,]*)?)(, *(?<Remainder>.*))?$";
		private const string entryPattern
			= @"^(?<EffectDescription>"
				+ @"(?<Effect>(?<EffectName>.*?)( (\+|\-)?[0-9]*%?)?)"
				+ @"( \(((?<Duration>[0-9]*) (?<DurationUnits>Rounds|Seconds))?(, )?((?<Chance>[0-9]*\%) Chance)?\))?"
				+ @"( \((?<Distance>[0-9]*) Meters\))?"
				+ @"( (?<Target>to (All|Self|Target)))?"
				+ @"("
					+ @" on (?<OnEvent>.*?)( to (?<OnEventTarget>.*))?"
				+ @"|"
					+ @" (?<Conditional>to All Targets with|if (Attacker|Target) (has|is)) (?<State>.*)"
				+ @"|"
					+ @" (with (?<WithWeapon>.*))"
				+ @")?"
				+ @"( (?<PerKeyword>per Keyword))?"
			+ @")$";
		private static readonly Dictionary<string, string> ElementNames;

		public static List<Dictionary<string, string>> Parse(string effectsText)
		{
			var effects = new List<Dictionary<string, string>>();
			var remainder = effectsText
				.XRegexReplaceSimple("[Rr]ounds?", "Rounds")
				.XRegexReplaceSimple("[Ss]econds?s?", "Seconds")
				.XRegexReplaceSimple("[Mm]eters?", "Meters")
				.XRegexReplaceSimple("[Cc]hance", "Chance")
				.XRegexReplaceSimple("[Tt]arget", "Target");
			while (remainder != "")
			{
				var entry = remainder.XRegexReplace(textPattern, "${Entry}", RegexReplaceEmptyResultBehaviors.ThrowError);
				var elements = new Dictionary<string, string>();
				foreach (var elementName in ElementNames.Keys)
				{
					elements[elementName] = entry.XRegexReplace(entryPattern, ElementNames[elementName]);
				}
				effects.Add(elements);
				remainder = remainder.XRegexReplace(textPattern, "${Remainder}");
			}
			return effects;
		}
	}
}
