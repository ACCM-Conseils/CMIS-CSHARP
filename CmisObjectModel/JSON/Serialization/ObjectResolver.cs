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
namespace CmisObjectModel.JSON.Serialization
{
    /// <summary>
   /// Baseclass of ObjectResolver-instances
   /// </summary>
   /// <remarks></remarks>
    public abstract class ObjectResolver
    {
        public object CreateInstance(IDictionary<string, object> dictionary)
        {
            return CreateInstanceCore(dictionary);
        }
        protected abstract object CreateInstanceCore(object source);
    }

    /// <summary>
   /// Objectresolver for Core.Properties.cmisProperty types
   /// </summary>
   /// <remarks></remarks>
    public class CmisPropertyResolver : Generic.ObjectResolver<CmisObjectModel.Core.Properties.cmisProperty>
    {

        public override CmisObjectModel.Core.Properties.cmisProperty CreateInstance(object source)
        {
            if (source is IDictionary<string, object>)
            {
                IDictionary<string, object> dictionary = (IDictionary<string, object>)source;
                string typeName = dictionary.ContainsKey("type") ? Conversions.ToString(dictionary["type"]) : "string";
                var type = default(CmisObjectModel.Core.enumPropertyType);

                switch (CommonFunctions.TryParse(typeName, ref type, true) ? type : CmisObjectModel.Core.enumPropertyType.@string)
                {
                    case CmisObjectModel.Core.enumPropertyType.@boolean:
                        {
                            return new CmisObjectModel.Core.Properties.cmisPropertyBoolean();
                        }
                    case CmisObjectModel.Core.enumPropertyType.@datetime:
                        {
                            return new CmisObjectModel.Core.Properties.cmisPropertyDateTime();
                        }
                    case CmisObjectModel.Core.enumPropertyType.@decimal:
                        {
                            if (CommonFunctions.DecimalRepresentation == enumDecimalRepresentation.@decimal)
                            {
                                return new CmisObjectModel.Core.Properties.cmisPropertyDecimal();
                            }
                            else
                            {
                                return new CmisObjectModel.Core.Properties.cmisPropertyDouble();
                            }
                        }
                    case CmisObjectModel.Core.enumPropertyType.html:
                        {
                            return new CmisObjectModel.Core.Properties.cmisPropertyHtml();
                        }
                    case CmisObjectModel.Core.enumPropertyType.id:
                        {
                            return new CmisObjectModel.Core.Properties.cmisPropertyId();
                        }
                    case CmisObjectModel.Core.enumPropertyType.integer:
                        {
                            return new CmisObjectModel.Core.Properties.cmisPropertyInteger();
                        }
                    case CmisObjectModel.Core.enumPropertyType.@string:
                        {
                            return new CmisObjectModel.Core.Properties.cmisPropertyString();
                        }
                    case CmisObjectModel.Core.enumPropertyType.uri:
                        {
                            return new CmisObjectModel.Core.Properties.cmisPropertyUri();
                        }

                    default:
                        {
                            return new CmisObjectModel.Core.Properties.cmisPropertyString();
                        }
                }
            }
            else if (source is null)
            {
                return new CmisObjectModel.Core.Properties.cmisPropertyObject();
            }
            else
            {
                return CmisObjectModel.Core.Properties.cmisProperty.FromType(source.GetType());
            }
        }
    }

    /// <summary>
   /// Objectresolver for Core.Definitions.Properties.cmisPropertyDefinitionType types
   /// </summary>
   /// <remarks></remarks>
    public class CmisPropertyDefinitionResolver : Generic.ObjectResolver<CmisObjectModel.Core.Definitions.Properties.cmisPropertyDefinitionType>
    {

