using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Actors.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebOfThings;
using ObjectActor.Interfaces;
using Microsoft.ServiceFabric.Actors;
using System.Fabric;
using Microsoft.AspNetCore.Http;

namespace ObjectAPI.Controllers {
   [Route("api/[controller]/{id}")]
   public class ExposedThingController: Controller {

      [HttpPost("start")]
      [ApiExplorerSettings(IgnoreApi = true)]
      public Task<IActionResult> StartAsync(string id) {
         throw new NotImplementedException();
      }

      [HttpPost("stop")]
      [ApiExplorerSettings(IgnoreApi = true)]
      public Task<IActionResult> StopAsync(string id) {
         throw new NotImplementedException();
      }

      // Note: The directory information should probably be read from the request body
      // instead.
      [HttpPost("register/{directory}")]
      [ApiExplorerSettings(IgnoreApi = true)]
      public Task<IActionResult> RegisterAsync(string id, string directory = null) {
         throw new NotImplementedException();
      }

      // Note: The directory information should probably be read from the request body
      // instead.
      [HttpPost("unregister/{directory}")]
      [ApiExplorerSettings(IgnoreApi = true)]
      public Task<IActionResult> UnregisterAsync(string id) {
         throw new NotImplementedException();
      }

      // Note: Passing payload to this one might be too much so it probably should come from
      // the body. However, this requires us to capture the body as a raw data/string and
      // forward it.
      [HttpPost("emit/{name}/{payload}")]
      [ApiExplorerSettings(IgnoreApi = true)]
      public Task<IActionResult> EmitEventAsync(string id, string name, dynamic payload) {
         throw new NotImplementedException();
      }

      /// <summary>
      /// Adds a Property to the Object.
      /// </summary>
      /// <remarks>
      /// Adds a Property that meets all the criteria specified on the provided JSON. Of particular importance is the name
      /// detail as this is the way to access the Property with the ConsumedThing interface.
      /// </remarks>
      /// <response code="201">The Property was created sucessfully and can be found at the specified location.</response>
      /// <response code="409">A Property with that name already exists.</response>
      /// <response code="500">Something went wrong with the server code.</response>
      [HttpPost("add-property")]
      public async Task<IActionResult> AddPropertyAsync(string id, [FromBody] WoTThingProperty property) {
         var actor = ActorProxy.Create<IObjectActor>(
            new ActorId(id),
            ObjectService.Name.ToServiceUri()
         );

         var potentialError = await actor.AddPropertyAsync(property);

         switch (potentialError.Type) {
            case WoTReplyType.Error:
               return StatusCode(StatusCodes.Status409Conflict, potentialError.Error.Description);
            case WoTReplyType.None:
            case WoTReplyType.Success:
               return CreatedAtAction(
                  nameof(ConsumedThingController.ReadPropertyAsync),
                  nameof(ConsumedThingController).AsControllerName(),
                  new { id, name = property.Name },
                  property
               );
            default:
               return StatusCode(StatusCodes.Status500InternalServerError);
         }
      }

      /// <summary>
      /// Removes a Property from the Object.
      /// </summary>
      /// <remarks>
      /// Removes the Property that matches the name provided as a route parameter.
      /// </remarks>
      /// <response code="204">The Property with that name was removed sucessfully.</response>
      /// <response code="410">A Property with that name doesn't exist.</response>
      /// <response code="500">Something went wrong with the server code.</response>
      [HttpDelete("remove-property/{name}")]
      public async Task<IActionResult> RemovePropertyAsync(string id, string name) {
         var actor = ActorProxy.Create<IObjectActor>(
            new ActorId(id),
            ObjectService.Name.ToServiceUri()
         );

         var potentialError = await actor.RemovePropertyAsync(name);

         switch (potentialError.Type) {
            case WoTReplyType.Error:
               return StatusCode(StatusCodes.Status410Gone, potentialError.Error.Description);
            case WoTReplyType.None:
            case WoTReplyType.Success:
               return NoContent();
            default:
               return StatusCode(StatusCodes.Status500InternalServerError);
         }

      }

