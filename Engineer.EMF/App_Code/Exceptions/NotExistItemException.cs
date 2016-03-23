using System;

namespace Engineer.EMF.Utils.Exceptions
{
    public class NotExistItemException : Exception
    {
        private string errorMessage;
        public NotExistItemException(string message)
        {
            this.errorMessage = message;
        }

        public string ErrorMessage
        {
            get
            {
                return errorMessage;
            }

            set
            {
                errorMessage = value;
            }
        }
    }
}