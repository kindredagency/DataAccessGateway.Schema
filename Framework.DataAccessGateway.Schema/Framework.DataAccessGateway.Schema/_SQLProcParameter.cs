namespace Framework.DataAccessGateway.Schema
{
    /// <summary>
    /// Class _SQLProcParameter.
    /// </summary>
    internal class _SQLProcParameter
    {
        /// <summary>
        /// Gets or sets the name of the proc.
        /// </summary>
        /// <value>The name of the proc.</value>
        public string PROC_NAME { get; set; }
        /// <summary>
        /// Gets or sets the paramenter identifier.
        /// </summary>
        /// <value>The paramenter identifier.</value>
        public int PARAMENTER_ID { get; set; }
        /// <summary>
        /// Gets or sets the name of the parameter.
        /// </summary>
        /// <value>The name of the parameter.</value>
        public string PARAMETER_NAME { get; set; }
        /// <summary>
        /// Gets or sets the type of the parameter data.
        /// </summary>
        /// <value>The type of the parameter data.</value>
        public string PARAMETER_DATA_TYPE { get; set; }
        /// <summary>
        /// Gets or sets the parameter max bytes.
        /// </summary>
        /// <value>The parameter max bytes.</value>
        public int PARAMETER_MAX_BYTES { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="_SQLProcParameter"/> is isoutputparameter.
        /// </summary>
        /// <value><c>true</c> if isoutputparameter; otherwise, <c>false</c>.</value>
        public bool ISOUTPUTPARAMETER { get; set; }
    }
}