using System;
using System.Collections.Generic;
using System.Text;

namespace WebOfThings {
   public class WoTThingEvent: WoTSemanticAnnotations {
      public string Name { get; set; }
      public WoTDataSchema Schema { get; set; }
   }
}
