namespace VkBot.BotApi
{
    public class VkDialogsParameters
    {
        public const int MaxMessagesCount = 200;

        public VkDialogsParameters(bool unread = false, bool important = false, bool unanswered = false, int count = 12, long offset = 0, int? previewLength = null)
        {
            Unread = unread;
            Important = important;
            Unanswered = unanswered;
            Count = count;
            Offset = offset;
            PreviewLength = previewLength;
        }

        private VkDialogsParameters()
        {
            
        }

        public bool Unread { get; set; }
        public bool Important { get; set; }
        public bool Unanswered { get; set; }
        public int Count { get; set; }
        public long Offset { get; set; }
        public int? PreviewLength { get; set; }
    }
}