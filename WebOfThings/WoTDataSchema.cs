using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebOfThings {
   // Source: https://w3c.github.io/wot-scripting-api/#dom-dataschema
   // Source: https://w3c.github.io/wot-thing-description/#property
   // This seem to include all the fields presented on the second source that do not
   // directly give information of the property, but instead of the value.
   [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), NamingStrategyParameters = new object[] { true, false, false })]
   public class WoTDataSchema {
      public WoTDataType Type { get; set; }
      public WoTDataSchema Items { get; set; }
      public Dictionary<string, WoTDataSchema> Properties { get; set; }
   }

}
