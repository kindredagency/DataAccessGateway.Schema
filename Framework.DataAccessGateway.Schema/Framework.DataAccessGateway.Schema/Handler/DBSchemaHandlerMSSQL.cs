using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Framework.DataAccessGateway.Schema.Interface;
using Framework.DataAccessGateway.Schema.DataStructure;
using Framework.DataAccessGateway.Core;
using Framework.DataAccessGateway.Schema.Exception;
using Framework.DataAccessGateway.Schema.Collection;
using Framework.DataAccessGateway.Schema.Enum;

namespace Framework.DataAccessGateway.Schema.Handler
{
    internal class DBSchemaHandlerMSSQL : IDBSchemaHandler
    {
        #region Properties

        /// <summary>
        /// Read Only: ConnectionString for the database.
        /// </summary>
        /// <value>The connection string.</value>
        public string ConnectionString { get; private set; }

        #endregion Properties

        #region IDBSchemaHandler Members

        /// <summary>
        /// Gets the database server listing.
        /// </summary>
        /// <returns>List{DBSchemaServerDefinition}.</returns>
        public List<DBSchemaServerDefinition> GetDBServerListing()
        {
            var sqlDataSourceEnumerator = SqlDataSourceEnumerator.Instance;
            var dtDataSources = sqlDataSourceEnumerator.GetDataSources();

            var dbSchemaServerInstanceList = new List<DBSchemaServerDefinition>();

            foreach (DataRow aRow in dtDataSources.Rows)
            {
                var dbSchemaServerInstance = new DBSchemaServerDefinition(aRow["ServerName"].ToString(),
                    aRow["InstanceName"].ToString(), aRow["IsClustered"].ToString(), aRow["Version"].ToString());
                dbSchemaServerInstanceList.Add(dbSchemaServerInstance);
            }

            return dbSchemaServerInstanceList;
        }

        /// <summary>
        /// Gets the database.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <returns>DBSchemaDBDefinition.</returns>
        public DBSchemaDBDefinition GetDB(string connectionString)
        {
            string dbServerName;
            string dbName;
            string dbUserID;
            string dbPassword;

            BreakConnectionString(connectionString, out dbServerName, out dbName, out dbUserID, out dbPassword);

            return GetDB(dbServerName, dbUserID, dbPassword, dbName);
        }

        /// <summary>
        /// Gets the database.
        /// </summary>
        /// <param name="dbServerName">Name of the database server.</param>
        /// <param name="dbUserID">The database user identifier.</param>
        /// <param name="dbPassword">The database password.</param>
        /// <param name="dbName">Name of the database.</param>
        /// <returns>DBSchemaDBDefinition.</returns>
        /// <exception cref="DBSchemaHandlerException">Unable to establish connection to specified database</exception>
        public DBSchemaDBDefinition GetDB(string dbServerName, string dbUserID, string dbPassword, string dbName)
        {
            // Default connection to master db of SQL Server
            var sqlConnectionString = BuildConnectionString(dbServerName, dbName, dbUserID, dbPassword);

            // Exec Query to retrive all DB Information information.
            var sql = @" Select

                            DATABASE_NAME			= SDB.[name],
                            DATABASE_SIZE			= convert(int,
                                                            case -- more than 2TB(maxint) worth of pages (by 8K each) can not fit an int...
                                                            when convert(bigint, sum(SMF.size)) >= 268435456
                                                            then null
                                                            else sum(SMF.size)*8 -- Convert from 8192 byte pages to Kb
                                                            end),
                            DATA_FILE_PATH		= (select physical_name from sys.master_files where database_id = SDB.dbid AND type_desc = 'ROWS'),
                            DATA_FILE_SIZE			= (select [size] from sys.master_files where database_id = SDB.dbid AND type_desc = 'ROWS'),
                            DATA_FILE_MAX_SIZE			= (select max_size from sys.master_files where database_id = SDB.dbid AND type_desc = 'ROWS'),
                            DATA_FILE_GROWTH			= (select growth from sys.master_files where database_id = SDB.dbid AND type_desc = 'ROWS'),
                            IS_PERCENTAGE_GROWTH		= (select is_percent_growth from sys.master_files where database_id = SDB.dbid AND type_desc = 'ROWS'),
                            LOG_FILE_PATH		= (select physical_name from sys.master_files where database_id = SDB.dbid AND type_desc = 'LOG'),
                            LOG_FILE_SIZE			= (select [size] from sys.master_files where database_id = SDB.dbid AND type_desc = 'LOG'),
                            LOG_FILE_MAX_SIZE			= (select max_size from sys.master_files where database_id = SDB.dbid AND type_desc = 'LOG'),
                            LOG_FILE_GROWTH			= (select growth from sys.master_files where database_id = SDB.dbid AND type_desc = 'LOG'),
                            IS_PERCENTAGE_LOG_FILE_GROWTH		= (select is_percent_growth from sys.master_files where database_id = SDB.dbid AND type_desc = 'LOG'),
                            DATE_CREATED				=  SDB.crdate

                            from
                            master..sysdatabases SDB,
                            sys.master_files SMF
                            Where
                            SDB.dbid = SMF.database_id
                            AND SMF.state = 0 -- ONLINE
                            AND has_dbaccess(db_name(SMF.database_id)) = 1 -- Only look at databases to which we have access
                            AND SDB.[name] = @DatabaseName

                            Group by SDB.[name],SDB.dbid,SDB.crdate";

            try
            {
                var dbHandler = new DBHandler(sqlConnectionString, DBHandlerType.DbHandlerMSSQL);

                var dbHandlerParameters = new DBHandlerParameter[1];
                dbHandlerParameters[0] = new DBHandlerParameter("DatabaseName", DBHandlerDataType.VarChar);
                dbHandlerParameters[0].Value = dbName;

                var sqlDBSchema = dbHandler.ExecuteQuery<_SQLDB>(sql, dbHandlerParameters, CommandType.Text).SingleOrDefault();

                DBSchemaDBDefinition dbSchemaDBInstance = new DBSchemaDBDefinition(dbName);

                if (sqlDBSchema != null)
                {
                    dbSchemaDBInstance.DatabaseSize = sqlDBSchema.DATABASE_SIZE;
                    dbSchemaDBInstance.DateCreated = sqlDBSchema.DATE_CREATED;
                    dbSchemaDBInstance.DataFilePath = sqlDBSchema.DATA_FILE_PATH;
                    dbSchemaDBInstance.DataFileSize = sqlDBSchema.DATA_FILE_SIZE;

                    if (sqlDBSchema.DATA_FILE_MAX_SIZE != -1)
                        dbSchemaDBInstance.DataFileMaxSize = sqlDBSchema.DATA_FILE_MAX_SIZE;

                    dbSchemaDBInstance.DataFileGrowth = sqlDBSchema.DATA_FILE_GROWTH;
                    dbSchemaDBInstance.IsPercentDataFileGrowth = sqlDBSchema.IS_PERCENTAGE_GROWTH;

                    dbSchemaDBInstance.LogFilePath = sqlDBSchema.LOG_FILE_PATH;
                    dbSchemaDBInstance.LogFileSize = sqlDBSchema.LOG_FILE_SIZE;

                    if (sqlDBSchema.LOG_FILE_MAX_SIZE != -1)
                        dbSchemaDBInstance.LogFileMaxSize = sqlDBSchema.LOG_FILE_MAX_SIZE;

                    dbSchemaDBInstance.LogFileGrowth = sqlDBSchema.LOG_FILE_GROWTH;
                    dbSchemaDBInstance.IsPercentLogFileGrowth = sqlDBSchema.IS_PERCENTAGE_LOG_FILE_GROWTH;
                }

                if (dbSchemaDBInstance != null)
                {
                    dbSchemaDBInstance.Tables = GetTableDefinitionListing(dbServerName, dbSchemaDBInstance.DatabaseName, dbUserID, dbPassword);

                    dbSchemaDBInstance.Procs = GetProcDefinitionListing(dbServerName, dbSchemaDBInstance.DatabaseName, dbUserID, dbPassword);

                    dbSchemaDBInstance.Triggers = GetTriggerDefinitionListing(dbServerName, dbSchemaDBInstance.DatabaseName, dbUserID, dbPassword);
                }

                dbHandler.Dispose();

                return dbSchemaDBInstance;
            }
            catch (System.Exception ex)
            {
                throw new DBSchemaHandlerException("Unable to establish connection to specified database", ex);
            }
        }

        /// <summary>
        /// Gets the database listing.
        /// </summary>
        /// <param name="dbServerName">Name of the database server.</param>
        /// <param name="dbUserID">The database user identifier.</param>
        /// <param name="dbPassword">The database password.</param>
        /// <returns>List{DBSchemaDBDefinition}.</returns>
        /// <exception cref="DBSchemaHandlerException">Unable to establish connection to specified database</exception>
        public List<DBSchemaDBDefinition> GetDBListing(string dbServerName, string dbUserID, string dbPassword)
        {
            var sqlConnectionString = BuildConnectionString(dbServerName, "MASTER", dbUserID, dbPassword);

            var sql = @" Select

                            DatabaseName			= SDB.[name],
                            DatabaseSize			= convert(int,
                                                            case -- more than 2TB(maxint) worth of pages (by 8K each) can not fit an int...
                                                            when convert(bigint, sum(SMF.size)) >= 268435456
                                                            then null
                                                            else sum(SMF.size)*8 -- Convert from 8192 byte pages to Kb
                                                            end),
                            DataFilePath		= (select physical_name from sys.master_files where database_id = SDB.dbid AND type_desc = 'ROWS'),
                            DataFileSize			= (select [size] from sys.master_files where database_id = SDB.dbid AND type_desc = 'ROWS'),
                            DataFileMaxSize			= (select max_size from sys.master_files where database_id = SDB.dbid AND type_desc = 'ROWS'),
                            DataFileGrowth			= (select growth from sys.master_files where database_id = SDB.dbid AND type_desc = 'ROWS'),
                            IsPercentDataFileGrowth		= (select is_percent_growth from sys.master_files where database_id = SDB.dbid AND type_desc = 'ROWS'),
                            LogFilePath		= (select physical_name from sys.master_files where database_id = SDB.dbid AND type_desc = 'LOG'),
                            LogFileSize			= (select [size] from sys.master_files where database_id = SDB.dbid AND type_desc = 'LOG'),
                            LogFileMaxSize			= (select max_size from sys.master_files where database_id = SDB.dbid AND type_desc = 'LOG'),
                            LogFileGrowth			= (select growth from sys.master_files where database_id = SDB.dbid AND type_desc = 'LOG'),
                            IsPercentLogFileGrowth		= (select is_percent_growth from sys.master_files where database_id = SDB.dbid AND type_desc = 'LOG'),
                            DateCreated				=  SDB.crdate

                            from
                            master..sysdatabases SDB,
                            sys.master_files SMF
                            Where
                            SDB.dbid = SMF.database_id
                            AND SMF.state = 0 -- ONLINE
                            AND has_dbaccess(db_name(SMF.database_id)) = 1 -- Only look at databases to which we have access

                            Group by SDB.[name],SDB.dbid,SDB.crdate";

            try
            {
                var dbHandler = new DBHandler(sqlConnectionString, DBHandlerType.DbHandlerMSSQL);

                return dbHandler.ExecuteQuery<DBSchemaDBDefinition>(sql, CommandType.Text).ToList();
            }
            catch (System.Exception ex)
            {
                throw new DBSchemaHandlerException("Unable to establish connection to specified database", ex);
            }
        }