      /// <summary>
      /// Adds an Action to the Object.
      /// </summary>
      /// <remarks>
      /// Adds an Action that meets all the criteria specified on the provided JSON. Of particular importance is the name
      /// detail as this is the way to access the Action with the ConsumedThing interface.
      /// </remarks>
      /// <response code="201">The Action was created sucessfully and can be found at the specified location.</response>
      /// <response code="409">An Action with that name already exists.</response>
      /// <response code="500">Something went wrong with the server code.</response>
      [HttpPost("add-action")]
      public async Task<IActionResult> AddActionAsync(string id, [FromBody] WoTThingAction action) {
         var actor = ActorProxy.Create<IObjectActor>(
            new ActorId(id),
            ObjectService.Name.ToServiceUri()
         );

         var potentialError = await actor.AddActionAsync(action);

         switch (potentialError.Type) {
            case WoTReplyType.Error:
               return StatusCode(StatusCodes.Status409Conflict, potentialError.Error.Description);
            case WoTReplyType.None:
            case WoTReplyType.Success:
               return CreatedAtAction(
                  nameof(ConsumedThingController.InvokeActionAsync),
                  nameof(ConsumedThingController).AsControllerName(),
                  new { id, name = action.Name },
                  action
               );
            default:
               return StatusCode(StatusCodes.Status500InternalServerError);
         }
      }

      /// <summary>
      /// Removes an Action from the Object.
      /// </summary>
      /// <remarks>
      /// Removes the Action that matches the name provided as a route parameter.
      /// </remarks>
      /// <response code="204">The Action with that name was removed sucessfully.</response>
      /// <response code="410">An Action with that name doesn't exist.</response>
      /// <response code="500">Something went wrong with the server code.</response>
      [HttpDelete("remove-action/{name}")]
      public async Task<IActionResult> RemoveActionAsync(string id, string name) {
         var actor = ActorProxy.Create<IObjectActor>(
            new ActorId(id),
            ObjectService.Name.ToServiceUri()
         );

         var potentialError = await actor.RemoveActionAsync(name);

         switch (potentialError.Type) {
            case WoTReplyType.Error:
               return StatusCode(StatusCodes.Status410Gone, potentialError.Error.Description);
            case WoTReplyType.None:
            case WoTReplyType.Success:
               return NoContent();
            default:
               return StatusCode(StatusCodes.Status500InternalServerError);
         }
      }

      // Note: The event details should be captured from the request body.
      [HttpPost("add-event")]
      [ApiExplorerSettings(IgnoreApi = true)]
      public Task<IActionResult> AddEventAsync(string id) {
         throw new NotImplementedException();
      }

      [HttpDelete("remove-event/{name}")]
      [ApiExplorerSettings(IgnoreApi = true)]
      public Task<IActionResult> RemoveEventAsync(string id, string name) {
         throw new NotImplementedException();
      }

      /// <summary>
      /// *In Development.
      /// </summary>
      /// <remarks>
      /// This method is in development. It might work under very specific circumstances.
      /// </remarks>
      [HttpPut("set-property-read-handler/{propertyName}")]
      public async Task<IActionResult> SetPropertyReadHandlerAsync(string id, string propertyName, [FromBody] WoTPropertyReadHandler readHandler) {
         var actor = ActorProxy.Create<IObjectActor>(
            new ActorId(id),
            ObjectService.Name.ToServiceUri()
         );

         var potentialError = await actor.SetPropertyReadHandlerAsync(propertyName, readHandler);

         // TODO: Send a different action result depending on the presence, and type, of the error.
         return NoContent();
      }

      /// <summary>
      /// *In Development.
      /// </summary>
      /// <remarks>
      /// This method is in development. It might work under very specific circumstances.
      /// </remarks>
      [HttpPut("set-property-write-handler/{propertyName}")]
      public async Task<IActionResult> SetPropertyWriteHandlerAsync(string id, string propertyName, [FromBody] WoTPropertyWriteHandler writeHandler) {
         var actor = ActorProxy.Create<IObjectActor>(
            new ActorId(id),
            ObjectService.Name.ToServiceUri()
         );

         var potentialError = await actor.SetPropertyWriteHandlerAsync(propertyName, writeHandler);

         // TODO: Send a different action result depending on the presence, and type, of the error.
         return NoContent();
      }

      /// <summary>
      /// *In Development.
      /// </summary>
      /// <remarks>
      /// This method is in development. It might work under very specific circumstances.
      /// </remarks>
      [HttpPut("set-action-handler/{actionName}")]
      public async Task<IActionResult> SetActionHandlerAsync(string id, string actionName, [FromBody] WoTActionHandler actionHandler) {
         var actor = ActorProxy.Create<IObjectActor>(
            new ActorId(id),
            ObjectService.Name.ToServiceUri()
         );

         var potentialError = await actor.SetActionHandlerAsync(actionName, actionHandler);

         // TODO: Send a different action result depending on the presence, and type, of the error.
         return Ok(potentialError);
      }
   }
}
