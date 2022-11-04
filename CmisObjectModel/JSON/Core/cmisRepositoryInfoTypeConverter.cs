
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
/* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.Core
{
    public partial class cmisRepositoryInfoType
    {
        public static Common.Generic.DynamicProperty<cmisRepositoryInfoType, string> DefaultKeyProperty = new Common.Generic.DynamicProperty<cmisRepositoryInfoType, string>(item => item._repositoryId, (item, value) => item.RepositoryId = value, "RepositoryId");

        #region Additional properties for browser-binding
        /// <summary>
      /// Same as property AclCapability; using the BrowserBinding the AclCapability-parameter is called AclCapabilities
      /// </summary>
        public Security.cmisACLCapabilityType AclCapabilities
        {
            get
            {
                return _aclCapability;
            }
            set
            {
                AclCapability = value;
            }
        }

        /// <summary>
      /// Same as property PrincipalAnonymous; using the BrowserBinding the PrincipalAnonymous-parameter is called PrincipalIdAnonymous
      /// </summary>
        public string PrincipalIdAnonymous
        {
            get
            {
                return _principalAnonymous;
            }
            set
            {
                PrincipalAnonymous = value;
            }
        }

        /// <summary>
      /// Same as property PrincipalAnyone; using the BrowserBinding the PrincipalAnyone-parameter is called PrincipalIdAnyone
      /// </summary>
        public string PrincipalIdAnyone
        {
            get
            {
                return _principalAnyone;
            }
            set
            {
                PrincipalAnyone = value;
            }
        }

        private string _repositoryUrl;
        public string RepositoryUrl
        {
            get
            {
                return _repositoryUrl;
            }
            set
            {
                if ((_repositoryUrl ?? "") != (value ?? ""))
                {
                    string oldValue = _repositoryUrl;
                    _repositoryUrl = value;
                    OnPropertyChanged("RepositoryUrl", value, oldValue);
                }
            }
        } // RepositoryUrl

        private string _rootFolderUrl;
        public string RootFolderUrl
        {
            get
            {
                return _rootFolderUrl;
            }
            set
            {
                if ((_rootFolderUrl ?? "") != (value ?? ""))
                {
                    string oldValue = _rootFolderUrl;
                    _rootFolderUrl = value;
                    OnPropertyChanged("RootFolderUrl", value, oldValue);
                }
            }
        } // RootFolderUrl
        #endregion
    }
}

namespace CmisObjectModel.JSON.Core
{
    public partial class cmisRepositoryInfoTypeConverter
    {
    }
}