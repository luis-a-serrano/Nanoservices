using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace ObjectAPI {
   public static class ControllerName {

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static string AsControllerName(this string controller) {
         var controllerName = controller?.ToLowerInvariant() ?? String.Empty;
         var controllerSuffix = "Controller";
         return (controllerName.EndsWith(controllerSuffix, true, null) ? controllerName.Remove(controllerName.Length - controllerSuffix.Length) : controllerName);
      }
   }
}
