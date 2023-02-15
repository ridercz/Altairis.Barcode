using System;
using System.Drawing;
using System.Text.RegularExpressions;

namespace Altairis.Barcode {
    public class Code25IndustrialGenerator : Code25Generator {

        public override bool ValidateContent(string s) {
            if (string.IsNullOrEmpty(s)) return false;          // empty data
            if (!Regex.IsMatch(s, "^[0-9]{1,}$")) return false; // code is numeric only
            return true;
        }

        public override Size TotalSize {
            get {
                // Get number of modules
                var charLength = 8 + 2 * this.WideMultiplier;   // length of single char
                var totalLength =
                    4 + 2 * this.WideMultiplier                 // start seq
                    + charLength * this.Content.Length          // content
                    + 3 + 2 * this.WideMultiplier;              // stop seq

                // Get real size
                return this.Orientation == BarcodeOrientation.Horizontal
                    ? new Size(this.ModuleSize.Width * totalLength, this.ModuleSize.Height)
                    : new Size(this.ModuleSize.Height, this.ModuleSize.Width * totalLength);
            }
        }

        protected override void PopulateBars() {
            // Write start sequence
            this.AppendElement(true, true); this.AppendElement(false, false);   // W
            this.AppendElement(true, true); this.AppendElement(false, false);   // W
            this.AppendElement(true, false); this.AppendElement(false, false);  // N

            // Write text
            for (var i = 0; i < this.Content.Length; i++) {
                var codeChar = this.CODE_TABLE[int.Parse(this.Content.Substring(i, 1))];
                for (var exp = 4; exp >= 0; exp--) {
                    this.AppendElement(true, (codeChar & (int)Math.Pow(2, exp)) != 0);
                    this.AppendElement(false, false);
                }
            }

            // Write end sequence
            this.AppendElement(true, true); this.AppendElement(false, false);   // W
            this.AppendElement(true, false); this.AppendElement(false, false);  // N
            this.AppendElement(true, true);                                     // W
        }

    }
}
