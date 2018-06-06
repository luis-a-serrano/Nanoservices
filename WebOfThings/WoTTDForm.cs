using System;
using System.Collections.Generic;
using System.Text;

namespace WebOfThings {
   // TODO: Add the neccessary tags for it to serialize correctly (proper names and casing).
   // Source: https://w3c.github.io/wot-thing-description/#form
   public class WoTTDForm {
      public Uri Href { get; set; }
      public string MediaType { get; set; } = @"application/json";
   }
}
