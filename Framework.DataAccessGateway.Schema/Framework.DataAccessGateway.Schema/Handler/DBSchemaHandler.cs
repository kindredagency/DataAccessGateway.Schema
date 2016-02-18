using System.Collections.Generic;
using Framework.DataAccessGateway.Schema.Collection;
using Framework.DataAccessGateway.Schema.DataStructure;
using Framework.DataAccessGateway.Schema.Enum;
using Framework.DataAccessGateway.Schema.Interface;
using Framework.DataAccessGateway.Core;

namespace Framework.DataAccessGateway.Schema.Handler
{
    /// <summary>
    ///     Summary description for DBSchemaHandler
    /// </summary>
    public class DBSchemaHandler : IDBSchemaHandler
    {
        #region Private Variables

        private readonly IDBSchemaHandler independentDBHandler;

        #endregion Private Variables

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="DBSchemaHandler"/> class.
        /// </summary>
        /// <param name="dbHandlerType">Type of the database handler.</param>
        public DBSchemaHandler(DBHandlerType dbHandlerType)
        {
            //Include More Types as support becomes more available.

            switch (dbHandlerType)
            {
                case DBHandlerType.DbHandlerMSSQL:
                    independentDBHandler = new DBSchemaHandlerMSSQL();
                    break;
            }
        }

        #endregion Constructor

        #region Public Properties

        /// <summary>
        ///     Read Only: ConnectionString for the database.
        /// </summary>
        /// <value>The connection string.</value>
        public string ConnectionString
        {
            get { return independentDBHandler.ConnectionString; }
        }

        #endregion Public Properties

        #region IDBSchemaHandler Members

        /// <summary>
        ///     Returns a list DB Servers Connected to the network.
        ///     May not always return all servers on the network
        /// </summary>
        /// <returns>Generic string list of DBSchemaServerInstance objects</returns>
        public List<DBSchemaServerDefinition> GetDBServerListing()
        {
            return independentDBHandler.GetDBServerListing();
        }

        /// <summary>
        ///     Returns a database in the given server
        /// </summary>
        /// <param name="connectionString">DB Connection string of the database</param>
        /// <returns>DBSchemaDBInstance</returns>
        public DBSchemaDBDefinition GetDB(string connectionString)
        {
            return independentDBHandler.GetDB(connectionString);
        }

        /// <summary>
        ///     Gets the database.
        /// </summary>
        /// <param name="dbServerName">Name of the database server.</param>
        /// <param name="dbUserID">The database user identifier.</param>
        /// <param name="dbPassword">The database password.</param>
        /// <param name="dbName">Name of the database.</param>
        /// <returns>DBSchemaDBDefinition.</returns>
        public DBSchemaDBDefinition GetDB(string dbServerName, string dbUserID, string dbPassword, string dbName)
        {
            return independentDBHandler.GetDB(dbServerName, dbUserID, dbPassword, dbName);
        }

        /// <summary>
        ///     Gets the database listing.
        /// </summary>
        /// <param name="dbServerName">Name of the database server.</param>
        /// <param name="dbUserID">The database user identifier.</param>
        /// <param name="dbPassword">The database password.</param>
        /// <returns>List{DBSchemaDBDefinition}.</returns>
        public List<DBSchemaDBDefinition> GetDBListing(string dbServerName, string dbUserID, string dbPassword)
        {
            return independentDBHandler.GetDBListing(dbServerName, dbUserID, dbPassword);
        }

        /// <summary>
        ///     Gets the table definition.
        /// </summary>
        /// <param name="dbServerName">Name of the database server.</param>
        /// <param name="dbName">Name of the database.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="dbUserID">The database user identifier.</param>
        /// <param name="dbPassword">The database password.</param>
        /// <returns>DBSchemaTableDefinition.</returns>
        public DBSchemaTableDefinition GetTableDefinition(string dbServerName, string dbName, string tableName,
            string dbUserID, string dbPassword)
        {
            return independentDBHandler.GetTableDefinition(dbServerName, dbName, tableName, dbUserID, dbPassword);
        }

        /// <summary>
        ///     Gets the table definition listing.
        /// </summary>
        /// <param name="dbServerName">Name of the database server.</param>
        /// <param name="dbName">Name of the database.</param>
        /// <param name="dbUserID">The database user identifier.</param>
        /// <param name="dbPassword">The database password.</param>
        /// <returns>DBSchemaTableDefinitionCollection.</returns>
        public DBSchemaTableDefinitionCollection GetTableDefinitionListing(string dbServerName, string dbName,
            string dbUserID, string dbPassword)
        {
            return independentDBHandler.GetTableDefinitionListing(dbServerName, dbName, dbUserID, dbPassword);
        }

