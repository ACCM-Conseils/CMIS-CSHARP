using System;
using System.Collections.Generic;
using System.Data;
using sx = System.Xml;
using sxs = System.Xml.Serialization;
using cac = CmisObjectModel.Attributes.CmisTypeInfoAttribute;
using CmisObjectModel.Constants;

namespace CmisObjectModel.Extensions.Data
{
    /// <summary>
   /// Support for structured database contents
   /// </summary>
   /// <remarks></remarks>
    [sxs.XmlRoot("rowCollection", Namespace = Namespaces.com)]
    [cac("com:rowCollection", null, "rowCollection")]
    [Attributes.JavaScriptConverter(typeof(JSON.Extensions.Data.RowCollectionConverter))]
    public class RowCollection : Extension
    {

        #region IXmlSerializable
        private static Dictionary<string, Action<RowCollection, string>> _setter = new Dictionary<string, Action<RowCollection, string>>() { { "rowindexpropertydefinitionid", SetRowIndexPropertyDefinitionId }, { "rowtypeid", SetRowTypeId }, { "tablename", SetTableName } }; // _setter

        /// <summary>
      /// Deserialization of all properties stored in attributes
      /// </summary>
      /// <param name="reader"></param>
      /// <remarks></remarks>
        protected override void ReadAttributes(sx.XmlReader reader)
        {
            // at least one property is serialized in an attribute-value
            if (_setter.Count > 0)
            {
                for (int attributeIndex = 0, loopTo = reader.AttributeCount - 1; attributeIndex <= loopTo; attributeIndex++)
                {
                    reader.MoveToAttribute(attributeIndex);
                    // attribute name
                    string key = reader.Name.ToLowerInvariant();
                    if (_setter.ContainsKey(key))
                        _setter[key].Invoke(this, reader.GetAttribute(attributeIndex));
                }
            }
        }

        protected override void ReadXmlCore(sx.XmlReader reader, Serialization.XmlAttributeOverrides attributeOverrides)
        {
            var rows = ReadArray(reader, attributeOverrides, "row", Namespaces.com, GenericXmlSerializableFactory<Row>);

            try
            {
                _busy += 1;
                if (rows is not null)
                {
                    foreach (Row row in rows)
                    {
                        if (row is not null)
                        {
                            if (row.RowState == DataRowState.Detached)
                                row.SetAdded();
                            row.SetOwner(this);
                            _rows.Add(row);
                            row.RowStateChanged += _row_RowStateChanged;
                        }
                    }
                }
            }
            finally
            {
                _busy -= 1;
            }
        }

        protected override void WriteXmlCore(sx.XmlWriter writer, Serialization.XmlAttributeOverrides attributeOverrides)
        {
            WriteAttribute(writer, attributeOverrides, "tableName", null, _tableName);
            WriteAttribute(writer, attributeOverrides, "rowTypeId", null, _rowTypeId);
            WriteAttribute(writer, attributeOverrides, "rowIndexPropertyDefinitionId", null, _rowIndexPropertyDefinitionId);
            if (_rows.Count > 0)
                WriteArray(writer, attributeOverrides, "row", Namespaces.com, _rows.ToArray());
        }
        #endregion

        #region LoadData
        /// <summary>
      /// Informs the collection that data loading starts
      /// </summary>
        public void BeginLoadData()
        {
            _busy += 1;
        }

        private int _busy = 0;

        /// <summary>
      /// Informs the collection that data loading is completed
      /// </summary>
        public void EndLoadData()
        {
            if (_busy == 1)
            {
                SortRows();
                _busy = 0;
                if (_rows.Count > 0)
                    OnPropertyChanged("Rows");
            }
            else if (_busy > 1)
            {
                _busy -= 1;
            }
        }

        public bool IsBusy
        {
            get
            {
                return _busy > 0;
            }
        }
        #endregion

        /// <summary>
      /// Sets all rows to RowState unChanged
      /// </summary>
      /// <remarks></remarks>
        public void AcceptChanges()
        {
            foreach (Row row in _rows)
                row.AcceptChanges();
        }

