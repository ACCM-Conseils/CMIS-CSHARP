using System;
using System.Collections.Generic;
using sx = System.Xml;
using cac = CmisObjectModel.Attributes.CmisTypeInfoAttribute;
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

namespace CmisObjectModel.Extensions
{
    [Attributes.JavaScriptObjectResolver(typeof(JSON.Serialization.ExtensionResolver<Extension>), "{\"\":\"TExtension\"}")]
    public abstract class Extension : Serialization.XmlSerializable
    {

        #region Constructors
        static Extension()
        {
            // search for all types supporting cmis typedefinition ...
            if (!ExploreAssembly(typeof(Extension).Assembly))
            {
                // ... failed.
                // At least register well-known Alfresco-extension-classes
                cac.ExploreTypes(_factories, _genericTypeDefinition, typeof(Alfresco.Aspects), typeof(Alfresco.SetAspects), typeof(Data.ConverterDefinition), typeof(Data.RowCollection));
                ExploreFactories();
            }
        }
        protected Extension()
        {
        }
        protected Extension(bool? initClassSupported) : base(initClassSupported)
        {
        }

        /// <summary>
      /// Creates a new instance (similar to ReadXml() in IXmlSerializable-classes)
      /// </summary>
      /// <param name="reader"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public static Extension CreateInstance(sx.XmlReader reader)
        {
            // first chance: from current node name
            string nodeName = CommonFunctions.GetCurrentStartElementLocalName(reader);
            int argunknownUriIndex = 0;
            string namespacePrefix = CommonFunctions.GetDefaultPrefix(reader.NamespaceURI, ref argunknownUriIndex);

            if (!(string.IsNullOrEmpty(nodeName) || string.IsNullOrEmpty(namespacePrefix)))
            {
                string key = namespacePrefix.ToLowerInvariant() + ":" + nodeName.ToLowerInvariant();
                if (_factories.ContainsKey(key))
                {
                    var retVal = _factories[key].CreateInstance() as Extension;

                    if (retVal is not null)
                    {
                        retVal.ReadXml(reader);
                        return retVal;
                    }
                }
            }

            // unable to interpret node as extension
            return null;
        }

        /// <summary>
      /// Creates a new instance for known extensionTypeName
      /// </summary>
      /// <param name="extensionTypeName"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public static Extension CreateInstance(string extensionTypeName)
        {
            extensionTypeName = (extensionTypeName ?? string.Empty).ToLowerInvariant();
            if (_factories.ContainsKey(extensionTypeName))
            {
                return _factories[extensionTypeName].CreateInstance();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
      /// Searches in assembly for types supporting extensions
      /// </summary>
      /// <param name="assembly"></param>
      /// <remarks></remarks>
        public static bool ExploreAssembly(System.Reflection.Assembly assembly)
        {
            try
            {
                // explore the complete assembly if possible
                if (assembly is not null)
                {
                    cac.ExploreTypes(_factories, _genericTypeDefinition, assembly.GetTypes());
                }
                ExploreFactories();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
      /// Updates GetExtensionType-support
      /// </summary>
      /// <remarks></remarks>
        private static void ExploreFactories()
        {
            _extensionTypes.Clear();
            foreach (KeyValuePair<string, CmisObjectModel.Common.Generic.Factory<Extension>> de in _factories)
            {
                var factoryType = de.Value.GetType();
                if (factoryType.IsGenericType)
                {
                    var genericArgumentTypes = de.Value.GetType().GetGenericArguments();
                    if (genericArgumentTypes is not null && genericArgumentTypes.Length == 2)
                    {
                        var extensionType = genericArgumentTypes[1];
                        if (typeof(Extension).IsAssignableFrom(extensionType))
                            _extensionTypes.Add(de.Key, genericArgumentTypes[1]);
                    }
                }
            }
        }

        /// <summary>
      /// Returns the identifier of the extension-class
      /// </summary>
      /// <value></value>
      /// <returns></returns>
      /// <remarks></remarks>
        public string ExtensionTypeName
        {
            get
            {
                var attrs = GetType().GetCustomAttributes(typeof(cac), false);

                if (attrs is not null && attrs.Length > 0)
                {
                    return ((cac)attrs[0]).CmisTypeName;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        protected static Dictionary<string, Type> _extensionTypes = new Dictionary<string, Type>();
        protected static Dictionary<string, CmisObjectModel.Common.Generic.Factory<Extension>> _factories = new Dictionary<string, CmisObjectModel.Common.Generic.Factory<Extension>>();

        /// <summary>
      /// Gets the extension type that declared the extensionTypeName
      /// </summary>
        public static Type GetExtensionType(string extensionTypeName)
        {
            extensionTypeName = (extensionTypeName ?? string.Empty).ToLowerInvariant();
            return _extensionTypes.ContainsKey(extensionTypeName) ? _extensionTypes[extensionTypeName] : null;
        }

        /// <summary>
      /// GetType(Generic.Factory(Of Extension, TDerivedFromExtension))
      /// </summary>
      /// <remarks></remarks>
        private static Type _genericTypeDefinition = typeof(CmisObjectModel.Common.Generic.Factory<Extension, Alfresco.Aspects>).GetGenericTypeDefinition();
        #endregion

    }
}