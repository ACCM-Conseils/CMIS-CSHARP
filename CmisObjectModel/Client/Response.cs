using System;
using System.Collections.Generic;
using sn = System.Net;
using ssc = System.Security.Cryptography;
using ss = System.ServiceModel;
using ssw = System.ServiceModel.Web;
using sx = System.Xml;
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
/* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.Client
{
    /// <summary>
   /// Represents the response of a webservice-request
   /// </summary>
   /// <remarks></remarks>
    public class Response : IDisposable
    {

        /// <summary>
      /// StatusCodes of successful requests
      /// </summary>
      /// <remarks></remarks>
        protected static HashSet<sn.HttpStatusCode> _successCodes = new HashSet<sn.HttpStatusCode>() { sn.HttpStatusCode.Created, sn.HttpStatusCode.NonAuthoritativeInformation, sn.HttpStatusCode.OK, sn.HttpStatusCode.PartialContent };

        #region Constructors
        public Response(sn.HttpStatusCode statusCode, string message, string contentType, long? contentLength = default, System.IO.Stream stream = null)
        {
            _statusCode = statusCode;
            _message = message;
            _contentType = contentType;
            _contentLength = contentLength;
            _stream = stream;
        }

        public Response(sn.HttpWebResponse response)
        {
            string contentTransferEncoding = response.Headers[RFC2231Helper.ContentTransferEncoding];
            bool isBase64 = contentTransferEncoding is not null && string.Compare(contentTransferEncoding, "base64", true) == 0;
            const int _64k = 0x10000;

            _webResponse = response;
            _statusCode = response.StatusCode;
            _message = response.StatusDescription;
            _contentType = response.ContentType;
            /* TODO ERROR: Skipped IfDirectiveTrivia
            #If xs_Integer = "Int32" OrElse xs_Integer = "Integer" OrElse xs_Integer = "Single" Then
            *//* TODO ERROR: Skipped DisabledTextTrivia
                     _contentLength = If(response.ContentLength = -1, Nothing, CInt(response.ContentLength))
            *//* TODO ERROR: Skipped ElseDirectiveTrivia
            #Else
            */
            _contentLength = response.ContentLength == -1 ? default : response.ContentLength;
            /* TODO ERROR: Skipped EndIfDirectiveTrivia
            #End If
            */
            if (_successCodes.Contains(_statusCode))
            {
                if (isBase64)
                {
                    _stream = new System.IO.BufferedStream(new ssc.CryptoStream(response.GetResponseStream(), new ssc.FromBase64Transform(), ssc.CryptoStreamMode.Read), _64k);
                }
                else
                {
                    _stream = new System.IO.BufferedStream(response.GetResponseStream(), _64k);
                }
            }
            else
            {
                try
                {
                    _webResponse = null;
                    response.Close();
                }
                catch
                {
                }
            }
        }

        public Response(ss.FaultException exception)
        {
            _statusCode = GenericRuntimeHelper.GetStatusCode(exception);
            _message = exception.Message;
            _exception = exception;
            if (exception is ssw.WebFaultException<Messaging.cmisFaultType>)
            {
                _cmisFault = ((ssw.WebFaultException<Messaging.cmisFaultType>)exception).Detail;
            }
        }

        public Response(sn.WebException exception)
        {
            var response = exception.Response;
            sn.HttpWebResponse httpResponse = response as sn.HttpWebResponse;

            if (httpResponse is null)
            {
                _statusCode = sn.HttpStatusCode.InternalServerError;
                _message = exception.Status.ToString();
                _exception = new ssw.WebFaultException<string>(_message, sn.HttpStatusCode.InternalServerError);
            }
            else
            {
                _statusCode = httpResponse.StatusCode;
                _message = httpResponse.StatusDescription;
                _contentType = httpResponse.ContentType;
                if (string.IsNullOrEmpty(_contentType))
                {
                    _exception = new ssw.WebFaultException<string>(_message, _statusCode);
                }
                else
                {
                    var responseStream = response.GetResponseStream();

                    if (_contentType.StartsWith("text/", StringComparison.InvariantCultureIgnoreCase))
                    {
                        using (var sr = new System.IO.StreamReader(responseStream))
                        {
                            _errorContent = sr.ReadToEnd();
                            sr.Close();
                        }
                        _exception = new ssw.WebFaultException<string>(_errorContent, httpResponse.StatusCode);
                    }
                    else if (_contentType.StartsWith(MediaTypes.XmlApplication, StringComparison.InvariantCultureIgnoreCase))
                    {
                        using (var sr = new System.IO.StreamReader(responseStream, new System.Text.UTF8Encoding(false)))
                        {
                            _errorContent = sr.ReadToEnd();
                            sr.Close();
                        }
                        using (var sr = new System.IO.StringReader(_errorContent))
                        {
                            var reader = sx.XmlReader.Create(sr);
                            var xmlRoot = typeof(Messaging.cmisFaultType).GetXmlRootAttribute(exactNonNullResult: true);

                            reader.MoveToContent();
                            if (string.Compare(reader.Name ?? "", xmlRoot.ElementName ?? "", true) == 0 && string.Compare(reader.NamespaceURI ?? "", xmlRoot.Namespace ?? "", true) == 0)
                            {
                                _cmisFault = new Messaging.cmisFaultType();
                                _cmisFault.ReadXml(reader);
                                _exception = new ssw.WebFaultException<Messaging.cmisFaultType>(_cmisFault, httpResponse.StatusCode);
                            }
                            else
                            {
                                _exception = new ssw.WebFaultException<string>(_errorContent, httpResponse.StatusCode);
                            }
                            reader.Close();
                            sr.Close();
                        }
                    }
                    else if (_contentType.StartsWith(MediaTypes.Json, StringComparison.InvariantCultureIgnoreCase))
                    {
                        using (var sr = new System.IO.StreamReader(responseStream, new System.Text.UTF8Encoding(false)))
                        {
                            _errorContent = sr.ReadToEnd();
                            sr.Close();
                        }
                        var serializer = new JSON.Serialization.JavaScriptSerializer();
                        _cmisFault = serializer.Deserialize<Messaging.cmisFaultType>(_errorContent);
                        _exception = new ssw.WebFaultException<Messaging.cmisFaultType>(_cmisFault, httpResponse.StatusCode);
                    }
                    else
                    {
                        _exception = new ssw.WebFaultException<string>(_message, _statusCode);
                    }
                    responseStream.Close();
                }
            }
        }
        #endregion

        #region IDisposable
        public void Dispose()
        {
            Dispose(true);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_stream is not null)
                    {
                        try
                        {
                            _stream.Close();
                            _stream = null;
                        }
                        catch
                        {
                        }
                    }
                    try
                    {
                        _webResponse.Close();
                    }
                    catch
                    {
                    }
                }
                _disposed = true;
            }
        }

        protected bool _disposed = false;
        public bool Disposed
        {
            get
            {
                return _disposed;
            }
        }
        #endregion

        protected Messaging.cmisFaultType _cmisFault;
        public Messaging.cmisFaultType CmisFault
        {
            get
            {
                return _cmisFault;
            }
        }

        protected long? _contentLength;
        public long? ContentLength
        {
            get
            {
                return _contentLength;
            }
        }

        protected string _contentType;
        public string ContentType
        {
            get
            {
                return _contentType;
            }
        }

        protected string _errorContent;
        public string ErrorContent
        {
            get
            {
                return _errorContent;
            }
        }

        protected ss.FaultException _exception;
        public ss.FaultException Exception
        {
            get
            {
                return _exception;
            }
        }

        protected string _message;
        public string Message
        {
            get
            {
                return _message;
            }
        }

        protected sn.HttpStatusCode _statusCode;
        public sn.HttpStatusCode StatusCode
        {
            get
            {
                return _statusCode;
            }
        }

        protected System.IO.Stream _stream;
        public System.IO.Stream Stream
        {
            get
            {
                return _stream;
            }
        }

        protected sn.WebResponse _webResponse;
        public sn.WebResponse WebResponse
        {
            get
            {
                return _webResponse;
            }
        }

        public static implicit operator System.IO.Stream(Response value)
        {
            return value is null ? null : value._stream;
        }
        public static implicit operator Response(ss.FaultException value)
        {
            return new Response(value);
        }
        public static implicit operator Response(System.IO.Stream value)
        {
            return new Response(sn.HttpStatusCode.OK, "OK", MediaTypes.Stream, stream: value);
        }
    }
}