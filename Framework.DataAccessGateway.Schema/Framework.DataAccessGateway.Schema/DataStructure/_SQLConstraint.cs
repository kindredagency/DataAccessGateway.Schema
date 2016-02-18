namespace Framework.DataAccessGateway.Schema.DataStructure
{
    /// <summary>
    /// Class _SQLConstraint.
    /// </summary>
    internal class _SQLConstraint
    {
        public string TABLE_NAME { get; set; }

        public string COLUMN_NAME { get; set; }

        public string CONSTRAINT_TYPE { get; set; }

        public string CONSTRAINT_NAME { get; set; }

        public string RELATED_TABLE_NAME { get; set; }

        public string RELATED_COLUMN_NAME { get; set; }

        public int? POSITION { get; set; }

        public int? RELATED_POSITION { get; set; }
    }
}