using System;
using System.Collections.Generic;
using System.Xml.Linq;
using ca = CmisObjectModel.AtomPub;
using ccg1 = CmisObjectModel.Collections.Generic;
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
using ccdp = CmisObjectModel.Core.Definitions.Properties;

namespace CmisObjectModel.Core.Definitions.Types
{
    [Attributes.JavaScriptObjectResolver(typeof(JSON.Serialization.CmisTypeDefinitionResolver))]
    [Attributes.JavaScriptConverter(typeof(JSON.Core.Definitions.Types.Generic.cmisTypeDefinitionTypeConverter<cmisTypeDefinitionType>), "{\"\":\"TTypeDefinition\"}")]
    public partial class cmisTypeDefinitionType
    {

        #region Constructors
        protected cmisTypeDefinitionType(string id, string localName, string displayName, string queryName, params ccdp.cmisPropertyDefinitionType[] propertyDefinitions) : base(id, localName, displayName, queryName)
        {
            _propertyDefinitions = propertyDefinitions;
        }
        protected cmisTypeDefinitionType(string id, string localName, string displayName, string queryName, string parentId, params ccdp.cmisPropertyDefinitionType[] propertyDefinitions) : base(id, localName, displayName, queryName)
        {
            _propertyDefinitions = propertyDefinitions;
            _parentId = parentId;
        }

        /// <summary>
      /// Creates a new instance suitable for the current node of the reader
      /// </summary>
      /// <param name="reader"></param>
      /// <returns></returns>
      /// <remarks>Node "baseId" is responsible for type of returned TypeDefinition-instance</remarks>
        public static cmisTypeDefinitionType CreateInstance(System.Xml.XmlReader reader)
        {
            reader.MoveToContent();
            // support for xsi:type-attribute
            if (reader.MoveToAttribute("type", Namespaces.w3instance))
            {
                var retVal = CreateInstance<cmisTypeDefinitionType>(reader.GetAttribute("type", Namespaces.w3instance));

                reader.MoveToContent();
                if (retVal is not null)
                {
                    retVal.ReadXml(reader);
                    return retVal;
                }
            }
            return CreateInstance<cmisTypeDefinitionType>(reader, "baseId");
        }
        #endregion

        public virtual enumBaseObjectTypeIds BaseId
        {
            get
            {
                return _baseId;
            }
        }
        protected abstract enumBaseObjectTypeIds _baseId { get; }

        private ccg1.ArrayMapper<cmisTypeDefinitionType, ccdp.cmisPropertyDefinitionType> initial_propertyDefinitionsAsReadOnly() => new ccg1.ArrayMapper<cmisTypeDefinitionType, ccdp.cmisPropertyDefinitionType>(this, "PropertyDefinitions", () => _propertyDefinitions, "Id", propertyDefinition => propertyDefinition.Id);

        protected ccg1.ArrayMapper<cmisTypeDefinitionType, ccdp.cmisPropertyDefinitionType> _propertyDefinitionsAsReadOnly;
        protected abstract override void InitClass();
        protected void MyBaseInitClass()
        {
            base.InitClass();
        }

        /// <summary>
      /// Access to PropertyDefinitions via index or Id
      /// </summary>
      /// <value></value>
      /// <returns></returns>
      /// <remarks></remarks>
        public ccg1.ArrayMapper<cmisTypeDefinitionType, ccdp.cmisPropertyDefinitionType> PropertyDefinitionsAsReadOnly
        {
            get
            {
                return _propertyDefinitionsAsReadOnly;
            }
        }

        protected abstract string GetCmisTypeName();

        /// <summary>
      /// Returns the properties specified by the given propertyDefinitionIds
      /// </summary>
      /// <param name="propertyDefinitionIds"></param>
      /// <returns></returns>
      /// <remarks>The given propertyDefinitionIds handled casesensitive, if there is
      /// none at all, all properties of this instance will be returned</remarks>
        public Dictionary<string, ccdp.cmisPropertyDefinitionType> GetPropertyDefinitions(params string[] propertyDefinitionIds)
        {
            return GetPropertyDefinitions(enumKeySyntax.original, propertyDefinitionIds);
        }

        /// <summary>
      /// Returns the properties specified by the given propertyDefinitionIds
      /// </summary>
      /// <param name="ignoreCase">If True each propertyDefinitionId is compared case insensitive</param>
      /// <param name="propertyDefinitionIds"></param>
      /// <returns>Dictionary of all existing propertyDefinitions specified by propertyDefinitionsIds.
      /// Notice: if ignoreCase is set to True, then the keys of the returned dictionary are lowercase</returns>
      /// <remarks>If there are no propertyDefinitionIds defined, all properties of this instance will be returned</remarks>
        public Dictionary<string, ccdp.cmisPropertyDefinitionType> GetPropertyDefinitions(bool ignoreCase, params string[] propertyDefinitionIds)
        {
            return GetPropertyDefinitions(ignoreCase ? enumKeySyntax.lowerCase : enumKeySyntax.original, propertyDefinitionIds);
        }

