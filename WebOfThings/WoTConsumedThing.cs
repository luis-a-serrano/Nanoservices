using System;
using System.Threading.Tasks;

namespace WebOfThings {
   // Source: https://w3c.github.io/wot-scripting-api/#dom-consumedthing
   public abstract class WoTConsumedThing {
      public abstract string Name { get; }
      public abstract WoTThingDescription GetThingDescription();

      public abstract Task<dynamic> ReadProperty(string name);
      public abstract Task WriteProperty(string name, dynamic value);
      public abstract Task<dynamic> InvokeAction(string name, dynamic parameters);

      public abstract WoTObservable OnPropertyChange(string name);
      public abstract WoTObservable OnEvent(string name);
      public abstract WoTObservable OnTDChange();
   }
}
