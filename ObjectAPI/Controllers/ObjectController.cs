using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Client;
using ObjectActor.Interfaces;

namespace ObjectAPI.Controllers {
   [Route("api/[controller]")]
   public class ObjectController: Controller {
      [HttpPost("new")]
      public Task<IActionResult> ConstructInstanceAsync() { }

      [HttpPost("destroy")]
      public Task<IActionResult> DestroyInstanceAsync() { }

      [HttpPost("{id}/property")]
      public Task<IActionResult> DefinePropertyAsync() { }

      [HttpDelete("{id}/property")]
      public Task<IActionResult> ErasePropertyAsync() { }

      [HttpGet("{id}/{property}")]
      public Task<IActionResult> GetPropertyAsync() { }

      [HttpPut("{id}/{property}/{value}")]
      public async Task<IActionResult> SetPropertyAsync(string id, string property, string value) {
         var actor = ActorProxy.Create<IObjectActor>(
            new ActorId(id),
            "" // Service Name
         );

         // Are we catching "exceptions" here or are they gracefully handled
         // on the actor?
         await actor.SetPropertyValueAsync(property, value);

         return Ok();
      }

      [HttpPost("{id}/method")]
      public Task<IActionResult> DefineMethodAsync() { }

      [HttpDelete("{id}/method")]
      public Task<IActionResult> EraseMethodAsync() { }

      [HttpGet("{id}/{method}")]
      public Task<IActionResult> ExecuteMethodAsync() { }
   }
}
