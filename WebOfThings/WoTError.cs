﻿using System;
using System.Collections.Generic;
using System.Text;

namespace WebOfThings {
   // Note: Error "thrown" by some of the methods
   public class WoTError {
      public string Description { get; set; }   

      public WoTError() { }

      public WoTError(string description) {
         Description = description;
      }
   }
}
