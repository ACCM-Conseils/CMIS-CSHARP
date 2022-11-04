using System;
using System.Collections.Generic;
using ssw = System.ServiceModel.Web;
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
using CmisObjectModel.Constants;
using Microsoft.VisualBasic.CompilerServices;

namespace CmisObjectModel.Messaging
{
    [sxs.XmlRoot(ElementName = "cmisFault", Namespace = Namespaces.cmism)]
    public partial class cmisFaultType
    {

        #region Constructors
        public cmisFaultType(enumServiceException type, string message) : this(type.ToHttpStatusCode(), type, message)
        {
        }

        public cmisFaultType(System.Net.HttpStatusCode code, enumServiceException type, string message)
        {
            _code = (long)code;
            _message = message;
            _type = type;
        }

        /// <summary>
      /// Creates a cmisFaultType-instance from ex
      /// </summary>
      /// <param name="ex"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public static cmisFaultType CreateInstance(Exception ex)
        {
            var extensions = new Extensions.Extension[] { new ExceptionExtension(ex) };
            return new cmisFaultType(System.Net.HttpStatusCode.InternalServerError, enumServiceException.runtime, ex.Message) { _extensions = extensions };
        }

        public static cmisFaultType CreateInstance(ssw.WebFaultException ex)
        {
            var extensions = new Extensions.Extension[] { new ExceptionExtension(ex) };
            return new cmisFaultType(ex.StatusCode, ex.StatusCode.ToServiceException(), ex.Message) { _extensions = extensions };
        }

        public static cmisFaultType CreateInstance<T>(ssw.WebFaultException<cmisFaultType> ex)
        {
            var extensions = new Extensions.Extension[] { new ExceptionExtension(ex) };
            if (ReferenceEquals(typeof(T), typeof(string)))
            {
                return new cmisFaultType(ex.StatusCode, ex.StatusCode.ToServiceException(), ex.Message + Microsoft.VisualBasic.Constants.vbCrLf + ((ssw.WebFaultException<cmisFaultType>)ex).Detail) { _extensions = extensions };
            }
            else if (ReferenceEquals(typeof(T), typeof(cmisFaultType)))
            {
                var retVal = ((ssw.WebFaultException<cmisFaultType>)ex).Detail;
                if (retVal._extensions is null)
                {
                    retVal._extensions = extensions;
                }
                else
                {
                    {
                        var withBlock = new List<Extensions.Extension>(retVal._extensions.Length + extensions.Length);
                        withBlock.AddRange(retVal._extensions);
                        withBlock.AddRange(extensions);
                        retVal._extensions = withBlock.ToArray();
                    }
                }
                return retVal;
            }
            else
            {
                return new cmisFaultType(ex.StatusCode, ex.StatusCode.ToServiceException(), ex.Message) { _extensions = extensions };
            }
        }
        #endregion

        #region Helper-classes
        /// <summary>
      /// Extension to encapsulate non WebExceptions
      /// </summary>
      /// <remarks></remarks>
        [sxs.XmlRoot(ElementName = "exceptionExtension", Namespace = Namespaces.cmism)]
        [Attributes.CmisTypeInfo("cmism:exceptionExtension", null, "exceptionExtension")]
        public class ExceptionExtension : Extensions.Extension
        {

            #region Constructors
            public ExceptionExtension()
            {
            }

            public ExceptionExtension(Exception ex)
            {
                _exceptionTypeName = ex.GetType().FullName;
                _source = ex.Source;
                _stackTrace = ex.StackTrace;
                if (ex.InnerException is not null)
                    _innerException = cmisFaultType.CreateInstance(ex.InnerException);
            }
            #endregion

            #region IXmlSerializable
            private static Dictionary<string, Action<ExceptionExtension, string>> _setter = new Dictionary<string, Action<ExceptionExtension, string>>() { { "exceptiontypename", SetExceptionTypeName } }; // _setter

            protected override void ReadAttributes(System.Xml.XmlReader reader)
            {
                // at least one property is serialized in an attribute-value
                if (_setter.Count > 0)
                {
                    for (int attributeIndex = 0, loopTo = reader.AttributeCount - 1; attributeIndex <= loopTo; attributeIndex++)
                    {
                        reader.MoveToAttribute(attributeIndex);
                        // attribute name
                        string key = reader.Name.ToLowerInvariant();
                        if (_setter.ContainsKey(key))
                            _setter[key].Invoke(this, reader.GetAttribute(attributeIndex));
                    }
                }
            }

            protected override void ReadXmlCore(System.Xml.XmlReader reader, Serialization.XmlAttributeOverrides attributeOverrides)
            {
                _innerException = Read(reader, attributeOverrides, "innerException", Namespaces.cmism, GenericXmlSerializableFactory<cmisFaultType>);
                _source = Read(reader, attributeOverrides, "source", Namespaces.cmism, _source);
                _stackTrace = Read(reader, attributeOverrides, "stackTrace", Namespaces.cmism, _stackTrace);
            }

            protected override void WriteXmlCore(System.Xml.XmlWriter writer, Serialization.XmlAttributeOverrides attributeOverrides)
            {
                WriteAttribute(writer, attributeOverrides, "exceptionTypeName", null, _exceptionTypeName);
                WriteElement(writer, attributeOverrides, "innerException", Namespaces.cmism, _innerException);
                if (!string.IsNullOrEmpty(_source))
                    WriteElement(writer, attributeOverrides, "source", Namespaces.cmism, _source);
                if (!string.IsNullOrEmpty(_stackTrace))
                    WriteElement(writer, attributeOverrides, "stackTrace", Namespaces.cmism, _stackTrace);
            }
            #endregion

