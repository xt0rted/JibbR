namespace JibbR.Queuing
{
    public class JabbrMessage
    {
        public JabbrMessage(MessageType type)
        {
            Type = type;
        }

        public MessageType Type { get; set; }
        public string Room { get; set; }
        public string To { get; set; }
        public string Message { get; set; }
    }
}