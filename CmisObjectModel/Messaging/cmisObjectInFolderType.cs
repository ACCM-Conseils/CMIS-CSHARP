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

namespace CmisObjectModel.Messaging
{
    public partial class cmisObjectInFolderType
    {

        public Core.cmisObjectType.cmisObjectTypeComparer get_Comparer(params string[] propertyNames)
        {
            return new Core.cmisObjectType.cmisObjectTypeComparer(Object, propertyNames);
        }

        /// <summary>
      /// Converts AtomEntry of object
      /// </summary>
      /// <param name="value"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public static implicit operator cmisObjectInFolderType(AtomPub.AtomEntry value)
        {
            var cmisObject = value is null ? null : value.Object;

            return cmisObject is null ? null : new cmisObjectInFolderType() { _object = cmisObject, _pathSegment = value.PathSegment };
        }

    }
}