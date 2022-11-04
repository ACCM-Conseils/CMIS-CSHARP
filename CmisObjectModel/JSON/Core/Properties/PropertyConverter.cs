using System.Collections;
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
/* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.JSON.Core.Properties
{
    public abstract class PropertyConverter : Serialization.Generic.JavaScriptConverter<CmisObjectModel.Core.Properties.cmisProperty>
    {

        #region Constructors
        protected PropertyConverter() : base(new Serialization.CmisPropertyResolver())
        {
        }
        protected PropertyConverter(Serialization.Generic.ObjectResolver<CmisObjectModel.Core.Properties.cmisProperty> objectResolver) : base(objectResolver)
        {
        }
        #endregion

        protected override void Deserialize(SerializationContext context)
        {
            context.Object.PropertyDefinitionId = Read(context.Dictionary, "propertyDefinitionId", context.Object.PropertyDefinitionId);
            // type is readonly
            context.Object.LocalName = Read(context.Dictionary, "localName", context.Object.LocalName);
            context.Object.DisplayName = Read(context.Dictionary, "displayName", context.Object.DisplayName);
            context.Object.QueryName = Read(context.Dictionary, "queryName", context.Object.QueryName);

            object value = null;
            bool containsKey = context.Dictionary.TryGetValue("value", out value);

            if (context.Object is CmisObjectModel.Core.Properties.cmisPropertyDateTime)
            {
                CmisObjectModel.Core.Properties.cmisPropertyDateTime @object = (CmisObjectModel.Core.Properties.cmisPropertyDateTime)context.Object;
                if (!containsKey || value is null)
                {
                    @object.Values = null;
                }
                else if (value is ICollection)
                {
                    @object.Values = ReadArray(context.Dictionary, "value", @object.Values);
                }
                else
                {
                    @object.Value = Read(context.Dictionary, "value", @object.Value);
                }
            }
            else if (!containsKey || value is null)
            {
                context.Object.Values = null;
            }
            else if (value is ICollection)
            {
                context.Object.Values = ReadArray(context.Dictionary, "value", context.Object.Values);
            }
            else
            {
                context.Object.Value = Read(context.Dictionary, "value", context.Object.Value);
            }
        }

        protected override void Serialize(SerializationContext context)
        {
            context.Add("propertyDefinitionId", context.Object.PropertyDefinitionId);
            context.Add("type", context.Object.Type.GetName());
            if (!string.IsNullOrEmpty(context.Object.LocalName))
                context.Add("localName", context.Object.LocalName);
            if (!string.IsNullOrEmpty(context.Object.DisplayName))
                context.Add("displayName", context.Object.DisplayName);
            if (!string.IsNullOrEmpty(context.Object.QueryName))
                context.Add("queryName", context.Object.QueryName);
            context.Add("value", SerializeValue(context));
        }

        protected abstract object SerializeValue(SerializationContext context);
    }
}