using Framework.DataAccessGateway.Core;

namespace Framework.DataAccessGateway.Schema
{
    /// <summary>
    ///     Class DBSchemaStoredProcedureParameterDefinition.
    /// </summary>
    public class DBSchemaStoredProcedureParameterDefinition
    {
        /// <summary>
        ///     Gets or sets the parameter identifier.
        /// </summary>
        /// <value>The parameter identifier.</value>
        public int ParameterID { get; set; }

        /// <summary>
        ///     Gets or sets the name of the parameter.
        /// </summary>
        /// <value>The name of the parameter.</value>
        public string ParameterName { get; set; }

        /// <summary>
        ///     Gets or sets the type of the data.
        /// </summary>
        /// <value>The type of the data.</value>
        public DBHandlerDataType DataType { get; set; }

        /// <summary>
        ///     Gets or sets the size.
        /// </summary>
        /// <value>The size.</value>
        public int? Size { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is output parameter.
        /// </summary>
        /// <value><c>true</c> if this instance is output parameter; otherwise, <c>false</c>.</value>
        public bool IsOutputParameter { get; set; }
    }
}
