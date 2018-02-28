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
using System.Windows.Shapes;
using Animutivator.Api;
using JetBrains.Annotations;

namespace Animutivator
{
    /// <summary>
    /// Interaction logic for SelectGirlWindow.xaml
    /// </summary>
    public partial class SelectGirlWindow : Window
    {
		private Action<Girl> _callback;


        public SelectGirlWindow([NotNull] IEnumerable<Girl> girls, 
			[NotNull] Action<Girl> callback)
        {
			Asure.NotNull(girls, callback);
			_callback = callback;

            InitializeComponent();
			var children = this.girlsPanel.Children;
			var tiles = girls.Select(g =>
			{
				var bitmap = Api.Api.CreateImageFromPath(g.Path);
				// set girl as id for tile so that we know which was clicked
				var tile = new TitledTile(g)
				{
					Title = g.Name,
					TileImage = bitmap
				};
				tile.Click += OnGirlTileClicked;
				return tile;
			});

			foreach (var tile in tiles)
				children.Add(tile);

			Show();
        }

		private void OnGirlTileClicked(object sender, RoutedEventArgs e)
		{
			e.Handled = true;

			// e.Source resolves to same tile
			var tile = sender as TitledTile;
			var girl = tile.ID as Girl;

			_callback.Invoke(girl);
			Close();
		}
    }
}
