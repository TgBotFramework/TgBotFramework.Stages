using System;
using System.Diagnostics.CodeAnalysis;

namespace TgBotFramework.Stages
{
    public static class GuidFromIds
    {
        public static Guid Get(long userId, [DisallowNull] long? chatId = 0)
        {
            if (chatId == null) 
                throw new ArgumentNullException(nameof(chatId));
            byte[] guidData = new byte[16];
            Array.Copy(BitConverter.GetBytes(userId), guidData, 8);
            Array.Copy(BitConverter.GetBytes(chatId.Value), 0, guidData, 8, 8);
            return new Guid(guidData);
        }
    }
}