        /// <summary>
        ///     Gets the proc definition.
        /// </summary>
        /// <param name="dbServerName">Name of the database server.</param>
        /// <param name="dbName">Name of the database.</param>
        /// <param name="procName">Name of the proc.</param>
        /// <param name="dbUserID">The database user identifier.</param>
        /// <param name="dbPassword">The database password.</param>
        /// <returns>DBSchemaStoredProcedureDefinition.</returns>
        public DBSchemaStoredProcedureDefinition GetProcDefinition(string dbServerName, string dbName, string procName,
            string dbUserID, string dbPassword)
        {
            return independentDBHandler.GetProcDefinition(dbServerName, dbName, procName, dbUserID, dbPassword);
        }

        /// <summary>
        ///     Gets the proc definition listing.
        /// </summary>
        /// <param name="dbServerName">Name of the database server.</param>
        /// <param name="dbName">Name of the database.</param>
        /// <param name="dbUserID">The database user identifier.</param>
        /// <param name="dbPassword">The database password.</param>
        /// <returns>DBSchemaStoredProcedureDefinitionCollection.</returns>
        public DBSchemaStoredProcedureDefinitionCollection GetProcDefinitionListing(string dbServerName, string dbName,
            string dbUserID, string dbPassword)
        {
            return independentDBHandler.GetProcDefinitionListing(dbServerName, dbName, dbUserID, dbPassword);
        }

        /// <summary>
        ///     Gets the trigger definition.
        /// </summary>
        /// <param name="dbServerName">Name of the database server.</param>
        /// <param name="dbName">Name of the database.</param>
        /// <param name="triggerName">Name of the trigger.</param>
        /// <param name="dbUserID">The database user identifier.</param>
        /// <param name="dbPassword">The database password.</param>
        /// <returns>DBSchemaTriggerDefinition.</returns>
        public DBSchemaTriggerDefinition GetTriggerDefinition(string dbServerName, string dbName, string triggerName,
            string dbUserID, string dbPassword)
        {
            return independentDBHandler.GetTriggerDefinition(dbServerName, dbName, triggerName, dbUserID, dbPassword);
        }

        /// <summary>
        ///     Gets the trigger definition listing.
        /// </summary>
        /// <param name="dbServerName">Name of the database server.</param>
        /// <param name="dbName">Name of the database.</param>
        /// <param name="dbUserID">The database user identifier.</param>
        /// <param name="dbPassword">The database password.</param>
        /// <returns>DBSchemaTriggerDefinitionCollection.</returns>
        public DBSchemaTriggerDefinitionCollection GetTriggerDefinitionListing(string dbServerName, string dbName,
            string dbUserID, string dbPassword)
        {
            return independentDBHandler.GetTriggerDefinitionListing(dbServerName, dbName, dbUserID, dbPassword);
        }

        /// <summary>
        ///     Creates the database.
        /// </summary>
        /// <param name="dbServerName">Name of the database server.</param>
        /// <param name="dbUserID">The database user identifier.</param>
        /// <param name="dbPassword">The database password.</param>
        /// <param name="dbSchemaDBInstance">The database schema database instance.</param>
        public void CreateDatabase(string dbServerName, string dbUserID, string dbPassword,
            DBSchemaDBDefinition dbSchemaDBInstance)
        {
            independentDBHandler.CreateDatabase(dbServerName, dbUserID, dbPassword, dbSchemaDBInstance);
        }

        /// <summary>
        ///     Creates the database.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="dbSchemaDBInstance">The database schema database instance.</param>
        public void CreateDatabase(string connectionString, DBSchemaDBDefinition dbSchemaDBInstance)
        {
            independentDBHandler.CreateDatabase(connectionString, dbSchemaDBInstance);
        }

        /// <summary>
        ///     Alters the database.
        /// </summary>
        /// <param name="dbServerName">Name of the database server.</param>
        /// <param name="dbUserID">The database user identifier.</param>
        /// <param name="dbPassword">The database password.</param>
        /// <param name="dbSchemaDBInstance">The database schema database instance.</param>
        public void AlterDatabase(string dbServerName, string dbUserID, string dbPassword,
            DBSchemaDBDefinition dbSchemaDBInstance)
        {
            independentDBHandler.AlterDatabase(dbServerName, dbUserID, dbPassword, dbSchemaDBInstance);
        }

        /// <summary>
        ///     Alters the database.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="dbSchemaDBInstance">The database schema database instance.</param>
        public void AlterDatabase(string connectionString, DBSchemaDBDefinition dbSchemaDBInstance)
        {
            independentDBHandler.AlterDatabase(connectionString, dbSchemaDBInstance);
        }

        /// <summary>
        ///     Drops the database.
        /// </summary>
        /// <param name="dbServerName">Name of the database server.</param>
        /// <param name="dbUserID">The database user identifier.</param>
        /// <param name="dbPassword">The database password.</param>
        /// <param name="dbName">Name of the database.</param>
        public void DropDatabase(string dbServerName, string dbUserID, string dbPassword, string dbName)
        {
            independentDBHandler.DropDatabase(dbServerName, dbUserID, dbPassword, dbName);
        }

        /// <summary>
        ///     Drops the database.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="dbName">Name of the database.</param>
        public void DropDatabase(string connectionString, string dbName)
        {
            independentDBHandler.DropDatabase(connectionString, dbName);
        }

