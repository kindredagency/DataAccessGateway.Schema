using System;

namespace Framework.DataAccessGateway.Schema
{
    /// <summary>
    ///     Summary description for DBHandlerException.
    /// </summary>
    [Serializable]
    internal sealed class DBSchemaHandlerException : Exception
    {
        /// <summary>
        ///     Exception Handler for DBHandler
        /// </summary>
        public DBSchemaHandlerException()
        {
        }

        /// <summary>
        ///     Exception Handler for DBHandler
        /// </summary>
        /// <param name="message"></param>
        public DBSchemaHandlerException(string message)
            : base(message)
        {
        }

        /// <summary>
        ///     Exception Handler for DBHandler
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public DBSchemaHandlerException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}