        /// <summary>
        /// Gets the table definition.
        /// </summary>
        /// <param name="dbServerName">Name of the database server.</param>
        /// <param name="dbName">Name of the database.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="dbUserID">The database user identifier.</param>
        /// <param name="dbPassword">The database password.</param>
        /// <returns>DBSchemaTableDefinition.</returns>
        /// <exception cref="DBSchemaHandlerException">Unable to establish connection or retrieve information from specified database</exception>
        public DBSchemaTableDefinition GetTableDefinition(string dbServerName, string dbName, string tableName, string dbUserID, string dbPassword)
        {
            var sqlConnectionString = BuildConnectionString(dbServerName, dbName, dbUserID, dbPassword);

            //QUERY 1 : Retrieve detailed table definition Information from selected DB
            //QUERY 2 : Retrieve all constraints based on a column / table from the selected DB
            var sql1 = @"    Select

                                        TABLE_NAME			        =	SO.[name],
                                        COLUMN_NAME			        =	SC.[name],
                                        [TYPE]				        =	CD.[Data_Type],
                                        DEFAULT_VALUE		        =	CD.Column_Default,
                                        LENGTH				        =	CD.Character_Maximum_Length,
                                        ISNULLABLE			        =	CASE(CD.Is_Nullable)
						                                                WHEN 'YES'
							                                                THEN CAST(1 AS BIT)
						                                                ELSE
							                                                CAST(0 AS BIT)
						                                                END,

                                        ISIDENTITY			        =	CASE (Select 'True' FROM sys.identity_columns Where [Object_ID] = SO.ID AND Column_id = SC.colid )
						                                                WHEN 'True'
							                                                THEN CAST(1 AS BIT)
						                                                ELSE
							                                                CAST(0 AS BIT)
						                                                END,
                                        IDENTITY_SEEDVALUE	        =	(Select Seed_value FROM sys.identity_columns Where [Object_ID] = SO.ID AND Column_id = SC.colid ),
                                        IDENTITY_INCREMENTVALUE		=	(Select increment_value FROM sys.identity_columns Where [Object_ID] = SO.ID AND Column_id = SC.colid )

                                        from dbo.sysobjects SO, syscolumns SC, Information_Schema.Columns CD
                                        where SO.[type] ='U'
                                        AND SO.ID = SC.ID
                                        AND CD.Table_Name = SO.[name]
                                        AND CD.Column_Name = SC.[name]
                                        AND CD.Table_Name = @tableName

                                        order by SO.[name],SC.colorder asc

                                    ";

            var sql2 = @"   SELECT

                                        TABLE_NAME = FK.TABLE_NAME,
                                        COLUMN_NAME = CU.COLUMN_NAME,
                                        CONSTRAINT_TYPE = FK.Constraint_Type,
                                        CONSTRAINT_NAME = C.CONSTRAINT_NAME,
                                        RELATED_TABLE_NAME = PK.TABLE_NAME,
                                        RELATED_COLUMN_NAME = PT.COLUMN_NAME,
                                        POSITION = (

                                        SELECT i2.ORDINAL_POSITION
                                        FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS i1
                                        INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE i2 ON i1.CONSTRAINT_NAME = i2.CONSTRAINT_NAME
                                        WHERE i1.CONSTRAINT_TYPE = 'PRIMARY KEY'
                                        and i1.TABLE_NAME = FK.TABLE_NAME and i2.COLUMN_NAME = CU.COLUMN_NAME
                                        ),
                                        RELATED_POSTION = (

                                        SELECT i2.ORDINAL_POSITION
                                        FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS i1
                                        INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE i2 ON i1.CONSTRAINT_NAME = i2.CONSTRAINT_NAME
                                        WHERE i1.CONSTRAINT_TYPE = 'PRIMARY KEY'
                                        and i1.TABLE_NAME = PK.TABLE_NAME and i2.COLUMN_NAME = PT.COLUMN_NAME

                                        )

                                        FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS C
                                        INNER JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS FK ON C.CONSTRAINT_NAME = FK.CONSTRAINT_NAME
                                        INNER JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS PK ON C.UNIQUE_CONSTRAINT_NAME = PK.CONSTRAINT_NAME
                                        INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE CU ON C.CONSTRAINT_NAME = CU.CONSTRAINT_NAME
                                        INNER JOIN (
                                        SELECT TOP 100000 i1.TABLE_NAME, i2.COLUMN_NAME, i2.ORDINAL_POSITION
                                        FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS i1
                                        INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE i2 ON i1.CONSTRAINT_NAME = i2.CONSTRAINT_NAME
                                        WHERE i1.CONSTRAINT_TYPE = 'PRIMARY KEY' order by i2.ORDINAL_POSITION
                                        ) PT ON PT.TABLE_NAME = PK.TABLE_NAME AND CU.table_name = @tableName

                                        UNION

                                        SELECT

                                        TABLE_NAME			=	CU.table_name,
                                        COLUMN_NAME			=	CU.Column_Name,
                                        CONSTRAINT_TYPE		=	TC.Constraint_Type,
                                        CONSTRAINT_NAME		=	CU.constraint_Name,
                                        RELATED_TABLE_NAME		=	'',
                                        RELATED_COLUMN_NAME		=	'',
                                        POSITION = (

                                        SELECT i2.ORDINAL_POSITION
                                        FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS i1
                                        INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE i2 ON i1.CONSTRAINT_NAME = i2.CONSTRAINT_NAME
                                        WHERE i1.CONSTRAINT_TYPE = 'PRIMARY KEY'
                                        and i1.TABLE_NAME = CU.table_name and i2.COLUMN_NAME = CU.COLUMN_NAME
                                        ),
                                        RELATED_POSTION = ''

                                        FROM INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE CU ,INFORMATION_SCHEMA.TABLE_CONSTRAINTS TC
                                        WHERE CU.Constraint_Name = TC.Constraint_Name AND TC.Constraint_Type = 'PRIMARY KEY'
                                        AND CU.table_name = @tableName

                                        order by CONSTRAINT_NAME, TABLE_NAME, POSITION, RELATED_POSTION";

            try
            {
                var dbHandler = new DBHandler(sqlConnectionString, DBHandlerType.DbHandlerMSSQL);

                var dbHandlerParameters = new DBHandlerParameter[1];
                dbHandlerParameters[0] = new DBHandlerParameter("tableName", DBHandlerDataType.VarChar);
                dbHandlerParameters[0].Value = tableName;

                var sqlTableDefinitions = dbHandler.ExecuteQuery<_SQLTableDefinition>(sql1, dbHandlerParameters, CommandType.Text);
                var sqlConstraints = dbHandler.ExecuteQuery<_SQLConstraint>(sql2, dbHandlerParameters, CommandType.Text);

                var dbSchemaTableDefinition = new DBSchemaTableDefinition(tableName);

                //Iterate through filtered list for table column information.
                foreach (var table in sqlTableDefinitions)
                {
                    var dbSchemaTableColumnDefinition = new DBSchemaTableColumnDefinition(table.COLUMN_NAME);

                    dbSchemaTableColumnDefinition.DataType = table.TYPE.ToDBHandlerDataType();
                    dbSchemaTableColumnDefinition.DefaultValue = table.DEFAULT_VALUE;
                    dbSchemaTableColumnDefinition.Length = table.LENGTH;
                    dbSchemaTableColumnDefinition.IsNullable = table.ISNULLABLE;
                    dbSchemaTableColumnDefinition.IsIdentity = table.ISIDENTITY;
                    dbSchemaTableColumnDefinition.IdentitySeed = table.IDENTITY_SEEDVALUE;
                    dbSchemaTableColumnDefinition.IdentityIncrement = table.IDENTITY_INCREMENTVALUE;

                    var constraints = sqlConstraints.Where(c => c.TABLE_NAME == tableName && c.COLUMN_NAME == table.COLUMN_NAME);

                    foreach (var constraint in constraints)
                    {
                        var dbSchemaConstraintDefinition = new DBSchemaConstraintDefinition(constraint.CONSTRAINT_NAME);
                        dbSchemaConstraintDefinition.Constraint = constraint.CONSTRAINT_TYPE.ToConstraintType();
                        dbSchemaConstraintDefinition.TableName = constraint.TABLE_NAME;
                        dbSchemaConstraintDefinition.ColumnName = constraint.COLUMN_NAME;
                        dbSchemaConstraintDefinition.RelatedTableName = constraint.RELATED_TABLE_NAME;
                        dbSchemaConstraintDefinition.RelatedColumnName = constraint.RELATED_TABLE_NAME;
                        dbSchemaTableColumnDefinition.DBSchemaConstraintDefinitionList.Add(dbSchemaConstraintDefinition);
                    }

                    // Add Constraint to table definition
                    dbSchemaTableDefinition.ColumnDefinitionList.Add(dbSchemaTableColumnDefinition);
                }

                dbHandler.Dispose();

                return dbSchemaTableDefinition;
            }
            catch (System.Exception ex)
            {
                throw new DBSchemaHandlerException("Unable to establish connection or retrive information from specified database", ex);
            }
        }

