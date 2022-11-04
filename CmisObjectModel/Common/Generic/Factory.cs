﻿
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

namespace CmisObjectModel.Common.Generic
{
    /// <summary>
   /// Common factory
   /// </summary>
   /// <typeparam name="TFactoryResult"></typeparam>
   /// <remarks></remarks>
    public abstract class Factory<TFactoryResult>
    {
        public abstract TFactoryResult CreateInstance();
    }
    /// <summary>
   /// Common factory for all kinds of classes with default constructor
   /// </summary>
   /// <typeparam name="TBase"></typeparam>
   /// <typeparam name="TDerived"></typeparam>
   /// <remarks>
   /// Useful for dynamic creation of generic types or types derived from generic types
   /// (see CmisObjectModel.Definitions.DefinitionBase._factories)
   /// </remarks>
    public class Factory<TBase, TDerived> : Factory<TBase> where TDerived : TBase, new()
    {

        public override TBase CreateInstance()
        {
            return new TDerived();
        }
    }
}