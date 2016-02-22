namespace Framework.DataAccessGateway.Schema
{
    /// <summary>
    ///     Stored procedure definition
    /// </summary>
    public class DBSchemaStoredProcedureDefinition
    {
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
            get; set;
        }        

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
    }    
}