﻿using DarkUI.Config;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace DarkUI.Controls
{
    public class DarkComboImageBox : ComboBox
    {
        private ComboCollection<ComboBoxItem> _items;

        /// <summary>
        /// The imaged ComboBox items.
        /// this property is invisibile for design serializer.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new ComboCollection<ComboBoxItem> Items
        {
            get { return _items; }
            set { _items = value; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new Color ForeColor { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new Color BackColor { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new FlatStyle FlatStyle { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new ComboBoxStyle DropDownStyle { get; set; }

        private Bitmap _buffer;

        public DarkComboImageBox() : base()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.UserPaint, true);

            DrawMode = DrawMode.OwnerDrawVariable;

            base.FlatStyle = FlatStyle.Flat;
            base.DropDownStyle = ComboBoxStyle.DropDownList;

            //using DrawItem event we need to draw item
            DrawItem += ComboBoxDrawItemEvent;
            MeasureItem += ComboBox1_MeasureItem;
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _buffer = null;

            base.Dispose(disposing);
        }

        protected override void OnTabStopChanged(EventArgs e)
        {
            base.OnTabStopChanged(e);
            Invalidate();
        }

        protected override void OnTabIndexChanged(EventArgs e)
        {
            base.OnTabIndexChanged(e);
            Invalidate();
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            Invalidate();
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            Invalidate();
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            Invalidate();
        }

        protected override void OnTextUpdate(EventArgs e)
        {
            base.OnTextUpdate(e);
            Invalidate();
        }

        protected override void OnSelectedValueChanged(EventArgs e)
        {
            base.OnSelectedValueChanged(e);
            Invalidate();
        }

        protected override void OnInvalidated(InvalidateEventArgs e)
        {
            base.OnInvalidated(e);
            PaintCombobox();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            _buffer = null;
            Invalidate();
        }

        private void ComboBox1_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            var g = CreateGraphics();
            var maxWidth = 0;
            foreach (var width in Items.ItemsBase.Cast<object>().Select(element => (int)g.MeasureString(element.ToString(), Font).Width).Where(width => width > maxWidth))
            {
                maxWidth = width;
            }
            DropDownWidth = maxWidth + 20;
        }

        protected override ControlCollection CreateControlsInstance()
        {
            _items = new ComboCollection<ComboBoxItem>
            {
                ItemsBase = base.Items
            };

            _items.UpdateItems += UpdateItems;

            return base.CreateControlsInstance();
        }

        private void UpdateItems(object sender, EventArgs e)
        {
        }



        private void PaintCombobox()
        {
            if (_buffer == null)
                _buffer = new Bitmap(ClientRectangle.Width, ClientRectangle.Height);

            using (var g = Graphics.FromImage(_buffer))
            {
                var rect = new Rectangle(0, 0, ClientSize.Width, ClientSize.Height);

                var textColor = Colors.LightText;
                var borderColor = Colors.GreySelection;
                var fillColor = Colors.LightBackground;

                if (Focused && TabStop)
                    borderColor = Colors.BlueHighlight;

                using (var b = new SolidBrush(fillColor))
                {
                    g.FillRectangle(b, rect);
                }

                using (var p = new Pen(borderColor, 1))
                {
                    var modRect = new Rectangle(rect.Left, rect.Top, rect.Width - 1, rect.Height - 1);
                    g.DrawRectangle(p, modRect);
                }

                var icon = ScrollIcons.scrollbar_arrow_hot;
                g.DrawImageUnscaled(icon,
                                    rect.Right - icon.Width - (Consts.Padding / 2),
                                    (rect.Height / 2) - (icon.Height / 2));

                var text = SelectedItem != null ? SelectedItem.ToString() : Text;

                using (var b = new SolidBrush(textColor))
                {
                    var padding = 2;

                    var modRect = new Rectangle(rect.Left + padding,
                                                rect.Top + padding,
                                                rect.Width - icon.Width - (Consts.Padding / 2) - (padding * 2),
                                                rect.Height - (padding * 2));

                    var stringFormat = new StringFormat
                    {
                        LineAlignment = StringAlignment.Center,
                        Alignment = StringAlignment.Near,
                        FormatFlags = StringFormatFlags.NoWrap,
                        Trimming = StringTrimming.EllipsisCharacter
                    };

                    g.DrawString(text, Font, b, modRect, stringFormat);
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (_buffer == null)
                PaintCombobox();

            var g = e.Graphics;
            g.DrawImageUnscaled(_buffer, Point.Empty);
        }
        private void ComboBoxDrawItemEvent(object sender, DrawItemEventArgs e)
        {
            //Draw backgroud of the item
            e.DrawBackground();
            if (e.Index != -1)
            {
                var comboboxItem = Items[e.Index];
                //Draw the image in combo box using its bound and ItemHeight
                e.Graphics.DrawImage(comboboxItem.Image, e.Bounds.X, e.Bounds.Y, ItemHeight, ItemHeight);

                //we need to draw the item as string because we made drawmode to ownervariable
                e.Graphics.DrawString(Items[e.Index].Value.ToString(), Font, Brushes.Black,
                                      new RectangleF(e.Bounds.X + ItemHeight, e.Bounds.Y, DropDownWidth,
                                                     ItemHeight));
            }
            //draw rectangle over the item selected
            e.DrawFocusRectangle();
        }
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            var g = e.Graphics;
            var rect = e.Bounds;

            var textColor = Colors.LightText;
            var fillColor = Colors.LightBackground;

            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected ||
                (e.State & DrawItemState.Focus) == DrawItemState.Focus ||
                (e.State & DrawItemState.NoFocusRect) != DrawItemState.NoFocusRect)
                fillColor = Colors.BlueSelection;

            using (var b = new SolidBrush(fillColor))
            {
                g.FillRectangle(b, rect);
            }

            if (e.Index >= 0 && e.Index < Items.Count)
            {
                var text = Items[e.Index].ToString();

                using (var b = new SolidBrush(textColor))
                {
                    var padding = 2;

                    var modRect = new Rectangle(rect.Left + padding,
                        rect.Top + padding,
                        rect.Width - (padding * 2),
                        rect.Height - (padding * 2));

                    var stringFormat = new StringFormat
                    {
                        LineAlignment = StringAlignment.Center,
                        Alignment = StringAlignment.Near,
                        FormatFlags = StringFormatFlags.NoWrap,
                        Trimming = StringTrimming.EllipsisCharacter
                    };
                    var comboboxItem = Items[e.Index];
                    g.DrawImage(comboboxItem.Image, e.Bounds.X, e.Bounds.Y, ItemHeight, ItemHeight);
                    //g.DrawString(text, Font, b, modRect, stringFormat);
                    g.DrawString(Items[e.Index].Value.ToString(), Font, Brushes.Black,
                                      new RectangleF(e.Bounds.X + ItemHeight, e.Bounds.Y, DropDownWidth,
                                                     ItemHeight));
                }
            }
        }
    }
}