        /// <summary>
      /// Adds a row instance to the collection if the has no owner by now
      /// </summary>
      /// <param name="row"></param>
      /// <remarks></remarks>
        public void AddRow(Row row)
        {
            InsertRow(_rows.Count, row);
        }

        /// <summary>
      /// Returns rowIndex-value in valid range
      /// </summary>
        internal int GetValidRowIndex(int proposedRowIndex)
        {
            return Math.Min(Math.Max(0, proposedRowIndex), _rows.Count - 1);
        }

        public void InsertRow(int rowIndex, Row row)
        {
            if (row is not null && row.Owner is null)
            {
                try
                {
                    _busy += 1;
                    rowIndex = Math.Min(Math.Max(0, rowIndex), _rows.Count);
                    row.SetOwner(this);
                    row.SetAdded();
                    if (rowIndex >= _rows.Count)
                    {
                        _rows.Add(row);
                    }
                    else
                    {
                        _rows.Insert(rowIndex, row);
                    }
                    row.RowStateChanged += _row_RowStateChanged;
                    if (_busy == 1)
                    {
                        for (int index = rowIndex, loopTo = _rows.Count - 1; index <= loopTo; index++)
                            _rows[index].RowIndex = index;
                        OnPropertyChanged("Rows");
                    }
                }
                finally
                {
                    _busy -= 1;
                }
            }
        }

        /// <summary>
      /// Build a new row with the given properties
      /// </summary>
      /// <param name="fAcceptChanges"></param>
      /// <param name="properties"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public Row LoadRow(bool fAcceptChanges, params Core.Properties.cmisProperty[] properties)
        {
            try
            {
                _busy += 1;

                var retVal = new Row(this, properties);

                if (fAcceptChanges)
                {
                    retVal.AcceptChanges();
                }
                else
                {
                    retVal.SetAdded();
                }
                _rows.Add(retVal);
                retVal.RowStateChanged += _row_RowStateChanged;
                if (_busy == 1)
                {
                    retVal.RowIndex = _rows.Count - 1;
                    OnPropertyChanged("Rows");
                }

                return retVal;
            }
            finally
            {
                _busy -= 1;
            }
        }

        /// <summary>
      /// Moves row to the specified rowIndex
      /// </summary>
        public void MoveRow(Row row, int rowIndex)
        {
            int currentRowIndex;

            if (_busy == 0 && row is not null && ReferenceEquals(row.Owner, this))
            {
                currentRowIndex = GetValidRowIndex(row.RowIndex);
                rowIndex = GetValidRowIndex(rowIndex);
                if (currentRowIndex < 0 || !ReferenceEquals(_rows[currentRowIndex], row))
                {
                    currentRowIndex = _rows.IndexOf(row);
                }
                if (currentRowIndex >= 0 && currentRowIndex != rowIndex)
                {
                    try
                    {
                        _busy += 1;
                        if (currentRowIndex < rowIndex)
                        {
                            for (int index = currentRowIndex, loopTo = rowIndex - 1; index <= loopTo; index++)
                            {
                                _rows[index] = _rows[index + 1];
                                _rows[index].RowIndex = index;
                            }
                        }
                        else
                        {
                            for (int index = currentRowIndex, loopTo1 = rowIndex + 1; index >= loopTo1; index -= 1)
                            {
                                _rows[index] = _rows[index - 1];
                                _rows[index].RowIndex = index;
                            }
                        }
                        _rows[rowIndex] = row;
                        row.RowIndex = rowIndex;
                    }
                    finally
                    {
                        _busy -= 1;
                    }
                }
            }
        }

