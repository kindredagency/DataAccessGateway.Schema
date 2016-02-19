using System;

namespace Framework.DataAccessGateway.Schema
{
    /// <summary>
    /// Class _SQLDB.
    /// </summary>
    internal class _SQLDB
    {
        /// <summary>
        /// Gets or sets the name of the database.
        /// </summary>
        /// <value>The name of the database.</value>
        public string DATABASE_NAME { get; set; }

        /// <summary>
        /// Gets or sets the size of the database.
        /// </summary>
        /// <value>The size of the database.</value>
        public int DATABASE_SIZE { get; set; }

        /// <summary>
        /// Gets or sets the data file path.
        /// </summary>
        /// <value>The data file path.</value>
        public string DATA_FILE_PATH { get; set; }

        /// <summary>
        /// Gets or sets the size of the data file.
        /// </summary>
        /// <value>The size of the data file.</value>
        public int DATA_FILE_SIZE { get; set; }

        /// <summary>
        /// Gets or sets the size of the data file max.
        /// </summary>
        /// <value>The size of the data file max.</value>
        public int DATA_FILE_MAX_SIZE { get; set; }

        /// <summary>
        /// Gets or sets the data file growth.
        /// </summary>
        /// <value>The data file growth.</value>
        public int DATA_FILE_GROWTH { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [is percentage growth].
        /// </summary>
        /// <value><c>true</c> if [is percentage growth]; otherwise, <c>false</c>.</value>
        public bool IS_PERCENTAGE_GROWTH { get; set; }

        /// <summary>
        /// Gets or sets the log file path.
        /// </summary>
        /// <value>The log file path.</value>
        public string LOG_FILE_PATH { get; set; }

        /// <summary>
        /// Gets or sets the size of the log file.
        /// </summary>
        /// <value>The size of the log file.</value>
        public int LOG_FILE_SIZE { get; set; }

        /// <summary>
        /// Gets or sets the size of the log file max.
        /// </summary>
        /// <value>The size of the log file max.</value>
        public int LOG_FILE_MAX_SIZE { get; set; }

        /// <summary>
        /// Gets or sets the log file growth.
        /// </summary>
        /// <value>The log file growth.</value>
        public int LOG_FILE_GROWTH { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [is percentage log file growth].
        /// </summary>
        /// <value><c>true</c> if [is percentage log file growth]; otherwise, <c>false</c>.</value>
        public bool IS_PERCENTAGE_LOG_FILE_GROWTH { get; set; }

        /// <summary>
        /// Gets or sets the date created.
        /// </summary>
        /// <value>The date created.</value>
        public DateTime DATE_CREATED { get; set; }
    }
}