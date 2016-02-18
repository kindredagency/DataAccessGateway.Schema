using System.Collections;
using System.Collections.Generic;
using Framework.DataAccessGateway.Schema.DataStructure;

namespace Framework.DataAccessGateway.Schema
{
    /// <summary>
    /// Class DBSchemaTriggerDefinitionCollection.
    /// </summary>
    public class DBSchemaTriggerDefinitionCollection : IList<DBSchemaTriggerDefinition>
    {
        #region Private Variables

        private readonly List<DBSchemaTriggerDefinition> triggers = new List<DBSchemaTriggerDefinition>();

        #endregion Private Variables

        #region IEnumerable<DBSchemaTriggerDefinition> Members

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<DBSchemaTriggerDefinition> GetEnumerator()
        {
            return triggers.GetEnumerator();
        }

        #endregion IEnumerable<DBSchemaTriggerDefinition> Members

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return triggers.GetEnumerator();
        }

        #endregion IEnumerable Members

        #region IList<DBSchemaTriggerDefinition> Members

        /// <summary>
        /// Indexes the of.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>System.Int32.</returns>
        public int IndexOf(DBSchemaTriggerDefinition item)
        {
            return triggers.IndexOf(item);
        }

        /// <summary>
        /// Inserts the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="item">The item.</param>
        public void Insert(int index, DBSchemaTriggerDefinition item)
        {
            triggers.Insert(index, item);
        }

        /// <summary>
        /// Removes at.
        /// </summary>
        /// <param name="index">The index.</param>
        public void RemoveAt(int index)
        {
            triggers.RemoveAt(index);
        }

        /// <summary>
        /// Gets or sets the <see cref="DBSchemaTriggerDefinition"/> at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>DBSchemaTriggerDefinition.</returns>
        public DBSchemaTriggerDefinition this[int index]
        {
            get { return triggers[index]; }
            set { triggers[index] = value; }
        }

        /// <summary>
        /// Gets or sets the <see cref="DBSchemaTriggerDefinition"/> with the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>DBSchemaTriggerDefinition.</returns>
        public DBSchemaTriggerDefinition this[string name]
        {
            get
            {
                foreach (var dbSchemaTriggersDefinition in triggers)
                {
                    if (dbSchemaTriggersDefinition.TriggerName == name)
                    {
                        return dbSchemaTriggersDefinition;
                    }
                }

                return null;
            }
            set
            {
                DBSchemaTriggerDefinition tempDbSchemaTriggerDefinition = null;
                foreach (var dbSchemaTriggerDefinition in triggers)
                {
                    if (dbSchemaTriggerDefinition.TriggerName == name)
                    {
                        tempDbSchemaTriggerDefinition = dbSchemaTriggerDefinition;
                    }
                }

                tempDbSchemaTriggerDefinition = value;
            }
        }

        #endregion IList<DBSchemaTriggerDefinition> Members

        #region ICollection<DBSchemaTriggerDefinition> Members

        /// <summary>
        /// Adds the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Add(DBSchemaTriggerDefinition item)
        {
            triggers.Add(item);
        }

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        public void Clear()
        {
            triggers.Clear();
        }

        /// <summary>
        /// Determines whether [contains] [the specified item].
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns><c>true</c> if [contains] [the specified item]; otherwise, <c>false</c>.</returns>
        public bool Contains(DBSchemaTriggerDefinition item)
        {
            return triggers.Contains(item);
        }

        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="arrayIndex">Index of the array.</param>
        public void CopyTo(DBSchemaTriggerDefinition[] array, int arrayIndex)
        {
            triggers.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <value>The count.</value>
        public int Count
        {
            get { return triggers.Count; }
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
        public bool Remove(DBSchemaTriggerDefinition item)
        {
            return triggers.Remove(item);
        }

        #endregion ICollection<DBSchemaTriggerDefinition> Members
    }
}