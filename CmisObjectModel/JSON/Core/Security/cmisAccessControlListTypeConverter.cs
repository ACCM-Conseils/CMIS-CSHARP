
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
using ccs = CmisObjectModel.Core.Security;
/* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.Core.Security
{
    [Attributes.JavaScriptConverter(typeof(JSON.Core.Security.cmisAccessControlListTypeConverter))]
    public partial class cmisAccessControlListType
    {
    }
}

namespace CmisObjectModel.JSON.Core.Security
{
    public class cmisAccessControlListTypeConverter : Serialization.Generic.JavaScriptConverter<ccs.cmisAccessControlListType>
    {

        #region Constructors
        public cmisAccessControlListTypeConverter() : base(new Serialization.Generic.DefaultObjectResolver<ccs.cmisAccessControlListType>())
        {
        }
        public cmisAccessControlListTypeConverter(Serialization.Generic.ObjectResolver<ccs.cmisAccessControlListType> objectResolver) : base(objectResolver)
        {
        }
        #endregion

        protected override void Deserialize(SerializationContext context)
        {
            context.Object.ACEs = ReadArray(context, "aces", context.Object.ACEs);
            context.Object.IsExact = ReadNullable(context.Dictionary, "isExact", context.Object.IsExact);
        }

        protected override void Serialize(SerializationContext context)
        {
            WriteArray(context, context.Object.ACEs, "aces");
            if (context.Object.IsExact.HasValue)
                context.Add("isExact", CommonFunctions.Convert(context.Object.IsExact.Value));
        }
    }
}