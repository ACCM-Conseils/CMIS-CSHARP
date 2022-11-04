
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
namespace CmisObjectModel.Messaging
{
    [Attributes.JavaScriptConverter(typeof(JSON.Messaging.cmisFaultTypeConverter))]
    public partial class cmisFaultType
    {
    }
}

namespace CmisObjectModel.JSON.Messaging
{
    /// <summary>
   /// Error handling
   /// </summary>
   /// <remarks>see chapter 5.2.10  Error Handling and Return Codes in cmis documentation</remarks>
    public class cmisFaultTypeConverter : Serialization.Generic.JavaScriptConverter<CmisObjectModel.Messaging.cmisFaultType>
    {

        #region Constructors
        public cmisFaultTypeConverter() : base(new Serialization.Generic.DefaultObjectResolver<CmisObjectModel.Messaging.cmisFaultType>())
        {
        }
        public cmisFaultTypeConverter(Serialization.Generic.ObjectResolver<CmisObjectModel.Messaging.cmisFaultType> objectResolver) : base(objectResolver)
        {
        }
        #endregion

        protected override void Deserialize(SerializationContext context)
        {
            context.Object.Type = ReadEnum(context.Dictionary, "exception", context.Object.Type);
            context.Object.Message = Read(context.Dictionary, "message", context.Object.Message);
            context.Object.Code = Read(context.Dictionary, "code", context.Object.Code);
            context.Object.Extensions = ReadArray(context, "extensions", context.Object.Extensions);
        }

        protected override void Serialize(SerializationContext context)
        {
            context.Add("exception", context.Object.Type.GetName());
            context.Add("message", context.Object.Message);
            context.Add("code", context.Object.Code);
            WriteArray(context, context.Object.Extensions, "extensions");
        }
    }
}