        /// <summary>
        /// Gets the table definition listing.
        /// </summary>
        /// <param name="dbServerName">Name of the database server.</param>
        /// <param name="dbName">Name of the database.</param>
        /// <param name="dbUserID">The database user identifier.</param>
        /// <param name="dbPassword">The database password.</param>
        /// <returns>DBSchemaTableDefinitionCollection.</returns>
        /// <exception cref="DBSchemaHandlerException">Unable to establish connection or retrieve information from specified database</exception>
        public DBSchemaTableDefinitionCollection GetTableDefinitionListing(string dbServerName, string dbName,
            string dbUserID, string dbPassword)
        {
            //Connection string to DB.
            var sqlConnectionString = BuildConnectionString(dbServerName, dbName, dbUserID, dbPassword);

            //QUERY 1 : Retrieve all table names from selected DB
            //QUERY 2 : Retrieve detailed table definition Information from selected DB
            //QUERY 3 : Retrieve all constraints based on a column / table from the selected DB
            var sql = @"Select TABLE_NAME = [name]  from dbo.sysobjects where type ='U' order by name asc ;";

            var sql1 = @"Select

                            TABLE_NAME			        =	SO.[name],
                            COLUMN_NAME			        =	SC.[name],
                            [TYPE]				        =	CD.[Data_Type],
                            DEFAULT_VALUE		        =	CD.Column_Default,
                            LENGTH				        =	CD.Character_Maximum_Length,
                            ISNULLABLE			        =	CASE(CD.Is_Nullable)
						                                    WHEN 'YES'
							                                    THEN CAST(1 AS BIT)
						                                    ELSE
							                                    CAST(0 AS BIT)
						                                    END,

                            ISIDENTITY			        =	CASE (Select 'True' FROM sys.identity_columns Where [Object_ID] = SO.ID AND Column_id = SC.colid )
						                                    WHEN 'True'
							                                    THEN CAST(1 AS BIT)
						                                    ELSE
							                                    CAST(0 AS BIT)
						                                    END,
                            IDENTITY_SEEDVALUE	        =	(Select Seed_value FROM sys.identity_columns Where [Object_ID] = SO.ID AND Column_id = SC.colid ),
                            IDENTITY_INCREMENTVALUE		=	(Select increment_value FROM sys.identity_columns Where [Object_ID] = SO.ID AND Column_id = SC.colid )

                            from dbo.sysobjects SO, syscolumns SC, Information_Schema.Columns CD
                            where SO.[type] ='U'
                            AND SO.ID = SC.ID
                            AND CD.Table_Name = SO.[name]
                            AND CD.Column_Name = SC.[name]
                            order by SO.[name],SC.colorder asc;";

            var sql2 = @"
                            SELECT

                            TABLE_NAME = FK.TABLE_NAME,
                            COLUMN_NAME = CU.COLUMN_NAME,
                            CONSTRAINT_TYPE = FK.Constraint_Type,
                            CONSTRAINT_NAME = C.CONSTRAINT_NAME,
                            RELATED_TABLE_NAME = PK.TABLE_NAME,
                            RELATED_COLUMN_NAME = PT.COLUMN_NAME,
                            POSITION = (

                            SELECT i2.ORDINAL_POSITION
                            FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS i1
                            INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE i2 ON i1.CONSTRAINT_NAME = i2.CONSTRAINT_NAME
                            WHERE i1.CONSTRAINT_TYPE = 'PRIMARY KEY'
                            and i1.TABLE_NAME = FK.TABLE_NAME and i2.COLUMN_NAME = CU.COLUMN_NAME
                            ),
                            RELATED_POSTION = (

                            SELECT i2.ORDINAL_POSITION
                            FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS i1
                            INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE i2 ON i1.CONSTRAINT_NAME = i2.CONSTRAINT_NAME
                            WHERE i1.CONSTRAINT_TYPE = 'PRIMARY KEY'
                            and i1.TABLE_NAME = PK.TABLE_NAME and i2.COLUMN_NAME = PT.COLUMN_NAME

                            )

                            FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS C
                            INNER JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS FK ON C.CONSTRAINT_NAME = FK.CONSTRAINT_NAME
                            INNER JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS PK ON C.UNIQUE_CONSTRAINT_NAME = PK.CONSTRAINT_NAME
                            INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE CU ON C.CONSTRAINT_NAME = CU.CONSTRAINT_NAME
                            INNER JOIN (
                            SELECT TOP 100000 i1.TABLE_NAME, i2.COLUMN_NAME, i2.ORDINAL_POSITION
                            FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS i1
                            INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE i2 ON i1.CONSTRAINT_NAME = i2.CONSTRAINT_NAME
                            WHERE i1.CONSTRAINT_TYPE = 'PRIMARY KEY' order by i2.ORDINAL_POSITION
                            ) PT ON PT.TABLE_NAME = PK.TABLE_NAME

                            UNION

                            SELECT

                            TABLE_NAME			=	CU.table_name,
                            COLUMN_NAME			=	CU.Column_Name,
                            CONSTRAINT_TYPE		=	TC.Constraint_Type,
                            CONSTRAINT_NAME		=	CU.constraint_Name,
                            RELATED_TABLE_NAME		=	'',
                            RELATED_COLUMN_NAME		=	'',
                            POSITION = (

                            SELECT i2.ORDINAL_POSITION
                            FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS i1
                            INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE i2 ON i1.CONSTRAINT_NAME = i2.CONSTRAINT_NAME
                            WHERE i1.CONSTRAINT_TYPE = 'PRIMARY KEY'
                            and i1.TABLE_NAME = CU.table_name and i2.COLUMN_NAME = CU.COLUMN_NAME
                            ),
                            RELATED_POSTION = ''

                            FROM INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE CU ,INFORMATION_SCHEMA.TABLE_CONSTRAINTS TC
                            WHERE CU.Constraint_Name = TC.Constraint_Name AND TC.Constraint_Type = 'PRIMARY KEY'

                            order by CONSTRAINT_NAME, TABLE_NAME, POSITION, RELATED_POSTION";

            try
            {
                var dbHandler = new DBHandler(sqlConnectionString, DBHandlerType.DbHandlerMSSQL);

                var sqlTables = dbHandler.ExecuteQuery<_SQLTable>(sql, CommandType.Text);
                var sqlTableDefinitions = dbHandler.ExecuteQuery<_SQLTableDefinition>(sql1, CommandType.Text);
                var sqlConstraints = dbHandler.ExecuteQuery<_SQLConstraint>(sql2, CommandType.Text);

                var dbSchemaTableDefinitionList = new DBSchemaTableDefinitionCollection();

                foreach (var aTable in sqlTables)
                {
                    var dbSchemaTableDefinition = new DBSchemaTableDefinition(aTable.TABLE_NAME);

                    var aTableDefinitions = sqlTableDefinitions.Where(c => c.TABLE_NAME == aTable.TABLE_NAME);

                    foreach (var aTableDefinition in aTableDefinitions)
                    {
                        var dbSchemaTableColumnDefinition = new DBSchemaTableColumnDefinition(aTableDefinition.COLUMN_NAME);

                        dbSchemaTableColumnDefinition.DataType = aTableDefinition.TYPE.ToDBHandlerDataType();
                        dbSchemaTableColumnDefinition.DefaultValue = aTableDefinition.DEFAULT_VALUE;
                        dbSchemaTableColumnDefinition.Length = aTableDefinition.LENGTH;
                        dbSchemaTableColumnDefinition.IsNullable = aTableDefinition.ISNULLABLE;
                        dbSchemaTableColumnDefinition.IsIdentity = aTableDefinition.ISIDENTITY;
                        dbSchemaTableColumnDefinition.IdentitySeed = aTableDefinition.IDENTITY_SEEDVALUE;
                        dbSchemaTableColumnDefinition.IdentityIncrement = aTableDefinition.IDENTITY_INCREMENTVALUE;

                        var aTableConstraintDefinitions = sqlConstraints.Where(c => c.TABLE_NAME == aTable.TABLE_NAME && c.COLUMN_NAME == aTableDefinition.COLUMN_NAME);

                        foreach (var aTableConstraintDefinition in aTableConstraintDefinitions)
                        {
                            var dbSchemaConstraintDefinition = new DBSchemaConstraintDefinition(aTableConstraintDefinition.CONSTRAINT_NAME);

                            dbSchemaConstraintDefinition.Constraint = aTableConstraintDefinition.CONSTRAINT_TYPE.ToConstraintType();
                            dbSchemaConstraintDefinition.TableName = aTableConstraintDefinition.TABLE_NAME;
                            dbSchemaConstraintDefinition.ColumnName = aTableConstraintDefinition.COLUMN_NAME;
                            dbSchemaConstraintDefinition.RelatedTableName = aTableConstraintDefinition.RELATED_TABLE_NAME;
                            dbSchemaConstraintDefinition.RelatedColumnName = aTableConstraintDefinition.RELATED_COLUMN_NAME;
                            dbSchemaTableColumnDefinition.DBSchemaConstraintDefinitionList.Add(dbSchemaConstraintDefinition);
                        }

                        dbSchemaTableDefinition.ColumnDefinitionList.Add(dbSchemaTableColumnDefinition);
                    }

                    dbSchemaTableDefinitionList.Add(dbSchemaTableDefinition);
                }

                dbHandler.Dispose();

                return dbSchemaTableDefinitionList;
            }
            catch (System.Exception ex)
            {
                throw new DBSchemaHandlerException(
                    "Unable to establish connection or retrieve information from specified database", ex);
            }
        }

        /// <summary>
        /// Gets the proc definition.
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
            return GetProcDefinitionListing(dbServerName, dbName, dbUserID, dbPassword)[procName];
        }

