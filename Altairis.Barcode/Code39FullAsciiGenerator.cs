using System;
using System.Text;

namespace Altairis.Barcode {
    public class Code39FullAsciiGenerator : Code39Generator {
        private readonly string[] CODE_TABLE_EXTENDED = { "%U", "$A", "$B", "$C", "$D", "$E", "$F", "$G", "$H", "$I", "$J", "$K", "$L", "$M", "$N", "$O", "$P", "$Q", "$R", "$S", "$T", "$U", "$V", "$W", "$X", "$Y", "$Z", "%A", "%B", "%C", "%D", "%E", " ", "/A", "/B", "/C", "/D", "/E", "/F", "/G", "/H", "/I", "/J", "/K", "/L", "-", ".", "/O", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "/Z", "%F", "%G", "%H", "%I", "%J", "%V", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "%K", "%L", "%M", "%N", "%O", "%W", "+A", "+B", "+C", "+D", "+E", "+F", "+G", "+H", "+I", "+J", "+K", "+L", "+M", "+N", "+O", "+P", "+Q", "+R", "+S", "+T", "+U", "+V", "+W", "+X", "+Y", "+Z", "%P", "%Q", "%R", "%S", "%T" };

        public override bool ValidateContent(string s) {
            if (s == null) return false;
            var ascii = Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(s));
            return s.Equals(ascii, StringComparison.Ordinal);
        }

        protected override string EncodedContent {
            get {
                // Encode content to basic Code39 alphabet
                var sb = new StringBuilder(this.Content.Length);
                sb.Append("*");
                foreach (var b in Encoding.ASCII.GetBytes(this.Content)) {
                    sb.Append(this.CODE_TABLE_EXTENDED[b]);
                }
                sb.Append("*");
                return sb.ToString();
            }
        }

    }
}
