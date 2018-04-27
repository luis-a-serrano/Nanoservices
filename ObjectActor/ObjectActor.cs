using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;
using Microsoft.ServiceFabric.Actors.Client;
using ObjectActor.Interfaces;
using WebOfThings;

namespace ObjectActor {

   [ActorService(Name = ObjectService.Name)]
   [StatePersistence(StatePersistence.Persisted)]
   internal class ObjectActor: Actor, IObjectActor {

      private const string PropertyPrefix = "P:";
      private const string PropertyReadHandlerPrefix = "PRH:";
      private const string PropertyWriteHandlerPrefix = "PWH:";

      private const string ActionPrefix = "A:";
      private const string ActionHandlerPrefix = "AH:";

      public ObjectActor(ActorService actorService, ActorId actorId)
          : base(actorService, actorId) {
      }

      protected override Task OnActivateAsync() {
         ActorEventSource.Current.ActorMessage(this, "Actor activated.");
         return Task.CompletedTask;
      }

      /* IWoTExposedThing */
      public Task Start() {
         throw new NotImplementedException();
      }

      public Task Stop() {
         throw new NotImplementedException();
      }

      public Task Register(string directory = null) {
         throw new NotImplementedException();
      }

      public Task Unregister(string directory = null) {
         throw new NotImplementedException();
      }

      public Task EmitEvent(string eventName, dynamic payload) {
         throw new NotImplementedException();
      }


      public async Task<WoTError> AddProperty(WoTThingProperty property) {
         var desiredProperty = await this.StateManager.TryAddStateAsync(PropertyPrefix + property.Name, property);
         // TODO: Logic to construct the error if needed.
         return null;
      }

      public async Task<WoTError> RemoveProperty(string name) {
         var desiredProperty = await this.StateManager.TryRemoveStateAsync(PropertyPrefix + name);
         // TODO: Logic to construct the error if needed.
         return null;
      }

      public async Task<WoTError> AddAction(WoTThingAction action) {
         var desiredAction = await this.StateManager.TryAddStateAsync(ActionPrefix + action.Name, action);
         // TODO: Logic to construct the error if needed.
         return null;
      }

      public async Task<WoTError> RemoveAction(string name) {
         var desiredAction = await this.StateManager.TryRemoveStateAsync(ActionPrefix + name);
         // TODO: Logic to construct the error if needed.
         return null;
      }

      public Task<WoTError> AddEvent(WoTThingEvent thingEvent) {
         throw new NotImplementedException();
      }

      public Task<WoTError> RemoveEvent(string name) {
         throw new NotImplementedException();
      }

      public async Task<WoTError> SetPropertyReadHandler(string name, WoTPropertyReadHandler readHandler) {
         var desiredPropertyReadHandler = await this.StateManager.TryAddStateAsync(PropertyReadHandlerPrefix + name, readHandler);
         // TODO: Logic to construct the error if needed.
         return null;
      }

      public async Task<WoTError> SetPropertyWriteHandler(string name, WoTPropertyWriteHandler writeHandler) {
         var desiredPropertyWriteHandler = await this.StateManager.TryAddStateAsync(PropertyWriteHandlerPrefix + name, writeHandler);
         // TODO: Logic to construct the error if needed.
         return null;
      }

      public async Task<WoTError> SetActionHandler(string name, WoTActionHandler action) {
         var desiredActionHandler = await this.StateManager.TryAddStateAsync(ActionHandlerPrefix + name, action);
         // TODO: Logic to construct the error if needed.
         return null;
      }

      /* IWoTConsumedThing */
      public async Task<string> GetName() {
         // TODO: Return the proper name.
         return null;
      }

      public Task<WoTThingDescription> GetThingDescription() {
         // TODO: We have two options, either we dynamically create the TD at this moment by querying all the
         // properties, actions, etc. and return it. Or we return an already created TD which is updated each time
         // we update something that affects it.
         // Addendum: Due to "OnTDChange" we probably choose the second option.
         throw new NotImplementedException();
      }

      public async Task<dynamic> ReadProperty(string name) {
         dynamic result = null;
         var desiredPropertyReadHandler = await this.StateManager.TryGetStateAsync<WoTPropertyReadHandler>(PropertyReadHandlerPrefix + name);

         if (desiredPropertyReadHandler.HasValue) {
            // TODO: Assuming that the handler is valid then we execute it and send it's result back.
         } else {
            var desiredProperty = await this.StateManager.TryGetStateAsync<WoTThingProperty>(PropertyPrefix + name);
            
            if (desiredProperty.HasValue) {
               result = desiredProperty.Value.Value;
            } else {
               // Construct an error.
            }
         }

         return result;
      }

      public async Task<WoTError> WriteProperty(string name, dynamic value) {
         WoTError result = null;
         var desiredPropertyWriteHandler = await this.StateManager.TryGetStateAsync<WoTPropertyWriteHandler>(PropertyWriteHandlerPrefix + name);

         if (desiredPropertyWriteHandler.HasValue) {
            // TODO: Assuming that the handler is valid then we execute it and send it's result back.
         } else {
            var desiredProperty = await this.StateManager.TryGetStateAsync<WoTThingProperty>(PropertyPrefix + name);

            if (desiredProperty.HasValue) {
               desiredProperty.Value.Value = value;
               await this.StateManager.SetStateAsync(PropertyPrefix + name, desiredProperty.Value);
            } else {
               // Construct an error.
            }
         }

         return result;
      }

      public async Task<dynamic> InvokeAction(string name, dynamic parameters) {
         dynamic result = null;

         var desiredActionHandler = await this.StateManager.TryGetStateAsync<WoTActionHandler>(ActionHandlerPrefix + name);

         if (desiredActionHandler.HasValue) {
            // TODO: Assuming that the handler is valid then we execute it and send it's result back.
         } else {
            // Construct an error.
         }

         return result;
      }

      public WoTObservable OnPropertyChange(string name) {
         // TODO: This will probably require talking to EventGrid to the the subscription part of
         // the observables.
         throw new NotImplementedException();
      }

      public WoTObservable OnTDChange() {
         // TODO: This will probably require talking to EventGrid to the the subscription part of
         // the observables.
         throw new NotImplementedException();
      }

      public WoTObservable OnEvent(string name) {
         // TODO: This will probably require talking to EventGrid to the the subscription part of
         // the observables.
         throw new NotImplementedException();
      }
   }
}
