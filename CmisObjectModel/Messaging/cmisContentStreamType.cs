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
using CmisObjectModel.Common;
using CmisObjectModel.Constants;

namespace CmisObjectModel.Messaging
{
    public partial class cmisContentStreamType
    {

        public cmisContentStreamType(System.IO.Stream stream, string filename, string mimeType, bool internalUsage = false)
        {
            if (stream is not null)
            {
                var ms = new System.IO.MemoryStream();
                stream.CopyTo(ms);
                ms.Position = 0L;
                set_BinaryStream(internalUsage, ms);
            }
            _filename = filename;
            _mimeType = mimeType;
        }

        public cmisContentStreamType(System.IO.Stream stream, string filename, string mimeType, enumGetContentStreamResult result, bool internalUsage = true) : this(stream, filename, mimeType, internalUsage)
        {
            _result = result;
        }

        private System.IO.MemoryStream _binaryStream;
        public System.IO.Stream BinaryStream
        {
            get
            {
                if (_binaryStream is not null)
                {
                    return _binaryStream;
                }
                else if (string.IsNullOrEmpty(_stream))
                {
                    return null;
                }
                else
                {
                    return new System.IO.MemoryStream(Convert.FromBase64String(_stream));
                }
            }
        }
        /// <summary>
      /// Writes the binary stream
      /// </summary>
      /// <param name="internalUseOnly">Set this parameter to false to update the Stream- and Length-property also</param>
      /// <value></value>
      /// <remarks></remarks>
        public void set_BinaryStream(bool internalUseOnly, System.IO.MemoryStream value)
        {
            var oldValue = _binaryStream;

            _binaryStream = value;
            if (!internalUseOnly)
            {
                if (value is null)
                {
                    _stream = null;
                    _length = 0;
                    _result = enumGetContentStreamResult.NotSet;
                }
                else
                {
                    Stream = Convert.ToBase64String(value.ToArray());
                    /* TODO ERROR: Skipped IfDirectiveTrivia
                    #If xs_Integer = "Int32" OrElse xs_integer = "Integer" OrElse xs_integer = "Single" Then
                    *//* TODO ERROR: Skipped DisabledTextTrivia
                                      Length = CInt(value.Length)
                    *//* TODO ERROR: Skipped ElseDirectiveTrivia
                    #Else
                    */
                    Length = value.Length;
                    /* TODO ERROR: Skipped EndIfDirectiveTrivia
                    #End If
                    */
                    _result = enumGetContentStreamResult.Content;
                }
            }
            OnPropertyChanged("BinaryStream", value, oldValue);
        }

        /// <summary>
      /// If necessary the method appends missed appropriate properties (cmis:contentStreamFileName,
      /// cmis:contentStreamLength and cmis:contentStreamMimeType) for the instance-properties
      /// FileName, Length and MimeType
      /// </summary>
      /// <param name="cmisObject"></param>
      /// <remarks></remarks>
        public void ExtendProperties(Core.cmisObjectType cmisObject)
        {
            if (cmisObject is not null)
                cmisObject.Properties = ExtendProperties(cmisObject.Properties);
        }
        /// <summary>
      /// If necessary the method appends missed appropriate properties (cmis:contentStreamFileName,
      /// cmis:contentStreamLength and cmis:contentStreamMimeType) for the instance-properties
      /// FileName, Length and MimeType
      /// </summary>
      /// <param name="properties"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public Core.Collections.cmisPropertiesType ExtendProperties(Core.Collections.cmisPropertiesType properties)
        {
            if (!string.IsNullOrEmpty(_filename) || _length.HasValue || !string.IsNullOrEmpty(_mimeType))
            {
                var factory = new PredefinedPropertyDefinitionFactory(null);

                if (properties is null)
                    properties = new Core.Collections.cmisPropertiesType();
                {
                    var withBlock = properties.FindProperties(true, CmisPredefinedPropertyNames.ContentStreamFileName, CmisPredefinedPropertyNames.ContentStreamLength, CmisPredefinedPropertyNames.ContentStreamMimeType);
                    if (!(string.IsNullOrEmpty(_filename) || withBlock.ContainsKey(CmisPredefinedPropertyNames.ContentStreamFileName)))
                    {
                        properties.Append(factory.ContentStreamFileName().CreateProperty(_filename));
                    }
                    if (_length.HasValue && !withBlock.ContainsKey(CmisPredefinedPropertyNames.ContentStreamLength))
                    {
                        properties.Append(factory.ContentStreamLength().CreateProperty(_length.Value));
                    }
                    if (!(string.IsNullOrEmpty(_mimeType) || withBlock.ContainsKey(CmisPredefinedPropertyNames.ContentStreamMimeType)))
                    {
                        properties.Append(factory.ContentStreamMimeType().CreateProperty(_mimeType));
                    }
                }
            }

            return properties;
        }

        private enumGetContentStreamResult _result = enumGetContentStreamResult.NotSet;
        public enumGetContentStreamResult Result
        {
            get
            {
                return _result;
            }
            set
            {
                _result = value;
            }
        }

        public System.Net.HttpStatusCode StatusCode
        {
            get
            {
                return (System.Net.HttpStatusCode)(int)_result;
            }
        }

    }
}