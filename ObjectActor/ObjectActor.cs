using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;
using Microsoft.ServiceFabric.Actors.Client;
using ObjectActor.Interfaces;

namespace ObjectActor {

   [StatePersistence(StatePersistence.Persisted)]
   internal class ObjectActor: Actor, IObjectActor {
      private const string PropertyPrefix = "P:";
      private const string MethodPrefix = "M:";

      public ObjectActor(ActorService actorService, ActorId actorId)
          : base(actorService, actorId) {
      }

      protected override Task OnActivateAsync() {
         ActorEventSource.Current.ActorMessage(this, "Actor activated.");
         return Task.CompletedTask;
      }

      Task<string> IObjectActor.ConstructInstanceAsync() {
         // NOTE: Should it be here where we program the code that allows the user
         // to create a private object? It could probably involve using an additional
         // key when creating the object.
         throw new NotImplementedException();
      }

      async Task<bool> IObjectActor.DefineMethodAsync(string name, string source, bool overwrite) {
         var reply = true;
         var desiredMethod = await this.StateManager.TryGetStateAsync<string>(MethodPrefix + name);

         if (!desiredMethod.HasValue || overwrite) {
            await this.StateManager.SetStateAsync(MethodPrefix + name, source);
         } else {
            reply = false;
         }

         return reply;
      }

      async Task<bool> IObjectActor.DefinePropertyAsync(string name, PropertyDescriptor propertyDescriptor, bool overwrite) {
         var reply = true;
         var desiredProperty = await this.StateManager.TryGetStateAsync<PropertyDescriptor>(PropertyPrefix + name);   
         
         if (!desiredProperty.HasValue || overwrite) {
            await this.StateManager.SetStateAsync(PropertyPrefix + name, propertyDescriptor);
         } else {
            reply = false;
         }

         return reply;
      }

      Task<bool> IObjectActor.DestroyInstanceAsync() {
         throw new NotImplementedException();
      }

      async Task<bool> IObjectActor.EraseMethodAsync(string name) {
         return await this.StateManager.TryRemoveStateAsync(MethodPrefix + name);
      }

      async Task<bool> IObjectActor.ErasePropertyAsync(string name) {
         return await this.StateManager.TryRemoveStateAsync(PropertyPrefix + name);
      }

      async Task IObjectActor.ExecuteMethodAsync(string name) {
         var desiredMethod = await this.StateManager.TryGetStateAsync<string>(MethodPrefix + name);

         if (desiredMethod.HasValue) {
            // TODO: Do a Restful call to the URI stored.
         } else {
            // Throw?
         }
      }

      async Task<string> IObjectActor.GetPropertyValueAsync(string name) {
         var reply = "";
         var desiredProperty = await this.StateManager.TryGetStateAsync<PropertyDescriptor>(PropertyPrefix + name);

         if (desiredProperty.HasValue) {
            switch (desiredProperty.Value.Source) {
               case SourceType.External:
                  // TODO: Restful call.
                  break;
               case SourceType.Internal:  
               default:
                  reply = desiredProperty.Value.Value;
                  break;
            }
         } else {
            // Throw?
         }

         return reply;
      }

      async Task IObjectActor.SetPropertyValueAsync(string name, string value) {
         var desiredProperty = await this.StateManager.TryGetStateAsync<PropertyDescriptor>(PropertyPrefix + name);

         if (desiredProperty.HasValue) {
            switch (desiredProperty.Value.Source) {
               case SourceType.External:
                  // TODO: Not sure what behavior we want to allow here.
                  break;
               case SourceType.Internal:
               default:
                  desiredProperty.Value.Value = value;
                  break;
            }

            await this.StateManager.SetStateAsync(PropertyPrefix + name, desiredProperty.Value);
         } else {
            // Throw?
         }
      }
   }
}
