using System;
using System.Collections.Generic;
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

namespace CmisObjectModel.Data
{
    /// <summary>
   /// Simple mapper 
   /// </summary>
   /// <remarks></remarks>
    public class Mapper
    {

        public Mapper(enumMapDirection allowedDirections = enumMapDirection.none)
        {
            _allowedDirections = allowedDirections;
        }

        private enumMapDirection _allowedDirections;
        public enumMapDirection AllowedDirections
        {
            get
            {
                return _allowedDirections;
            }
            set
            {
                _allowedDirections = value & enumMapDirection.bidirectional;
            }
        }

        /// <summary>
      /// Remove all PropertyValueConverters within this mapper-instance
      /// </summary>
        public void ClearPropertyValueConverters()
        {
            _propertyValueConverters.Clear();
        }

        /// <summary>
      /// Maps the values of properties for the remote system (direction = outgoing) or current system (direction = incoming) and
      /// returns a delegate to rollback the mapping
      /// </summary>
      /// <param name="properties"></param>
      /// <param name="direction"></param>
      /// <returns>Delegate to rollback the mapping</returns>
      /// <remarks></remarks>
        public Action MapProperties(Core.Collections.cmisPropertiesType properties, enumMapDirection direction)
        {
            var rollbackSettings = new Dictionary<Core.Properties.cmisProperty, object[]>();

            direction = direction & _allowedDirections;
            if (properties is not null && direction != enumMapDirection.none)
            {
                MapProperties(properties, direction, rollbackSettings);
            }
            if (rollbackSettings.Count == 0)
            {
                return null;
            }
            else
            {
                return new Action(() => { foreach (KeyValuePair<Core.Properties.cmisProperty, object[]> de in rollbackSettings) de.Key.SetValuesSilent(de.Value); });
            }
        }
        internal void MapProperties(Core.Collections.cmisPropertiesType properties, enumMapDirection direction, Dictionary<Core.Properties.cmisProperty, object[]> rollbackSettings)
        {
            // property mapping for current properties-instance
            if (properties.Properties is not null)
            {
                foreach (Core.Properties.cmisProperty property in properties)
                {
                    var converter = property is null || rollbackSettings.ContainsKey(property) ? null : get_PropertyValueConverter(property.PropertyDefinitionId);
                    if (converter is not null)
                    {
                        rollbackSettings.Add(property, property.SetValuesSilent(direction == enumMapDirection.incoming ? converter.Convert(property.Values) : converter.ConvertBack(property.Values)));
                    }
                }
            }
            // property mapping for rowCollections
            if (properties.Extensions is not null)
            {
                foreach (Extensions.Extension extension in properties.Extensions)
                {
                    if (extension is Extensions.Data.RowCollection)
                    {
                        Extensions.Data.RowCollection rows = (Extensions.Data.RowCollection)extension;

                        if (rows.Rows is not null)
                        {
                            foreach (Extensions.Data.Row row in rows.Rows)
                            {
                                if (row is not null)
                                    row.MapProperties(this, direction, rollbackSettings);
                            }
                        }
                    }
                }
            }
        }

        private Dictionary<string, PropertyValueConverter> _propertyValueConverters = new Dictionary<string, PropertyValueConverter>();
        /// <summary>
      /// Gets or sets the propertyValueConverter for a specified propertyDefinitionId
      /// </summary>
      /// <param name="propertyDefinitionId"></param>
      /// <value></value>
      /// <returns></returns>
      /// <remarks></remarks>
        public PropertyValueConverter get_PropertyValueConverter(string propertyDefinitionId)
        {
            return string.IsNullOrEmpty(propertyDefinitionId) || !_propertyValueConverters.ContainsKey(propertyDefinitionId) ? null : _propertyValueConverters[propertyDefinitionId];
            // for mapper instances there MUST NOT be a type-conversion defined
            // for mapper instances there MUST NOT be a type-conversion defined
        }

        public void set_PropertyValueConverter(string propertyDefinitionId, PropertyValueConverter value)
        {
            if (!string.IsNullOrEmpty(propertyDefinitionId))
            {
                if (_propertyValueConverters.ContainsKey(propertyDefinitionId))
                {
                    if (value is null)
                    {
                        _propertyValueConverters.Remove(propertyDefinitionId);
                    }
                    else if (ReferenceEquals(value.LocalType, value.RemoteType))
                    {
                        _propertyValueConverters[propertyDefinitionId] = value;
                    }
                }
                else if (value is not null && ReferenceEquals(value.LocalType, value.RemoteType))
                {
                    _propertyValueConverters.Add(propertyDefinitionId, value);
                }
            }
        }

    }
}