using System;
using System.Collections.Generic;
using ss = System.ServiceModel;
using ssw = System.ServiceModel.Web;
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
using CmisObjectModel.Common;
using ccg = CmisObjectModel.Common.Generic;
using cm = CmisObjectModel.Messaging;
/* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.ServiceModel.Base
{
    public abstract class CmisService
    {

        #region Repository
        /// <summary>
      /// Returns all available repositories
      /// </summary>
        protected TResult GetRepositories<TResult>(Func<Core.cmisRepositoryInfoType[], TResult> fnSuccess)
        {
            var serviceImpl = CmisServiceImpl;
            // enable possibility to specify repositoryId through queryString
            string repositoryId = CommonFunctions.GetRequestParameter("repositoryId");

            if (string.IsNullOrEmpty(repositoryId))
            {
                ccg.Result<Core.cmisRepositoryInfoType[]> result;

                try
                {
                    result = serviceImpl.GetRepositories();
                    if (result.Failure is null)
                    {
                        return fnSuccess(result.Success ?? (new Core.cmisRepositoryInfoType[] { }));
                    }
                }
                catch (Exception ex)
                {
                    if (IsWebException(ex))
                    {
                        /* TODO ERROR: Skipped IfDirectiveTrivia
                        #If EnableExceptionLogging = "True" Then
                        */
                        serviceImpl.LogException(ex);
                        /* TODO ERROR: Skipped EndIfDirectiveTrivia
                        #End If
                        */
                        throw;
                    }
                    else
                    {
                        result = cm.cmisFaultType.CreateUnknownException(ex);
                    }
                }

                // failure
                throw LogException(result.Failure, serviceImpl);
            }
            else
            {
                return GetRepositoryInfo(repositoryId, fnSuccess);
            }
        }

        /// <summary>
      /// Returns the specified repository
      /// </summary>
        protected TResult GetRepositoryInfo<TResult>(string repositoryId, Func<Core.cmisRepositoryInfoType[], TResult> fnSuccess)
        {
            if (string.IsNullOrEmpty(repositoryId))
            {
                // if there is no repositoryId specified return all available repositories
                return GetRepositories(fnSuccess);
            }
            else
            {
                var serviceImpl = CmisServiceImpl;
                ccg.Result<Core.cmisRepositoryInfoType> result;
                Dictionary<string, string> queryParameters;

                try
                {
                    queryParameters = CommonFunctions.GetRequestParameters();
                    if (queryParameters.ContainsKey("logout"))
                    {
                        {
                            var withBlock = CmisServiceImpl.Logout(repositoryId);
                            if (withBlock.Failure is null)
                            {
                                ssw.WebOperationContext.Current.OutgoingResponse.StatusCode = withBlock.Success;
                                return fnSuccess(null);
                            }
                            else
                            {
                                result = withBlock.Failure;
                            }
                        }
                    }
                    else if (queryParameters.ContainsKey("ping"))
                    {
                        {
                            var withBlock1 = CmisServiceImpl.Ping(repositoryId);
                            if (withBlock1.Failure is null)
                            {
                                ssw.WebOperationContext.Current.OutgoingResponse.StatusCode = withBlock1.Success;
                                return fnSuccess(null);
                            }
                            else
                            {
                                result = withBlock1.Failure;
                            }
                        }
                    }
                    else
                    {
                        result = serviceImpl.GetRepositoryInfo(repositoryId);

                        if (result is null)
                        {
                            result = cm.cmisFaultType.CreateUnknownException();
                        }
                        else if (result.Failure is null)
                        {
                            var repositoryInfo = result.Success;
                            if (repositoryInfo is null)
                            {
                                return fnSuccess(new Core.cmisRepositoryInfoType[] { });
                            }
                            else
                            {
                                return fnSuccess(new Core.cmisRepositoryInfoType[] { repositoryInfo });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (IsWebException(ex))
                    {
                        /* TODO ERROR: Skipped IfDirectiveTrivia
                        #If EnableExceptionLogging = "True" Then
                        */
                        serviceImpl.LogException(ex);
                        /* TODO ERROR: Skipped EndIfDirectiveTrivia
                        #End If
                        */
                        throw;
                    }
                    else
                    {
                        result = cm.cmisFaultType.CreateUnknownException(ex);
                    }
                }

                // failure
                throw LogException(result.Failure, serviceImpl);
            }
        }
        #endregion

        /// <summary>
      /// BaseUri of the CMIS service
      /// </summary>
      /// <value></value>
      /// <returns></returns>
      /// <remarks></remarks>
        public Uri BaseUri
        {
            get
            {
                return ss.OperationContext.Current.EndpointDispatcher.EndpointAddress.Uri;
            }
        }

        private Generic.ServiceImplFactory<Contracts.ICmisServicesImpl> _cmisServiceImplFactory;
        /// <summary>
      /// Returns ICmisService-instance that implements the services declared in cmis
      /// </summary>
      /// <value></value>
      /// <returns></returns>
      /// <remarks></remarks>
        protected Contracts.ICmisServicesImpl CmisServiceImpl
        {
            get
            {
                if (_cmisServiceImplFactory is null)
                    _cmisServiceImplFactory = new Generic.ServiceImplFactory<Contracts.ICmisServicesImpl>(BaseUri);
                return _cmisServiceImplFactory.CmisServiceImpl;
            }
        }

        /// <summary>
      /// Logs an exception before it will be thrown
      /// </summary>
      /// <param name="ex"></param>
      /// <param name="serviceImpl"></param>
      /// <returns></returns>
      /// <remarks>Compiler constant EnableExceptionLogging must be set to 'True'</remarks>
        protected Exception LogException(Exception ex, Contracts.ICmisServicesImpl serviceImpl)
        {
            /* TODO ERROR: Skipped IfDirectiveTrivia
            #If EnableExceptionLogging = "True" Then
            */
            serviceImpl.LogException(ex);
            /* TODO ERROR: Skipped EndIfDirectiveTrivia
            #End If
            */
            if (ssw.WebOperationContext.Current.OutgoingResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                if (ex is ssw.WebFaultException<cm.cmisFaultType>)
                {
                    ssw.WebOperationContext.Current.OutgoingResponse.StatusCode = ((ssw.WebFaultException<cm.cmisFaultType>)ex).StatusCode;
                }
                else
                {
                    ssw.WebOperationContext.Current.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                }
            }
            return ex;
        }

        private static Type _genericWebFaultExceptionType = typeof(ssw.WebFaultException<string>).GetGenericTypeDefinition();
        /// <summary>
      /// Returns True if ex based on WebFaultException or WebFaultException(Of T)
      /// </summary>
      /// <param name="ex"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        protected static bool IsWebException(Exception ex)
        {
            return ex is ssw.WebFaultException || ex.GetType().BasedOnGenericTypeDefinition(_genericWebFaultExceptionType);
        }

        /// <summary>
      /// Returns True if parameterName exists in the key list of the queryparameters within the current request
      /// </summary>
      /// <param name="parameterName"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        protected bool QueryParameterExists(string parameterName)
        {
            var requestParams = ssw.WebOperationContext.Current is null ? null : ssw.WebOperationContext.Current.IncomingRequest.UriTemplateMatch.QueryParameters;
            if (requestParams is null)
            {
                return false;
            }
            else
            {
                foreach (string key in requestParams.AllKeys)
                {
                    if (string.Compare(key, parameterName, true) == 0)
                        return true;
                }
            }

            return false;
        }
    }
}