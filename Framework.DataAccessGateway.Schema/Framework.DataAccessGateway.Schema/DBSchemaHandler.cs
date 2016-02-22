using Framework.DataAccessGateway.Core;

namespace Framework.DataAccessGateway.Schema
{
    /// <summary>
    ///     Summary description for DBSchemaHandler
    /// </summary>
    public class DBSchemaHandler : IDBSchemaHandler
    {
        private readonly IDBSchemaHandler independentDBHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="DBSchemaHandler"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="dbHandlerType">Type of the database handler.</param>
        public DBSchemaHandler(string connectionString, DBHandlerType dbHandlerType)
        {
            switch (dbHandlerType)
            {
                case DBHandlerType.DbHandlerMSSQL: independentDBHandler = new DBSchemaHandlerMSSQL(connectionString);
                    break;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DBSchemaHandler"/> class.
        /// </summary>
        /// <param name="dbSchemaHandler">The database schema handler.</param>
        public DBSchemaHandler(IDBSchemaHandler dbSchemaHandler)
        {
            independentDBHandler = dbSchemaHandler;
        }

        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <value>The connection string.</value>
        public string ConnectionString
        {
            get
            {
                return independentDBHandler.ConnectionString;
            }
        }

        /// <summary>
        /// Gets the name of the server.
        /// </summary>
        /// <value>The name of the server.</value>
        public string ServerName
        {
            get
            {
                return independentDBHandler.ServerName;
            }
        }

        /// <summary>
        /// Gets the name of the data base.
        /// </summary>
        /// <value>The name of the data base.</value>
        public string DataBaseName
        {
            get
            {
                return independentDBHandler.DataBaseName;
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            independentDBHandler.Dispose();
        }

        /// <summary>
        /// Gets the data base definition.
        /// </summary>
        /// <returns>DBSchemaDataBaseDefinition.</returns>
        public DBSchemaDataBaseDefinition GetDataBaseDefinition()
        {
            return independentDBHandler.GetDataBaseDefinition();
        }

        /// <summary>
        /// Gets the stored procedure definition.
        /// </summary>
        /// <param name="storedProcedureName">Name of the stored procedure.</param>
        /// <returns>DBSchemaStoredProcedureDefinition.</returns>
        public DBSchemaStoredProcedureDefinition GetStoredProcedureDefinition(string storedProcedureName)
        {
            return independentDBHandler.GetStoredProcedureDefinition(storedProcedureName);
        }

        /// <summary>
        /// Gets the stored procedure definition listing.
        /// </summary>
        /// <returns>DBSchemaStoredProcedureDefinitionCollection.</returns>
        public DBSchemaStoredProcedureDefinitionCollection GetStoredProcedureDefinitionListing()
        {
            return independentDBHandler.GetStoredProcedureDefinitionListing();
        }

        /// <summary>
        /// Gets the table definition.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <returns>DBSchemaTableDefinition.</returns>
        public DBSchemaTableDefinition GetTableDefinition(string tableName)
        {
            return independentDBHandler.GetTableDefinition(tableName);
        }

        /// <summary>
        /// Gets the table definition listing.
        /// </summary>
        /// <returns>DBSchemaTableDefinitionCollection.</returns>
        public DBSchemaTableDefinitionCollection GetTableDefinitionListing()
        {
            return independentDBHandler.GetTableDefinitionListing();
        }

        /// <summary>
        /// Gets the trigger definition.
        /// </summary>
        /// <param name="triggerName">Name of the trigger.</param>
        /// <returns>DBSchemaTriggerDefinition.</returns>
        public DBSchemaTriggerDefinition GetTriggerDefinition(string triggerName)
        {
            return independentDBHandler.GetTriggerDefinition(triggerName);
        }

        /// <summary>
        /// Gets the trigger definition listing.
        /// </summary>
        /// <returns>DBSchemaTriggerDefinitionCollection.</returns>
        public DBSchemaTriggerDefinitionCollection GetTriggerDefinitionListing()
        {
            return independentDBHandler.GetTriggerDefinitionListing();
        }

        /// <summary>
        /// Gets the view.
        /// </summary>
        /// <param name="viewName">Name of the view.</param>
        /// <returns>DBSchemaViewDefinition.</returns>
        public DBSchemaViewDefinition GetView(string viewName)
        {
            return independentDBHandler.GetView(viewName);
        }

        /// <summary>
        /// Gets the view listing.
        /// </summary>
        /// <returns>DBSchemaViewDefinitionCollection.</returns>
        public DBSchemaViewDefinitionCollection GetViewListing()
        {
            return independentDBHandler.GetViewListing();
        }

        /// <summary>
        /// Gets the constraint.
        /// </summary>
        /// <param name="constraintName">Name of the constraint.</param>
        /// <returns>DBSchemaConstraintDefinition.</returns>
        public DBSchemaConstraintDefinition GetConstraint(string constraintName)
        {
            return independentDBHandler.GetConstraint(constraintName);
        }

        /// <summary>
        /// Gets the constraint definition collection.
        /// </summary>
        /// <returns>DBSchemaConstraintDefinitionCollection.</returns>
        public DBSchemaConstraintDefinitionCollection GetConstraintDefinitionCollection()
        {
            return independentDBHandler.GetConstraintDefinitionCollection();
        }

        /// <summary>
        /// Gets the user defined table types.
        /// </summary>
        /// <param name="typeName">Name of the type.</param>
        /// <returns>DBUserDefinedTableTypeDefinition.</returns>
        public DBUserDefinedTableTypeDefinition GetUserDefinedTableType(string typeName)
        {
            return independentDBHandler.GetUserDefinedTableType(typeName);
        }

        /// <summary>
        /// Gets the user defined table types.
        /// </summary>
        /// <returns>DBUserDefinedTableTypeCollection.</returns>
        public DBUserDefinedTableTypeCollection GetUserDefinedTableTypes()
        {
            return independentDBHandler.GetUserDefinedTableTypes();
        }
    }
}