using System.Data;
using System.Linq;
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
    /// <summary>
   /// Converter for cmisProperty-instances in query results
   /// </summary>
   /// <remarks></remarks>
    public class QueryPropertyConverter : PropertyConverter
    {

        #region Constructors
        public QueryPropertyConverter() : base()
        {
        }
        public QueryPropertyConverter(Serialization.Generic.ObjectResolver<CmisObjectModel.Core.Properties.cmisProperty> objectResolver) : base(objectResolver)
        {
        }
        #endregion

        protected override object SerializeValue(SerializationContext context)
        {
            if (context.Object.Values is null)
            {
                return null;
            }
            else if (context.Object is CmisObjectModel.Core.Properties.cmisPropertyDateTime)
            {
                CmisObjectModel.Core.Properties.cmisPropertyDateTime @object = (CmisObjectModel.Core.Properties.cmisPropertyDateTime)context.Object;

                if (@object.Values.Length == 1)
                {
                    return @object.Value.DateTime.ToJSONTime();
                }
                else
                {
                    return (from value in @object.Values
                            select value.DateTime.ToJSONTime()).ToArray();
                }
            }
            else if (context.Object.Values.Length == 1)
            {
                return context.Object.Value;
            }
            else
            {
                return context.Object.Values;
            }
        }

    }
}