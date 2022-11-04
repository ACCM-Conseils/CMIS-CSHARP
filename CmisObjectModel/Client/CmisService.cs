using System.Collections.Generic;
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
using ss = System.ServiceModel;
/* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.Client
{
    /// <summary>
   /// Encapsulates Cmis-Service-Requests to discover the workspaces the client has access to
   /// </summary>
   /// <remarks></remarks>
    public class CmisService
    {

        public CmisService(Contracts.ICmisClient client)
        {
            _client = client;
        }

        protected Contracts.ICmisClient _client;
        public Contracts.ICmisClient Client
        {
            get
            {
                return _client;
            }
        }

        protected virtual CmisRepository CreateCmisRepository(Core.cmisRepositoryInfoType repositoryInfo)
        {
            return new CmisRepository(repositoryInfo, _client);
        }

        /// <summary>
      /// Returns all repositories
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public Dictionary<string, Messaging.cmisRepositoryEntryType> GetRepositories()
        {
            var retVal = new Dictionary<string, Messaging.cmisRepositoryEntryType>();
            var result = _client.GetRepositories(new Messaging.Requests.getRepositories());

            _lastException = result.Exception;
            if (_lastException is null && result.Response is not null)
            {
                foreach (Messaging.cmisRepositoryEntryType entry in result.Response.Repositories)
                {
                    if (entry is not null && !string.IsNullOrEmpty(entry.RepositoryId) && !retVal.ContainsKey(entry.RepositoryId))
                    {
                        retVal.Add(entry.RepositoryId, entry);
                    }
                }
            }

            return retVal;
        }

        /// <summary>
      /// Returns the workspace of specified repository or null
      /// </summary>
        public CmisRepository GetRepositoryInfo(string repositoryId)
        {
            var result = _client.GetRepositoryInfo(new Messaging.Requests.getRepositoryInfo() { RepositoryId = repositoryId }, true);

            _lastException = result.Exception;
            return _lastException is null && result.Response is not null ? CreateCmisRepository(result.Response.RepositoryInfo) : null;
        }

        protected ss.FaultException _lastException;
        public ss.FaultException LastException
        {
            get
            {
                return _lastException;
            }
        }

    }
}