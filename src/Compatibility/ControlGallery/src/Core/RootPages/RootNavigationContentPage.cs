using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Graphics;

namespace Microsoft.Maui.Controls.ControlGallery
{
	// NavigationPage -> ContentPage
	public class RootNavigationContentPage : NavigationPage
	{

		public RootNavigationContentPage(string hierarchy)
		{
			AutomationId = hierarchy + "PageId";

			var content = new ContentPage
			{
				BackgroundColor = Colors.Yellow,
				Title = "Testing 123",
				Content = new SwapHierachyStackLayout(hierarchy)
			};

			PushAsync(content);
		}
	}
}