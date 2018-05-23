using Microsoft.ServiceFabric.Actors;
using System;
using System.Threading.Tasks;
using WebOfThings;

namespace ObjectActor.Interfaces {
   // Based on the WoTConsumedThing class inside the WebOfThings project.
   public interface IWoTConsumedThing : IActor {
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
