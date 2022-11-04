

// depends on the chosen interpretation of the xs:integer expression in a xsd-file
/* TODO ERROR: Skipped IfDirectiveTrivia
#If xs_Integer = "Int32" OrElse xs_integer = "Integer" OrElse xs_integer = "Single" Then
*//* TODO ERROR: Skipped DisabledTextTrivia
Imports xs_Integer = System.Int32
*//* TODO ERROR: Skipped ElseDirectiveTrivia
#Else
*/
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
namespace CmisObjectModel.JSON.Extensions.Data
{
    /// <summary>
   /// Converter for ConverterDefinition-instances
   /// </summary>
   /// <remarks></remarks>
    public class ConverterDefinitionConverter : Serialization.Generic.JavaScriptConverter<CmisObjectModel.Extensions.Data.ConverterDefinition>
    {

        #region Constructors
        public ConverterDefinitionConverter() : base(new Serialization.Generic.DefaultObjectResolver<CmisObjectModel.Extensions.Data.ConverterDefinition>())
        {
        }
        public ConverterDefinitionConverter(Serialization.Generic.ObjectResolver<CmisObjectModel.Extensions.Data.ConverterDefinition> objectResolver) : base(objectResolver)
        {
        }
        #endregion

        protected override void Deserialize(SerializationContext context)
        {
            CmisObjectModel.Extensions.Data.ConverterDefinitionItem[] items = null;

            context.Object.ConverterIdentifier = Read(context.Dictionary, "converterIdentifier", context.Object.ConverterIdentifier);
            items = ReadArray(context, "items", items);
            if (items is not null)
            {
                foreach (CmisObjectModel.Extensions.Data.ConverterDefinitionItem item in items)
                    context.Object.Add(item);
            }
            context.Object.LocalType = ReadOptionalEnum(context.Dictionary, "localType", context.Object.LocalType);
            context.Object.NullValueMapping = Read(context.Dictionary, "nullValueMapping", context.Object.NullValueMapping);
            context.Object.PropertyDefinitionId = Read(context.Dictionary, "propertyDefinitionId", context.Object.PropertyDefinitionId);
            context.Object.RemoteType = ReadOptionalEnum(context.Dictionary, "remoteType", context.Object.RemoteType);
        }

        protected override void Serialize(SerializationContext context)
        {
            var items = context.Object.Items;

            context.Add("converterIdentifier", context.Object.ConverterIdentifier);
            context.Add("extensionTypeName", context.Object.ExtensionTypeName);
            if (items is not null && items.Length > 0)
                WriteArray(context, items, "items");
            if (context.Object.LocalType.HasValue)
                context.Add("localType", context.Object.LocalType.Value.GetName());
            context.Add("nullValueMapping", context.Object.NullValueMapping);
            context.Add("propertyDefinitionId", context.Object.PropertyDefinitionId);
            if (context.Object.RemoteType.HasValue)
                context.Add("remoteType", context.Object.RemoteType.Value.GetName());
        }

    }
}