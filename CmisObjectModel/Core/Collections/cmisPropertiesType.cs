using System.Collections;
using System.Collections.Generic;
using System.Linq;
using sxs = System.Xml.Serialization;
using ccg = CmisObjectModel.Collections.Generic;
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

namespace CmisObjectModel.Core.Collections
{
    [sxs.XmlRoot("properties", Namespace = Constants.Namespaces.cmis)]
    [Attributes.JavaScriptConverter(typeof(JSON.Collections.PropertiesConverter))]
    public partial class cmisPropertiesType : IEnumerable
    {

        public cmisPropertiesType(params Properties.cmisProperty[] properties)
        {
            _properties = new HashSet<Properties.cmisProperty>(properties, _equalityComparer);
        }

        public static implicit operator cmisPropertiesType(List<Properties.cmisProperty> value)
        {
            if (value is null)
            {
                return null;
            }
            else
            {
                return new cmisPropertiesType(value.ToArray());
            }
        }

        public static implicit operator cmisPropertiesType(Properties.cmisProperty value)
        {
            if (value is null)
            {
                return null;
            }
            else
            {
                return new cmisPropertiesType(value);
            }
        }

        public static implicit operator cmisPropertiesType(Properties.cmisProperty[] value)
        {
            if (value is null)
            {
                return null;
            }
            else
            {
                return new cmisPropertiesType(value);
            }
        }

        #region IEnumerable
        public IEnumerator GetEnumerator()
        {
            return _properties.GetEnumerator();
        }
        #endregion

        #region IXmlSerialization
        public override void ReadXml(System.Xml.XmlReader reader)
        {
            base.ReadXml(reader);

            // support: foreach cmisProperty.ExtendedProperty(DeclaringType)
            if (_properties is not null)
            {
                var objectTypeId = FindProperty(Constants.CmisPredefinedPropertyNames.ObjectTypeId);
                if (objectTypeId is not null)
                {
                    string declaringType = objectTypeId.Value as string;
                    foreach (Properties.cmisProperty property in _properties)
                    {
                        if (property is not null)
                        {
                            var extendedProperties = property.get_ExtendedProperties();
                            if (!extendedProperties.ContainsKey(Constants.ExtendedProperties.DeclaringType))
                            {
                                extendedProperties.Add(Constants.ExtendedProperties.DeclaringType, declaringType);
                            }
                        }
                    }
                }
            }
        }
        #endregion

        /// <summary>
      /// Appends a new cmisProperty-instance to the Properties-array
      /// </summary>
      /// <param name="cmisProperty"></param>
      /// <remarks></remarks>
        public bool Append(Properties.cmisProperty cmisProperty)
        {
            return AddProperty(cmisProperty);
        }

