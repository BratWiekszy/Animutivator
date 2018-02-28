using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using JetBrains.Annotations;

namespace Animutivator
{
    public class QuoteEmojiAnalizer : IQuoteAnalizer
    {
		private Regex _regex;
		private Dictionary<string, string> _emojiPaths;
		private MatchCollection _matches;
		private string _quote;

		public QuoteEmojiAnalizer([NotNull] Regex regex, 
			[NotNull] Dictionary<string, string> emojiPaths)
		{
			Asure.NotNull(regex, emojiPaths);
			_regex      = regex;
			_emojiPaths = emojiPaths;
		}

		public void Analize(string quote)
		{
			_matches = _regex.Matches(quote);
			_quote   = quote;
		}

		public void ProcessAndStyle(InlineCollection line)
		{
			int index  = 0;
			
			foreach(Match match in _matches)
			{
				var substring = _quote.Substring(index, match.Index - index);
				line.Add(substring);

				index = match.Index + match.Length + 1;

				var image = new Image();
				image.Height = 16;
				image.Width  = 16;
				image.Source = Api.Api.CreateImageFromPath(_emojiPaths[match.Value]);

				line.Add(image);
			}

			if (index < _quote.Length)
				line.Add(_quote.Substring(index));
		}

    }

}
