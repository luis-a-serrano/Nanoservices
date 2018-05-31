using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ObjectAPI {
   public static class RequestBody {

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static async Task<string> GetBodyAsStringAsync(this HttpRequest request) {
         using (StreamReader reader = new StreamReader(request.Body, Encoding.UTF8)) {
            return await reader.ReadToEndAsync();
         }
      }
   }
}
