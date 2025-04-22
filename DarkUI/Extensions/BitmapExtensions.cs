using System;
using System.Drawing;

namespace DarkUI.Extensions
{
	// Token: 0x02000016 RID: 22
	internal static class BitmapExtensions
	{
		// Token: 0x0600008A RID: 138 RVA: 0x000055D0 File Offset: 0x000037D0
		internal static Bitmap SetColor(this Bitmap bitmap, Color color)
		{
			Bitmap newBitmap = new Bitmap(bitmap.Width, bitmap.Height);
			for (int i = 0; i < bitmap.Width; i++)
			{
				for (int j = 0; j < bitmap.Height; j++)
				{
					bool flag = bitmap.GetPixel(i, j).A > 0;
					if (flag)
					{
						newBitmap.SetPixel(i, j, color);
					}
				}
			}
			return newBitmap;
		}

		// Token: 0x0600008B RID: 139 RVA: 0x0000564C File Offset: 0x0000384C
		internal static Bitmap ChangeColor(this Bitmap bitmap, Color oldColor, Color newColor)
		{
			Bitmap newBitmap = new Bitmap(bitmap.Width, bitmap.Height);
			for (int i = 0; i < bitmap.Width; i++)
			{
				for (int j = 0; j < bitmap.Height; j++)
				{
					Color pixel = bitmap.GetPixel(i, j);
					bool flag = pixel == oldColor;
					if (flag)
					{
						newBitmap.SetPixel(i, j, newColor);
					}
				}
			}
			return newBitmap;
		}
	}
}
