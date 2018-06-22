using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using Newtonsoft.Json;
using ObjectActor.Interfaces;
using ObjectAPI.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebOfThings;

namespace ObjectAPI.Controllers {
   [Route("api/[controller]/{id}")]
   public class ConsumedThingController: Controller {

      [HttpGet("name")]
      [ApiExplorerSettings(IgnoreApi = true)]
      public Task<IActionResult> GetNameAsync(string id) {
         throw new NotImplementedException();
      }

      [HttpGet("td")]
      [ApiExplorerSettings(IgnoreApi = true)]
      public Task<IActionResult> GetThingDescriptionAsync(string id) {
         throw new NotImplementedException();
      }

      /// <summary>
      /// Reads the Property of the Object.
      /// </summary>
      /// <remarks>
      /// Reads the stored value for the Property that matches the name specified on the route.
      /// </remarks>
      /// <response code="200">The stored value was returned sucessfully.</response>
      /// <response code="404">The specified Property doesn't exist.</response>
      /// <response code="410">The value for the specified Property couldn't be read.</response>
      /// <response code="500">Something went wrong with the server code.</response>
      [HttpGet("property/{name}")]
      public async Task<IActionResult> ReadPropertyAsync(string id, string name) {
         var actor = ActorProxy.Create<IObjectActor>(
            new ActorId(id),
            ObjectService.Name.ToServiceUri()
         );

         var potentialError = await actor.ReadPropertyAsync(name);

         switch (potentialError.Type) {
            case WoTReplyType.Error:
               if (potentialError.Result != null && potentialError.Result.Type == WoTDataType.Unknown) {
                  return StatusCode(StatusCodes.Status410Gone, potentialError.Error.Description);
               } else {
                  return StatusCode(StatusCodes.Status404NotFound, potentialError.Error.Description);
               }
            case WoTReplyType.RawResult:
            case WoTReplyType.Result:
               return Ok(potentialError.Result.Value);
            default:
               return StatusCode(StatusCodes.Status500InternalServerError);
         }
      }

      /// <summary>
      /// Writes a value to the Property of the Object.
      /// </summary>
      /// <remarks>
      /// Stores a value for the Property that matches the name specified on the route.
      /// </remarks>
      /// <response code="204">The value was stored sucessfully.</response>
      /// <response code="404">The specified Property doesn't exist.</response>
      /// <response code="410">The value for the specified Property couldn't be writen.</response>
      /// <response code="500">Something went wrong with the server code.</response>
      [HttpPut("property/{name}")]
      public async Task<IActionResult> WritePropertyAsync(string id, string name, [FromBody] object value) {
         var actor = ActorProxy.Create<IObjectActor>(
            new ActorId(id),
            ObjectService.Name.ToServiceUri()
         );

         var potentialError = await actor.WritePropertyAsync(name, JsonConvert.SerializeObject(value));

         switch (potentialError.Type) {
            case WoTReplyType.Error:
               if (potentialError.Result != null && potentialError.Result.Type == WoTDataType.Unknown) {
                  return StatusCode(StatusCodes.Status410Gone, potentialError.Error.Description);
               } else {
                  return StatusCode(StatusCodes.Status404NotFound, potentialError.Error.Description);
               }
            case WoTReplyType.Success:
               return NoContent();
            default:
               return StatusCode(StatusCodes.Status500InternalServerError);
         }
      }

      /// <summary>
      /// *In Development.
      /// </summary>
      /// <remarks>
      /// This method is in development. It might work under very specific circumstances.
      /// </remarks>
      [HttpPost("action/{name}")]
      public async Task<IActionResult> InvokeActionAsync(string id, string name, [FromBody] object parameters) {
         var actor = ActorProxy.Create<IObjectActor>(
            new ActorId(id),
            ObjectService.Name.ToServiceUri()
         );

         var potentialError = await actor.InvokeActionAsync(name, JsonConvert.SerializeObject(parameters));

         // TODO: Send a different action result depending on the presence, and type, of the error.
         return Ok(potentialError);
      }

      /// <summary>
      /// *In Development.
      /// </summary>
      /// <remarks>
      /// This method is in development. It might work under very specific circumstances.
      /// </remarks>
      [HttpPost("action")]
      
      public async Task<IActionResult> InvokeAnonymousActionAsync(string id) {
         var actor = ActorProxy.Create<IObjectActor>(
            new ActorId(id),
            ObjectService.Name.ToServiceUri()
         );

         // TODO: Send a validated object instead of the raw string.
         var potentialError = await actor.InvokeAnonymousActionAsync(await Request.GetBodyAsStringAsync());

         // TODO: Send a different action result depending on the presence, and type, of the error.
         return Ok(potentialError);
      }

      [HttpPost("on-property-change/{name}")]
      [ApiExplorerSettings(IgnoreApi = true)]
      public Task<IActionResult> OnPropertyChangeAsync(string id, string name) {
         throw new NotImplementedException();
      }

      [HttpPost("on-td-change")]
      [ApiExplorerSettings(IgnoreApi = true)]
      public Task<IActionResult> OnTDChangeAsync(string id) {
         throw new NotImplementedException();
      }

      [HttpPost("on-event/{name}")]
      [ApiExplorerSettings(IgnoreApi = true)]
      public Task<IActionResult> OnEventAsync(string id, string name) {
         throw new NotImplementedException();
      }
   }
}
