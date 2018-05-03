using System;
using System.Collections.Generic;
using System.Text;

namespace WebOfThings {
   public class WoTReply {
      public WoTReplyType Type { get; set; } = WoTReplyType.None;
      public WoTResult Result { get; set; } = null;
      public WoTError Error { get; set; } = null;
   }
}
