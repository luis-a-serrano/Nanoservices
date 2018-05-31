using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using Newtonsoft.Json;
using ObjectActor.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebOfThings;

namespace ObjectAPI.Controllers {
   [Route("api/[controller]/{id}")]
   public class ObjectActorController: Controller {

      [HttpPost("add-property")]
      public async Task<IActionResult> AddPropertyAsync(string id, WoTThingProperty property) {
         var actor = ActorProxy.Create<IObjectActor>(
            new ActorId(id),
            ObjectService.Name.ToServiceUri()
         );

         var potentialError = await actor.AddPropertyAsync(property);

         return CreatedAtAction(
            nameof(ObjectActorController.ReadPropertyAsync),
            nameof(ObjectActorController),
            new { id, name = property.Name },
            property
         );
      }

      [HttpPut("set-property-read-handler/{propertyName}")]
      public async Task<IActionResult> SetPropertyReadHandlerAsync(string id, string propertyName, WoTPropertyReadHandler readHandler) {
         var actor = ActorProxy.Create<IObjectActor>(
            new ActorId(id),
            ObjectService.Name.ToServiceUri()
         );

         var potentialError = await actor.SetPropertyReadHandlerAsync(propertyName, readHandler);

         return NoContent();
      }

      [HttpPut("set-property-write-handler/{propertyName}")]
      public async Task<IActionResult> SetPropertyWriteHandlerAsync(string id, string propertyName, WoTPropertyWriteHandler writeHandler) {
         var actor = ActorProxy.Create<IObjectActor>(
            new ActorId(id),
            ObjectService.Name.ToServiceUri()
         );

         var potentialError = await actor.SetPropertyWriteHandlerAsync(propertyName, writeHandler);

         return NoContent();
      }

      [HttpGet("property/{name}")]
      public async Task<IActionResult> ReadPropertyAsync(string id, string name) {
         var actor = ActorProxy.Create<IObjectActor>(
            new ActorId(id),
            ObjectService.Name.ToServiceUri()
         );

         var potentialError = await actor.ReadPropertyAsync(name);

         return Ok(potentialError);
      }

      [HttpPut("property/{name}")]
      public async Task<IActionResult> WritePropertyAsync(string id, string name, dynamic value) {
         var actor = ActorProxy.Create<IObjectActor>(
            new ActorId(id),
            ObjectService.Name.ToServiceUri()
         );

         var potentialError = await actor.WritePropertyAsync(name, JsonConvert.SerializeObject(value));

         return NoContent();
      }

      // Note: Mostly syntactic sugar
      [HttpPost("action")]
      public async Task<IActionResult> InvokeAnonymousActionAsync(string id) {
         var actor = ActorProxy.Create<IObjectActor>(
            new ActorId(id),
            ObjectService.Name.ToServiceUri()
         );

         var potentialError = await actor.InvokeAnonymousActionAsync(await Request.GetBodyAsStringAsync());

         // TODO: Send a different action result depending on the presence, and type, of the error.
         return Ok(potentialError);
      }
   }
}
