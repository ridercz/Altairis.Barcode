using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Drawing;

namespace Altairis.Barcode {
    public class Code25InterleavedGenerator : Code25Generator {

        public override bool ValidateContent(string s) {
            if (string.IsNullOrEmpty(s)) return false;          // empty data
            if (!Regex.IsMatch(s, "^[0-9]{1,}$")) return false; // code is numeric only
            return true;
        }

        public new string Content {
            get { return base.Content; }
            set {
                // Append leading 0 to odd-length codes
                if (value.Length % 2 == 1) {
                    base.Content = "0" + value;
                }
                else {
                    base.Content = value;
                }
            }
        }

        public override System.Drawing.Size TotalSize {
            get {
                // Get number of modules
                int pairLength = 6 + 4 * this.WideMultiplier;   // length of pair of chars
                int totalLength =
                    4                                           // start seq
                    + pairLength * (this.Content.Length / 2)    // content
                    + 2 + this.WideMultiplier;                  // stop seq

                // Get real size
                if (this.Orientation == BarcodeOrientation.Horizontal) {
                    return new Size(this.ModuleSize.Width * totalLength, this.ModuleSize.Height);
                }
                else {
                    return new Size(this.ModuleSize.Height, this.ModuleSize.Width * totalLength);
                }
            }
        }

        protected override void PopulateBars() {
            // Write start sequence
            this.AppendElement(true, false); this.AppendElement(false, false);  // N
            this.AppendElement(true, false); this.AppendElement(false, false);  // N

            // Write text
            for (int i = 0; i < this.Content.Length; i += 2) {
                byte barCodeChar = this.CODE_TABLE[int.Parse(this.Content.Substring(i, 1))];
                byte spcCodeChar = this.CODE_TABLE[int.Parse(this.Content.Substring(i + 1, 1))];
                for (int exp = 4; exp >= 0; exp--) {
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
