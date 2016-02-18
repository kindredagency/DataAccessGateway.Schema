using System;
using System.Collections.Generic;

namespace Framework.DataAccessGateway.Schema
{
    /// <summary>
    ///     Summary description for IDBSchemaHandler
    /// </summary>
    public interface IDBSchemaHandler : IDisposable
    {
        #region Public Properties

        /// <summary>
        ///     Read Only: ConnectionString for the database.
        /// </summary>
        string ConnectionString { get; }

        #endregion Public Properties

        #region IDisposable Members

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        new void Dispose();

        #endregion IDisposable Members

        #region Public Methods

        /// <summary>
        ///     Gets the database server listing.
        /// </summary>
        /// <returns>List{DBSchemaServerDefinition}.</returns>
        List<DBSchemaServerDefinition> GetDBServerListing();

        /// <summary>
        ///     Gets the database.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <returns>DBSchemaDBDefinition.</returns>
        DBSchemaDBDefinition GetDB(string connectionString);

        /// <summary>
        ///     Gets the database.
        /// </summary>
        /// <param name="dbServerName">Name of the database server.</param>
        /// <param name="dbUserID">The database user identifier.</param>
        /// <param name="dbPassword">The database password.</param>
        /// <param name="dbName">Name of the database.</param>
        /// <returns>DBSchemaDBDefinition.</returns>
        DBSchemaDBDefinition GetDB(string dbServerName, string dbUserID, string dbPassword, string dbName);

        /// <summary>
        ///     Gets the database listing.
        /// </summary>
        /// <param name="dbServerName">Name of the database server.</param>
        /// <param name="dbUserID">The database user identifier.</param>
        /// <param name="dbPassword">The database password.</param>
        /// <returns>List{DBSchemaDBDefinition}.</returns>
        List<DBSchemaDBDefinition> GetDBListing(string dbServerName, string dbUserID, string dbPassword);

        /// <summary>
        ///     Gets the table definition.
        /// </summary>
        /// <param name="dbServerName">Name of the database server.</param>
        /// <param name="dbName">Name of the database.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="dbUserID">The database user identifier.</param>
        /// <param name="dbPassword">The database password.</param>
        /// <returns>DBSchemaTableDefinition.</returns>
        DBSchemaTableDefinition GetTableDefinition(string dbServerName, string dbName, string tableName, string dbUserID,
            string dbPassword);

        /// <summary>
        ///     Gets the table definition listing.
        /// </summary>
        /// <param name="dbServerName">Name of the database server.</param>
        /// <param name="dbName">Name of the database.</param>
        /// <param name="dbUserID">The database user identifier.</param>
        /// <param name="dbPassword">The database password.</param>
        /// <returns>DBSchemaTableDefinitionCollection.</returns>
        DBSchemaTableDefinitionCollection GetTableDefinitionListing(string dbServerName, string dbName, string dbUserID,
            string dbPassword);

        /// <summary>
        ///     Gets the proc definition.
        /// </summary>
        /// <param name="dbServerName">Name of the database server.</param>
        /// <param name="dbName">Name of the database.</param>
        /// <param name="procName">Name of the proc.</param>
        /// <param name="dbUserID">The database user identifier.</param>
        /// <param name="dbPassword">The database password.</param>
        /// <returns>DBSchemaStoredProcedureDefinition.</returns>
        DBSchemaStoredProcedureDefinition GetProcDefinition(string dbServerName, string dbName, string procName,
            string dbUserID, string dbPassword);

        /// <summary>
        ///     Gets the proc definition listing.
        /// </summary>
        /// <param name="dbServerName">Name of the database server.</param>
        /// <param name="dbName">Name of the database.</param>
        /// <param name="dbUserID">The database user identifier.</param>
        /// <param name="dbPassword">The database password.</param>
        /// <returns>DBSchemaStoredProcedureDefinitionCollection.</returns>
        DBSchemaStoredProcedureDefinitionCollection GetProcDefinitionListing(string dbServerName, string dbName,
            string dbUserID, string dbPassword);

        /// <summary>
        ///     Gets the trigger definition.
        /// </summary>
        /// <param name="dbServerName">Name of the database server.</param>
        /// <param name="dbName">Name of the database.</param>
        /// <param name="triggerName">Name of the trigger.</param>
        /// <param name="dbUserID">The database user identifier.</param>
        /// <param name="dbPassword">The database password.</param>
        /// <returns>DBSchemaTriggerDefinition.</returns>
        DBSchemaTriggerDefinition GetTriggerDefinition(string dbServerName, string dbName, string triggerName,
            string dbUserID, string dbPassword);

        /// <summary>
        ///     Gets the trigger definition listing.
        /// </summary>
        /// <param name="dbServerName">Name of the database server.</param>
        /// <param name="dbName">Name of the database.</param>
        /// <param name="dbUserID">The database user identifier.</param>
        /// <param name="dbPassword">The database password.</param>
        /// <returns>DBSchemaTriggerDefinitionCollection.</returns>
        DBSchemaTriggerDefinitionCollection GetTriggerDefinitionListing(string dbServerName, string dbName,
            string dbUserID, string dbPassword);       

        /// <summary>
        ///     Builds the connection string.
        /// </summary>
        /// <param name="dbServerName">Name of the database server.</param>
        /// <param name="dbName">Name of the database.</param>
        /// <param name="dbUserID">The database user identifier.</param>
        /// <param name="dbPassword">The database password.</param>
        /// <returns>System.String.</returns>
        string BuildConnectionString(string dbServerName, string dbName, string dbUserID, string dbPassword);

        /// <summary>
        ///     Breaks the connection string.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="dbServerName">Name of the database server.</param>
        /// <param name="dbName">Name of the database.</param>
        /// <param name="dbUserID">The database user identifier.</param>
        /// <param name="dbPassword">The database password.</param>
        void BreakConnectionString(string connectionString, out string dbServerName, out string dbName,
            out string dbUserID, out string dbPassword);

        #endregion Public Methods
    }
}