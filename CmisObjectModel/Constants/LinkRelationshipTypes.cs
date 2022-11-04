
// *******************************************************************************************
// * Copyright Brügmann Software GmbH, Papenburg
// * Author: Björn Kremer
// * Contact: codeplex<at>patorg.de
// * 
// * VB.CMIS is a VB.NET implementation of the Content Management Interoperability Services (CMIS) standard
// *
// * This file is part of VB.CMIS.
// * 
// * VB.CMIS is free software: you can redistribute it and/or modify
// * it under the terms of the GNU Lesser General Public License as published by
// * the Free Software Foundation, either version 3 of the License, or
// * (at your option) any later version.
// * 
// * VB.CMIS is distributed in the hope that it will be useful,
// * but WITHOUT ANY WARRANTY; without even the implied warranty of
// * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// * GNU Lesser General Public License for more details.
// * 
// * You should have received a copy of the GNU Lesser General Public License
// * along with VB.CMIS. If not, see <http://www.gnu.org/licenses/>.
// *******************************************************************************************

namespace CmisObjectModel.Constants
{
    public abstract class LinkRelationshipTypes
    {
        private LinkRelationshipTypes()
        {
        }

        public const string Alternate = "alternate";
        public const string CurrentVersion = "current-version";
        public const string DescribedBy = "describedby";
        public const string Down = "down";
        public const string Edit = "edit";
        public const string EditMedia = "edit-media";
        public const string Enclosure = "enclosure";
        public const string First = "first";
        public const string Last = "last";
        public const string Next = "next";
        public const string Previous = "previous";
        public const string Self = "self";
        public const string Service = "service";
        public const string Up = "up";
        public const string Via = "via";
        public const string VersionHistory = "version-history";
        public const string WorkingCopy = "working-copy";

        public const string Acl = "http://docs.oasis-open.org/ns/cmis/link/200908/acl";
        public const string AllowableActions = "http://docs.oasis-open.org/ns/cmis/link/200908/allowableactions";
        public const string Changes = "http://docs.oasis-open.org/ns/cmis/link/200908/changes";
        public const string FolderTree = "http://docs.oasis-open.org/ns/cmis/link/200908/foldertree";
        public const string Policies = "http://docs.oasis-open.org/ns/cmis/link/200908/policies";
        public const string Relationships = "http://docs.oasis-open.org/ns/cmis/link/200908/relationships";
        public const string RootDescendants = "http://docs.oasis-open.org/ns/cmis/link/200908/rootdescendants";
        public const string Source = "http://docs.oasis-open.org/ns/cmis/link/200908/source";
        public const string Target = "http://docs.oasis-open.org/ns/cmis/link/200908/target";
        public const string TypeDescendants = "http://docs.oasis-open.org/ns/cmis/link/200908/typedescendants";

        // link to the content-stream of a document
        public const string ContentStream = ServiceURIs.GetContentStream;
    }
}