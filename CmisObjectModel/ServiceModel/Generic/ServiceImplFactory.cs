using System;
using System.Data;
using System.Linq;
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
using sr = System.Reflection;
using sre = System.Reflection.Emit;

namespace CmisObjectModel.ServiceModel.Generic
{
    /// <summary>
   /// Implements mechanism to create a TServiceImpl-instance to handle all requests
   /// of this cmis framework
   /// </summary>
   /// <remarks>
   /// Creates TServiceImpl when needed. The built-in mechanism searches in the AppSettings
   /// for a given ProviderDllPath, loads this dll and searches for a class inherited from/
   /// implementing the TServiceImpl with a constructor taking an uri parameter. To avoid
   /// reflection set the CustomInstanceFactory property.
   /// </remarks>
    public class ServiceImplFactory<TServiceImpl>
    {

        public ServiceImplFactory(Uri baseUri)
        {
            _cmisServiceImpl = CreateInstance(baseUri);
        }

        #region Helper classes
        public delegate TServiceImpl InstanceFactory(Uri baseUri);
        #endregion

        #region TServiceImpl-Factory
        private static InstanceFactory _customInstanceFactory;
        /// <summary>
      /// Provides injection of 
      /// </summary>
      /// <value></value>
      /// <returns></returns>
      /// <remarks></remarks>
        public static InstanceFactory CustomInstanceFactory
        {
            get
            {
                return _customInstanceFactory;
            }
            set
            {
                _customInstanceFactory = value;
            }
        }

        /// <summary>
      /// Creates a new instance that implements the ICmisServicesImpl interface
      /// </summary>
      /// <param name="baseUri"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        private static TServiceImpl CreateInstance(Uri baseUri)
        {
            if (_customInstanceFactory is null)
            {
                string dllPath = System.Configuration.ConfigurationManager.AppSettings["ProviderDllPath"];

                if (!string.IsNullOrEmpty(dllPath))
                {
                    dllPath = dllPath.Replace("[$BaseDirectory]", AppDomain.CurrentDomain.BaseDirectory);

                    if (!System.IO.File.Exists(dllPath))
                    {
                        throw new System.IO.FileNotFoundException(null, dllPath);
                    }
                    else
                    {
                        var assembly = sr.Assembly.LoadFrom(dllPath);
                        var cis = (from type in assembly.GetTypes()
                                   let ci = !type.IsAbstract && (type.IsPublic || type.IsNestedPublic) && typeof(TServiceImpl).IsAssignableFrom(type) ? type.GetConstructor(new Type[] { typeof(Uri) }) : null
                                   where ci is not null
                                   select ci).ToArray();
                        if (cis.Length == 0)
                        {
                            throw new sr.InvalidFilterCriteriaException(string.Format(My.Resources.Resources.ServicesImplNotFound, typeof(TServiceImpl).IsInterface ? "implement" : "inherit", typeof(TServiceImpl).FullName));
                        }
                        else
                        {
                            // generate IL code for _customInstanceFactory
                            var method = new sre.DynamicMethod("", typeof(TServiceImpl), new Type[] { typeof(Uri) });
                            var il = method.GetILGenerator(256);

                            il.Emit(sre.OpCodes.Ldarg_0);
                            il.Emit(sre.OpCodes.Newobj, cis[0]);
                            il.Emit(sre.OpCodes.Ret);
                            _customInstanceFactory = (InstanceFactory)method.CreateDelegate(typeof(InstanceFactory));
                        }
                    }
                }
            }

            return _customInstanceFactory.Invoke(baseUri);
        }
        #endregion

        private readonly TServiceImpl _cmisServiceImpl;
        /// <summary>
      /// Returns a valid instance
      /// </summary>
      /// <value></value>
      /// <returns></returns>
      /// <remarks></remarks>
        public TServiceImpl CmisServiceImpl
        {
            get
            {
                return _cmisServiceImpl;
            }
        }

    }
}