        /// <summary>
      /// Updates the entries of result: if a key exists in the properties-collection the corresponding property
      /// is copied to the value of the dictionary-entry
      /// </summary>
      /// <param name="result"></param>
      /// <param name="ignoreCase"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public Dictionary<string, Properties.cmisProperty> CopyTo(Dictionary<string, Properties.cmisProperty> result, bool ignoreCase)
        {
            var map = new Dictionary<string, string>(); // map for lowerCase to requested propertyDefinitionId

            if (result is null)
                result = new Dictionary<string, Properties.cmisProperty>();
            // mapping
            foreach (string propertyDefinitionId in result.Keys)
            {
                string key = ignoreCase ? propertyDefinitionId.ToLowerInvariant() : propertyDefinitionId;

                if (!map.ContainsKey(key))
                {
                    map.Add(key, propertyDefinitionId);
                }
            }
            // search for property-instances
            if (_properties is not null)
            {
                foreach (Properties.cmisProperty property in _properties)
                {
                    if (property is not null)
                    {
                        string propertyDefinitionId = property.PropertyDefinitionId;

                        if (!string.IsNullOrEmpty(propertyDefinitionId))
                        {
                            string key = ignoreCase ? propertyDefinitionId.ToLowerInvariant() : propertyDefinitionId;

                            if (map.ContainsKey(key))
                                result[map[key]] = property;
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
      /// Returns _properties.Length or 0, if _properties is null
      /// </summary>
      /// <value></value>
      /// <returns></returns>
      /// <remarks></remarks>
        public int Count
        {
            get
            {
                return _properties.Count;
            }
        }

        /// <summary>
      /// Search for a specified extension type
      /// </summary>
        public TExtension FindExtension<TExtension>() where TExtension : Extensions.Extension
        {
            if (_extensions is not null)
            {
                foreach (Extensions.Extension extension in _extensions)
                {
                    if (extension is TExtension)
                        return (TExtension)extension;
                }
            }

            // nothing found
            return null;
        }

        /// <summary>
      /// Returns a dictionary that contains an entry for all propertyDefinitionIds that are not null or empty
      /// </summary>
      /// <param name="ignoreCase"></param>
      /// <param name="propertyDefinitionIds"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public Dictionary<string, Properties.cmisProperty> FindProperties(bool ignoreCase, params string[] propertyDefinitionIds)
        {
            var retVal = new Dictionary<string, Properties.cmisProperty>();
            var map = new Dictionary<string, string>(); // map for lowerCase to requested propertyDefinitionId

            // the returned dictionary MUST support all requested valid propertyDefinitionIds (not null or empty)
            if (propertyDefinitionIds is not null)
            {
                foreach (string propertyDefinitionId in propertyDefinitionIds)
                {
                    if (!string.IsNullOrEmpty(propertyDefinitionId))
                    {
                        string key = ignoreCase ? propertyDefinitionId.ToLowerInvariant() : propertyDefinitionId;

                        if (!map.ContainsKey(key))
                        {
                            map.Add(key, propertyDefinitionId);
                            retVal.Add(propertyDefinitionId, null);
                        }
                    }
                }
            }
            // search for property-instances
            if (_properties is not null)
            {
                foreach (Properties.cmisProperty property in _properties)
                {
                    if (property is not null)
                    {
                        string propertyDefinitionId = property.PropertyDefinitionId;

                        if (!string.IsNullOrEmpty(propertyDefinitionId))
                        {
                            string key = ignoreCase ? propertyDefinitionId.ToLowerInvariant() : propertyDefinitionId;

                            if (map.ContainsKey(key))
                                retVal[map[key]] = property;
                        }
                    }
                }
            }

            return retVal;
        }

        public Properties.cmisProperty FindProperty(string propertyDefinitionId, bool ignoreCase = true)
        {
            if (!string.IsNullOrEmpty(propertyDefinitionId) && _properties is not null)
            {
                foreach (Properties.cmisProperty retVal in _properties)
                {
                    if (retVal is not null && string.Compare(retVal.PropertyDefinitionId, propertyDefinitionId, ignoreCase) == 0)
                        return retVal;
                }
            }

            // unable to find property
            return null;
        }

        public TResult FindProperty<TResult>(string propertyDefinitionId, bool ignoreCase = true) where TResult : Properties.cmisProperty
        {
            return FindProperty(propertyDefinitionId, ignoreCase) as TResult;
        }

        /// <summary>
      /// Returns the properties specified by the given propertyDefinitionIds
      /// </summary>
      /// <param name="propertyDefinitionIds"></param>
      /// <returns></returns>
      /// <remarks>The given propertyDefinitionIds handled casesensitive, if there is
      /// none at all, all properties of this instance will be returned</remarks>
        public Dictionary<string, Properties.cmisProperty> GetProperties(params string[] propertyDefinitionIds)
        {
            return GetProperties(enumKeySyntax.original, propertyDefinitionIds);
        }

        /// <summary>
      /// Returns the properties specified by the given propertyDefinitionIds
      /// </summary>
      /// <param name="ignoreCase">If True each propertyDefinitionId is compared case insensitive</param>
      /// <param name="propertyDefinitionIds"></param>
      /// <returns>Dictionary of all existing properties specified by propertyDefinitionsIds.
      /// Notice: if ignoreCase is set to True, then the keys of the returned dictionary are lowercase</returns>
      /// <remarks>If there are no propertyDefinitionIds defined, all properties of this instance will be returned</remarks>
        public Dictionary<string, Properties.cmisProperty> GetProperties(bool ignoreCase, params string[] propertyDefinitionIds)
        {
            return GetProperties(ignoreCase ? enumKeySyntax.lowerCase : enumKeySyntax.original, propertyDefinitionIds);
        }

        /// <summary>
      /// Returns the properties specified by the given propertyDefinitionIds
      /// </summary>
      /// <param name="keySyntax">If lowerCase-bit is set each propertyDefinitionId is compared case insensitive</param>
      /// <param name="propertyDefinitionIds"></param>
      /// <returns>Dictionary of all existing properties specified by propertyDefinitionsIds.
      /// Notice: if keySyntax is set to lowerCase, then the keys of the returned dictionary are lowercase</returns>
      /// <remarks>If there are no propertyDefinitionIds defined, all properties of this instance will be returned</remarks>
        public Dictionary<string, Properties.cmisProperty> GetProperties(enumKeySyntax keySyntax, params string[] propertyDefinitionIds)
        {
            var retVal = new Dictionary<string, Properties.cmisProperty>();
            bool ignoreCase = (keySyntax & enumKeySyntax.searchIgnoreCase) == enumKeySyntax.searchIgnoreCase;
            bool lowerCase = (keySyntax & enumKeySyntax.lowerCase) == enumKeySyntax.lowerCase;
            bool originalCase = (keySyntax & enumKeySyntax.original) == enumKeySyntax.original;

            // collect requested properties
            if (_properties is not null)
            {
                // collect requested propertyDefinitionIds
                var verifyIds = new HashSet<string>();

                if (propertyDefinitionIds is null || propertyDefinitionIds.Length == 0)
                {
                    verifyIds.Add("*");
                }
                else
                {
                    foreach (string propertyDefinitionId in propertyDefinitionIds)
                    {
                        string prop = propertyDefinitionId;

                        if (propertyDefinitionId is null)
                            prop = "";
                        if (ignoreCase)
                            prop = propertyDefinitionId.ToLowerInvariant();
                        verifyIds.Add(prop);
                    }
                }

                // collect requested properties
                foreach (Properties.cmisProperty property in _properties)
                {
                    string originalName = property.PropertyDefinitionId;
                    string name;

                    if (originalName is null)
                    {
                        originalName = "";
                        name = "";
                    }
                    else if (ignoreCase)
                    {
                        name = originalName.ToLowerInvariant();
                    }
                    else
                    {
                        name = originalName;
                    }
                    if (verifyIds.Contains(name) || verifyIds.Contains("*"))
                    {
                        if (lowerCase && !retVal.ContainsKey(name))
                            retVal.Add(name, property);
                        if (originalCase && !retVal.ContainsKey(originalName))
                            retVal.Add(originalName, property);
                    }
                }
            }

            return retVal;
        }

        private ccg.ArrayMapper<cmisPropertiesType, Properties.cmisProperty> initial_propertiesAsReadOnly() => new ccg.ArrayMapper<cmisPropertiesType, Properties.cmisProperty>(this, "Properties", () => _properties.ToArray(), "PropertyDefinitionId", property => property.PropertyDefinitionId);

        private ccg.ArrayMapper<cmisPropertiesType, Properties.cmisProperty> _propertiesAsReadOnly;
        /// <summary>
      /// Access to properties via index or PropertyDefinitionId
      /// </summary>
      /// <value></value>
      /// <returns></returns>
      /// <remarks></remarks>
        public ccg.ArrayMapper<cmisPropertiesType, Properties.cmisProperty> PropertiesAsReadOnly
        {
            get
            {
                return _propertiesAsReadOnly;
            }
        }

        /// <summary>
      /// Remove the specified property
      /// </summary>
      /// <param name="propertyDefinitionId"></param>
      /// <param name="ignoreCase"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public bool RemoveProperty(string propertyDefinitionId, bool ignoreCase = true)
        {
            bool retVal = false;

            if (_properties is not null)
            {
                var properties = new List<Properties.cmisProperty>(_properties);

                for (int index = properties.Count - 1; index >= 0; index -= 1)
                {
                    if (properties[index] is null || string.Compare(properties[index].PropertyDefinitionId, propertyDefinitionId, ignoreCase) == 0)
                    {
                        properties.RemoveAt(index);
                        retVal = true;
                    }
                }

                if (retVal)
                    Properties = properties.Count > 0 ? properties.ToArray() : null;
            }

            return retVal;
        }

    }
}