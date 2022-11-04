using System;
using System.Collections.Generic;
using swss = System.Web.Script.Serialization;
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
using ccdt = CmisObjectModel.Core.Definitions.Types;
using cjs = CmisObjectModel.JSON.Serialization;
using cjsg = CmisObjectModel.JSON.Serialization.Generic;
/* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.Core.Definitions.Types
{
    public partial class cmisTypeDefinitionType
    {
        public Common.Generic.DynamicProperty<ccdp.cmisPropertyDefinitionType[]> DefaultArrayProperty
        {
            get
            {
                return new Common.Generic.DynamicProperty<ccdp.cmisPropertyDefinitionType[]>(() => _propertyDefinitions, value => PropertyDefinitions = value, "PropertyDefinitions");

            }
        }
    }

    [Attributes.JavaScriptConverter(typeof(JSON.Core.Definitions.Types.cmisTypeDocumentDefinitionTypeConverter))]
    public partial class cmisTypeDocumentDefinitionType
    {
    }


    [Attributes.JavaScriptConverter(typeof(JSON.Core.Definitions.Types.cmisTypeRelationshipDefinitionTypeConverter))]
    public partial class cmisTypeRelationshipDefinitionType
    {
    }
}

namespace CmisObjectModel.JSON.Core.Definitions.Types
{
    /// <summary>
   /// Converter for cmisTypeDocumentDefinitionType
   /// </summary>
   /// <remarks></remarks>
    public class cmisTypeDocumentDefinitionTypeConverter : Generic.cmisTypeDefinitionTypeConverter<ccdt.cmisTypeDocumentDefinitionType>
    {

        #region Constructors
        public cmisTypeDocumentDefinitionTypeConverter() : base()
        {
        }
        public cmisTypeDocumentDefinitionTypeConverter(cjsg.ObjectResolver<ccdt.cmisTypeDefinitionType> objectObserver) : base(objectObserver)
        {
        }
        #endregion

        protected override void Deserialize(SerializationContext context)
        {
            base.Deserialize(context);
            context.Object.Versionable = Read(context.Dictionary, "versionable", context.Object.Versionable);
            context.Object.ContentStreamAllowed = ReadEnum(context.Dictionary, "contentStreamAllowed", context.Object.ContentStreamAllowed);
        }

        protected override void Serialize(SerializationContext context)
        {
            base.Serialize(context);
            context.Add("versionable", context.Object.Versionable);
            context.Add("contentStreamAllowed", context.Object.ContentStreamAllowed.GetName());
        }
    }

    public class cmisTypeRelationshipDefinitionTypeConverter : Generic.cmisTypeDefinitionTypeConverter<ccdt.cmisTypeRelationshipDefinitionType>
    {

        #region Constructors
        public cmisTypeRelationshipDefinitionTypeConverter() : base()
        {
        }
        public cmisTypeRelationshipDefinitionTypeConverter(cjsg.ObjectResolver<ccdt.cmisTypeDefinitionType> objectObserver) : base(objectObserver)
        {
        }
        #endregion

        protected override void Deserialize(SerializationContext context)
        {
            base.Deserialize(context);
            context.Object.AllowedSourceTypes = ReadArray(context.Dictionary, "allowedSourceTypes", context.Object.AllowedSourceTypes);
            context.Object.AllowedTargetTypes = ReadArray(context.Dictionary, "allowedTargetTypes", context.Object.AllowedTargetTypes);
        }

        protected override void Serialize(SerializationContext context)
        {
            base.Serialize(context);
            var emptyArray = new string[] { };
            WriteArray(context, context.Object.AllowedSourceTypes ?? emptyArray, "allowedSourceTypes");
            WriteArray(context, context.Object.AllowedTargetTypes ?? emptyArray, "allowedTargetTypes");
        }
    }

    /// <summary>
   /// Contract to access non generic methods across generic cmisTypeDefinitionTypeConverter-types
   /// </summary>
   /// <remarks></remarks>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    internal interface IcmisTypeDefinitionTypeConverter
    {
        void Deserialize(ccdt.cmisTypeDefinitionType retVal, IDictionary<string, object> dictionary, cjs.JavaScriptSerializer serializer);
        IDictionary<string, object> Serialize(ccdt.cmisTypeDefinitionType obj, cjs.JavaScriptSerializer serializer);
    }
    namespace Generic
    {
        /// <summary>
      /// Base class of all cmisTypeDefinitionType converter-types
      /// </summary>
      /// <typeparam name="TTypeDefinition"></typeparam>
      /// <remarks></remarks>
        public class cmisTypeDefinitionTypeConverter<TTypeDefinition> : cjsg.JavaScriptConverter<TTypeDefinition, cjsg.ObjectResolver<ccdt.cmisTypeDefinitionType>>, IcmisTypeDefinitionTypeConverter where TTypeDefinition : ccdt.cmisTypeDefinitionType
        {

            #region Constructors
            public cmisTypeDefinitionTypeConverter() : base(new cjs.CmisTypeDefinitionResolver())
            {
            }
            public cmisTypeDefinitionTypeConverter(cjsg.ObjectResolver<ccdt.cmisTypeDefinitionType> objectResolver) : base(objectResolver)
            {
            }
            #endregion

            /// <summary>
         /// Converts dictionary to a XmlSerializable-instance of type SupportedType
         /// </summary>
         /// <param name="serializer">MUST be a CmisObjectModelLibrary.JSON.JavaScriptSerializer</param>
         /// <param name="type">ignored, should be nothing</param>
            public sealed override object Deserialize(IDictionary<string, object> dictionary, Type type, swss.JavaScriptSerializer serializer)
            {
                if (dictionary is null)
                {
                    return null;
                }
                else
                {
                    var retVal = _objectResolver.CreateInstance(dictionary);
                    if (retVal is not null)
                    {
                        {
                            var withBlock = (cjs.JavaScriptSerializer)serializer;
                            var converter = withBlock.GetJavaScriptConverter(retVal.GetType()) as IcmisTypeDefinitionTypeConverter ?? this;
                            converter.Deserialize(retVal, dictionary, withBlock.Self());
                        }
                    }
                    return retVal;
                }
            }
            protected virtual void Deserialize(ccdt.cmisTypeDefinitionType retVal, IDictionary<string, object> dictionary, cjs.JavaScriptSerializer serializer)
            {
                Deserialize(new SerializationContext((TTypeDefinition)retVal, dictionary, serializer));
            }

            void IcmisTypeDefinitionTypeConverter.Deserialize(ccdt.cmisTypeDefinitionType retVal, IDictionary<string, object> dictionary, cjs.JavaScriptSerializer serializer) => Deserialize(retVal, dictionary, serializer);
            protected virtual void Deserialize(SerializationContext context)
            {
                context.Object.Id = Read(context.Dictionary, "id", context.Object.Id);
                context.Object.LocalName = Read(context.Dictionary, "localName", context.Object.LocalName);
                context.Object.DisplayName = Read(context.Dictionary, "displayName", context.Object.DisplayName);
                context.Object.QueryName = Read(context.Dictionary, "queryName", context.Object.QueryName);
                context.Object.Description = Read(context.Dictionary, "description", context.Object.Description);
                // baseId is readonly
                context.Object.ParentId = Read(context.Dictionary, "parentId", context.Object.ParentId);
                context.Object.Creatable = Read(context.Dictionary, "creatable", context.Object.Creatable);
                context.Object.Fileable = Read(context.Dictionary, "fileable", context.Object.Fileable);
                context.Object.Queryable = Read(context.Dictionary, "queryable", context.Object.Queryable);
                context.Object.FulltextIndexed = Read(context.Dictionary, "fulltextIndexed", context.Object.FulltextIndexed);
                context.Object.IncludedInSupertypeQuery = Read(context.Dictionary, "includedInSupertypeQuery", context.Object.IncludedInSupertypeQuery);
                context.Object.ControllablePolicy = Read(context.Dictionary, "controllablePolicy", context.Object.ControllablePolicy);
                context.Object.ControllableACL = Read(context.Dictionary, "controllableACL", context.Object.ControllableACL);
                context.Object.TypeMutability = Read(context, "typeMutability", context.Object.TypeMutability);
                if (context.Dictionary.ContainsKey("propertyDefinitions"))
                {
                    var mapper = new CmisObjectModel.Collections.Generic.ArrayMapper<ccdt.cmisTypeDefinitionType, ccdp.cmisPropertyDefinitionType, string>(context.Object, context.Object.DefaultArrayProperty, ccdp.cmisPropertyDefinitionType.DefaultKeyProperty);
                    mapper.JavaImport(context.Dictionary["propertyDefinitions"], context.Serializer);
                }
                context.Object.Extensions = ReadArray(context, "extensions", context.Object.Extensions);
            }

            /// <summary>
         /// Converts XmlSerializable-instance to IDictionary(Of String, Object)
         /// </summary>
         /// <param name="serializer">MUST be a CmisObjectModelLibrary.JSON.JavaScriptSerializer</param>
            public sealed override IDictionary<string, object> Serialize(object obj, swss.JavaScriptSerializer serializer)
            {
                if (obj is null)
                {
                    return null;
                }
                else
                {
                    {
                        var withBlock = (cjs.JavaScriptSerializer)serializer;
                        var converter = withBlock.GetJavaScriptConverter(obj.GetType()) as IcmisTypeDefinitionTypeConverter ?? this;
                        return converter.Serialize((ccdt.cmisTypeDefinitionType)obj, withBlock.Self());
                    }
                }
            }
            protected virtual IDictionary<string, object> Serialize(ccdt.cmisTypeDefinitionType obj, cjs.JavaScriptSerializer serializer)
            {
                var retVal = new Dictionary<string, object>();
                Serialize(new SerializationContext((TTypeDefinition)obj, retVal, serializer));
                return retVal;
            }

            IDictionary<string, object> IcmisTypeDefinitionTypeConverter.Serialize(ccdt.cmisTypeDefinitionType obj, cjs.JavaScriptSerializer serializer) => Serialize(obj, serializer);
            protected virtual void Serialize(SerializationContext context)
            {
                context.Add("id", context.Object.Id);
                context.Add("localName", context.Object.LocalName);
                if (!string.IsNullOrEmpty(context.Object.LocalNamespace))
                    context.Add("localNamespace", context.Object.LocalNamespace);
                if (!string.IsNullOrEmpty(context.Object.DisplayName))
                    context.Add("displayName", context.Object.DisplayName);
                if (!string.IsNullOrEmpty(context.Object.QueryName))
                    context.Add("queryName", context.Object.QueryName);
                if (!string.IsNullOrEmpty(context.Object.Description))
                    context.Add("description", context.Object.Description);
                context.Add("baseId", context.Object.BaseId.GetName());
                if (!string.IsNullOrEmpty(context.Object.ParentId))
                    context.Add("parentId", context.Object.ParentId);
                context.Add("creatable", context.Object.Creatable);
                context.Add("fileable", context.Object.Fileable);
                context.Add("queryable", context.Object.Queryable);
                context.Add("fulltextIndexed", context.Object.FulltextIndexed);
                context.Add("includedInSupertypeQuery", context.Object.IncludedInSupertypeQuery);
                context.Add("controllablePolicy", context.Object.ControllablePolicy);
                context.Add("controllableACL", context.Object.ControllableACL);
                if (context.Object.TypeMutability is not null)
                    Write(context, context.Object.TypeMutability, "typeMutability");
                var mapper = new CmisObjectModel.Collections.Generic.ArrayMapper<ccdt.cmisTypeDefinitionType, ccdp.cmisPropertyDefinitionType, string>(context.Object, context.Object.DefaultArrayProperty, ccdp.cmisPropertyDefinitionType.DefaultKeyProperty);
                context.Add("propertyDefinitions", mapper.JavaExport(null, context.Serializer));
                if (context.Object.Extensions is not null)
                    WriteArray(context, context.Object.Extensions, "extensions");
            }
        }
    }
}