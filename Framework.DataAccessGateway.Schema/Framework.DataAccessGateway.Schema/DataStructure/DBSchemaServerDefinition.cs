namespace Framework.DataAccessGateway.Schema.DataStructure
{
    /// <summary>
    ///     Summary description for DBSchemaServerInstance
    /// </summary>
    public class DBSchemaServerDefinition
    {
        #region Constructor

        /// <summary>
        ///     DBSchemaServerInstance
        /// </summary>
        /// <param name="serverName">server name</param>
        /// <param name="instanceName">unique db instance name if none present then null</param>
        /// <param name="isClustered">if the server is clustered</param>
        /// <param name="version">version no of the db server</param>
        public DBSchemaServerDefinition(string serverName, string instanceName, string isClustered, string version)
        {
            ServerName = serverName;
            InstanceName = instanceName;
            IsClustered = isClustered;
            Version = version;
        }

        #endregion Constructor

        #region Public Properties

        /// <summary>
        ///     Server name on which the database is running
        /// </summary>
        public string ServerName { get; private set; }

        /// <summary>
        ///     Instance name of the DB.
        ///     In some cases more than one database servers are run on one physical machine.
        ///     Therefore a unique instance names has to be used to identify them individually.
        /// </summary>
        public string InstanceName { get; private set; }

        /// <summary>
        ///     Identifies if the server is clustered
        /// </summary>
        public string IsClustered { get; private set; }

        /// <summary>
        ///     Version no of the server
        /// </summary>
        public string Version { get; private set; }

        #endregion Public Properties
    }
}