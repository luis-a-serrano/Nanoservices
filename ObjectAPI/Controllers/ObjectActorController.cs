using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ObjectActor.Interfaces;
using System;
using System.Threading.Tasks;
using WebOfThings;

namespace ObjectAPI.Controllers {
   [Route("api/[controller]/{id}")]
   public class ObjectActorController: Controller {

      [HttpPost("add-property")]
      public async Task<IActionResult> AddPropertyAsync([FromRoute] string id, [FromBody] WoTThingProperty property) {
         return BadRequest("Blessed");

         WoTReply potentialError = null;
         try {
            if (!ModelState.IsValid) {
               throw new Exception("Model not valid");
            }
            var actor = ActorProxy.Create<IObjectActor>(
               new ActorId(id),
               null,
               ObjectService.Name,
               null
            );
            potentialError = await actor.AddPropertyAsync(property);
         } catch (Exception exp) {
            potentialError.Type = WoTReplyType.Error;
            potentialError.Result = new WoTResult { Value = exp.Message };
         }

         //return BadRequest(potentialError);
         return BadRequest("All is good.");

         return CreatedAtAction(
            nameof(ReadPropertyAsync),
            new { id, name = property.Name },
            property
         );
      }

      [HttpPut("set-property-read-handler/{propertyName}")]
      public async Task<IActionResult> SetPropertyReadHandlerAsync(string id, string propertyName, WoTPropertyReadHandler readHandler) {
         var actor = ActorProxy.Create<IObjectActor>(
            new ActorId(id),
            null,
            ObjectService.Name,
            null
         );

         var potentialError = await actor.SetPropertyReadHandlerAsync(propertyName, readHandler);

         return NoContent();
      }

      [HttpPut("set-property-write-handler/{propertyName}")]
      public async Task<IActionResult> SetPropertyWriteHandlerAsync(string id, string propertyName, WoTPropertyWriteHandler writeHandler) {
         var actor = ActorProxy.Create<IObjectActor>(
            new ActorId(id),
            null,
            ObjectService.Name,
            null
         );

         var potentialError = await actor.SetPropertyWriteHandlerAsync(propertyName, writeHandler);

         return NoContent();
      }

      [HttpGet("property/{name}")]
      public async Task<IActionResult> ReadPropertyAsync([FromRoute] string id, [FromRoute] string name) {
         var actor = ActorProxy.Create<IObjectActor>(
            new ActorId(id),
            null,
            ObjectService.Name,
            null
         );

         var potentialError = await actor.ReadPropertyAsync(name);

         return Ok(potentialError);
      }

      [HttpPut("property/{name}")]
      public async Task<IActionResult> WritePropertyAsync(string id, string name, dynamic value) {
         var actor = ActorProxy.Create<IObjectActor>(
            new ActorId(id),
            null,
            ObjectService.Name,
            null
         );

         var potentialError = await actor.WritePropertyAsync(name, JsonConvert.SerializeObject(value));

         return NoContent();
      }

      [HttpPost("action")]
      public async Task<IActionResult> InvokeAnonymousActionAsync(string id) {
         WoTReply potentialError = null;
         try {
            var actor = ActorProxy.Create<IObjectActor>(
               new ActorId(id),
               null,
               ObjectService.Name,
               null
            );

            potentialError = await actor.InvokeAnonymousActionAsync(await Request.GetBodyAsStringAsync());
         } catch (Exception exp) {
            return StatusCode(StatusCodes.Status500InternalServerError, exp);
         }
         
         if (potentialError != null && potentialError.Type == WoTReplyType.Error) {
            return StatusCode(StatusCodes.Status500InternalServerError, potentialError);
         }

         return Ok(potentialError);
      }

      public class Fet {
         public string P1 { get; set; }
      }
   }
}
