using System;
using System.Collections.Generic;
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

namespace CmisObjectModel.Core.Definitions
{
    /// <summary>
   /// Baseclass for all Definition-Classes
   /// (see namespaces CmisObjectModel.Core.Definitions.*)
   /// </summary>
   /// <remarks>This type is not directly defined in the cmis-documentation!</remarks>
    public abstract class DefinitionBase : Serialization.XmlSerializable
    {

        #region Constructors
        static DefinitionBase()
        {
            // search for all types supporting cmis typedefinition ...
            if (!ExploreAssembly(typeof(DefinitionBase).Assembly))
            {
                // ... failed.
                // At least register well-known TypeDefinition-classes ...
                cac.ExploreTypes(_factories, _genericTypeDefinition, typeof(Types.cmisTypeDocumentDefinitionType), typeof(Types.cmisTypeFolderDefinitionType), typeof(Types.cmisTypeItemDefinitionType), typeof(Types.cmisTypePolicyDefinitionType), typeof(Types.cmisTypeRelationshipDefinitionType), typeof(Types.cmisTypeRM_ClientMgtRetentionDefinitionType), typeof(Types.cmisTypeRM_DestructionRetentionDefinitionType), typeof(Types.cmisTypeRM_HoldDefinitionType), typeof(Types.cmisTypeRM_RepMgtRetentionDefinitionType), typeof(Types.cmisTypeSecondaryDefinitionType));
                // ... and PropertyDetinition-classes
                cac.ExploreTypes(_factories, _genericTypeDefinition, typeof(Properties.cmisPropertyBooleanDefinitionType), typeof(Properties.cmisPropertyDateTimeDefinitionType), typeof(Properties.cmisPropertyDecimalDefinitionType), typeof(Properties.cmisPropertyHtmlDefinitionType), typeof(Properties.cmisPropertyIdDefinitionType), typeof(Properties.cmisPropertyIntegerDefinitionType), typeof(Properties.cmisPropertyStringDefinitionType), typeof(Properties.cmisPropertyUriDefinitionType));
            }

            CommonFunctions.DecimalRepresentationChanged += OnDecimalRepresentationChanged;
            if (CommonFunctions.DecimalRepresentation != enumDecimalRepresentation.@decimal)
            {
                OnDecimalRepresentationChanged(CommonFunctions.DecimalRepresentation);
            }
        }
        protected DefinitionBase()
        {
            InitClass();
        }
        /// <summary>
      /// this constructor is only used if derived classes from this class needs an InitClass()-call
      /// </summary>
      /// <param name="initClassSupported"></param>
      /// <remarks></remarks>
        protected DefinitionBase(bool? initClassSupported) : base(initClassSupported)
        {
        }
        protected DefinitionBase(string id, string localName, string displayName, string queryName)
        {
            InitClass();
            _id = id;
            _localName = localName;
            _queryName = queryName;
            _displayName = displayName;
        }
        protected DefinitionBase(string id, string localName, string localNamespace, string displayName, string queryName, bool queryable)
        {
            InitClass();
            _id = id;
            _localName = localName;
            _localNamespace = localNamespace;
            _displayName = displayName;
            _queryName = queryName;
            _queryable = queryable;
        }
        protected DefinitionBase(string id, string localName, string localNamespace, string displayName, string queryName, string description, bool queryable)
        {
            InitClass();
            _id = id;
            _localName = localName;
            _localNamespace = localNamespace;
            _displayName = displayName;
            _queryName = queryName;
            _description = description;
            _queryable = queryable;
        }
        /// <summary>
      /// Creates a BaseDefinition-instance from the current node of the reader-object using the
      /// elementName-Node to determine, which implementation is to be used
      /// </summary>
      /// <typeparam name="TResult"></typeparam>
      /// <param name="reader"></param>
      /// <param name="elementName"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        protected static TResult CreateInstance<TResult>(System.Xml.XmlReader reader, string elementName) where TResult : DefinitionBase
        {
            string outerXml;

            try
            {
                // read the complete node to determine the required PropertyDefinition-type to allow random access
                reader.MoveToContent();
                outerXml = reader.ReadOuterXml();

                if (!string.IsNullOrEmpty(outerXml))
                {
                    var xmlDoc = new System.Xml.XmlDocument();

                    xmlDoc.LoadXml(outerXml);
                    foreach (System.Xml.XmlNode node in xmlDoc.DocumentElement.ChildNodes)
                    {
                        if (string.Compare(elementName, node.LocalName, true) == 0)
                        {
                            var retVal = CreateInstance<TResult>(node.InnerText);

                            if (retVal is not null)
                            {
                                using (var ms = new System.IO.MemoryStream())
                                {
                                    xmlDoc.Save(ms);
                                    ms.Position = 0L;
                                    reader = System.Xml.XmlReader.Create(ms);
                                    retVal.ReadXml(reader);
                                }

                                return retVal;
                            }
                        }
                    }
                }
            }
            catch
            {
            }

            // current node doesn't describe a TResult-instance
            return null;
        }
        protected static TResult CreateInstance<TResult>(string key) where TResult : DefinitionBase
        {
            key = key is null ? "" : key.ToLowerInvariant();
            if (_factories.ContainsKey(key))
            {
                return _factories[key].CreateInstance() as TResult;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
      /// Searches in assembly for types supporting cmis typedefinition
      /// </summary>
      /// <param name="assembly"></param>
      /// <remarks></remarks>
        public static bool ExploreAssembly(System.Reflection.Assembly assembly)
        {
            try
            {
                // explore the complete assembly if possible
                if (assembly is not null)
                {
                    cac.ExploreTypes(_factories, _genericTypeDefinition, assembly.GetTypes());
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        protected static Dictionary<string, Common.Generic.Factory<DefinitionBase>> _factories = new Dictionary<string, Common.Generic.Factory<DefinitionBase>>();
        /// <summary>
      /// GetType(Generic.Factory(Of DefinitionBase, TDerivedFromDefinitionBase))
      /// </summary>
      /// <remarks></remarks>
        protected static Type _genericTypeDefinition = typeof(Common.Generic.Factory<DefinitionBase, Properties.cmisPropertyBooleanDefinitionType>).GetGenericTypeDefinition();
        #endregion

        #region base property-set of definition-types
        protected string _description;
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                if (_description != value)
                {
                    string oldValue = _description;
                    _description = value;
                    OnPropertyChanged("Description", value, oldValue);
                }
            }
        }

        protected string _displayName;
        public string DisplayName
        {
            get
            {
                return _displayName;
            }
            set
            {
                if ((_displayName ?? "") != (value ?? ""))
                {
                    string oldValue = _displayName;
                    _displayName = value;
                    OnPropertyChanged("DisplayName", value, oldValue);
                }
            }
        }

        protected string _id;
        public string Id
        {
            get
            {
                return _id;
            }
            set
            {
                if ((_id ?? "") != (value ?? ""))
                {
                    string oldValue = _id;
                    _id = value;
                    OnPropertyChanged("Id", value, oldValue);
                }
            }
        }

        protected string _localName;
        public string LocalName
        {
            get
            {
                return _localName;
            }
            set
            {
                if ((_localName ?? "") != (value ?? ""))
                {
                    string oldValue = _localName;
                    _localName = value;
                    OnPropertyChanged("LocalName", value, oldValue);
                }
            }
        }

        protected string _localNamespace;
        public string LocalNamespace
        {
            get
            {
                return _localNamespace;
            }
            set
            {
                if ((_localNamespace ?? "") != (value ?? ""))
                {
                    string oldValue = _localNamespace;
                    _localNamespace = value;
                    OnPropertyChanged("LocalNamespace", value, oldValue);
                }
            }
        }

        protected bool _queryable;
        public bool Queryable
        {
            get
            {
                return _queryable;
            }
            set
            {
                if (_queryable != value)
                {
                    bool oldValue = _queryable;
                    _queryable = value;
                    OnPropertyChanged("Queryable", value, oldValue);
                }
            }
        }

        protected string _queryName;
        public string QueryName
        {
            get
            {
                return _queryName;
            }
            set
            {
                if ((_queryName ?? "") != (value ?? ""))
                {
                    string oldValue = _queryName;
                    _queryName = value.ValidateQueryName();
                    OnPropertyChanged("QueryName", _queryName, oldValue);
                }
            }
        }
        #endregion

        private static void OnDecimalRepresentationChanged(enumDecimalRepresentation newValue)
        {
            var attrs = typeof(Properties.cmisPropertyDecimalDefinitionType).GetCustomAttributes(typeof(cac), false);
            var attr = attrs is not null && attrs.Length > 0 ? (cac)attrs[0] : null;

            switch (newValue)
            {
                case enumDecimalRepresentation.@decimal:
                    {
                        cac.AppendTypeFactory(_factories, _genericTypeDefinition, typeof(Properties.cmisPropertyDecimalDefinitionType), attr);
                        break;
                    }
                case enumDecimalRepresentation.@double:
                    {
                        cac.AppendTypeFactory(_factories, _genericTypeDefinition, typeof(Properties.cmisPropertyDoubleDefinitionType), attr);
                        break;
                    }
            }
        }

    }
}