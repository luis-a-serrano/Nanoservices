namespace WebOfThings {
   public class WoTReply {
      public WoTReplyType Type { get; set; } = WoTReplyType.None;
      public WoTResult Result { get; set; } = null;
      public WoTError Error { get; set; } = null;

      public void SetError(string description) {
         Type = WoTReplyType.Error;
         Error = new WoTError(description);
      }
   }
}
