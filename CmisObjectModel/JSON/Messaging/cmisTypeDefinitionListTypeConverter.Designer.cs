﻿
// ***********************************************************************************************************************
// * Project: CmisObjectModelLibrary
// * Copyright (c) 2014, Brügmann Software GmbH, Papenburg, All rights reserved
// * Author: BSW_BER
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
/* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.Messaging
{
    [Attributes.JavaScriptConverter(typeof(JSON.Messaging.cmisTypeDefinitionListTypeConverter))]
    public partial class cmisTypeDefinitionListType
    {
    }
}

namespace CmisObjectModel.JSON.Messaging
{
    [Attributes.AutoGenerated()]
    public partial class cmisTypeDefinitionListTypeConverter : Serialization.Generic.JavaScriptConverter<CmisObjectModel.Messaging.cmisTypeDefinitionListType>
    {

        #region Constructors
        public cmisTypeDefinitionListTypeConverter() : base(new Serialization.Generic.DefaultObjectResolver<CmisObjectModel.Messaging.cmisTypeDefinitionListType>())
        {
        }
        public cmisTypeDefinitionListTypeConverter(Serialization.Generic.ObjectResolver<CmisObjectModel.Messaging.cmisTypeDefinitionListType> objectResolver) : base(objectResolver)
        {
        }
        #endregion

        protected override void Deserialize(SerializationContext context)
        {
            context.Object.HasMoreItems = Read(context.Dictionary, "hasMoreItems", context.Object.HasMoreItems);
            context.Object.NumItems = ReadNullable(context.Dictionary, "numItems", context.Object.NumItems);
            context.Object.Types = ReadArray(context, "types", context.Object.Types);
        }

        protected override void Serialize(SerializationContext context)
        {
            context.Add("hasMoreItems", context.Object.HasMoreItems);
            if (context.Object.NumItems.HasValue)
                context.Add("numItems", context.Object.NumItems.Value);
            WriteArray(context, context.Object.Types, "types");
        }
    }
}