using System.Collections;
using System.Collections.Generic;
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
using Microsoft.VisualBasic.CompilerServices;
/* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.JSON.Extensions.Alfresco
{
    /// <summary>
   /// Converter for SetAspects-instances
   /// </summary>
   /// <remarks></remarks>
    public class SetAspectsConverter : Serialization.Generic.JavaScriptConverter<CmisObjectModel.Extensions.Alfresco.SetAspects>
    {

        #region Constructors
        public SetAspectsConverter() : base(new Serialization.Generic.DefaultObjectResolver<CmisObjectModel.Extensions.Alfresco.SetAspects>())
        {
        }
        public SetAspectsConverter(Serialization.Generic.ObjectResolver<CmisObjectModel.Extensions.Alfresco.SetAspects> objectResolver) : base(objectResolver)
        {
        }
        #endregion

        protected override void Deserialize(SerializationContext context)
        {
            var setAspects = context.Dictionary.ContainsKey("aspects") ? context.Dictionary["aspects"] as IEnumerable : null;

            if (setAspects is not null)
            {
                var propertiesType = typeof(CmisObjectModel.Core.Collections.cmisPropertiesType);
                var propertiesConverter = context.Serializer.GetJavaScriptConverter(propertiesType);
                var aspects = new List<CmisObjectModel.Extensions.Alfresco.SetAspects.Aspect>();

                foreach (object rawAspect in setAspects)
                {
                    IDictionary<string, object> aspect = rawAspect as IDictionary<string, object>;

                    if (aspect is not null)
                    {
                        var action = ReadEnum(aspect, "action", CmisObjectModel.Extensions.Alfresco.SetAspects.enumSetAspectsAction.aspectsToAdd);
                        string aspectName = aspect.ContainsKey("aspectName") ? Conversions.ToString(aspect.ContainsKey("aspectName")) : null;
                        var propertyCollection = aspect.ContainsKey("properties") ? aspect["properties"] as IDictionary<string, object> : null;
                        var properties = propertyCollection is null ? null : propertiesConverter.Deserialize(propertyCollection, propertiesType, context.Serializer) as CmisObjectModel.Core.Collections.cmisPropertiesType;
                        aspects.Add(new CmisObjectModel.Extensions.Alfresco.SetAspects.Aspect(action, aspectName, properties));
                    }
                }

                context.Object.Aspects = aspects.ToArray();
            }
        }

        protected override void Serialize(SerializationContext context)
        {
            if (context.Object.Aspects is not null)
            {
                var propertiesType = typeof(CmisObjectModel.Core.Collections.cmisPropertiesType);
                var propertiesConverter = context.Serializer.GetJavaScriptConverter(propertiesType);
                var aspects = new List<IDictionary<string, object>>();

                foreach (CmisObjectModel.Extensions.Alfresco.SetAspects.Aspect setAspect in context.Object.Aspects)
                {
                    if (setAspect is not null)
                    {
                        var aspect = new Dictionary<string, object>();

                        aspect.Add("action", setAspect.Action.GetName());
                        aspect.Add("aspectName", setAspect.AspectName);
                        if (setAspect.Properties is not null)
                        {
                            aspect.Add("properties", propertiesConverter.Serialize(setAspect.Properties, context.Serializer));
                        }
                        aspects.Add(aspect);
                    }
                }
                context.Add("aspects", aspects.ToArray());
            }
            context.Add("extensionTypeName", context.Object.ExtensionTypeName);
        }

    }
}