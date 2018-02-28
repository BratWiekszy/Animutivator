using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Animutivator
{
	public class GirlWindow
	{
		private MainWindow       _mainWindow;
		private QuoteEmojiConfig _emojiConfig;
		private bool             _visible = false;

		public GirlWindow()
		{
			_mainWindow  = new MainWindow();
			_emojiConfig = new QuoteEmojiConfig();
		}

		public void SetGirl([NotNull] Girl girl)
		{
			Asure.NotNull(girl);
			_mainWindow.SetGirl(girl);
			_mainWindow.QuoteProvider = new GirlQuoteProvider(girl.Quotes, 
				_emojiConfig.GetQuoteAnalizer());

			if (_visible == false)
			{
				_mainWindow.Show();
				_visible = true;
			}
		}

		public void Show(bool show)
		{
			Asure.CantBe(show == _visible);

			if(show) _mainWindow.Show();
			else
				_mainWindow.Hide();

			_visible = show;
		}

		public void Close()
		{
			_mainWindow.Close();
		}
	}
}
