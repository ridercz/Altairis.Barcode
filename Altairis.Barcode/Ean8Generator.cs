﻿using System;
using System.Text.RegularExpressions;

namespace Altairis.Barcode {
    public class Ean8Generator : EanGenerator {
        public Ean8Generator(string content) : base(content) {
            if (!Regex.IsMatch(content, "^[0-9]{7,8}$")) {
                // Invalid characters
                throw new ArgumentException("Content must be 7 or 8 decimal numbers.", nameof(content));
            } else if (this.content.Length == 7) {
                // 7 digits - add checksum
                this.content += Convert.ToString(ComputeCheckDigit(this.content));
            } else if (!ValidateCheckDigit(content)) {
                // 8 digits - validate checksum
                throw new ArgumentException("Content contains invalid check digit.", nameof(content));
            }
        }

        public static bool ValidateCheckDigit(string s) {
            if (s == null) throw new ArgumentNullException(nameof(s));
            if (!Regex.IsMatch(s, "^[0-9]{8}$")) throw new FormatException("Invalid EAN format - must be exactly 8 decimal digits.");

            var checkDigit = ComputeCheckDigit(s.Substring(0, 7));

            if (checkDigit != Convert.ToByte(s.Substring(7, 1))) throw new Exception(string.Format("Check digit is {0}, should be {1}.", s.Substring(7, 1), checkDigit));
            return checkDigit == Convert.ToByte(s.Substring(7, 1));
        }

        protected override int NumberOfBars => 67;

        protected override void PopulateBars() {
            // Lead-in
            this.AppendStartStop();

            // First four numbers
            for (var i = 0; i < 4; i++) this.AppendDigit(CODE_TABLES[Convert.ToByte(this.content.Substring(i, 1)), 0]);

            // Separator
            this.AppendSeparator();

            // Last three numbers + checksum
            for (var i = 4; i < 8; i++) this.AppendDigit(CODE_TABLES[Convert.ToByte(this.content.Substring(i, 1)), 2]);

            // Lead-out
            this.AppendStartStop();
        }

    }
}
