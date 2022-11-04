using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ccg = CmisObjectModel.Collections.Generic;
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
using ccg1 = CmisObjectModel.Common.Generic;
using ccc = CmisObjectModel.Core.Collections;
using ccp = CmisObjectModel.Core.Properties;
/* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.JSON.Collections
{
    /// <summary>
   /// Representation of cmisPropertiesType as a string-to-object-map
   /// </summary>
   /// <remarks>SuccinctProperties definition in a cmisObjectType</remarks>
    public class SuccinctProperties : ccg.ArrayMapper<ccc.cmisPropertiesType, ccp.cmisProperty, string, object, Serialization.CmisPropertyResolver>
    {

        public SuccinctProperties(ccc.cmisPropertiesType owner) : base(owner, owner.DefaultArrayProperty, ccp.cmisProperty.DefaultKeyProperty, new ccg1.DynamicProperty<ccp.cmisProperty, object>(cmisProperty =>
{
var values = cmisProperty.Values;
int length = values is null ? 0 : values.Length;

switch (length)
{
case 0:
{
return null;
}
case 1:
{
return values[0];
}

default:
{
return cmisProperty.PropertyType.CreateValuesArray(values);
}
}
}, (cmisProperty, values) => { if (values is null) { cmisProperty.Values = null; cmisProperty.Cardinality = CmisObjectModel.Core.enumCardinality.multi; } else if (values.GetType().IsArray) { cmisProperty.Values = (from value in (IEnumerable<object>)values select value).ToArray(); cmisProperty.Cardinality = CmisObjectModel.Core.enumCardinality.multi; } else { cmisProperty.Value = values; cmisProperty.Cardinality = CmisObjectModel.Core.enumCardinality.single; } }, "Value"))
        {
        }

        #region IJavaSerializationProvider
        /// <summary>
      /// More comfortable access to JavaExport
      /// </summary>
        public new IDictionary<string, object> JavaExport()
        {
            return base.JavaExport(this, null) as IDictionary<string, object>;
        }

        /// <summary>
      /// More comfortable access to JavaImport
      /// </summary>
        public new ccc.cmisPropertiesType JavaImport(IDictionary<string, object> source)
        {
            base.JavaImport(source, null);
            return _owner;
        }
        #endregion

    }
}