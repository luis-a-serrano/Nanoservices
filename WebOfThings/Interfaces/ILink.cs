using System;
using System.Collections.Generic;
using System.Text;

namespace WebOfThings.Interfaces {
   public interface ILink {
      string Href { get; }
      string MediaType { get; }
      string Rel { get; }
   }
}
