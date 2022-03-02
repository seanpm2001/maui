﻿using System;
using System.Drawing;
using CoreGraphics;
using Foundation;
using ObjCRuntime;
using UIKit;

namespace Microsoft.Maui.Platform
{
	public class MauiSearchBar : UISearchBar, IMauiSearchBar
	{
		event EventHandler? _onTraitCollectionDidChange;
		event EventHandler? IMauiSearchBar.OnTraitCollectionDidChange
		{
			add => _onTraitCollectionDidChange += value;
			remove => _onTraitCollectionDidChange -= value;
		}

		public MauiSearchBar() : this(RectangleF.Empty)
		{
		}

		public MauiSearchBar(NSCoder coder) : base(coder)
		{
		}

		public MauiSearchBar(CGRect frame) : base(frame)
		{
		}

		protected MauiSearchBar(NSObjectFlag t) : base(t)
		{
		}

		protected internal MauiSearchBar(NativeHandle handle) : base(handle)
		{
		}

		// Native Changed doesn't fire when the Text Property is set in code
		// We use this event as a way to fire changes whenever the Text changes
		// via code or user interaction.
		public event EventHandler<UISearchBarTextChangedEventArgs>? TextSetOrChanged;

		public override string? Text
		{
			get => base.Text;
			set
			{
				var old = base.Text;

				base.Text = value;

				if (old != value)
				{
					TextSetOrChanged?.Invoke(this, new UISearchBarTextChangedEventArgs(value ?? string.Empty));
				}
			}
		}

		public override void TraitCollectionDidChange(UITraitCollection? previousTraitCollection)
		{
			base.TraitCollectionDidChange(previousTraitCollection);

			// Make sure the control adheres to changes in UI theme
			if (PlatformVersion.IsAtLeast(13) && previousTraitCollection?.UserInterfaceStyle != TraitCollection.UserInterfaceStyle)
			{
				_onTraitCollectionDidChange?.Invoke(this, EventArgs.Empty);
			}
		}
	}
}