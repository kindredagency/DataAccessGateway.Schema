namespace Framework.DataAccessGateway.Schema
{
    /// <summary>
    /// Class _SQLProcParameter.
    /// </summary>
    internal class _SQLProcParameter
    {
        public string PROC_NAME { get; set; }
        public int PARAMENTER_ID { get; set; }
        public string PARAMETER_NAME { get; set; }
        public string PARAMETER_DATA_TYPE { get; set; }
        public int PARAMETER_MAX_BYTES { get; set; }
        public bool ISOUTPUTPARAMETER { get; set; }
    }
}