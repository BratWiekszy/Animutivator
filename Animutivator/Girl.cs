using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JsonRazor.Serialization;

namespace Animutivator
{
	public class Girl
	{
		public string Path { get; set; }
		public string Name { get; set; }
		public List<string> Quotes { get; set; }

		public Girl() { }
	}
}
