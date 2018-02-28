using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using JetBrains.Annotations;
using JsonRazor.Serialization;

namespace Animutivator
{
    public class QuoteEmojiConfig
    {
		private const string ConfigFileName = "emoji.config.json";
		private const string EmojiFolder    = "images";
		private const string EscapedChars   = "|)";

		private List<Emoji> _config;
		private Regex       _analizer;
		private Dictionary<string, string> _pathLookup;

		public QuoteEmojiConfig()
		{
			if (_config == null)
			{
				using (var reader = new StreamReader(ConfigFileName))
				{
					_config = Deserializer.Consume<List<Emoji>>(reader, ModelMembers.Property);
					Asure.NotNull(_config);
				}
			}
			NormalizeCodes();
			BuildPathsLookup();
			BuildAnalizer();
		}

		/// <summary>
		/// Normalize (escape) chars that are meaningfull in regex expression.
		/// </summary>
		private void NormalizeCodes()
		{
			_config.ForEach(e =>
			{
				foreach(var c in EscapedChars)
				{
					e.Code = e.Code.Replace(c.ToString(), $@"\{c}");
				}
			});
		}

		/// <summary>
		/// Build dictionary to speed up search for emoji after code found.
		/// </summary>
		private void BuildPathsLookup()
		{
			_pathLookup = new Dictionary<string, string>(_config.Count);
			foreach (var emoji in _config)
			{
				var path = Path.Combine(EmojiFolder, emoji.FileName);
				path = Path.GetFullPath(path);
				_pathLookup.Add(emoji.Code, path);
			}
		}

		private void BuildAnalizer()
		{
			var pattern = string.Join("|", _config.Select(e => e.Code));
			_analizer   = new Regex(pattern);
		}

		[NotNull]
		public IQuoteAnalizer GetQuoteAnalizer()
		{
			return new QuoteEmojiAnalizer(_analizer, _pathLookup);
		}

		private class Emoji
		{
			public string Code { get; set; }
			public string FileName { get; set; }
		}
	}
}
