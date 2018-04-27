using System;
using System.Threading.Tasks;

namespace WebOfThings {
   // Source: https://w3c.github.io/wot-scripting-api/#dom-consumedthing
   public interface IWoTConsumedThing {
      Task<string> GetName();
      Task<WoTThingDescription> GetThingDescription();

      // Note: I'm not sure how Service Fabric behaves when you return a dynamic object.
      // Thus, this might need to be changed to something more stable.
      Task<dynamic> ReadProperty(string name);
      Task<WoTError> WriteProperty(string name, dynamic value);
      Task<dynamic> InvokeAction(string name, dynamic parameters);

      WoTObservable OnPropertyChange(string name);
      WoTObservable OnTDChange();
      WoTObservable OnEvent(string name);
   }
}
