using System;
using System.Collections.Generic;
using System.Text;
using WebOfThings.Enumerations;

namespace WebOfThings.Interfaces {
   public interface IDataSchema {
      DataType Type { get; }
      bool Required { get; }
      string Description { get; }
      bool Const { get; }
   }
}