        /// <summary>
        /// Gets the proc definition listing.
        /// </summary>
        /// <param name="dbServerName">Name of the database server.</param>
        /// <param name="dbName">Name of the database.</param>
        /// <param name="dbUserID">The database user identifier.</param>
        /// <param name="dbPassword">The database password.</param>
        /// <returns>DBSchemaStoredProcedureDefinitionCollection.</returns>
        /// <exception cref="DBSchemaHandlerException">Unable to establish connection or retrive information from specified database</exception>
        public DBSchemaStoredProcedureDefinitionCollection GetProcDefinitionListing(string dbServerName, string dbName,
            string dbUserID, string dbPassword)
        {
            //Connection string to DB.
            var sqlConnectionString = BuildConnectionString(dbServerName, dbName, dbUserID, dbPassword);

            var sql = @"SELECT
                            PROC_NAME = XXX.[name],
                            PROC_BODY = (SELECT Top 1 [TEXT] FROM Syscomments
                                        WHERE id = (SELECT Top 1 id FROM sysobjects WHERE xtype='p' and NAME = XXX.[name]))

                            FROM sys.procedures XXX where [type] = 'P' and is_ms_shipped = 0 and [name] not like 'sp[_]%diagram%'";

            var sql1 = @"SELECT
                        PROC_NAME = SO.name,
                        PARAMENTER_ID = P.parameter_id,
                        PARAMETER_NAME = P.name,
                        PARAMETER_DATA_TYPE = IIF ((SELECT is_table_type FROM sys.types WHERE name = TYPE_NAME(P.user_type_id)) = 1, 'structured', TYPE_NAME(P.user_type_id)),
                        PARAMETER_MAX_BYTES = CAST(P.max_length as int),
                        ISOUTPUTPARAMETER = P.is_output
                        FROM sys.objects AS SO
                        INNER JOIN sys.parameters AS P
                        ON SO.OBJECT_ID = P.OBJECT_ID
                        WHERE SO.OBJECT_ID IN ( SELECT OBJECT_ID
                        FROM sys.objects
                        WHERE TYPE IN ('P','FN'))";

            var dbSchemaStoredProcedureDefinitionCollection = new DBSchemaStoredProcedureDefinitionCollection();

            try
            {
                // Create DBHandler connection to database.
                var dbHandler = new DBHandler(sqlConnectionString, DBHandlerType.DbHandlerMSSQL);

                var procDefinitions = dbHandler.ExecuteQuery<_SQLProc>(sql, CommandType.Text);
                var procParameterDefinitions = dbHandler.ExecuteQuery<_SQLProcParameter>(sql1, CommandType.Text);

                foreach (var aProcDefinition in procDefinitions)
                {
                    var dbSchemaStoredProcedureDefinition = new DBSchemaStoredProcedureDefinition(aProcDefinition.PROC_NAME);
                    dbSchemaStoredProcedureDefinition.ProcedureBody = aProcDefinition.PROC_BODY;

                    var aProcsParameterDefinitions = procParameterDefinitions.Where(c => c.PROC_NAME == aProcDefinition.PROC_NAME);

                    foreach (var aProcsParameterDefinition in aProcsParameterDefinitions)
                    {
                        var dbSchemaStoredProcedureParameterDefinition = new DBSchemaStoredProcedureParameterDefinition();
                        dbSchemaStoredProcedureParameterDefinition.ParameterID = aProcsParameterDefinition.PARAMENTER_ID;
                        dbSchemaStoredProcedureParameterDefinition.ParameterName = aProcsParameterDefinition.PARAMETER_NAME;
                        dbSchemaStoredProcedureParameterDefinition.DataType = aProcsParameterDefinition.PARAMETER_DATA_TYPE.ToDBHandlerDataType();
                        dbSchemaStoredProcedureParameterDefinition.Size = aProcsParameterDefinition.PARAMETER_MAX_BYTES;
                        dbSchemaStoredProcedureParameterDefinition.IsOutputParameter = aProcsParameterDefinition.ISOUTPUTPARAMETER;

                        dbSchemaStoredProcedureDefinition.Parameters.Add(dbSchemaStoredProcedureParameterDefinition);
                    }

                    dbSchemaStoredProcedureDefinitionCollection.Add(dbSchemaStoredProcedureDefinition);
                }
            }
            catch (System.Exception ex)
            {
                throw new DBSchemaHandlerException(
                    "Unable to establish connection or retrieve information from specified database", ex);
            }

            return dbSchemaStoredProcedureDefinitionCollection;
        }

