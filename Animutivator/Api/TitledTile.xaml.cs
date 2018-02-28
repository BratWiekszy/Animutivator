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

namespace Animutivator.Api
{
    /// <summary>
    /// Interaction logic for TitledTile.xaml
    /// </summary>
    public partial class TitledTile : UserControl
    {
		public string Title 
		{
			get { return this.title.Content as string; }
			set { this.title.Content = value; } 
		}
		public BitmapImage TileImage
		{
			get { return this.image.Source as BitmapImage; }
			set { this.image.Source = value; }
		}

		public static readonly RoutedEvent ClickEvent =
			EventManager.RegisterRoutedEvent(nameof(Click),
											RoutingStrategy.Bubble,
											typeof(RoutedEventHandler),
											typeof(TitledTile));
		public event RoutedEventHandler Click
		{
			add { AddHandler(ClickEvent, value); }
			remove { RemoveHandler(ClickEvent, value); }
		}

		public object ID { get; set; }


        public TitledTile(object id)
        {
            InitializeComponent();
			this.DataContext = this;
			ID = id;
        }

		private void button_Click(object sender, RoutedEventArgs e)
		{
			RaiseEvent(new RoutedEventArgs(ClickEvent));
		}
	}
}
