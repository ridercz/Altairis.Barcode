using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;

namespace Altairis.Barcode {
    public abstract class EanGenerator : Generator {
        protected static readonly byte[,] CODE_TABLES = { { 0x0D, 0x27, 0x72 }, { 0x19, 0x33, 0x66 }, { 0x13, 0x1B, 0x6C }, { 0x3D, 0x21, 0x42 }, { 0x23, 0x1D, 0x5C }, { 0x31, 0x39, 0x4E }, { 0x2F, 0x05, 0x50 }, { 0x3B, 0x11, 0x44 }, { 0x37, 0x09, 0x48 }, { 0x0B, 0x17, 0x74 } };
        private static readonly bool[] START_STOP_BARS = { true, false, true };
        private static readonly bool[] SEPARATOR_BARS = { false, true, false, true, false };

        private List<bool> bars = new List<bool>();

        public override void DrawTo(System.Drawing.Graphics g, System.Drawing.Point position) {
            this.PopulateBars();
            this.DrawFixedWidthBars(this.bars.ToArray(), g, position);
        }

        public override System.Drawing.Size TotalSize {
            get {
                if (this.Orientation == BarcodeOrientation.Horizontal) {
                    return new Size(this.ModuleSize.Width * this.NumberOfBars, this.ModuleSize.Height);
                }
                else {
                    return new Size(this.ModuleSize.Height, this.ModuleSize.Width * this.NumberOfBars);
                }
            }
        }

        protected abstract void PopulateBars();

        protected abstract int NumberOfBars { get; }

        protected void AppendStartStop() {
            this.bars.AddRange(START_STOP_BARS);
        }

        protected void AppendSeparator() {
            this.bars.AddRange(SEPARATOR_BARS);
        }

        protected void AppendDigit(byte digit) {
            for (int i = 0; i < 7; i++) {
                this.bars.Add((digit & (byte)Math.Pow(2, 6 - i)) > 0);
            }

        }

        protected static byte ComputeCheckDigit(string s) {
            if (s == null) throw new ArgumentNullException("s");
            if (string.IsNullOrEmpty(s)) throw new ArgumentException("Value cannot be null or empty string.", "s");
            if (!Regex.IsMatch(s, "^[0-9]{12}$") && !Regex.IsMatch(s, "^[0-9]{7}$")) throw new FormatException("Invalid EAN format -- must be 7 or 12 decimal digits.");

            int sum = 0;
            for (int i = s.Length - 1; i >= 0; i -= 2) sum += Convert.ToByte(s.Substring(i, 1)) * 3;
            for (int i = s.Length - 2; i >= 0; i -= 2) sum += Convert.ToByte(s.Substring(i, 1));

            return (byte)((10 - (sum % 10)) % 10);
        }

    }
}
