using System;
using System.Collections.Generic;
using System.Drawing;

namespace Altairis.Barcode {
    public abstract class Code39Generator : Generator {
        private readonly Dictionary<char, int> codeTable = new Dictionary<char, int> {
            ['0'] = 0x0a6d,
            ['1'] = 0x0d2b,
            ['2'] = 0x0b2b,
            ['3'] = 0x0d95,
            ['4'] = 0x0a6b,
            ['5'] = 0x0d35,
            ['6'] = 0x0b35,
            ['7'] = 0x0a5b,
            ['8'] = 0x0d2d,
            ['9'] = 0x0b2d,
            ['A'] = 0x0d4b,
            ['B'] = 0x0b4b,
            ['C'] = 0x0da5,
            ['D'] = 0x0acb,
            ['E'] = 0x0d65,
            ['F'] = 0x0b65,
            ['G'] = 0x0a9b,
            ['H'] = 0x0d4d,
            ['I'] = 0x0b4d,
            ['J'] = 0x0acd,
            ['K'] = 0x0d53,
            ['L'] = 0x0b53,
            ['M'] = 0x0da9,
            ['N'] = 0x0ad3,
            ['O'] = 0x0d69,
            ['P'] = 0x0b69,
            ['Q'] = 0x0ab3,
            ['R'] = 0x0d59,
            ['S'] = 0x0b59,
            ['T'] = 0x0ad9,
            ['U'] = 0x0cab,
            ['V'] = 0x09ab,
            ['W'] = 0x0cd5,
            ['X'] = 0x096b,
            ['Y'] = 0x0cb5,
            ['Z'] = 0x09b5,
            ['-'] = 0x095b,
            ['.'] = 0x0cad,
            [' '] = 0x09ad,
            ['$'] = 0x0925,
            ['/'] = 0x0929,
            ['+'] = 0x0949,
            ['%'] = 0x0a49,
            ['*'] = 0x096d,
        };

        protected abstract string EncodedContent { get; }

        public override Size TotalSize =>
            this.Orientation == BarcodeOrientation.Horizontal
                ? new Size(this.ModuleSize.Width * (13 * this.EncodedContent.Length), this.ModuleSize.Height)
                : new Size(this.ModuleSize.Height, this.ModuleSize.Width * (13 * this.EncodedContent.Length));

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
