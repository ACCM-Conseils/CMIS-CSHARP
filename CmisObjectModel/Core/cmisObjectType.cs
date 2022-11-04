using System;
using System.Collections.Generic;
using System.Data;
// depends on the chosen interpretation of the xs:integer expression in a xsd-file
/* TODO ERROR: Skipped IfDirectiveTrivia
#If xs_Integer = "Int32" OrElse xs_integer = "Integer" OrElse xs_integer = "Single" Then
*//* TODO ERROR: Skipped DisabledTextTrivia
Imports xs_Integer = System.Int32
*//* TODO ERROR: Skipped ElseDirectiveTrivia
#Else
*/
using xs_Integer = System.Int64;
using System.Linq;
using System.Xml.Linq;
using ca = CmisObjectModel.AtomPub;
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
using ccg = CmisObjectModel.Common.Generic;
using CmisObjectModel.Constants;
/* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.Core
{
    public partial class cmisObjectType
    {

        private PredefinedPropertyDefinitionFactory _predefinedPropertyDefinitionFactory = new PredefinedPropertyDefinitionFactory(null);
        public cmisObjectType(params Properties.cmisProperty[] properties)
        {
            if (properties is not null && properties.Length > 0)
            {
                _properties = new Collections.cmisPropertiesType(properties);
            }
            else
            {
                _properties = new Collections.cmisPropertiesType();
            }
            InitClass();
        }

        public cmisObjectType(Collections.cmisPropertiesType properties)
        {
            if (properties is not null)
                _properties = new Collections.cmisPropertiesType() { Extensions = properties.Extensions, Properties = properties.Properties };
            InitClass();
        }

        protected override void InitClass()
        {
            base.InitClass();
        }

        #region Helper-classes
        /// <summary>
      /// Compares two cmisObjectType-instances by comparisons of the selected properties
      /// </summary>
      /// <remarks></remarks>
        public class cmisObjectTypeComparer : IComparable<cmisObjectTypeComparer>
        {

            public cmisObjectTypeComparer(cmisObjectType cmisObject, string[] propertyNames)
            {
                _cmisObject = cmisObject;
                _propertyNames = propertyNames;
            }

            #region IComparable
            public int CompareTo(cmisObjectTypeComparer other)
            {
                var properties = _cmisObject is null ? null : _cmisObject.PropertiesAsReadOnly;
                var otherProperties = other is null || other._cmisObject is null ? null : other._cmisObject.PropertiesAsReadOnly;

                if (otherProperties is null)
                {
                    return properties is null ? 0 : 1;
                }
                else if (properties is null)
                {
                    return -1;
                }
                else
                {
                    int length = _propertyNames is null ? 0 : _propertyNames.Length;
                    int otherLength = other._propertyNames is null ? 0 : other._propertyNames.Length;

                    if (otherLength == 0)
                    {
                        return length == 0 ? 0 : 1;
                    }
                    else if (length == 0)
                    {
                        return -1;
                    }
                    else
                    {
                        for (int index = 0, loopTo = Math.Min(length, otherLength) - 1; index <= loopTo; index++)
                        {
                            int result = Core.Properties.cmisProperty.Compare(properties[_propertyNames[index]], otherProperties[other._propertyNames[index]]);
                            if (result != 0)
                                return result;
                        }
                        return length == otherLength ? 0 : length < otherLength ? -1 : 1;
                    }
                }
            }
            #endregion

            private cmisObjectType _cmisObject;
            private string[] _propertyNames;

        }
        #endregion

        #region predefined properties
        private T GetPropertyValue<T>(string id)
        {
            var p = _properties.GetByPropertyDefinitionId(id);
            if (p is null || p.Value is null)
                return default;
            return (T)p.Value;
        }
        private T[] GetPropertyValues<T>(string id)
        {
            var p = _properties.GetByPropertyDefinitionId(id);
            if (p is null || p.Values is null)
                return null;
            return (from v in p.Values
                    select (T)v).ToArray();
        }
        private void SetPropertyValue<T>(string id, string name, T value, Func<T, Properties.cmisProperty> createPropertyFunc)
        {
            var p = _properties.GetByPropertyDefinitionId(id);
            if (p is null)
            {
                _properties.AddProperty(createPropertyFunc(value));
            }
            else
            {
                var oldValue = p.Value;
                p.Value = value;
                OnPropertyChanged(name, value, oldValue);
            }
        }

        private void SetPropertyValues(string id, string name, object[] value, Func<object[], Properties.cmisProperty> createPropertyFunc)
        {
            var p = _properties.GetByPropertyDefinitionId(id);
            if (p is null)
            {
                _properties.AddProperty(createPropertyFunc(value));
            }
            else
            {
                var oldValue = p.Value;
                p.Values = value;
                OnPropertyChanged(name, value, oldValue);
            }
        }

        public ccg.Nullable<string[]> AllowedChildObjectTypeIds
        {
            get
            {
                return GetPropertyValues<string>(CmisPredefinedPropertyNames.AllowedChildObjectTypeIds);
            }
            set
            {
                SetPropertyValues(CmisPredefinedPropertyNames.AllowedChildObjectTypeIds, "AllowedChildObjectTypeIds", value, v => _predefinedPropertyDefinitionFactory.AllowedChildObjectTypeIds().CreateProperty(v));
            }
        } // AllowedChildObjectTypeIds

        public ccg.Nullable<string> BaseTypeId
        {
            get
            {
                return GetPropertyValue<string>(CmisPredefinedPropertyNames.BaseTypeId);
            }
            set
            {
                SetPropertyValue<string>(CmisPredefinedPropertyNames.BaseTypeId, "BaseTypeId", value, v => _predefinedPropertyDefinitionFactory.BaseTypeId().CreateProperty(v));
            }
        } // BaseTypeId

        public ccg.Nullable<string> ChangeToken
        {
            get
            {
                return GetPropertyValue<string>(CmisPredefinedPropertyNames.ChangeToken);
            }
            set
            {
                SetPropertyValue<string>(CmisPredefinedPropertyNames.ChangeToken, "ChangeToken", value, v => _predefinedPropertyDefinitionFactory.ChangeToken().CreateProperty(v));
            }
        } // ChangeToken

        public ccg.Nullable<string> CheckinComment
        {
            get
            {
                return GetPropertyValue<string>(CmisPredefinedPropertyNames.CheckinComment);
            }
            set
            {
                SetPropertyValue<string>(CmisPredefinedPropertyNames.CheckinComment, "CheckinComment", value, v => _predefinedPropertyDefinitionFactory.CheckinComment().CreateProperty(v));
            }
        } // CheckinComment

        public ccg.Nullable<string> ContentStreamFileName
        {
            get
            {
                return GetPropertyValue<string>(CmisPredefinedPropertyNames.ContentStreamFileName);
            }
            set
            {
                SetPropertyValue<string>(CmisPredefinedPropertyNames.ContentStreamFileName, "ContentStreamFileName", value, v => _predefinedPropertyDefinitionFactory.ContentStreamFileName().CreateProperty(v));
            }
        } // ContentStreamFileName

        public ccg.Nullable<string> ContentStreamId
        {
            get
            {
                return GetPropertyValue<string>(CmisPredefinedPropertyNames.ContentStreamId);
            }
            set
            {
                SetPropertyValue<string>(CmisPredefinedPropertyNames.ContentStreamId, "ContentStreamId", value, v => _predefinedPropertyDefinitionFactory.ContentStreamId().CreateProperty(v));
            }
        } // ContentStreamId

        public long? ContentStreamLength
        {
            get
            {
                return GetPropertyValue<xs_Integer?>(CmisPredefinedPropertyNames.ContentStreamLength);
            }
            set
            {
                SetPropertyValue(CmisPredefinedPropertyNames.ContentStreamLength, "ContentStreamLength", value, v => _predefinedPropertyDefinitionFactory.ContentStreamLength().CreateProperty(v));
            }
        } // ContentStreamLength

        public ccg.Nullable<string> ContentStreamMimeType
        {
            get
            {
                return GetPropertyValue<string>(CmisPredefinedPropertyNames.ContentStreamMimeType);
            }
            set
            {
                SetPropertyValue<string>(CmisPredefinedPropertyNames.ContentStreamMimeType, "ContentStreamMimeType", value, v => _predefinedPropertyDefinitionFactory.ContentStreamMimeType().CreateProperty(v));
            }
        } // ContentStreamMimeType

        public ccg.Nullable<string> CreatedBy
        {
            get
            {
                return GetPropertyValue<string>(CmisPredefinedPropertyNames.CreatedBy);
            }
            set
            {
                SetPropertyValue<string>(CmisPredefinedPropertyNames.CreatedBy, "CreatedBy", value, v => _predefinedPropertyDefinitionFactory.CreatedBy().CreateProperty(v));
            }
        } // CreatedBy

        public DateTimeOffset? CreationDate
        {
            get
            {
                return GetPropertyValue<DateTimeOffset?>(CmisPredefinedPropertyNames.CreationDate);
            }
            set
            {
                SetPropertyValue(CmisPredefinedPropertyNames.CreationDate, "CreationDate", value, v => _predefinedPropertyDefinitionFactory.CreationDate().CreateProperty(v));
            }
        } // CreationDate

        public ccg.Nullable<string> Description
        {
            get
            {
                return GetPropertyValue<string>(CmisPredefinedPropertyNames.Description);
            }
            set
            {
                SetPropertyValue<string>(CmisPredefinedPropertyNames.Description, "Description", value, v => _predefinedPropertyDefinitionFactory.Description().CreateProperty(v));
            }
        } // Description

        public bool? IsImmutable
        {
            get
            {
                return GetPropertyValue<bool?>(CmisPredefinedPropertyNames.IsImmutable);
            }
            set
            {
                SetPropertyValue(CmisPredefinedPropertyNames.IsImmutable, "IsImmutable", value, v => _predefinedPropertyDefinitionFactory.IsImmutable().CreateProperty(v));
            }
        } // IsImmutable

        public bool? IsLatestMajorVersion
        {
            get
            {
                return GetPropertyValue<bool?>(CmisPredefinedPropertyNames.IsLatestMajorVersion);
            }
            set
            {
                SetPropertyValue(CmisPredefinedPropertyNames.IsLatestMajorVersion, "IsLatestMajorVersion", value, v => _predefinedPropertyDefinitionFactory.IsLatestMajorVersion().CreateProperty(v));
            }
        } // IsLatestMajorVersion

        public bool? IsLatestVersion
        {
            get
            {
                return GetPropertyValue<bool?>(CmisPredefinedPropertyNames.IsLatestVersion);
            }
            set
            {
                SetPropertyValue(CmisPredefinedPropertyNames.IsLatestVersion, "IsLatestVersion", value, v => _predefinedPropertyDefinitionFactory.IsLatestVersion().CreateProperty(v));
            }
        } // IsLatestVersion

        public bool? IsMajorVersion
        {
            get
            {
                return GetPropertyValue<bool?>(CmisPredefinedPropertyNames.IsMajorVersion);
            }
            set
            {
                SetPropertyValue(CmisPredefinedPropertyNames.IsMajorVersion, "IsMajorVersion", value, v => _predefinedPropertyDefinitionFactory.IsMajorVersion().CreateProperty(v));
            }
        } // IsMajorVersion

        private static void SetIsMajorVersion(cmisObjectType instance, bool? value)
        {
            instance.IsMajorVersion = value;
        }

        public bool? IsPrivateWorkingCopy
        {
            get
            {
                return GetPropertyValue<bool?>(CmisPredefinedPropertyNames.IsPrivateWorkingCopy);
            }
            set
            {
                SetPropertyValue(CmisPredefinedPropertyNames.IsPrivateWorkingCopy, "IsPrivateWorkingCopy", value, v => _predefinedPropertyDefinitionFactory.IsPrivateWorkingCopy().CreateProperty(v));
            }
        } // IsPrivateWorkingCopy

        public bool? IsVersionSeriesCheckedOut
        {
            get
            {
                return GetPropertyValue<bool?>(CmisPredefinedPropertyNames.IsVersionSeriesCheckedOut);
            }
            set
            {
                SetPropertyValue(CmisPredefinedPropertyNames.IsVersionSeriesCheckedOut, "IsVersionSeriesCheckedOut", value, v => _predefinedPropertyDefinitionFactory.IsVersionSeriesCheckedOut().CreateProperty(v));
            }
        } // IsVersionSeriesCheckedOut

        public DateTimeOffset? LastModificationDate
        {
            get
            {
                return GetPropertyValue<DateTimeOffset?>(CmisPredefinedPropertyNames.LastModificationDate);
            }
            set
            {
                SetPropertyValue(CmisPredefinedPropertyNames.LastModificationDate, "LastModificationDate", value, v => _predefinedPropertyDefinitionFactory.LastModificationDate().CreateProperty(v));
            }
        } // LastModificationDate

        public ccg.Nullable<string> LastModifiedBy
        {
            get
            {
                return GetPropertyValue<string>(CmisPredefinedPropertyNames.LastModifiedBy);
            }
            set
            {
                SetPropertyValue<string>(CmisPredefinedPropertyNames.LastModifiedBy, "LastModifiedBy", value, v => _predefinedPropertyDefinitionFactory.LastModifiedBy().CreateProperty(v));
            }
        } // LastModifiedBy

        public ccg.Nullable<string> Name
        {
            get
            {
                return GetPropertyValue<string>(CmisPredefinedPropertyNames.Name);
            }
            set
            {
                SetPropertyValue<string>(CmisPredefinedPropertyNames.Name, "Name", value, v => _predefinedPropertyDefinitionFactory.Name().CreateProperty(v));
            }
        } // Name

        public ccg.Nullable<string> ObjectId
        {
            get
            {
                return GetPropertyValue<string>(CmisPredefinedPropertyNames.ObjectId);
            }
            set
            {
                SetPropertyValue<string>(CmisPredefinedPropertyNames.ObjectId, "ObjectId", value, v => _predefinedPropertyDefinitionFactory.ObjectId().CreateProperty(v));
            }
        } // ObjectId

        public ccg.Nullable<string> ObjectTypeId
        {
            get
            {
                return GetPropertyValue<string>(CmisPredefinedPropertyNames.ObjectTypeId);
            }
            set
            {
                SetPropertyValue<string>(CmisPredefinedPropertyNames.ObjectTypeId, "ObjectTypeId", value, v => _predefinedPropertyDefinitionFactory.ObjectTypeId().CreateProperty(v));
            }
        } // ObjectTypeId

        public ccg.Nullable<string> ParentId
        {
            get
            {
                return GetPropertyValue<string>(CmisPredefinedPropertyNames.ParentId);
            }
            set
            {
                SetPropertyValue<string>(CmisPredefinedPropertyNames.ParentId, "ParentId", value, v => _predefinedPropertyDefinitionFactory.ParentId().CreateProperty(v));
            }
        } // ParentId


        public ccg.Nullable<string> Path
        {
            get
            {
                return GetPropertyValue<string>(CmisPredefinedPropertyNames.Path);
            }
            set
            {
                SetPropertyValue<string>(CmisPredefinedPropertyNames.Path, "Path", value, v => _predefinedPropertyDefinitionFactory.Path().CreateProperty(v));
            }
        } // Path

        public ccg.Nullable<string> PolicyText
        {
            get
            {
                return GetPropertyValue<string>(CmisPredefinedPropertyNames.PolicyText);
            }
            set
            {
                SetPropertyValue<string>(CmisPredefinedPropertyNames.PolicyText, "PolicyText", value, v => _predefinedPropertyDefinitionFactory.PolicyText().CreateProperty(v));
            }
        } // PolicyText

        public ccg.Nullable<string[]> SecondaryObjectTypeIds
        {
            get
            {
                return GetPropertyValues<string>(CmisPredefinedPropertyNames.SecondaryObjectTypeIds);
            }
            set
            {
                SetPropertyValues(CmisPredefinedPropertyNames.SecondaryObjectTypeIds, "SecondaryObjectTypeIds", value, v => _predefinedPropertyDefinitionFactory.SecondaryObjectTypeIds().CreateProperty(v));
            }
        } // SecondaryObjectTypeIds

        public ccg.Nullable<string> SourceId
        {
            get
            {
                return GetPropertyValue<string>(CmisPredefinedPropertyNames.SourceId);
            }
            set
            {
                SetPropertyValue<string>(CmisPredefinedPropertyNames.SourceId, "SourceId", value, v => _predefinedPropertyDefinitionFactory.SourceId().CreateProperty(v));
            }
        } // SourceId

        public ccg.Nullable<string> TargetId
        {
            get
            {
                return GetPropertyValue<string>(CmisPredefinedPropertyNames.TargetId);
            }
            set
            {
                SetPropertyValue<string>(CmisPredefinedPropertyNames.TargetId, "TargetId", value, v => _predefinedPropertyDefinitionFactory.TargetId().CreateProperty(v));
            }
        } // TargetId

        public ccg.Nullable<string> VersionLabel
        {
            get
            {
                return GetPropertyValue<string>(CmisPredefinedPropertyNames.VersionLabel);
            }
            set
            {
                SetPropertyValue<string>(CmisPredefinedPropertyNames.VersionLabel, "VersionLabel", value, v => _predefinedPropertyDefinitionFactory.VersionLabel().CreateProperty(v));
            }
        } // VersionLabel

        public ccg.Nullable<string> VersionSeriesCheckedOutBy
        {
            get
            {
                return GetPropertyValue<string>(CmisPredefinedPropertyNames.VersionSeriesCheckedOutBy);
            }
            set
            {
                SetPropertyValue<string>(CmisPredefinedPropertyNames.VersionSeriesCheckedOutBy, "VersionSeriesCheckedOutBy", value, v => _predefinedPropertyDefinitionFactory.VersionSeriesCheckedOutBy().CreateProperty(v));
            }
        } // VersionSeriesCheckedOutBy

        public ccg.Nullable<string> VersionSeriesCheckedOutId
        {
            get
            {
                return GetPropertyValue<string>(CmisPredefinedPropertyNames.VersionSeriesCheckedOutId);
            }
            set
            {
                SetPropertyValue<string>(CmisPredefinedPropertyNames.VersionSeriesCheckedOutId, "VersionSeriesCheckedOutId", value, v => _predefinedPropertyDefinitionFactory.VersionSeriesCheckedOutId().CreateProperty(v));
            }
        } // VersionSeriesCheckedOutId

        public ccg.Nullable<string> VersionSeriesId
        {
            get
            {
                return GetPropertyValue<string>(CmisPredefinedPropertyNames.VersionSeriesId);
            }
            set
            {
                SetPropertyValue<string>(CmisPredefinedPropertyNames.VersionSeriesId, "VersionSeriesId", value, v => _predefinedPropertyDefinitionFactory.VersionSeriesId().CreateProperty(v));
            }
        } // VersionSeriesId

        #endregion

        #region IXmlSerializable
        public override void ReadXml(System.Xml.XmlReader reader)
        {
            base.ReadXml(reader);
            // RefreshObservation()
        }
        #endregion

        #region general links of a cmisObjectType-instance
        /// <summary>
      /// Creates a list of links for a cmisObjectType-instance
      /// </summary>
      /// <typeparam name="TLink"></typeparam>
      /// <param name="baseUri"></param>
      /// <param name="repositoryInfo"></param>
      /// <param name="elementFactory"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        protected List<TLink> GetLinks<TLink>(Uri baseUri, cmisRepositoryInfoType repositoryInfo, ca.Factory.GenericDelegates<Uri, TLink>.CreateLinkDelegate elementFactory)
        {
            string repositoryId = repositoryInfo.RepositoryId;
            string objId = ObjectId;
            string objTypeId = ObjectTypeId;
            var retVal = new List<TLink>() { elementFactory(new Uri(baseUri, ServiceURIs.get_ObjectUri(ServiceURIs.enumObjectUri.self).ReplaceUri("repositoryId", repositoryId, "id", objId)), LinkRelationshipTypes.Self, MediaTypes.Entry, objId, null), elementFactory(new Uri(baseUri, ServiceURIs.GetRepositoryInfo.ReplaceUri("repositoryId", repositoryId)), LinkRelationshipTypes.Service, MediaTypes.Service, null, null), elementFactory(new Uri(baseUri, ServiceURIs.get_TypeUri(ServiceURIs.enumTypeUri.typeId).ReplaceUri("repositoryId", repositoryId, "id", objTypeId)), LinkRelationshipTypes.DescribedBy, MediaTypes.Entry, null, null), elementFactory(new Uri(baseUri, ServiceURIs.GetAllowableActions.ReplaceUri("repositoryId", repositoryId, "id", objId)), LinkRelationshipTypes.AllowableActions, MediaTypes.AllowableActions, null, null) };

            if (repositoryInfo.Capabilities.CapabilityACL != enumCapabilityACL.none)
            {
                retVal.Add(elementFactory(new Uri(baseUri, ServiceURIs.GetAcl.ReplaceUri("repositoryId", repositoryId, "id", objId)), LinkRelationshipTypes.Acl, MediaTypes.Acl, null, null));
            }
            retVal.Add(elementFactory(new Uri(baseUri, ServiceURIs.get_PoliciesUri(ServiceURIs.enumPoliciesUri.objectId).ReplaceUri("repositoryId", repositoryId, "id", objId)), LinkRelationshipTypes.Policies, MediaTypes.Feed, null, null));

            return retVal;
        }

        /// <summary>
      /// Creates a list of links for a cmisObjectType-instance
      /// </summary>
      /// <param name="baseUri"></param>
      /// <param name="repositoryInfo"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public List<ca.AtomLink> GetLinks(Uri baseUri, cmisRepositoryInfoType repositoryInfo)
        {
            return GetLinks(baseUri, repositoryInfo, (uri, relationshipType, mediaType, id, renditionKind) => new ca.AtomLink(uri, relationshipType, mediaType, id, renditionKind));
        }
        /// <summary>
      /// Creates a list of links for a cmisObjectType-instance
      /// </summary>
      /// <param name="baseUri"></param>
      /// <param name="repositoryInfo"></param>
      /// <param name="ns"></param>
      /// <param name="elementName"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public List<XElement> GetLinks(Uri baseUri, cmisRepositoryInfoType repositoryInfo, XNamespace ns, string elementName)
        {
            {
                var withBlock = new ca.Factory.XElementBuilder(ns, elementName);
                return GetLinks(baseUri, repositoryInfo, withBlock.CreateXElement);
            }
        }
        #endregion

        #region links of a cmisdocument
        /// <summary>
      /// Returns links for a cmisdocument
      /// </summary>
      /// <typeparam name="TLink"></typeparam>
      /// <param name="baseUri"></param>
      /// <param name="repositoryInfo"></param>
      /// <param name="elementFactory"></param>
      /// <param name="canGetAllVersions"></param>
      /// <param name="isLatestVersion"></param>
      /// <param name="originId">ObjectId of the document (current version).
      /// The parameter differs from the Id-property if this instance references to the private working copy</param>
      /// <param name="privateWorkingCopyId">ObjectId of the private working copy if existing.
      /// The parameter differs from the Id-property if this instance doesn't reference to the private working copy</param>
      /// <returns></returns>
      /// <remarks>Used Link Relations:
      /// 3.11.2 Document Entry, 3.11.3 PWC Entry; 3.4.3 CMIS Link Relations</remarks>
        protected List<TLink> GetDocumentLinks<TLink>(Uri baseUri, cmisRepositoryInfoType repositoryInfo, ca.Factory.GenericDelegates<Uri, TLink>.CreateLinkDelegate elementFactory, bool canGetAllVersions, bool isLatestVersion, string originId, string privateWorkingCopyId)
        {
            string objId = ObjectId;
            var retVal = GetLinks(baseUri, repositoryInfo, elementFactory);
            bool isWorkingCopy = !(string.IsNullOrEmpty(privateWorkingCopyId) || (objId ?? "") != (privateWorkingCopyId ?? ""));

            retVal.Add(elementFactory(new Uri(baseUri, ServiceURIs.get_ObjectUri(ServiceURIs.enumObjectUri.self).ReplaceUri("repositoryId", repositoryInfo.RepositoryId, "id", objId)), LinkRelationshipTypes.Edit, MediaTypes.Entry, null, null));
            retVal.Add(elementFactory(new Uri(baseUri, ServiceURIs.get_ObjectParentsUri(ServiceURIs.enumObjectParentsUri.objectId).ReplaceUri("repositoryId", repositoryInfo.RepositoryId, "id", objId)), LinkRelationshipTypes.Up, MediaTypes.Feed, null, null));
            if (canGetAllVersions)
            {
                retVal.Add(elementFactory(new Uri(baseUri, ServiceURIs.get_AllVersionsUri(ServiceURIs.enumAllVersionsUri.objectId).ReplaceUri("repositoryId", repositoryInfo.RepositoryId, "id", objId)), LinkRelationshipTypes.VersionHistory, MediaTypes.Feed, null, null));
            }
            if (!(isLatestVersion || isWorkingCopy))
            {
                retVal.Add(elementFactory(new Uri(baseUri, ServiceURIs.get_ObjectUri(ServiceURIs.enumObjectUri.specifyVersion).ReplaceUri("returnVersion", RestAtom.enumReturnVersion.latest.GetName(), "repositoryId", repositoryInfo.RepositoryId, "id", objId)), LinkRelationshipTypes.CurrentVersion, MediaTypes.Entry, null, null));
            }
            if (repositoryInfo.Capabilities.CapabilityContentStreamUpdatability != enumCapabilityContentStreamUpdates.none)
            {
                retVal.Add(elementFactory(new Uri(baseUri, ServiceURIs.get_ContentUri(ServiceURIs.enumContentUri.objectId).ReplaceUri("repositoryId", repositoryInfo.RepositoryId, "id", objId)), LinkRelationshipTypes.EditMedia, MediaTypes.Stream, null, null));
            }
            if (!string.IsNullOrEmpty(privateWorkingCopyId))
            {
                retVal.Add(elementFactory(new Uri(baseUri, ServiceURIs.get_ObjectUri(ServiceURIs.enumObjectUri.workingCopy).ReplaceUri("repositoryId", repositoryInfo.RepositoryId, "id", privateWorkingCopyId, "pwc", "true")), LinkRelationshipTypes.WorkingCopy, MediaTypes.Entry, privateWorkingCopyId, null));
            }
            if (_renditions is not null && _renditions.Count() > 0)
            {
                foreach (cmisRenditionType rendition in _renditions)
                    retVal.Add(elementFactory(new Uri(baseUri, ServiceURIs.get_ContentUri(ServiceURIs.enumContentUri.getContentStream).ReplaceUri("repositoryId", repositoryInfo.RepositoryId, "id", objId, "streamId", rendition.StreamId)), LinkRelationshipTypes.Alternate, rendition.Mimetype, null, rendition.Kind));
            }
            retVal.Add(elementFactory(new Uri(baseUri, ServiceURIs.get_RelationshipsUri(ServiceURIs.enumRelationshipsUri.objectId).ReplaceUri("repositoryId", repositoryInfo.RepositoryId, "id", objId)), LinkRelationshipTypes.Relationships, MediaTypes.Feed, null, null));

            if (isWorkingCopy && !string.IsNullOrEmpty(originId))
            {
                retVal.Add(elementFactory(new Uri(baseUri, ServiceURIs.get_ObjectUri(ServiceURIs.enumObjectUri.self).ReplaceUri("repositoryId", repositoryInfo.RepositoryId, "id", originId)), LinkRelationshipTypes.Via, MediaTypes.Entry, originId, null));
            }

            return retVal;
        }

        /// <summary>
      /// Returns links for a cmisdocument in System.ServiceModel.Syndication.SyndicationLink-format
      /// </summary>
      /// <param name="baseUri"></param>
      /// <param name="repositoryInfo"></param>
      /// <param name="canGetAllVersions"></param>
      /// <param name="isLatestVersion"></param>
      /// <param name="originId">ObjectId of the document (current version).
      /// The parameter differs from the Id-property if this instance references to the private working copy</param>
      /// <param name="privateWorkingCopyId">ObjectId of the private working copy if existing.
      /// The parameter differs from the Id-property if this instance doesn't reference to the private working copy</param>
      /// <returns></returns>
      /// <remarks></remarks>
        public List<ca.AtomLink> GetDocumentLinks(Uri baseUri, cmisRepositoryInfoType repositoryInfo, bool canGetAllVersions, bool isLatestVersion, string originId, string privateWorkingCopyId)
        {
            return GetDocumentLinks(baseUri, repositoryInfo, (uri, relationshipType, mediaType, id, renditionKind) => new ca.AtomLink(uri, relationshipType, mediaType, id, renditionKind), canGetAllVersions, isLatestVersion, originId, privateWorkingCopyId);
        }
        /// <summary>
      /// Returns links for a cmisdocument in System.Xml.Linq.XElement-format
      /// </summary>
      /// <param name="baseUri"></param>
      /// <param name="repositoryInfo"></param>
      /// <param name="ns"></param>
      /// <param name="elementName"></param>
      /// <param name="canGetAllVersions"></param>
      /// <param name="isLatestVersion"></param>
      /// <param name="originId">ObjectId of the document (current version).
      /// The parameter differs from the Id-property if this instance references to the private working copy</param>
      /// <param name="privateWorkingCopyId">ObjectId of the private working copy if existing.
      /// The parameter differs from the Id-property if this instance doesn't reference to the private working copy</param>
      /// <returns></returns>
      /// <remarks></remarks>
        public List<XElement> GetDocumentLinks(Uri baseUri, cmisRepositoryInfoType repositoryInfo, XNamespace ns, string elementName, bool canGetAllVersions, bool isLatestVersion, string originId, string privateWorkingCopyId)
        {
            {
                var withBlock = new ca.Factory.XElementBuilder(ns, elementName);
                return GetDocumentLinks(baseUri, repositoryInfo, withBlock.CreateXElement, canGetAllVersions, isLatestVersion, originId, privateWorkingCopyId);
            }
        }
        #endregion

        #region links for a cmisfolder
        /// <summary>
      /// Returns links for a cmisfolder
      /// </summary>
      /// <typeparam name="TLink"></typeparam>
      /// <param name="baseUri"></param>
      /// <param name="repositoryInfo"></param>
      /// <param name="elementFactory"></param>
      /// <param name="parentId"></param>
      /// <returns></returns>
      /// <remarks>Used Link Relations:
      /// 3.11.4 Folder Entry; 3.4.3 CMIS Link Relations</remarks>
        protected List<TLink> GetFolderLinks<TLink>(Uri baseUri, cmisRepositoryInfoType repositoryInfo, ca.Factory.GenericDelegates<Uri, TLink>.CreateLinkDelegate elementFactory, string parentId)
        {
            string objId = ObjectId;
            var retVal = GetLinks(baseUri, repositoryInfo, elementFactory);

            retVal.Add(elementFactory(new Uri(baseUri, ServiceURIs.get_ObjectUri(ServiceURIs.enumObjectUri.self).ReplaceUri("repositoryId", repositoryInfo.RepositoryId, "id", objId)), LinkRelationshipTypes.Edit, MediaTypes.Entry, null, null));
            retVal.Add(elementFactory(new Uri(baseUri, ServiceURIs.get_ChildrenUri(ServiceURIs.enumChildrenUri.folderId).ReplaceUri("repositoryId", repositoryInfo.RepositoryId, "id", objId)), LinkRelationshipTypes.Down, MediaTypes.Feed, null, null));
            if (repositoryInfo.Capabilities.CapabilityGetDescendants)
            {
                retVal.Add(elementFactory(new Uri(baseUri, ServiceURIs.get_DescendantsUri(ServiceURIs.enumDescendantsUri.folderId).ReplaceUri("repositoryId", repositoryInfo.RepositoryId, "id", objId)), LinkRelationshipTypes.Down, MediaTypes.Tree, null, null));
            }
            if (!string.IsNullOrEmpty(parentId))
            {
                retVal.Add(elementFactory(new Uri(baseUri, ServiceURIs.get_ObjectUri(ServiceURIs.enumObjectUri.self).ReplaceUri("repositoryId", repositoryInfo.RepositoryId, "id", parentId)), LinkRelationshipTypes.Up, MediaTypes.Entry, parentId, null));
            }
            if (_renditions is not null && _renditions.Count() > 0)
            {
                foreach (cmisRenditionType rendition in _renditions)
                    retVal.Add(elementFactory(new Uri(baseUri, ServiceURIs.get_ObjectUri(ServiceURIs.enumObjectUri.self).ReplaceUri("repositoryId", repositoryInfo.RepositoryId, "id", objId)), LinkRelationshipTypes.Alternate, rendition.Mimetype, null, rendition.Kind));
            }
            retVal.Add(elementFactory(new Uri(baseUri, ServiceURIs.get_RelationshipsUri(ServiceURIs.enumRelationshipsUri.objectId).ReplaceUri("repositoryId", repositoryInfo.RepositoryId, "id", objId)), LinkRelationshipTypes.Relationships, MediaTypes.Feed, null, null));
            if (repositoryInfo.Capabilities.CapabilityGetFolderTree)
            {
                retVal.Add(elementFactory(new Uri(baseUri, ServiceURIs.get_FolderTreeUri(ServiceURIs.enumFolderTreeUri.folderId).ReplaceUri("repositoryId", repositoryInfo.RepositoryId, "folderId", objId)), LinkRelationshipTypes.FolderTree, MediaTypes.Tree, null, null));
            }

            return retVal;
        }

        /// <summary>
      /// Returns links for a cmisfolder in System.ServiceModel.Syndication.SyndicationLink-format
      /// </summary>
      /// <param name="baseUri"></param>
      /// <param name="repositoryInfo"></param>
      /// <param name="parentId"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public List<ca.AtomLink> GetFolderLinks(Uri baseUri, cmisRepositoryInfoType repositoryInfo, string parentId)
        {
            return GetFolderLinks(baseUri, repositoryInfo, (uri, relationshipType, mediaType, id, renditionKind) => new ca.AtomLink(uri, relationshipType, mediaType, id, renditionKind), parentId);
        }
        /// <summary>
      /// Returns links for a cmisfolder in System.Xml.Linq.XElement-format
      /// </summary>
      /// <param name="baseUri"></param>
      /// <param name="repositoryInfo"></param>
      /// <param name="ns"></param>
      /// <param name="elementName"></param>
      /// <param name="parentId"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public List<XElement> GetFolderLinks(Uri baseUri, cmisRepositoryInfoType repositoryInfo, XNamespace ns, string elementName, string parentId)
        {
            {
                var withBlock = new ca.Factory.XElementBuilder(ns, elementName);
                return GetFolderLinks(baseUri, repositoryInfo, withBlock.CreateXElement, parentId);
            }
        }
        #endregion

        #region links for a cmisitem
        /// <summary>
      /// Returns links for a cmisitem
      /// </summary>
      /// <typeparam name="TLink"></typeparam>
      /// <param name="baseUri"></param>
      /// <param name="repositoryInfo"></param>
      /// <param name="elementFactory"></param>
      /// <returns></returns>
      /// <remarks>Used Link Relations:
      /// 3.11.7 Item Entry; 3.4.3 CMIS Link Relations</remarks>
        protected List<TLink> GetItemLinks<TLink>(Uri baseUri, cmisRepositoryInfoType repositoryInfo, ca.Factory.GenericDelegates<Uri, TLink>.CreateLinkDelegate elementFactory)
        {
            string objId = ObjectId;
            var retVal = GetLinks(baseUri, repositoryInfo, elementFactory);

            retVal.Add(elementFactory(new Uri(baseUri, ServiceURIs.get_ObjectUri(ServiceURIs.enumObjectUri.self).ReplaceUri("repositoryId", repositoryInfo.RepositoryId, "id", objId)), LinkRelationshipTypes.Edit, MediaTypes.Entry, null, null));
            retVal.Add(elementFactory(new Uri(baseUri, ServiceURIs.get_RelationshipsUri(ServiceURIs.enumRelationshipsUri.objectId).ReplaceUri("repositoryId", repositoryInfo.RepositoryId, "id", objId)), LinkRelationshipTypes.Relationships, MediaTypes.Feed, null, null));

            return retVal;
        }

        /// <summary>
      /// Returns links for a cmisitem in System.ServiceModel.Syndication.SyndicationLink-format
      /// </summary>
      /// <param name="baseUri"></param>
      /// <param name="repositoryInfo"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public List<ca.AtomLink> GetItemLinks(Uri baseUri, cmisRepositoryInfoType repositoryInfo)
        {
            return GetItemLinks(baseUri, repositoryInfo, (uri, relationshipType, mediaType, id, renditionKind) => new ca.AtomLink(uri, relationshipType, mediaType, id, renditionKind));
        }

        /// <summary>
      /// Returns links for a cmisitem in System.Xml.Linq.XElement-format
      /// </summary>
      /// <param name="baseUri"></param>
      /// <param name="repositoryInfo"></param>
      /// <param name="ns"></param>
      /// <param name="elementName"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public List<XElement> GetItemLinks(Uri baseUri, cmisRepositoryInfoType repositoryInfo, XNamespace ns, string elementName)
        {
            {
                var withBlock = new ca.Factory.XElementBuilder(ns, elementName);
                return GetItemLinks(baseUri, repositoryInfo, withBlock.CreateXElement);
            }
        }
        #endregion

        #region links for a cmispolicy
        /// <summary>
      /// Returns links for a cmispolicy
      /// </summary>
      /// <typeparam name="TLink"></typeparam>
      /// <param name="baseUri"></param>
      /// <param name="repositoryInfo"></param>
      /// <param name="elementFactory"></param>
      /// <returns></returns>
      /// <remarks>Used Link Relations:
      /// 3.11.6 Policy Entry; 3.4.3 CMIS Link Relations</remarks>
        protected List<TLink> GetPolicyLinks<TLink>(Uri baseUri, cmisRepositoryInfoType repositoryInfo, ca.Factory.GenericDelegates<Uri, TLink>.CreateLinkDelegate elementFactory)
        {
            string objId = ObjectId;
            var retVal = GetLinks(baseUri, repositoryInfo, elementFactory);

            retVal.Add(elementFactory(new Uri(baseUri, ServiceURIs.get_ObjectUri(ServiceURIs.enumObjectUri.self).ReplaceUri("repositoryId", repositoryInfo.RepositoryId, "id", objId)), LinkRelationshipTypes.Edit, MediaTypes.Entry, null, null));
            if (_renditions is not null && _renditions.Count() > 0)
            {
                foreach (cmisRenditionType rendition in _renditions)
                    retVal.Add(elementFactory(new Uri(baseUri, ServiceURIs.get_ObjectUri(ServiceURIs.enumObjectUri.self).ReplaceUri("repositoryId", repositoryInfo.RepositoryId, "id", objId)), LinkRelationshipTypes.Alternate, rendition.Mimetype, null, rendition.Kind));
            }
            retVal.Add(elementFactory(new Uri(baseUri, ServiceURIs.get_RelationshipsUri(ServiceURIs.enumRelationshipsUri.objectId).ReplaceUri("repositoryId", repositoryInfo.RepositoryId, "id", objId)), LinkRelationshipTypes.Relationships, MediaTypes.Feed, null, null));

            return retVal;
        }

        /// <summary>
      /// Returns links for a cmispolicy in System.ServiceModel.Syndication.SyndicationLink-format
      /// </summary>
      /// <param name="baseUri"></param>
      /// <param name="repositoryInfo"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public List<ca.AtomLink> GetPolicyLinks(Uri baseUri, cmisRepositoryInfoType repositoryInfo)
        {
            return GetPolicyLinks(baseUri, repositoryInfo, (uri, relationshipType, mediaType, id, renditionKind) => new ca.AtomLink(uri, relationshipType, mediaType, id, renditionKind));
        }

        /// <summary>
      /// Returns links for a cmispolicy in System.Xml.Linq.XElement-format
      /// </summary>
      /// <param name="baseUri"></param>
      /// <param name="repositoryInfo"></param>
      /// <param name="ns"></param>
      /// <param name="elementName"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public List<XElement> GetPolicyLinks(Uri baseUri, cmisRepositoryInfoType repositoryInfo, XNamespace ns, string elementName)
        {
            {
                var withBlock = new ca.Factory.XElementBuilder(ns, elementName);
                return GetPolicyLinks(baseUri, repositoryInfo, withBlock.CreateXElement);
            }
        }
        #endregion

        #region links for a cmisrelationship
        /// <summary>
      /// Returns links for a cmisrelationship
      /// </summary>
      /// <typeparam name="TLink"></typeparam>
      /// <param name="baseUri"></param>
      /// <param name="repositoryInfo"></param>
      /// <param name="elementFactory"></param>
      /// <param name="sourceId"></param>
      /// <param name="targetId"></param>
      /// <returns></returns>
      /// <remarks>Used Link Relations:
      /// 3.11.5 Relationship Entry; 3.4.3 CMIS Link Relations</remarks>
        protected List<TLink> GetRelationshipLinks<TLink>(Uri baseUri, cmisRepositoryInfoType repositoryInfo, ca.Factory.GenericDelegates<Uri, TLink>.CreateLinkDelegate elementFactory, string sourceId, string targetId)
        {
            var retVal = GetLinks(baseUri, repositoryInfo, elementFactory);

            retVal.Add(elementFactory(new Uri(baseUri, ServiceURIs.get_ObjectUri(ServiceURIs.enumObjectUri.self).ReplaceUri("repositoryId", repositoryInfo.RepositoryId, "id", ObjectId)), LinkRelationshipTypes.Edit, MediaTypes.Entry, null, null));
            retVal.Add(elementFactory(new Uri(baseUri, ServiceURIs.get_ObjectUri(ServiceURIs.enumObjectUri.self).ReplaceUri("repositoryId", repositoryInfo.RepositoryId, "id", sourceId)), LinkRelationshipTypes.Source, MediaTypes.Entry, sourceId, null));
            retVal.Add(elementFactory(new Uri(baseUri, ServiceURIs.get_ObjectUri(ServiceURIs.enumObjectUri.self).ReplaceUri("repositoryId", repositoryInfo.RepositoryId, "id", targetId)), LinkRelationshipTypes.Target, MediaTypes.Entry, targetId, null));

            return retVal;
        }

        /// <summary>
      /// Returns links for a cmisrelationship in System.ServiceModel.Syndication.SyndicationLink-format
      /// </summary>
      /// <param name="baseUri"></param>
      /// <param name="repositoryInfo"></param>
      /// <param name="sourceId"></param>
      /// <param name="targetId"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public List<ca.AtomLink> GetRelationshipLinks(Uri baseUri, cmisRepositoryInfoType repositoryInfo, string sourceId, string targetId)
        {
            return GetRelationshipLinks(baseUri, repositoryInfo, (uri, relationshipType, mediaType, id, renditionKind) => new ca.AtomLink(uri, relationshipType, mediaType, id, renditionKind), sourceId, targetId);
        }

        /// <summary>
      /// Returns links for a cmisrelationship in System.Xml.Linq.XElement-format
      /// </summary>
      /// <param name="baseUri"></param>
      /// <param name="repositoryInfo"></param>
      /// <param name="ns"></param>
      /// <param name="elementName"></param>
      /// <param name="sourceId"></param>
      /// <param name="targetId"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public List<XElement> GetRelationshipLinks(Uri baseUri, cmisRepositoryInfoType repositoryInfo, XNamespace ns, string elementName, string sourceId, string targetId)
        {
            {
                var withBlock = new ca.Factory.XElementBuilder(ns, elementName);
                return GetRelationshipLinks(baseUri, repositoryInfo, withBlock.CreateXElement, sourceId, targetId);
            }
        }
        #endregion

        #region links for a cmissecondary
        /// <summary>
      /// Returns links for a cmissecondary
      /// </summary>
      /// <typeparam name="TLink"></typeparam>
      /// <param name="baseUri"></param>
      /// <param name="repositoryInfo"></param>
      /// <param name="elementFactory"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        protected List<TLink> GetSecondaryLinks<TLink>(Uri baseUri, cmisRepositoryInfoType repositoryInfo, ca.Factory.GenericDelegates<Uri, TLink>.CreateLinkDelegate elementFactory)
        {
            return GetLinks(baseUri, repositoryInfo, elementFactory);
        }

        /// <summary>
      /// Returns links for a cmisitem in System.ServiceModel.Syndication.SyndicationLink-format
      /// </summary>
      /// <param name="baseUri"></param>
      /// <param name="repositoryInfo"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public List<ca.AtomLink> GetSecondaryLinks(Uri baseUri, cmisRepositoryInfoType repositoryInfo)
        {
            return GetSecondaryLinks(baseUri, repositoryInfo, (uri, relationshipType, mediaType, id, renditionKind) => new ca.AtomLink(uri, relationshipType, mediaType, id, renditionKind));
        }

        /// <summary>
      /// Returns links for a cmisitem in System.Xml.Linq.XElement-format
      /// </summary>
      /// <param name="baseUri"></param>
      /// <param name="repositoryInfo"></param>
      /// <param name="ns"></param>
      /// <param name="elementName"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public List<XElement> GetSecondaryLinks(Uri baseUri, cmisRepositoryInfoType repositoryInfo, XNamespace ns, string elementName)
        {
            {
                var withBlock = new ca.Factory.XElementBuilder(ns, elementName);
                return GetSecondaryLinks(baseUri, repositoryInfo, withBlock.CreateXElement);
            }
        }
        #endregion

        public cmisObjectTypeComparer get_Comparer(params string[] propertyNames)
        {
            return new cmisObjectTypeComparer(this, propertyNames);
        }

        /// <summary>
      /// Returns the objectTypeId followed by the secondaryObjectTypeIds separated by comma
      /// </summary>
        public string GetCompositeObjectTypeId()
        {
            return string.Join(",", GetObjectTypeIds());
        }

        /// <summary>
      /// Returns as first element the objectTypeId of the current object followed by the secondaryTypeIds if defined
      /// </summary>
        public IEnumerable<string> GetObjectTypeIds()
        {
            string objectTypeId = ObjectTypeId;
            string[] secondaryObjectTypeIds = SecondaryObjectTypeIds;
            var verify = new HashSet<string>();

            if (string.IsNullOrEmpty(objectTypeId))
            {
                yield return objectTypeId;
            }
            else
            {
                var objectTypeIds = objectTypeId.Split(',');

                for (int index = 0, loopTo = objectTypeIds.Length - 1; index <= loopTo; index++)
                {
                    objectTypeId = objectTypeIds[index];
                    if (verify.Add(objectTypeId))
                        yield return objectTypeId;
                }
            }

            // defined secondaryObjectTypes
            if (secondaryObjectTypeIds is not null)
            {
                for (int index = 0, loopTo1 = secondaryObjectTypeIds.Length - 1; index <= loopTo1; index++)
                {
                    string secondaryObjectTypeId = secondaryObjectTypeIds[index];
                    if (!string.IsNullOrEmpty(secondaryObjectTypeId) && verify.Add(secondaryObjectTypeId))
                        yield return secondaryObjectTypeId;
                }
            }
        }

        /// <summary>
      /// Search for a specified extension type
      /// </summary>
      /// <typeparam name="TExtension"></typeparam>
      /// <returns></returns>
      /// <remarks></remarks>
        public TExtension FindExtension<TExtension>() where TExtension : Extensions.Extension
        {
            return _properties is null ? null : _properties.FindExtension<TExtension>();
        }

        /// <summary>
      /// Access to properties via index or PropertyDefinitionId
      /// </summary>
      /// <value></value>
      /// <returns></returns>
      /// <remarks></remarks>
        public CmisObjectModel.Collections.Generic.ArrayMapper<Collections.cmisPropertiesType, Properties.cmisProperty> PropertiesAsReadOnly
        {
            get
            {
                return _properties is null ? null : _properties.PropertiesAsReadOnly;
            }
        }

        private CmisObjectModel.Collections.Generic.ArrayMapper<cmisObjectType, cmisObjectType> initial_relationshipsAsReadOnly() => new CmisObjectModel.Collections.Generic.ArrayMapper<cmisObjectType, cmisObjectType>(this, "Relationships", () => _relationships, "ObjectId", cmisRelation => { if (cmisRelation is ServiceModel.cmisObjectType) { return ((ServiceModel.cmisObjectType)cmisRelation).ServiceModel.ObjectId; } else { return cmisRelation.ObjectId; } });

        private CmisObjectModel.Collections.Generic.ArrayMapper<cmisObjectType, cmisObjectType> _relationshipsAsReadOnly;
        /// <summary>
      /// Access to relationships via index or objectId of the relationship
      /// </summary>
      /// <value></value>
      /// <returns></returns>
      /// <remarks></remarks>
        public CmisObjectModel.Collections.Generic.ArrayMapper<cmisObjectType, cmisObjectType> RelationshipsAsReadOnly
        {
            get
            {
                return _relationshipsAsReadOnly;
            }
        }

        /// <summary>
      /// link to the content of a cmisdocument
      /// </summary>
      /// <param name="baseUri"></param>
      /// <param name="repositoryInfo"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public ca.AtomLink GetContentLink(Uri baseUri, cmisRepositoryInfoType repositoryInfo, string mediaType = MediaTypes.Stream)
        {
            var uri = new Uri(baseUri, ServiceURIs.get_ContentUri(ServiceURIs.enumContentUri.objectId).ReplaceUri("repositoryId", repositoryInfo.RepositoryId, "id", ObjectId));
            return new ca.AtomLink(uri, LinkRelationshipTypes.ContentStream, mediaType);
        }

        /// <summary>
      /// link to the content of a cmisdocument
      /// </summary>
      /// <param name="baseUri"></param>
      /// <param name="repositoryInfo"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public static ca.AtomLink GetContentLink(Uri baseUri, cmisRepositoryInfoType repositoryInfo, string objectId, string mediaType)
        {
            var uri = new Uri(baseUri, ServiceURIs.get_ContentUri(ServiceURIs.enumContentUri.objectId).ReplaceUri("repositoryId", repositoryInfo.RepositoryId, "id", objectId));
            return new ca.AtomLink(uri, LinkRelationshipTypes.ContentStream, mediaType);
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

        public Dictionary<string, Properties.cmisProperty> GetProperties(enumKeySyntax keySyntax, params string[] propertyDefinitionIds)
        {
            if (_properties is not null)
            {
                return _properties.GetProperties(keySyntax, propertyDefinitionIds);
            }
            else
            {
                return new Dictionary<string, Properties.cmisProperty>();
            }
        }

        /// <summary>
      /// Handles all property changed events to make sure that _id and _typeId are up to date
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      /// <remarks></remarks>
        private void xmlSerializable_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            // Nothing todo..
        }

        public static Client.CmisObject.PreStage operator +(cmisObjectType arg1, Contracts.ICmisClient arg2)
        {
            return new Client.CmisObject.PreStage(arg2, arg1);
        }
        public static Client.CmisObject.PreStage operator +(Contracts.ICmisClient arg1, cmisObjectType arg2)
        {
            return new Client.CmisObject.PreStage(arg1, arg2);
        }

        public static explicit operator cmisObjectType(ca.AtomEntry value)
        {
            return value is null ? null : value.Object;
        }

    }
}