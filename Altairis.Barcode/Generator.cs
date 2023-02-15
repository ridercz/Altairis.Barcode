using System;
using System.Drawing;

namespace Altairis.Barcode {

    public enum BarcodeOrientation { Horizontal, Vertical }

    public abstract class Generator {

        private string content;

        public BarcodeOrientation Orientation { get; set; } = BarcodeOrientation.Horizontal;

        public Color SpaceColor { get; set; } = Color.White;

        public Color BarColor { get; set; } = Color.Black;

        public string Content {
            get => this.content;
            set {
                if (!this.ValidateContent(value)) throw new FormatException();
                this.content = value;
            }
        }

        public Size ModuleSize { get; set; }

        public abstract Size TotalSize { get; }

        public abstract bool ValidateContent(string s);

        public abstract void DrawTo(Graphics g, Point position);

        public void DrawTo(Graphics g, int x, int y) => this.DrawTo(g, new Point(x, y));

        public void DrawTo(Graphics g) => this.DrawTo(g, new Point(0, 0));

        protected internal Size ModuleSizeOriented {
            get {
                if (this.Orientation == BarcodeOrientation.Horizontal) {
                    return this.ModuleSize;
                } else {
                    return new Size(this.ModuleSize.Height, this.ModuleSize.Width);
                }
            }
        }

        protected internal void DrawFixedWidthBars(bool[] bars, Graphics g, Point position) {
            var blacks = new System.Collections.Generic.List<Rectangle>();
            var whites = new System.Collections.Generic.List<Rectangle>();

            for (var i = 0; i < bars.Length; i++) {
                var r = new Rectangle(position, this.ModuleSizeOriented);
                if (this.Orientation == BarcodeOrientation.Horizontal) {
                    position.Offset(this.ModuleSizeOriented.Width, 0);
                } else {
                    position.Offset(0, this.ModuleSizeOriented.Height);
                }
                if (bars[i]) blacks.Add(r); else whites.Add(r);
            }

            using (Brush barBrush = new SolidBrush(this.BarColor), spaceBrush = new SolidBrush(this.SpaceColor)) {
                g.FillRectangles(barBrush, blacks.ToArray());
                g.FillRectangles(spaceBrush, whites.ToArray());
            }
        }

    }
}