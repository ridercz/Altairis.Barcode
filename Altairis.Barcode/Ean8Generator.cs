using System;
using System.Text.RegularExpressions;

namespace Altairis.Barcode {
    public class Ean8Generator : EanGenerator {

        public override bool ValidateContent(string s) {
            if (string.IsNullOrEmpty(s)) return false;              // null or empty string
            if (!Regex.IsMatch(s, "^[0-9]{7,8}$")) return false;    // must be 7 or 8 numbers
            if (s.Length == 7) return true;                         // no checksum, all numbers
            return ValidateCheckDigit(s);                           // check checksum
        }

        public static bool ValidateCheckDigit(string s) {
            if (s == null) throw new ArgumentNullException("s");
            if (!System.Text.RegularExpressions.Regex.IsMatch(s, "^[0-9]{8}$")) throw new FormatException("Invalid EAN format -- must be exactly 8 decimal digits.");

            byte checkDigit = ComputeCheckDigit(s.Substring(0, 7));

            if (checkDigit != Convert.ToByte(s.Substring(7, 1))) throw new Exception(string.Format("Check digit is {0}, should be {1}.", s.Substring(7, 1), checkDigit));

            return checkDigit == Convert.ToByte(s.Substring(7, 1));
        }

        protected override int NumberOfBars {
            get { return 67; }
        }

        protected override void PopulateBars() {
            // Create bar array
            string s = this.Content;
            if (s.Length == 7) s += Convert.ToString(ComputeCheckDigit(s)); // Add checksum

            // Lead-in
            this.AppendStartStop();

            // First four numbers
            for (int i = 0; i < 4; i++) this.AppendDigit(CODE_TABLES[Convert.ToByte(s.Substring(i, 1)), 0]);

            // Separator
            this.AppendSeparator();

            // Last three numbers + checksum
            for (int i = 4; i < 8; i++) this.AppendDigit(CODE_TABLES[Convert.ToByte(s.Substring(i, 1)), 2]);

            // Lead-out
            this.AppendStartStop();
        }

    }
}