        /// <summary>
      /// Returns the properties specified by the given propertyDefinitionIds
      /// </summary>
      /// <param name="keySyntax">If the lowerCase-bit is set each propertyDefinitionId is compared case insensitive</param>
      /// <param name="propertyDefinitionIds"></param>
      /// <returns>Dictionary of all existing propertyDefinitions specified by propertyDefinitionsIds.
      /// Notice: if keySyntax is set to lowerCase, then the keys of the returned dictionary are lowercase</returns>
      /// <remarks>If there are no propertyDefinitionIds defined, all properties of this instance will be returned</remarks>
        public Dictionary<string, ccdp.cmisPropertyDefinitionType> GetPropertyDefinitions(enumKeySyntax keySyntax, params string[] propertyDefinitionIds)
        {
            var retVal = new Dictionary<string, ccdp.cmisPropertyDefinitionType>();
            var verifyIds = new HashSet<string>();
            bool ignoreCase = (keySyntax & enumKeySyntax.searchIgnoreCase) == enumKeySyntax.searchIgnoreCase;
            bool lowerCase = (keySyntax & enumKeySyntax.lowerCase) == enumKeySyntax.lowerCase;
            bool originalCase = (keySyntax & enumKeySyntax.original) == enumKeySyntax.original;

            // collect requested propertyDefinitionIds
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
            if (_propertyDefinitions is not null)
            {
                foreach (ccdp.cmisPropertyDefinitionType pd in _propertyDefinitions)
                {
                    string originalName = pd.Id;
                    string name;

                    if (originalName is null)
                    {
                        name = "";
                        originalName = "";
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
                            retVal.Add(name, pd);
                        if (originalCase && !retVal.ContainsKey(originalName))
                            retVal.Add(originalName, pd);
                    }
                }
            }

            return retVal;
        }

        #region IXmlSerialization
        public override void ReadXml(System.Xml.XmlReader reader)
        {
            base.ReadXml(reader);
            if (_propertyDefinitions is not null)
            {
                // support properties extension (alfresco)
                foreach (ccdp.cmisPropertyDefinitionType propertyDefinition in _propertyDefinitions)
                    propertyDefinition.get_ExtendedProperties().Add(ExtendedProperties.DeclaringType, _id);
            }
        }

        public override void WriteXml(System.Xml.XmlWriter writer)
        {
            string typeName = GetCmisTypeName();
            if (!string.IsNullOrEmpty(typeName))
                WriteAttribute(writer, null, "type", Namespaces.w3instance, typeName);
            base.WriteXml(writer);
        }
        #endregion

        #region Links of type definition
        /// <summary>
      /// Creates a list of links for a typedefinition-instance
      /// </summary>
      /// <typeparam name="TLink"></typeparam>
      /// <param name="baseUri"></param>
      /// <param name="repositoryId"></param>
      /// <param name="elementFactory"></param>
      /// <returns></returns>
      /// <remarks>
      /// see http://docs.oasis-open.org/cmis/CMIS/v1.1/cs01/CMIS-v1.1-cs01.html
      /// 3.11.1.1 HTTP GET
      /// </remarks>
        protected virtual List<TLink> GetLinks<TLink>(Uri baseUri, string repositoryId, ca.Factory.GenericDelegates<Uri, TLink>.CreateLinkDelegate elementFactory)
        {
            var retVal = new List<TLink>() { elementFactory(new Uri(baseUri, ServiceURIs.get_TypeUri(ServiceURIs.enumTypeUri.typeId).ReplaceUri("repositoryId", repositoryId, "id", _id)), LinkRelationshipTypes.Self, MediaTypes.Entry, _id, null), elementFactory(new Uri(baseUri, ServiceURIs.GetRepositoryInfo.ReplaceUri("repositoryId", repositoryId)), LinkRelationshipTypes.Service, MediaTypes.Service, null, null), elementFactory(new Uri(baseUri, ServiceURIs.get_TypeUri(ServiceURIs.enumTypeUri.typeId).ReplaceUri("repositoryId", repositoryId, "id", _baseId.GetName())), LinkRelationshipTypes.DescribedBy, MediaTypes.Entry, _baseId.GetName(), null), elementFactory(new Uri(baseUri, ServiceURIs.get_TypesUri(ServiceURIs.enumTypesUri.typeId).ReplaceUri("repositoryId", repositoryId, "id", _id)), LinkRelationshipTypes.Down, MediaTypes.Feed, null, null), elementFactory(new Uri(baseUri, ServiceURIs.get_TypeDescendantsUri(ServiceURIs.enumTypeDescendantsUri.typeId).ReplaceUri("repositoryId", repositoryId, "id", _id)), LinkRelationshipTypes.Down, MediaTypes.Tree, null, null) };

            if (!string.IsNullOrEmpty(_parentId))
            {
                retVal.Add(elementFactory(new Uri(baseUri, ServiceURIs.get_TypeUri(ServiceURIs.enumTypeUri.typeId).ReplaceUri("repositoryId", repositoryId, "id", _parentId)), LinkRelationshipTypes.Up, MediaTypes.Entry, _parentId, null));
            }

            return retVal;
        }
        public List<ca.AtomLink> GetLinks(Uri baseUri, string repositoryId)
        {
            return GetLinks(baseUri, repositoryId, (uri, relationshipType, mediaType, id, renditionKind) => new ca.AtomLink(uri, relationshipType, mediaType, id, renditionKind));
        }
        public List<XElement> GetLinks(Uri baseUri, string repositoryId, XNamespace ns, string elementName)
        {
            {
                var withBlock = new ca.Factory.XElementBuilder(ns, elementName);
                return GetLinks(baseUri, repositoryId, withBlock.CreateXElement);
            }
        }
        #endregion

        public static Client.CmisType.PreStage operator +(cmisTypeDefinitionType arg1, Contracts.ICmisClient arg2)
        {
            return new Client.CmisType.PreStage(arg2, arg1);
        }
        public static Client.CmisType.PreStage operator +(Contracts.ICmisClient arg1, cmisTypeDefinitionType arg2)
        {
            return new Client.CmisType.PreStage(arg1, arg2);
        }

        public static explicit operator cmisTypeDefinitionType(ca.AtomEntry value)
        {
            return value is null ? null : value.Type;
        }

    }
}