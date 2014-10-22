using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Android.Widget;
using FormsSVG.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XamSvg;
using Xamarin.Forms.Labs.Controls;

[assembly: ExportRenderer (typeof(SvgImage), typeof(SvgImageRenderer))]

namespace FormsSVG.Droid.Renderers
{
	public class SvgImageRenderer : ImageRenderer
	{
		private bool _svgSourceSet;
		private Dictionary<string, Stream> _svgStreamByPath;

		private Dictionary<string, Stream> SvgStreamByPath {
			get {
				if (_svgStreamByPath == null) {
					_svgStreamByPath = new Dictionary<string, Stream> ();
				}

				return _svgStreamByPath;
			}
		}

		protected override void OnElementPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			var imageView = Control as ImageView;
			var formsControl = sender as SvgImage;

			if (imageView != null && formsControl != null && !formsControl.IsLoading && !_svgSourceSet) {
				try {
					_svgSourceSet = true;

					var width = (int)formsControl.WidthRequest <= 0 ? 100 : (int)formsControl.WidthRequest;
					var height = (int)formsControl.HeightRequest <= 0 ? 100 : (int)formsControl.HeightRequest;

					var svg = SvgFactory.GetBitmap (GetSvgStream (formsControl.SvgResourceId, formsControl.ResourceAssembly), width, height);

					imageView.SetImageBitmap (svg);
				} catch (Exception ex) {
					throw new Exception ("Problem setting image source", ex);
				}
			}

			base.OnElementPropertyChanged (sender, e);
		}

		private Stream GetSvgStream (string svgResourceId, Assembly resourceAssembly)
		{
			Stream stream = null;
			//Insert into Dictionary
			if (!SvgStreamByPath.ContainsKey (svgResourceId)) {
				//todo there is probably a better way to get the proper assembly, rather then passing it in
				stream = resourceAssembly.GetManifestResourceStream (svgResourceId);

				if (stream == null) {
					throw new Exception (string.Format ("Not able to retrieve stream for file {0}", svgResourceId));
				}

				SvgStreamByPath.Add (svgResourceId, stream);
        
				return stream;
			}

			//Get from dictionary
			stream = SvgStreamByPath [svgResourceId];
			//Reset the stream position otherwise an error is thrown (SvgFactory.GetBitmap sets the position to position max)
			stream.Position = 0;

			return stream;
		}
	}
}