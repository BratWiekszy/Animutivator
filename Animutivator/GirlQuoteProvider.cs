using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using JetBrains.Annotations;

namespace Animutivator
{
    public class GirlQuoteProvider : IQuoteProvider
    {
		private IQuoteAnalizer _analizer;
		private IList<string>  _quotes;
		private Random         _quoteRandomizer;

		public GirlQuoteProvider([NotNull] IList<string> quotes, 
			[NotNull] IQuoteAnalizer quoteAnalizer)
		{
			Asure.NotNull(quotes, quoteAnalizer);
			_quotes          = quotes;
			_analizer        = quoteAnalizer;
			_quoteRandomizer = new Random();
		}

		private string GetNextQuote()
		{
			var limit = _quotes.Count;
			var next = (_quoteRandomizer.Next() & int.MaxValue) % limit;

			Asure.CantBe(next < 0 && next >= limit);
			return _quotes[next];
		}

		public void ProvideQuote([NotNull] InlineCollection inlines)
		{
			Asure.NotNull(inlines);

			inlines.Clear();
			var quote = GetNextQuote();
			_analizer.Analize(quote);
			_analizer.ProcessAndStyle(inlines);
		}

		private InlineCollection StyleFontWeight(string quote, InlineCollection inlines)
		{
			FontWeight weight;
			if (quote.Contains('!'))
				weight = FontWeights.Bold;
			else if (quote.Contains('?'))
				weight = FontWeights.Light;
			else 
				return inlines;

			var inline = new Run(quote);
			var span   = new Span(inline);
			span.FontWeight = weight;

			inlines.Add(span);
			return span.Inlines;
		}
    }
}