            protected string _exceptionTypeName;
            public string ExceptionTypeName
            {
                get
                {
                    return _exceptionTypeName;
                }
                set
                {
                    if ((value ?? "") != (_exceptionTypeName ?? ""))
                    {
                        string oldValue = _exceptionTypeName;
                        _exceptionTypeName = value;
                        OnPropertyChanged("ExceptionTypeName", value, oldValue);
                    }
                }
            } // ExceptionTypeName
            private static void SetExceptionTypeName(ExceptionExtension instance, string value)
            {
                instance.ExceptionTypeName = value;
            }

            protected cmisFaultType _innerException;
            public cmisFaultType InnerException
            {
                get
                {
                    return _innerException;
                }
                set
                {
                    if (!ReferenceEquals(value, _innerException))
                    {
                        var oldValue = _innerException;
                        _innerException = value;
                        OnPropertyChanged("InnerException", value, oldValue);
                    }
                }
            } // InnerException

            protected string _source;
            public string Source
            {
                get
                {
                    return _source;
                }
                set
                {
                    if ((_source ?? "") != (value ?? ""))
                    {
                        string oldValue = _source;
                        _source = value;
                        OnPropertyChanged("Source", value, oldValue);
                    }
                }
            } // Source

            protected string _stackTrace;
            public string StackTrace
            {
                get
                {
                    return _stackTrace;
                }
                set
                {
                    if ((value ?? "") != (_stackTrace ?? ""))
                    {
                        string oldValue = _stackTrace;
                        _stackTrace = value;
                        OnPropertyChanged("StackTrace", value, oldValue);
                    }
                }
            } // StackTrace

        }
        #endregion

        /// <summary>
      /// Generates a WebFaultException(Of cmisFaultType) for an invalid argument exception
      /// </summary>
      /// <param name="argumentNameOrErrorText">To define a MUST-NOT-be-null-or-empty-InvalidArgumentException set this parameter
      /// to the name of the invalid argument and set the isNotNullOrEmptyException-parameter to True. Otherwise this parameter
      /// must contain the complete errorText and the isNotNullOrEmptyException must be set to False.</param>
      /// <param name="isNotNullOrEmptyException">If True the argumentNameOrErrorText-parameter is handled as argumentName</param>
      /// <returns></returns>
      /// <remarks></remarks>
        public static ssw.WebFaultException<cmisFaultType> CreateInvalidArgumentException(string argumentNameOrErrorText, bool isNotNullOrEmptyException = true)
        {
            var httpStatusCode = enumServiceException.invalidArgument.ToHttpStatusCode();

            return new ssw.WebFaultException<cmisFaultType>(new cmisFaultType(httpStatusCode, enumServiceException.invalidArgument, isNotNullOrEmptyException ? string.Format(My.Resources.Resources.InvalidArgument, argumentNameOrErrorText) : argumentNameOrErrorText), httpStatusCode);
        }

        /// <summary>
      /// Generates a WebFaultException(Of cmisFaultType) for a not found exception
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public static ssw.WebFaultException<cmisFaultType> CreateNotFoundException(string id = null)
        {
            var httpStatusCode = enumServiceException.objectNotFound.ToHttpStatusCode();

            return new ssw.WebFaultException<cmisFaultType>(new cmisFaultType(httpStatusCode, enumServiceException.objectNotFound, "Object " + (id is null ? "" : "'" + id + "' ") + "not found."), httpStatusCode);
        }

        public static ssw.WebFaultException<cmisFaultType> CreateNotSupportedException(string methodName)
        {
            var httpStatusCode = enumServiceException.notSupported.ToHttpStatusCode();

            return new ssw.WebFaultException<cmisFaultType>(new cmisFaultType(httpStatusCode, enumServiceException.notSupported, string.Format(My.Resources.Resources.NotSupported, methodName)), httpStatusCode);
        }

        public static ssw.WebFaultException<cmisFaultType> CreatePermissionDeniedException()
        {
            var httpStatusCode = enumServiceException.permissionDenied.ToHttpStatusCode();

            return new ssw.WebFaultException<cmisFaultType>(new cmisFaultType(httpStatusCode, enumServiceException.permissionDenied, "Permission denied."), httpStatusCode);
        }

        /// <summary>
      /// Generates a WebFaultException(Of cmisFaultType) for an unknown error
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public static ssw.WebFaultException<cmisFaultType> CreateUnknownException()
        {
            var httpStatusCode = enumServiceException.runtime.ToHttpStatusCode();

            return new ssw.WebFaultException<cmisFaultType>(new cmisFaultType(httpStatusCode, enumServiceException.runtime, "Unknown server error."), httpStatusCode);
        }

        /// <summary>
      /// Generates a WebFaultException(Of cmisFaultType) for an unspecific exception
      /// </summary>
      /// <param name="ex"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public static ssw.WebFaultException<cmisFaultType> CreateUnknownException(Exception ex)
        {
            return new ssw.WebFaultException<cmisFaultType>(CreateInstance(ex), System.Net.HttpStatusCode.InternalServerError);
        }

        /// <summary>
      /// Creates WebFaultException(Of cmisFaultType) containing this instance
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public ssw.WebFaultException<cmisFaultType> ToFaultException()
        {
            return new ssw.WebFaultException<cmisFaultType>(this, (System.Net.HttpStatusCode)Conversions.ToInteger(_code));
        }

    }
}