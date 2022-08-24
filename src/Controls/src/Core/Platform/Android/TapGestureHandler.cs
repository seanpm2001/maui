#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using Android.Views;
using Microsoft.Maui.Controls.Internals;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Platform;
using AView = Android.Views.View;

namespace Microsoft.Maui.Controls.Platform
{
	internal class TapGestureHandler
	{
		public TapGestureHandler(Func<View?> getView, Func<IList<GestureElement>> getChildElements)
		{
			GetView = getView;
			GetChildElements = getChildElements;
		}

		Func<IList<GestureElement>> GetChildElements { get; }
		Func<View?> GetView { get; }

		public void OnSingleClick()
		{
			// only handle click if we don't have double tap registered
			if (TapGestureRecognizers(2).Any())
				return;

			OnTap(1, null);
		}

		public bool OnTap(int count, MotionEvent? e)
		{
			Point point;

			if (e == null)
				point = new Point(-1, -1);
			else
				point = new Point(e.GetX(), e.GetY());

			var view = GetView();

			if (view == null)
				return false;

			var captured = false;

			var children = view.GetChildElements(point);

			if (children != null)
			{
				foreach (var recognizer in children.GetChildGesturesFor<TapGestureRecognizer>(recognizer => recognizer.NumberOfTapsRequired == count))
				{
					recognizer.SendTapped(view, CalculatePosition);
					captured = true;
				}
			}

			if (captured)
				return captured;

			IEnumerable<TapGestureRecognizer> gestureRecognizers = TapGestureRecognizers(count);
			foreach (var gestureRecognizer in gestureRecognizers)
			{
				gestureRecognizer.SendTapped(view, CalculatePosition);
				captured = true;
			}

			return captured;

			Point? CalculatePosition(IElement? element)
			{
				var context = GetView()?.Handler?.MauiContext?.Context;

				if (context == null)
					return null;

				if (e == null)
					return null;

				if (element == null)
				{
					return new Point(context.FromPixels(e.RawX), context.FromPixels(e.RawY));
				}

				if (element == GetView())
				{
					return new Point(context.FromPixels(e.GetX()), context.FromPixels(e.GetY()));
				}

				if (element?.Handler?.PlatformView is AView aView)
				{
					var location = aView.GetLocationOnScreenPx();

					var x = e.RawX - location.X;
					var y = e.RawY - location.Y;

					return new Point(context.FromPixels(x), context.FromPixels(y));
				}

				return null;
			}
		}

		public bool HasAnyGestures()
		{
			var view = GetView();
			return view != null && view.GestureRecognizers.OfType<TapGestureRecognizer>().Any()
								|| GetChildElements().GetChildGesturesFor<TapGestureRecognizer>().Any();
		}

		public IEnumerable<TapGestureRecognizer> TapGestureRecognizers(int count)
		{
			var view = GetView();
			if (view == null)
				return Enumerable.Empty<TapGestureRecognizer>();

			return view.GestureRecognizers.GetGesturesFor<TapGestureRecognizer>(recognizer => recognizer.NumberOfTapsRequired == count);
		}

	}
}