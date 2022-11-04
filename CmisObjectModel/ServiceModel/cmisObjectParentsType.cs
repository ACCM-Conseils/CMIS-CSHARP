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
namespace CmisObjectModel.ServiceModel
{
    public class cmisObjectParentsType : Messaging.cmisObjectParentsType, Contracts.IServiceModelObject
    {

        public new cmisObjectType Object
        {
            get
            {
                return _object as cmisObjectType;
            }
            set
            {
                base.Object = value;
            }
        }

        #region IServiceModelObject
        private cmisObjectType IServiceModelObject_Object
        {
            get
            {
                return Object;
            }
        }

        cmisObjectType Contracts.IServiceModelObject.Object { get => IServiceModelObject_Object; }

        public string PathSegment
        {
            get
            {
                return null;
            }
        }

        private string IServiceModelObject_RelativePathSegment
        {
            get
            {
                return _relativePathSegment;
            }
        }

        string Contracts.IServiceModelObject.RelativePathSegment { get => IServiceModelObject_RelativePathSegment; }

        public cmisObjectType.ServiceModelExtension ServiceModel
        {
            get
            {
                var cmisObject = Object;
                return (cmisObject is null ? null : cmisObject.ServiceModel) ?? new cmisObjectType.ServiceModelExtension(cmisObject);
            }
        }
        #endregion

    }
}