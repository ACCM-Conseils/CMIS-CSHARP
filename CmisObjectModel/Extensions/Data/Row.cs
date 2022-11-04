using System;
using System.Collections.Generic;
using sc = System.ComponentModel;
using System.Data;
using System.Runtime.CompilerServices;
using sx = System.Xml;
using sxs = System.Xml.Serialization;
using cac = CmisObjectModel.Attributes.CmisTypeInfoAttribute;
// ***********************************************************************************************************************
// * Project: CmisObjectModelLibrary
// * Copyright (c) 2014, Brügmann Software GmbH, Papenburg, All rights reserved
// *
// * Contact: opensource<at>patorg.de
// * 
// * CmisObjectModelLibrary is a VB.NET implementation of the Content Management Interoperability Services (CMIS) standard
// *
// * This file is part of CmisObjectModelLibrary.
// * 
// * This library is free software; you can redistribute it and/or
// * modify it under the terms of the GNU Lesser General Public
// * License as published by the Free Software Foundation; either
// * version 3.0 of the License, or (at your option) any later version.
// *
// * This library is distributed in the hope that it will be useful,
// * but WITHOUT ANY WARRANTY; without even the implied warranty of
// * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// * Lesser General Public License for more details.
// *
// * You should have received a copy of the GNU Lesser General Public
// * License along with this library (lgpl.txt).
// * If not, see <http://www.gnu.org/licenses/lgpl.txt>.
// ***********************************************************************************************************************
using CmisObjectModel.Common;
using CmisObjectModel.Constants;
/* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.Extensions.Data
{
    [sxs.XmlRoot("row", Namespace = Namespaces.com)]
    [cac("com:row", null, "row")]
    [Attributes.JavaScriptConverter(typeof(JSON.Extensions.Data.RowConverter))]
    public class Row : Serialization.XmlSerializable
    {

        #region Constructors
        public Row()
        {

            _rowIndex = new RowIndexHelper(this);
        }

        public Row(params Core.Properties.cmisProperty[] properties)
        {
            _rowIndex = new RowIndexHelper(this);
            if (properties is not null)
            {
                _properties = new Core.Collections.cmisPropertiesType(properties);
            }
        }

        internal Row(RowCollection owner, params Core.Properties.cmisProperty[] properties) : this(properties)
        {
            SetOwner(owner);
        }
        #endregion

        #region Helper classes
        private class RowIndexHelper : Core.Properties.cmisPropertyInteger
        {

            public RowIndexHelper(Row row)
            {
                _values = new long[] { int.MaxValue };
                _row = row;
                RefreshRowIndex();
            }

            private Core.Properties.cmisPropertyInteger __innerProperty;

            private Core.Properties.cmisPropertyInteger _innerProperty
            {
                [MethodImpl(MethodImplOptions.Synchronized)]
                get
                {
                    return __innerProperty;
                }

                [MethodImpl(MethodImplOptions.Synchronized)]
                set
                {
                    if (__innerProperty != null)
                    {
                        __innerProperty.PropertyChanged -= ValueChanged;
                    }

                    __innerProperty = value;
                    if (__innerProperty != null)
                    {
                        __innerProperty.PropertyChanged += ValueChanged;
                    }
                }
            }

            /// <summary>
         /// Updates row-dependencies
         /// </summary>
            private void RefreshEventHandler()
            {
                _row_Properties = _row._properties;
                _rows = _row._owner;
            }

            private void RefreshRowIndex()
            {
                string rowIndexPropertyDefinitionId;

                RefreshEventHandler();
                rowIndexPropertyDefinitionId = _rows is null ? null : _rows.RowIndexPropertyDefinitionId;
                if (_row_Properties is null || string.IsNullOrEmpty(rowIndexPropertyDefinitionId))
                {
                    _innerProperty = null;
                }
                else
                {
                    var properties = _row_Properties.GetProperties();
                    _innerProperty = properties.ContainsKey(rowIndexPropertyDefinitionId) ? properties[rowIndexPropertyDefinitionId] as Core.Properties.cmisPropertyInteger : null;
                }
                if (_innerProperty is not null)
                    Value = _innerProperty.Value;
            }

            /// <summary>
         /// Observes events that may involve a change of the rowIndex value
         /// </summary>
            private void RefreshRowIndex(object sender, sc.PropertyChangedEventArgs e)
            {
                // the rowIndex perhaps changed, if
                // - the ownership of the row changed
                // - the properties of the row or the properties-array of _row_Properties changed
                // - the rowIndexPropertyDefinitionId
                if (string.Compare(e.PropertyName, "Owner", true) == 0 || string.Compare(e.PropertyName, "Properties", true) == 0 || string.Compare(e.PropertyName, "RowIndexPropertyDefinitionId", true) == 0)
                {
                    RefreshRowIndex();
                }
            }

            private Row __row;

            private Row _row
            {
                [MethodImpl(MethodImplOptions.Synchronized)]
                get
                {
                    return __row;
                }

                [MethodImpl(MethodImplOptions.Synchronized)]
                set
                {
                    if (__row != null)
                    {
                        __row.PropertyChanged -= RefreshRowIndex;
                    }

                    __row = value;
                    if (__row != null)
                    {
                        __row.PropertyChanged += RefreshRowIndex;
                    }
                }
            }
            private Core.Collections.cmisPropertiesType __row_Properties;

            private Core.Collections.cmisPropertiesType _row_Properties
            {
                [MethodImpl(MethodImplOptions.Synchronized)]
                get
                {
                    return __row_Properties;
                }

                [MethodImpl(MethodImplOptions.Synchronized)]
                set
                {
                    if (__row_Properties != null)
                    {
                        __row_Properties.PropertyChanged -= RefreshRowIndex;
                    }

                    __row_Properties = value;
                    if (__row_Properties != null)
                    {
                        __row_Properties.PropertyChanged += RefreshRowIndex;
                    }
                }
            }
            private RowCollection __rows;

            private RowCollection _rows
            {
                [MethodImpl(MethodImplOptions.Synchronized)]
                get
                {
                    return __rows;
                }

                [MethodImpl(MethodImplOptions.Synchronized)]
                set
                {
                    if (__rows != null)
                    {
                        __rows.PropertyChanged -= RefreshRowIndex;
                    }

                    __rows = value;
                    if (__rows != null)
                    {
                        __rows.PropertyChanged += RefreshRowIndex;
                    }
                }
            }
            private long? _uncommittedValue = default;

            public override long Value
            {
                get
                {
                    return _uncommittedValue.HasValue ? _uncommittedValue.Value : base.Value;
                }
                set
                {
                    if (base.Value != value)
                    {
                        if (_rows is null || _rows.IsBusy)
                        {
                            base.Value = value;
                        }
                        else
                        {
                            // respect bounds
                            value = _rows.GetValidRowIndex((int)value);
                            _uncommittedValue = base.Value;
                            base.Value = value;
                            // refresh position in rowCollection
                            _rows.MoveRow(_row, (int)value);
                            _uncommittedValue = default;
                        }
                        // update innerProperty, if available and necessary
                        if (_innerProperty is not null && _innerProperty.Value != value)
                            _innerProperty.Value = value;
                    }
                }
            }

            private void ValueChanged(object sender, sc.PropertyChangedEventArgs e)
            {
                if (_innerProperty is not null)
                {
                    Value = _innerProperty.Value;
                }
            }
        }
        #endregion

        #region IXmlSerializable
        protected override void ReadAttributes(sx.XmlReader reader)
        {
        }

        protected override void ReadXmlCore(sx.XmlReader reader, Serialization.XmlAttributeOverrides attributeOverrides)
        {
            _rowState = ReadEnum(reader, attributeOverrides, "rowState", Namespaces.com, _rowState);
            _properties = Read(reader, attributeOverrides, "properties", Namespaces.cmis, GenericXmlSerializableFactory<Core.Collections.cmisPropertiesType>);
            switch (_rowState)
            {
                case DataRowState.Deleted:
                case DataRowState.Modified:
                    {
                        _originalProperties = Read(reader, attributeOverrides, "originalProperties", Namespaces.com, GenericXmlSerializableFactory<Core.Collections.cmisPropertiesType>);
                        break;
                    }
                case DataRowState.Unchanged:
                    {
                        if (_properties is not null)
                        {
                            _originalProperties = (Core.Collections.cmisPropertiesType)_properties.Copy();
                        }
                        AddHandler();
                        break;
                    }
            }
        }

        protected override void WriteXmlCore(sx.XmlWriter writer, Serialization.XmlAttributeOverrides attributeOverrides)
        {
            WriteElement(writer, attributeOverrides, "rowState", Namespaces.com, _rowState.GetName());
            if (_propertiesSupport.Contains(_rowState))
                WriteElement(writer, attributeOverrides, "properties", Namespaces.cmis, _properties);
            // serialization of originalProperties only for states which support differences between
            // properties and originalProperties
            if (_rowState == DataRowState.Deleted || _rowState == DataRowState.Modified)
            {
                WriteElement(writer, attributeOverrides, "originalProperties", Namespaces.com, _originalProperties);
            }
        }
        #endregion

        #region Observe Properties
        private List<Serialization.XmlSerializable> _observedSerializables = new List<Serialization.XmlSerializable>();
        /// <summary>
      /// Setup the necessary event handlers to observe the rowState
      /// </summary>
      /// <remarks></remarks>
        private void AddHandler()
        {
            RemoveHandler();
            if (_properties is not null)
            {
                AddHandler(_properties);
                if (_properties.Properties is not null)
                {
                    foreach (Core.Properties.cmisProperty prop in _properties.Properties)
                    {
                        if (prop is not null)
                            AddHandler(prop);
                    }
                }
            }
        }
        private void AddHandler(Serialization.XmlSerializable observedSerializable)
        {
            if (observedSerializable is Core.Collections.cmisPropertiesType)
            {
                observedSerializable.AddHandler(_observedSerializable_PropertyChanged, "Properties");
            }
            else
            {
                observedSerializable.AddHandler(_observedSerializable_PropertyChanged, "Values");
            }
            _observedSerializables.Add(observedSerializable);
        }

        /// <summary>
      /// Event-Dispatcher
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      /// <remarks></remarks>
        private void _observedSerializable_PropertyChanged(object sender, sc.PropertyChangedEventArgs e)
        {
            SetRowState(DataRowState.Modified);
        }

        /// <summary>
      /// Removes necessary event handlers to observe the rowState
      /// </summary>
      /// <remarks></remarks>
        private void RemoveHandler()
        {
            foreach (Serialization.XmlSerializable observedSerializable in _observedSerializables)
            {
                if (observedSerializable is Core.Collections.cmisPropertiesType)
                {
                    observedSerializable.RemoveHandler(_observedSerializable_PropertyChanged, "Properties");
                }
                else
                {
                    observedSerializable.RemoveHandler(_observedSerializable_PropertyChanged, "Values");
                }
            }
            _observedSerializables.Clear();
        }
        #endregion

        #region Methods to modify the row state
        public void AcceptChanges()
        {
            SetRowState(DataRowState.Unchanged);
        }

        public void Delete()
        {
            SetRowState(DataRowState.Deleted);
        }

        public void RejectChanges()
        {
            _properties = _originalProperties;
            SetRowState(DataRowState.Unchanged);
        }

        public event EventHandler RowStateChanged;

        public void SetAdded()
        {
            SetRowState(DataRowState.Added);
        }

        public void SetModified()
        {
            SetRowState(DataRowState.Modified);
        }

        private void SetRowState(DataRowState value)
        {
            if (value != _rowState)
            {
                var oldValue = _rowState;
                switch (value)
                {
                    case DataRowState.Added:
                        {
                            if (oldValue == DataRowState.Deleted)
                                _properties = _originalProperties;
                            _originalProperties = null;
                            RemoveHandler();
                            break;
                        }
                    case DataRowState.Deleted:
                        {
                            if (oldValue == DataRowState.Added)
                            {
                                value = DataRowState.Detached;
                            }
                            else
                            {
                                _properties = null;
                            }
                            RemoveHandler();
                            break;
                        }
                    case DataRowState.Modified:
                        {
                            RemoveHandler();
                            break;
                        }
                    case DataRowState.Unchanged:
                        {
                            _originalProperties = _properties is null ? null : (Core.Collections.cmisPropertiesType)_properties.Copy();
                            AddHandler();
                            break;
                        }
                }
                _rowState = value;
                OnPropertyChanged("RowState", value, oldValue);
                RowStateChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        #endregion

        internal void MapProperties(CmisObjectModel.Data.Mapper mapper, enumMapDirection direction, Dictionary<Core.Properties.cmisProperty, object[]> rollbackSettings)
        {
            if (_originalProperties is not null)
                mapper.MapProperties(_originalProperties, direction, rollbackSettings);
            if (_properties is not null)
                mapper.MapProperties(_properties, direction, rollbackSettings);
        }

        private static HashSet<DataRowState> _originalPropertiesSupport = new HashSet<DataRowState>() { DataRowState.Deleted, DataRowState.Modified, DataRowState.Unchanged };
        private Core.Collections.cmisPropertiesType _originalProperties;
        /// <summary>
      /// Returns a copy of the originalProperties if available, otherwise null
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public Core.Collections.cmisPropertiesType GetOriginalProperties()
        {
            return _originalProperties is not null && _originalPropertiesSupport.Contains(_rowState) ? (Core.Collections.cmisPropertiesType)_originalProperties.Copy() : null;
        }

        private RowCollection _owner;
        /// <summary>
      /// Returns the row-collection the current instance belongs to
      /// </summary>
      /// <value></value>
      /// <returns></returns>
      /// <remarks></remarks>
        [System.Web.Script.Serialization.ScriptIgnore()]
        public RowCollection Owner
        {
            get
            {
                return _owner;
            }
        }
        internal void SetOwner(RowCollection owner)
        {
            if (!ReferenceEquals(_owner, owner))
            {
                var oldValue = _owner;
                _owner = owner;
                OnPropertyChanged("Owner", owner, oldValue);
            }
        }

        private static HashSet<DataRowState> _propertiesSupport = new HashSet<DataRowState>() { DataRowState.Added, DataRowState.Detached, DataRowState.Modified, DataRowState.Unchanged };
        protected Core.Collections.cmisPropertiesType _properties;
        /// <summary>
      /// The current properties of this instance
      /// </summary>
      /// <value></value>
      /// <returns></returns>
      /// <remarks></remarks>
        public virtual Core.Collections.cmisPropertiesType Properties
        {
            get
            {
                return _propertiesSupport.Contains(_rowState) ? _properties : null;
            }
            set
            {
                if (!ReferenceEquals(value, _properties) && _propertiesSupport.Contains(_rowState))
                {
                    var oldValue = _properties;
                    _properties = value;
                    OnPropertyChanged("Properties", value, oldValue);
                    if (_rowState == DataRowState.Unchanged)
                    {
                        SetRowState(DataRowState.Modified);
                    }
                    else if (_observedSerializables.Count > 0)
                    {
                        RemoveHandler();
                    }
                }
            }
        } // Properties

        private RowIndexHelper _rowIndex;
        public int RowIndex
        {
            get
            {
                return (int)_rowIndex.Value;
            }
            set
            {
                _rowIndex.Value = value;
            }
        }

        private DataRowState _rowState = DataRowState.Detached;
        /// <summary>
      /// The RowState of this instance
      /// </summary>
      /// <value></value>
      /// <returns></returns>
      /// <remarks></remarks>
        public DataRowState RowState
        {
            get
            {
                return _rowState;
            }
        } // RowState


        /// <summary>
      /// Initializes instance with given parameters
      /// </summary>
        protected static void SilentInitialization(Row instance, Core.Collections.cmisPropertiesType properties, Core.Collections.cmisPropertiesType originalProperties, DataRowState rowState)
        {
            if (instance is not null)
            {
                instance.RemoveHandler();
                instance._rowState = rowState;
                instance._properties = properties;
                instance._originalProperties = originalProperties;
                if (rowState == DataRowState.Unchanged)
                    instance.AddHandler();
            }
        }

    }
}