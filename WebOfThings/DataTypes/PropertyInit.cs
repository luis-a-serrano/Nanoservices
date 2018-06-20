using System;
using System.Collections.Generic;
using System.Text;
using WebOfThings.Interfaces;

namespace WebOfThings.DataTypes {
   public class PropertyInit: IPropertyInit, IDataSchema {
      // IPropertyInit
      public bool Writable { get; set; } = false;
      public bool Observable { get; set; } = false;
      public dynamic Value { get; set; }

      // IDataSchema

   }
}
