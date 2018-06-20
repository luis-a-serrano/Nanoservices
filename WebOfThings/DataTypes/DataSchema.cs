using System;
using System.Collections.Generic;
using System.Text;
using WebOfThings.Enumerations;
using WebOfThings.Interfaces;

namespace WebOfThings.DataTypes {
   public class DataSchema: IDataSchema {
      public DataType Type { get; set; } // Required
      public bool Required { get; set; } = false;
      public string Description { get; set; }
      public bool Const { get; set; }
   }
}
