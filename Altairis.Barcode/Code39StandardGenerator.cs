using System.Text.RegularExpressions;

namespace Altairis.Barcode {
    public class Code39StandardGenerator : Code39Generator {

        protected override string EncodedContent => "*" + this.Content + "*";

        public override bool ValidateContent(string s) => s != null && (s == string.Empty || Regex.IsMatch(s, @"^[0-9A-Z-. $/+%]{1,}$"));

    }
}
