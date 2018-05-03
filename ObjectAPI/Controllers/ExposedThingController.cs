using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Actors.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebOfThings;
using ObjectActor.Interfaces;
using Microsoft.ServiceFabric.Actors;

namespace ObjectAPI.Controllers {
   [Route("api/[controller]/{id}")]
   public class ExposedThingController: Controller {

      [HttpPost("start")]
      public Task<IActionResult> StartAsync(string id) {
         throw new NotImplementedException();
      }

      [HttpPost("stop")]
      public Task<IActionResult> StopAsync(string id) {
         throw new NotImplementedException();
      }

      // Note: The directory information should probably be read from the request body
      // instead.
      [HttpPost("register/{directory}")]
      public Task<IActionResult> RegisterAsync(string id, string directory = null) {
         throw new NotImplementedException();
      }

      // Note: The directory information should probably be read from the request body
      // instead.
      [HttpPost("unregister/{directory}")]
      public Task<IActionResult> UnregisterAsync(string id) {
         throw new NotImplementedException();
      }

      // Note: Passing payload to this one might be too much so it probably should come from
      // the body. However, this requires us to capture the body as a raw data/string and
      // forward it.
      [HttpPost("emit/{name}/{payload}")]
      public Task<IActionResult> EmitEventAsync(string id, string name, dynamic payload) {
         throw new NotImplementedException();
      }

      [HttpPost("add-property")]
      public async Task<IActionResult> AddPropertyAsync(string id, WoTThingProperty property) {
         var actor = ActorProxy.Create<IObjectActor>(
            new ActorId(id),
            ObjectService.Name.ToServiceUri()
         );

         // TODO: Validate the "property" parameter.
         var potentialError = await actor.AddPropertyAsync(property);

         // TODO: Send a different action result depending on the presence, and type, of the error.
         return CreatedAtAction(
            nameof(ConsumedThingController.ReadPropertyAsync),
            nameof(ConsumedThingController),
            new { id, name = property.Name },
            property
         );
      }

      [HttpDelete("remove-property/{name}")]
      public async Task<IActionResult> RemovePropertyAsync(string id, string name) {
         var actor = ActorProxy.Create<IObjectActor>(
            new ActorId(id),
            ObjectService.Name.ToServiceUri()
         );

         var potentialError = await actor.RemovePropertyAsync(name);

         // TODO: Send a different action result depending on the presence, and type, of the error.
         return NoContent();
      }

      [HttpPost("add-action")]
      public async Task<IActionResult> AddActionAsync(string id, WoTThingAction action) {
         var actor = ActorProxy.Create<IObjectActor>(
            new ActorId(id),
            ObjectService.Name.ToServiceUri()
         );

         // TODO: Validate the "action" parameter.
         var potentialError = await actor.AddActionAsync(action);

         // TODO: Send a different action result depending on the presence, and type, of the error.
         return CreatedAtAction(
            nameof(ConsumedThingController.InvokeActionAsync),
            nameof(ConsumedThingController),
            new { id, name = action.Name },
            action
         );
      }

      [HttpDelete("remove-action/{name}")]
      public async Task<IActionResult> RemoveActionAsync(string id, string name) {
         var actor = ActorProxy.Create<IObjectActor>(
            new ActorId(id),
            ObjectService.Name.ToServiceUri()
         );

         var potentialError = await actor.RemoveActionAsync(name);

         // TODO: Send a different action result depending on the presence, and type, of the error.
         return NoContent();
      }

      // Note: The event details should be captured from the request body.
      [HttpPost("add-event")]
      public Task<IActionResult> AddEventAsync(string id) {
         throw new NotImplementedException();
      }

      [HttpDelete("remove-event/{name}")]
      public Task<IActionResult> RemoveEventAsync(string id, string name) {
         throw new NotImplementedException();
      }

      [HttpPut("set-property-read-handler/{propertyName}")]
      public async Task<IActionResult> SetPropertyReadHandlerAsync(string id, string propertyName, WoTPropertyReadHandler readHandler) {
         var actor = ActorProxy.Create<IObjectActor>(
            new ActorId(id),
            ObjectService.Name.ToServiceUri()
         );

         var potentialError = await actor.SetPropertyReadHandlerAsync(propertyName, readHandler);

         // TODO: Send a different action result depending on the presence, and type, of the error.
         return NoContent();
      }

      [HttpPut("set-property-write-handler/{propertyName}")]
      public async Task<IActionResult> SetPropertyWriteHandlerAsync(string id, string propertyName, WoTPropertyWriteHandler writeHandler) {
         var actor = ActorProxy.Create<IObjectActor>(
            new ActorId(id),
            ObjectService.Name.ToServiceUri()
         );

         var potentialError = await actor.SetPropertyWriteHandlerAsync(propertyName, writeHandler);

         // TODO: Send a different action result depending on the presence, and type, of the error.
         return NoContent();
      }

      [HttpPut("set-action-handler/{actionName}")]
      public async Task<IActionResult> SetActionHandlerAsync(string id, string actionName, WoTActionHandler actionHandler) {
         var actor = ActorProxy.Create<IObjectActor>(
            new ActorId(id),
            ObjectService.Name.ToServiceUri()
         );

         var potentialError = await actor.SetActionHandlerAsync(actionName, actionHandler);

         // TODO: Send a different action result depending on the presence, and type, of the error.
         return NoContent();
      }
   }
}
