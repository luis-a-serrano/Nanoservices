using System;
using System.Collections.Generic;
using System.Text;

namespace WebOfThings {
   // Source: https://w3c.github.io/wot-scripting-api/#dom-subscription
   public class WoTSubscription {
      public bool Closed { get; }

      public void Unsubscribe() {
         throw new NotImplementedException();
      }
   }
}
