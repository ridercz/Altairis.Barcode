using System;
using System.Collections.Generic;
using System.Drawing;

namespace Altairis.Barcode {
    public abstract class Code39Generator : Generator {
        private Dictionary<char, int> codeTable;

        protected Code39Generator() {
            this.PopulateCodeTable();
        }

        protected abstract string EncodedContent { get; }

        private void PopulateCodeTable() {
            var codeChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ-. $/+%*".ToCharArray();
            int[] codeValues = { 0x0a6d, 0x0d2b, 0x0b2b, 0x0d95, 0x0a6b, 0x0d35, 0x0b35, 0x0a5b, 0x0d2d, 0x0b2d, 0x0d4b, 0x0b4b, 0x0da5, 0x0acb, 0x0d65, 0x0b65, 0x0a9b, 0x0d4d, 0x0b4d, 0x0acd, 0x0d53, 0x0b53, 0x0da9, 0x0ad3, 0x0d69, 0x0b69, 0x0ab3, 0x0d59, 0x0b59, 0x0ad9, 0x0cab, 0x09ab, 0x0cd5, 0x096b, 0x0cb5, 0x09b5, 0x095b, 0x0cad, 0x09ad, 0x0925, 0x0929, 0x0949, 0x0a49, 0x096d };
            this.codeTable = new Dictionary<char, int>();
            for (var i = 0; i < codeChars.Length; i++) {
                this.codeTable.Add(codeChars[i], codeValues[i]);
            }
        }

        public override Size TotalSize {
            get {
                if (this.Orientation == BarcodeOrientation.Horizontal) {
                    return new Size(this.ModuleSize.Width * (13 * this.EncodedContent.Length), this.ModuleSize.Height);
                }
                else {
                    return new Size(this.ModuleSize.Height, this.ModuleSize.Width * (13 * this.EncodedContent.Length));
                }
            }
        }

        public override void DrawTo(Graphics g, Point position) {
            // Populate bars
            var bars = new List<bool>();
            foreach (var c in this.EncodedContent) {
                var code = this.codeTable[c];
                for (var exp = 11; exp >= 0; exp--) {
                    bars.Add((code & (int)Math.Pow(2, exp)) != 0);
                }
                bars.Add(false);
            }

            // Draw bars
            this.DrawFixedWidthBars(bars.ToArray(), g, position);
        }
    }
}
