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
using ss = System.ServiceModel;
using Microsoft.VisualBasic;

namespace CmisObjectModel.Common
{
    [HideModuleName()]
    public static class AppSettings
    {

        #region Constants
        public const string CacheLeaseTimeKey = "CmisObjectModel.Cache.LeaseTime";
        public const string CacheSizeObjectsKey = "CmisObjectModel.Cache.Objects.Size";
        public const string CacheSizeRepositoriesKey = "CmisObjectModel.Cache.Repositories.Size";
        public const string CacheSizeTypesKey = "CmisObjectModel.Cache.Types.Size";
        public const string ClientCredentialTypeKey = "CmisObjectModel.ClientCredentialType";
        public const string CompressionKey = "CmisObjectModel.Compression";
        public const string SupportDebugInformationKey = "CmisObjectModel.Debug";
        #endregion

        #region AppSettings
        private static double? _cacheLeaseTime = default;
        public static double CacheLeaseTime
        {
            get
            {
                if (!_cacheLeaseTime.HasValue)
                {
                    // Default: 10 minutes
                    _cacheLeaseTime = ReadValue(CacheLeaseTimeKey, 600.0d);
                }

                return _cacheLeaseTime.Value;
            }
        }

        private static int? _cacheSizeObjects = default;
        public static int CacheSizeObjects
        {
            get
            {
                if (!_cacheSizeObjects.HasValue)
                {
                    // Default: 500
                    _cacheSizeObjects = ReadValue(CacheSizeObjectsKey, 500);
                    if (_cacheSizeObjects.Value < 1)
                        _cacheSizeObjects = 500;
                }

                return _cacheSizeObjects.Value;
            }
        }

        private static int? _cacheSizeRepositories = default;
        public static int CacheSizeRepositories
        {
            get
            {
                if (!_cacheSizeRepositories.HasValue)
                {
                    // Default: 10
                    _cacheSizeRepositories = ReadValue(CacheSizeRepositoriesKey, 10);
                    if (_cacheSizeRepositories.Value < 1)
                        _cacheSizeRepositories = 10;
                }

                return _cacheSizeRepositories.Value;
            }
        }

        private static int? _cacheSizeTypes = default;
        public static int CacheSizeTypes
        {
            get
            {
                if (!_cacheSizeTypes.HasValue)
                {
                    // Default: 100
                    _cacheSizeTypes = ReadValue(CacheSizeTypesKey, 100);
                    if (_cacheSizeTypes.Value < 1)
                        _cacheSizeTypes = 100;
                }

                return _cacheSizeTypes.Value;
            }
        }

        private static ss.HttpClientCredentialType? _clientCredentialType = default;
        public static ss.HttpClientCredentialType ClientCredentialType
        {
            get
            {
                if (!_clientCredentialType.HasValue)
                {
                    _clientCredentialType = ReadEnum(ClientCredentialTypeKey, ss.HttpClientCredentialType.Basic);
                }

                return _clientCredentialType.Value;
            }
        }

        private static bool? _compression = default;
        public static bool Compression
        {
            get
            {
                if (!_compression.HasValue)
                {
                    // False is default!
                    _compression = ReadBoolean(CompressionKey, false);
                }

                return _compression.Value;
            }
        }

        private static bool? _supportsDebugInformation = default;
        /// <summary>
      /// Should the service return debug informations?
      /// </summary>
      /// <value></value>
      /// <returns></returns>
      /// <remarks></remarks>
        public static bool SupportsDebugInformation
        {
            get
            {
                if (!_supportsDebugInformation.HasValue)
                {
                    // True is default!
                    _supportsDebugInformation = ReadBoolean(SupportDebugInformationKey, true);
                }

                return _supportsDebugInformation.Value;
            }
        }
        #endregion

        #region Read methods via System.Configuration.ConfigurationManager
        /// <summary>
      /// Reads any Boolean from the AppSettings
      /// </summary>
      /// <param name="key"></param>
      /// <param name="defaultValue"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        private static bool ReadBoolean(string key, bool defaultValue)
        {
            string value = System.Configuration.ConfigurationManager.AppSettings[key];
            bool boolValue;
            if (defaultValue)
            {
                // True is default!
                return string.IsNullOrEmpty(value) || !bool.TryParse(value, out boolValue) || boolValue;
            }
            else
            {
                // False is default!
                return !string.IsNullOrEmpty(value) && bool.TryParse(value, out boolValue) && boolValue;
            }
        }

        /// <summary>
      /// Reads any Enum from the AppSettings
      /// </summary>
      /// <typeparam name="TEnum"></typeparam>
      /// <param name="key"></param>
      /// <param name="defaultValue"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        private static TEnum ReadEnum<TEnum>(string key, TEnum defaultValue) where TEnum : struct
        {
            string value = System.Configuration.ConfigurationManager.AppSettings[key];
            TEnum result;

            return !string.IsNullOrEmpty(value) && Enum.TryParse(value, true, out result) ? result : defaultValue;
        }

        /// <summary>
      /// Reads specified value-type from the AppSettings
      /// </summary>
      /// <typeparam name="TValue"></typeparam>
      /// <param name="key"></param>
      /// <param name="defaultValue"></param>
      /// <returns></returns>
      /// <remarks>Specified value-type MUST be defined in DefaultXmlConverter</remarks>
        private static TValue ReadValue<TValue>(string key, TValue defaultValue)
        {
            try
            {
                string value = System.Configuration.ConfigurationManager.AppSettings[key];

                return CommonFunctions.DefaultXmlConverter.ContainsKey(typeof(TValue)) ? ((Tuple<Func<TValue, string>, Func<string, TValue>>)CommonFunctions.DefaultXmlConverter[typeof(TValue)]).Item2(value) : defaultValue;
            }
            catch
            {
                return defaultValue;
            }
        }
        #endregion

    }
}