        /// <summary>
        /// Gets the trigger definition.
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
            return GetTriggerDefinitionListing(dbServerName, dbName, dbUserID, dbPassword)[triggerName];
        }

        /// <summary>
        /// Gets the trigger definition listing.
        /// </summary>
        /// <param name="dbServerName">Name of the database server.</param>
        /// <param name="dbName">Name of the database.</param>
        /// <param name="dbUserID">The database user identifier.</param>
        /// <param name="dbPassword">The database password.</param>
        /// <returns>DBSchemaTriggerDefinitionCollection.</returns>
        /// <exception cref="DBSchemaHandlerException">Unable to establish connection or retrieve information from specified database</exception>
        public DBSchemaTriggerDefinitionCollection GetTriggerDefinitionListing(string dbServerName, string dbName,
            string dbUserID, string dbPassword)
        {
            //Connection string to DB.
            var sqlConnectionString = BuildConnectionString(dbServerName, dbName, dbUserID, dbPassword);

            var sql = @"SELECT TRIGGER_NAME = SO.Name,
	                               TRIGGER_OWNER = USER_NAME(SO.uid),
	                               TABLE_NAME = OBJECT_NAME(SO.parent_obj),
                                   TRIGGER_DEFINITION = (select object_definition(object_id) from sys.triggers where name = SO.name),
	                               IS_UPDATE = OBJECTPROPERTY( SO.id, 'ExecIsUpdateTrigger'),
                                   IS_DELETE = OBJECTPROPERTY( SO.id, 'ExecIsDeleteTrigger'),
                                   IS_INSERT = OBJECTPROPERTY( SO.id, 'ExecIsInsertTrigger'),
	                               IS_AFTER = OBJECTPROPERTY( SO.id, 'ExecIsAfterTrigger'),
                                   IS_INSTEAD_OF = OBJECTPROPERTY( SO.id, 'ExecIsInsteadOfTrigger'),
	                               TRStatus = CASE OBJECTPROPERTY(SO.id, 'ExecIsTriggerDisabled') WHEN 1 THEN 'Disabled' ELSE 'Enabled' END
                            FROM Sysobjects SO
                            WHERE type = 'TR'";

            var dbSchemaTriggerDefinitionCollection = new DBSchemaTriggerDefinitionCollection();

            try
            {
                var dbHandler = new DBHandler(sqlConnectionString, DBHandlerType.DbHandlerMSSQL);

                var trSQLDefinitions = dbHandler.ExecuteQuery<_SQLTrigger>(sql, CommandType.Text);

                foreach (var trSQLDefinition in trSQLDefinitions)
                {
                    var trDefinition = new DBSchemaTriggerDefinition(trSQLDefinition.TRIGGER_NAME);

                    trDefinition.TriggerOwner = trSQLDefinition.TRIGGER_OWNER;
                    trDefinition.TableName = trSQLDefinition.TABLE_NAME;
                    trDefinition.TriggerDefinition = trSQLDefinition.TRIGGER_DEFINITION;
                    trDefinition.IsUpdate = trSQLDefinition.IS_UPDATE;
                    trDefinition.IsDelete = trSQLDefinition.IS_DELETE;
                    trDefinition.IsInsert = trSQLDefinition.IS_INSERT;
                    trDefinition.IsAfter = trSQLDefinition.IS_AFTER;
                    trDefinition.IsInsteadOf = trSQLDefinition.IS_INSTEAD_OF;

                    dbSchemaTriggerDefinitionCollection.Add(trDefinition);
                }
            }
            catch (System.Exception ex)
            {
                throw new DBSchemaHandlerException(
                    "Unable to establish connection or retrieve information from specified database", ex);
            }

            return dbSchemaTriggerDefinitionCollection;
        }

        /// <summary>
        /// Creates the database.
        /// </summary>
        /// <param name="dbServerName">Name of the database server.</param>
        /// <param name="dbUserID">The database user identifier.</param>
        /// <param name="dbPassword">The database password.</param>
        /// <param name="dbSchemaDBInstance">The database schema database instance.</param>
        public void CreateDatabase(string dbServerName, string dbUserID, string dbPassword,
            DBSchemaDBDefinition dbSchemaDBInstance)
        {
            // Build connection string
            var sqlConnectionString = BuildConnectionString(dbServerName, "Master", dbUserID, dbPassword);
            CreateDatabase(sqlConnectionString, dbSchemaDBInstance);
        }

        /// <summary>
        /// Creates the database.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="dbSchemaDBInstance">The database schema database instance.</param>
        /// <exception cref="DBSchemaHandlerException">
        /// SqlException - Unable to create database
        /// or
        /// Unspecified error in create database
        /// </exception>
        public void CreateDatabase(string connectionString, DBSchemaDBDefinition dbSchemaDBInstance)
        {
            // Create Kill Script for Exclusive access of Model DB of SQL server.
            var activeConnectionsSQL = ActiveConnectionsSQL();

            // Create DB Script
            var sql = CreateDatabaseSQL(dbSchemaDBInstance);

            try
            {
                //// Create DBHandler connection to database.
                var dbHandler = new DBHandler(connectionString, DBHandlerType.DbHandlerMSSQL);

                // Get active connections which are currently present to the model database.
                var activeConnections = dbHandler.ExecuteQuery<_SQLActiveConnection>(activeConnectionsSQL, CommandType.Text);

                // Iterate thorough active connections to Model DB and kill them
                // Build Kill SQL
                var killSQL = new StringBuilder();
                foreach (var activeConnection in activeConnections)
                {
                    killSQL.Append("Kill " + activeConnection.ACTIVE_CONNECTION_ID + "; ");
                }
                // Execute kill sql
                if (killSQL.ToString() != "")
                {
                    dbHandler.ExecuteNonQuery(killSQL.ToString(), CommandType.Text);
                }

                // Execute create DB
                dbHandler.ExecuteNonQuery(sql, CommandType.Text);

                // Build create table scripts for tables in DB
                var createTableSQL = new StringBuilder();

                // Switch Databases - switch to the created DB
                createTableSQL.Append("Use [" + dbSchemaDBInstance.DatabaseName + "];");
                foreach (var dbSchemaTableDefinition in dbSchemaDBInstance.Tables)
                {
                    createTableSQL.Append(CreateTableSQL(dbSchemaTableDefinition) + ";");
                }

                if (createTableSQL.ToString() != "")
                {
                    // Execute create tables
                    dbHandler.ExecuteNonQuery(createTableSQL.ToString(), CommandType.Text);
                }

                #region ER Constraint Definition Updates

                // Apply the ER constraints now that the table have been made in the system.
                var erConstraintBuilder = new StringBuilder();

                // Switch Databases - switch to the created DB
                erConstraintBuilder.Append("Use [" + dbSchemaDBInstance.DatabaseName + "];");

                foreach (var dbSchemaTableDefinition in dbSchemaDBInstance.Tables)
                {
                    foreach (var dbSchemaTableColumnDefinition in dbSchemaTableDefinition.ColumnDefinitionList)
                    {
                        foreach (
                            var dbSchemaConstraintDefinition in
                                dbSchemaTableColumnDefinition.DBSchemaConstraintDefinitionList)
                        {
                            if (dbSchemaConstraintDefinition.Constraint == ConstraintType.ForeignKey)
                            {
                                // Create relationship
                                erConstraintBuilder.Append(CreateERConstraintSQL(dbSchemaConstraintDefinition) + ";");
                            }
                            else if (dbSchemaConstraintDefinition.Constraint == ConstraintType.Unique)
                            {
                                // Create unique constraint
                            }
                        }
                    }
                }

                if (erConstraintBuilder.ToString() != "")
                {
                    // Execute create tables
                    dbHandler.ExecuteNonQuery(erConstraintBuilder.ToString(), CommandType.Text);
                }

                #endregion ER Constraint Definition Updates

                #region Stored Proc Update

                string server;
                string dbName;
                string userID;
                string password;

                BreakConnectionString(connectionString, out server, out dbName, out userID, out password);

                connectionString = BuildConnectionString(server, dbSchemaDBInstance.DatabaseName, userID, password);

                //Reset DB Handler with connection to the newly created database.
                dbHandler = new DBHandler(connectionString, DBHandlerType.DbHandlerMSSQL);

                foreach (var dbSchemaStoredProcedureDefinition in dbSchemaDBInstance.Procs)
                {
                    try
                    {
                        // Execute create proc
                        dbHandler.ExecuteNonQuery(dbSchemaStoredProcedureDefinition.ProcedureBody, CommandType.Text);
                    }
                    catch
                    {
                        // Do nothing. Skip error based proc.
                    }
                }

                #endregion Stored Proc Update

                #region Trigger Update

                foreach (var dbSchemaTriggerDefinition in dbSchemaDBInstance.Triggers)
                {
                    try
                    {
                        // Execute create trigger
                        dbHandler.ExecuteNonQuery(dbSchemaTriggerDefinition.TriggerDefinition, CommandType.Text);
                    }
                    catch
                    {
                        // Do nothing. Skip error based proc
                    }
                }

                #endregion Trigger Update

                dbHandler.Dispose();
            }
            catch (SqlException ex)
            {
                throw new DBSchemaHandlerException("SqlException - Unable to create database", ex);
            }
            catch (System.Exception ex)
            {
                throw new DBSchemaHandlerException("Unspecified error in create database", ex);
            }
        }

        /// <summary>
        /// Alters the database.
        /// </summary>
        /// <param name="dbServerName">Name of the database server.</param>
        /// <param name="dbUserID">The database user identifier.</param>
        /// <param name="dbPassword">The database password.</param>
        /// <param name="dbSchemaDBInstance">The database schema database instance.</param>
        public void AlterDatabase(string dbServerName, string dbUserID, string dbPassword,
            DBSchemaDBDefinition dbSchemaDBInstance)
        {
            // Build connection string
            var sqlConnectionString = BuildConnectionString(dbServerName, "Master", dbUserID, dbPassword);
            AlterDatabase(sqlConnectionString, dbSchemaDBInstance);
        }

        /// <summary>
        /// Alters the database.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="dbSchemaDBInstance">The database schema database instance.</param>
        /// <exception cref="DBSchemaHandlerException">
        /// SqlException - Unable to create database
        /// or
        /// Unspecified error in create database
        /// </exception>
        public void AlterDatabase(string connectionString, DBSchemaDBDefinition dbSchemaDBInstance)
        {
            // Build Alter DB SQL
            var sql = AlterDatabaseSQL(dbSchemaDBInstance);

            try
            {
                // Create DBHandler connection to database.
                var dbHandler = new DBHandler(connectionString, DBHandlerType.DbHandlerMSSQL);

                if (sql != "")
                {
                    dbHandler.ExecuteNonQuery(sql, CommandType.Text);
                }

                dbHandler.Dispose();
            }
            catch (SqlException ex)
            {
                throw new DBSchemaHandlerException("SqlException - Unable to create database", ex);
            }
            catch (System.Exception ex)
            {
                throw new DBSchemaHandlerException("Unspecified error in create database", ex);
            }
        }

        /// <summary>
        /// Drops the database.
        /// </summary>
        /// <param name="dbServerName">Name of the database server.</param>
        /// <param name="dbUserID">The database user identifier.</param>
        /// <param name="dbPassword">The database password.</param>
        /// <param name="dbName">Name of the database.</param>
        public void DropDatabase(string dbServerName, string dbUserID, string dbPassword, string dbName)
        {
            // Build connection string
            var sqlConnectionString = BuildConnectionString(dbServerName, "Master", dbUserID, dbPassword);
            DropDatabase(sqlConnectionString, dbName);
        }

        /// <summary>
        /// Drops the database.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="dbName">Name of the database.</param>
        /// <exception cref="DBSchemaHandlerException">
        /// SqlException - Unable to drop database
        /// or
        /// Unspecified error in Drop Database
        /// </exception>
        public void DropDatabase(string connectionString, string dbName)
        {
            // Build drop db sql
            var sql = DropDatabaseSQL(dbName);

            try
            {
                // Create DBHandler connection to database.
                var dbHandler = new DBHandler(connectionString, DBHandlerType.DbHandlerMSSQL);
                dbHandler.ExecuteNonQuery(sql, CommandType.Text);
                dbHandler.Dispose();
            }
            catch (SqlException ex)
            {
                throw new DBSchemaHandlerException("SqlException - Unable to drop database", ex);
            }
            catch (System.Exception ex)
            {
                throw new DBSchemaHandlerException("Unspecified error in Drop Database", ex);
            }
        }

        /// <summary>
        /// Creates the table.
        /// </summary>
        /// <param name="dbServerName">Name of the database server.</param>
        /// <param name="dbName">Name of the database.</param>
        /// <param name="dbUserID">The database user identifier.</param>
        /// <param name="dbPassword">The database password.</param>
        /// <param name="dbSchemaTableDefinition">The database schema table definition.</param>
        public void CreateTable(string dbServerName, string dbName, string dbUserID, string dbPassword,
            DBSchemaTableDefinition dbSchemaTableDefinition)
        {
            // Build connection string.
            var sqlConnectionString = BuildConnectionString(dbServerName, dbName, dbUserID, dbPassword);
            CreateTable(sqlConnectionString, dbSchemaTableDefinition);
        }

        /// <summary>
        /// Creates the table.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="dbSchemaTableDefinition">The database schema table definition.</param>
        /// <exception cref="DBSchemaHandlerException">
        /// SqlException - Unable to create table
        /// or
        /// Unspecified error in CreateTable
        /// </exception>
        public void CreateTable(string connectionString, DBSchemaTableDefinition dbSchemaTableDefinition)
        {
            //Build create table sql
            var sql = CreateTableSQL(dbSchemaTableDefinition);

            try
            {
                // Create DBHandler connection to database.
                var dbHandler = new DBHandler(connectionString, DBHandlerType.DbHandlerMSSQL);
                dbHandler.ExecuteNonQuery(sql, CommandType.Text);
                dbHandler.Dispose();
            }
            catch (SqlException ex)
            {
                throw new DBSchemaHandlerException("SqlException - Unable to create table", ex);
            }
            catch (System.Exception ex)
            {
                throw new DBSchemaHandlerException("Unspecified error in CreateTable", ex);
            }
        }

        public void AlterTable(string dbServerName, string dbName, string dbUserID, string dbPassword,
            DBSchemaTableDefinition dbOldSchemaTableDefinition, DBSchemaTableDefinition dbNewSchemaTableDefinition,
            DBSchemaAlterTableOperation dbSchemaAlterTableOperation)
        {
            // Build connection string.
            var sqlConnectionString = BuildConnectionString(dbServerName, dbName, dbUserID, dbPassword);
            AlterTable(sqlConnectionString, dbOldSchemaTableDefinition, dbNewSchemaTableDefinition,
                dbSchemaAlterTableOperation);
        }

        public void AlterTable(string connectionString, DBSchemaTableDefinition dbOldSchemaTableDefinition,
            DBSchemaTableDefinition dbNewSchemaTableDefinition, DBSchemaAlterTableOperation dbSchemaAlterTableOperation)
        {
            // Build alter table sql
            var sql = AlterTableSQL(dbOldSchemaTableDefinition, dbNewSchemaTableDefinition, dbSchemaAlterTableOperation);

            try
            {
                // Create DBHandler connection to database.
                var dbHandler = new DBHandler(connectionString, DBHandlerType.DbHandlerMSSQL);
                dbHandler.ExecuteNonQuery(sql, CommandType.Text);
                dbHandler.Dispose();
            }
            catch (SqlException ex)
            {
                throw new DBSchemaHandlerException("SqlException - Unable to alter table", ex);
            }
            catch (System.Exception ex)
            {
                throw new DBSchemaHandlerException("Unspecified error in AlterTable", ex);
            }
        }

        /// <summary>
        /// Drops the table.
        /// </summary>
        /// <param name="dbServerName">Name of the database server.</param>
        /// <param name="dbName">Name of the database.</param>
        /// <param name="dbUserID">The database user identifier.</param>
        /// <param name="dbPassword">The database password.</param>
        /// <param name="tableName">Name of the table.</param>
        public void DropTable(string dbServerName, string dbName, string dbUserID, string dbPassword, string tableName)
        {
            // Build connection string.
            var sqlConnectionString = BuildConnectionString(dbServerName, dbName, dbUserID, dbPassword);
            DropTable(sqlConnectionString, tableName);
        }

        /// <summary>
        /// Drops the table.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <exception cref="DBSchemaHandlerException">
        /// SqlException - Unable to drop table
        /// or
        /// Unspecified error in DropTable
        /// </exception>
        public void DropTable(string connectionString, string tableName)
        {
            // Build drop sql
            var sql = DropTableSQL(tableName);

            try
            {
                // Create DBHandler connection to database.
                var dbHandler = new DBHandler(connectionString, DBHandlerType.DbHandlerMSSQL);
                dbHandler.ExecuteNonQuery(sql, CommandType.Text);
                dbHandler.Dispose();
            }
            catch (SqlException ex)
            {
                throw new DBSchemaHandlerException("SqlException - Unable to drop table", ex);
            }
            catch (System.Exception ex)
            {
                throw new DBSchemaHandlerException("Unspecified error in DropTable", ex);
            }
        }

        /// <summary>
        /// Builds the connection string.
        /// </summary>
        /// <param name="dbServerName">Name of the database server.</param>
        /// <param name="dbName">Name of the database.</param>
        /// <param name="dbUserID">The database user identifier.</param>
        /// <param name="dbPassword">The database password.</param>
        /// <returns>System.String.</returns>
        public string BuildConnectionString(string dbServerName, string dbName, string dbUserID, string dbPassword)
        {
            //Connection string to DB.
            var sqlConnectionString = "Data Source={0};Initial Catalog={1};User Id={2};Password={3};";

            // Assign variables.
            sqlConnectionString = sqlConnectionString.Replace("{0}", dbServerName);
            sqlConnectionString = sqlConnectionString.Replace("{1}", dbName);
            sqlConnectionString = sqlConnectionString.Replace("{2}", dbUserID);
            sqlConnectionString = sqlConnectionString.Replace("{3}", dbPassword);

            // set current connection string as output connotion string.
            ConnectionString = sqlConnectionString;

            return sqlConnectionString;
        }

        /// <summary>
        /// Breaks the connection string.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="dbServerName">Name of the database server.</param>
        /// <param name="dbName">Name of the database.</param>
        /// <param name="dbUserID">The database user identifier.</param>
        /// <param name="dbPassword">The database password.</param>
        public void BreakConnectionString(string connectionString, out string dbServerName, out string dbName,
            out string dbUserID, out string dbPassword)
        {
            var parameters = connectionString.Split(';');

            dbServerName = null;
            dbName = null;
            dbUserID = null;
            dbPassword = null;

            foreach (var parameter in parameters)
            {
                switch (parameter.Split('=')[0].ToLower())
                {
                    case "data source":
                        dbServerName = parameter.Split('=')[1];
                        break;

                    case "initial catalog":
                        dbName = parameter.Split('=')[1];
                        break;

                    case "user id":
                        dbUserID = parameter.Split('=')[1];
                        break;

                    case "password":
                        dbPassword = parameter.Split('=')[1];
                        break;

                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
        }

        #endregion IDBSchemaHandler Members

        #region Private Helper SQL Methods

        /// <summary>
        /// Creates the database SQL.
        /// </summary>
        /// <param name="dbSchemaDBInstance">The database schema database instance.</param>
        /// <returns>System.String.</returns>
        private string CreateDatabaseSQL(DBSchemaDBDefinition dbSchemaDBInstance)
        {
            // Build create database sql
            var sql = new StringBuilder();

            sql.Append(" CREATE DATABASE [");
            sql.Append(dbSchemaDBInstance.DatabaseName + "]");

            if ((dbSchemaDBInstance.DataFileName != null) && (dbSchemaDBInstance.DataFileName != "") &&
                (dbSchemaDBInstance.DataFilePath != null) && (dbSchemaDBInstance.DataFileName != null))
            {
                sql.Append(" ON ");
                sql.Append(" ( NAME = " + dbSchemaDBInstance.DataFileName + ", ");
                sql.Append(" FILENAME = '" + dbSchemaDBInstance.DataFilePath + "'");

                // Size
                if (dbSchemaDBInstance.DataFileSize != null)
                {
                    sql.Append(", SIZE = " + dbSchemaDBInstance.DataFileSize);
                }

                // Maxsize
                if (dbSchemaDBInstance.DataFileMaxSize != null)
                {
                    sql.Append(", MAXSIZE  = " + dbSchemaDBInstance.DataFileMaxSize);
                }

                // Filegrowth
                if (dbSchemaDBInstance.DataFileGrowth != null)
                {
                    if (dbSchemaDBInstance.IsPercentDataFileGrowth)
                    {
                        sql.Append(", FILEGROWTH   = " + dbSchemaDBInstance.DataFileGrowth + "%");
                    }
                    else
                    {
                        sql.Append(", FILEGROWTH   = " + dbSchemaDBInstance.DataFileGrowth + "MB");
                    }
                }

                sql.Append(" ) ");
            }

            if ((dbSchemaDBInstance.LogFileName != null) && (dbSchemaDBInstance.LogFileName != "") &&
                (dbSchemaDBInstance.LogFilePath != null) && (dbSchemaDBInstance.LogFilePath != null))
            {
                sql.Append(" LOG ON ");
                sql.Append(" ( NAME = " + dbSchemaDBInstance.LogFileName + ", ");
                sql.Append(" FILENAME = '" + dbSchemaDBInstance.LogFilePath + "'");

                // Size
                if (dbSchemaDBInstance.LogFileSize != null)
                {
                    sql.Append(", SIZE = " + dbSchemaDBInstance.LogFileSize);
                }

                // Maxsize
                if (dbSchemaDBInstance.LogFileMaxSize != null)
                {
                    sql.Append(", MAXSIZE  = " + dbSchemaDBInstance.LogFileMaxSize);
                }

                // Filegrowth
                if (dbSchemaDBInstance.LogFileGrowth != null)
                {
                    if (dbSchemaDBInstance.IsPercentLogFileGrowth)
                    {
                        sql.Append(", FILEGROWTH   = " + dbSchemaDBInstance.LogFileGrowth + "%");
                    }
                    else
                    {
                        sql.Append(", FILEGROWTH   = " + dbSchemaDBInstance.LogFileGrowth + "MB");
                    }
                }

                sql.Append(" ) ");
            }

            return sql.ToString();
        }

        /// <summary>
        /// Alters the database SQL.
        /// </summary>
        /// <param name="dbSchemaDBInstance">The database schema database instance.</param>
        /// <returns>System.String.</returns>
        private string AlterDatabaseSQL(DBSchemaDBDefinition dbSchemaDBInstance)
        {
            // Build create database sql
            var sql = new StringBuilder();

            if ((dbSchemaDBInstance.DataFileName != null) && (dbSchemaDBInstance.DataFileName != "") &&
                (dbSchemaDBInstance.DataFilePath != null) && (dbSchemaDBInstance.DataFileName != null))
            {
                sql.Append(" ALTER DATABASE [");
                sql.Append(dbSchemaDBInstance.DatabaseName + "] ");

                sql.Append(" MODIFY FILE   ");
                sql.Append(" ( NAME = " + dbSchemaDBInstance.DataFileName + ", ");
                sql.Append(" FILENAME = '" + dbSchemaDBInstance.DataFilePath + "'");

                // Size
                if (dbSchemaDBInstance.DataFileSize != null)
                {
                    sql.Append(", SIZE = " + dbSchemaDBInstance.DataFileSize);
                }

                // Maxsize
                if (dbSchemaDBInstance.DataFileMaxSize != null)
                {
                    sql.Append(", MAXSIZE  = " + dbSchemaDBInstance.DataFileMaxSize);
                }

                // Filegrowth
                if (dbSchemaDBInstance.DataFileGrowth != null)
                {
                    if (dbSchemaDBInstance.IsPercentDataFileGrowth)
                    {
                        sql.Append(", FILEGROWTH   = " + dbSchemaDBInstance.DataFileGrowth + "%");
                    }
                    else
                    {
                        sql.Append(", FILEGROWTH   = " + dbSchemaDBInstance.DataFileGrowth + "MB");
                    }
                }

                sql.Append(" ) ");
            }

            sql.Append(";");

            if ((dbSchemaDBInstance.LogFileName != null) && (dbSchemaDBInstance.LogFileName != "") &&
                (dbSchemaDBInstance.LogFilePath != null) && (dbSchemaDBInstance.LogFilePath != null))
            {
                sql.Append(" ALTER DATABASE [");
                sql.Append(dbSchemaDBInstance.DatabaseName + "] ");

                sql.Append(" MODIFY FILE ");
                sql.Append(" ( NAME = " + dbSchemaDBInstance.LogFileName + ", ");
                sql.Append(" FILENAME = '" + dbSchemaDBInstance.LogFilePath + "'");

                // Size
                if (dbSchemaDBInstance.LogFileSize != null)
                {
                    sql.Append(", SIZE = " + dbSchemaDBInstance.LogFileSize);
                }

                // Maxsize
                if (dbSchemaDBInstance.LogFileMaxSize != null)
                {
                    sql.Append(", MAXSIZE  = " + dbSchemaDBInstance.LogFileMaxSize);
                }

                // Filegrowth
                if (dbSchemaDBInstance.LogFileGrowth != null)
                {
                    if (dbSchemaDBInstance.IsPercentLogFileGrowth)
                    {
                        sql.Append(", FILEGROWTH   = " + dbSchemaDBInstance.LogFileGrowth + "%");
                    }
                    else
                    {
                        sql.Append(", FILEGROWTH   = " + dbSchemaDBInstance.LogFileGrowth + "MB");
                    }
                }

                sql.Append(" ) ");
            }

            return sql.ToString();
        }

        /// <summary>
        ///     Drops the database SQL.
        /// </summary>
        /// <param name="dbName">Name of the database.</param>
        /// <returns>System.String.</returns>
        private string DropDatabaseSQL(string dbName)
        {
            var sql = new StringBuilder();
            sql.Append(" ALTER DATABASE [" + dbName + "] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;");
            sql.Append(" DROP DATABASE [" + dbName + "];");

            return sql.ToString();
        }

        /// <summary>
        /// Creates the table SQL.
        /// </summary>
        /// <param name="dbSchemaTableDefinition">The database schema table definition.</param>
        /// <returns>System.String.</returns>
        private string CreateTableSQL(DBSchemaTableDefinition dbSchemaTableDefinition)
        {
            // Build create database sql
            var sql = new StringBuilder();

            // Start create table statement
            sql.Append(" CREATE TABLE [");
            sql.Append(dbSchemaTableDefinition.TableName);
            sql.Append("]( ");

            var columnDefCount = 0;
            foreach (var dbSchemaTableColumnDefinition in dbSchemaTableDefinition.ColumnDefinitionList)
            {
                // Set column name, column size
                if ((DBHandlerDataType.NVarChar == dbSchemaTableColumnDefinition.DataType) ||
                    (DBHandlerDataType.VarChar == dbSchemaTableColumnDefinition.DataType) ||
                    (DBHandlerDataType.VarBinary == dbSchemaTableColumnDefinition.DataType))
                {
                    // column size only applys if column type is of NVarChar and Varchar
                    if (dbSchemaTableColumnDefinition.Length != null)
                    {
                        // If size is available then use it
                        if (dbSchemaTableColumnDefinition.Length != -1)
                        {
                            sql.Append("[" + dbSchemaTableColumnDefinition.ColumnName + "] " +
                                       dbSchemaTableColumnDefinition.DataType + "(" +
                                       dbSchemaTableColumnDefinition.Length + ")");
                        }
                        else
                        {
                            sql.Append("[" + dbSchemaTableColumnDefinition.ColumnName + "] " +
                                       dbSchemaTableColumnDefinition.DataType + "(Max)");
                        }
                    }
                    else
                    {
                        // If not available default it to 255.
                        sql.Append("[" + dbSchemaTableColumnDefinition.ColumnName + "] " +
                                   dbSchemaTableColumnDefinition.DataType + "(255)");
                    }
                }
                else
                {
                    sql.Append("[" + dbSchemaTableColumnDefinition.ColumnName + "] " +
                               dbSchemaTableColumnDefinition.DataType);
                }

                // Apply default value if present
                if ((dbSchemaTableColumnDefinition.DefaultValue != "") &&
                    (dbSchemaTableColumnDefinition.DefaultValue != null))
                {
                    sql.Append(" default " + dbSchemaTableColumnDefinition.DefaultValue);
                }

                // Apply Not Null Check
                if (!dbSchemaTableColumnDefinition.IsNullable)
                {
                    sql.Append(" Not Null ");
                }

                // Apply Identity Check
                if (dbSchemaTableColumnDefinition.IsIdentity)
                {
                    sql.Append(" Identity(" + dbSchemaTableColumnDefinition.IdentitySeed + "," +
                               dbSchemaTableColumnDefinition.IdentityIncrement + ")");
                }

                // End of a single column definition

                // Append limit only if loop is not on its last iteration.
                if (columnDefCount < dbSchemaTableDefinition.ColumnDefinitionList.Count - 1)
                {
                    sql.Append(",");
                }

                // Increment count;
                columnDefCount++;
            }

            // Write constraint definitions.
            string primaryKeyColumns = null;
            string pkConstraintName = null;
            string uniqueKeyColumns = null;
            string uqConstraintName = null;
            foreach (var dbSchemaTableColumnDefinition in dbSchemaTableDefinition.ColumnDefinitionList)
            {
                // Apply constraints Primary Key and Unique
                foreach (
                    var dbSchemaConstraintDefinition in dbSchemaTableColumnDefinition.DBSchemaConstraintDefinitionList)
                {
                    switch (dbSchemaConstraintDefinition.Constraint)
                    {
                        case ConstraintType.PrimaryKey:

                            if (primaryKeyColumns == null)
                            {
                                primaryKeyColumns = dbSchemaTableColumnDefinition.ColumnName;
                            }
                            else
                            {
                                primaryKeyColumns += "," + dbSchemaTableColumnDefinition.ColumnName;
                            }

                            break;

                        case ConstraintType.Unique:

                            if (uniqueKeyColumns == null)
                            {
                                uniqueKeyColumns = dbSchemaTableColumnDefinition.ColumnName;
                            }
                            else
                            {
                                uniqueKeyColumns += "," + dbSchemaTableColumnDefinition.ColumnName;
                            }

                            break;
                    }
                }

                // Create unique name for primary key constraint
                if (pkConstraintName == null)
                {
                    pkConstraintName = "PK_DAL_" + dbSchemaTableDefinition.TableName + "_" +
                                       Guid.NewGuid().ToString().Substring(0, 8);
                }

                //  Create unique name for unique constraint
                if (uqConstraintName == null)
                {
                    uqConstraintName = "UQ_DAL_" + dbSchemaTableDefinition.TableName + "_" +
                                       Guid.NewGuid().ToString().Substring(0, 8);
                }
            }

            // Append Primary key constraint sql
            if (primaryKeyColumns != null)
            {
                sql.Append(", Constraint " + pkConstraintName + " Primary Key (" + primaryKeyColumns + ")");
            }

            // Append Unique key constraint sql
            if (uniqueKeyColumns != null)
            {
                sql.Append(", Constraint " + uqConstraintName + " Unique (" + uniqueKeyColumns + ")");
            }

            sql.Append(" ) ");

            return sql.ToString();
        }

        /// <summary>
        ///     Alters the table SQL.
        /// </summary>
        /// <param name="dbOldSchemaTableDefinition">The database old schema table definition.</param>
        /// <param name="dbNewSchemaTableDefinition">The database new schema table definition.</param>
        /// <param name="dbSchemaAlterTableOperation">The database schema alter table operation.</param>
        /// <returns>System.String.</returns>
        private string AlterTableSQL(DBSchemaTableDefinition dbOldSchemaTableDefinition,
            DBSchemaTableDefinition dbNewSchemaTableDefinition, DBSchemaAlterTableOperation dbSchemaAlterTableOperation)
        {
            var sql = new StringBuilder();
            // Check make add column, delete column, Modify data type and rename lists

            if (dbSchemaAlterTableOperation == DBSchemaAlterTableOperation.AddColumn)
            {
                // New column is added
                var columnDefCount = 0;
                foreach (var dbSchemaTableColumnDefinition in dbNewSchemaTableDefinition.ColumnDefinitionList)
                {
                    //If original definition does not contain column then it is taken as a newly added column
                    if (!dbOldSchemaTableDefinition.ColumnDefinitionList.Contains(dbSchemaTableColumnDefinition))
                    {
                        if ((DBHandlerDataType.NVarChar == dbSchemaTableColumnDefinition.DataType) ||
                            (DBHandlerDataType.VarChar == dbSchemaTableColumnDefinition.DataType) ||
                            (DBHandlerDataType.VarBinary == dbSchemaTableColumnDefinition.DataType))
                        {
                            // Check if length is present
                            if (dbSchemaTableColumnDefinition.Length != null)
                            {
                                if (dbSchemaTableColumnDefinition.Length != -1)
                                {
                                    sql.Append(" ALTER table [" + dbOldSchemaTableDefinition.TableName + "] add [" +
                                               dbSchemaTableColumnDefinition.ColumnName + "] " +
                                               dbSchemaTableColumnDefinition.DataType + "(" +
                                               dbSchemaTableColumnDefinition.Length + ") ");
                                }
                                else
                                {
                                    sql.Append(" ALTER table [" + dbOldSchemaTableDefinition.TableName + "] add [" +
                                               dbSchemaTableColumnDefinition.ColumnName + "] " +
                                               dbSchemaTableColumnDefinition.DataType + "(Max) ");
                                }
                            }
                            else
                            {
                                // Set default length value
                                sql.Append(" ALTER table [" + dbOldSchemaTableDefinition.TableName + "] add [" +
                                           dbSchemaTableColumnDefinition.ColumnName + "] " +
                                           dbSchemaTableColumnDefinition.DataType + "(255) ");
                            }
                        }
                        else
                        {
                            sql.Append(" ALTER table [" + dbOldSchemaTableDefinition.TableName + "] add [" +
                                       dbSchemaTableColumnDefinition.ColumnName + "] " +
                                       dbSchemaTableColumnDefinition.DataType + " ");
                        }

                        // Apply default value if present
                        if ((dbSchemaTableColumnDefinition.DefaultValue != "") &&
                            (dbSchemaTableColumnDefinition.DefaultValue != null))
                        {
                            sql.Append(" default " + dbSchemaTableColumnDefinition.DefaultValue);
                        }

                        // Apply Not Null Check
                        if (!dbSchemaTableColumnDefinition.IsNullable)
                        {
                            sql.Append(" Not Null ");
                        }

                        // Apply Identity Check
                        if (dbSchemaTableColumnDefinition.IsIdentity)
                        {
                            sql.Append(" Identity(" + dbSchemaTableColumnDefinition.IdentitySeed + "," +
                                       dbSchemaTableColumnDefinition.IdentityIncrement + ")");
                        }

                        // End of a single column definition

                        // Append limit only if loop is not on its last iteration.
                        if (columnDefCount < dbNewSchemaTableDefinition.ColumnDefinitionList.Count - 1)
                        {
                            sql.Append(",");
                        }
                    }

                    // Increment count;
                    columnDefCount++;
                }

                // Write constraint defintions.
                string primaryKeyColumns = null;
                string pkConstraintName = null;
                string uniqueKeyColumns = null;
                string uqConstraintName = null;
                string obsoletePKConstraintName = null;

                foreach (var dbSchemaTableColumnDefinition in dbNewSchemaTableDefinition.ColumnDefinitionList)
                {
                    //If original definition does not contain column then it is taken as a newly added column
                    if (!dbOldSchemaTableDefinition.ColumnDefinitionList.Contains(dbSchemaTableColumnDefinition))
                    {
                        // Apply constraints Primary Key and Unique
                        foreach (
                            var dbSchemaConstraintDefinition in
                                dbSchemaTableColumnDefinition.DBSchemaConstraintDefinitionList)
                        {
                            switch (dbSchemaConstraintDefinition.Constraint)
                            {
                                case ConstraintType.PrimaryKey:

                                    if (primaryKeyColumns == null)
                                    {
                                        primaryKeyColumns = dbSchemaTableColumnDefinition.ColumnName;
                                    }
                                    else
                                    {
                                        primaryKeyColumns += "," + dbSchemaTableColumnDefinition.ColumnName;
                                    }

                                    break;

                                case ConstraintType.Unique:

                                    if (uniqueKeyColumns == null)
                                    {
                                        uniqueKeyColumns = dbSchemaTableColumnDefinition.ColumnName;
                                    }
                                    else
                                    {
                                        uniqueKeyColumns += "," + dbSchemaTableColumnDefinition.ColumnName;
                                    }

                                    break;
                            }
                        }

                        // Create unique name for primary key constraint
                        if (pkConstraintName == null)
                        {
                            pkConstraintName = "PK_DAL_" + dbNewSchemaTableDefinition.TableName + "_" +
                                               Guid.NewGuid().ToString().Substring(0, 8);
                        }

                        //  Create unique name for unique constraint
                        if (uqConstraintName == null)
                        {
                            uqConstraintName = "UQ_DAL_" + dbNewSchemaTableDefinition.TableName + "_" +
                                               Guid.NewGuid().ToString().Substring(0, 8);
                        }
                    }
                    else
                    {
                        foreach (
                            var dbSchemaConstraintDefinition in
                                dbSchemaTableColumnDefinition.DBSchemaConstraintDefinitionList)
                        {
                            // A check for only primary key is made here since while adding a new column a unique constraint cannot already exist for it
                            // Only pk constraints are checked and if they exist they are removed and replaced with a new pk constraint only if the new column is a pk
                            if (ConstraintType.PrimaryKey == dbSchemaConstraintDefinition.Constraint)
                            {
                                obsoletePKConstraintName = dbSchemaConstraintDefinition.ConstraintName;
                            }
                        }
                    }
                }

                // Append Primary key constraint sql
                if (primaryKeyColumns != null)
                {
                    sql.Append(", Constraint " + pkConstraintName + " Primary Key (" + primaryKeyColumns + ")");
                }

                // Append Unique key constraint sql
                if (uniqueKeyColumns != null)
                {
                    sql.Append(", Constraint " + uqConstraintName + " Unique (" + uniqueKeyColumns + ")");
                }

                // Insert drop primary key statement if present at the start only if definition for new primary key is present.
                if (obsoletePKConstraintName != null)
                {
                    sql.Insert(0,
                        "ALTER TABLE " + dbOldSchemaTableDefinition.TableName + " DROP CONSTRAINT " +
                        obsoletePKConstraintName + ";");
                }
            }
            else if (dbSchemaAlterTableOperation == DBSchemaAlterTableOperation.RenameColumn)
            {
                // Rename columns: Note columns are expected to be in the same index.
                var dbOldSchemaTableDefinitionColumnCount = dbOldSchemaTableDefinition.ColumnDefinitionList.Count;

                for (var count = 0; count < dbOldSchemaTableDefinitionColumnCount; count++)
                {
                    sql.Append(" EXEC sp_rename '" + dbOldSchemaTableDefinition.TableName + "." +
                               dbOldSchemaTableDefinition.ColumnDefinitionList[count].ColumnName + "','" +
                               dbNewSchemaTableDefinition.ColumnDefinitionList[count].ColumnName + "','COLUMN' ;");
                }
            }
            else if (dbSchemaAlterTableOperation == DBSchemaAlterTableOperation.ModifyColumn)
            {
                // Delete Old constraints
                foreach (var dbSchemaTableColumnDefinition in dbOldSchemaTableDefinition.ColumnDefinitionList)
                {
                    // Delete constraints Primary Key and Unique
                    foreach (
                        var dbSchemaConstraintDefinition in
                            dbSchemaTableColumnDefinition.DBSchemaConstraintDefinitionList)
                    {
                        if (
                            !sql.ToString()
                                .Contains("ALTER TABLE " + dbOldSchemaTableDefinition.TableName + " DROP CONSTRAINT " +
                                          dbSchemaConstraintDefinition.ConstraintName + ";"))
                            sql.Append("ALTER TABLE " + dbOldSchemaTableDefinition.TableName + " DROP CONSTRAINT " +
                                       dbSchemaConstraintDefinition.ConstraintName + ";");
                    }
                }

                // Change columns properties.
                foreach (var dbSchemaTableColumnDefinition in dbNewSchemaTableDefinition.ColumnDefinitionList)
                {
                    //If original definition does not contain column then it is taken as a newly added column
                    if (!dbOldSchemaTableDefinition.ColumnDefinitionList.Contains(dbSchemaTableColumnDefinition))
                    {
                        if ((DBHandlerDataType.NVarChar == dbSchemaTableColumnDefinition.DataType) ||
                            (DBHandlerDataType.VarChar == dbSchemaTableColumnDefinition.DataType) ||
                            (DBHandlerDataType.VarBinary == dbSchemaTableColumnDefinition.DataType))
                        {
                            // Check if length is present
                            if (dbSchemaTableColumnDefinition.Length != null)
                            {
                                sql.Append(" ALTER table [" + dbOldSchemaTableDefinition.TableName + "] Alter Column [" +
                                           dbSchemaTableColumnDefinition.ColumnName + "] " +
                                           dbSchemaTableColumnDefinition.DataType + "(" +
                                           dbSchemaTableColumnDefinition.Length + ") ");
                            }
                            else
                            {
                                // Set default length value
                                sql.Append(" ALTER table [" + dbOldSchemaTableDefinition.TableName + "] Alter Column [" +
                                           dbSchemaTableColumnDefinition.ColumnName + "] " +
                                           dbSchemaTableColumnDefinition.DataType + "(255) ");
                            }
                        }
                        else
                        {
                            sql.Append(" ALTER table [" + dbOldSchemaTableDefinition.TableName + "] Alter Column [" +
                                       dbSchemaTableColumnDefinition.ColumnName + "] " +
                                       dbSchemaTableColumnDefinition.DataType + " ");
                        }

                        // Apply Not Null Check
                        if (!dbSchemaTableColumnDefinition.IsNullable)
                        {
                            sql.Append(" Not Null ");
                        }

                        sql.Append(";");
                    }
                }

                // Write constraint defintions.
                string primaryKeyColumns = null;
                string uniqueKeyColumns = null;

                // Apply New constraints
                foreach (var dbSchemaTableColumnDefinition in dbNewSchemaTableDefinition.ColumnDefinitionList)
                {
                    // Apply constraints Primary Key and Unique
                    foreach (
                        var dbSchemaConstraintDefinition in
                            dbSchemaTableColumnDefinition.DBSchemaConstraintDefinitionList)
                    {
                        switch (dbSchemaConstraintDefinition.Constraint)
                        {
                            case ConstraintType.PrimaryKey:

                                if (primaryKeyColumns == null)
                                {
                                    primaryKeyColumns = dbSchemaTableColumnDefinition.ColumnName;
                                }
                                else
                                {
                                    primaryKeyColumns += "," + dbSchemaTableColumnDefinition.ColumnName;
                                }

                                break;

                            case ConstraintType.Unique:

                                if (uniqueKeyColumns == null)
                                {
                                    uniqueKeyColumns = dbSchemaTableColumnDefinition.ColumnName;
                                }
                                else
                                {
                                    uniqueKeyColumns += "," + dbSchemaTableColumnDefinition.ColumnName;
                                }

                                break;
                        }
                    }
                }

                if (primaryKeyColumns != null)
                {
                    sql.Append("ALTER TABLE " + dbOldSchemaTableDefinition.TableName + " Add CONSTRAINT " + "PK_DAL_" +
                               dbNewSchemaTableDefinition.TableName + "_" + Guid.NewGuid().ToString().Substring(0, 8) +
                               " Primary key (" + primaryKeyColumns + ");");
                }

                if (uniqueKeyColumns != null)
                {
                    sql.Append("ALTER TABLE " + dbOldSchemaTableDefinition.TableName + " Add CONSTRAINT " + "UQ_DAL_" +
                               dbNewSchemaTableDefinition.TableName + "_" + Guid.NewGuid().ToString().Substring(0, 8) +
                               " Unique (" + uniqueKeyColumns + ");");
                }
            }
            else if (dbSchemaAlterTableOperation == DBSchemaAlterTableOperation.DropColumn)
            {
                // Delete a column.
                foreach (var dbSchemaTableColumnDefinition in dbOldSchemaTableDefinition.ColumnDefinitionList)
                {
                    //If new definition does not contain column then it is taken as a deleted column
                    if (!dbNewSchemaTableDefinition.ColumnDefinitionList.Contains(dbSchemaTableColumnDefinition))
                    {
                        //Delete constraints on the column
                        foreach (
                            var dbSchemaConstraintDefinition in
                                dbSchemaTableColumnDefinition.DBSchemaConstraintDefinitionList)
                        {
                            if (
                                !sql.ToString()
                                    .Contains("ALTER TABLE " + dbOldSchemaTableDefinition.TableName +
                                              " DROP CONSTRAINT " + dbSchemaConstraintDefinition.ConstraintName + ";"))
                                sql.Append("ALTER TABLE " + dbOldSchemaTableDefinition.TableName + " DROP CONSTRAINT " +
                                           dbSchemaConstraintDefinition.ConstraintName + ";");
                        }

                        //Drop column
                        sql.Append(" Alter Table [" + dbOldSchemaTableDefinition.TableName + "] drop column [" +
                                   dbSchemaTableColumnDefinition.ColumnName + "];");
                    }
                }
            }

            return sql.ToString();
        }

        /// <summary>
        /// Drops the table SQL.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <returns>System.String.</returns>
        private string DropTableSQL(string tableName)
        {
            var sql = "Drop Table [" + tableName + "]";

            return sql;
        }

        /// <summary>
        ///     Creates the er constraint SQL.
        /// </summary>
        /// <param name="dbSchemaConstraintDefinition">The database schema constraint definition.</param>
        /// <returns>System.String.</returns>
        private string CreateERConstraintSQL(DBSchemaConstraintDefinition dbSchemaConstraintDefinition)
        {
            // Build create database sql
            var sql = new StringBuilder();

            sql.Append(" ALTER TABLE  [");
            sql.Append(dbSchemaConstraintDefinition.TableName + "]");
            sql.Append(" ADD CONSTRAINT ");
            sql.Append(dbSchemaConstraintDefinition.ConstraintName);
            sql.Append(" FOREIGN KEY ([");
            sql.Append(dbSchemaConstraintDefinition.ColumnName + "])");
            sql.Append(" REFERENCES [");
            sql.Append(dbSchemaConstraintDefinition.RelatedTableName + "]");
            sql.Append(" ([" + dbSchemaConstraintDefinition.RelatedColumnName + "]) ");

            return sql.ToString();
        }

        /// <summary>
        ///     Drops the er constraint SQL.
        /// </summary>
        /// <param name="dbSchemaConstraintDefinition">The database schema constraint definition.</param>
        /// <returns>System.String.</returns>
        private string DropERConstraintSQL(DBSchemaConstraintDefinition dbSchemaConstraintDefinition)
        {
            return null;
        }

        /// <summary>
        ///     Actives the connections SQL.
        /// </summary>
        /// <returns>System.String.</returns>
        private string ActiveConnectionsSQL()
        {
            return @"SELECT spid
                    FROM master..sysprocesses
                    WHERE dbid = DB_ID('model')";
        }

        #endregion Private Helper SQL Methods
    }
}