        public override CmisObjectModel.Core.Definitions.Properties.cmisPropertyDefinitionType CreateInstance(object source)
        {
            if (source is IDictionary<string, object>)
            {
                IDictionary<string, object> dictionary = (IDictionary<string, object>)source;
                string typeName = dictionary.ContainsKey("propertyType") ? Conversions.ToString(dictionary["propertyType"]) : "string";
                var type = default(CmisObjectModel.Core.enumPropertyType);

                switch (CommonFunctions.TryParse(typeName, ref type, true) ? type : CmisObjectModel.Core.enumPropertyType.@string)
                {
                    case CmisObjectModel.Core.enumPropertyType.@boolean:
                        {
                            return new CmisObjectModel.Core.Definitions.Properties.cmisPropertyBooleanDefinitionType();
                        }
                    case CmisObjectModel.Core.enumPropertyType.datetime:
                        {
                            return new CmisObjectModel.Core.Definitions.Properties.cmisPropertyDateTimeDefinitionType();
                        }
                    case CmisObjectModel.Core.enumPropertyType.@decimal:
                        {
                            if (CommonFunctions.DecimalRepresentation == enumDecimalRepresentation.@decimal)
                            {
                                return new CmisObjectModel.Core.Definitions.Properties.cmisPropertyDecimalDefinitionType();
                            }
                            else
                            {
                                return new CmisObjectModel.Core.Definitions.Properties.cmisPropertyDoubleDefinitionType();
                            }
                        }
                    case CmisObjectModel.Core.enumPropertyType.html:
                        {
                            return new CmisObjectModel.Core.Definitions.Properties.cmisPropertyHtmlDefinitionType();
                        }
                    case CmisObjectModel.Core.enumPropertyType.id:
                        {
                            return new CmisObjectModel.Core.Definitions.Properties.cmisPropertyIdDefinitionType();
                        }
                    case CmisObjectModel.Core.enumPropertyType.integer:
                        {
                            return new CmisObjectModel.Core.Definitions.Properties.cmisPropertyIntegerDefinitionType();
                        }
                    case CmisObjectModel.Core.enumPropertyType.@string:
                        {
                            return new CmisObjectModel.Core.Definitions.Properties.cmisPropertyStringDefinitionType();
                        }
                    case CmisObjectModel.Core.enumPropertyType.uri:
                        {
                            return new CmisObjectModel.Core.Definitions.Properties.cmisPropertyUriDefinitionType();
                        }
                }
            }

            return null;
        }
    }

    /// <summary>
   /// Objectresolver for Core.Definitions.Types.cmisTypeDefinitionType types
   /// </summary>
   /// <remarks></remarks>
    public class CmisTypeDefinitionResolver : Generic.ObjectResolver<CmisObjectModel.Core.Definitions.Types.cmisTypeDefinitionType>
    {

        public override CmisObjectModel.Core.Definitions.Types.cmisTypeDefinitionType CreateInstance(object source)
        {
            if (source is IDictionary<string, object>)
            {
                IDictionary<string, object> dictionary = (IDictionary<string, object>)source;
                string baseIdName = dictionary.ContainsKey("baseId") ? Conversions.ToString(dictionary["baseId"]) : CmisObjectModel.Core.enumBaseObjectTypeIds.cmisDocument.GetName();
                var baseId = default(CmisObjectModel.Core.enumBaseObjectTypeIds);

                switch (CommonFunctions.TryParse(baseIdName, ref baseId, true) ? baseId : CmisObjectModel.Core.enumBaseObjectTypeIds.cmisDocument)
                {
                    case CmisObjectModel.Core.enumBaseObjectTypeIds.cmisDocument:
                        {
                            return new CmisObjectModel.Core.Definitions.Types.cmisTypeDocumentDefinitionType();
                        }
                    case CmisObjectModel.Core.enumBaseObjectTypeIds.cmisFolder:
                        {
                            return new CmisObjectModel.Core.Definitions.Types.cmisTypeFolderDefinitionType();
                        }
                    case CmisObjectModel.Core.enumBaseObjectTypeIds.cmisItem:
                        {
                            return new CmisObjectModel.Core.Definitions.Types.cmisTypeItemDefinitionType();
                        }
                    case CmisObjectModel.Core.enumBaseObjectTypeIds.cmisPolicy:
                        {
                            return new CmisObjectModel.Core.Definitions.Types.cmisTypePolicyDefinitionType();
                        }
                    case CmisObjectModel.Core.enumBaseObjectTypeIds.cmisRelationship:
                        {
                            return new CmisObjectModel.Core.Definitions.Types.cmisTypeRelationshipDefinitionType();
                        }
                    case CmisObjectModel.Core.enumBaseObjectTypeIds.cmisSecondary:
                        {
                            string id = dictionary.ContainsKey("id") ? Conversions.ToString(dictionary["id"]) : CmisObjectModel.Core.Definitions.Types.cmisTypeSecondaryDefinitionType.TargetTypeName;
                            switch (true)
                            {
                                case object _ when string.Compare(id, CmisObjectModel.Core.Definitions.Types.cmisTypeSecondaryDefinitionType.TargetTypeName, true) == 0:
                                    {
                                        return new CmisObjectModel.Core.Definitions.Types.cmisTypeSecondaryDefinitionType();
                                    }
                                case object _ when string.Compare(id, CmisObjectModel.Core.Definitions.Types.cmisTypeRM_ClientMgtRetentionDefinitionType.TargetTypeName, true) == 0:
                                    {
                                        return new CmisObjectModel.Core.Definitions.Types.cmisTypeRM_ClientMgtRetentionDefinitionType();
                                    }
                                case object _ when string.Compare(id, CmisObjectModel.Core.Definitions.Types.cmisTypeRM_DestructionRetentionDefinitionType.TargetTypeName, true) == 0:
                                    {
                                        return new CmisObjectModel.Core.Definitions.Types.cmisTypeRM_DestructionRetentionDefinitionType();
                                    }
                                case object _ when string.Compare(id, CmisObjectModel.Core.Definitions.Types.cmisTypeRM_HoldDefinitionType.TargetTypeName, true) == 0:
                                    {
                                        return new CmisObjectModel.Core.Definitions.Types.cmisTypeRM_HoldDefinitionType();
                                    }
                                case object _ when string.Compare(id, CmisObjectModel.Core.Definitions.Types.cmisTypeRM_RepMgtRetentionDefinitionType.TargetTypeName, true) == 0:
                                    {
                                        return new CmisObjectModel.Core.Definitions.Types.cmisTypeRM_RepMgtRetentionDefinitionType();
                                    }

                                default:
                                    {
                                        return new CmisObjectModel.Core.Definitions.Types.cmisTypeSecondaryDefinitionType();
                                    }
                            }

                            break;
                        }
                }
            }

            return null;
        }
    }

