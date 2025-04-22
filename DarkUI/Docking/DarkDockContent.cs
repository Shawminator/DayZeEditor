using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace DarkUI.Docking
{
	// Token: 0x02000019 RID: 25
	[ToolboxItem(false)]
	public class DarkDockContent : UserControl
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x0600008F RID: 143 RVA: 0x00005770 File Offset: 0x00003970
		// (remove) Token: 0x06000090 RID: 144 RVA: 0x000057A8 File Offset: 0x000039A8
		[field: DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event EventHandler DockTextChanged;

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000091 RID: 145 RVA: 0x000057E0 File Offset: 0x000039E0
		// (set) Token: 0x06000092 RID: 146 RVA: 0x000057F8 File Offset: 0x000039F8
		[Category("Appearance")]
		[Description("Determines the text that will appear in the content tabs and headers.")]
		public string DockText
		{
			get
			{
				return this._dockText;
			}
			set
			{
				string oldText = this._dockText;
				this._dockText = value;
				bool flag = this.DockTextChanged != null;
				if (flag)
				{
					this.DockTextChanged(this, null);
				}
				base.Invalidate();
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000093 RID: 147 RVA: 0x00005838 File Offset: 0x00003A38
		// (set) Token: 0x06000094 RID: 148 RVA: 0x00005850 File Offset: 0x00003A50
		[Category("Appearance")]
		[Description("Determines the icon that will appear in the content tabs and headers.")]
		public Image Icon
		{
			get
			{
				return this._icon;
			}
			set
			{
				this._icon = value;
				base.Invalidate();
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000095 RID: 149 RVA: 0x00005861 File Offset: 0x00003A61
		// (set) Token: 0x06000096 RID: 150 RVA: 0x00005869 File Offset: 0x00003A69
		[Category("Layout")]
		[Description("Determines the default area of the dock panel this content will be added to.")]
		[DefaultValue(DarkDockArea.Document)]
		public DarkDockArea DefaultDockArea { get; set; }

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000097 RID: 151 RVA: 0x00005872 File Offset: 0x00003A72
		// (set) Token: 0x06000098 RID: 152 RVA: 0x0000587A File Offset: 0x00003A7A
		[Category("Behavior")]
		[Description("Determines the key used by this content in the dock serialization.")]
		public string SerializationKey { get; set; }

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000099 RID: 153 RVA: 0x00005883 File Offset: 0x00003A83
		// (set) Token: 0x0600009A RID: 154 RVA: 0x0000588B File Offset: 0x00003A8B
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public DarkDockPanel DockPanel { get; internal set; }

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x0600009B RID: 155 RVA: 0x00005894 File Offset: 0x00003A94
		// (set) Token: 0x0600009C RID: 156 RVA: 0x0000589C File Offset: 0x00003A9C
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public DarkDockRegion DockRegion { get; internal set; }

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x0600009D RID: 157 RVA: 0x000058A5 File Offset: 0x00003AA5
		// (set) Token: 0x0600009E RID: 158 RVA: 0x000058AD File Offset: 0x00003AAD
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public DarkDockGroup DockGroup { get; internal set; }

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x0600009F RID: 159 RVA: 0x000058B6 File Offset: 0x00003AB6
		// (set) Token: 0x060000A0 RID: 160 RVA: 0x000058BE File Offset: 0x00003ABE
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public DarkDockArea DockArea { get; set; }

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060000A1 RID: 161 RVA: 0x000058C7 File Offset: 0x00003AC7
		// (set) Token: 0x060000A2 RID: 162 RVA: 0x000058CF File Offset: 0x00003ACF
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int Order { get; set; }

		// Token: 0x060000A4 RID: 164 RVA: 0x000058E4 File Offset: 0x00003AE4
		public virtual void Close()
		{
			bool flag = this.DockPanel != null;
			if (flag)
			{
				this.DockPanel.RemoveContent(this);
			}
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x0000590C File Offset: 0x00003B0C
		protected override void OnEnter(EventArgs e)
		{
			base.OnEnter(e);
			bool flag = this.DockPanel == null;
			if (!flag)
			{
				this.DockPanel.ActiveContent = this;
			}
		}

		// Token: 0x04000136 RID: 310
		private string _dockText;

		// Token: 0x04000137 RID: 311
		private Image _icon;
	}
}
