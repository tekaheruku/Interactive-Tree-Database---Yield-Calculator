using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using TreeYield.WinForms.UI;

namespace TreeYield.WinForms.Controls
{
    /// <summary>Visual style of a <see cref="ChipButton"/>.</summary>
    public enum ChipStyle
    {
        /// <summary>Filter chip: gray, or light blue when <see cref="ChipButton.Selected"/>.</summary>
        Toggle,

        /// <summary>Main action: solid blue with white text.</summary>
        Primary,

        /// <summary>Other actions: white with a gray border.</summary>
        Secondary,

        /// <summary>Destructive action (delete): red text and border.</summary>
        Danger
    }

    /// <summary>
    /// A pill-shaped button used for the filter chips ("All Species", "Fruit-Bearing" ...)
    /// and for action buttons. It inherits from Button, so it keeps keyboard support
    /// (Tab, Enter, Space), AcceptButton/CancelButton support and screen-reader support;
    /// only the painting is custom.
    /// </summary>
    [ToolboxItem(true)]
    [Description("Pill-shaped button for filter chips and actions.")]
    public class ChipButton : Button
    {
        private bool _selected;
        private ChipStyle _chipStyle = ChipStyle.Toggle;
        private bool _hovering;
        private bool _pressed;

        /// <summary>Creates the button.</summary>
        public ChipButton()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint
                | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            UseVisualStyleBackColor = false;
            Cursor = Cursors.Hand;
            Padding = new Padding(18, 0, 18, 0);
            Size = new Size(120, 36);
        }

        /// <summary>For <see cref="ChipStyle.Toggle"/>: whether this chip is the active choice in its group.</summary>
        [Category("Appearance")]
        [DefaultValue(false)]
        [Description("Whether the chip is the active choice in its group.")]
        public bool Selected
        {
            get { return _selected; }
            set
            {
                if (_selected == value)
                {
                    return;
                }

                _selected = value;
                Invalidate();
                // Lets screen readers announce the new checked state.
                AccessibilityNotifyClients(AccessibleEvents.StateChange, -1);
            }
        }

        /// <summary>Which color scheme the button uses.</summary>
        [Category("Appearance")]
        [DefaultValue(ChipStyle.Toggle)]
        [Description("Toggle (filter chip), Primary, Secondary or Danger.")]
        public ChipStyle ChipStyle
        {
            get { return _chipStyle; }
            set
            {
                _chipStyle = value;
                Invalidate();
            }
        }

        /// <summary>Width that fits the text plus padding; used when AutoSize is true.</summary>
        public override Size GetPreferredSize(Size proposedSize)
        {
            Size text = TextRenderer.MeasureText(Text ?? string.Empty, Font);
            return new Size(text.Width + Padding.Horizontal, Math.Max(text.Height + 16, 32));
        }

        /// <inheritdoc />
        protected override void OnMouseEnter(EventArgs e)
        {
            _hovering = true;
            Invalidate();
            base.OnMouseEnter(e);
        }

        /// <inheritdoc />
        protected override void OnMouseLeave(EventArgs e)
        {
            _hovering = false;
            _pressed = false;
            Invalidate();
            base.OnMouseLeave(e);
        }

        /// <inheritdoc />
        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            if (mevent.Button == MouseButtons.Left)
            {
                _pressed = true;
                Invalidate();
            }

            base.OnMouseDown(mevent);
        }

        /// <inheritdoc />
        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            _pressed = false;
            Invalidate();
            base.OnMouseUp(mevent);
        }

        /// <inheritdoc />
        protected override void OnEnabledChanged(EventArgs e)
        {
            Invalidate();
            base.OnEnabledChanged(e);
        }

        /// <summary>Draws the pill, the text and (when focused by keyboard) a focus ring.</summary>
        protected override void OnPaint(PaintEventArgs pevent)
        {
            Graphics g = pevent.Graphics;
            g.Clear(Drawing.GetParentBackColor(this));
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Color back;
            Color fore;
            Color border;
            GetColors(out back, out fore, out border);

            var bounds = new RectangleF(0.5f, 0.5f, Width - 2f, Height - 2f);
            using (GraphicsPath path = Drawing.RoundedRectangle(bounds, bounds.Height / 2f))
            {
                using (var brush = new SolidBrush(back))
                {
                    g.FillPath(brush, path);
                }

                if (!border.IsEmpty)
                {
                    using (var pen = new Pen(border))
                    {
                        g.DrawPath(pen, path);
                    }
                }
            }

            TextFormatFlags flags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter
                | TextFormatFlags.SingleLine | TextFormatFlags.EndEllipsis;
            if (!ShowKeyboardCues)
            {
                flags |= TextFormatFlags.HidePrefix;
            }

            Rectangle textBounds = new Rectangle(Padding.Left / 2, 0, Width - Padding.Horizontal / 2, Height);
            TextRenderer.DrawText(g, Text, Font, textBounds, fore, flags);

            // Visible focus indicator for keyboard users (WCAG 2.4.7).
            if (Focused && ShowFocusCues)
            {
                var ring = new RectangleF(2f, 2f, Width - 5f, Height - 5f);
                using (GraphicsPath path = Drawing.RoundedRectangle(ring, ring.Height / 2f))
                using (var pen = new Pen(Theme.Accent, 2f))
                {
                    g.DrawPath(pen, path);
                }
            }
        }

        /// <summary>Reports "checked" to screen readers when a toggle chip is selected.</summary>
        protected override AccessibleObject CreateAccessibilityInstance()
        {
            return new ChipAccessibleObject(this);
        }

        private void GetColors(out Color back, out Color fore, out Color border)
        {
            border = Color.Empty;

            if (!Enabled)
            {
                back = Color.FromArgb(241, 241, 241);
                fore = Theme.TextDisabled;
                return;
            }

            switch (_chipStyle)
            {
                case ChipStyle.Primary:
                    back = _pressed || _hovering ? Theme.AccentHover : Theme.Accent;
                    fore = Color.White;
                    break;

                case ChipStyle.Secondary:
                    back = _pressed ? Theme.ChipHover : _hovering ? Color.FromArgb(243, 243, 243) : Theme.Surface;
                    fore = Theme.TextPrimary;
                    border = Theme.Border;
                    break;

                case ChipStyle.Danger:
                    back = _pressed || _hovering ? Theme.DangerSoft : Theme.Surface;
                    fore = Theme.Danger;
                    border = Color.FromArgb(240, 180, 176);
                    break;

                default:
                    if (_selected)
                    {
                        back = _hovering ? Color.FromArgb(207, 226, 252) : Theme.AccentSoft;
                        fore = Theme.AccentText;
                    }
                    else
                    {
                        back = _pressed || _hovering ? Theme.ChipHover : Theme.Chip;
                        fore = Theme.TextPrimary;
                    }

                    break;
            }
        }

        private sealed class ChipAccessibleObject : ButtonBaseAccessibleObject
        {
            public ChipAccessibleObject(ChipButton owner)
                : base(owner)
            {
            }

            public override AccessibleStates State
            {
                get
                {
                    var chip = (ChipButton)Owner;
                    AccessibleStates state = base.State;
                    return chip.ChipStyle == ChipStyle.Toggle && chip.Selected ? state | AccessibleStates.Checked : state;
                }
            }
        }
    }
}
