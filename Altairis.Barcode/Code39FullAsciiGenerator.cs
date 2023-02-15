using System;
using System.Text;

namespace Altairis.Barcode {
    public class Code39FullAsciiGenerator : Code39Generator {
        private readonly string[] CODE_TABLE_EXTENDED = { "%U", "$A", "$B", "$C", "$D", "$E", "$F", "$G", "$H", "$I", "$J", "$K", "$L", "$M", "$N", "$O", "$P", "$Q", "$R", "$S", "$T", "$U", "$V", "$W", "$X", "$Y", "$Z", "%A", "%B", "%C", "%D", "%E", " ", "/A", "/B", "/C", "/D", "/E", "/F", "/G", "/H", "/I", "/J", "/K", "/L", "-", ".", "/O", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "/Z", "%F", "%G", "%H", "%I", "%J", "%V", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "%K", "%L", "%M", "%N", "%O", "%W", "+A", "+B", "+C", "+D", "+E", "+F", "+G", "+H", "+I", "+J", "+K", "+L", "+M", "+N", "+O", "+P", "+Q", "+R", "+S", "+T", "+U", "+V", "+W", "+X", "+Y", "+Z", "%P", "%Q", "%R", "%S", "%T" };

        public Code39FullAsciiGenerator(string content) : base(content) {
            if (!content.Equals(Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(content)), StringComparison.Ordinal)) throw new ArgumentException("Content contains invalid characters.", nameof(content));

            // Encode content to basic Code39 alphabet
            var sb = new StringBuilder(this.content.Length);
            sb.Append("*");
            foreach (var b in Encoding.ASCII.GetBytes(this.content)) {
                sb.Append(this.CODE_TABLE_EXTENDED[b]);
            }
            sb.Append("*");
            this.content = sb.ToString();
        }


    }
}
