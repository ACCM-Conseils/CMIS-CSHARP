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
   /// Defines non standard instance factories for union constructs
   /// </summary>
   /// <remarks></remarks>
    public class JavaScriptObjectResolverAttribute : Attribute
    {

        public JavaScriptObjectResolverAttribute(Type objectResolverType)
        {
            _objectResolverTypeDescriptor = DynamicTypeDescriptor.CreateInstance(objectResolverType);
        }

        /// <summary>
      /// To define a generic union factory type
      /// </summary>
      /// <param name="objectResolverType"></param>
      /// <param name="jsonFormattedGenericArgumentsMapping">Maps generic arguments of sourceType to
      /// generic arguments of the genericTargetType in JSON format
      /// i.e. {"TSourceTypeArgument3":"TTargetTypeArgument0","TSourceTypeArgument1":"TTargetTypeArgument1"}</param>
      /// <remarks></remarks>
        public JavaScriptObjectResolverAttribute(Type objectResolverType, string jsonFormattedGenericArgumentsMapping)
        {
            _objectResolverTypeDescriptor = DynamicTypeDescriptor.CreateInstance(objectResolverType, jsonFormattedGenericArgumentsMapping);
        }

        public Type get_ObjectResolverType(Type sourceType)
        {
            return _objectResolverTypeDescriptor.CreateType(sourceType);
        }

        private readonly DynamicTypeDescriptor _objectResolverTypeDescriptor;
    }
}