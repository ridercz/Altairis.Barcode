using System;
using System.Drawing;
using System.Text.RegularExpressions;

namespace Altairis.Barcode {
    public class Code25InterleavedGenerator : Code25Generator {
        public Code25InterleavedGenerator(string content)
            : base(content.Length % 2 == 1 ? "0" + content : content) { }

        public override Size TotalSize {
            get {
                // Get number of modules
                var pairLength = 6 + 4 * this.WideMultiplier;   // length of pair of chars
                var totalLength =
                    4                                           // start seq
                    + pairLength * (this.content.Length / 2)    // content
                    + 2 + this.WideMultiplier;                  // stop seq

                // Get real size
                return this.Orientation == BarcodeOrientation.Horizontal
                    ? new Size(this.ModuleSize.Width * totalLength, this.ModuleSize.Height)
                    : new Size(this.ModuleSize.Height, this.ModuleSize.Width * totalLength);
            }
        }

        protected override void PopulateBars() {
            // Write start sequence
            this.AppendElement(true, false); this.AppendElement(false, false);  // N
            this.AppendElement(true, false); this.AppendElement(false, false);  // N

            // Write text
            for (var i = 0; i < this.content.Length; i += 2) {
                var barCodeChar = this.CODE_TABLE[int.Parse(this.content.Substring(i, 1))];
                var spcCodeChar = this.CODE_TABLE[int.Parse(this.content.Substring(i + 1, 1))];
                for (var exp = 4; exp >= 0; exp--) {
                    this.AppendElement(true, (barCodeChar & (int)Math.Pow(2, exp)) != 0);
                    this.AppendElement(false, (spcCodeChar & (int)Math.Pow(2, exp)) != 0);
                }
            }

            // Write end sequence
            this.AppendElement(true, true); this.AppendElement(false, false);   // W
            this.AppendElement(true, false);                                    // N
        }

    }
}
