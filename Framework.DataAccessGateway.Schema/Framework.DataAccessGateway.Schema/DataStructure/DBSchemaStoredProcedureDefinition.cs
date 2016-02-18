using Framework.DataAccessGateway.Core;
using Framework.DataAccessGateway.Schema.Collection;

namespace Framework.DataAccessGateway.Schema.DataStructure
{
    /// <summary>
    ///     Stored procedure definition
    /// </summary>
    public class DBSchemaStoredProcedureDefinition
    {
        #region Private Variables

        private DBSchemaStoredProcedureParameterDefinitionCollection
            dbSchemaStoredProcedureParameterDefinitionCollection;

        #endregion Private Variables

        #region Public Methods

        /// <summary>
        ///     Copies this instance.
        /// </summary>
        /// <returns>DBSchemaStoredProcedureDefinition.</returns>
        public DBSchemaStoredProcedureDefinition Copy()
        {
            var dbSchemaStoredProcedureDefinitionCopy = new DBSchemaStoredProcedureDefinition(Name);

            dbSchemaStoredProcedureDefinitionCopy.ProcedureBody = ProcedureBody;

            return dbSchemaStoredProcedureDefinitionCopy;
        }

        #endregion Public Methods

        #region Public Properties

        /// <summary>
        ///     Name of the stored procedure
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Command sql for stored procedure
        /// </summary>
        public string ProcedureBody { get; set; }

        /// <summary>
        ///     Gets or sets the parameters.
        /// </summary>
        /// <value>The parameters.</value>
        public DBSchemaStoredProcedureParameterDefinitionCollection Parameters
        {
            get { return dbSchemaStoredProcedureParameterDefinitionCollection; }
            set { dbSchemaStoredProcedureParameterDefinitionCollection = value; }
        }

        #endregion Public Properties

        #region Constructor

        /// <summary>
        ///     Initializes a new instance of the <see cref="DBSchemaStoredProcedureDefinition" /> class.
        /// </summary>
        public DBSchemaStoredProcedureDefinition()
        {
            ProcedureBody = null;
            Name = null;
            Parameters = new DBSchemaStoredProcedureParameterDefinitionCollection();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="DBSchemaStoredProcedureDefinition" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public DBSchemaStoredProcedureDefinition(string name)
        {
            ProcedureBody = null;
            Name = name;
            Parameters = new DBSchemaStoredProcedureParameterDefinitionCollection();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="DBSchemaStoredProcedureDefinition" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="storedProcedureBody">The stored procedure body.</param>
        public DBSchemaStoredProcedureDefinition(string name, string storedProcedureBody)
        {
            Name = name;
            ProcedureBody = storedProcedureBody;
            Parameters = new DBSchemaStoredProcedureParameterDefinitionCollection();
        }

        #endregion Constructor
    }

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