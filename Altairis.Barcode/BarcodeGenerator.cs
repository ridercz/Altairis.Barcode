using System;
using System.Drawing;

namespace Altairis.Barcode {

    public enum BarcodeOrientation { Horizontal, Vertical }

    public abstract class BarcodeGenerator {
        protected string content;

        // Constructor

        protected BarcodeGenerator(string content) {
            if (content == null) throw new ArgumentNullException(nameof(content));
            if (string.IsNullOrEmpty(content)) throw new ArgumentException("Content cannot be empty.", nameof(content));
            this.content = content;
        }

        // Public configuration properties

        public BarcodeOrientation Orientation { get; set; } = BarcodeOrientation.Horizontal;

        public Color SpaceColor { get; set; } = Color.White;

        public Color BarColor { get; set; } = Color.Black;

        public Size ModuleSize { get; set; } = new Size(1, 100);
        
        // Public computer properties

        public abstract Size TotalSize { get; }

        // Public methods

        public abstract void DrawTo(Graphics g, Point position);

        public void DrawTo(Graphics g, int x, int y) => this.DrawTo(g, new Point(x, y));

        public void DrawTo(Graphics g) => this.DrawTo(g, new Point(0, 0));

        // Protected helper methods

        protected Size GetModuleSizeOriented() => this.Orientation == BarcodeOrientation.Horizontal ? this.ModuleSize : new Size(this.ModuleSize.Height, this.ModuleSize.Width);

        protected void DrawFixedWidthBars(bool[] bars, Graphics g, Point position) {
            var blacks = new System.Collections.Generic.List<Rectangle>();
            var whites = new System.Collections.Generic.List<Rectangle>();

            for (var i = 0; i < bars.Length; i++) {
                var r = new Rectangle(position, this.GetModuleSizeOriented());
                if (this.Orientation == BarcodeOrientation.Horizontal) {
                    position.Offset(this.GetModuleSizeOriented().Width, 0);
                } else {
                    position.Offset(0, this.GetModuleSizeOriented().Height);
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