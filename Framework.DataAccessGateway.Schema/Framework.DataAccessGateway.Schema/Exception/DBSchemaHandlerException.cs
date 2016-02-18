using System;

namespace Framework.DataAccessGateway.Schema.Exception
{
    /// <summary>
    ///     Summary description for DBHandlerException.
    /// </summary>
    [Serializable]
    internal sealed class DBSchemaHandlerException : System.Exception
    {
        #region Constructor Definitions

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
        public DBSchemaHandlerException(string message, System.Exception innerException)
            : base(message, innerException)
        {
        }

        #endregion Constructor Definitions
    }
}