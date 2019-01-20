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
			elementNames = new Dictionary<string, string>();
			Regex patternParser = new Regex(elementPattern);
			foreach (Match m in patternParser.Matches(entryPattern))
			{
				string elementName = m.Value.XRegexReplaceSimple(elementPattern, "${ElementName}");
				elementNames[elementName] = "${" + elementName + "}";
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
		private static Dictionary<string, string> elementNames;

		public static List<Dictionary<string, string>> Parse(string effectsText)
		{
			List<Dictionary<string, string>> effects = new List<Dictionary<string, string>>();
			string remainder = effectsText
				.XRegexReplaceSimple("[Rr]ounds?", "Rounds")
				.XRegexReplaceSimple("[Ss]econds?s?", "Seconds")
				.XRegexReplaceSimple("[Mm]eters?", "Meters")
				.XRegexReplaceSimple("[Cc]hance", "Chance")
				.XRegexReplaceSimple("[Tt]arget", "Target");
			while (remainder != "")
			{
				string entry = remainder.XRegexReplace(textPattern, "${Entry}", RegexReplaceEmptyResultBehaviors.ThrowError);
				Dictionary<string, string> elements = new Dictionary<string, string>();
				foreach (string elementName in elementNames.Keys)
				{
					elements[elementName] = entry.XRegexReplace(entryPattern, elementNames[elementName], RegexReplaceEmptyResultBehaviors.Ignore);
				}
				effects.Add(elements);
				remainder = remainder.XRegexReplace(textPattern, "${Remainder}", RegexReplaceEmptyResultBehaviors.Ignore);
			}
			return effects;
		}
	}
}
