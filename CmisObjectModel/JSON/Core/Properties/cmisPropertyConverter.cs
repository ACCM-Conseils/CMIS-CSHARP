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
namespace CmisObjectModel.Core.Properties
{
    [Attributes.JavaScriptConverter(typeof(JSON.Core.Properties.cmisPropertyConverter))]
    public partial class cmisProperty
    {
        public static Common.Generic.DynamicProperty<cmisProperty, string> DefaultKeyProperty = new Common.Generic.DynamicProperty<cmisProperty, string>(item => item._propertyDefinitionId, (item, value) => item.PropertyDefinitionId = value, "PropertyDefinitionId");
    }
}

namespace CmisObjectModel.JSON.Core.Properties
{
    /// <summary>
   /// Defaultconverter for a cmisProperty-instance
   /// </summary>
   /// <remarks></remarks>
    public class cmisPropertyConverter : PropertyConverter
    {

        #region Constructors
        public cmisPropertyConverter() : base()
        {
        }
        public cmisPropertyConverter(Serialization.Generic.ObjectResolver<CmisObjectModel.Core.Properties.cmisProperty> objectResolver) : base(objectResolver)
        {
        }
        #endregion

        protected override void Deserialize(SerializationContext context)
        {
            base.Deserialize(context);
            context.Object.Cardinality = ReadEnum(context.Dictionary, "cardinality", context.Object.Cardinality);
        }

        protected override void Serialize(SerializationContext context)
        {
            base.Serialize(context);
            context.Add("cardinality", context.Object.Cardinality.GetName());
        }

        protected override object SerializeValue(SerializationContext context)
        {
            if (context.Object is CmisObjectModel.Core.Properties.cmisPropertyDateTime)
            {
                CmisObjectModel.Core.Properties.cmisPropertyDateTime @object = (CmisObjectModel.Core.Properties.cmisPropertyDateTime)context.Object;

                if (@object.Values is null)
                {
                    return null;
                }
                else if (context.Object.Cardinality == CmisObjectModel.Core.enumCardinality.multi)
                {
                    return (from value in @object.Values
                            select value.DateTime.ToJSONTime()).ToArray();
                }
                else
                {
                    return @object.Value.DateTime.ToJSONTime();
                }
            }
            else if (context.Object.Cardinality == CmisObjectModel.Core.enumCardinality.multi)
            {
                return context.Object.Values;
            }
            else
            {
                return context.Object.Value;
            }
        }
    }
}