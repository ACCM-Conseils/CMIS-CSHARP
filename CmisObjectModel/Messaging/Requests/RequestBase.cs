using System;
using System.Collections.Generic;
using sxs = System.Xml.Serialization;
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

namespace CmisObjectModel.Messaging.Requests
{
    public abstract partial class RequestBase : Serialization.XmlSerializable
    {

        #region Constructors
        static RequestBase()
        {
            ExploreAssembly(typeof(RequestBase).Assembly);
        }
        protected RequestBase()
        {
        }
        protected RequestBase(bool? initClassSupported) : base(initClassSupported)
        {
        }

        /// <summary>
      /// Creates a RequestBase-instance from the current node of the reader-object using the
      /// name of the current node to determine the suitable type
      /// </summary>
      /// <param name="reader"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public static RequestBase CreateInstance(System.Xml.XmlReader reader)
        {
            reader.MoveToContent();

            string nodeName = reader.LocalName;
            if (!string.IsNullOrEmpty(nodeName))
            {
                nodeName = nodeName.ToLowerInvariant();
                if (_factories.ContainsKey(nodeName))
                {
                    var retVal = _factories[nodeName].CreateInstance();

                    if (retVal is not null)
                    {
                        var attributeOverrides = Serialization.XmlAttributeOverrides.GetInstance(reader);

                        if (attributeOverrides is not null)
                        {
                            // duplicate xmlRoot for retVal-type, if available
                            var xmlRoot = attributeOverrides.get_XmlRoot(typeof(RequestBase));

                            if (xmlRoot is not null)
                                attributeOverrides.set_XmlRoot(retVal.GetType(), xmlRoot);
                        }
                        retVal.ReadXml(reader);
                        return retVal;
                    }
                }
            }

            // current node doesn't describe a RequestBase-instance
            return null;
        }

        /// <summary>
      /// Searches in assembly for types supporting requests
      /// </summary>
      /// <param name="assembly"></param>
      /// <remarks></remarks>
        public static bool ExploreAssembly(System.Reflection.Assembly assembly)
        {
            try
            {
                var baseType = typeof(RequestBase);

                // explore the complete assembly if possible
                if (assembly is not null)
                {
                    foreach (Type type in assembly.GetTypes())
                    {
                        try
                        {
                            var attrs = type.GetCustomAttributes(typeof(sxs.XmlRootAttribute), false);

                            if (baseType.IsAssignableFrom(type) && !type.IsAbstract && type.GetConstructor(new Type[] { }) is not null)
                            {
                                string elementName = attrs is not null && attrs.Length > 0 ? ((sxs.XmlRootAttribute)attrs[0]).ElementName : null;

                                if (!string.IsNullOrEmpty(elementName) && !_factories.ContainsKey(elementName))
                                {
                                    // create factory for this valid type
                                    _factories.Add(elementName.ToLowerInvariant(), (Common.Generic.Factory<RequestBase>)Activator.CreateInstance(_genericTypeDefinition.MakeGenericType(baseType, type)));

                                }
                            }
                        }
                        catch
                        {
                        }
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        protected static Dictionary<string, Common.Generic.Factory<RequestBase>> _factories = new Dictionary<string, Common.Generic.Factory<RequestBase>>();
        /// <summary>
      /// GetType(Generic.Factory(Of RequestBase, TDerivedFromRequestBase))
      /// </summary>
      /// <remarks></remarks>
        private static Type _genericTypeDefinition = typeof(Common.Generic.Factory<RequestBase, addObjectToFolder>).GetGenericTypeDefinition();
        #endregion

        /// <summary>
      /// Returns value if not null or empty, otherwise defaultValue
      /// </summary>
      /// <param name="value"></param>
      /// <param name="defaultValue"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        protected string Read(string value, string defaultValue)
        {
            return string.IsNullOrEmpty(value) ? defaultValue : value;
        }

        /// <summary>
      /// Converts value if not null or empty, otherwise defaultValue
      /// </summary>
      /// <typeparam name="T"></typeparam>
      /// <param name="value"></param>
      /// <param name="defaultValue"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        protected T Read<T>(string value, T defaultValue)
        {
            return string.IsNullOrEmpty(value) ? defaultValue : CommonFunctions.ConvertBack(value, defaultValue);
        }

        /// <summary>
      /// Converts value to TEnum if not null or empty, otherwise defaultValue
      /// </summary>
      /// <typeparam name="TEnum"></typeparam>
      /// <param name="value"></param>
      /// <param name="defaultValue"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        protected TEnum ReadEnum<TEnum>(string value, TEnum defaultValue) where TEnum : struct
        {
            var enumValue = default(TEnum);

            return string.IsNullOrEmpty(value) || !CommonFunctions.TryParse(value, ref enumValue, true) ? defaultValue : enumValue;
        }

        protected TEnum? ReadOptionalEnum<TEnum>(string value, TEnum? defaultValue) where TEnum : struct
        {
            var enumValue = default(TEnum);

            return string.IsNullOrEmpty(value) || !CommonFunctions.TryParse(value, ref enumValue, true) ? defaultValue : enumValue;
        }

        /// <summary>
      /// Reads the queryStringParameters of the current request and copies the values to
      /// corresponding properties
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <remarks></remarks>
        public virtual void ReadQueryString(string repositoryId)
        {
        }

    }
}