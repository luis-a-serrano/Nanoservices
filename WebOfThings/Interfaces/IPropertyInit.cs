using System;
using System.Collections.Generic;
using System.Text;

namespace WebOfThings.Interfaces {
   public interface IPropertyInit {
      bool Writable { get; }
      bool Observable { get; }
      dynamic Value { get; }
   }
}
