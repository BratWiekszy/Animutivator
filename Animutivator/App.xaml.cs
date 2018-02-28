using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Animutivator
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		private AppBoot _boot;

		public App()
		{
			this.Startup += (s, e) => Boot();
		}

		private void Boot()
		{
			_boot = new AppBoot();
		}
	}
}
