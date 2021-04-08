using System;
using System.Runtime.Serialization;

#nullable enable
namespace CommonLibrary.Exception
{  [Serializable]
     
     public class MinimumAvatarPacketCreativeException : SystemException
     {
         public MinimumAvatarPacketCreativeException()
         {
             
         }
 
         public MinimumAvatarPacketCreativeException(string? message)
             : base(message)
         {
         }
 
         public MinimumAvatarPacketCreativeException(string? message, System.Exception? innerException)
             : base(message, innerException)
         {
         }
 
         internal MinimumAvatarPacketCreativeException(SerializationInfo info, StreamingContext context) : base(info, context)
         {
         }
     }
  
}