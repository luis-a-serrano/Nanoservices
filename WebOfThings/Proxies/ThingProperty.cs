using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebOfThings.DataTypes;
using WebOfThings.Enumerations;
using WebOfThings.Interfaces;

namespace WebOfThings.Proxies {
   // TODO: Observable behaviour to be added later.
   // Note: Actually need to make two versions; one works as a proxy and we redefine the behaviour of
   // properties and methods, the other is a POCO so that we can pass it around when needed.
   public abstract class ThingProperty: IInteractionInit, IPropertyInit, IDataSchema, IInteraction {
      // IPropertyInit
      public abstract bool Writable { get; set; }
      public abstract bool Observable { get; set; }
      public abstract dynamic Value { get; set; }

      // IDataSchema
      public DataType Type { get; set; } // Required
      public bool Required { get; set; } = false;
      public string Description { get; set; }
      public bool Const { get; set; }

      // IInteractionInit
      public string Label { get; set; }

      // IInteraction
      public Form[] Forms { get; set; }

      // ThingProperty
      public abstract Task<dynamic> Get(); // Can throw.
      public abstract Task Set(dynamic value); // Can throw.
   }
}
