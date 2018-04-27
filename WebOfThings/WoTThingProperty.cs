using System;
using System.Collections.Generic;
using System.Text;

namespace WebOfThings {
   // Source: https://w3c.github.io/wot-scripting-api/#dom-thingproperty
   public class WoTThingProperty: WoTSemanticAnnotations {
      public string Name { get; set; }
      public WoTDataSchema Schema { get; set; }
      public dynamic Value { get; set; }
      public bool Writable { get; set; } = false;
      public bool Observable { get; set; } = false;
   }
}
