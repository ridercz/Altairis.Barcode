using System;
using System.Text.RegularExpressions;

namespace Altairis.Barcode {
    public class Ean13Generator : EanGenerator {
        private static readonly byte[] CODE_MAPS = { 0x00, 0x0B, 0x0D, 0x0E, 0x13, 0x19, 0x1C, 0x15, 0x16, 0x1A };

        public Ean13Generator(string content) : base(content) {
            if (!Regex.IsMatch(content, "^[0-9]{12,13}$")) {
                // Invalid characters
                throw new ArgumentException("Content must be 12 or 13 decimal numbers.", nameof(content));
            } else if (this.content.Length == 12) {
                // 12 digits - add checksum
                this.content += Convert.ToString(ComputeCheckDigit(this.content));
            } else if (!ValidateCheckDigit(content)) {
                // 13 digits - validate checksum
                throw new ArgumentException("Content contains invalid check digit.", nameof(content));
            }
        }

        public static bool ValidateCheckDigit(string s) {
            if (s == null) throw new ArgumentNullException(nameof(s));
            if (!Regex.IsMatch(s, "^[0-9]{13}$")) throw new FormatException("Invalid EAN format - must be exactly 13 decimal digits.");

            var checkDigit = ComputeCheckDigit(s.Substring(0, 12));
            return checkDigit == Convert.ToByte(s.Substring(12, 1));
        }

        protected override int NumberOfBars => 95;

        protected override void PopulateBars() {
            // Lead-in
            this.AppendStartStop();

            // Get code map for first six numbers
            var codeMap = CODE_MAPS[Convert.ToByte(this.content.Substring(0, 1))];

            // First six numbers
            for (var i = 1; i <= 6; i++) {
                var codeTableIndex = Math.Sign(codeMap & (byte)Math.Pow(2, 6 - i));
                this.AppendDigit(CODE_TABLES[Convert.ToByte(this.content.Substring(i, 1)), codeTableIndex]);
            }

            // Separator
            this.AppendSeparator();

            // Last six numbers
            for (var i = 7; i <= 12; i++) this.AppendDigit(CODE_TABLES[Convert.ToByte(this.content.Substring(i, 1)), 2]);

            // Lead-out
            this.AppendStartStop();
        }

    }
}
