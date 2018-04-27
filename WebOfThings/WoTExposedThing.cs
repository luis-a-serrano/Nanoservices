using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WebOfThings {
   // Source: https://w3c.github.io/wot-scripting-api/#the-exposedthing-interface
   public abstract class WoTExposedThing: WoTConsumedThing {
      public abstract Task Start();
      public abstract Task Stop();
      public abstract Task Register(string directory = null);
      public abstract Task Unregister(string directory = null);
      public abstract Task EmitEvent(string eventName, dynamic payload);

      public abstract Task<WoTError> AddProperty(WoTThingProperty property);
      public abstract Task<WoTError> RemoveProperty(string name);
      public abstract Task<WoTError> AddAction(WoTThingAction action);
      public abstract Task<WoTError> RemoveAction(string name);
      public abstract Task<WoTError> AddEvent(WoTThingEvent thingEvent);
      public abstract Task<WoTError> RemoveEvent(string name);

      public abstract Task<WoTError> SetPropertyReadHandler(string name, WoTPropertyReadHandler readHandler);
      public abstract Task<WoTError> SetPropertyWriteHandler(string name, WoTPropertyWriteHandler writeHandler);
      public abstract Task<WoTError> SetActionHandler(string name, WoTActionHandler action);
   }
}
