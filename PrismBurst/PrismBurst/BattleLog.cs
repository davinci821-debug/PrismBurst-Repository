namespace PrismBurst
{
    internal class BattleLog
    {
        public string Message
        {
            get;
            private set;
        }

        public BattleLog()
        {
            Message = "";
        }

        public void SetMessage(string text)
        {
            Message = text;
        }

        public string GetMessage()
        {
            return Message;
        }
    }
}