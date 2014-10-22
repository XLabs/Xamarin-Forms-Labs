using System;
using System.Collections.Generic;
using Xamarin.Forms.Labs.Controls;
using System.Reflection;

namespace Xamarin.Forms.Labs.Sample
{
	public class SvgImagePage : ContentPage
	{
		public SvgImagePage ()
		{
			var stackLayout = new StackLayout ();
			stackLayout.Children.Add (new Label {
				Text = "SVGs are AWESOME!!!",
				Font = Font.BoldSystemFontOfSize (20),
				HorizontalOptions = LayoutOptions.Center
			});

			var svgsNamespace = "Xamarin.Forms.Labs.Sample.Images";

			//Get SVGs from http://thenounproject.com/
			var svgResourceIds = new List<string> {
				string.Format ("{0}.tiger.svg", svgsNamespace),
				string.Format ("{0}.star_depressed.svg", svgsNamespace),
				string.Format ("{0}.star_off.svg", svgsNamespace),
				string.Format ("{0}.star_on.svg", svgsNamespace),
				string.Format ("{0}.pin.svg", svgsNamespace),
				string.Format ("{0}.hipster.svg", svgsNamespace),
			};

			var resourceAssembly = typeof(SvgImagePage).GetTypeInfo ().Assembly;

			foreach (var svgResourceId in svgResourceIds) {
				var horizontalStackLayout = new StackLayout { Orientation = StackOrientation.Horizontal };

				for (var y = 1; y <= 5; y++) {
					var svgImage = new SvgImage {
						SvgResourceId = svgResourceId,
						ResourceAssembly = resourceAssembly,
						HeightRequest = y * 30,
						WidthRequest = y * 30
					};

					if (svgResourceId.Contains ("hipster"))
						svgImage.BackgroundColor = Color.White;

					horizontalStackLayout.Children.Add (svgImage);
				}

				stackLayout.Children.Add (horizontalStackLayout);
			}

			Content = new ScrollView { Content = stackLayout };
		}
	}
}

