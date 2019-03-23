﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Altairis.Barcode {
    public class Code39StandardGenerator : Code39Generator {

        protected override string EncodedContent {
            get {
                return "*" + this.Content + "*";
            }
        }

        public override bool ValidateContent(string s) {
            if (s == null) return false;
            if (s == string.Empty) return true;
            return Regex.IsMatch(s, @"^[0-9A-Z-. $/+%]{1,}$");
        }

    }
}
