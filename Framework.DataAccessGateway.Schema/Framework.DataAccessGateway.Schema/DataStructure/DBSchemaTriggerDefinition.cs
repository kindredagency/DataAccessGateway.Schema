namespace Framework.DataAccessGateway.Schema.DataStructure
{
    /// <summary>
    ///     Trigger Definition
    /// </summary>
    public class DBSchemaTriggerDefinition
    {
        #region Constructor

        /// <summary>
        ///     Initializes a new instance of the <see cref="DBSchemaTriggerDefinition" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public DBSchemaTriggerDefinition(string name)
        {
            TriggerName = name;
        }

        #endregion Constructor

        #region Public Methods

        /// <summary>
        ///     Copies this instance.
        /// </summary>
        /// <returns>DBSchemaTriggerDefinition.</returns>
        public DBSchemaTriggerDefinition Copy()
        {
            var dbSchemaTriggerDefinition = new DBSchemaTriggerDefinition(TriggerName);

            dbSchemaTriggerDefinition.IsAfter = IsAfter;
            dbSchemaTriggerDefinition.IsDelete = IsDelete;
            dbSchemaTriggerDefinition.IsInsert = IsInsert;
            dbSchemaTriggerDefinition.IsInsteadOf = IsInsteadOf;
            dbSchemaTriggerDefinition.IsUpdate = IsUpdate;
            dbSchemaTriggerDefinition.TriggerDefinition = TriggerDefinition;
            dbSchemaTriggerDefinition.TableName = TableName;
            dbSchemaTriggerDefinition.TriggerOwner = TriggerOwner;
            dbSchemaTriggerDefinition.TRStatus = TRStatus;

            return dbSchemaTriggerDefinition;
        }

        #endregion Public Methods

        #region Public Variable

        /// <summary>
        ///     Name if the trigger
        /// </summary>
        public string TriggerName { get; set; }

        /// <summary>
        ///     Owner of the Trigger
        /// </summary>
        public string TriggerOwner { get; set; }

        /// <summary>
        ///     Table name of the trigger
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        ///     Trigger Definition
        /// </summary>
        public string TriggerDefinition { get; set; }

        /// <summary>
        ///     Is an update trigger
        /// </summary>
        public bool IsUpdate { get; set; }

        /// <summary>
        ///     Is a delete trigger
        /// </summary>
        public bool IsDelete { get; set; }

        /// <summary>
        ///     Is an insert trigger
        /// </summary>
        public bool IsInsert { get; set; }

        /// <summary>
        ///     Is an after trigger
        /// </summary>
        public bool IsAfter { get; set; }

        /// <summary>
        ///     Is a instead of trigger
        /// </summary>
        public bool IsInsteadOf { get; set; }

        /// <summary>
        ///     The status of the trigger
        /// </summary>
        public string TRStatus { get; set; }

        #endregion Public Variable
    }
}