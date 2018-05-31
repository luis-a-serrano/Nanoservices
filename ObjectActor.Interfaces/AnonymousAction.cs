using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ObjectActor.Interfaces {
   public class AnonymousAction {
      public Uri Address { get; set; }
      public dynamic Payload { get; set; }
   }
}
