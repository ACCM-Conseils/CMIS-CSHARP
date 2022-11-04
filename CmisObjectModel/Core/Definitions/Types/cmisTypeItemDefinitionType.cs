﻿using System.Collections.Generic;
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

namespace CmisObjectModel.Core.Definitions.Types
{
    [Attributes.CmisTypeInfo(CMISTypeName, TargetTypeName, DefaultElementName)]
    public partial class cmisTypeItemDefinitionType
    {

        public cmisTypeItemDefinitionType(string id, string localName, string displayName, string queryName, params Properties.cmisPropertyDefinitionType[] propertyDefinitions) : base(id, localName, displayName, queryName, propertyDefinitions)
        {
        }
        public cmisTypeItemDefinitionType(string id, string localName, string displayName, string queryName, string parentId, params Properties.cmisPropertyDefinitionType[] propertyDefinitions) : base(id, localName, displayName, queryName, parentId, propertyDefinitions)
        {
        }

        #region Constants
        public new const string CMISTypeName = "cmis:cmisTypeItemDefinitionType";
        public const string TargetTypeName = "cmis:item";
        public const string DefaultElementName = "typeItemDefinition";
        #endregion

        protected override enumBaseObjectTypeIds _baseId
        {
            get
            {
                return enumBaseObjectTypeIds.cmisItem;
            }
        }

        /// <summary>
      /// Returns the defaultProperties of a TypeItemDefinition-instance
      /// </summary>
      /// <param name="localNamespace"></param>
      /// <param name="isBaseType"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public static List<Properties.cmisPropertyDefinitionType> GetDefaultProperties(string localNamespace, bool isBaseType)
        {
            {
                var withBlock = new PredefinedPropertyDefinitionFactory(localNamespace, isBaseType);
                return new List<Properties.cmisPropertyDefinitionType>() { withBlock.Name(), withBlock.Description(), withBlock.ObjectId(), withBlock.BaseTypeId(), withBlock.ObjectTypeId(), withBlock.SecondaryObjectTypeIds(), withBlock.CreatedBy(), withBlock.CreationDate(), withBlock.LastModifiedBy(), withBlock.LastModificationDate(), withBlock.ChangeToken() };
            }
        }

        protected override string GetCmisTypeName()
        {
            return CMISTypeName;
        }

        protected override void InitClass()
        {
            MyBaseInitClass();
            _id = TargetTypeName;
            _queryName = TargetTypeName;
        }

    }
}