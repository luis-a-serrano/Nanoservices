using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WebOfThings {
   // Source: https://w3c.github.io/wot-scripting-api/#the-exposedthing-interface
   public interface IWoTExposedThing: IWoTConsumedThing {
      Task Start();
      Task Stop();
      Task Register(string directory = null);
      Task Unregister(string directory = null);
      Task EmitEvent(string eventName, dynamic payload);

      Task<WoTError> AddProperty(WoTThingProperty property);
      Task<WoTError> RemoveProperty(string name);
      Task<WoTError> AddAction(WoTThingAction action);
      Task<WoTError> RemoveAction(string name);
      Task<WoTError> AddEvent(WoTThingEvent thingEvent);
      Task<WoTError> RemoveEvent(string name);

      Task<WoTError> SetPropertyReadHandler(string name, WoTPropertyReadHandler readHandler);
      Task<WoTError> SetPropertyWriteHandler(string name, WoTPropertyWriteHandler writeHandler);
      Task<WoTError> SetActionHandler(string name, WoTActionHandler action);
   }
}
