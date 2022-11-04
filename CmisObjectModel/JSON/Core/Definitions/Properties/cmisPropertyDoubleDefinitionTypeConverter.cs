
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
using ccdp = CmisObjectModel.Core.Definitions.Properties;
/* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.Core.Definitions.Properties
{
    [Attributes.JavaScriptConverter(typeof(JSON.Core.Definitions.Properties.cmisPropertyDoubleDefinitionTypeConverter))]
    public partial class cmisPropertyDoubleDefinitionType
    {
    }
}

namespace CmisObjectModel.JSON.Core.Definitions.Properties
{
    public class cmisPropertyDoubleDefinitionTypeConverter : Generic.cmisPropertyDefinitionTypeConverter<double, CmisObjectModel.Core.Choices.cmisChoiceDouble, CmisObjectModel.Core.Properties.cmisPropertyDouble, ccdp.cmisPropertyDoubleDefinitionType>
    {

        #region Constructors
        public cmisPropertyDoubleDefinitionTypeConverter() : base(new Serialization.Generic.DefaultObjectResolver<ccdp.cmisPropertyDefinitionType, ccdp.cmisPropertyDoubleDefinitionType>())
        {
        }
        public cmisPropertyDoubleDefinitionTypeConverter(Serialization.Generic.ObjectResolver<ccdp.cmisPropertyDefinitionType> objectResolver) : base(objectResolver)
        {
        }
        #endregion

        protected override void Deserialize(SerializationContext context)
        {
            base.Deserialize(context);
            context.Object.MinValue = ReadNullable(context.Dictionary, "minValue", context.Object.MinValue);
            context.Object.MaxValue = ReadNullable(context.Dictionary, "maxValue", context.Object.MaxValue);
            context.Object.Precision = ReadOptionalEnum(context.Dictionary, "precision", context.Object.Precision);
        }

        protected override void Serialize(SerializationContext context)
        {
            base.Serialize(context);
            if (context.Object.MinValue.HasValue)
                context.Add("minValue", context.Object.MinValue.Value);
            if (context.Object.MaxValue.HasValue)
                context.Add("maxValue", context.Object.MaxValue.Value);
            if (context.Object.Precision.HasValue)
                context.Add("pecision", context.Object.Precision.Value.GetName());
        }
    }
}