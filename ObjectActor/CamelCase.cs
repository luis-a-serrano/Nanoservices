using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace ObjectActor {
   public static class CamelCase {

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static string ToCamelCase(this string phrase) {
         var result = phrase;
         if (!String.IsNullOrEmpty(phrase)) {
            result = Char.ToLowerInvariant(phrase[0]) + (phrase.Length > 1 ? phrase.Substring(1) : String.Empty);
         }
         return result;
      }
   }
}
