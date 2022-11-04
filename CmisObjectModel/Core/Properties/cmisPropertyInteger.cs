using System;
using System.Data;
using System.Linq;
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
/* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.Core.Properties
{
    [sxs.XmlRoot(DefaultElementName, Namespace = Constants.Namespaces.cmis)]
    [Attributes.CmisTypeInfo(CmisTypeName, TargetTypeName, DefaultElementName)]
    public partial class cmisPropertyInteger
    {

        public cmisPropertyInteger(string propertyDefinitionId, string localName, string displayName, string queryName, params long[] values) : base(propertyDefinitionId, localName, displayName, queryName, values)
        {
        }

        #region Constants
        public const string CmisTypeName = "cmis:cmisPropertyInteger";
        public const string TargetTypeName = "integer";
        public const string DefaultElementName = "propertyInteger";
        #endregion

        #region IComparable
        protected override int CompareTo(params long[] other)
        {
            int length = _values is null ? 0 : _values.Length;
            int otherLength = other is null ? 0 : other.Length;
            if (otherLength == 0)
            {
                return length == 0 ? 0 : 1;
            }
            else if (length == 0)
            {
                return -1;
            }
            else
            {
                for (int index = 0, loopTo = Math.Min(length, otherLength) - 1; index <= loopTo; index++)
                {
                    long first = _values[index];
                    long second = other[index];
                    if (first < second)
                    {
                        return -1;
                    }
                    else if (first > second)
                    {
                        return 1;
                    }
                }
                return length == otherLength ? 0 : length > otherLength ? 1 : -1;
            }
        }
        #endregion

        /// <summary>
      /// Represent the values of the current instance as a cmisPropertyDateTime-instance
      /// </summary>
        public cmisPropertyDateTime ToDateTimeProperty()
        {
            var values = _values is null ? null : (from value in _values
                                                   select ((DateTimeOffset)CommonFunctions.FromJSONTime(value))).ToArray();
            return new cmisPropertyDateTime(PropertyDefinitionId, LocalName, DisplayName, QueryName, values) { Cardinality = Cardinality };
        }

        public override enumPropertyType Type
        {
            get
            {
                return enumPropertyType.integer;
            }
        }

    }
}