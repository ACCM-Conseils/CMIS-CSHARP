using System;
using System.Collections.Generic;
using ss = System.ServiceModel;
using ssd = System.ServiceModel.Description;
using sss = System.ServiceModel.Security;
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

namespace CmisObjectModel.ServiceModel.Base
{
    /// <summary>
   /// Opens and closes servicehosts for baseAdresses
   /// </summary>
   /// <remarks></remarks>
    public abstract class ServiceManager : IDisposable
    {

        #region Constants
        private const int ciMaxReceivedMessageSize = 52428800;
        #endregion

        #region IDisposable Support
        private bool _isDisposed;

        // IDisposable
        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    lock (_syncObject)
                    {
                        foreach (ss.ServiceHost host in _hosts.Values)
                        {
                            try
                            {
                                host.Close();
                            }
                            catch
                            {
                            }
                        }
                        _hosts.Clear();
                    }
                }
            }
            _isDisposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

        #region Helper classes
        /// <summary>
      /// Parameters to create a new servicehost
      /// </summary>
      /// <remarks></remarks>
        private class OpenHostParameter
        {

            public OpenHostParameter(Uri baseAddress)
            {
                AbsoluteUri = baseAddress.AbsoluteUri;
                BaseAddress = baseAddress;
                IsHttps = AbsoluteUri.StartsWith("https://", StringComparison.InvariantCultureIgnoreCase);
                SecurityMode = IsHttps ? ss.WebHttpSecurityMode.Transport : ss.WebHttpSecurityMode.TransportCredentialOnly;
            }

            public readonly string AbsoluteUri;
            public readonly Uri BaseAddress;
            public readonly bool IsHttps;
            public readonly ss.WebHttpSecurityMode SecurityMode;
            public object Result;
            /// <summary>
         /// Used for synchronization between calling thread and workerthread
         /// </summary>
         /// <remarks></remarks>
            public readonly System.Threading.EventWaitHandle WaitHandle = new System.Threading.EventWaitHandle(false, System.Threading.EventResetMode.ManualReset);
        }

        /// <summary>
      /// ContentTypeMapper to receive data-parameter as IO.Stream in WebMethods
      /// </summary>
      /// <remarks></remarks>
        private class WebContentTypeMapper : ss.Channels.WebContentTypeMapper
        {

            /// <summary>
         /// Returns always WebContentFormat.Raw
         /// </summary>
         /// <param name="contentType"></param>
         /// <returns></returns>
         /// <remarks></remarks>
            public override ss.Channels.WebContentFormat GetMessageFormatForContentType(string contentType)
            {
                return ss.Channels.WebContentFormat.Raw;
            }
        }
        #endregion

        private Dictionary<string, ss.ServiceHost> _hosts = new Dictionary<string, ss.ServiceHost>();
        private object _syncObject = new object();

        /// <summary>
      /// Terminates the service for specified baseAddresses
      /// </summary>
      /// <param name="baseAddresses"></param>
      /// <remarks></remarks>
        public void Close(params Uri[] baseAddresses)
        {
            if (baseAddresses is not null)
            {
                lock (_syncObject)
                {
                    foreach (Uri baseAddress in baseAddresses)
                    {
                        if (baseAddress is not null)
                        {
                            string absoluteUri = baseAddress.AbsoluteUri;

                            if (!string.IsNullOrEmpty(absoluteUri) && _hosts.ContainsKey(absoluteUri))
                            {
                                try
                                {
                                    _hosts[absoluteUri].Close();
                                }
                                catch
                                {
                                }
                                _hosts.Remove(absoluteUri);
                            }
                        }
                    }
                }
            }
        }

        protected abstract Type GetImplementedContractType();
        protected abstract Type GetServiceType();

        /// <summary>
      /// Starts selfhosted WCF services for given baseAddresses
      /// </summary>
      /// <param name="baseAddresses"></param>
      /// <remarks></remarks>
        public void Open(params Uri[] baseAddresses)
        {
            if (baseAddresses is not null)
            {
                try
                {
                    // warmup/selftest to determine if an ICmisServiceImpl-instance can be created
                    var factory = new Generic.ServiceImplFactory<Contracts.ICmisServicesImpl>(null);

                    lock (_syncObject)
                    {
                        foreach (Uri baseAddress in baseAddresses)
                        {
                            if (baseAddress is not null)
                            {
                                var @params = new OpenHostParameter(baseAddress);
                                var thread = new System.Threading.Thread(OpenHost);

                                // ensure that there is no host opened for this baseAddress
                                Close(baseAddress);
                                // open servicehost
                                thread.Start(@params);
                                // wait max. 20sec for servicehost-start
                                if (@params.WaitHandle.WaitOne(20000))
                                {
                                    if (@params.Result is ss.ServiceHost)
                                    {
                                        _hosts.Add(@params.AbsoluteUri, (ss.ServiceHost)@params.Result);
                                    }
                                    else if (@params.Result is Exception)
                                    {
                                        throw (Exception)@params.Result;
                                    }
                                }
                                else
                                {
                                    // don't wait any longer for servicehost
                                    System.Threading.ThreadPool.QueueUserWorkItem(state => thread.Abort());
                                }
                            }
                        }
                    }
                }
                catch
                {
                }
            }
        }

        /// <summary>
      /// Creates a new servicehost in a workerthread to avoid application crashing
      /// if the service for one baseAddress crashes
      /// </summary>
      /// <param name="state"></param>
      /// <remarks></remarks>
        private void OpenHost(object state)
        {
            OpenHostParameter param = (OpenHostParameter)state;

            try
            {
                var host = new ss.ServiceHost(GetServiceType(), param.BaseAddress);
                var binding = new ss.WebHttpBinding(param.SecurityMode)
                {
                    TransferMode = ss.TransferMode.Streamed,
                    AllowCookies = true,
                    MaxReceivedMessageSize = ciMaxReceivedMessageSize
                };
                if (SupportsClientCredentialType())
                    binding.Security.Transport.ClientCredentialType = AppSettings.ClientCredentialType;

                var endPoint = host.AddServiceEndpoint(GetImplementedContractType(), binding, "");
                endPoint.Behaviors.Add(new ssd.WebHttpBehavior() { HelpEnabled = true });
                binding.ContentTypeMapper = new WebContentTypeMapper();

                var debugBehavior = host.Description.Behaviors.Find<ssd.ServiceDebugBehavior>();
                if (debugBehavior is null)
                {
                    debugBehavior = new ssd.ServiceDebugBehavior();
                    host.Description.Behaviors.Add(debugBehavior);
                }
                debugBehavior.IncludeExceptionDetailInFaults = AppSettings.SupportsDebugInformation;
                if (param.IsHttps)
                {
                    debugBehavior.HttpHelpPageEnabled = false;
                    debugBehavior.HttpsHelpPageUrl = param.BaseAddress.Combine(Constants.ServiceURIs.DebugHelpPageUri);
                }
                else
                {
                    debugBehavior.HttpsHelpPageEnabled = false;
                    debugBehavior.HttpHelpPageUrl = param.BaseAddress.Combine(Constants.ServiceURIs.DebugHelpPageUri);
                }

                var metaDataBehaviour = host.Description.Behaviors.Find<ssd.ServiceMetadataBehavior>();
                if (metaDataBehaviour is not null)
                {
                    if (param.IsHttps)
                    {
                        metaDataBehaviour.HttpGetEnabled = false;
                        metaDataBehaviour.HttpsGetUrl = param.BaseAddress.Combine(Constants.ServiceURIs.MetaDataUri);
                    }
                    else
                    {
                        metaDataBehaviour.HttpsGetEnabled = false;
                        metaDataBehaviour.HttpGetUrl = param.BaseAddress.Combine(Constants.ServiceURIs.MetaDataUri);
                    }
                }

                host.Credentials.UserNameAuthentication.CustomUserNamePasswordValidator = new UserNamePasswordValidator(param.BaseAddress);
                host.Credentials.UserNameAuthentication.UserNamePasswordValidationMode = sss.UserNamePasswordValidationMode.Custom;
                host.Open();
                param.Result = host;
                param.WaitHandle.Set();
            }
            catch (Exception ex)
            {
                param.Result = ex;
            }
        }

        protected abstract bool SupportsClientCredentialType();


    }
}