using System;
using System.Collections.Generic;
using System.Text;

namespace WebOfThings {
   // Source: https://w3c.github.io/wot-scripting-api/#dom-observable
   public class WoTObservable {
      public WoTSubscription Subscribe(WoTObserver observer) {
         throw new NotImplementedException();
      }
      // Note: The overloaded method uses callbacks to set the next, error and complete handlers.
      // Due to how our code is structured this might need to be modified.
   }
}
