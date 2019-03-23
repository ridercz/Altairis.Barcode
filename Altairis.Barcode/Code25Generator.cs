using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Altairis.Barcode {
    public abstract class Code25Generator : Generator {
        protected readonly byte[] CODE_TABLE = new byte[] { 0x06, 0x11, 0x09, 0x18, 0x05, 0x14, 0x0c, 0x03, 0x12, 0x0a };

        private List<bool> bars = new List<bool>();
        private int wideMultiplier = 3;

        public int WideMultiplier {
            get { return wideMultiplier; }
            set {
                if (wideMultiplier < 2) throw new ArgumentOutOfRangeException("value", "WideMultiplier value must be at least 2.");
                wideMultiplier = value;
            }
        }

        protected void AppendElement(bool isBar, bool isWide) {
            if (isWide) {
                for (int i = 0; i < wideMultiplier; i++) this.bars.Add(isBar);
            }
            else {
                this.bars.Add(isBar);
            }
        }

        protected abstract void PopulateBars();

        public override void DrawTo(System.Drawing.Graphics g, System.Drawing.Point position) {
            this.PopulateBars();
            this.DrawFixedWidthBars(bars.ToArray(), g, position);
        }

    }
}
