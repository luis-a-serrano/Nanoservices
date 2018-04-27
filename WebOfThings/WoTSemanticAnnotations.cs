using System;
using System.Collections.Generic;
using System.Text;

namespace WebOfThings {
   // Source: https://w3c.github.io/wot-scripting-api/#dom-semanticannotations
   public class WoTSemanticAnnotations {
      public WoTSemanticType[] SemanticType { get; set; }
      public WoTSemanticMetadata[] Metadata { get; set; }
   }
}
