namespace Framework.DataAccessGateway.Schema
{
    /// <summary>
    /// Class _SQLTrigger.
    /// </summary>
    internal class _SQLTrigger
    {
        /// <summary>
        /// Gets or sets the name of the trigger.
        /// </summary>
        /// <value>The name of the trigger.</value>
        public string TRIGGER_NAME { get; set; }

        /// <summary>
        /// Gets or sets the trigger owner.
        /// </summary>
        /// <value>The trigger owner.</value>
        public string TRIGGER_OWNER { get; set; }

        /// <summary>
        /// Gets or sets the name of the table.
        /// </summary>
        /// <value>The name of the table.</value>
        public string TABLE_NAME { get; set; }

        /// <summary>
        /// Gets or sets the trigger definition.
        /// </summary>
        /// <value>The trigger definition.</value>
        public string TRIGGER_DEFINITION { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [is update].
        /// </summary>
        /// <value><c>true</c> if [is update]; otherwise, <c>false</c>.</value>
        public bool IS_UPDATE { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [is delete].
        /// </summary>
        /// <value><c>true</c> if [is delete]; otherwise, <c>false</c>.</value>
        public bool IS_DELETE { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [is after].
        /// </summary>
        /// <value><c>true</c> if [is after]; otherwise, <c>false</c>.</value>
        public bool IS_AFTER { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [is insert].
        /// </summary>
        /// <value><c>true</c> if [is insert]; otherwise, <c>false</c>.</value>
        public bool IS_INSERT { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [is instead of].
        /// </summary>
        /// <value><c>true</c> if [is instead of]; otherwise, <c>false</c>.</value>
        public bool IS_INSTEAD_OF { get; set; }

        /// <summary>
        /// Gets or sets the tr status.
        /// </summary>
        /// <value>The tr status.</value>
        public string TR_STATUS { get; set; }
    }
}