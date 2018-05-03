using System;
using System.Collections.Generic;
using System.Text;

namespace WebOfThings {
   public class WoTResult {
      public WoTDataType Type { get; set; }
      public dynamic Value { get; set; }

      public WoTResult() { }

      public WoTResult(WoTDataType type, string value) {
         Type = type;
         Value = value;
      }
   }
}
