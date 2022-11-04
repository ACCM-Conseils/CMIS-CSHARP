using System;
using System.Collections.Generic;
using sx = System.Xml;
// ***********************************************************************************************************************
// * Project: CmisObjectModelLibrary
// * Copyright (c) 2014, Brügmann Software GmbH, Papenburg, All rights reserved
// * Author: auto-generated code
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
/* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.Core
{
    /// <summary>
   /// </summary>
   /// <remarks>
   /// see cmisAllowableActionsType
   /// in http://docs.oasis-open.org/cmis/CMIS/v1.1/cs01/schema/CMIS-Core.xsd
   /// </remarks>
    [System.CodeDom.Compiler.GeneratedCode("CmisXsdConverter", "1.0.0.0")]
    public partial class cmisAllowableActionsType : Serialization.XmlSerializable
    {

        public cmisAllowableActionsType()
        {
        }
        /// <summary>
      /// this constructor is only used if derived classes from this class needs an InitClass()-call
      /// </summary>
      /// <param name="initClassSupported"></param>
      /// <remarks></remarks>
        protected cmisAllowableActionsType(bool? initClassSupported) : base(initClassSupported)
        {
        }

        #region IXmlSerializable
        private static Dictionary<string, Action<cmisAllowableActionsType, string>> _setter = new Dictionary<string, Action<cmisAllowableActionsType, string>>() { }; // _setter

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

        /// <summary>
      /// Deserialization of all properties stored in subnodes
      /// </summary>
      /// <param name="reader"></param>
      /// <remarks></remarks>
        protected override void ReadXmlCore(sx.XmlReader reader, Serialization.XmlAttributeOverrides attributeOverrides)
        {
            _canDeleteObject = Read(reader, attributeOverrides, "canDeleteObject", Constants.Namespaces.cmis, _canDeleteObject);
            _canUpdateProperties = Read(reader, attributeOverrides, "canUpdateProperties", Constants.Namespaces.cmis, _canUpdateProperties);
            _canGetFolderTree = Read(reader, attributeOverrides, "canGetFolderTree", Constants.Namespaces.cmis, _canGetFolderTree);
            _canGetProperties = Read(reader, attributeOverrides, "canGetProperties", Constants.Namespaces.cmis, _canGetProperties);
            _canGetObjectRelationships = Read(reader, attributeOverrides, "canGetObjectRelationships", Constants.Namespaces.cmis, _canGetObjectRelationships);
            _canGetObjectParents = Read(reader, attributeOverrides, "canGetObjectParents", Constants.Namespaces.cmis, _canGetObjectParents);
            _canGetFolderParent = Read(reader, attributeOverrides, "canGetFolderParent", Constants.Namespaces.cmis, _canGetFolderParent);
            _canGetDescendants = Read(reader, attributeOverrides, "canGetDescendants", Constants.Namespaces.cmis, _canGetDescendants);
            _canMoveObject = Read(reader, attributeOverrides, "canMoveObject", Constants.Namespaces.cmis, _canMoveObject);
            _canDeleteContentStream = Read(reader, attributeOverrides, "canDeleteContentStream", Constants.Namespaces.cmis, _canDeleteContentStream);
            _canCheckOut = Read(reader, attributeOverrides, "canCheckOut", Constants.Namespaces.cmis, _canCheckOut);
            _canCancelCheckOut = Read(reader, attributeOverrides, "canCancelCheckOut", Constants.Namespaces.cmis, _canCancelCheckOut);
            _canCheckIn = Read(reader, attributeOverrides, "canCheckIn", Constants.Namespaces.cmis, _canCheckIn);
            _canSetContentStream = Read(reader, attributeOverrides, "canSetContentStream", Constants.Namespaces.cmis, _canSetContentStream);
            _canGetAllVersions = Read(reader, attributeOverrides, "canGetAllVersions", Constants.Namespaces.cmis, _canGetAllVersions);
            _canAddObjectToFolder = Read(reader, attributeOverrides, "canAddObjectToFolder", Constants.Namespaces.cmis, _canAddObjectToFolder);
            _canRemoveObjectFromFolder = Read(reader, attributeOverrides, "canRemoveObjectFromFolder", Constants.Namespaces.cmis, _canRemoveObjectFromFolder);
            _canGetContentStream = Read(reader, attributeOverrides, "canGetContentStream", Constants.Namespaces.cmis, _canGetContentStream);
            _canApplyPolicy = Read(reader, attributeOverrides, "canApplyPolicy", Constants.Namespaces.cmis, _canApplyPolicy);
            _canGetAppliedPolicies = Read(reader, attributeOverrides, "canGetAppliedPolicies", Constants.Namespaces.cmis, _canGetAppliedPolicies);
            _canRemovePolicy = Read(reader, attributeOverrides, "canRemovePolicy", Constants.Namespaces.cmis, _canRemovePolicy);
            _canGetChildren = Read(reader, attributeOverrides, "canGetChildren", Constants.Namespaces.cmis, _canGetChildren);
            _canCreateDocument = Read(reader, attributeOverrides, "canCreateDocument", Constants.Namespaces.cmis, _canCreateDocument);
            _canCreateFolder = Read(reader, attributeOverrides, "canCreateFolder", Constants.Namespaces.cmis, _canCreateFolder);
            _canCreateRelationship = Read(reader, attributeOverrides, "canCreateRelationship", Constants.Namespaces.cmis, _canCreateRelationship);
            _canCreateItem = Read(reader, attributeOverrides, "canCreateItem", Constants.Namespaces.cmis, _canCreateItem);
            _canDeleteTree = Read(reader, attributeOverrides, "canDeleteTree", Constants.Namespaces.cmis, _canDeleteTree);
            _canGetRenditions = Read(reader, attributeOverrides, "canGetRenditions", Constants.Namespaces.cmis, _canGetRenditions);
            _canGetACL = Read(reader, attributeOverrides, "canGetACL", Constants.Namespaces.cmis, _canGetACL);
            _canApplyACL = Read(reader, attributeOverrides, "canApplyACL", Constants.Namespaces.cmis, _canApplyACL);
        }

        /// <summary>
      /// Serialization of properties
      /// </summary>
      /// <param name="writer"></param>
      /// <remarks></remarks>
        protected override void WriteXmlCore(sx.XmlWriter writer, Serialization.XmlAttributeOverrides attributeOverrides)
        {
            if (_canDeleteObject.HasValue)
                WriteElement(writer, attributeOverrides, "canDeleteObject", Constants.Namespaces.cmis, CommonFunctions.Convert(_canDeleteObject));
            if (_canUpdateProperties.HasValue)
                WriteElement(writer, attributeOverrides, "canUpdateProperties", Constants.Namespaces.cmis, CommonFunctions.Convert(_canUpdateProperties));
            if (_canGetFolderTree.HasValue)
                WriteElement(writer, attributeOverrides, "canGetFolderTree", Constants.Namespaces.cmis, CommonFunctions.Convert(_canGetFolderTree));
            if (_canGetProperties.HasValue)
                WriteElement(writer, attributeOverrides, "canGetProperties", Constants.Namespaces.cmis, CommonFunctions.Convert(_canGetProperties));
            if (_canGetObjectRelationships.HasValue)
                WriteElement(writer, attributeOverrides, "canGetObjectRelationships", Constants.Namespaces.cmis, CommonFunctions.Convert(_canGetObjectRelationships));
            if (_canGetObjectParents.HasValue)
                WriteElement(writer, attributeOverrides, "canGetObjectParents", Constants.Namespaces.cmis, CommonFunctions.Convert(_canGetObjectParents));
            if (_canGetFolderParent.HasValue)
                WriteElement(writer, attributeOverrides, "canGetFolderParent", Constants.Namespaces.cmis, CommonFunctions.Convert(_canGetFolderParent));
            if (_canGetDescendants.HasValue)
                WriteElement(writer, attributeOverrides, "canGetDescendants", Constants.Namespaces.cmis, CommonFunctions.Convert(_canGetDescendants));
            if (_canMoveObject.HasValue)
                WriteElement(writer, attributeOverrides, "canMoveObject", Constants.Namespaces.cmis, CommonFunctions.Convert(_canMoveObject));
            if (_canDeleteContentStream.HasValue)
                WriteElement(writer, attributeOverrides, "canDeleteContentStream", Constants.Namespaces.cmis, CommonFunctions.Convert(_canDeleteContentStream));
            if (_canCheckOut.HasValue)
                WriteElement(writer, attributeOverrides, "canCheckOut", Constants.Namespaces.cmis, CommonFunctions.Convert(_canCheckOut));
            if (_canCancelCheckOut.HasValue)
                WriteElement(writer, attributeOverrides, "canCancelCheckOut", Constants.Namespaces.cmis, CommonFunctions.Convert(_canCancelCheckOut));
            if (_canCheckIn.HasValue)
                WriteElement(writer, attributeOverrides, "canCheckIn", Constants.Namespaces.cmis, CommonFunctions.Convert(_canCheckIn));
            if (_canSetContentStream.HasValue)
                WriteElement(writer, attributeOverrides, "canSetContentStream", Constants.Namespaces.cmis, CommonFunctions.Convert(_canSetContentStream));
            if (_canGetAllVersions.HasValue)
                WriteElement(writer, attributeOverrides, "canGetAllVersions", Constants.Namespaces.cmis, CommonFunctions.Convert(_canGetAllVersions));
            if (_canAddObjectToFolder.HasValue)
                WriteElement(writer, attributeOverrides, "canAddObjectToFolder", Constants.Namespaces.cmis, CommonFunctions.Convert(_canAddObjectToFolder));
            if (_canRemoveObjectFromFolder.HasValue)
                WriteElement(writer, attributeOverrides, "canRemoveObjectFromFolder", Constants.Namespaces.cmis, CommonFunctions.Convert(_canRemoveObjectFromFolder));
            if (_canGetContentStream.HasValue)
                WriteElement(writer, attributeOverrides, "canGetContentStream", Constants.Namespaces.cmis, CommonFunctions.Convert(_canGetContentStream));
            if (_canApplyPolicy.HasValue)
                WriteElement(writer, attributeOverrides, "canApplyPolicy", Constants.Namespaces.cmis, CommonFunctions.Convert(_canApplyPolicy));
            if (_canGetAppliedPolicies.HasValue)
                WriteElement(writer, attributeOverrides, "canGetAppliedPolicies", Constants.Namespaces.cmis, CommonFunctions.Convert(_canGetAppliedPolicies));
            if (_canRemovePolicy.HasValue)
                WriteElement(writer, attributeOverrides, "canRemovePolicy", Constants.Namespaces.cmis, CommonFunctions.Convert(_canRemovePolicy));
            if (_canGetChildren.HasValue)
                WriteElement(writer, attributeOverrides, "canGetChildren", Constants.Namespaces.cmis, CommonFunctions.Convert(_canGetChildren));
            if (_canCreateDocument.HasValue)
                WriteElement(writer, attributeOverrides, "canCreateDocument", Constants.Namespaces.cmis, CommonFunctions.Convert(_canCreateDocument));
            if (_canCreateFolder.HasValue)
                WriteElement(writer, attributeOverrides, "canCreateFolder", Constants.Namespaces.cmis, CommonFunctions.Convert(_canCreateFolder));
            if (_canCreateRelationship.HasValue)
                WriteElement(writer, attributeOverrides, "canCreateRelationship", Constants.Namespaces.cmis, CommonFunctions.Convert(_canCreateRelationship));
            if (_canCreateItem.HasValue)
                WriteElement(writer, attributeOverrides, "canCreateItem", Constants.Namespaces.cmis, CommonFunctions.Convert(_canCreateItem));
            if (_canDeleteTree.HasValue)
                WriteElement(writer, attributeOverrides, "canDeleteTree", Constants.Namespaces.cmis, CommonFunctions.Convert(_canDeleteTree));
            if (_canGetRenditions.HasValue)
                WriteElement(writer, attributeOverrides, "canGetRenditions", Constants.Namespaces.cmis, CommonFunctions.Convert(_canGetRenditions));
            if (_canGetACL.HasValue)
                WriteElement(writer, attributeOverrides, "canGetACL", Constants.Namespaces.cmis, CommonFunctions.Convert(_canGetACL));
            if (_canApplyACL.HasValue)
                WriteElement(writer, attributeOverrides, "canApplyACL", Constants.Namespaces.cmis, CommonFunctions.Convert(_canApplyACL));
        }
        #endregion

        protected bool? _canAddObjectToFolder;
        public virtual bool? CanAddObjectToFolder
        {
            get
            {
                return _canAddObjectToFolder;
            }
            set
            {
                if (!_canAddObjectToFolder.Equals(value))
                {
                    var oldValue = _canAddObjectToFolder;
                    _canAddObjectToFolder = value;
                    OnPropertyChanged("CanAddObjectToFolder", value, oldValue);
                }
            }
        } // CanAddObjectToFolder

        protected bool? _canApplyACL;
        public virtual bool? CanApplyACL
        {
            get
            {
                return _canApplyACL;
            }
            set
            {
                if (!_canApplyACL.Equals(value))
                {
                    var oldValue = _canApplyACL;
                    _canApplyACL = value;
                    OnPropertyChanged("CanApplyACL", value, oldValue);
                }
            }
        } // CanApplyACL

        protected bool? _canApplyPolicy;
        public virtual bool? CanApplyPolicy
        {
            get
            {
                return _canApplyPolicy;
            }
            set
            {
                if (!_canApplyPolicy.Equals(value))
                {
                    var oldValue = _canApplyPolicy;
                    _canApplyPolicy = value;
                    OnPropertyChanged("CanApplyPolicy", value, oldValue);
                }
            }
        } // CanApplyPolicy

        protected bool? _canCancelCheckOut;
        public virtual bool? CanCancelCheckOut
        {
            get
            {
                return _canCancelCheckOut;
            }
            set
            {
                if (!_canCancelCheckOut.Equals(value))
                {
                    var oldValue = _canCancelCheckOut;
                    _canCancelCheckOut = value;
                    OnPropertyChanged("CanCancelCheckOut", value, oldValue);
                }
            }
        } // CanCancelCheckOut

        protected bool? _canCheckIn;
        public virtual bool? CanCheckIn
        {
            get
            {
                return _canCheckIn;
            }
            set
            {
                if (!_canCheckIn.Equals(value))
                {
                    var oldValue = _canCheckIn;
                    _canCheckIn = value;
                    OnPropertyChanged("CanCheckIn", value, oldValue);
                }
            }
        } // CanCheckIn

        protected bool? _canCheckOut;
        public virtual bool? CanCheckOut
        {
            get
            {
                return _canCheckOut;
            }
            set
            {
                if (!_canCheckOut.Equals(value))
                {
                    var oldValue = _canCheckOut;
                    _canCheckOut = value;
                    OnPropertyChanged("CanCheckOut", value, oldValue);
                }
            }
        } // CanCheckOut

        protected bool? _canCreateDocument;
        public virtual bool? CanCreateDocument
        {
            get
            {
                return _canCreateDocument;
            }
            set
            {
                if (!_canCreateDocument.Equals(value))
                {
                    var oldValue = _canCreateDocument;
                    _canCreateDocument = value;
                    OnPropertyChanged("CanCreateDocument", value, oldValue);
                }
            }
        } // CanCreateDocument

        protected bool? _canCreateFolder;
        public virtual bool? CanCreateFolder
        {
            get
            {
                return _canCreateFolder;
            }
            set
            {
                if (!_canCreateFolder.Equals(value))
                {
                    var oldValue = _canCreateFolder;
                    _canCreateFolder = value;
                    OnPropertyChanged("CanCreateFolder", value, oldValue);
                }
            }
        } // CanCreateFolder

        protected bool? _canCreateItem;
        public virtual bool? CanCreateItem
        {
            get
            {
                return _canCreateItem;
            }
            set
            {
                if (!_canCreateItem.Equals(value))
                {
                    var oldValue = _canCreateItem;
                    _canCreateItem = value;
                    OnPropertyChanged("CanCreateItem", value, oldValue);
                }
            }
        } // CanCreateItem

        protected bool? _canCreateRelationship;
        public virtual bool? CanCreateRelationship
        {
            get
            {
                return _canCreateRelationship;
            }
            set
            {
                if (!_canCreateRelationship.Equals(value))
                {
                    var oldValue = _canCreateRelationship;
                    _canCreateRelationship = value;
                    OnPropertyChanged("CanCreateRelationship", value, oldValue);
                }
            }
        } // CanCreateRelationship

        protected bool? _canDeleteContentStream;
        public virtual bool? CanDeleteContentStream
        {
            get
            {
                return _canDeleteContentStream;
            }
            set
            {
                if (!_canDeleteContentStream.Equals(value))
                {
                    var oldValue = _canDeleteContentStream;
                    _canDeleteContentStream = value;
                    OnPropertyChanged("CanDeleteContentStream", value, oldValue);
                }
            }
        } // CanDeleteContentStream

        protected bool? _canDeleteObject;
        public virtual bool? CanDeleteObject
        {
            get
            {
                return _canDeleteObject;
            }
            set
            {
                if (!_canDeleteObject.Equals(value))
                {
                    var oldValue = _canDeleteObject;
                    _canDeleteObject = value;
                    OnPropertyChanged("CanDeleteObject", value, oldValue);
                }
            }
        } // CanDeleteObject

        protected bool? _canDeleteTree;
        public virtual bool? CanDeleteTree
        {
            get
            {
                return _canDeleteTree;
            }
            set
            {
                if (!_canDeleteTree.Equals(value))
                {
                    var oldValue = _canDeleteTree;
                    _canDeleteTree = value;
                    OnPropertyChanged("CanDeleteTree", value, oldValue);
                }
            }
        } // CanDeleteTree

        protected bool? _canGetACL;
        public virtual bool? CanGetACL
        {
            get
            {
                return _canGetACL;
            }
            set
            {
                if (!_canGetACL.Equals(value))
                {
                    var oldValue = _canGetACL;
                    _canGetACL = value;
                    OnPropertyChanged("CanGetACL", value, oldValue);
                }
            }
        } // CanGetACL

        protected bool? _canGetAllVersions;
        public virtual bool? CanGetAllVersions
        {
            get
            {
                return _canGetAllVersions;
            }
            set
            {
                if (!_canGetAllVersions.Equals(value))
                {
                    var oldValue = _canGetAllVersions;
                    _canGetAllVersions = value;
                    OnPropertyChanged("CanGetAllVersions", value, oldValue);
                }
            }
        } // CanGetAllVersions

        protected bool? _canGetAppliedPolicies;
        public virtual bool? CanGetAppliedPolicies
        {
            get
            {
                return _canGetAppliedPolicies;
            }
            set
            {
                if (!_canGetAppliedPolicies.Equals(value))
                {
                    var oldValue = _canGetAppliedPolicies;
                    _canGetAppliedPolicies = value;
                    OnPropertyChanged("CanGetAppliedPolicies", value, oldValue);
                }
            }
        } // CanGetAppliedPolicies

        protected bool? _canGetChildren;
        public virtual bool? CanGetChildren
        {
            get
            {
                return _canGetChildren;
            }
            set
            {
                if (!_canGetChildren.Equals(value))
                {
                    var oldValue = _canGetChildren;
                    _canGetChildren = value;
                    OnPropertyChanged("CanGetChildren", value, oldValue);
                }
            }
        } // CanGetChildren

        protected bool? _canGetContentStream;
        public virtual bool? CanGetContentStream
        {
            get
            {
                return _canGetContentStream;
            }
            set
            {
                if (!_canGetContentStream.Equals(value))
                {
                    var oldValue = _canGetContentStream;
                    _canGetContentStream = value;
                    OnPropertyChanged("CanGetContentStream", value, oldValue);
                }
            }
        } // CanGetContentStream

        protected bool? _canGetDescendants;
        public virtual bool? CanGetDescendants
        {
            get
            {
                return _canGetDescendants;
            }
            set
            {
                if (!_canGetDescendants.Equals(value))
                {
                    var oldValue = _canGetDescendants;
                    _canGetDescendants = value;
                    OnPropertyChanged("CanGetDescendants", value, oldValue);
                }
            }
        } // CanGetDescendants

        protected bool? _canGetFolderParent;
        public virtual bool? CanGetFolderParent
        {
            get
            {
                return _canGetFolderParent;
            }
            set
            {
                if (!_canGetFolderParent.Equals(value))
                {
                    var oldValue = _canGetFolderParent;
                    _canGetFolderParent = value;
                    OnPropertyChanged("CanGetFolderParent", value, oldValue);
                }
            }
        } // CanGetFolderParent

        protected bool? _canGetFolderTree;
        public virtual bool? CanGetFolderTree
        {
            get
            {
                return _canGetFolderTree;
            }
            set
            {
                if (!_canGetFolderTree.Equals(value))
                {
                    var oldValue = _canGetFolderTree;
                    _canGetFolderTree = value;
                    OnPropertyChanged("CanGetFolderTree", value, oldValue);
                }
            }
        } // CanGetFolderTree

        protected bool? _canGetObjectParents;
        public virtual bool? CanGetObjectParents
        {
            get
            {
                return _canGetObjectParents;
            }
            set
            {
                if (!_canGetObjectParents.Equals(value))
                {
                    var oldValue = _canGetObjectParents;
                    _canGetObjectParents = value;
                    OnPropertyChanged("CanGetObjectParents", value, oldValue);
                }
            }
        } // CanGetObjectParents

        protected bool? _canGetObjectRelationships;
        public virtual bool? CanGetObjectRelationships
        {
            get
            {
                return _canGetObjectRelationships;
            }
            set
            {
                if (!_canGetObjectRelationships.Equals(value))
                {
                    var oldValue = _canGetObjectRelationships;
                    _canGetObjectRelationships = value;
                    OnPropertyChanged("CanGetObjectRelationships", value, oldValue);
                }
            }
        } // CanGetObjectRelationships

        protected bool? _canGetProperties;
        public virtual bool? CanGetProperties
        {
            get
            {
                return _canGetProperties;
            }
            set
            {
                if (!_canGetProperties.Equals(value))
                {
                    var oldValue = _canGetProperties;
                    _canGetProperties = value;
                    OnPropertyChanged("CanGetProperties", value, oldValue);
                }
            }
        } // CanGetProperties

        protected bool? _canGetRenditions;
        public virtual bool? CanGetRenditions
        {
            get
            {
                return _canGetRenditions;
            }
            set
            {
                if (!_canGetRenditions.Equals(value))
                {
                    var oldValue = _canGetRenditions;
                    _canGetRenditions = value;
                    OnPropertyChanged("CanGetRenditions", value, oldValue);
                }
            }
        } // CanGetRenditions

        protected bool? _canMoveObject;
        public virtual bool? CanMoveObject
        {
            get
            {
                return _canMoveObject;
            }
            set
            {
                if (!_canMoveObject.Equals(value))
                {
                    var oldValue = _canMoveObject;
                    _canMoveObject = value;
                    OnPropertyChanged("CanMoveObject", value, oldValue);
                }
            }
        } // CanMoveObject

        protected bool? _canRemoveObjectFromFolder;
        public virtual bool? CanRemoveObjectFromFolder
        {
            get
            {
                return _canRemoveObjectFromFolder;
            }
            set
            {
                if (!_canRemoveObjectFromFolder.Equals(value))
                {
                    var oldValue = _canRemoveObjectFromFolder;
                    _canRemoveObjectFromFolder = value;
                    OnPropertyChanged("CanRemoveObjectFromFolder", value, oldValue);
                }
            }
        } // CanRemoveObjectFromFolder

        protected bool? _canRemovePolicy;
        public virtual bool? CanRemovePolicy
        {
            get
            {
                return _canRemovePolicy;
            }
            set
            {
                if (!_canRemovePolicy.Equals(value))
                {
                    var oldValue = _canRemovePolicy;
                    _canRemovePolicy = value;
                    OnPropertyChanged("CanRemovePolicy", value, oldValue);
                }
            }
        } // CanRemovePolicy

        protected bool? _canSetContentStream;
        public virtual bool? CanSetContentStream
        {
            get
            {
                return _canSetContentStream;
            }
            set
            {
                if (!_canSetContentStream.Equals(value))
                {
                    var oldValue = _canSetContentStream;
                    _canSetContentStream = value;
                    OnPropertyChanged("CanSetContentStream", value, oldValue);
                }
            }
        } // CanSetContentStream

        protected bool? _canUpdateProperties;
        public virtual bool? CanUpdateProperties
        {
            get
            {
                return _canUpdateProperties;
            }
            set
            {
                if (!_canUpdateProperties.Equals(value))
                {
                    var oldValue = _canUpdateProperties;
                    _canUpdateProperties = value;
                    OnPropertyChanged("CanUpdateProperties", value, oldValue);
                }
            }
        } // CanUpdateProperties

    }
}