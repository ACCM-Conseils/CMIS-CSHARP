using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
using cccg = CmisObjectModel.Core.Choices.Generic;
using ccdp = CmisObjectModel.Core.Definitions.Properties;
using ccdpg = CmisObjectModel.Core.Definitions.Properties.Generic;
using ccpg = CmisObjectModel.Core.Properties.Generic;
using cjs = CmisObjectModel.JSON.Serialization;
using cjsg = CmisObjectModel.JSON.Serialization.Generic;
/* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.Core.Definitions.Properties
{
    public partial class cmisPropertyDefinitionType
    {
        public static readonly Common.Generic.DynamicProperty<cmisPropertyDefinitionType, string> DefaultKeyProperty = new Common.Generic.DynamicProperty<cmisPropertyDefinitionType, string>(item => item._id, (item, value) => item.Id = value, "Id");
    }
}

namespace CmisObjectModel.JSON.Core.Definitions.Properties
{
    /// <summary>
   /// cmisPropertyDefinitionType-converter
   /// </summary>
   /// <remarks></remarks>
    public sealed class cmisPropertyDefinitionTypeConverter : Generic.cmisPropertyDefinitionTypeConverter<ccdp.cmisPropertyDefinitionType>
    {

        #region Constructors
        public cmisPropertyDefinitionTypeConverter() : base()
        {
        }
        public cmisPropertyDefinitionTypeConverter(cjsg.ObjectResolver<ccdp.cmisPropertyDefinitionType> objectResolver) : base(objectResolver)
        {
        }
        #endregion

    }

    /// <summary>
   /// Contract to access non generic methods across generic cmisPropertyDefinitionTypeConverter-types
   /// </summary>
   /// <remarks></remarks>
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    internal interface IcmisPropertyDefinitionTypeConverter
    {
        void Deserialize(ccdp.cmisPropertyDefinitionType retVal, IDictionary<string, object> dictionary, cjs.JavaScriptSerializer serializer);
        IDictionary<string, object> Serialize(ccdp.cmisPropertyDefinitionType obj, cjs.JavaScriptSerializer serializer);
    }
    namespace Generic
    {
        /// <summary>
      /// Baseclass of all cmisPropertyDefintionType-converters
      /// </summary>
      /// <remarks></remarks>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public abstract class cmisPropertyDefinitionTypeConverter<TPropertyDefinition> : cjsg.JavaScriptConverter<TPropertyDefinition, cjsg.ObjectResolver<ccdp.cmisPropertyDefinitionType>>, IcmisPropertyDefinitionTypeConverter where TPropertyDefinition : ccdp.cmisPropertyDefinitionType
        {

            #region Constructors
            public cmisPropertyDefinitionTypeConverter() : base(new cjs.CmisPropertyDefinitionResolver())
            {
            }
            public cmisPropertyDefinitionTypeConverter(cjsg.ObjectResolver<ccdp.cmisPropertyDefinitionType> objectResolver) : base(objectResolver)
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
                            var converter = withBlock.GetJavaScriptConverter(retVal.GetType()) as IcmisPropertyDefinitionTypeConverter ?? this;
                            converter.Deserialize(retVal, dictionary, withBlock.Self());
                        }
                    }
                    return retVal;
                }
            }
            protected virtual void Deserialize(ccdp.cmisPropertyDefinitionType retVal, IDictionary<string, object> dictionary, cjs.JavaScriptSerializer serializer)
            {
                Deserialize(new SerializationContext((TPropertyDefinition)retVal, dictionary, serializer));
            }

            void IcmisPropertyDefinitionTypeConverter.Deserialize(ccdp.cmisPropertyDefinitionType retVal, IDictionary<string, object> dictionary, cjs.JavaScriptSerializer serializer) => Deserialize(retVal, dictionary, serializer);
            protected virtual void Deserialize(SerializationContext context)
            {
                context.Object.Id = Read(context.Dictionary, "id", context.Object.Id);
                context.Object.LocalName = Read(context.Dictionary, "localName", context.Object.LocalName);
                context.Object.LocalNamespace = Read(context.Dictionary, "localNamespace", context.Object.LocalNamespace);
                context.Object.DisplayName = Read(context.Dictionary, "displayName", context.Object.DisplayName);
                context.Object.QueryName = Read(context.Dictionary, "queryName", context.Object.QueryName);
                context.Object.Description = Read(context.Dictionary, "description", context.Object.Description);
                // propertyType is readOnly
                context.Object.Cardinality = ReadEnum(context.Dictionary, "cardinality", context.Object.Cardinality);
                context.Object.Updatability = ReadEnum(context.Dictionary, "updatability", context.Object.Updatability);
                context.Object.Inherited = ReadNullable(context.Dictionary, "inherited", context.Object.Inherited);
                context.Object.Required = Read(context.Dictionary, "required", context.Object.Required);
                context.Object.Queryable = Read(context.Dictionary, "queryable", context.Object.Queryable);
                context.Object.Orderable = Read(context.Dictionary, "orderable", context.Object.Orderable);
                context.Object.OpenChoice = ReadNullable(context.Dictionary, "openChoice", context.Object.OpenChoice);
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
                        var converter = withBlock.GetJavaScriptConverter(obj.GetType()) as IcmisPropertyDefinitionTypeConverter ?? this;
                        return converter.Serialize((ccdp.cmisPropertyDefinitionType)obj, withBlock.Self());
                    }
                }
            }
            protected virtual IDictionary<string, object> Serialize(ccdp.cmisPropertyDefinitionType obj, cjs.JavaScriptSerializer serializer)
            {
                var retVal = new Dictionary<string, object>();
                Serialize(new SerializationContext((TPropertyDefinition)obj, retVal, serializer));
                return retVal;
            }

            IDictionary<string, object> IcmisPropertyDefinitionTypeConverter.Serialize(ccdp.cmisPropertyDefinitionType obj, cjs.JavaScriptSerializer serializer) => Serialize(obj, serializer);
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
                context.Add("propertyType", context.Object.PropertyType.GetName());
                context.Add("cardinality", context.Object.Cardinality.GetName());
                context.Add("updatability", context.Object.Updatability.GetName());
                context.Add("inherited", context.Object.Inherited.HasValue && context.Object.Inherited.Value);
                context.Add("required", context.Object.Required);
                context.Add("queryable", context.Object.Queryable);
                context.Add("orderable", context.Object.Orderable);
                if (context.Object.OpenChoice.HasValue)
                    context.Add("openChoice", context.Object.OpenChoice.Value);
            }
        }

        /// <summary>
      /// Baseclass of all converters for types derived from cmisPropertyDefintionType
      /// </summary>
      /// <typeparam name="TProperty"></typeparam>
      /// <typeparam name="TChoice"></typeparam>
      /// <typeparam name="TDefaultValue"></typeparam>
      /// <typeparam name="TPropertyDefinition"></typeparam>
      /// <remarks></remarks>
        public class cmisPropertyDefinitionTypeConverter<TProperty, TChoice, TDefaultValue, TPropertyDefinition> : cmisPropertyDefinitionTypeConverter<TPropertyDefinition>
where TChoice : cccg.cmisChoice<TProperty, TChoice>, new()
where TDefaultValue : ccpg.cmisProperty<TProperty>, new()
where TPropertyDefinition : ccdpg.cmisPropertyDefinitionType<TProperty, TChoice, TDefaultValue>
        {

            #region Constructors
            public cmisPropertyDefinitionTypeConverter() : base()
            {
            }
            public cmisPropertyDefinitionTypeConverter(cjsg.ObjectResolver<ccdp.cmisPropertyDefinitionType> objectResolver) : base(objectResolver)
            {
            }
            #endregion

            protected override void Deserialize(SerializationContext context)
            {
                base.Deserialize(context);
                if (context.Dictionary.ContainsKey("defaultValue"))
                {
                    var defaultValue = context.Dictionary["defaultValue"];

                    context.Object.DefaultValue = (TDefaultValue)context.Object.CreateProperty();
                    if (defaultValue is ICollection)
                    {
                        context.Object.DefaultValue.Values = (from item in (ICollection<object>)defaultValue
                                                              select CommonFunctions.TryCastDynamic<TProperty>(item)).ToArray();
                    }
                    else
                    {
                        context.Object.DefaultValue.Value = defaultValue.TryCastDynamic<TProperty>();
                    }
                }
                context.Object.Choices = ReadArray(context, "choice", context.Object.Choices);
            }

            protected override void Serialize(SerializationContext context)
            {
                base.Serialize(context);
                if (context.Object.DefaultValue is not null)
                {
                    var values = context.Object.DefaultValue.Values;
                    if (values is not null)
                    {
                        if (values.Length == 1)
                        {
                            context.Add("defaultValue", values[0]);
                        }
                        else
                        {
                            context.Add("defaultValue", values);
                        }
                    }
                }
                if (context.Object.Choices is not null)
                    WriteArray(context, context.Object.Choices, "choice");
            }
        }
    }
}