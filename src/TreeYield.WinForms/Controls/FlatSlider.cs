using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Windows.Forms;
using TreeYield.WinForms.UI;

namespace TreeYield.WinForms.Controls
{
    /// <summary>
    /// A flat slider in the style of the design mock-up: a thick dark track up to the
    /// thumb, a thin gray rail after it, and a narrow vertical thumb. Works with the
    /// mouse (click, drag, wheel) and the keyboard (arrows, Page Up/Down, Home/End).
    /// </summary>
    [ToolboxItem(true)]
    [DefaultEvent("ValueChanged")]
    [DefaultProperty("Value")]
    [Description("Flat slider for choosing a whole number.")]
    public class FlatSlider : Control
    {
        private const int EdgePadding = 8;

        private int _minimum = 1;
        private int _maximum = 1000;
        private int _value = 25;
        private int _smallChange = 1;
        private int _largeChange = 10;
        private bool _dragging;

        /// <summary>Raised whenever <see cref="Value"/> changes.</summary>
        [Category("Action")]
        [Description("Occurs when the value changes.")]
        public event EventHandler ValueChanged;

        /// <summary>Creates the slider.</summary>
        public FlatSlider()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer
                | ControlStyles.ResizeRedraw | ControlStyles.Selectable | ControlStyles.StandardClick, true);
            TabStop = true;
            Cursor = Cursors.Hand;
            Size = new Size(240, 32);
            AccessibleRole = AccessibleRole.Slider;
        }

        /// <summary>Lowest value.</summary>
        [Category("Behavior")]
        [DefaultValue(1)]
        public int Minimum
        {
            get { return _minimum; }
            set
            {
                _minimum = value;
                if (_maximum < _minimum)
                {
                    _maximum = _minimum;
                }

                Value = _value;
                Invalidate();
            }
        }

        /// <summary>Highest value.</summary>
        [Category("Behavior")]
        [DefaultValue(1000)]
        public int Maximum
        {
            get { return _maximum; }
            set
            {
                _maximum = value;
                if (_minimum > _maximum)
                {
                    _minimum = _maximum;
                }

                Value = _value;
                Invalidate();
            }
        }

        /// <summary>Current value, always kept between <see cref="Minimum"/> and <see cref="Maximum"/>.</summary>
        [Category("Behavior")]
        [DefaultValue(25)]
        public int Value
        {
            get { return _value; }
            set
            {
                int clamped = Math.Max(_minimum, Math.Min(_maximum, value));
                if (clamped == _value)
                {
                    return;
                }

                _value = clamped;
                Invalidate();
                AccessibilityNotifyClients(AccessibleEvents.ValueChange, -1);
                OnValueChanged(EventArgs.Empty);
            }
        }

        /// <summary>Step for arrow keys and the mouse wheel.</summary>
        [Category("Behavior")]
        [DefaultValue(1)]
        public int SmallChange
        {
            get { return _smallChange; }
            set { _smallChange = Math.Max(1, value); }
        }

        /// <summary>Step for Page Up / Page Down.</summary>
        [Category("Behavior")]
        [DefaultValue(10)]
        public int LargeChange
        {
            get { return _largeChange; }
            set { _largeChange = Math.Max(1, value); }
        }

        /// <summary>Raises <see cref="ValueChanged"/>.</summary>
        protected virtual void OnValueChanged(EventArgs e)
        {
            EventHandler handler = ValueChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>Arrow keys normally move focus between controls; here they move the slider.</summary>
        protected override bool IsInputKey(Keys keyData)
        {
            switch (keyData & Keys.KeyCode)
            {
                case Keys.Left:
                case Keys.Right:
                case Keys.Up:
                case Keys.Down:
                    return true;
                default:
                    return base.IsInputKey(keyData);
            }
        }

        /// <inheritdoc />
        protected override void OnKeyDown(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                case Keys.Down:
                    Value -= _smallChange;
                    break;
                case Keys.Right:
                case Keys.Up:
                    Value += _smallChange;
                    break;
                case Keys.PageDown:
                    Value -= _largeChange;
                    break;
                case Keys.PageUp:
                    Value += _largeChange;
                    break;
                case Keys.Home:
                    Value = _minimum;
                    break;
                case Keys.End:
                    Value = _maximum;
                    break;
                default:
                    base.OnKeyDown(e);
                    return;
            }

            e.Handled = true;
        }

        /// <inheritdoc />
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button != MouseButtons.Left)
            {
                return;
            }

            Focus();
            _dragging = true;
            Capture = true;
            Value = ValueFromX(e.X);
        }

        /// <inheritdoc />
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (_dragging)
            {
                Value = ValueFromX(e.X);
            }
        }

        /// <inheritdoc />
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            _dragging = false;
            Capture = false;
        }

        /// <inheritdoc />
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            Value += Math.Sign(e.Delta) * _smallChange;

            // Stop the wheel from also scrolling the parent.
            var handled = e as HandledMouseEventArgs;
            if (handled != null)
            {
                handled.Handled = true;
            }
        }

        /// <inheritdoc />
        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            Invalidate();
        }

        /// <inheritdoc />
        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            Invalidate();
        }

        /// <inheritdoc />
        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            Invalidate();
        }

        /// <summary>Draws the track, rail, end dot and thumb.</summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(Drawing.GetParentBackColor(this));
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Color trackColor = Enabled ? Theme.SliderTrack : Theme.TextDisabled;
            float left = EdgePadding;
            float right = Width - EdgePadding;
            float centerY = Height / 2f;
            float thumbX = ThumbX();

            using (var rail = new Pen(Theme.SliderRail, 2f))
            {
                rail.StartCap = LineCap.Round;
                rail.EndCap = LineCap.Round;
                g.DrawLine(rail, thumbX, centerY, right, centerY);
            }

            using (var track = new Pen(trackColor, 4f))
            {
                track.StartCap = LineCap.Round;
                track.EndCap = LineCap.Round;
                g.DrawLine(track, left, centerY, thumbX, centerY);
            }

            // Small dot that marks the end of the range.
            using (var dot = new SolidBrush(trackColor))
            {
                g.FillEllipse(dot, right - 2f, centerY - 2f, 4f, 4f);
            }

            float thumbHeight = Math.Min(22f, Height - 6f);
            var thumb = new RectangleF(thumbX - 2.5f, centerY - thumbHeight / 2f, 5f, thumbHeight);

            if (Focused)
            {
                // Soft halo shows which control has keyboard focus.
                var halo = RectangleF.Inflate(thumb, 5f, 3f);
                using (GraphicsPath path = Drawing.RoundedRectangle(halo, 5f))
                using (var brush = new SolidBrush(Theme.AccentSoft))
                {
                    g.FillPath(brush, path);
                }
            }

            using (GraphicsPath path = Drawing.RoundedRectangle(thumb, 2.5f))
            using (var brush = new SolidBrush(trackColor))
            {
                g.FillPath(brush, path);
            }
        }

        /// <summary>Gives screen readers the current value.</summary>
        protected override AccessibleObject CreateAccessibilityInstance()
        {
            return new SliderAccessibleObject(this);
        }

        private float ThumbX()
        {
            float usable = Width - 2 * EdgePadding;
            if (_maximum == _minimum || usable <= 0)
            {
                return EdgePadding;
            }

            return EdgePadding + usable * (_value - _minimum) / (float)(_maximum - _minimum);
        }

        private int ValueFromX(int x)
        {
            float usable = Width - 2 * EdgePadding;
            if (usable <= 0)
            {
                return _minimum;
            }

            float fraction = Math.Max(0f, Math.Min(1f, (x - EdgePadding) / usable));
            return _minimum + (int)Math.Round(fraction * (_maximum - _minimum));
        }

        private sealed class SliderAccessibleObject : ControlAccessibleObject
        {
            public SliderAccessibleObject(FlatSlider owner)
                : base(owner)
            {
            }

            public override string Value
            {
                get { return ((FlatSlider)Owner).Value.ToString(CultureInfo.CurrentCulture); }
                set
                {
                    int parsed;
                    if (int.TryParse(value, NumberStyles.Integer, CultureInfo.CurrentCulture, out parsed))
                    {
                        ((FlatSlider)Owner).Value = parsed;
                    }
                }
            }

            public override AccessibleRole Role
            {
                get { return AccessibleRole.Slider; }
            }
        }
    }
}
