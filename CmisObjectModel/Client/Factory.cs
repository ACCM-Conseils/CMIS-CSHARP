using System;
// *******************************************************************************************
// * Copyright Brügmann Software GmbH, Papenburg
// * Author: BSW_COL
// * Contact: codeplex<at>patorg.de
// * 
// * VB.CMIS is a VB.NET implementation of the Content Management Interoperability Services (CMIS) standard
// *
// * This file is part of VB.CMIS.
// * 
// * VB.CMIS is free software: you can redistribute it and/or modify
// * it under the terms of the GNU Lesser General Public License as published by
// * the Free Software Foundation, either version 3 of the License, or
// * (at your option) any later version.
// * 
// * VB.CMIS is distributed in the hope that it will be useful,
// * but WITHOUT ANY WARRANTY; without even the implied warranty of
// * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// * GNU Lesser General Public License for more details.
// * 
// * You should have received a copy of the GNU Lesser General Public License
// * along with VB.CMIS. If not, see <http://www.gnu.org/licenses/>.
// *******************************************************************************************
using CmisObjectModel.Common;

namespace CmisObjectModel.Client
{
    /// <summary>
   /// Methods to create CMIS-Clients for all Bindings
   /// </summary>
   /// <remarks></remarks>
    public static class Factory
    {

        /// <summary>
      /// Creates an instance of CMIS-CLient
      /// </summary>
      /// <param name="clientType"></param>
      /// <param name="serviceDocUri"></param>
      /// <param name="vendor"></param>
      /// <param name="authentication"></param>
      /// <param name="connectTimeout"></param>
      /// <param name="readWriteTimeout"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public static Contracts.ICmisClient CreateClient(enumClientType clientType, Uri serviceDocUri, enumVendor vendor, AuthenticationProvider authentication, int? connectTimeout = default, int? readWriteTimeout = default)
        {

            Contracts.ICmisClient retVal = null;

            switch (clientType)
            {
                case enumClientType.AtomPub:
                    {
                        retVal = new AtomPub.CmisClient(serviceDocUri, vendor, authentication, connectTimeout, readWriteTimeout);
                        break;
                    }
                case enumClientType.BrowserBinding:
                    {
                        retVal = new Browser.CmisClient(serviceDocUri, vendor, authentication, connectTimeout, readWriteTimeout);
                        break;
                    }
                    // Case enumClientType.WebService

            }

            return retVal;

        }

    }

}