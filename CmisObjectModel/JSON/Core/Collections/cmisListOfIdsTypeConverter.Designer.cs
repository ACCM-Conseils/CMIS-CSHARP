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
namespace CmisObjectModel.Core.Collections
{
    [Attributes.JavaScriptConverter(typeof(JSON.Core.Collections.cmisListOfIdsTypeConverter))]
    public partial class cmisListOfIdsType
    {
    }
}

namespace CmisObjectModel.JSON.Core.Collections
{
    [Attributes.AutoGenerated()]
    public partial class cmisListOfIdsTypeConverter : Serialization.Generic.JavaScriptConverter<CmisObjectModel.Core.Collections.cmisListOfIdsType>
    {

        #region Constructors
        public cmisListOfIdsTypeConverter() : base(new Serialization.Generic.DefaultObjectResolver<CmisObjectModel.Core.Collections.cmisListOfIdsType>())
        {
        }
        public cmisListOfIdsTypeConverter(Serialization.Generic.ObjectResolver<CmisObjectModel.Core.Collections.cmisListOfIdsType> objectResolver) : base(objectResolver)
        {
        }
        #endregion

        protected override void Deserialize(SerializationContext context)
        {
            context.Object.Ids = ReadArray(context.Dictionary, "ids", context.Object.Ids);
        }

        protected override void Serialize(SerializationContext context)
        {
            WriteArray(context, context.Object.Ids, "ids");
        }
    }
}