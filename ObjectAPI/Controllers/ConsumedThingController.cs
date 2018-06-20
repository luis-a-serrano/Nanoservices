using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using Newtonsoft.Json;
using ObjectActor.Interfaces;
using System;
using System.Threading.Tasks;

namespace ObjectAPI.Controllers {
   [Route("api/[controller]/{id}")]
   public class ConsumedThingController: Controller {

      [HttpGet("name")]
      [ApiExplorerSettings(IgnoreApi = true)]
      public Task<IActionResult> GetNameAsync([FromRoute] string id) {
         throw new NotImplementedException();
      }

      [HttpGet("td")]
      [ApiExplorerSettings(IgnoreApi = true)]
      public Task<IActionResult> GetThingDescriptionAsync([FromRoute] string id) {
         throw new NotImplementedException();
      }

      [HttpGet("property/{name}")]
      public async Task<IActionResult> ReadPropertyAsync([FromRoute] string id, [FromRoute] string name) {
         var actor = ActorProxy.Create<IObjectActor>(
            new ActorId(id),
            ObjectService.Name.ToServiceUri()
         );

         var potentialError = await actor.ReadPropertyAsync(name);

         // TODO: Send a different action result depending on the presence, and type, of the error.
         return Ok(potentialError);
      }

      [HttpPut("property/{name}")]
      public async Task<IActionResult> WritePropertyAsync([FromRoute] string id, [FromRoute] string name, [FromBody] dynamic value) {
         var actor = ActorProxy.Create<IObjectActor>(
            new ActorId(id),
            ObjectService.Name.ToServiceUri()
         );

         var potentialError = await actor.WritePropertyAsync(name, JsonConvert.SerializeObject(value));

         // TODO: Send a different action result depending on the presence, and type, of the error.
         return NoContent();
      }

      [HttpPost("action/{name}")]
      public async Task<IActionResult> InvokeActionAsync([FromRoute] string id, [FromRoute] string name, [FromBody] dynamic parameters) {
         var actor = ActorProxy.Create<IObjectActor>(
            new ActorId(id),
            ObjectService.Name.ToServiceUri()
         );

         var potentialError = await actor.InvokeActionAsync(name, JsonConvert.SerializeObject(parameters));

         // TODO: Send a different action result depending on the presence, and type, of the error.
         return Ok(potentialError);
      }

      // Note: Mostly syntactic sugar
      [HttpPost("action")]
      public async Task<IActionResult> InvokeAnonymousActionAsync([FromRoute] string id) {
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
      public Task<IActionResult> OnPropertyChangeAsync([FromRoute] string id, [FromRoute] string name) {
         throw new NotImplementedException();
      }

      [HttpPost("on-td-change")]
      [ApiExplorerSettings(IgnoreApi = true)]
      public Task<IActionResult> OnTDChangeAsync([FromRoute] string id) {
         throw new NotImplementedException();
      }

      [HttpPost("on-event/{name}")]
      [ApiExplorerSettings(IgnoreApi = true)]
      public Task<IActionResult> OnEventAsync([FromRoute] string id, [FromRoute] string name) {
         throw new NotImplementedException();
      }
   }
}
