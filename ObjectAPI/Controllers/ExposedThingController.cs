using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

      // Note: The property details should be captured from the request body.
      [HttpPost("add-property")]
      public Task<IActionResult> AddPropertyAsync(string id) {
         throw new NotImplementedException();
      }

      [HttpDelete("remove-property/{name}")]
      public Task<IActionResult> RemovePropertyAsync(string id, string name) {
         throw new NotImplementedException();
      }

      // Note: The action details should be captured from the request body.
      [HttpPost("add-action")]
      public Task<IActionResult> AddActionAsync(string id) {
         throw new NotImplementedException();
      }

      [HttpDelete("remove-action/{name}")]
      public Task<IActionResult> RemoveActionAsync(string id, string name) {
         throw new NotImplementedException();
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

      // Note: The read handler details should be captured from the request body.
      [HttpPost("set-property-read-handler/{name}")]
      public Task<IActionResult> SetPropertyReadHandlerAsync(string id, string name) {
         throw new NotImplementedException();
      }

      // Note: The write handler details should be captured from the request body.
      [HttpPost("set-property-write-handler/{name}")]
      public Task<IActionResult> SetPropertyWriteHandlerAsync(string id, string name) {
         throw new NotImplementedException();
      }

      // Note: The action handler details should be captured from the request body.
      [HttpPost("set-action-handler/{name}")]
      public Task<IActionResult> SetActionHandlerAsync(string id, string name) {
         throw new NotImplementedException();
      }
   }
}
