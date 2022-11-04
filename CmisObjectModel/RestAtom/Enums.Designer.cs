﻿
// ***********************************************************************************************************************
// * Project: CmisObjectModelLibrary
// * Copyright (c) 2014, Brügmann Software GmbH, Papenburg, All rights reserved
// * Author: auto-generated code
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
/* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.RestAtom
{
    public enum enumArguments : int
    {
        allVersions,
        append,
        continueOnFailure,
        changeLogToken,
        checkin,
        checkinComment,
        depth,
        direction,
        filter,
        folderId,
        includeACL,
        includeAllowableActions,
        includeProperties,
        includePathSegment,
        includeRelativePathSegment,
        includePropertyDefinitions,
        includePolicyIds,
        includeRelationships,
        includeSubRelationshipTypes,
        isLastChunk,
        major,
        maxItems,
        onlyBasicPermissions,
        orderBy,
        overwriteFlag,
        relationshipDirection,
        relationshipType,
        renditionFilter,
        removeFrom,
        repositoryId,
        returnVersion,
        skipCount,
        sourceFolderId,
        streamId,
        targetFolderId,
        typeId,
        unfileObjects,
        versioningState
    }

    public enum enumCollectionType : int
    {
        root,
        unfiled,
        checkedout,
        types,
        query
    }

    public enum enumLinkRelations : int
    {
        self,
        edit,
        [srs.EnumMember(Value = "edit-media")]
        editMedia,
        via,
        up,
        down,
        alternate,
        [srs.EnumMember(Value = "version-history")]
        versionHistory,
        [srs.EnumMember(Value = "current-version")]
        currentVersion,
        [srs.EnumMember(Value = "working-copy")]
        workingCopy,
        service,
        describedby,
        first,
        last,
        next,
        previous,
        [srs.EnumMember(Value = "http://docs.oasis-open.org/ns/cmis/link/200908/allowableactions")]
        CMIS_ALLOWABLEACTIONS,
        [srs.EnumMember(Value = "http://docs.oasis-open.org/ns/cmis/link/200908/relationships")]
        CMIS_RELATIONSHIPS,
        [srs.EnumMember(Value = "http://docs.oasis-open.org/ns/cmis/link/200908/source")]
        CMIS_SOURCE,
        [srs.EnumMember(Value = "http://docs.oasis-open.org/ns/cmis/link/200908/target")]
        CMIS_TARGET,
        [srs.EnumMember(Value = "http://docs.oasis-open.org/ns/cmis/link/200908/policies")]
        CMIS_POLICIES,
        [srs.EnumMember(Value = "http://docs.oasis-open.org/ns/cmis/link/200908/acl")]
        CMIS_ACL,
        [srs.EnumMember(Value = "http://docs.oasis-open.org/ns/cmis/link/200908/changes")]
        CMIS_CHANGES,
        [srs.EnumMember(Value = "http://docs.oasis-open.org/ns/cmis/link/200908/foldertree")]
        CMIS_FOLDERTREE,
        [srs.EnumMember(Value = "http://docs.oasis-open.org/ns/cmis/link/200908/typedescendants")]
        CMIS_TYPEDESCENDANTS,
        [srs.EnumMember(Value = "http://docs.oasis-open.org/ns/cmis/link/200908/rootdescendants")]
        CMIS_ROOTDESCENDANTS
    }

    public enum enumReturnVersion : int
    {
        @this,
        latest,
        latestmajor
    }

    public enum enumUriTemplateType : int
    {
        objectbyid,
        objectbypath,
        query,
        typebyid
    }
}