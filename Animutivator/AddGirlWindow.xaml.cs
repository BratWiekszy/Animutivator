using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using JetBrains.Annotations;

namespace Animutivator
{
	/// <summary>
	/// Interaction logic for AddGirlWindow.xaml
	/// </summary>
	public partial class AddGirlWindow : Window
	{
		public string GirlName      { get; set; }
		public string GirlImagePath { get; set; }
		public ObservableCollection<StringItem> Quotes { get; set; } 
			= new ObservableCollection<StringItem>();

		private Action<Girl> _callback;


		public AddGirlWindow([NotNull] Action<Girl> callback)
		{
			Asure.NotNull(callback);
			_callback   = callback;

			this.DataContext = this;
			addQuoteButton_Click(null, null);
			InitializeComponent();
			confirmButton.IsEnabled = false;
			Show();
		}

		private void selectImageButton_Click(object sender, RoutedEventArgs e)
		{
			var fileDialog = new OpenFileDialog();
			fileDialog.CheckFileExists = true;
			fileDialog.CheckPathExists = true;
			fileDialog.Multiselect = false;
			fileDialog.Title = "Select girl image";
			fileDialog.Filter = "Images|*.png;*.gif;";

			if (fileDialog.ShowDialog() != System.Windows.Forms.DialogResult.OK)
				return;

			GirlImagePath = fileDialog.FileName;
			pathInput.Text = GirlImagePath;
			var bitmap = Api.Api.CreateImageFromPath(GirlImagePath);

			this.girlRefImage.Source = bitmap;
			confirmButton.IsEnabled  = true;
		}

		private void addQuoteButton_Click(object sender, RoutedEventArgs e)
		{
			if (Quotes.Any(q => q.Value == "..." || q.Value.Length < 3))
				return;

			Quotes.Add(new StringItem() { Value = "..." });
		}

		private void confirmButton_Click(object sender, RoutedEventArgs e)
		{
			if (string.IsNullOrEmpty(GirlImagePath) || string.IsNullOrEmpty(GirlName))
				return;

			string quote = Quotes[0].Value;
			if (Quotes.Count == 1 && (quote == "..." || quote.Length < 2))
				return;

			var girl = new Girl()
			{
				Name   = GirlName,
				Path   = GirlImagePath,
				Quotes = Quotes.Where(q => q.Value != "...").Select(q => q.Value).ToList()
			};

			_callback.Invoke(girl);
			Close();
		}
	}

	public class StringItem
	{
		public string Value { get; set; }
	}
}