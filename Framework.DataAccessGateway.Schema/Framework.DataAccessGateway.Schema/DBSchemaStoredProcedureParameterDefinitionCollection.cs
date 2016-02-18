using System.Collections;
using System.Collections.Generic;
using Framework.DataAccessGateway.Schema.DataStructure;

namespace Framework.DataAccessGateway.Schema
{
    /// <summary>
    /// Class DBSchemaStoredProcedureParameterDefinitionCollection.
    /// </summary>
    public class DBSchemaStoredProcedureParameterDefinitionCollection : IList<DBSchemaStoredProcedureParameterDefinition>
    {
        #region Private Variables

        private readonly List<DBSchemaStoredProcedureParameterDefinition> DBSchemaStoredProcedureParameterDefinitionList = new List<DBSchemaStoredProcedureParameterDefinition>();

        #endregion Private Variables

        #region IEnumerable<DBSchemaStoredProcedureParameterDefinition> Members

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<DBSchemaStoredProcedureParameterDefinition> GetEnumerator()
        {
            return DBSchemaStoredProcedureParameterDefinitionList.GetEnumerator();
        }

        #endregion IEnumerable<DBSchemaStoredProcedureParameterDefinition> Members

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return DBSchemaStoredProcedureParameterDefinitionList.GetEnumerator();
        }

        #endregion IEnumerable Members

        #region IList<DBSchemaStoredProcedureParameterDefinition> Members

        /// <summary>
        /// Indexes the of.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>System.Int32.</returns>
        public int IndexOf(DBSchemaStoredProcedureParameterDefinition item)
        {
            return DBSchemaStoredProcedureParameterDefinitionList.IndexOf(item);
        }

        /// <summary>
        /// Inserts the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="item">The item.</param>
        public void Insert(int index, DBSchemaStoredProcedureParameterDefinition item)
        {
            DBSchemaStoredProcedureParameterDefinitionList.Insert(index, item);
        }

        /// <summary>
        /// Removes at.
        /// </summary>
        /// <param name="index">The index.</param>
        public void RemoveAt(int index)
        {
            DBSchemaStoredProcedureParameterDefinitionList.RemoveAt(index);
        }

        /// <summary>
        /// Gets or sets the <see cref="DBSchemaStoredProcedureParameterDefinition"/> at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>DBSchemaStoredProcedureParameterDefinition.</returns>
        public DBSchemaStoredProcedureParameterDefinition this[int index]
        {
            get { return DBSchemaStoredProcedureParameterDefinitionList[index]; }
            set { DBSchemaStoredProcedureParameterDefinitionList[index] = value; }
        }

        #endregion IList<DBSchemaStoredProcedureParameterDefinition> Members

        #region ICollection<DBSchemaStoredProcedureParameterDefinition> Members

        /// <summary>
        /// Adds the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Add(DBSchemaStoredProcedureParameterDefinition item)
        {
            DBSchemaStoredProcedureParameterDefinitionList.Add(item);
        }

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        public void Clear()
        {
            DBSchemaStoredProcedureParameterDefinitionList.Clear();
        }

        /// <summary>
        /// Determines whether [contains] [the specified item].
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns><c>true</c> if [contains] [the specified item]; otherwise, <c>false</c>.</returns>
        public bool Contains(DBSchemaStoredProcedureParameterDefinition item)
        {
            return DBSchemaStoredProcedureParameterDefinitionList.Contains(item);
        }

        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="arrayIndex">Index of the array.</param>
        public void CopyTo(DBSchemaStoredProcedureParameterDefinition[] array, int arrayIndex)
        {
            DBSchemaStoredProcedureParameterDefinitionList.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <value>The count.</value>
        public int Count
        {
            get { return DBSchemaStoredProcedureParameterDefinitionList.Count; }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only.
        /// </summary>
        /// <value><c>true</c> if this instance is read only; otherwise, <c>false</c>.</value>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns><c>true</c> if item cannot be found then, <c>false</c> otherwise.</returns>
        public bool Remove(DBSchemaStoredProcedureParameterDefinition item)
        {
            return DBSchemaStoredProcedureParameterDefinitionList.Remove(item);
        }

        #endregion ICollection<DBSchemaStoredProcedureParameterDefinition> Members
    }
}