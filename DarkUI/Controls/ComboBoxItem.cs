using System;
using System.Drawing;

namespace DarkUI.Controls
{
	// Token: 0x0200002B RID: 43
	[Serializable]
	public class ComboBoxItem
	{
		// Token: 0x1700006F RID: 111
		// (get) Token: 0x0600016C RID: 364 RVA: 0x00009FB0 File Offset: 0x000081B0
		// (set) Token: 0x0600016D RID: 365 RVA: 0x00009FC8 File Offset: 0x000081C8
		public object Value
		{
			get
			{
				return this._value;
			}
			set
			{
				this._value = value;
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x0600016E RID: 366 RVA: 0x00009FD4 File Offset: 0x000081D4
		// (set) Token: 0x0600016F RID: 367 RVA: 0x00009FEC File Offset: 0x000081EC
		public Image Image
		{
			get
			{
				return this._image;
			}
			set
			{
				this._image = value;
			}
		}

		// Token: 0x06000170 RID: 368 RVA: 0x00009FF6 File Offset: 0x000081F6
		public ComboBoxItem()
		{
			this._value = string.Empty;
			this._image = new Bitmap(1, 1);
		}

		// Token: 0x06000171 RID: 369 RVA: 0x0000A018 File Offset: 0x00008218
		public ComboBoxItem(object value)
		{
			this._value = value;
			this._image = new Bitmap(1, 1);
		}

		// Token: 0x06000172 RID: 370 RVA: 0x0000A036 File Offset: 0x00008236
		public ComboBoxItem(object value, Image image)
		{
			this._value = value;
			this._image = image;
		}

		// Token: 0x06000173 RID: 371 RVA: 0x0000A050 File Offset: 0x00008250
		public override string ToString()
		{
			return this._value.ToString();
		}

		// Token: 0x04000190 RID: 400
		private object _value;

		// Token: 0x04000191 RID: 401
		private Image _image;
	}
}
