using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace WebOfThings {
   // Note: Error "thrown" by some of the methods
   [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy), NamingStrategyParameters = new object[] { true, false, false })]
   public class WoTError {
      public string Description { get; set; }   

      public WoTError() { }

      public WoTError(string description) {
         Description = description;
      }
   }
}
