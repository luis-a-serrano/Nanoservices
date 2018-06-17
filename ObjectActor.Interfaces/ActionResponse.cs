using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace ObjectActor.Interfaces {
   [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), NamingStrategyParameters = new object[] { true, false, false })]
   public class ActionResponse {
      public Dictionary<string, dynamic> Updates { get; set; }
      public dynamic Result { get; set; } = null;
   }
}