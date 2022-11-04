using System;
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
   /// Defines the JavaScriptConverterType for a type
   /// </summary>
   /// <remarks></remarks>
    [AttributeUsage(AttributeTargets.Class)]
    public class JavaScriptConverterAttribute : Attribute
    {

        /// <summary>
      /// To define a converter type directly
      /// </summary>
      /// <param name="javaScriptConverterType"></param>
      /// <remarks></remarks>
        public JavaScriptConverterAttribute(Type javaScriptConverterType)
        {
            _javaScriptConverterType = DynamicTypeDescriptor.CreateInstance(javaScriptConverterType);
        }

        /// <summary>
      /// To define a generic converter type
      /// </summary>
      /// <param name="javaScriptConverterType"></param>
      /// <param name="jsonFormattedGenericArgumentsMapping">Maps generic arguments of sourceType to
      /// generic arguments of the genericTargetType in JSON format
      /// i.e. {"TSourceTypeArgument3":"TTargetTypeArgument0","TSourceTypeArgument1":"TTargetTypeArgument1"}</param>
      /// <remarks></remarks>
        public JavaScriptConverterAttribute(Type javaScriptConverterType, string jsonFormattedGenericArgumentsMapping)
        {
            _javaScriptConverterType = DynamicTypeDescriptor.CreateInstance(javaScriptConverterType, jsonFormattedGenericArgumentsMapping);
        }

        private DynamicTypeDescriptor _javaScriptConverterType;

        public Type get_JavaScriptConverterType(Type sourceType)
        {
            return _javaScriptConverterType.CreateType(sourceType);
        }
    }
}