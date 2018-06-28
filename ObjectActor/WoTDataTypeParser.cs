using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using WebOfThings;

namespace ObjectActor {
   public static class WoTDataTypeParser {
      public static bool TryParse(string source, WoTDataSchema dataType, out object result) {
         JToken parsedSource;
         try {
            parsedSource = JToken.Parse(source);
            result = parsedSource;

            switch (dataType.Type) {
               case WoTDataType.Boolean:
                  result = parsedSource.Value<bool>();
                  return parsedSource.Type == JTokenType.Boolean;
               case WoTDataType.Integer:
                  result = parsedSource.Value<int>();
                  return parsedSource.Type == JTokenType.Integer;
               case WoTDataType.Number:
                  result = parsedSource.Value<double>();
                  return parsedSource.Type == JTokenType.Float;
               case WoTDataType.String:
                  result = parsedSource.Value<string>();
                  return parsedSource.Type == JTokenType.String;
               case WoTDataType.Null:
                  result = null;
                  return parsedSource.Type == JTokenType.Null;

               case WoTDataType.Array:
                  // Need to deal with nested types to do this.
                  //return parsedSource.Type == JTokenType.Array;
               case WoTDataType.Object:
                  // Need to deal with nested types to do this.
                  //return parsedSource.Type == JTokenType.Object;
               case WoTDataType.Unknown:
               default:
                  return false;
            }
         } catch (Exception) {
            result = null;
            return false;
         }
      }

      public static bool ValidateParse(JToken source, WoTDataSchema schema, out object result) {
         if (schema.Type == WoTDataType.Object && schema.Properties != null) {

            foreach (var child in source.Children()) {
               if (child.Type == JTokenType.Property) {

               }
            }

         }
         
      }

      private static JTokenType ToJtokenEnum(this WoTDataType dataType) {
         switch (dataType) {
            case WoTDataType.Boolean:
               return JTokenType.Boolean;
            case WoTDataType.Integer:
               return JTokenType.Integer;
            case WoTDataType.Number:
               return JTokenType.Float;
            case WoTDataType.String:
               return JTokenType.String;
            case WoTDataType.Array:
               return JTokenType.Array;
            case WoTDataType.Object:
               return JTokenType.Object;
            case WoTDataType.Null:
               return JTokenType.Null;
            case WoTDataType.Unknown:
            default:
               throw new Exception("Data type not understood.");
         }
      }
   }
}
