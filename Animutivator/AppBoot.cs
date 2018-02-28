using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Animutivator
{
	public class AppBoot
	{
		private GirlWindow  _mainWindow;
		private TrayIcon    _notifyIcon;
		private GirlsConfig _config;

		/// <summary>
		/// Boots application components.
		/// </summary>
		public AppBoot()
		{
			_config = new GirlsConfig();
			_mainWindow = new GirlWindow();
			_mainWindow.SetGirl(_config.CurrentGirl);

			_notifyIcon = new TrayIcon(new TrayCallbacks(this), true);
		}

		private void Exit()
		{
			_mainWindow.Close();
		}


		private class TrayCallbacks : ITrayMenuActions
		{
			private AppBoot _boot;

			public TrayCallbacks([NotNull] AppBoot appBoot)
			{
				_boot = appBoot ?? throw new ArgumentNullException();
			}

			void ITrayMenuActions.AddGirl()
			{
				var addWindow = new AddGirlWindow(_boot._config.AddGirl);
			}

			void ITrayMenuActions.Exit()
			{
				_boot.Exit();
			}

			void ITrayMenuActions.SelectGirl()
			{
				var selectWindow = new SelectGirlWindow(_boot._config.Girls, g =>
				{
					_boot._config.SelectGirl(g.Name);
					_boot._mainWindow.SetGirl(g);
				});
			}

			void ITrayMenuActions.ShowGirl(bool show)
			{
				_boot._mainWindow.Show(show);
			}
		}
	}
}
