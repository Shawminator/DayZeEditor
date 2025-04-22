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
	// Token: 0x02000007 RID: 7
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
	[DebuggerNonUserCode]
	[CompilerGenerated]
	internal class TreeViewIcons
	{
		// Token: 0x0600002D RID: 45 RVA: 0x00002050 File Offset: 0x00000250
		internal TreeViewIcons()
		{
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x0600002E RID: 46 RVA: 0x000026E4 File Offset: 0x000008E4
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				bool flag = TreeViewIcons.resourceMan == null;
				if (flag)
				{
					ResourceManager temp = new ResourceManager("DarkUI.Icons.TreeViewIcons", typeof(TreeViewIcons).Assembly);
					TreeViewIcons.resourceMan = temp;
				}
				return TreeViewIcons.resourceMan;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x0600002F RID: 47 RVA: 0x0000272C File Offset: 0x0000092C
		// (set) Token: 0x06000030 RID: 48 RVA: 0x00002743 File Offset: 0x00000943
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture
		{
			get
			{
				return TreeViewIcons.resourceCulture;
			}
			set
			{
				TreeViewIcons.resourceCulture = value;
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000031 RID: 49 RVA: 0x0000274C File Offset: 0x0000094C
		internal static Bitmap node_closed_empty
		{
			get
			{
				object obj = TreeViewIcons.ResourceManager.GetObject("node_closed_empty", TreeViewIcons.resourceCulture);
				return (Bitmap)obj;
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000032 RID: 50 RVA: 0x0000277C File Offset: 0x0000097C
		internal static Bitmap node_closed_full
		{
			get
			{
				object obj = TreeViewIcons.ResourceManager.GetObject("node_closed_full", TreeViewIcons.resourceCulture);
				return (Bitmap)obj;
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000033 RID: 51 RVA: 0x000027AC File Offset: 0x000009AC
		internal static Bitmap node_open
		{
			get
			{
				object obj = TreeViewIcons.ResourceManager.GetObject("node_open", TreeViewIcons.resourceCulture);
				return (Bitmap)obj;
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000034 RID: 52 RVA: 0x000027DC File Offset: 0x000009DC
		internal static Bitmap node_open_empty
		{
			get
			{
				object obj = TreeViewIcons.ResourceManager.GetObject("node_open_empty", TreeViewIcons.resourceCulture);
				return (Bitmap)obj;
			}
		}

		// Token: 0x0400000B RID: 11
		private static ResourceManager resourceMan;

		// Token: 0x0400000C RID: 12
		private static CultureInfo resourceCulture;
	}
}
