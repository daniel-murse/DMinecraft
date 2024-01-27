using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMinecraft.PhysicalClient.Graphics.OpenGL.HighLevel.Text
{

	[Serializable]
	public class FontException : Exception
	{
		public FontException() { }
		public FontException(string message) : base(message) { }
		public FontException(string message, Exception inner) : base(message, inner) { }
	}
}
