using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace Animutivator
{
	public class TrayIcon
	{
		private const int ShowGirlActionIndex = 2;

		private ITrayMenuActions _trayActions;
		private NotifyIcon       _icon;

		public TrayIcon([NotNull] ITrayMenuActions actions, bool isGirlVisible)
		{
			_trayActions = actions ??
				throw new ArgumentNullException(nameof(actions));

			_icon = new NotifyIcon();
			using (var stream = new FileStream(GetString("tray.icon"), FileMode.Open))
				_icon.Icon = new System.Drawing.Icon(stream);

			var contextMenu   = CreateContextMenu(isGirlVisible);
			_icon.ContextMenu = contextMenu;
			_icon.Visible     = true;
		}

		private ContextMenu CreateContextMenu(bool isGirlVisible)
		{
			var menu = new MenuItem[]
			{
				new MenuItem(GetString("tray.addGirl"), 
					(s, e) => _trayActions.AddGirl()),
				new MenuItem(GetString("tray.selectGirl"), 
					(s, e) => _trayActions.SelectGirl()),
				null,
				new MenuItem(GetString("tray.exit"), (s, e) => 
				{
					_icon.Dispose();
					_trayActions.Exit();
				}),
			};

			menu[2] = GetGirlVisibilityAction(isGirlVisible);

			return new ContextMenu(menu);
		}

		private MenuItem GetGirlVisibilityAction(bool visible)
		{
			return visible
				? new MenuItem(GetString("tray.hideGirl"),
					(s, e) => SwitchGirlVisibility(false))
				: new MenuItem(GetString("tray.showGirl"),
					(s, e) => SwitchGirlVisibility(true));
		}

		private void SwitchGirlVisibility(bool visible)
		{
			_trayActions.ShowGirl(visible);
			var items = _icon.ContextMenu.MenuItems;
			items.RemoveAt(ShowGirlActionIndex);
			items.Add(ShowGirlActionIndex, GetGirlVisibilityAction(visible));
		}

		private string GetString([NotNull] string key)
		{
			return (string)System.Windows.Application.Current.FindResource(key);
		}
	}
}