        /// <summary>
      /// Removes a row if the rowState turned to Detached
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      /// <remarks></remarks>
        private void _row_RowStateChanged(object sender, EventArgs e)
        {
            Row row = (Row)sender;

            if (row.RowState == DataRowState.Detached)
            {
                int rowIndex = GetValidRowIndex(row.RowIndex);

                row.RowStateChanged -= _row_RowStateChanged;
                if (rowIndex < 0 || !ReferenceEquals(_rows[rowIndex], row))
                {
                    rowIndex = _rows.IndexOf(row);
                }
                if (rowIndex >= 0)
                {
                    _rows.RemoveAt(rowIndex);
                    row.SetOwner(null);
                    try
                    {
                        _busy += 1;
                        for (int index = rowIndex, loopTo = _rows.Count - 1; index <= loopTo; index++)
                            _rows[index].RowIndex = index;
                    }
                    finally
                    {
                        _busy -= 1;
                    }
                    OnPropertyChanged("Rows");
                }
            }
        }

        private string _rowIndexPropertyDefinitionId;
        public string RowIndexPropertyDefinitionId
        {
            get
            {
                return _rowIndexPropertyDefinitionId;
            }
            set
            {
                if ((value ?? "") != (_rowIndexPropertyDefinitionId ?? ""))
                {
                    string oldValue = _rowIndexPropertyDefinitionId;
                    _rowIndexPropertyDefinitionId = value;
                    OnPropertyChanged("RowIndexPropertyDefinitionId", value, oldValue);
                }
            }
        } // RowIndexPropertyDefinitionId
        private static void SetRowIndexPropertyDefinitionId(RowCollection instance, string value)
        {
            instance._rowIndexPropertyDefinitionId = value;
        }

        private List<Row> _rows = new List<Row>();
        /// <summary>
      /// RowCollection
      /// </summary>
      /// <value></value>
      /// <returns></returns>
      /// <remarks></remarks>
        public Row[] Rows
        {
            get
            {
                return _rows.ToArray();
            }
        } // Rows

        private string _rowTypeId;
        /// <summary>
      /// TypeId to describe the properties of the row
      /// </summary>
      /// <value></value>
      /// <returns></returns>
      /// <remarks>Secondary Type recommend</remarks>
        public string RowTypeId
        {
            get
            {
                return _rowTypeId;
            }
            set
            {
                if ((_rowTypeId ?? "") != (value ?? ""))
                {
                    string oldValue = _rowTypeId;
                    _rowTypeId = value;
                    OnPropertyChanged("RowTypeId", value, oldValue);
                }
            }
        } // RowTypeId
        private static void SetRowTypeId(RowCollection instance, string value)
        {
            instance.RowTypeId = value;
        }

        /// <summary>
      /// Initializes instance with given parameters
      /// </summary>
        protected static void SilentInitialization(RowCollection instance, string rowIndexPropertyDefinitionId, string rowTypeId, string tableName, Row[] rows)
        {
            if (instance is not null)
            {
                try
                {
                    instance._busy += 1;
                    instance._rowIndexPropertyDefinitionId = rowIndexPropertyDefinitionId;
                    instance._rowTypeId = rowTypeId;
                    instance._tableName = tableName;
                    instance._rows.Clear();
                    if (rows is not null)
                    {
                        foreach (Row row in rows)
                        {
                            if (row is not null)
                            {
                                if (row.RowState == DataRowState.Detached)
                                    row.SetAdded();
                                row.SetOwner(instance);
                                instance._rows.Add(row);
                                row.RowStateChanged += instance._row_RowStateChanged;
                            }
                        }
                    }
                }
                finally
                {
                    instance._busy -= 1;
                }
            }
        }

        private void SortRows()
        {
            try
            {
                _busy += 1;
                if (!string.IsNullOrEmpty(_rowIndexPropertyDefinitionId))
                {
                    _rows.Sort((first, second) => first.RowIndex - second.RowIndex);
                }
                for (int index = 0, loopTo = _rows.Count - 1; index <= loopTo; index++)
                    _rows[index].RowIndex = index;
            }
            finally
            {
                _busy -= 1;
            }
        }

        private string _tableName;
        public string TableName
        {
            get
            {
                return _tableName;
            }
            set
            {
                if ((_tableName ?? "") != (value ?? ""))
                {
                    string oldValue = _tableName;
                    _tableName = value;
                    OnPropertyChanged("TableName", value, oldValue);
                }
            }
        } // TableName
        private static void SetTableName(RowCollection instance, string value)
        {
            instance.TableName = value;
        }

    }
}