        /// <summary>
        ///     Creates the table.
        /// </summary>
        /// <param name="dbServerName">Name of the database server.</param>
        /// <param name="dbName">Name of the database.</param>
        /// <param name="dbUserID">The database user identifier.</param>
        /// <param name="dbPassword">The database password.</param>
        /// <param name="dbSchemaTableDefinition">The database schema table definition.</param>
        public void CreateTable(string dbServerName, string dbName, string dbUserID, string dbPassword,
            DBSchemaTableDefinition dbSchemaTableDefinition)
        {
            independentDBHandler.CreateTable(dbServerName, dbName, dbUserID, dbPassword, dbSchemaTableDefinition);
        }

        /// <summary>
        ///     Creates the table.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="dbSchemaTableDefinition">The database schema table definition.</param>
        public void CreateTable(string connectionString, DBSchemaTableDefinition dbSchemaTableDefinition)
        {
            independentDBHandler.CreateTable(connectionString, dbSchemaTableDefinition);
        }

        /// <summary>
        ///     Alters the table.
        /// </summary>
        /// <param name="dbServerName">Name of the database server.</param>
        /// <param name="dbName">Name of the database.</param>
        /// <param name="dbUserID">The database user identifier.</param>
        /// <param name="dbPassword">The database password.</param>
        /// <param name="dbOldSchemaTableDefinition">The database old schema table definition.</param>
        /// <param name="dbNewSchemaTableDefinition">The database new schema table definition.</param>
        /// <param name="dbSchemaAlterTableOperation">The database schema alter table operation.</param>
        public void AlterTable(string dbServerName, string dbName, string dbUserID, string dbPassword,
            DBSchemaTableDefinition dbOldSchemaTableDefinition, DBSchemaTableDefinition dbNewSchemaTableDefinition,
            DBSchemaAlterTableOperation dbSchemaAlterTableOperation)
        {
            independentDBHandler.AlterTable(dbServerName, dbName, dbUserID, dbPassword, dbOldSchemaTableDefinition,
                dbNewSchemaTableDefinition, dbSchemaAlterTableOperation);
        }

        /// <summary>
        ///     Alters the table.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="dbOldSchemaTableDefinition">The database old schema table definition.</param>
        /// <param name="dbNewSchemaTableDefinition">The database new schema table definition.</param>
        /// <param name="dbSchemaAlterTableOperation">The database schema alter table operation.</param>
        public void AlterTable(string connectionString, DBSchemaTableDefinition dbOldSchemaTableDefinition,
            DBSchemaTableDefinition dbNewSchemaTableDefinition, DBSchemaAlterTableOperation dbSchemaAlterTableOperation)
        {
            independentDBHandler.AlterTable(connectionString, dbOldSchemaTableDefinition, dbNewSchemaTableDefinition,
                dbSchemaAlterTableOperation);
        }

        /// <summary>
        ///     Drops the table.
        /// </summary>
        /// <param name="dbServerName">Name of the database server.</param>
        /// <param name="dbName">Name of the database.</param>
        /// <param name="dbUserID">The database user identifier.</param>
        /// <param name="dbPassword">The database password.</param>
        /// <param name="tableName">Name of the table.</param>
        public void DropTable(string dbServerName, string dbName, string dbUserID, string dbPassword, string tableName)
        {
            independentDBHandler.DropTable(dbServerName, dbName, dbUserID, dbPassword, tableName);
        }

        /// <summary>
        ///     Drops the table.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="tableName">Name of the table.</param>
        public void DropTable(string connectionString, string tableName)
        {
            independentDBHandler.DropTable(connectionString, tableName);
        }

        /// <summary>
        ///     Builds the connection string.
        /// </summary>
        /// <param name="dbServerName">Name of the database server.</param>
        /// <param name="dbName">Name of the database.</param>
        /// <param name="dbUserID">The database user identifier.</param>
        /// <param name="dbPassword">The database password.</param>
        /// <returns>System.String.</returns>
        public string BuildConnectionString(string dbServerName, string dbName, string dbUserID, string dbPassword)
        {
            return independentDBHandler.BuildConnectionString(dbServerName, dbName, dbUserID, dbPassword);
        }

        /// <summary>
        ///     Breaks the connection string.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="dbServerName">Name of the database server.</param>
        /// <param name="dbName">Name of the database.</param>
        /// <param name="dbUserID">The database user identifier.</param>
        /// <param name="dbPassword">The database password.</param>
        public void BreakConnectionString(string connectionString, out string dbServerName, out string dbName,
            out string dbUserID, out string dbPassword)
        {
            independentDBHandler.BreakConnectionString(connectionString, out dbServerName, out dbName, out dbUserID,
                out dbPassword);
        }

        /// <summary>
        ///     Dispose unmanaged resources
        /// </summary>
        public void Dispose()
        {
            independentDBHandler.Dispose();
        }

        #endregion IDBSchemaHandler Members
    }
}