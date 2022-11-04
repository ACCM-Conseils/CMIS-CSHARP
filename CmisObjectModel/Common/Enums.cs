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
using srs = System.Runtime.Serialization;

namespace CmisObjectModel.Common
{
    public enum enumAccessModifier : int
    {
        @this = 0,
        @base = 1
    }

    public enum enumCheckedOutState : int
    {
        notCheckedOut = 0,
        checkedOut = 1,
        checkedOutByMe = 3
    }

    /// <summary>
   /// Specifies the local and remote type in converters
   /// </summary>
   /// <remarks></remarks>
    public enum enumConverterSupportedTypes : int
    {
        boolean = 1,
        @decimal = 2,
        integer = 3,
        @string = 0 // default
    }

    public enum enumDecimalRepresentation : int
    {
        /// <summary>
      /// interpret values of cmisPropertyDecimal as decimal
      /// </summary>
      /// <remarks></remarks>
        @decimal,
        /// <summary>
      /// interpret values of cmisPropertyDecimal as double
      /// </summary>
      /// <remarks></remarks>
        @double
    }

    [Flags()]
    public enum enumKeySyntax : int
    {
        searchIgnoreCase = 1,
        lowerCase = 2 | searchIgnoreCase,
        original = 4,
        originalSearchIgnoreCase = searchIgnoreCase | original,
        both = lowerCase | original
    }

    /// <summary>
   /// Specifies the mapping of data between Server and Client
   /// </summary>
   /// <remarks>If some values transferred between server and client have to be converted this enum defines, which side
   /// has to do the necessary conversion</remarks>
    public enum enumMapDirection : int
    {
        none = 0,
        incoming = 1,
        outgoing = 2,
        bidirectional = incoming | outgoing
    }

    /// <summary>
   /// Specifies the state of protection of a cmis-object
   /// </summary>
    [Flags()]
    public enum enumRetentionState : int
    {
        none = 0,
        preservedByClientHoldIds = 1,
        preservedByExpirationDate = 2,
        preservedByRepository = 4
    }

    /// <summary>
   /// Specifies the supported retentions for documents within the repository
   /// </summary>
    [Flags()]
    public enum enumRetentionCapability : int
    {
        none = 0,
        clientMgt = 1,
        repositoryMgt = 2
    }

    /// <summary>
   /// Specifies the synchronization-mode between two cmis-systems
   /// </summary>
   /// <remarks></remarks>
    public enum enumSyncDirection : int
    {
        custom = -1,
        none = 0,
        oneWay = 1,
        biDirectional = 3
    }

    /// <summary>
   /// Specifies the needed synchronization between two cmis-systems
   /// </summary>
   /// <remarks></remarks>
    public enum enumSyncRequired : int
    {
        custom = enumSyncDirection.custom,
        none = enumSyncDirection.none,
        oneWay = enumSyncDirection.oneWay,
        biDirectional = enumSyncDirection.biDirectional,
        suspended = 4,
        suspendedOneWay = oneWay | suspended,
        suspendedBiDirectional = biDirectional | suspended
    }

    /// <summary>
   /// Specifies the Cmis-Server a client is connected to
   /// </summary>
   /// <remarks></remarks>
    public enum enumVendor : int
    {
        other = 0,
        PatOrg = 1,
        Alfresco = 2,
        [srs.EnumMember(Value = "d.3")]
        d3 = 3,
        SharePoint = 4,
        Agorum = 5
    }

    /// <summary>
   /// Specifies the Type (Binding) of a CMIS-Client
   /// </summary>
   /// <remarks></remarks>
    public enum enumClientType
    {
        AtomPub,
        BrowserBinding,
        Unknown
        // WebService
    }

}