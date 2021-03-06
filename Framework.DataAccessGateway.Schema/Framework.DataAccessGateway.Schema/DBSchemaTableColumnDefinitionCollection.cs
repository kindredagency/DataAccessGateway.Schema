﻿using System.Collections;
using System.Collections.Generic;

namespace Framework.DataAccessGateway.Schema
{
    /// <summary>
    /// Class DBSchemaTableColumnDefinitionCollection.
    /// </summary>
    public class DBSchemaTableColumnDefinitionCollection : IList<DBSchemaTableColumnDefinition>
    {
        private readonly List<DBSchemaTableColumnDefinition> columnDefinitionList = new List<DBSchemaTableColumnDefinition>();      

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<DBSchemaTableColumnDefinition> GetEnumerator()
        {
            return columnDefinitionList.GetEnumerator();
        }       

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return columnDefinitionList.GetEnumerator();
        }      

        /// <summary>
        /// Indexes the of.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>System.Int32.</returns>
        public int IndexOf(DBSchemaTableColumnDefinition item)
        {
            return columnDefinitionList.IndexOf(item);
        }

        /// <summary>
        /// Inserts the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="item">The item.</param>
        public void Insert(int index, DBSchemaTableColumnDefinition item)
        {
            columnDefinitionList.Insert(index, item);
        }

        /// <summary>
        /// Removes at.
        /// </summary>
        /// <param name="index">The index.</param>
        public void RemoveAt(int index)
        {
            columnDefinitionList.RemoveAt(index);
        }

        /// <summary>
        /// Gets or sets the <see cref="DBSchemaTableColumnDefinition"/> at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>DBSchemaTableColumnDefinition.</returns>
        public DBSchemaTableColumnDefinition this[int index]
        {
            get { return columnDefinitionList[index]; }
            set { columnDefinitionList[index] = value; }
        }

        /// <summary>
        /// Gets or sets the <see cref="DBSchemaTableColumnDefinition"/> with the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>DBSchemaTableColumnDefinition.</returns>
        public DBSchemaTableColumnDefinition this[string name]
        {
            get
            {
                foreach (var dbSchemaTableColumnDefinition in columnDefinitionList)
                {
                    if (dbSchemaTableColumnDefinition.ColumnName == name)
                    {
                        return dbSchemaTableColumnDefinition;
                    }
                }

                return null;
            }
            set
            {
                DBSchemaTableColumnDefinition tempDbSchemaTableDefinition = null;
                foreach (var dbSchemaTableColumnDefinition in columnDefinitionList)
                {
                    if (dbSchemaTableColumnDefinition.ColumnName == name)
                    {
                        tempDbSchemaTableDefinition = dbSchemaTableColumnDefinition;
                    }
                }
                tempDbSchemaTableDefinition = value;
            }
        }
       
        /// <summary>
        /// Adds the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Add(DBSchemaTableColumnDefinition item)
        {
            columnDefinitionList.Add(item);
        }

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        public void Clear()
        {
            columnDefinitionList.Clear();
        }

        /// <summary>
        /// Determines whether [contains] [the specified item].
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns><c>true</c> if [contains] [the specified item]; otherwise, <c>false</c>.</returns>
        public bool Contains(DBSchemaTableColumnDefinition item)
        {
            foreach (var y in columnDefinitionList)
            {
                var allValuesMatch = true;

                if (y.ColumnName != item.ColumnName)
                {
                    allValuesMatch = false;
                }

                if (y.DataType != item.DataType)
                {
                    allValuesMatch = false;
                }

                if (y.DefaultValue != item.DefaultValue)
                {
                    allValuesMatch = false;
                }

                if (y.IdentityIncrement != item.IdentityIncrement)
                {
                    allValuesMatch = false;
                }

                if (y.IdentitySeed != item.IdentitySeed)
                {
                    allValuesMatch = false;
                }

                if (y.IsIdentity != item.IsIdentity)
                {
                    allValuesMatch = false;
                }

                if (y.IsNullable != item.IsNullable)
                {
                    allValuesMatch = false;
                }

                if (y.Length != item.Length)
                {
                    allValuesMatch = false;
                }

                if (allValuesMatch)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="arrayIndex">Index of the array.</param>
        public void CopyTo(DBSchemaTableColumnDefinition[] array, int arrayIndex)
        {
            columnDefinitionList.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <value>The count.</value>
        public int Count
        {
            get { return columnDefinitionList.Count; }
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
        public bool Remove(DBSchemaTableColumnDefinition item)
        {
            return columnDefinitionList.Remove(item);
        }
        
    }
}