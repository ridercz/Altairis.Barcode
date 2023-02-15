using System;
using System.Collections.Generic;
using System.Drawing;

namespace Altairis.Barcode {
    public abstract class Code25Generator : Generator {
        protected readonly byte[] CODE_TABLE = { 0x06, 0x11, 0x09, 0x18, 0x05, 0x14, 0x0c, 0x03, 0x12, 0x0a };

        private readonly List<bool> bars = new List<bool>();
        private int wideMultiplier = 3;

        public int WideMultiplier {
            get => this.wideMultiplier;
            set {
                if (this.wideMultiplier < 2) throw new ArgumentOutOfRangeException(nameof(value), "WideMultiplier value must be at least 2.");
                this.wideMultiplier = value;
            }
        }

        protected void AppendElement(bool isBar, bool isWide) {
            if (isWide) {
                for (var i = 0; i < this.wideMultiplier; i++) this.bars.Add(isBar);
            } else {
                this.bars.Add(isBar);
            }
        }

        protected abstract void PopulateBars();

        public override void DrawTo(Graphics g, Point position) {
            this.PopulateBars();
            this.DrawFixedWidthBars(this.bars.ToArray(), g, position);
        }

    }
}
