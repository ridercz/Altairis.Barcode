using System;
using System.Text.RegularExpressions;

namespace Altairis.Barcode {
    public class Ean13Generator : EanGenerator {
        private static readonly byte[] CODE_MAPS = { 0x00, 0x0B, 0x0D, 0x0E, 0x13, 0x19, 0x1C, 0x15, 0x16, 0x1A };

        public override bool ValidateContent(string s) {
            if (string.IsNullOrEmpty(s)) return false;              // null or empty string
            if (!Regex.IsMatch(s, "^[0-9]{12,13}$")) return false;  // must be 12 or 13 numbers
            if (s.Length == 12) return true;                        // no checksum, all numbers
            return ValidateCheckDigit(s);                           // check checksum
        }

        public static bool ValidateCheckDigit(string s) {
            if (s == null) throw new ArgumentNullException("s");
            if (!System.Text.RegularExpressions.Regex.IsMatch(s, "^[0-9]{13}$")) throw new FormatException("Invalid EAN format -- must be exactly 13 decimal digits.");

            byte checkDigit = ComputeCheckDigit(s.Substring(0, 12));
            return checkDigit == Convert.ToByte(s.Substring(12, 1));
        }

        protected override int NumberOfBars {
            get { return 95; }
        }

        protected override void PopulateBars() {
            // Create bar array
            string s = this.Content;
            if (s.Length == 12) s += Convert.ToString(ComputeCheckDigit(s)); // Add checksum

            // Lead-in
            this.AppendStartStop();

            // Get code map for first six numbers
            byte codeMap = CODE_MAPS[Convert.ToByte(s.Substring(0, 1))];

            // First six numbers
            for (int i = 1; i <= 6; i++) {
                int codeTableIndex = Math.Sign(codeMap & (byte)Math.Pow(2, 6 - i));
                this.AppendDigit(CODE_TABLES[Convert.ToByte(s.Substring(i, 1)), codeTableIndex]);
            }

            // Separator
            this.AppendSeparator();

            // Last six numbers
            for (int i = 7; i <= 12; i++) this.AppendDigit(CODE_TABLES[Convert.ToByte(s.Substring(i, 1)), 2]);

            // Lead-out
            this.AppendStartStop();
        }

    }
}
