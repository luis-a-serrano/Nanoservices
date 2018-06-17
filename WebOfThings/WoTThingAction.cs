using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebOfThings {
   // Source: https://w3c.github.io/wot-scripting-api/#dom-thingaction
   [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), NamingStrategyParameters = new object[] { true, false, false })]
   public class WoTThingAction: WoTSemanticAnnotations {
      public string Name { get; set; }
      public WoTDataSchema InputSchema { get; set; }
      public WoTDataSchema OutputSchema { get; set; }
   }
}
