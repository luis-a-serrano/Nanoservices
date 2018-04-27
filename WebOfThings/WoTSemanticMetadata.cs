using System;
using System.Collections.Generic;
using System.Text;

namespace WebOfThings {
   // Source: https://w3c.github.io/wot-scripting-api/#dom-semanticmetadata
   public class WoTSemanticMetadata {
      public WoTSemanticType Type { get; set; }
      public dynamic Value { get; set; }
   }
}
