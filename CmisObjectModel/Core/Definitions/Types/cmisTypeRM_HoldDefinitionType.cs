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

namespace CmisObjectModel.Core.Definitions.Types
{
    [Attributes.CmisTypeInfo(CMISTypeName, TargetTypeName, DefaultElementName)]
    public partial class cmisTypeRM_HoldDefinitionType
    {

        public cmisTypeRM_HoldDefinitionType(string id, string localName, string displayName, string queryName, params Properties.cmisPropertyDefinitionType[] propertyDefinitions) : base(id, localName, displayName, queryName, cmisTypeSecondaryDefinitionType.TargetTypeName, propertyDefinitions)
        {
        }
        public cmisTypeRM_HoldDefinitionType(string id, string localName, string displayName, string queryName, string parentId, params Properties.cmisPropertyDefinitionType[] propertyDefinitions) : base(id, localName, displayName, queryName, parentId, propertyDefinitions)
        {
        }

        #region Constants
        public new const string CMISTypeName = "cmis:cmisTypeRM_HoldDefinitionType";
        public new const string TargetTypeName = "cmis:rm_hold";
        public new const string DefaultElementName = "typeRM_HoldDefinition";
        #endregion

        /// <summary>
      /// Returns the defaultProperties of a TypeRM_HoldDefinition-instance
      /// </summary>
        public static new List<Properties.cmisPropertyDefinitionType> GetDefaultProperties(string localNamespace, bool isBaseType, bool holdIdsQueryable = true, enumUpdatability holdIdsUpdatability = enumUpdatability.readwrite)
        {
            var retVal = cmisTypeSecondaryDefinitionType.GetDefaultProperties(localNamespace, false);

            {
                var withBlock = new PredefinedPropertyDefinitionFactory(localNamespace, isBaseType);
                retVal.Add(withBlock.RM_HoldIds(queryable: holdIdsQueryable, updatability: holdIdsUpdatability));
            }

            return retVal;
        }

        protected override string GetCmisTypeName()
        {
            return CMISTypeName;
        }

    }
}