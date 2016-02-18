namespace Framework.DataAccessGateway.Schema.Enum
{
    /// <summary>
    ///     Alter table schema operations
    /// </summary>
    public enum DBSchemaAlterTableOperation
    {
        /// <summary>
        ///     Add a new column
        /// </summary>
        AddColumn = 1,

        /// <summary>
        ///     Delete a column
        /// </summary>
        DropColumn = 2,

        /// <summary>
        ///     Rename a column
        /// </summary>
        RenameColumn = 3,

        /// <summary>
        ///     Modify a column
        /// </summary>
        ModifyColumn = 4
    }
}