using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using JsonRazor;
using JsonRazor.Serialization;

namespace Animutivator
{
	public class GirlsConfig : IDisposable
	{
		private const string FallbackGirlPhotoUrl 
			= @"https://pre00.deviantart.net/ab52/th/pre/f/2016/123/6/1/kancolle_transparent_background_by_juanwan-da14pf6.png";
		private const string ConfigFileName  = "girls.config.json";
		private const string GirlsFolder     = "Girls";
		private const string DefaultGirlName = "Secret admirer";
		private const string DefaultGirlPath = @"Girls\Secret Admirer.png";

		private Config _config;
		private bool _pendingChanges;

		public Girl CurrentGirl => _config.GetCurrentGirl();

		public GirlsConfig()
		{
			if(! File.Exists(ConfigFileName))
			{
				WriteDefaultConfig();
			}

			if (! Directory.Exists(GirlsFolder))
			{
				CreateDefaultGirl();
			}

			if (_config == null)
			{
				using (var reader = new StreamReader(ConfigFileName))
				{
					_config = Deserializer.Consume<Config>(reader, ModelMembers.Property);
					Asure.NotNull(_config);
				}
			}
		}

		~GirlsConfig() {
			Dispose();
		}

		private void WriteDefaultConfig()
		{
			/*
			var array = new JsonArray();
			array.Append(
				new JsonObject()
				.Append(
					new JsonToken(nameof(Girl.Path), GirlsFolder), null, false)
					.Append(
					new JsonToken(nameof(Girl.Name), DefaultGirlName), null, false)
					.Append(
					new JsonArray()
					.Append(
						new JsonValue("I believe in You!"), false)
						.Append(
						new JsonValue("You are great! :)"), false)
						.Append(
						new JsonValue("You can do this, senpai! :3"), false),
					nameof(Girl.Quotes), false),
				 false);

			var config = new JsonObject()
				.Append(
				new JsonToken("Selected", DefaultGirlName), null, false)
				.Append(array, "Girls");
				*/
			_config = new Config();
			_config.Girls = new List<Girl>()
			{
				new Girl()
				{
					Name = DefaultGirlName,
					Path = DefaultGirlPath,
					Quotes = new List<string>()
					{
						"I believe in You!",
						"You are great! :)",
						"You can do this, senpai! :3"
					}
				}
			};
			_config.SetCurrentGirl(_config.Girls.First());

			WriteConfig();
		}

		private void CreateDefaultGirl()
		{
			Directory.CreateDirectory(GirlsFolder);

			using(var downloader = new WebClient())
			{
				var path = Path.GetFullPath(DefaultGirlPath);
				downloader.DownloadFile(FallbackGirlPhotoUrl, path);
			}
		}

		public void AddGirl([NotNull] Girl girl)
		{
			_config.Girls.Add(girl);
			_pendingChanges = true;
			//WriteConfig();
		}

		public Girl SelectGirl([NotNull] string name)
		{
			var girl = _config.Girls.First(g => g.Name == name);
			_config.SetCurrentGirl(girl);
			_pendingChanges = true;
			return girl;
		}

		private void WriteConfig()
		{
			using (var writer = new StreamWriter(ConfigFileName))
			{
				var json = Serializer.ProduceTokens(_config, ModelMembers.Property);
				writer.Write(json.ToString());
			}
		}

		public IEnumerable<Girl> Girls => _config.Girls;

		public void Dispose() 
		{
			GC.SuppressFinalize(this);
			if(_pendingChanges){
				_pendingChanges = false;
				WriteConfig();
			}
		}

		private class Config
		{
			[JsonOmit]
			private Girl      _current;
			public string     Selected { get; private set; }
			public List<Girl> Girls    { get; set; }

			public void SetCurrentGirl(Girl girl)
			{
				_current = girl;
				Selected = girl.Name;
			}

			[NotNull]
			public Girl GetCurrentGirl()
			{
				if(_current == null)
				{
					_current = Girls.First(g => g.Name == Selected);
				}
				return _current;
			}
		}
	}
}
