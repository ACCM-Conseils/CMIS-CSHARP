using System.Data;
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
   /// Converter for Row-instances
   /// </summary>
   /// <remarks></remarks>
    public class RowConverter : Serialization.Generic.JavaScriptConverter<CmisObjectModel.Extensions.Data.Row>
    {

        #region Constructors
        public RowConverter() : base(new Serialization.Generic.DefaultObjectResolver<CmisObjectModel.Extensions.Data.Row>())
        {
        }
        public RowConverter(Serialization.Generic.ObjectResolver<CmisObjectModel.Extensions.Data.Row> objectResolver) : base(objectResolver)
        {
        }
        #endregion

        #region Helper-classes
        private sealed class RowWriter : CmisObjectModel.Extensions.Data.Row
        {

            private RowWriter() : base()
            {
            }

            public static void Write(CmisObjectModel.Extensions.Data.Row instance, CmisObjectModel.Core.Collections.cmisPropertiesType properties, CmisObjectModel.Core.Collections.cmisPropertiesType originalProperties, DataRowState rowState)
            {
                SilentInitialization(instance, properties, originalProperties, rowState);
            }
        }
        #endregion

        protected override void Deserialize(SerializationContext context)
        {
            var originalProperties = Read<CmisObjectModel.Core.Collections.cmisPropertiesType>(context, "originalProperties", null);
            var properties = Read<CmisObjectModel.Core.Collections.cmisPropertiesType>(context, "properties", null);
            var rowState = ReadEnum(context.Dictionary, "rowState", DataRowState.Detached);

            // serialization suppressed originalProperties for rows in Unchanged-state
            if (rowState == DataRowState.Unchanged && properties is not null)
            {
                originalProperties = (CmisObjectModel.Core.Collections.cmisPropertiesType)properties.Copy();
            }
            RowWriter.Write(context.Object, properties, originalProperties, rowState);
        }

        protected override void Serialize(SerializationContext context)
        {
            // serialization of originalProperties only for states which support differences between
            // properties and originalProperties
            if (context.Object.RowState == DataRowState.Deleted || context.Object.RowState == DataRowState.Modified)
            {
                Write(context, context.Object.GetOriginalProperties(), "originalProperties");
            }
            Write(context, context.Object.Properties, "properties");
            context.Add("rowState", context.Object.RowState.GetName());
        }

    }
}