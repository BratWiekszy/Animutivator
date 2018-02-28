using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using JetBrains.Annotations;

namespace Animutivator
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public string Quote { get; set; }

		private IQuoteProvider _quoteProvider;
		private bool           _quoteShown;

		public IQuoteProvider QuoteProvider
		{
			set { _quoteProvider = value; }
		}

		private bool QuoteShown
		{
			set {
				this.quoteField.Visibility = value
										   ? Visibility.Visible
										   : Visibility.Hidden;
				_quoteShown = value;
			}							   
		}

		public MainWindow()
		{
			InitializeComponent();
			var screen = SystemParameters.WorkArea;
			this.Top   = screen.Height - this.Height;
			this.Left  = screen.Width  - this.Width;
			QuoteShown = false;
		}

		public void SetGirl([NotNull] Girl girl)
		{
			Asure.NotNull(girl, girl.Path, girl.Name, girl.Quotes);

			QuoteShown = false;
			BitmapImage image = Api.Api.CreateImageFromPath(girl.Path);

			this.girlImage.Source  = image;
			this.girlImage.ToolTip = girl.Name;
		}

		private void girlImage_MouseDown(object sender, MouseButtonEventArgs e)
		{
			QuoteShown = e.ChangedButton == MouseButton.Left;

			_quoteProvider.ProvideQuote(this.quoteField.Inlines);
		}
	}
}
