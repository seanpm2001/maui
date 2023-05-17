﻿using System.Linq;
using Microsoft.Maui.Controls.CustomAttributes;
using Microsoft.Maui.Controls.Internals;
using Microsoft.Maui.Graphics;

#if UITEST
using Xamarin.UITest;
using NUnit.Framework;
#endif

namespace Microsoft.Maui.Controls.ControlGallery.Issues
{
#if UITEST
	[NUnit.Framework.Category(Compatibility.UITests.UITestCategories.Github5000)]
#endif
	[Preserve(AllMembers = true)]
	[Issue(IssueTracker.Github, 1426, "SetHasNavigationBar screen height wrong", PlatformAffected.iOS)]
	public class Issue1426 : TestTabbedPage
	{
		protected override void Init()
		{
			Children.Add(new NavigationPage(new HomePage()) { Title = "Home", BarBackgroundColor = Colors.Red });
		}

		class HomePage : ContentPage
		{
			public HomePage()
			{
				Title = "Home";
				var grd = new Grid { BackgroundColor = Colors.Brown };
				grd.RowDefinitions.Add(new RowDefinition());
				grd.RowDefinitions.Add(new RowDefinition());
				grd.RowDefinitions.Add(new RowDefinition());

				var boxView = new BoxView { BackgroundColor = Colors.Blue };
				grd.Add(boxView, 0, 0);
				var btn = new Button()
				{
					BackgroundColor = Colors.Pink,
					Text = "NextButtonID",
					AutomationId = "NextButtonID",
					Command = new Command(async () =>
					{
						var btnPop = new Button { Text = "PopButtonId", AutomationId = "PopButtonId", Command = new Command(async () => await Navigation.PopAsync()) };
						var page = new ContentPage
						{
							Title = "Detail",
							Content = btnPop,
							BackgroundColor = Colors.Yellow
						};
						//This breaks layout when you pop!
						NavigationPage.SetHasNavigationBar(page, false);
						await Navigation.PushAsync(page);
					})
				};

				grd.Add(btn, 0, 1);
				var image = new Image() { Source = "coffee.png", AutomationId = "CoffeeImageId", BackgroundColor = Colors.Yellow };
				image.VerticalOptions = LayoutOptions.End;
				grd.Add(image, 0, 2);
				Content = grd;

			}
		}

#if UITEST
		[Test]
		public void Github1426Test()
		{
			RunningApp.Screenshot("You can see the coffe mug");
			RunningApp.WaitForElement(q => q.Marked("CoffeeImageId"));
			RunningApp.WaitForElement(q => q.Marked("NextButtonID"));
			RunningApp.Tap(q => q.Marked("NextButtonID"));
			RunningApp.WaitForElement(q => q.Marked("PopButtonId"));
			RunningApp.Tap(q => q.Marked("PopButtonId"));
			RunningApp.WaitForElement(q => q.Marked("CoffeeImageId"));
			RunningApp.Screenshot("Coffe mug Image is still there on the bottom");
		}
#endif
	}
}