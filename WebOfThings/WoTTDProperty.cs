using System;
using System.Collections.Generic;
using System.Text;

namespace WebOfThings {
   // TODO: Add the neccessary tags for it to serialize correctly (proper names and casing).
   // Source: https://w3c.github.io/wot-thing-description/#property
   public class WoTTDProperty {
      public bool Observable { get; set; } = false;
      public bool Writable { get; set; } = false;
      public WoTDataType Type { get; set; }
      public List<WoTTDForm> Forms { get; set; }
   }
}
