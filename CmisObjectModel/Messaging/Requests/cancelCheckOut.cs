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

namespace CmisObjectModel.Messaging.Requests
{
    public partial class cancelCheckOut
    {

        protected bool _pwcRequired = true;
        /// <summary>
      /// Set this property to True to make sure, that CancelCheckOut is supported for private working copies only. The default is True.
      /// </summary>
      /// <value></value>
      /// <returns></returns>
      /// <remarks></remarks>
        public bool PWCLinkRequired
        {
            get
            {
                return _pwcRequired;
            }
            set
            {
                if (value != _pwcRequired)
                {
                    _pwcRequired = value;
                    OnPropertyChanged("PWCLinkRequired", value, !value);
                }
            }
        } // PWCLinkRequired

    }
}