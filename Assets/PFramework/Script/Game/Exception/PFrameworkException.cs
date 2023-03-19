using System;
using System.Runtime.Serialization;

namespace PFramework
{
    public class NotFoundException : SystemException
    {
        /// <summary>
        /// Not Found Exception
        /// </summary>
        public NotFoundException(string message) : base(message)
        {

        }
    }

    public class AlreadyHaveException : SystemException
    {
        /// <summary>
        /// Not Found Exception
        /// </summary>
        public AlreadyHaveException(string message) : base(message)
        {
            
        }
    }
}
