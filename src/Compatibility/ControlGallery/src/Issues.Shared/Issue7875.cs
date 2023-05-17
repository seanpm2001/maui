﻿using Microsoft.Maui.Controls.CustomAttributes;
using Microsoft.Maui.Controls.Internals;
using Microsoft.Maui.Graphics;

#if UITEST
using Microsoft.Maui.Controls.Compatibility.UITests;
using Xamarin.UITest;
using NUnit.Framework;
#endif

namespace Microsoft.Maui.Controls.ControlGallery.Issues
{
#if UITEST
	[Category(UITestCategories.Button)]
#endif
	[Preserve(AllMembers = true)]
	[Issue(IssueTracker.Github, 7875, "Button size changes when setting Accessibility properties", PlatformAffected.Android)]
	public class Issue7875 : TestContentPage
	{
		public Issue7875()
		{
			Title = "Issue 7875";

			var layout = new Grid();

			layout.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			layout.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			layout.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

			var instructions = new Label
			{
				BackgroundColor = Colors.Black,
				TextColor = Colors.White,
				Text = "If the buttons below have the same size, the test has passed."
			};
			layout.Add(instructions, 0, 0);

			var button = new Button
			{
				BackgroundColor = Colors.Gray,
				HorizontalOptions = LayoutOptions.Center,
				ImageSource = "calculator.png",
				Text = "Text"
			};
			layout.Add(button, 0, 1);

			var accesibilityButton = new Button
			{
				BackgroundColor = Colors.Gray,
				HorizontalOptions = LayoutOptions.Center,
				ImageSource = "calculator.png",
				Text = "Text"
			};
			accesibilityButton.SetValue(AutomationProperties.NameProperty, "AccesibilityButton");
			accesibilityButton.SetValue(AutomationProperties.HelpTextProperty, "Help Large Text");
			layout.Add(accesibilityButton, 0, 2);

			Content = layout;
		}

		protected override void Init()
		{

		}
	}
}