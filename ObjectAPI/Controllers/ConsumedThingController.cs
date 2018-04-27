using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ObjectAPI.Controllers {
   [Route("api/[controller]/{id}")]
   public class ConsumedThingController: Controller {

      [HttpGet("name")]
      public Task<IActionResult> GetNameAsync(string id) {
         throw new NotImplementedException();
      }

      [HttpGet("td")]
      public Task<IActionResult> GetThingDescriptionAsync(string id) {
         throw new NotImplementedException();
      }

      [HttpGet("read/{name}")]
      public Task<IActionResult> ReadPropertyAsync(string id, string name) {
         throw new NotImplementedException();
      }

      // Note: Passing a value to this one might be too much so it probably should come from
      // the body. However, this requires us to capture the body as a raw data/string and
      // forward it.
      [HttpPut("write/{name}/{value}")]
      public Task<IActionResult> WritePropertyAsync(string id, string name, dynamic value) {
         throw new NotImplementedException();
      }

      // Note: Passing parameters to this one might be too much so it probably should come from
      // the body. However, this requires us to capture the body as a raw data/string and
      // forward it.
      [HttpPost("invoke/{name}/{parameters}")]
      public Task<IActionResult> InvokeActionAsync(string id, string name, dynamic parameters) {
         throw new NotImplementedException();
      }

      [HttpPost("on-property-change/{name}")]
      public Task<IActionResult> OnPropertyChangeAsync(string id, string name) {
         throw new NotImplementedException();
      }

      [HttpPost("on-td-change")]
      public Task<IActionResult> OnTDChangeAsync(string id) {
         throw new NotImplementedException();
      }

      [HttpPost("on-event/{name}")]
      public Task<IActionResult> OnEventAsync(string id, string name) {
         throw new NotImplementedException();
      }
   }
}
