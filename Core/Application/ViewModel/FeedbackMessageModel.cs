using Core.Application.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Application.ViewModel
{
    public class FeedbackMessageModel
    {
        public string Message { get; private set; }

        public MessageType MessageType { get; private set; }

        public static FeedbackMessageModel CreateSuccessMessage(string message)
        {
            return CreateModel(message, MessageType.Success);
        }

        public static FeedbackMessageModel CreateWarningMessage(string message)
        {
            return CreateModel(message, MessageType.Warning);
        }

        public static FeedbackMessageModel CreateErrorMessage(string message)
        {
            return CreateModel(message, MessageType.Error);
        }

        private static FeedbackMessageModel CreateModel(string message, MessageType messageType)
        {
            return new FeedbackMessageModel { Message = message, MessageType = messageType };
        }

        private FeedbackMessageModel()
        {

        }
    }
}
