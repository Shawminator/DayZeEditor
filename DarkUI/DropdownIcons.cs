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
	// Token: 0x02000003 RID: 3
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
	[DebuggerNonUserCode]
	[CompilerGenerated]
	internal class DropdownIcons
	{
		// Token: 0x0600000F RID: 15 RVA: 0x00002050 File Offset: 0x00000250
		internal DropdownIcons()
		{
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000010 RID: 16 RVA: 0x000022A4 File Offset: 0x000004A4
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				bool flag = DropdownIcons.resourceMan == null;
				if (flag)
				{
					ResourceManager temp = new ResourceManager("DarkUI.Icons.DropdownIcons", typeof(DropdownIcons).Assembly);
					DropdownIcons.resourceMan = temp;
				}
				return DropdownIcons.resourceMan;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000011 RID: 17 RVA: 0x000022EC File Offset: 0x000004EC
		// (set) Token: 0x06000012 RID: 18 RVA: 0x00002303 File Offset: 0x00000503
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture
		{
			get
			{
				return DropdownIcons.resourceCulture;
			}
			set
			{
				DropdownIcons.resourceCulture = value;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000013 RID: 19 RVA: 0x0000230C File Offset: 0x0000050C
		internal static Bitmap small_arrow
		{
			get
			{
				object obj = DropdownIcons.ResourceManager.GetObject("small_arrow", DropdownIcons.resourceCulture);
				return (Bitmap)obj;
			}
		}

		// Token: 0x04000003 RID: 3
		private static ResourceManager resourceMan;

		// Token: 0x04000004 RID: 4
		private static CultureInfo resourceCulture;
	}
}
