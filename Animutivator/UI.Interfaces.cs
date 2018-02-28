using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using JetBrains.Annotations;

namespace Animutivator
{
	public interface ITrayMenuActions
	{
		void Exit();
		void ShowGirl(bool show);
		void AddGirl();
		void SelectGirl();
	}

	public interface IQuoteProvider
	{
		void ProvideQuote([NotNull] InlineCollection inlines);
	}

	public interface IQuoteAnalizer
	{
		void Analize(string quote);
		void ProcessAndStyle(InlineCollection inlines);
	}
}
