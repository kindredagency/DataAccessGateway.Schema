namespace Framework.DataAccessGateway.Schema
{
    public enum ConstraintType
    {
        /// <summary>
        ///     Primary Key column
        /// </summary>
        PrimaryKey = 1,

        /// <summary>
        ///     Foreign Key column
        /// </summary>
        ForeignKey = 2,

        /// <summary>
        ///     Unique constraint on a column
        /// </summary>
        Unique = 3,

        /// <summary>
        ///     Not defined
        /// </summary>
        NotDefined = 4
    }
}
