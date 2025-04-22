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
	// Token: 0x02000005 RID: 5
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
	[DebuggerNonUserCode]
	[CompilerGenerated]
	internal class MessageBoxIcons
	{
		// Token: 0x0600001A RID: 26 RVA: 0x00002050 File Offset: 0x00000250
		internal MessageBoxIcons()
		{
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600001B RID: 27 RVA: 0x00002404 File Offset: 0x00000604
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				bool flag = MessageBoxIcons.resourceMan == null;
				if (flag)
				{
					ResourceManager temp = new ResourceManager("DarkUI.Icons.MessageBoxIcons", typeof(MessageBoxIcons).Assembly);
					MessageBoxIcons.resourceMan = temp;
				}
				return MessageBoxIcons.resourceMan;
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600001C RID: 28 RVA: 0x0000244C File Offset: 0x0000064C
		// (set) Token: 0x0600001D RID: 29 RVA: 0x00002463 File Offset: 0x00000663
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture
		{
			get
			{
				return MessageBoxIcons.resourceCulture;
			}
			set
			{
				MessageBoxIcons.resourceCulture = value;
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600001E RID: 30 RVA: 0x0000246C File Offset: 0x0000066C
		internal static Bitmap error
		{
			get
			{
				object obj = MessageBoxIcons.ResourceManager.GetObject("error", MessageBoxIcons.resourceCulture);
				return (Bitmap)obj;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600001F RID: 31 RVA: 0x0000249C File Offset: 0x0000069C
		internal static Bitmap info
		{
			get
			{
				object obj = MessageBoxIcons.ResourceManager.GetObject("info", MessageBoxIcons.resourceCulture);
				return (Bitmap)obj;
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000020 RID: 32 RVA: 0x000024CC File Offset: 0x000006CC
		internal static Bitmap warning
		{
			get
			{
				object obj = MessageBoxIcons.ResourceManager.GetObject("warning", MessageBoxIcons.resourceCulture);
				return (Bitmap)obj;
			}
		}

		// Token: 0x04000007 RID: 7
		private static ResourceManager resourceMan;

		// Token: 0x04000008 RID: 8
		private static CultureInfo resourceCulture;
	}
}
