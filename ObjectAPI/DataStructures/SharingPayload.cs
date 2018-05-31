using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ObjectAPI.DataStructures {
   public class SharingPayload {
      public string TargetProperty { get; set; }
      public double[] Coefficients { get; set; }
      public string[] Neighbors { get; set; }
   }
}
