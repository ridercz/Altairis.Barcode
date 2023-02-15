using System;
using System.Drawing;

namespace Altairis.Barcode {
    public class Code25IataGenerator : Code25Generator {
        public Code25IataGenerator(string content)
            : base(content) { }

        public override Size TotalSize {
            get {
                // Get number of modules
                var charLength = 8 + 2 * this.WideMultiplier;   // length of single char
                var totalLength =
                    4                                           // start seq
                    + charLength * this.content.Length          // content
                    + 3 + this.WideMultiplier;                  // stop seq

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
            for (var i = 0; i < this.content.Length; i++) {
                var codeChar = this.CODE_TABLE[int.Parse(this.content.Substring(i, 1))];
                for (var exp = 4; exp >= 0; exp--) {
                    this.AppendElement(true, (codeChar & (int)Math.Pow(2, exp)) != 0);
                    this.AppendElement(false, false);
                }
            }

            // Write end sequence
            this.AppendElement(true, true); this.AppendElement(false, false);   // W
            this.AppendElement(true, false); this.AppendElement(false, false);  // N
        }

    }
}