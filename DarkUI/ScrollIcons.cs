using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace DarkUI
{
	// Token: 0x02000006 RID: 6
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
	[DebuggerNonUserCode]
	[CompilerGenerated]
	internal class ScrollIcons
	{
		// Token: 0x06000021 RID: 33 RVA: 0x00002050 File Offset: 0x00000250
		internal ScrollIcons()
		{
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000022 RID: 34 RVA: 0x000024FC File Offset: 0x000006FC
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				bool flag = ScrollIcons.resourceMan == null;
				if (flag)
				{
					ResourceManager temp = new ResourceManager("DarkUI.Icons.ScrollIcons", typeof(ScrollIcons).Assembly);
					ScrollIcons.resourceMan = temp;
				}
				return ScrollIcons.resourceMan;
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000023 RID: 35 RVA: 0x00002544 File Offset: 0x00000744
		// (set) Token: 0x06000024 RID: 36 RVA: 0x0000255B File Offset: 0x0000075B
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture
		{
			get
			{
				return ScrollIcons.resourceCulture;
			}
			set
			{
				ScrollIcons.resourceCulture = value;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000025 RID: 37 RVA: 0x00002564 File Offset: 0x00000764
		internal static Bitmap scrollbar_arrow
		{
			get
			{
				object obj = ScrollIcons.ResourceManager.GetObject("scrollbar_arrow", ScrollIcons.resourceCulture);
				return (Bitmap)obj;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000026 RID: 38 RVA: 0x00002594 File Offset: 0x00000794
		internal static Bitmap scrollbar_arrow_clicked
		{
			get
			{
				object obj = ScrollIcons.ResourceManager.GetObject("scrollbar_arrow_clicked", ScrollIcons.resourceCulture);
				return (Bitmap)obj;
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000027 RID: 39 RVA: 0x000025C4 File Offset: 0x000007C4
		internal static Bitmap scrollbar_arrow_disabled
		{
			get
			{
				object obj = ScrollIcons.ResourceManager.GetObject("scrollbar_arrow_disabled", ScrollIcons.resourceCulture);
				return (Bitmap)obj;
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000028 RID: 40 RVA: 0x000025F4 File Offset: 0x000007F4
		internal static Bitmap scrollbar_arrow_hot
		{
			get
			{
				object obj = ScrollIcons.ResourceManager.GetObject("scrollbar_arrow_hot", ScrollIcons.resourceCulture);
				return (Bitmap)obj;
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000029 RID: 41 RVA: 0x00002624 File Offset: 0x00000824
		internal static Bitmap scrollbar_arrow_small_clicked
		{
			get
			{
				object obj = ScrollIcons.ResourceManager.GetObject("scrollbar_arrow_small_clicked", ScrollIcons.resourceCulture);
				return (Bitmap)obj;
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600002A RID: 42 RVA: 0x00002654 File Offset: 0x00000854
		internal static Bitmap scrollbar_arrow_small_hot
		{
			get
			{
				object obj = ScrollIcons.ResourceManager.GetObject("scrollbar_arrow_small_hot", ScrollIcons.resourceCulture);
				return (Bitmap)obj;
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600002B RID: 43 RVA: 0x00002684 File Offset: 0x00000884
		internal static Bitmap scrollbar_arrow_small_standard
		{
			get
			{
				object obj = ScrollIcons.ResourceManager.GetObject("scrollbar_arrow_small_standard", ScrollIcons.resourceCulture);
				return (Bitmap)obj;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600002C RID: 44 RVA: 0x000026B4 File Offset: 0x000008B4
		internal static Bitmap scrollbar_arrow_standard
		{
			get
			{
				object obj = ScrollIcons.ResourceManager.GetObject("scrollbar_arrow_standard", ScrollIcons.resourceCulture);
				return (Bitmap)obj;
			}
		}

		// Token: 0x04000009 RID: 9
		private static ResourceManager resourceMan;

		// Token: 0x0400000A RID: 10
		private static CultureInfo resourceCulture;
	}
}
