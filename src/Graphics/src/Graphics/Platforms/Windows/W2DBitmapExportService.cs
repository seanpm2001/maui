﻿using System;

#if MAUI_GRAPHICS_WIN2D
namespace Microsoft.Maui.Graphics.Win2D
#else
namespace Microsoft.Maui.Graphics.Platform
#endif
{
	public class W2DBitmapExportService : IBitmapExportService
	{
		public BitmapExportContext CreateContext(int width, int height, float displayScale = 1)
		{
			throw new NotImplementedException();
		}
	}
}