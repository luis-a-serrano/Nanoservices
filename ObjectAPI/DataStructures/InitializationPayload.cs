using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ObjectAPI.DataStructures {
   public class InitializationPayload {
      public CoefficientsInfo Coefficients { get; set; }

      public class CoefficientsInfo {
         public string[] Ids { get; set; }
         public double[] Values { get; set; }
      }

   }
}