    /// <summary>
   /// ObjectResolver for Extensions.Extension types
   /// </summary>
   /// <remarks></remarks>
    public class ExtensionResolver<TExtension> : Generic.ObjectResolver<TExtension> where TExtension : CmisObjectModel.Extensions.Extension
    {

        public override TExtension CreateInstance(object source)
        {
            if (source is IDictionary<string, object>)
            {
                IDictionary<string, object> dictionary = (IDictionary<string, object>)source;
                // try quick 
                if (dictionary.ContainsKey("extensionTypeName"))
                {
                    return CmisObjectModel.Extensions.Extension.CreateInstance(Conversions.ToString(dictionary["extensionTypeName"])) as TExtension;
                }
                else if (dictionary.ContainsKey("ExtensionTypeName"))
                {
                    return CmisObjectModel.Extensions.Extension.CreateInstance(Conversions.ToString(dictionary["ExtensionTypeName"])) as TExtension;
                }
                else
                {
                    // try slow
                    foreach (KeyValuePair<string, object> de in dictionary)
                    {
                        if (string.Compare(de.Key, "extensionTypeName", true) == 0)
                        {
                            return CmisObjectModel.Extensions.Extension.CreateInstance(Conversions.ToString(de.Value)) as TExtension;
                        }
                    }
                }
            }
            return null;
        }
    }

    namespace Generic
    {
        /// <summary>
      /// Objects with default constructor
      /// </summary>
      /// <typeparam name="T"></typeparam>
      /// <remarks></remarks>
        public class DefaultObjectResolver<T> : ObjectResolver<T> where T : new()
        {

            public override T CreateInstance(object source)
            {
                return new T();
            }
        }

        /// <summary>
      /// Objects with default constructor of derived class
      /// </summary>
      /// <typeparam name="T"></typeparam>
      /// <remarks></remarks>
        public class DefaultObjectResolver<TBase, T> : ObjectResolver<TBase> where T : TBase, new()
        {

            public override TBase CreateInstance(object source)
            {
                return new T();
            }
        }

        public abstract class ObjectResolver<T> : ObjectResolver
        {

            public abstract new T CreateInstance(object source);
            protected sealed override object CreateInstanceCore(object source)
            {
                return CreateInstance(source);
            }
        }
    }
}