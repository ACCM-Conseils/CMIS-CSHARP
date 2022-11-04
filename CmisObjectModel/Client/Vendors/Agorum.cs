
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
using CmisObjectModel.Constants;
using CmisObjectModel.Core.Collections;
using Microsoft.VisualBasic.CompilerServices;

namespace CmisObjectModel.Client.Vendors
{
    public class Agorum : Vendor
    {

        #region Constructors
        public Agorum(Contracts.ICmisClient client) : base(client)
        {
        }
        #endregion

        #region Patches
        /// <summary>
      /// In agorum the cmis:versionSeriesId property of the pwc differs from the cmis:versionSeriesId
      /// of the checkedin-versions of the document.
      /// </summary>
      /// <param name="properties"></param>
        public override void PatchProperties(Core.cmisRepositoryInfoType repositoryInfo, cmisPropertiesType properties)
        {
            if (properties is not null)
            {
                try
                {
                    {
                        var withBlock = properties.GetProperties(CmisPredefinedPropertyNames.ObjectId, CmisPredefinedPropertyNames.VersionSeriesCheckedOutId, CmisPredefinedPropertyNames.VersionSeriesId);
                        if (withBlock.Count == 3)
                        {
                            var versionSeriesIdProperty = withBlock[CmisPredefinedPropertyNames.VersionSeriesId];
                            string objectId = Conversions.ToString(withBlock[CmisPredefinedPropertyNames.ObjectId].Value);
                            // check if properties belong to a pwc
                            if (!string.IsNullOrEmpty(objectId) && string.Compare(objectId, Conversions.ToString(withBlock[CmisPredefinedPropertyNames.VersionSeriesCheckedOutId].Value)) == 0)
                            {
                                string filter = string.Join(",", CmisPredefinedPropertyNames.ObjectId, CmisPredefinedPropertyNames.VersionSeriesId);

                                {
                                    var withBlock1 = _client.GetObjectOfLatestVersion(new Messaging.Requests.getObjectOfLatestVersion() { Filter = filter, ObjectId = objectId, RepositoryId = repositoryInfo.RepositoryId });
                                    if (withBlock1.Exception is null && withBlock1.Response is not null)
                                    {
                                        // replace value of cmis:versionSeriesId property of pwc with the
                                        // value returned by the last checkedin version of the document
                                        string versionSeriesId = withBlock1.Response.Object.VersionSeriesId.Value;
                                        if (!string.IsNullOrEmpty(versionSeriesId))
                                            versionSeriesIdProperty.Value = versionSeriesId;
                                    }
                                }
                            }
                        }
                    }
                }
                catch
                {
                }
            }
            base.PatchProperties(repositoryInfo, properties);
        }
        #endregion

    }
}