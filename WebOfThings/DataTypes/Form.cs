using System;
using System.Collections.Generic;
using System.Text;
using WebOfThings.Interfaces;

namespace WebOfThings.DataTypes {
   public class Form: ILink {
      public string Href { get; set; }
      public string MediaType { get; set; }
      public string Rel { get; set; }
   }
}
