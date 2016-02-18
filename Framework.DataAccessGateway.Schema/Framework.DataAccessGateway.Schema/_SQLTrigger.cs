namespace Framework.DataAccessGateway.Schema
{
    /// <summary>
    /// Class _SQLTrigger.
    /// </summary>
    internal class _SQLTrigger
    {
        public string TRIGGER_NAME { get; set; }

        public string TRIGGER_OWNER { get; set; }

        public string TABLE_NAME { get; set; }

        public string TRIGGER_DEFINITION { get; set; }

        public bool IS_UPDATE { get; set; }

        public bool IS_DELETE { get; set; }

        public bool IS_AFTER { get; set; }

        public bool IS_INSERT { get; set; }

        public bool IS_INSTEAD_OF { get; set; }

        public string TR_STATUS { get; set; }
    }
}