using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Remoting.FabricTransport;
using Microsoft.ServiceFabric.Services.Remoting;
using WebOfThings;

[assembly: FabricTransportActorRemotingProvider(RemotingListener = RemotingListener.V2Listener, RemotingClient = RemotingClient.V2Client)]
namespace ObjectActor.Interfaces {

   public interface IObjectActor: IActor, IWoTExposedThing {
      //Task<string> ConstructInstanceAsync();
      //Task<bool> DestroyInstanceAsync();

      //Task<bool> DefinePropertyAsync(string name, PropertyDescriptor property, bool overwrite);
      //Task<bool> ErasePropertyAsync(string name);
      //Task<string> GetPropertyValueAsync(string name);
      //Task SetPropertyValueAsync(string name, string value);

      //Task<bool> DefineMethodAsync(string name, string source, bool overwrite);
      //Task<bool> EraseMethodAsync(string name);
      //Task ExecuteMethodAsync(string name);
   }
}
