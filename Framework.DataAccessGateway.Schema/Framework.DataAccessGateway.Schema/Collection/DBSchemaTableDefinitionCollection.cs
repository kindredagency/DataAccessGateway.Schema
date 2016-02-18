﻿using System.Collections;
using System.Collections.Generic;
using Framework.DataAccessGateway.Schema.DataStructure;

namespace Framework.DataAccessGateway.Schema.Collection
{
    /// <summary>
    /// Class DBSchemaTableDefinitionCollection.
    /// </summary>
    public class DBSchemaTableDefinitionCollection : IList<DBSchemaTableDefinition>
    {
        #region Private Variables

        private readonly List<DBSchemaTableDefinition> tables = new List<DBSchemaTableDefinition>();

        #endregion Private Variables

        #region IEnumerable<DBSchemaTableDefinition> Members

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<DBSchemaTableDefinition> GetEnumerator()
        {
            return tables.GetEnumerator();
        }

        #endregion IEnumerable<DBSchemaTableDefinition> Members

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return tables.GetEnumerator();
        }

        #endregion IEnumerable Members

        #region IList<DBSchemaTableDefinition> Members

        /// <summary>
        /// Indexes the of.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>System.Int32.</returns>
        public int IndexOf(DBSchemaTableDefinition item)
        {
            return tables.IndexOf(item);
        }

        /// <summary>
        /// Inserts the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="item">The item.</param>
        public void Insert(int index, DBSchemaTableDefinition item)
        {
            tables.Insert(index, item);
        }

        /// <summary>
        /// Removes at.
        /// </summary>
        /// <param name="index">The index.</param>
        public void RemoveAt(int index)
        {
            tables.RemoveAt(index);
        }

        /// <summary>
        /// Gets or sets the <see cref="DBSchemaTableDefinition"/> at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>DBSchemaTableDefinition.</returns>
        public DBSchemaTableDefinition this[int index]
        {
            get { return tables[index]; }
            set { tables[index] = value; }
        }

        /// <summary>
        /// Gets or sets the <see cref="DBSchemaTableDefinition"/> with the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>DBSchemaTableDefinition.</returns>
        public DBSchemaTableDefinition this[string name]
        {
            get
            {
                foreach (var dbSchemaTableDefinition in tables)
                {
                    if (dbSchemaTableDefinition.TableName == name)
                    {
                        return dbSchemaTableDefinition;
                    }
                }

                return null;
            }
            set
            {
                DBSchemaTableDefinition tempDbSchemaTableDefinition = null;
                foreach (var dbSchemaTableDefinition in tables)
                {
                    if (dbSchemaTableDefinition.TableName == name)
                    {
                        tempDbSchemaTableDefinition = dbSchemaTableDefinition;
                    }
                }
                tempDbSchemaTableDefinition = value;
            }
        }

        #endregion IList<DBSchemaTableDefinition> Members

        #region ICollection<DBSchemaTableDefinition> Members

        /// <summary>
        /// Adds the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Add(DBSchemaTableDefinition item)
        {
            tables.Add(item);
        }

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        public void Clear()
        {
            tables.Clear();
        }

        /// <summary>
        /// Determines whether [contains] [the specified item].
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns><c>true</c> if [contains] [the specified item]; otherwise, <c>false</c>.</returns>
        public bool Contains(DBSchemaTableDefinition item)
        {
            return tables.Contains(item);
        }

        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="arrayIndex">Index of the array.</param>
        public void CopyTo(DBSchemaTableDefinition[] array, int arrayIndex)
        {
            tables.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <value>The count.</value>
        public int Count
        {
            get { return tables.Count; }
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
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool Remove(DBSchemaTableDefinition item)
        {
            return tables.Remove(item);
        }

        #endregion ICollection<DBSchemaTableDefinition> Members
    }
}