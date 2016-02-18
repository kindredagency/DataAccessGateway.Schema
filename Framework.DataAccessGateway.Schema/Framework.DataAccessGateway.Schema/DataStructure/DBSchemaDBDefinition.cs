using System;
using System.IO;
using Framework.DataAccessGateway.Schema.Collection;

namespace Framework.DataAccessGateway.Schema.DataStructure
{
    /// <summary>
    /// Class DBSchemaDBDefinition.
    /// </summary>
    public class DBSchemaDBDefinition
    {
        #region private variables

        private string databaseName;

        // Datafile variables

        private string dataFilePath;
        private int? dataFileGrowth = 10;
        private bool isPercentDataFileGrowth = true;

        // Log file variables

        private string logFilePath;
        private int? logFileGrowth = 10;
        private bool isPercentLogFileGrowth = true;

        // Collections
        private DBSchemaTableDefinitionCollection tables = new DBSchemaTableDefinitionCollection();

        private DBSchemaStoredProcedureDefinitionCollection procs = new DBSchemaStoredProcedureDefinitionCollection();
        private DBSchemaTriggerDefinitionCollection triggers = new DBSchemaTriggerDefinitionCollection();

        #endregion private variables

        #region methods

        /// <summary>
        /// Initializes a new instance of the <see cref="DBSchemaDBDefinition"/> class.
        /// </summary>
        /// <param name="databaseName">Name of the database.</param>
        public DBSchemaDBDefinition(string databaseName)
        {
            LogFileMaxSize = null;
            LogFileSize = null;
            LogFileName = null;
            DataFileMaxSize = null;
            DataFileSize = null;
            DataFileName = null;
            this.databaseName = databaseName;
            InitValues(this.databaseName);
        }

        /// <summary>
        /// Copies this instance.
        /// </summary>
        /// <returns>DBSchemaDBDefinition.</returns>
        public DBSchemaDBDefinition Copy()
        {
            var dbSchemaDBInstanceCopy = new DBSchemaDBDefinition(databaseName);

            //Set attributes
            dbSchemaDBInstanceCopy.DatabaseSize = DatabaseSize;
            dbSchemaDBInstanceCopy.dataFileGrowth = dataFileGrowth;
            dbSchemaDBInstanceCopy.DataFileMaxSize = DataFileMaxSize;
            dbSchemaDBInstanceCopy.DataFileName = DataFileName;
            dbSchemaDBInstanceCopy.dataFilePath = dataFilePath;
            dbSchemaDBInstanceCopy.DataFileSize = DataFileSize;
            dbSchemaDBInstanceCopy.DateCreated = DateCreated;
            dbSchemaDBInstanceCopy.isPercentDataFileGrowth = isPercentDataFileGrowth;
            dbSchemaDBInstanceCopy.isPercentLogFileGrowth = isPercentLogFileGrowth;
            dbSchemaDBInstanceCopy.logFileGrowth = logFileGrowth;
            dbSchemaDBInstanceCopy.LogFileMaxSize = LogFileMaxSize;
            dbSchemaDBInstanceCopy.LogFileName = LogFileName;
            dbSchemaDBInstanceCopy.logFilePath = logFilePath;
            dbSchemaDBInstanceCopy.LogFileSize = LogFileSize;

            // Copy internal table definitions
            foreach (var dbSchemaTableDefinition in tables)
            {
                dbSchemaDBInstanceCopy.tables.Add(dbSchemaTableDefinition.Copy());
            }

            // Copy internal procs
            foreach (var dbSchemaStoredProcedureDefinition in Procs)
            {
                dbSchemaDBInstanceCopy.Procs.Add(dbSchemaStoredProcedureDefinition.Copy());
            }

            // Copy internal triggers
            foreach (var dbSchemaTriggerDefinition in triggers)
            {
                dbSchemaDBInstanceCopy.triggers.Add(dbSchemaTriggerDefinition.Copy());
            }

            return dbSchemaDBInstanceCopy;
        }

        #endregion methods

        #region properties

        /// <summary>
        ///     Gets or sets the name of the database.
        /// </summary>
        /// <value>The name of the database.</value>
        public string DatabaseName
        {
            get { return databaseName; }
            set
            {
                databaseName = value;
                InitValues(databaseName);
            }
        }

        /// <summary>
        ///     Gets the size of the database.
        /// </summary>
        /// <value>The size of the database.</value>
        public long DatabaseSize { get; internal set; }

        /// <summary>
        ///     Gets the date created.
        /// </summary>
        /// <value>The date created.</value>
        public DateTime DateCreated { get; internal set; }

