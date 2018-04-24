using System;
using System.Collections.Generic;
using System.Text;

namespace ObjectActor.Interfaces {
   public enum SourceType {
      Internal,
      External
   }

   public class PropertyDescriptor {
      public bool CanRead { get; set; }
      public bool CanWrite { get; set; }
      public SourceType Source { get; set; }
      public string Value { get; set; }
   }
}
