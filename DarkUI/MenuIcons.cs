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
	// Token: 0x02000004 RID: 4
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
	[DebuggerNonUserCode]
	[CompilerGenerated]
	public class MenuIcons
	{
		// Token: 0x06000014 RID: 20 RVA: 0x00002050 File Offset: 0x00000250
		internal MenuIcons()
		{
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000015 RID: 21 RVA: 0x0000233C File Offset: 0x0000053C
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static ResourceManager ResourceManager
		{
			get
			{
				bool flag = MenuIcons.resourceMan == null;
				if (flag)
				{
					ResourceManager temp = new ResourceManager("DarkUI.Icons.MenuIcons", typeof(MenuIcons).Assembly);
					MenuIcons.resourceMan = temp;
				}
				return MenuIcons.resourceMan;
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000016 RID: 22 RVA: 0x00002384 File Offset: 0x00000584
		// (set) Token: 0x06000017 RID: 23 RVA: 0x0000239B File Offset: 0x0000059B
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static CultureInfo Culture
		{
			get
			{
				return MenuIcons.resourceCulture;
			}
			set
			{
				MenuIcons.resourceCulture = value;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000018 RID: 24 RVA: 0x000023A4 File Offset: 0x000005A4
		public static Bitmap grip
		{
			get
			{
				object obj = MenuIcons.ResourceManager.GetObject("grip", MenuIcons.resourceCulture);
				return (Bitmap)obj;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000019 RID: 25 RVA: 0x000023D4 File Offset: 0x000005D4
		public static Bitmap tick
		{
			get
			{
				object obj = MenuIcons.ResourceManager.GetObject("tick", MenuIcons.resourceCulture);
				return (Bitmap)obj;
			}
		}

		// Token: 0x04000005 RID: 5
		private static ResourceManager resourceMan;

		// Token: 0x04000006 RID: 6
		private static CultureInfo resourceCulture;
	}
}
