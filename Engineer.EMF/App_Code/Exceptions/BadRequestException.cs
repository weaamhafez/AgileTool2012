using System;

namespace Engineer.EMF.Utils.Exceptions
{
    public class BadRequestException : Exception
    {
        private string errorMessage;
        public BadRequestException(string message)
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