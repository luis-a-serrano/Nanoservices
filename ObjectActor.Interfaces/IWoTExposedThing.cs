using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebOfThings;

namespace ObjectActor.Interfaces {
   // Based on the WoTConsumedThing class inside the WebOfThings project.
   public interface IWoTExposedThing: IWoTConsumedThing {
      Task StartAsync();
      Task StopAsync();
      Task RegisterAsync(string directory);
      Task UnregisterAsync(string directory);
      Task EmitEventAsync(string eventName, dynamic payload);

      Task<WoTReply> AddPropertyAsync(WoTThingProperty property);
      Task<WoTReply> RemovePropertyAsync(string name);
      Task<WoTReply> AddActionAsync(WoTThingAction action);
      Task<WoTReply> RemoveActionAsync(string name);
      Task<WoTReply> AddEventAsync(WoTThingEvent thingEvent);
      Task<WoTReply> RemoveEventAsync(string name);

      Task<WoTReply> SetPropertyReadHandlerAsync(string name, WoTPropertyReadHandler readHandler);
      Task<WoTReply> SetPropertyWriteHandlerAsync(string name, WoTPropertyWriteHandler writeHandler);
      Task<WoTReply> SetActionHandlerAsync(string name, WoTActionHandler action);
   }
}
