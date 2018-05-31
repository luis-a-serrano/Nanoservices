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

   public interface IObjectActor: IActor {
      Task<WoTReply> AddPropertyAsync(WoTThingProperty property);

      Task<WoTReply> SetPropertyReadHandlerAsync(string name, WoTPropertyReadHandler readHandler);
      Task<WoTReply> SetPropertyWriteHandlerAsync(string name, WoTPropertyWriteHandler writeHandler);

      Task<WoTReply> ReadPropertyAsync(string name);
      Task<WoTReply> WritePropertyAsync(string name, dynamic value);

      Task<WoTReply> InvokeAnonymousActionAsync(string rawAction);
   }
}
