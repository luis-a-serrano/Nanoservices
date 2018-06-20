using System;
using System.Collections.Generic;
using System.Text;
using WebOfThings.DataTypes;

namespace WebOfThings.Interfaces {
   public interface IInteraction {
      string Label { get; }
      Form[] Forms { get; }
   }
}
