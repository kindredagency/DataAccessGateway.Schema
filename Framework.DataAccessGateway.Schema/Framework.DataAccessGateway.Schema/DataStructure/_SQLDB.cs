using System;

namespace Framework.DataAccessGateway.Schema.DataStructure
{
    /// <summary>
    /// Class _SQLDB.
    /// </summary>
    internal class _SQLDB
    {
        public string DATABASE_NAME { get; set; }

        public int DATABASE_SIZE { get; set; }

        public string DATA_FILE_PATH { get; set; }

        public int DATA_FILE_SIZE { get; set; }

        public int DATA_FILE_MAX_SIZE { get; set; }

        public int DATA_FILE_GROWTH { get; set; }

        public bool IS_PERCENTAGE_GROWTH { get; set; }

        public string LOG_FILE_PATH { get; set; }

        public int LOG_FILE_SIZE { get; set; }

        public int LOG_FILE_MAX_SIZE { get; set; }

        public int LOG_FILE_GROWTH { get; set; }

        public bool IS_PERCENTAGE_LOG_FILE_GROWTH { get; set; }

        public DateTime DATE_CREATED { get; set; }
    }
}