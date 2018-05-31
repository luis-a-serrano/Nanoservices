using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ObjectAPI.DataStructures {
   public class LearningPayload {
      public IterationInfo Iteration { get; set; }
      public double[] OwnCoefficients { get; set; }
      public double[][] NeighborsCoefficients { get; set; }

      public class IterationInfo {
         public double Signal { get; set; }
         public double[] PastValues { get; set; }
      }
   }
}
