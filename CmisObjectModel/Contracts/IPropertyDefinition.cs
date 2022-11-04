using System;
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
using cc = CmisObjectModel.Core;
using ccdp = CmisObjectModel.Core.Definitions.Properties;

namespace CmisObjectModel.Contracts
{
    public interface IPropertyDefinition
    {
        cc.enumCardinality Cardinality { get; set; }
        cc.Choices.cmisChoice[] Choices { get; set; }
        ccg.ArrayMapper<ccdp.cmisPropertyDefinitionType, cc.Choices.cmisChoice> ChoicesAsReadOnly { get; }
        Type ChoiceType { get; }
        cc.Properties.cmisProperty CreateProperty();
        cc.Properties.cmisProperty CreateProperty(params object[] values);
        Type CreatePropertyResultType { get; }
        cc.Properties.cmisProperty DefaultValue { get; set; }
        bool? Inherited { get; set; }
        bool? OpenChoice { get; set; }
        bool Orderable { get; set; }
        cc.enumPropertyType PropertyType { get; }
        Type PropertyValueType { get; }
        bool Required { get; set; }
        cc.enumUpdatability Updatability { get; set; }
    }
}