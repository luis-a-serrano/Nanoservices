using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ObjectActor.Interfaces;
using ObjectAPI.DataStructures;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ObjectAPI.Controllers {
   [Route("api/[controller]")]
   public class AdaptiveNetworkController: Controller {
      private readonly HttpClient _Client = new HttpClient();

      [HttpPost("initialize")]
      public async Task<IActionResult> InitializeAsync(InitializationPayload payload) {

         var response = new ActionResponse() {
            Updates = new Dictionary<string, dynamic>()
         };

         foreach (var property in payload.Coefficients.Ids) {
            response.Updates.Add(property, payload.Coefficients.Values);
         }

         return Ok(response);
      }

      [HttpPost("learn")]
      public async Task<IActionResult> LearnAsync(LearningPayload payload) {
         var normalizer = 0.0001;
         var step = 0.02;

         var coefficients = payload.OwnCoefficients;

         for (var l = 0; l < coefficients.Length; l++) {
            for (var u = 0; u < payload.NeighborsCoefficients.Length; u++) {
               coefficients[l] += payload.NeighborsCoefficients[u][l];
            }

            coefficients[l] /= payload.NeighborsCoefficients.Length + 1;
         }

         var range = payload.Iteration.PastValues;
         var psi = new double[range.Length];

         var estimation = 0.0;
         var convergence = normalizer;

         for (var l = 0; l < range.Length; l++) {
            estimation += coefficients[l] * range[l];
            convergence += range[l] * range[l];
         }

         convergence = step / convergence;

         var signal = payload.Iteration.Signal;
         var error = signal - estimation;

         for (var l = 0; l < range.Length; l++) {
            psi[l] = coefficients[l] + (convergence * error * range[l]);
         }

         var response = new ActionResponse() {
            Updates = new Dictionary<string, dynamic> {
               [nameof(LearningPayload.OwnCoefficients)] = psi
            },
            Result = estimation
         };

         return Ok(response);
      }

      [HttpPost("share")]
      public async Task<IActionResult> ShareAsync(SharingPayload payload) {

         _Client.DefaultRequestHeaders.Accept.Clear();
         _Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

         var replies = new List<Task<HttpResponseMessage>>();
         foreach (var neighbor in payload.Neighbors) {
            var message = new AnonymousAction {
               Address = new Uri($"api/AdaptiveNetwork/listen"), //~~
               Payload = new ListeningPayload {
                  TargetProperty = payload.TargetProperty,
                  Coefficients = payload.Coefficients
               }
            };
            replies.Add(_Client.PostAsync(
                  $"api/ObjectActor/{neighbor}/action", //~~
                  new StringContent(JsonConvert.SerializeObject(message), Encoding.UTF8, "application/json")));
         }

         var results = await Task.WhenAll(replies);

         return Ok(new ActionResponse());
      }

      [HttpPost("listen")]
      public async Task<IActionResult> ListenAsync(ListeningPayload payload) {

         var response = new ActionResponse() {
            Updates = new Dictionary<string, dynamic> {
               [payload.TargetProperty] = payload.Coefficients
            }
         };

         return Ok(response);
      }

   }
}
