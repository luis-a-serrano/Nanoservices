using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using ObjectActor.Interfaces;
using System;
using System.Threading.Tasks;
using WebOfThings;

namespace ObjectAPI.Controllers {
   [Route("api/[controller]/{id}")]
   public class ExposedThingController: Controller {

      [HttpPost("start")]
      [ApiExplorerSettings(IgnoreApi = true)]
      public Task<IActionResult> StartAsync([FromRoute] string id) {
         throw new NotImplementedException();
      }

      [HttpPost("stop")]
      [ApiExplorerSettings(IgnoreApi = true)]
      public Task<IActionResult> StopAsync([FromRoute] string id) {
         throw new NotImplementedException();
      }

      [HttpPost("register")]
      [ApiExplorerSettings(IgnoreApi = true)]
      public Task<IActionResult> RegisterAsync([FromRoute] string id, [FromBody] string directory) {
         throw new NotImplementedException();
      }

      [HttpPost("unregister")]
      [ApiExplorerSettings(IgnoreApi = true)]
      public Task<IActionResult> UnregisterAsync([FromRoute] string id, [FromBody] string directory) {
         throw new NotImplementedException();
      }

      [HttpPost("emit/{name}")]
      [ApiExplorerSettings(IgnoreApi = true)]
      public Task<IActionResult> EmitEventAsync([FromRoute] string id, [FromRoute] string name, [FromBody] dynamic payload) {
         throw new NotImplementedException();
      }

      [HttpPost("add-property")]
      [Produces("application/json")]
      [ProducesResponseType(typeof(WoTThingProperty), StatusCodes.Status201Created)]
      [ProducesResponseType(typeof(WoTError), StatusCodes.Status400BadRequest)]
      [ProducesResponseType(StatusCodes.Status500InternalServerError)]
      public async Task<IActionResult> AddPropertyAsync([FromRoute] string id, [FromBody] WoTThingProperty property) {
         var actor = ActorProxy.Create<IObjectActor>( new ActorId(id) );
         //var actor = ActorProxy.Create<IObjectActor>(
         //   new ActorId(id),
         //   ObjectService.Name.ToServiceUri()
         //);

         var potentialError = await actor.AddPropertyAsync(property);

         switch (potentialError.Type) {
            case WoTReplyType.Error:
               return BadRequest(potentialError.Error);
            case WoTReplyType.Success:
               return CreatedAtAction(
                  nameof(ConsumedThingController.ReadPropertyAsync),
                  nameof(ConsumedThingController).AsControllerName(),
                  new { id, name = property.Name },
                  property
               );
            default:
               return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected response from the object.");
         }

         // TODO: Send a different action result depending on the presence, and type, of the error.
         //return CreatedAtAction(
         //   nameof(ConsumedThingController.ReadPropertyAsync),
         //   nameof(ConsumedThingController).AsControllerName(),
         //   new { id, name = property.Name },
         //   property
         //);
      }

      [HttpDelete("remove-property/{name}")]
      public async Task<IActionResult> RemovePropertyAsync([FromRoute] string id, [FromRoute] string name) {
         var actor = ActorProxy.Create<IObjectActor>(
            new ActorId(id),
            ObjectService.Name.ToServiceUri()
         );

         var potentialError = await actor.RemovePropertyAsync(name);

         // TODO: Send a different action result depending on the presence, and type, of the error.
         return NoContent();
      }

      [HttpPost("add-action")]
      public async Task<IActionResult> AddActionAsync([FromRoute] string id, [FromBody] WoTThingAction action) {
         var actor = ActorProxy.Create<IObjectActor>(
            new ActorId(id),
            ObjectService.Name.ToServiceUri()
         );

         // TODO: Validate the "action" parameter.
         var potentialError = await actor.AddActionAsync(action);

         // TODO: Send a different action result depending on the presence, and type, of the error.
         return CreatedAtAction(
            nameof(ConsumedThingController.InvokeActionAsync),
            nameof(ConsumedThingController).AsControllerName(),
            new { id, name = action.Name },
            action
         );
      }

      [HttpDelete("remove-action/{name}")]
      public async Task<IActionResult> RemoveActionAsync([FromRoute] string id, [FromRoute] string name) {
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
      [ApiExplorerSettings(IgnoreApi = true)]
      public Task<IActionResult> AddEventAsync([FromRoute] string id) {
         throw new NotImplementedException();
      }

      [HttpDelete("remove-event/{name}")]
      [ApiExplorerSettings(IgnoreApi = true)]
      public Task<IActionResult> RemoveEventAsync([FromRoute] string id, [FromRoute] string name) {
         throw new NotImplementedException();
      }

      [HttpPut("set-property-read-handler/{propertyName}")]
      public async Task<IActionResult> SetPropertyReadHandlerAsync([FromRoute] string id, [FromRoute] string propertyName, [FromBody] WoTPropertyReadHandler readHandler) {
         var actor = ActorProxy.Create<IObjectActor>(
            new ActorId(id),
            ObjectService.Name.ToServiceUri()
         );

         var potentialError = await actor.SetPropertyReadHandlerAsync(propertyName, readHandler);

         // TODO: Send a different action result depending on the presence, and type, of the error.
         return NoContent();
      }

      [HttpPut("set-property-write-handler/{propertyName}")]
      public async Task<IActionResult> SetPropertyWriteHandlerAsync([FromRoute] string id, [FromRoute] string propertyName, [FromBody] WoTPropertyWriteHandler writeHandler) {
         var actor = ActorProxy.Create<IObjectActor>(
            new ActorId(id),
            ObjectService.Name.ToServiceUri()
         );

         var potentialError = await actor.SetPropertyWriteHandlerAsync(propertyName, writeHandler);

         // TODO: Send a different action result depending on the presence, and type, of the error.
         return NoContent();
      }

      [HttpPut("set-action-handler/{actionName}")]
      public async Task<IActionResult> SetActionHandlerAsync([FromRoute] string id, [FromRoute] string actionName, [FromBody] WoTActionHandler actionHandler) {
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
