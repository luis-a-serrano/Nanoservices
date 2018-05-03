using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace ObjectAPI {
   public static class ServiceUri {

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static Uri ConstructFrom(string serviceName) {
         return new Uri($"{FabricRuntime.GetActivationContext().ApplicationName}/{serviceName}");
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static Uri ToServiceUri(this string serviceName) {
         return new Uri($"{FabricRuntime.GetActivationContext().ApplicationName}/{serviceName}");
      }
   }
}
