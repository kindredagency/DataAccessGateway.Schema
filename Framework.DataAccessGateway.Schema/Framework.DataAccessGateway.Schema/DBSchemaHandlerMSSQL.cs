using System.Data;
using System.Linq;
using Framework.DataAccessGateway.Core;

namespace Framework.DataAccessGateway.Schema
{
    internal class DBSchemaHandlerMSSQL : IDBSchemaHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DBSchemaHandlerMSSQL"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public DBSchemaHandlerMSSQL(string connectionString)
        {
            ConnectionString = connectionString;           

            BreakConnectionString(connectionString);
        }

        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <value>The connection string.</value>
        public string ConnectionString { get; private set; }

        /// <summary>
        /// Gets the name of the server.
        /// </summary>
        /// <value>The name of the server.</value>
        public string ServerName
        {
            get; private set;
        }

        /// <summary>
        /// Gets the name of the data base.
        /// </summary>
        /// <value>The name of the data base.</value>
        public string DataBaseName
        {
            get; private set;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// Gets the data base definition.
        /// </summary>
        /// <returns>DBSchemaDataBaseDefinition.</returns>
        /// <exception cref="Framework.DataAccessGateway.Schema.DBSchemaHandlerException">Unable to establish connection to specified database</exception>
        public DBSchemaDataBaseDefinition GetDataBaseDefinition()
        {
            // Exec Query to retrieve all DB Information information.
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
                var dbHandler = new DBHandler(ConnectionString, DBHandlerType.DbHandlerMSSQL);

                var dbHandlerParameters = new DBHandlerParameter[1];
                dbHandlerParameters[0] = new DBHandlerParameter("DatabaseName", DBHandlerDataType.VarChar);
                dbHandlerParameters[0].Value = DataBaseName;

                var sqlDBSchema = dbHandler.ExecuteQuery<_SQLDB>(sql, dbHandlerParameters, CommandType.Text).SingleOrDefault();

                DBSchemaDataBaseDefinition dbSchemaDBInstance = new DBSchemaDataBaseDefinition(DataBaseName);

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
                    dbSchemaDBInstance.Tables = GetTableDefinitionListing();

                    dbSchemaDBInstance.Procs = GetStoredProcedureDefinitionListing();

                    dbSchemaDBInstance.Triggers = GetTriggerDefinitionListing();
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
        /// Gets the table definition.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <returns>DBSchemaTableDefinition.</returns>
        /// <exception cref="Framework.DataAccessGateway.Schema.DBSchemaHandlerException">Unable to establish connection or retrieve information from specified database</exception>
        public DBSchemaTableDefinition GetTableDefinition(string tableName)
        {
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
                var dbHandler = new DBHandler(ConnectionString, DBHandlerType.DbHandlerMSSQL);

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
                throw new DBSchemaHandlerException("Unable to establish connection or retrieve information from specified database", ex);
            }
        }
        
        /// <summary>
        /// Gets the table definition listing.
        /// </summary>
        /// <returns>DBSchemaTableDefinitionCollection.</returns>
        /// <exception cref="Framework.DataAccessGateway.Schema.DBSchemaHandlerException">Unable to establish connection or retrieve information from specified database</exception>
        public DBSchemaTableDefinitionCollection GetTableDefinitionListing()
        {
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
                var dbHandler = new DBHandler(ConnectionString, DBHandlerType.DbHandlerMSSQL);

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
                throw new DBSchemaHandlerException("Unable to establish connection or retrieve information from specified database", ex);
            }
        }

        /// <summary>
        /// Gets the stored procedure definition.
        /// </summary>
        /// <param name="storedProcedureName">Name of the stored procedure.</param>
        /// <returns>DBSchemaStoredProcedureDefinition.</returns>
        public DBSchemaStoredProcedureDefinition GetStoredProcedureDefinition(string storedProcedureName)
        {
            return GetStoredProcedureDefinitionListing()[storedProcedureName];
        }

        /// <summary>
        /// Gets the stored procedure definition listing.
        /// </summary>
        /// <returns>DBSchemaStoredProcedureDefinitionCollection.</returns>
        /// <exception cref="Framework.DataAccessGateway.Schema.DBSchemaHandlerException">Unable to establish connection or retrieve information from specified database</exception>
        public DBSchemaStoredProcedureDefinitionCollection GetStoredProcedureDefinitionListing()
        {   

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
                var dbHandler = new DBHandler(ConnectionString, DBHandlerType.DbHandlerMSSQL);

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
                throw new DBSchemaHandlerException("Unable to establish connection or retrieve information from specified database", ex);
            }

            return dbSchemaStoredProcedureDefinitionCollection;
        }

        /// <summary>
        /// Gets the trigger definition.
        /// </summary>
        /// <param name="triggerName">Name of the trigger.</param>
        /// <returns>DBSchemaTriggerDefinition.</returns>
        public DBSchemaTriggerDefinition GetTriggerDefinition(string triggerName)
        {
            return GetTriggerDefinitionListing()[triggerName];
        }

        /// <summary>
        /// Gets the trigger definition listing.
        /// </summary>
        /// <returns>DBSchemaTriggerDefinitionCollection.</returns>
        /// <exception cref="Framework.DataAccessGateway.Schema.DBSchemaHandlerException">Unable to establish connection or retrieve information from specified database</exception>
        public DBSchemaTriggerDefinitionCollection GetTriggerDefinitionListing()
        {   
            var sql = @"SELECT TRIGGER_NAME = SO.Name,
	                               TRIGGER_OWNER = USER_NAME(SO.uid),
	                               TABLE_NAME = OBJECT_NAME(SO.parent_obj),
                                   TRIGGER_DEFINITION = (select object_definition(object_id) from sys.triggers where name = SO.name),
	                               IS_UPDATE = CAST( OBJECTPROPERTY( SO.id, 'ExecIsUpdateTrigger') as bit),
                                   IS_DELETE = CAST( OBJECTPROPERTY( SO.id, 'ExecIsDeleteTrigger') as bit),
                                   IS_INSERT = CAST( OBJECTPROPERTY( SO.id, 'ExecIsInsertTrigger') as bit),
	                               IS_AFTER =  CAST( OBJECTPROPERTY( SO.id, 'ExecIsAfterTrigger') as bit),
                                   IS_INSTEAD_OF = CAST( OBJECTPROPERTY( SO.id, 'ExecIsInsteadOfTrigger') as bit),
	                               TR_STATUS = CASE OBJECTPROPERTY(SO.id, 'ExecIsTriggerDisabled') WHEN 1 THEN 'Disabled' ELSE 'Enabled' END
                            FROM Sysobjects SO
                            WHERE type = 'TR'";

            var dbSchemaTriggerDefinitionCollection = new DBSchemaTriggerDefinitionCollection();

            try
            {
                var dbHandler = new DBHandler(ConnectionString, DBHandlerType.DbHandlerMSSQL);

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
        /// Breaks the connection string.
        /// </summary>
        /// <param name="ConnectionString">The connection string.</param>
        private void BreakConnectionString(string ConnectionString)
        {
            var parameters = ConnectionString.Split(';');

            foreach (var parameter in parameters)
            {
                switch (parameter.Split('=')[0].ToLower())
                {
                    case "data source":
                        ServerName = parameter.Split('=')[1];
                        break;

                    case "initial catalog":
                        DataBaseName = parameter.Split('=')[1];
                        break;

                    case "user id":
                        var userId = parameter.Split('=')[1];
                        break;

                    case "password":
                        var password = parameter.Split('=')[1];
                        break;

                    default:
                        break;
                }
            }
        }
               
    }
}