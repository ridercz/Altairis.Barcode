using System;
using System.Drawing;

namespace Altairis.Barcode {

    public enum BarcodeOrientation { Horizontal, Vertical }

    public abstract class Generator {

        private string content;
        private Size moduleSize;
        private Color barColor = Color.Black;
        private Color spaceColor = Color.White;
        private BarcodeOrientation orientation = BarcodeOrientation.Horizontal;

        public BarcodeOrientation Orientation {
            get { return orientation; }
            set { orientation = value; }
        }

        public Color SpaceColor {
            get { return spaceColor; }
            set { spaceColor = value; }
        }

        public Color BarColor {
            get { return barColor; }
            set { barColor = value; }
        }

        public string Content {
            get { return content; }
            set {
                if (!ValidateContent(value)) throw new FormatException();
                content = value;
            }
        }

        public Size ModuleSize {
            get { return moduleSize; }
            set { moduleSize = value; }
        }

        public abstract Size TotalSize { get; }

        public abstract bool ValidateContent(string s);

        public abstract void DrawTo(Graphics g, Point position);

        public void DrawTo(Graphics g, int x, int y) {
            this.DrawTo(g, new Point(x, y));
        }

        public void DrawTo(Graphics g) {
            this.DrawTo(g, new Point(0, 0));
        }

        protected internal Size ModuleSizeOriented {
            get {
                if (this.Orientation == BarcodeOrientation.Horizontal) {
                    return this.ModuleSize;
                }
                else {
                    return new Size(this.ModuleSize.Height, this.ModuleSize.Width);
                }
            }
        }

        protected internal void DrawFixedWidthBars(bool[] bars, Graphics g, Point position) {
            System.Collections.Generic.List<Rectangle> blacks = new System.Collections.Generic.List<Rectangle>();
            System.Collections.Generic.List<Rectangle> whites = new System.Collections.Generic.List<Rectangle>();

            for (int i = 0; i < bars.Length; i++) {
                Rectangle r = new Rectangle(position, this.ModuleSizeOriented);
                if (this.Orientation == BarcodeOrientation.Horizontal) {
                    position.Offset(this.ModuleSizeOriented.Width, 0);
                }
                else {
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