
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
namespace CmisObjectModel.Core
{
    public enum enumACLPropagation : int
    {
        repositorydetermined,
        objectonly,
        propagate
    }

    public enum enumAllowableActionsKey : int
    {
        [srs.EnumMember(Value = "canGetDescendents.Folder")]
        canGetDescendentsFolder,
        [srs.EnumMember(Value = "canGetChildren.Folder")]
        canGetChildrenFolder,
        [srs.EnumMember(Value = "canGetParents.Folder")]
        canGetParentsFolder,
        [srs.EnumMember(Value = "canGetFolderParent.Object")]
        canGetFolderParentObject,
        [srs.EnumMember(Value = "canCreateDocument.Folder")]
        canCreateDocumentFolder,
        [srs.EnumMember(Value = "canCreateFolder.Folder")]
        canCreateFolderFolder,
        [srs.EnumMember(Value = "canCreateRelationship.Source")]
        canCreateRelationshipSource,
        [srs.EnumMember(Value = "canCreateRelationship.Target")]
        canCreateRelationshipTarget,
        [srs.EnumMember(Value = "canGetProperties.Object")]
        canGetPropertiesObject,
        [srs.EnumMember(Value = "canViewContent.Object")]
        canViewContentObject,
        [srs.EnumMember(Value = "canUpdateProperties.Object")]
        canUpdatePropertiesObject,
        [srs.EnumMember(Value = "canMove.Object")]
        canMoveObject,
        [srs.EnumMember(Value = "canMove.Target")]
        canMoveTarget,
        [srs.EnumMember(Value = "canMove.Source")]
        canMoveSource,
        [srs.EnumMember(Value = "canDelete.Object")]
        canDeleteObject,
        [srs.EnumMember(Value = "canDeleteTree.Folder")]
        canDeleteTreeFolder,
        [srs.EnumMember(Value = "canSetContent.Document")]
        canSetContentDocument,
        [srs.EnumMember(Value = "canDeleteContent.Document")]
        canDeleteContentDocument,
        [srs.EnumMember(Value = "canAddToFolder.Object")]
        canAddToFolderObject,
        [srs.EnumMember(Value = "canAddToFolder.Folder")]
        canAddToFolderFolder,
        [srs.EnumMember(Value = "canRemoveFromFolder.Object")]
        canRemoveFromFolderObject,
        [srs.EnumMember(Value = "canRemoveFromFolder.Folder")]
        canRemoveFromFolderFolder,
        [srs.EnumMember(Value = "canCheckout.Document")]
        canCheckoutDocument,
        [srs.EnumMember(Value = "canCancelCheckout.Document")]
        canCancelCheckoutDocument,
        [srs.EnumMember(Value = "canCheckin.Document")]
        canCheckinDocument,
        [srs.EnumMember(Value = "canGetAllVersions.VersionSeries")]
        canGetAllVersionsVersionSeries,
        [srs.EnumMember(Value = "canGetObjectRelationships.Object")]
        canGetObjectRelationshipsObject,
        [srs.EnumMember(Value = "canAddPolicy.Object")]
        canAddPolicyObject,
        [srs.EnumMember(Value = "canAddPolicy.Policy")]
        canAddPolicyPolicy,
        [srs.EnumMember(Value = "canRemovePolicy.Object")]
        canRemovePolicyObject,
        [srs.EnumMember(Value = "canRemovePolicy.Policy")]
        canRemovePolicyPolicy,
        [srs.EnumMember(Value = "canGetAppliedPolicies.Object")]
        canGetAppliedPoliciesObject,
        [srs.EnumMember(Value = "canGetACL.Object")]
        canGetACLObject,
        [srs.EnumMember(Value = "canApplyACL.Object")]
        canApplyACLObject
    }

    public enum enumBaseObjectTypeIds : int
    {
        [srs.EnumMember(Value = "cmis:document")]
        cmisDocument,
        [srs.EnumMember(Value = "cmis:folder")]
        cmisFolder,
        [srs.EnumMember(Value = "cmis:relationship")]
        cmisRelationship,
        [srs.EnumMember(Value = "cmis:policy")]
        cmisPolicy,
        [srs.EnumMember(Value = "cmis:item")]
        cmisItem,
        [srs.EnumMember(Value = "cmis:secondary")]
        cmisSecondary
    }

    public enum enumBasicPermissions : int
    {
        [srs.EnumMember(Value = "cmis:read")]
        cmisRead,
        [srs.EnumMember(Value = "cmis:write")]
        cmisWrite,
        [srs.EnumMember(Value = "cmis:all")]
        cmisAll
    }

    public enum enumCapabilityACL : int
    {
        none,
        discover,
        manage
    }

    public enum enumCapabilityChanges : int
    {
        none,
        objectidsonly,
        properties,
        all
    }

    public enum enumCapabilityContentStreamUpdates : int
    {
        anytime,
        pwconly,
        none
    }

    public enum enumCapabilityJoin : int
    {
        none,
        inneronly,
        innerandouter
    }

    public enum enumCapabilityOrderBy : int
    {
        none,
        common,
        custom
    }

    public enum enumCapabilityQuery : int
    {
        none,
        metadataonly,
        fulltextonly,
        bothseparate,
        bothcombined
    }

