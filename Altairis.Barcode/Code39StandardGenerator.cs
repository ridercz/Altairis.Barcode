using System;
using System.Text.RegularExpressions;

namespace Altairis.Barcode {
    public class Code39StandardGenerator : Code39Generator {
        public Code39StandardGenerator(string content) 
            : base("*" + content + "*") {
            if (!Regex.IsMatch(content, @"^[0-9A-Z-. $/+%*]{1,}$")) throw new ArgumentException("Content contains unsupported characters.", nameof(content));
        }

    }
}
