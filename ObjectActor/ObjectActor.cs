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
using System.Net;
using Microsoft.ServiceFabric.Data;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace ObjectActor {

   [ActorService(Name = ObjectService.Name)]
   [StatePersistence(StatePersistence.Persisted)]
   internal class ObjectActor: Actor, IObjectActor {
      private const string PropertyPrefix = "P:";
      private const string PropertyReadHandlerPrefix = "PRH:";
      private const string PropertyWriteHandlerPrefix = "PWH:";

      private const string ActionPrefix = "A:";
      private const string ActionHandlerPrefix = "AH:";

      private const string PropertySubstitutionRegexGroupName = "propertyName";
      private static readonly Regex PropertySubstitutionRegex = new Regex($@"(?<delimeter>[""'])#(?<{PropertySubstitutionRegexGroupName}>\w+)#\k<delimeter>");

      private readonly HttpClient _Client = new HttpClient();

      public ObjectActor(ActorService actorService, ActorId actorId)
          : base(actorService, actorId) {
      }

      protected override Task OnActivateAsync() {
         ActorEventSource.Current.ActorMessage(this, "Actor activated.");
         return Task.CompletedTask;
      }


      public async Task<WoTReply> AddPropertyAsync(WoTThingProperty property) {
         var reply = new WoTReply();
         try {
            var addAttempt = await this.StateManager.TryAddStateAsync(PropertyPrefix + property.Name, property);

            if (!addAttempt) {
               reply.Type = WoTReplyType.Error;
               reply.Error = new WoTError("A property with the same name already exists.");
            }
         } catch (Exception exp) {
            reply.Type = WoTReplyType.Error;
            reply.Result = new WoTResult { Value = exp.Message };
         }
         

         return reply;
      }

      public async Task<WoTReply> SetPropertyReadHandlerAsync(string name, WoTPropertyReadHandler readHandler) {
         var reply = new WoTReply();

         var propertyExists = await this.StateManager.ContainsStateAsync(name);

         if (propertyExists) {
            await this.StateManager.AddOrUpdateStateAsync(
               PropertyReadHandlerPrefix + name, readHandler, (k, v) => readHandler
            );
            reply.Type = WoTReplyType.Success;
         } else {
            reply.Type = WoTReplyType.Error;
            reply.Error = new WoTError("The specified property doesn't exist.");
         }

         return reply;
      }

      public async Task<WoTReply> SetPropertyWriteHandlerAsync(string name, WoTPropertyWriteHandler writeHandler) {
         var reply = new WoTReply();
         
         var propertyExists = await this.StateManager.ContainsStateAsync(name);

         if (propertyExists) {
            await this.StateManager.AddOrUpdateStateAsync(
               PropertyWriteHandlerPrefix + name, writeHandler, (k, v) => writeHandler
            );
            reply.Type = WoTReplyType.Success;
         } else {
            reply.Type = WoTReplyType.Error;
            reply.Error = new WoTError("The specified property doesn't exist.");
         }

         return reply;
      }

      public async Task<WoTReply> ReadPropertyAsync(string name) {
         var reply = new WoTReply();

         var desiredPropertyReadHandler = await this.StateManager.TryGetStateAsync<WoTPropertyReadHandler>(PropertyReadHandlerPrefix + name);
         var desiredProperty = await this.StateManager.TryGetStateAsync<WoTThingProperty>(PropertyPrefix + name);

         if (desiredProperty.HasValue) {
            if (desiredPropertyReadHandler.HasValue) {
               // Note: We should allow the user to provide additional details for the request if these are needed.
               _Client.DefaultRequestHeaders.Accept.Clear();
               _Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

               var response = await _Client.PostAsync(
                  desiredPropertyReadHandler.Value.Address,
                  // Note: Here we are assuming that "desiredProperty.Value.Value" is of type string. A better check
                  // need to be performed, or a different approach should be used.
                  new StringContent(desiredProperty.Value.Value, Encoding.UTF8, "application/json"));

               if (response.IsSuccessStatusCode) {
                  reply.Type = WoTReplyType.RawResult;
                  reply.Result = new WoTResult {
                     Type = desiredProperty.Value.Schema.Type,
                     Value = await response.Content.ReadAsStringAsync()
                  };
               } else {
                  reply.Type = WoTReplyType.Error;
                  reply.Error = new WoTError("The specified property couldn't be read.");
               }

            } else {
               reply.Type = WoTReplyType.Result;
               reply.Result = new WoTResult {
                  Type = desiredProperty.Value.Schema.Type,
                  Value = desiredProperty.Value.Value
               };
            }
         } else {
            reply.Type = WoTReplyType.Error;
            reply.Error = new WoTError("The specified property doesn't exist.");
         }

         return reply;
      }

      public async Task<WoTReply> WritePropertyAsync(string name, dynamic value) {
         var reply = new WoTReply();

         var desiredPropertyWriteHandler = await this.StateManager.TryGetStateAsync<WoTPropertyWriteHandler>(PropertyWriteHandlerPrefix + name);
         var desiredProperty = await this.StateManager.TryGetStateAsync<WoTThingProperty>(PropertyPrefix + name);

         if (desiredProperty.HasValue) {
            if (desiredPropertyWriteHandler.HasValue) {
               // Note: We should allow the user to provide additional details for the request if these are needed.
               _Client.DefaultRequestHeaders.Accept.Clear();
               _Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

               var response = await _Client.PostAsync(
                  desiredPropertyWriteHandler.Value.Address,
                  // Note: Here we are assuming that "desiredProperty.Value.Value" and "value" is of type string.
                  // A better check need to be performed, or a different approach should be used.
                  new StringContent($@"{{old: {desiredProperty.Value.Value}, new: {value}}}", Encoding.UTF8, "application/json")
               );

               if (response.IsSuccessStatusCode) {
                  reply.Type = WoTReplyType.Success;
                  desiredProperty.Value.Value = await response.Content.ReadAsStringAsync();
                  await this.StateManager.SetStateAsync(PropertyPrefix + name, desiredProperty.Value);
               } else {
                  reply.Type = WoTReplyType.Error;
                  reply.Error = new WoTError("The specified property couldn't be written.");
               }

            } else {
               desiredProperty.Value.Value = value;
               await this.StateManager.SetStateAsync(PropertyPrefix + name, desiredProperty.Value);

               reply.Type = WoTReplyType.Success;
            }
         } else {
            reply.Type = WoTReplyType.Error;
            reply.Error = new WoTError("The specified property doesn't exist.");
         }

         return reply;
      }

      public async Task<WoTReply> InvokeAnonymousActionAsync(string rawAction) {
         var reply = new WoTReply();
         try {

            var matches = PropertySubstitutionRegex.Matches(rawAction);
            var cachedProperties = new Dictionary<string, WoTThingProperty>();

            foreach (Match match in matches) {
               var potentialPropertyName = match.Groups[PropertySubstitutionRegexGroupName]?.Value;

               if (!String.IsNullOrWhiteSpace(potentialPropertyName) && !cachedProperties.ContainsKey(potentialPropertyName)) {
                  var desiredProperty = await this.StateManager.TryGetStateAsync<WoTThingProperty>(PropertyPrefix + potentialPropertyName);
                  if (desiredProperty.HasValue) {
                     cachedProperties[potentialPropertyName] = desiredProperty.Value;
                  }
               }
            }

            var injectedAction = PropertySubstitutionRegex.Replace(rawAction, (match) => {
               var potentialPropertyName = match.Groups[PropertySubstitutionRegexGroupName]?.Value ?? String.Empty;

               if (cachedProperties.TryGetValue(potentialPropertyName, out var property)) {
                  return JsonConvert.SerializeObject(property.Value);
               } else {
                  return match.Value;
               }
            });

            reply.Type = WoTReplyType.Error;
            reply.Result = new WoTResult { Value = rawAction };
            return reply;

            var anonymusAction = JObject.Parse(injectedAction);

            var targetUri = anonymusAction?[nameof(AnonymousAction.Address)]?.ToString();
            var injectedPayload = anonymusAction?[nameof(AnonymousAction.Payload)]?.ToString() ?? String.Empty;

            if (!String.IsNullOrWhiteSpace(targetUri)) {
               // Note: We should allow the user to provide additional details for the request if these are needed.
               _Client.DefaultRequestHeaders.Accept.Clear();
               _Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

               var response = await _Client.PostAsync(
                  targetUri,
                  // Note: Here we are assuming that "value" is of type string. A better check need to be performed,
                  // or a different approach should be used.
                  new StringContent(injectedPayload, Encoding.UTF8, "application/json")
               );

               if (response.IsSuccessStatusCode) {
                  var responseContent = JsonConvert.DeserializeObject<ActionResponse>(await response.Content.ReadAsStringAsync());

                  foreach (var updatedProperty in responseContent.Updates) {
                     if (!cachedProperties.TryGetValue(updatedProperty.Key, out var property)) {
                        var desiredProperty = await this.StateManager.TryGetStateAsync<WoTThingProperty>(PropertyPrefix + updatedProperty.Key);

                        if (desiredProperty.HasValue) {
                           property = desiredProperty.Value;
                        } else {
                           continue;
                        }
                     }

                     property.Value = updatedProperty.Value;
                     await this.StateManager.SetStateAsync(PropertyPrefix + updatedProperty.Key, property);
                  }

                  reply.Type = WoTReplyType.Result;
                  reply.Result = new WoTResult {
                     Type = WoTDataType.Unknown,
                     Value = responseContent.Result
                  };

               } else {
                  reply.Type = WoTReplyType.Error;
                  reply.Error = new WoTError("The request to the specified API was not successful.");
               }
            } else {
               reply.Type = WoTReplyType.Error;
               reply.Error = new WoTError("No valid address was specified for the remote API.");
            }

         } catch (Exception exp) {
            reply.Type = WoTReplyType.Error;
            reply.Result = new WoTResult { Value = exp.Message };
         }

         return reply;
      }
   }

}