        /// <summary>
        ///     Gets the name of the data file.
        /// </summary>
        /// <value>The name of the data file.</value>
        public string DataFileName { get; private set; }

        /// <summary>
        ///     Gets or sets the data file path.
        /// </summary>
        /// <value>The data file path.</value>
        public string DataFilePath
        {
            get { return dataFilePath; }
            set
            {
                dataFilePath = value;
                DataFileName = new FileInfo(value).Name.Replace('.', '_');
            }
        }

        /// <summary>
        ///     Gets or sets the size of the data file.
        /// </summary>
        /// <value>The size of the data file.</value>
        public int? DataFileSize { get; set; }

        /// <summary>
        ///     Gets or sets the maximum size of the data file.
        /// </summary>
        /// <value>The maximum size of the data file.</value>
        public int? DataFileMaxSize { get; set; }

        /// <summary>
        ///     Gets or sets the data file growth.
        /// </summary>
        /// <value>The data file growth.</value>
        public int? DataFileGrowth
        {
            get { return dataFileGrowth; }
            set { dataFileGrowth = value; }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is percent data file growth.
        /// </summary>
        /// <value><c>true</c> if this instance is percent data file growth; otherwise, <c>false</c>.</value>
        public bool IsPercentDataFileGrowth
        {
            get { return isPercentDataFileGrowth; }
            set { isPercentDataFileGrowth = value; }
        }

        /// <summary>
        ///     Gets the name of the log file.
        /// </summary>
        /// <value>The name of the log file.</value>
        public string LogFileName { get; private set; }

        /// <summary>
        ///     Gets or sets the log file path.
        /// </summary>
        /// <value>The log file path.</value>
        public string LogFilePath
        {
            get { return logFilePath; }
            set
            {
                logFilePath = value;
                LogFileName = new FileInfo(value).Name.Replace('.', '_');
                ;
            }
        }

        /// <summary>
        ///     Gets or sets the size of the log file.
        /// </summary>
        /// <value>The size of the log file.</value>
        public int? LogFileSize { get; set; }

        /// <summary>
        ///     Gets or sets the maximum size of the log file.
        /// </summary>
        /// <value>The maximum size of the log file.</value>
        public int? LogFileMaxSize { get; set; }

        /// <summary>
        ///     Gets or sets the log file growth.
        /// </summary>
        /// <value>The log file growth.</value>
        public int? LogFileGrowth
        {
            get { return logFileGrowth; }
            set { logFileGrowth = value; }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is percent log file growth.
        /// </summary>
        /// <value><c>true</c> if this instance is percent log file growth; otherwise, <c>false</c>.</value>
        public bool IsPercentLogFileGrowth
        {
            get { return isPercentLogFileGrowth; }
            set { isPercentLogFileGrowth = value; }
        }

        /// <summary>
        ///     Gets the tables.
        /// </summary>
        /// <value>The tables.</value>
        public DBSchemaTableDefinitionCollection Tables
        {
            get { return tables; }
            internal set { tables = value; }
        }

        /// <summary>
        ///     Gets the procs.
        /// </summary>
        /// <value>The procs.</value>
        public DBSchemaStoredProcedureDefinitionCollection Procs
        {
            get { return procs; }
            internal set { procs = value; }
        }

        /// <summary>
        ///     Gets or sets the triggers.
        /// </summary>
        /// <value>The triggers.</value>
        public DBSchemaTriggerDefinitionCollection Triggers
        {
            get { return triggers; }
            set { triggers = value; }
        }

        #endregion properties

        #region private methods

        /// <summary>
        /// Initializes the values.
        /// </summary>
        /// <param name="dbName">Name of the database.</param>
        private void InitValues(string dbName)
        {
            if ((dataFilePath != "") && (dataFilePath != null))
            {
                var previousDataFileFolderPath = new FileInfo(dataFilePath).Directory.ToString();

                DataFilePath = previousDataFileFolderPath + "\\" + dbName + ".mdf";
            }

            if ((dataFilePath != "") && (dataFilePath != null))
            {
                var previousLogFileFolderPath = new FileInfo(logFilePath).Directory.ToString();

                LogFilePath = previousLogFileFolderPath + "\\" + dbName + "_log.ldf";
            }

            dataFileGrowth = 10;
            DataFileSize = null;
            DataFileMaxSize = null;

            logFileGrowth = 10;
            LogFileSize = 50;
            LogFileMaxSize = null;
        }

        #endregion private methods
    }
}