using System;

namespace Framework.DataAccessGateway.Schema
{
    /// <summary>
    ///     Summary description for IDBSchemaHandler
    /// </summary>
    public interface IDBSchemaHandler : IDisposable
    {
        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <value>The connection string.</value>
        string ConnectionString { get; }

        /// <summary>
        /// Gets the name of the server.
        /// </summary>
        /// <value>The name of the server.</value>
        string ServerName { get; }

        /// <summary>
        /// Gets the name of the data base.
        /// </summary>
        /// <value>The name of the data base.</value>
        string DataBaseName { get; }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        new void Dispose();

        /// <summary>
        /// Gets the data base definition.
        /// </summary>
        /// <returns>DBSchemaDataBaseDefinition.</returns>
        DBSchemaDataBaseDefinition GetDataBaseDefinition();

        /// <summary>
        /// Gets the table definition.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <returns>DBSchemaTableDefinition.</returns>
        DBSchemaTableDefinition GetTableDefinition(string tableName);

        /// <summary>
        /// Gets the table definition listing.
        /// </summary>
        /// <returns>DBSchemaTableDefinitionCollection.</returns>
        DBSchemaTableDefinitionCollection GetTableDefinitionListing();

        /// <summary>
        /// Gets the stored procedure definition.
        /// </summary>
        /// <param name="storedProcedureName">Name of the stored procedure.</param>
        /// <returns>DBSchemaStoredProcedureDefinition.</returns>
        DBSchemaStoredProcedureDefinition GetStoredProcedureDefinition(string storedProcedureName);

        /// <summary>
        /// Gets the stored procedure definition listing.
        /// </summary>
        /// <returns>DBSchemaStoredProcedureDefinitionCollection.</returns>
        DBSchemaStoredProcedureDefinitionCollection GetStoredProcedureDefinitionListing();

        /// <summary>
        /// Gets the trigger definition.
        /// </summary>
        /// <param name="triggerName">Name of the trigger.</param>
        /// <returns>DBSchemaTriggerDefinition.</returns>
        DBSchemaTriggerDefinition GetTriggerDefinition(string triggerName);

        /// <summary>
        /// Gets the trigger definition listing.
        /// </summary>
        /// <returns>DBSchemaTriggerDefinitionCollection.</returns>
        DBSchemaTriggerDefinitionCollection GetTriggerDefinitionListing();   
    }
}