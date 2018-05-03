using System;
using System.Threading.Tasks;

namespace WebOfThings {
   // Source: https://w3c.github.io/wot-scripting-api/#dom-consumedthing
   public interface IWoTConsumedThing {
      Task<string> GetNameAsync();
      Task<WoTThingDescription> GetThingDescriptionAsync();

      // Note: I'm not sure how Service Fabric behaves when you return a dynamic object.
      // Thus, this might need to be changed to something more stable.
      Task<WoTReply> ReadPropertyAsync(string name);
      Task<WoTReply> WritePropertyAsync(string name, dynamic value);
      Task<WoTReply> InvokeActionAsync(string name, dynamic parameters);

      Task<WoTObservable> OnPropertyChangeAsync(string name);
      Task<WoTObservable> OnTDChangeAsync();
      Task<WoTObservable> OnEventAsync(string name);
   }
}
