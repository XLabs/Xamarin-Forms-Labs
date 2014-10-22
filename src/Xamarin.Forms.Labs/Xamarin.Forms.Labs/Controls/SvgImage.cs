using System;
using Xamarin.Forms;
using System.Reflection;

namespace Xamarin.Forms.Labs.Controls
{
	/// <summary>
	/// Make sure the Build Action for all Svgs is set to EmbeddedResource
	/// </summary>
	public class SvgImage : Image
	{
		/// <summary>
		/// A reference to the Assembly containing the svg resource. 
		/// Example : typeof(SvgImagePage).GetTypeInfo ().Assembly;
		/// </summary>
		/// <value>The resource assembly.</value>
		public Assembly ResourceAssembly { get; set; }

		public string SvgResourceId { get; set; }
	}
}

