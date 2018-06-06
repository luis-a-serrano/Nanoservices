using System;
using System.Collections.Generic;
using System.Text;

namespace ObjectActor.Interfaces {
   public class ActionResponse {
      public Dictionary<string, dynamic> Updates { get; set; }
      public dynamic Result { get; set; } = null;
   }
}