    public enum enumCapabilityRendition : int
    {
        none,
        read
    }

    public enum enumCardinality : int
    {
        single,
        multi
    }

    public enum enumContentStreamAllowed : int
    {
        notallowed,
        allowed,
        required
    }

    public enum enumDateTimeResolution : int
    {
        year,
        date,
        time
    }

    public enum enumDecimalPrecision : long
    {
        [srs.EnumMember(Value = "32")]
        single = 32L,
        [srs.EnumMember(Value = "64")]
        @double = 64L
    }

    public enum enumIncludeRelationships : int
    {
        none,
        source,
        target,
        both
    }

    public enum enumPropertiesBase : int
    {
        [srs.EnumMember(Value = "cmis:name")]
        cmisName,
        [srs.EnumMember(Value = "cmis:description")]
        cmisDescription,
        [srs.EnumMember(Value = "cmis:objectId")]
        cmisObjectId,
        [srs.EnumMember(Value = "cmis:objectTypeId")]
        cmisObjectTypeId,
        [srs.EnumMember(Value = "cmis:baseTypeId")]
        cmisBaseTypeId,
        [srs.EnumMember(Value = "cmis:secondaryObjectTypeIds")]
        cmisSecondaryObjectTypeIds,
        [srs.EnumMember(Value = "cmis:createdBy")]
        cmisCreatedBy,
        [srs.EnumMember(Value = "cmis:creationDate")]
        cmisCreationDate,
        [srs.EnumMember(Value = "cmis:lastModifiedBy")]
        cmisLastModifiedBy,
        [srs.EnumMember(Value = "cmis:lastModificationDate")]
        cmisLastModificationDate,
        [srs.EnumMember(Value = "cmis:changeToken")]
        cmisChangeToken
    }

    public enum enumPropertiesDocument : int
    {
        [srs.EnumMember(Value = "cmis:isImmutable")]
        cmisIsImmutable,
        [srs.EnumMember(Value = "cmis:isLatestVersion")]
        cmisIsLatestVersion,
        [srs.EnumMember(Value = "cmis:isMajorVersion")]
        cmisIsMajorVersion,
        [srs.EnumMember(Value = "cmis:isLatestMajorVersion")]
        cmisIsLatestMajorVersion,
        [srs.EnumMember(Value = "cmis:isPrivateWorkingCopy")]
        cmisIsPrivateWorkingCopy,
        [srs.EnumMember(Value = "cmis:versionLabel")]
        cmisVersionLabel,
        [srs.EnumMember(Value = "cmis:versionSeriesId")]
        cmisVersionSeriesId,
        [srs.EnumMember(Value = "cmis:isVersionSeriesCheckedOut")]
        cmisIsVersionSeriesCheckedOut,
        [srs.EnumMember(Value = "cmis:versionSeriesCheckedOutBy")]
        cmisVersionSeriesCheckedOutBy,
        [srs.EnumMember(Value = "cmis:versionSeriesCheckedOutId")]
        cmisVersionSeriesCheckedOutId,
        [srs.EnumMember(Value = "cmis:checkinComment")]
        cmisCheckinComment,
        [srs.EnumMember(Value = "cmis:contentStreamLength")]
        cmisContentStreamLength,
        [srs.EnumMember(Value = "cmis:contentStreamMimeType")]
        cmisContentStreamMimeType,
        [srs.EnumMember(Value = "cmis:contentStreamFileName")]
        cmisContentStreamFileName,
        [srs.EnumMember(Value = "cmis:contentStreamId")]
        cmisContentStreamId
    }

    public enum enumPropertiesFolder : int
    {
        [srs.EnumMember(Value = "cmis:parentId")]
        cmisParentId,
        [srs.EnumMember(Value = "cmis:allowedChildObjectTypeIds")]
        cmisAllowedChildObjectTypeIds,
        [srs.EnumMember(Value = "cmis:path")]
        cmisPath
    }

    public enum enumPropertiesPolicy : int
    {
        [srs.EnumMember(Value = "cmis:policyText")]
        cmisPolicyText
    }

    public enum enumPropertiesRelationship : int
    {
        [srs.EnumMember(Value = "cmis:sourceId")]
        cmisSourceId,
        [srs.EnumMember(Value = "cmis:targetId")]
        cmisTargetId
    }

    public enum enumPropertyType : int
    {
        boolean,
        id,
        integer,
        datetime,
        @decimal,
        html,
        @string,
        uri
    }

    public enum enumRelationshipDirection : int
    {
        source,
        target,
        either
    }

    public enum enumRenditionKind : int
    {
        [srs.EnumMember(Value = "cmis:thumbnail")]
        cmisThumbnail
    }

    public enum enumSupportedPermissions : int
    {
        basic,
        repository,
        both
    }

    public enum enumTypeOfChanges : int
    {
        created,
        updated,
        deleted,
        security
    }

    public enum enumUnfileObject : int
    {
        unfile,
        deletesinglefiled,
        delete
    }

    public enum enumUpdatability : int
    {
        @readonly,
        readwrite,
        whencheckedout,
        oncreate
    }

    public enum enumUsers : int
    {
        [srs.EnumMember(Value = "cmis:user")]
        cmisUser
    }

    public enum enumVersioningState : int
    {
        none,
        checkedout,
        minor,
        major
    }
}