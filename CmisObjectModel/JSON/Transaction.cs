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
using cs = CmisObjectModel.Serialization;
/* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.JSON
{
    /// <summary>
   /// Allow Web-Applications to get access to the last result of a POST-request
   /// </summary>
   /// <remarks>see http://docs.oasis-open.org/cmis/CMIS/v1.1/os/CMIS-v1.1-os.html
   /// 5.4.4.4  Access to Form Response Content</remarks>
    [sxs.XmlRoot("transaction", Namespace = Constants.Namespaces.browser)]
    public class Transaction : cs.XmlSerializable
    {

        #region IXmlSerializable
        protected override void ReadAttributes(System.Xml.XmlReader reader)
        {
        }

        protected override void ReadXmlCore(System.Xml.XmlReader reader, cs.XmlAttributeOverrides attributeOverrides)
        {
            _code = Read(reader, attributeOverrides, "code", Constants.Namespaces.browser, _code);
            _objectId = Read(reader, attributeOverrides, "objectId", Constants.Namespaces.browser, _objectId);
            _exception = Read(reader, attributeOverrides, "exception", Constants.Namespaces.browser, _exception);
            _message = Read(reader, attributeOverrides, "message", Constants.Namespaces.browser, _message);
        }

        protected override void WriteXmlCore(System.Xml.XmlWriter writer, cs.XmlAttributeOverrides attributeOverrides)
        {
            WriteElement(writer, attributeOverrides, "code", Constants.Namespaces.browser, CommonFunctions.Convert(_code));
            if (!string.IsNullOrEmpty(_objectId))
                WriteElement(writer, attributeOverrides, "objectId", Constants.Namespaces.browser, _objectId);
            if (!string.IsNullOrEmpty(_exception))
                WriteElement(writer, attributeOverrides, "exception", Constants.Namespaces.browser, _exception);
            if (!string.IsNullOrEmpty(_message))
                WriteElement(writer, attributeOverrides, "message", Constants.Namespaces.browser, _message);
        }
        #endregion

        protected long _code;
        public virtual long Code
        {
            get
            {
                return _code;
            }
            set
            {
                long oldValue = _code;

                if (value != _code)
                {
                    _code = value;
                    OnPropertyChanged("Code", value, oldValue);
                }
            }
        } // Code

        protected string _exception;
        public virtual string Exception
        {
            get
            {
                return _exception;
            }
            set
            {
                string oldValue = _exception;

                if ((value ?? "") != (_exception ?? ""))
                {
                    _exception = value;
                    OnPropertyChanged("Exception", value, oldValue);
                }
            }
        } // Exception

        protected string _message;
        public virtual string Message
        {
            get
            {
                return _message;
            }
            set
            {
                string oldValue = _message;

                if ((value ?? "") != (_message ?? ""))
                {
                    _exception = value;
                    OnPropertyChanged("Message", value, oldValue);
                }
            }
        } // Message

        protected string _objectId;
        public virtual string ObjectId
        {
            get
            {
                return _objectId;
            }
            set
            {
                string oldValue = _objectId;

                if ((value ?? "") != (_objectId ?? ""))
                {
                    _objectId = value;
                    OnPropertyChanged("ObjectId", value, oldValue);
                }
            }
        } // ObjectId

    }
}