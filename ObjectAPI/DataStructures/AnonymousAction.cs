using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ObjectAPI.DataStructures {
   [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), NamingStrategyParameters = new object[] { true, false, false })]
   public class AnonymousAction {
      public Uri Address { get; set; }
      public dynamic Payload { get; set; }
   }
}
