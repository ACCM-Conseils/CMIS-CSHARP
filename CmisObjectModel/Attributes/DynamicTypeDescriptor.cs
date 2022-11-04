using System;
using System.Collections.Generic;
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

namespace CmisObjectModel.Attributes
{
    /// <summary>
   /// Baseclass to generate type that depends on sourceType (that is the type the current attribute is designed for)
   /// </summary>
   /// <remarks></remarks>
    public abstract class DynamicTypeDescriptor
    {

        #region Constructors
        /// <summary>
      /// Overrides only within this class
      /// </summary>
      /// <remarks></remarks>
        private DynamicTypeDescriptor()
        {
        }

        public static DynamicTypeDescriptor CreateInstance(Type staticType)
        {
            return new StaticTypeDescriptor(staticType);
        }

        /// <summary>
      /// To define a generic type
      /// </summary>
      /// <param name="genericTargetType"></param>
      /// <param name="jsonFormattedGenericArgumentsMapping">Maps generic arguments of sourceType to
      /// generic arguments of the genericTargetType in JSON format
      /// i.e. {"TSourceTypeArgument3":"TTargetTypeArgument0","TSourceTypeArgument1":"TTargetTypeArgument1"}</param>
      /// <remarks></remarks>
        public static DynamicTypeDescriptor CreateInstance(Type genericTargetType, string jsonFormattedGenericArgumentsMapping)
        {
            if (genericTargetType.IsGenericType)
            {
                return new GenericTypeDescriptor(genericTargetType, jsonFormattedGenericArgumentsMapping);
            }
            else
            {
                return new StaticTypeDescriptor(genericTargetType);
            }
        }
        #endregion

        #region Helper classes
        /// <summary>
      /// Parameters to create a generic type
      /// </summary>
      /// <remarks></remarks>
        protected class GenericTypeDescriptor : DynamicTypeDescriptor
        {

            /// <summary>
         /// To define a generic type
         /// </summary>
         /// <param name="genericTargetType"></param>
         /// <param name="jsonFormattedGenericArgumentsMapping">Maps generic arguments of sourceType to
         /// generic arguments of the genericTargetType in JSON format
         /// i.e. {"TSourceTypeArgument3":"TTargetTypeArgument0","TSourceTypeArgument1":"TTargetTypeArgument1"}</param>
         /// <remarks></remarks>
            public GenericTypeDescriptor(Type genericTargetType, string jsonFormattedGenericArgumentsMapping)
            {
                Dictionary<string, Type> genericTypePositions;
                var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                var genericArgumentsMapping = serializer.Deserialize<Dictionary<string, string>>(jsonFormattedGenericArgumentsMapping);

                _genericTypeDefinition = genericTargetType.GetGenericTypeDefinition();
                _genericArgumentsTemplate = genericTargetType.GetGenericArguments();
                _genericArgumentsLength = _genericArgumentsTemplate.Length;
                genericTypePositions = _genericTypeDefinition.GetGenericArguments().ToDictionary(currentType => currentType.Name);
                _genericArgumentsMapping = (from de in genericArgumentsMapping
                                            where !string.IsNullOrEmpty(de.Value) && genericTypePositions.ContainsKey(de.Value)
                                            let position = genericTypePositions[de.Value].GenericParameterPosition
                                            select new KeyValuePair<string, int>(de.Key, position)).ToDictionary(current => current.Key, current => current.Value);
            }

            /// <summary>
         /// Creates the generic type for sourceType (that is the type the current attribute is designed for)
         /// </summary>
         /// <param name="sourceType"></param>
         /// <returns></returns>
         /// <remarks></remarks>
            public override Type CreateType(Type sourceType)
            {
                Type[] typeArguments = (Type[])Array.CreateInstance(typeof(Type), _genericArgumentsLength);
                var processedArgumentNames = new HashSet<string>();

                // initialize with given template
                Array.Copy(_genericArgumentsTemplate, typeArguments, _genericArgumentsLength);
                // replace genericTypeArguments using _genericArgumentsMapping
                if (_genericArgumentsMapping.ContainsKey(string.Empty))
                {
                    typeArguments[_genericArgumentsMapping[string.Empty]] = sourceType;
                }
                while (!ReferenceEquals(sourceType, typeof(object)))
                {
                    if (sourceType.IsGenericType)
                    {
                        var genericSourceTypeArguments = sourceType.GetGenericArguments();

                        foreach (Type genericArgument in sourceType.GetGenericTypeDefinition().GetGenericArguments())
                        {
                            string argumentName = genericArgument.Name;

                            if (_genericArgumentsMapping.ContainsKey(argumentName) && processedArgumentNames.Add(argumentName))
                            {
                                typeArguments[_genericArgumentsMapping[argumentName]] = genericSourceTypeArguments[genericArgument.GenericParameterPosition];
                            }
                        }
                    }
                    sourceType = sourceType.BaseType;
                }

                return _genericTypeDefinition.MakeGenericType(typeArguments);
            }

            private readonly int _genericArgumentsLength;
            private readonly Dictionary<string, int> _genericArgumentsMapping;
            private readonly Type[] _genericArgumentsTemplate;
            private readonly Type _genericTypeDefinition;
        }

        /// <summary>
      /// Variant for static type to allow mixed usage of generic and non generic type definitions
      /// </summary>
      /// <remarks></remarks>
        protected class StaticTypeDescriptor : DynamicTypeDescriptor
        {

            public StaticTypeDescriptor(Type staticType)
            {
                _staticType = staticType;
            }

            public override Type CreateType(Type sourceType)
            {
                return _staticType;
            }

            private readonly Type _staticType;
        }
        #endregion

        public abstract Type CreateType(Type sourceType);
    }
}