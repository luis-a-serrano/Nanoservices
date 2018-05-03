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

      public abstract Task<WoTReply> AddProperty(WoTThingProperty property);
      public abstract Task<WoTReply> RemoveProperty(string name);
      public abstract Task<WoTReply> AddAction(WoTThingAction action);
      public abstract Task<WoTReply> RemoveAction(string name);
      public abstract Task<WoTReply> AddEvent(WoTThingEvent thingEvent);
      public abstract Task<WoTReply> RemoveEvent(string name);

      public abstract Task<WoTReply> SetPropertyReadHandler(string name, WoTPropertyReadHandler readHandler);
      public abstract Task<WoTReply> SetPropertyWriteHandler(string name, WoTPropertyWriteHandler writeHandler);
      public abstract Task<WoTReply> SetActionHandler(string name, WoTActionHandler action);
   }
}
