namespace Engineer.Service
{
    public class DTOMessage
    {
        private string _subject, _body;
        private string[] _ccList = null;
        public string Subject { get { return _subject; } set { _subject = value; } }
        public string Body { get { return _body; } set { _body = value; } }
        public string[] CcList { get { return _ccList; } set { _ccList = value; } }
        public DTOMessage(string subject, string body)
        {
            this._subject = subject;
            this._body = body;
        }
    }
}