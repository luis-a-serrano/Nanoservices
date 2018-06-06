using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using WebOfThings;

namespace ObjectActor.Interfaces {
   public class TDDataSchema {
      public WoTDataType Type { get; set; }

      // NOTE: I think we should go back with working with JToken directly instead of working with the JObject.
      // Although I need to make sure how they handle valid parsing vs not before checking the contents.
      // I would prefer to use the ExpandoObject class but it would require more coding for exploring the type of
      // data that it holds. Unless we use reflection and check for all the possible types that the serializer can
      // change stuff into.
      // NOTE: JObject provides a way to check the type of it's property but I need to search for a better way to
      // handle properties or for a way to read/get the property name.
      // NOTE: This might not be the best place to make the validation.
      // NOTE: Each subclass (check https://w3c.github.io/wot-thing-description/#dataschema) needs to implement
      // their own version, perhaps. Instead of making this big check in here.
      // NOTE: This assumes that the object was parsed sucessfully.
      public bool Validate(JObject pairs, string current) {
         var valid = true;

         // TODO: Make better checks for invalid data or paths.
         switch (Type) {
            case WoTDataType.Array:
               valid = pairs?[current]?.Type == JTokenType.Array;
               break;
            case WoTDataType.Boolean:
               valid = pairs?[current]?.Type == JTokenType.Boolean;
               break;
            case WoTDataType.Integer:
               valid = pairs?[current]?.Type == JTokenType.Integer;
               break;
            case WoTDataType.Null:
               valid = pairs?[current]?.Type == JTokenType.Null;
               break;
            case WoTDataType.Number:
               valid = pairs?[current]?.Type == JTokenType.Float;
               break;
            case WoTDataType.Object:
               valid = pairs?[current]?.Type == JTokenType.Object;
               if (valid) {
                  foreach (var child in pairs[current].Children()) {
                     // Note: A second Validate method needs to be created that doesn't use the
                     // name of the property to explore as a second argument.
                     valid = valid & Validate(child);
                  }
               }
               break;
            case WoTDataType.String:
               valid = pairs?[current]?.Type == JTokenType.String;
               break;
            case WoTDataType.Unknown:
            default:
               valid = false;
               break;
         }
         return valid;
      }
   }
}
