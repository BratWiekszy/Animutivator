using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using JetBrains.Annotations;

namespace Animutivator.Api
{
    public class Api
    {
		public static BitmapImage CreateImageFromPath([NotNull] string path)
		{
			Asure.NotNull(path);

			var bitmap = new BitmapImage();
			bitmap.BeginInit();
			bitmap.UriSource = new Uri(Path.GetFullPath(path));
			bitmap.EndInit();
			return bitmap;
		}
	}
}
