using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using CmisObjectModel.Client;
// Diese Datei enthält alle Funktionen, die vom CmisObjectModel aufgerufen werden.

using CmisObjectModel.Common.Generic;
using CmisObjectModel.Core;
using CmisObjectModel.Core.Collections;
using CmisObjectModel.Core.Definitions.Types;
using CmisObjectModel.Core.Properties;
using CmisObjectModel.Core.Security;
using CmisObjectModel.Messaging;
using CmisObjectModel.Messaging.Responses;
using CmisObjectModel.RestAtom;
using CmisObjectModel.ServiceModel;
using DocuWare.Platform.ServerClient;
using DocuWare.Services.Http;
using Microsoft.VisualBasic.CompilerServices;
using Microsoft.VisualBasic.Logging;

namespace CmisServer
{

    /// <summary>
/// Demo-Implementierung eines CmisServers
/// </summary>
/// <remarks>
/// CMIS-Standard 1.1 http://docs.oasis-open.org/cmis/CMIS/v1.1/os/CMIS-v1.1-os.html
/// </remarks>
    public partial class CmisServiceImpl : CmisServiceImplBase
    {
        private static ServiceConnection conn = null;
        private static Organization org = null;
        private static string currentRepository = null;

        #region Logging and Errors

        protected override void LogException(Exception ex, System.Reflection.MethodBase method)
        {
            System.ServiceModel.FaultException<cmisFaultType> cmisFault = ex as System.ServiceModel.FaultException<cmisFaultType>;
            if (cmisFault is not null)
            {
                ErrorLog_Internal(method.Name, ex.Message, cmisFault.Detail.Message);
            }
            else if (ex.InnerException is not null)
            {
                ErrorLog_Internal(method.Name, ex.Message, ex.InnerException.Message);
            }
            else
            {
                ErrorLog_Internal(method.Name, ex.Message, ex.GetType().ToString());
            }
        }

        #endregion

        #region Identity

        protected override System.ServiceModel.Syndication.SyndicationPerson GetSystemAuthor()
        {
            // Log_Internal("GetSystemAuthor")

            return SystemAuthor_Internal;
        }

        /// <remarks>
   /// Für die Browser-Binding ist die Validirung über diese Funktion deaktiviert. Bitte managen Sie 
   /// Anmeldetokens und verwenden Sie die Transportsicherheit mittels SSL-Verschlüsselung.
   /// Siehe: http://docs.oasis-open.org/cmis/CMIS/v1.1/errata01/os/CMIS-v1.1-errata01-os-complete.html#x1-5470009
   /// </remarks>
        public override bool ValidateUserNamePassword(string userName, string password)
        {
            try
            {
                Log_Internal("ValidateUserNamePassword");

                ServicePointManager.ServerCertificateValidationCallback += (send, cert, chain, sslPolicyErrors) => true;

                System.Net.ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                Log_Internal(ConfigurationManager.AppSettings["URLDocuware"]);

                conn = Helpers.Docuware.Connect(userName, password);

                if (conn != null)
                {
                    Log_Internal("Connexion OK");

                    org = Helpers.Docuware.GetOrganization(conn);

                    Log_Internal("Org OK");

                    return true;
                }
                else
                {
                    Log_Internal("Connexion en erreur");

                    return false;
                }
            }
            catch (Exception ex)
            {
                Log_Internal(ex.Message);

                return false;
            }
        }

        #endregion

        #region Repository

        protected override Result<cmisRepositoryInfoType[]> GetRepositories()
        {
            Log_Internal("GetRepositories");

            Organization org = Helpers.Docuware.GetOrganization(conn);

            UserInfo myUser = org.GetUserInfoFromUserInfoRelation();

            var fileCabinets = org.GetFileCabinetsFromFilecabinetsRelation().FileCabinet.Where(m => !m.IsBasket);

            cmisRepositoryInfoType[] repos = new cmisRepositoryInfoType[fileCabinets.Count()];

            var i = 0;

            foreach (FileCabinet cab in fileCabinets)
            {
                cmisRepositoryInfoType _repository = new cmisRepositoryInfoType();

                _repository.RepositoryId = cab.Id;
                _repository.ProductName = "CMIS Server Docuware";
                _repository.ProductVersion = "1.0";
                _repository.VendorName = "Altexence";
                _repository.RepositoryName = cab.Name;
                _repository.RepositoryDescription = cab.Name + " (" + _repoid + ")";
                _repository.RootFolderId = "root";
                _repository.CmisVersionSupported = "1.1";
                _repository.RepositoryUrl = BaseUri.ToString() + _repoid;

                _repository.PrincipalAnonymous = "guest";
                _repository.PrincipalAnyone = "GROUP_EVERYONE";

                _repository.Capabilities = new cmisRepositoryCapabilitiesType();
                _repository.Capabilities.CapabilityPWCUpdatable = true;

                repos[i] = _repository;

                i++;
            }

            return repos;
        }

        public override Result<cmisRepositoryInfoType> GetRepositoryInfo(string repositoryId)
        {
            currentRepository = repositoryId;

            Log_Internal("GetRepositoryInfo", repositoryId);

            return get_RepositoryInfo(repositoryId);
        }

        public override Result<cmisTypeDefinitionType> GetTypeDefinition(string repositoryId, string typeId)
        {
            Log_Internal("GetTypeDefinition", typeId);

            return TypeDefinition(repositoryId, typeId);
        }

        public override cmisTypeDefinitionType TypeDefinition(string repositoryId, string typeId)
        {
            return TypeDefinition_Internal(typeId);
        }

        public override CmisObjectModel.Core.cmisRepositoryInfoType get_RepositoryInfo(string repositoryId)
        {
            if (!String.IsNullOrEmpty(repositoryId))
            {
                Organization org = Helpers.Docuware.GetOrganization(conn);

                UserInfo myUser = org.GetUserInfoFromUserInfoRelation();

                FileCabinet defaultBasket = org.GetFileCabinetsFromFilecabinetsRelation().FileCabinet.FirstOrDefault(m => m.Id == repositoryId);

                _repository = new cmisRepositoryInfoType();

                _repoid = repositoryId;
                _repository.RepositoryId = repositoryId;
                _repository.ProductName = defaultBasket.Name;
                _repository.ProductVersion = "1.0";
                _repository.VendorName = "CMIS Docuware Altexence";
                _repository.RepositoryName = defaultBasket.Name;
                _repository.RepositoryDescription = defaultBasket.Name + " (" + _repoid + ")";
                _repository.RootFolderId = "root";
                _repository.CmisVersionSupported = "1.1";
                _repository.RepositoryUrl = BaseUri.ToString() + repositoryId;

                _repository.PrincipalAnonymous = "guest";
                _repository.PrincipalAnyone = "GROUP_EVERYONE";

                _repository.Capabilities = new cmisRepositoryCapabilitiesType();
                _repository.Capabilities.CapabilityPWCUpdatable = true;
                _repository.Capabilities.CapabilityGetDescendants = false;
                _repository.Capabilities.CapabilityQuery = enumCapabilityQuery.metadataonly;

                return _repository;
            }
            else
                throw new Exception("Repository " + repositoryId + " not exists. Use " + _repoid);
        }

        #endregion

        #region TypeDefinition

        protected override Result<cmisTypeDefinitionListType> GetTypeChildren(string repositoryId, string typeId, bool includePropertyDefinitions, long? maxItems, long? skipCount)
        {
            Log_Internal("GetTypeChildren", typeId);
            
            var list = new cmisTypeDefinitionListType();
            list.NumItems = 0;
            return list;
        }

        protected override Result<cmisTypeContainer> GetTypeDescendants(string repositoryId, string typeId, bool includePropertyDefinitions, long? depth)
        {
            Log_Internal("GetTypeDescendants", typeId);

            if (string.IsNullOrEmpty(typeId))
            {
                return new cmisTypeContainer() { Children = new cmisTypeContainer[] { new cmisTypeContainer() { Type = TypeDefinition_Internal("cmis:folder") }, new cmisTypeContainer() { Type = TypeDefinition_Internal("cmis:document") } } };
            }
            else
            {
                return new cmisTypeContainer() { Children = new cmisTypeContainer[] { } };
            }
        }

        protected override string GetParentTypeId(string repositoryId, string typeId)
        {
            return null;
        }

        #endregion

        #region Navigation

        protected override Result<CmisObjectModel.ServiceModel.cmisObjectInFolderListType> GetChildren(string repositoryId, string folderId, long? maxItems, long? skipCount, string filter, bool? includeAllowableActions, enumIncludeRelationships? includeRelationships, string renditionFilter, string orderBy, bool includePathSegment)
        {
            Log_Internal("GetChildren", folderId);

            var children = new List<CmisObjectModel.ServiceModel.cmisObjectInFolderType>();

            DocumentsQueryResult queryResult = conn.GetFromDocumentsForDocumentsQueryResultAsync(
                repositoryId,
                count: (int)1000)
                .Result;

            CmisObjectModel.ServiceModel.cmisObjectType[] liste = new CmisObjectModel.ServiceModel.cmisObjectType[queryResult.Items.Count];
            Document[] listeDocuware = queryResult.Items.ToArray();

            for(int i=0; i < listeDocuware.Length; i++)
            {
                try
                {
                    liste[i] = get_Object_InternalFromDocuware(listeDocuware[i]);

                    children.Add(new CmisObjectModel.ServiceModel.cmisObjectInFolderType()
                    {
                        Object = new CmisObjectModel.ServiceModel.cmisObjectInFolderType()
                        {
                            Object = liste[i]
                        }
                    });
                }
                catch(Exception e)
                {

                }
            }

            var list = new CmisObjectModel.ServiceModel.cmisObjectInFolderListType();
            list.Objects = children.ToArray();
            list.NumItems = list.Count();
            return list;
        }

        protected override Result<CmisObjectModel.ServiceModel.cmisObjectType> GetFolderParent(string repositoryId, string folderId, string filter)
        {
            Log_Internal("GetFolderParent", folderId);

            string parentFolderId = get_ParentFolderId_Internal(folderId);
            return get_Object_Internal(parentFolderId);
        }

        protected override Result<CmisObjectModel.ServiceModel.cmisObjectParentsType[]> GetObjectParents(string repositoryId, string objectId, string filter, bool? includeAllowableActions, enumIncludeRelationships? includeRelationships, string renditionFilter, bool? includeRelativePathSegment)
        {
            Log_Internal("GetObjectParents", objectId);

            string parentFolderId = get_ParentFolderId_Internal(objectId);
            var parentFolder = new CmisObjectModel.ServiceModel.cmisObjectParentsType()
            {
                Object = get_Object_Internal(parentFolderId),
                RelativePathSegment = objectId.Split('\\').Last()
            };
            return new CmisObjectModel.ServiceModel.cmisObjectParentsType[] { parentFolder };
        }

        #endregion

        #region Object

        public override bool get_Exists(string repositoryId, string objectId)
        {
            Log_Internal("Exists", objectId);

            return get_Exists_Internal(objectId);
        }

        protected override Result<CmisObjectModel.ServiceModel.cmisObjectType> GetObject(string repositoryId, string objectId, string filter, enumIncludeRelationships? includeRelationships, bool? includePolicyIds, string renditionFilter, bool? includeACL, bool? includeAllowableActions, enumReturnVersion? returnVersion, bool? privateWorkingCopy)
        {
            Log_Internal("GetObject", objectId, returnVersion.ToString());

            var obj = get_Object_Internal(objectId);

            if (returnVersion > enumReturnVersion.@this)
            {
                // GetObjectOfLatesVersion

                var versions = GetAllVersions_Internal(objectId);
                if (returnVersion == enumReturnVersion.latestmajor)
                {
                    // Latest major version

                    var last = from version in versions.Objects
                               where (bool)version.IsLatestMajorVersion && returnVersion == enumReturnVersion.latestmajor
                               select version;
                    if (last.Count() > 0)
                        obj = last.First();
                    else
                        throw new Exception("No Major Version");
                }
                else if (returnVersion == enumReturnVersion.latest)
                {
                    // Latest version

                    obj = (CmisObjectModel.ServiceModel.cmisObjectType)versions.First();
                }
            }

            return obj;
        }

        protected override string GetObjectId(string repositoryId, string path)
        {
            Log_Internal("GetObjectId", path);

            if ("/".Equals(path))
            {
                return "root";
            }
            else
            {
                string objectId = path.Substring(1).Replace("/", @"\");

                if (!get_Exists_Internal(objectId))
                {
                    throw new Exception("Object '" + objectId + "' not exists!");
                }

                return objectId;
            }
        }

        protected override Result<CmisObjectModel.ServiceModel.cmisObjectType> GetObjectByPath(string repositoryId, string path, string filter, bool? includeAllowableActions, bool? includePolicyIds, enumIncludeRelationships? includeRelationships, bool? includeACL, string renditionFilter)
        {
            Log_Internal("GetObjectByPath", path);

            string objectId = "/".Equals(path) || string.IsNullOrEmpty(path) ? "root" : path.Substring(1).Replace("/", @"\");

            return get_Object_Internal(objectId);
        }

        protected override Result<CmisObjectModel.Core.cmisAllowableActionsType> GetAllowableActions(string repositoryId, string id)
        {
            Log_Internal("GetAllowableActions", id);

            var obj = get_Object_Internal(id);
            return obj.AllowableActions;
        }

        #endregion

        #region Properties

        protected override Result<CmisObjectModel.ServiceModel.cmisObjectType> UpdateProperties(string repositoryId, string objectId, cmisPropertiesType properties, string changeToken)
        {
            Log_Internal("UpdateProperties", objectId);

            UpdateProperties_Internal(objectId, properties, changeToken);

            return get_Object_Internal(objectId);
        }

        #endregion

        #region Versionen

        protected override Result<CmisObjectModel.ServiceModel.cmisObjectListType> GetAllVersions(string repositoryId, string objectId, string versionSeriesId, string filter, bool? includeAllowableActions)
        {
            Log_Internal("GetAllVersions", objectId, versionSeriesId);

            var versions = GetAllVersions_Internal(objectId);

            return versions;
        }

        #endregion

        #region CheckOut/CheckIn

        protected override Result<CmisObjectModel.ServiceModel.cmisObjectType> CheckOut(string repositoryId, string objectId)
        {
            Log_Internal("CheckOut", objectId);

            string pwcId = CheckOut_Internal(objectId);

            return get_Object_Internal(pwcId);
        }

        protected override Exception CancelCheckOut(string repositoryId, string objectId)
        {
            Log_Internal("CancelCheckOut", objectId);

            CancelCheckOut_Internal(objectId);

            return null;
        }

        protected override Result<CmisObjectModel.ServiceModel.cmisObjectType> CheckIn(string repositoryId, string objectId, CmisObjectModel.Core.Collections.cmisPropertiesType properties, string[] policies, cmisContentStreamType content, bool major, string checkInComment, cmisAccessControlListType addACEs = default, cmisAccessControlListType removeACEs = default)
        {
            Log_Internal("CheckIn", objectId);

            string checkedInId = CheckIn_Internal(objectId, properties, policies, content, major, checkInComment, addACEs, removeACEs);

            return get_Object_Internal(checkedInId);
        }

        #endregion

        #region Content

        protected override Result<CmisObjectModel.Messaging.cmisContentStreamType> GetContentStream(string repositoryId, string objectId, string streamId)
        {
            Log_Internal("GetContentStream", objectId);

            var fileCabinets = org.GetFileCabinetsFromFilecabinetsRelation().FileCabinet;

            var defaultBasket = fileCabinets.FirstOrDefault(f => !f.IsBasket && f.Id == repositoryId);

            var dialogInfoItems = defaultBasket.GetDialogInfosFromSearchesRelation();
            var dialog = dialogInfoItems.Dialog.FirstOrDefault(m => m.IsDefault).GetDialogFromSelfRelation();

            var q = new DialogExpression()
            {
                Condition = new List<DialogExpressionCondition>()
                        {
                            DialogExpressionCondition.Create("DWDOCID", objectId )
                        },
                Count = 1
            };

            var queryResult = dialog.Query.PostToDialogExpressionRelationForDocumentsQueryResult(q);

            var downloadedFile = Helpers.Docuware.DownloadDocumentContent(queryResult.Items.First());

                var content = new cmisContentStreamType(downloadedFile.Stream, downloadedFile.FileName, downloadedFile.ContentType);

                return content;
        }

        protected override Result<setContentStreamResponse> SetContentStream(string repositoryId, string objectId, System.IO.Stream contentStream, string mimeType, string fileName, bool overwriteFlag, string changeToken)
        {
            Log_Internal("SetContentStream", objectId, fileName, mimeType);

            var lastModificationDate = SetContentStream_Internal(objectId, contentStream, mimeType, fileName, overwriteFlag, changeToken);

            return new setContentStreamResponse(objectId, lastModificationDate.ToString(), enumSetContentStreamResult.Created);
        }

        #endregion

        #region Create/Delete

        protected override Result<CmisObjectModel.ServiceModel.cmisObjectType> CreateFolder(string repositoryId, CmisObjectModel.Core.cmisObjectType newFolder, string parentFolderId, cmisAccessControlListType addACEs, cmisAccessControlListType removeACEs)
        {
            Log_Internal("CreateFolder", newFolder.Properties.GetProperties("cmis:name").First().Value.Value.ToString(), parentFolderId);

            string name = newFolder.Properties.GetProperties("cmis:name").First().Value.Value.ToString();
            string description = null;
            if (newFolder.Properties.GetProperties("cmis:description").Count > 0)
            {
                description = newFolder.Properties.GetProperties("cmis:description").First().Value.Value.ToString();
            }

            string path;
            string objectId;
            if ("root".Equals(parentFolderId))
            {
                path = System.IO.Path.Combine(_folder, name);
                objectId = name;
            }
            else
            {
                path = System.IO.Path.Combine(_folder, parentFolderId, name);
                objectId = System.IO.Path.Combine(parentFolderId, name);
            }

            if (get_Exists_Internal(objectId))
            {
                throw new Exception("Object '" + objectId + "' exists!");
            }

            System.IO.Directory.CreateDirectory(path);

            return get_Object_Internal(objectId);
        }

        protected override Result<CmisObjectModel.ServiceModel.cmisObjectType> CreateDocument(string repositoryId, CmisObjectModel.Core.cmisObjectType newDocument, string folderId, cmisContentStreamType content, enumVersioningState? versioningState, cmisAccessControlListType addACEs, cmisAccessControlListType removeACEs)
        {
            Log_Internal("CreateDocument", folderId, versioningState.Value.ToString());

            // 1. Eigenschaften

            string name = newDocument.Name;
            string description = newDocument.Description;

            List<DocumentIndexField> metavalues = new List<DocumentIndexField>();

            if (newDocument.GetProperties("docuware:metavalues").Count > 0)
            {
                object[] metas = newDocument.GetProperties("docuware:metavalues").First().Value.Values;

                foreach(object s in metas)
                {
                    String[] metavals = System.Convert.ToString(s).Split('=');

                    metavalues.Add(DocumentIndexField.Create(metavals[0], metavals[1]));
                }
            }

            string mimeType = null;
            if (content is not null && !string.IsNullOrWhiteSpace(content.MimeType))
            {
                mimeType = content.MimeType;
            }
            else
            {
                mimeType = Helper.get_MimeType(name);
            }

            if (string.IsNullOrEmpty(name))
            {
                if (content is not null && !string.IsNullOrWhiteSpace(content.Filename) && content.Filename.Contains("."))
                {
                    name = content.Filename;
                }
                else
                {
                    name = Guid.NewGuid().ToString("N");
                }
            }

            // 2. Object

            string path;
            string originalObjectId;
            if ("root".Equals(folderId))
            {
                path = System.IO.Path.Combine(_folder, name);
                originalObjectId = name;
            }
            else
            {
                path = System.IO.Path.Combine(_folder, folderId, name);
                originalObjectId = System.IO.Path.Combine(folderId, name);
            }

            /*if (get_Exists_Internal(originalObjectId))
            {
                throw new Exception("Object '" + originalObjectId + "' already exists!");
            }*/

            if(!System.IO.Directory.Exists(System.IO.Path.Combine(_folder, folderId)))
                System.IO.Directory.CreateDirectory(System.IO.Path.Combine(_folder, folderId));

            /*if (!(versioningState == enumVersioningState.checkedout))
            {
                System.IO.Directory.CreateDirectory(System.IO.Path.Combine(path, "Versionen"));
            }*/

            // 3. Metadaten

            var meta = new Metadata();
            meta.CreatedBy = CurrentAuthenticationInfo.User;
            meta.CreationDate = DateTime.Now.ToUniversalTime();
            meta.LastModifiedBy = CurrentAuthenticationInfo.User;
            meta.LastModificationDate = DateTime.Now.ToUniversalTime();
            meta.MimeType = mimeType;

            meta.MajorOfLatestVersion = versioningState == enumVersioningState.major ? 1 : 0;
            meta.MinorOfLatestVersion = versioningState == enumVersioningState.minor ? 1 : 0;

            if (versioningState == enumVersioningState.checkedout)
            {
                meta.VersionSeriesCheckedOutBy = CurrentAuthenticationInfo.User;
                meta.DescriptionPwc = description;
                //meta.AktePwc = akte;
            }
            else
            {
                meta.Description = description;
                //meta.Akte = akte;
                meta.AddComment("create");
            }

            string xml = Conversions.ToString(meta.ToXml());
            /*System.IO.File.WriteAllText(System.IO.Path.Combine(path, "metadata"), xml);

            // 4. Content

            string contentPath;
            if (versioningState == enumVersioningState.checkedout)
            {
                contentPath = System.IO.Path.Combine(path, "pwc");
            }
            else
            {
                contentPath = System.IO.Path.Combine(path, "Versionen", meta.LabelOfLatestVersion);
            }*/

            if (content is not null && content.BinaryStream is not null)
            {
                string filToCreate = System.IO.Path.Combine(_folder, folderId, name);

                System.IO.File.WriteAllBytes(filToCreate, ReadFully(content.BinaryStream));
            }

            // 5. Rückgabewert

            string objectId = originalObjectId + (versioningState == enumVersioningState.checkedout ? ";pwc" : string.Empty);

            if (versioningState == enumVersioningState.checkedout)
            {
                // Damit die angelegte Arbeitskopie über die versionSeriesId auffindbar ist, muss es eine Version in der Versionsserie geben.
                // Deshalb hier ein CheckIn und danach ein CheckOut, damit eine Version existiert und der Status wieder 'checkedout' ist.
                // (Beachte auch CmisServiceImpl_Internal.CancleCheckOut)
                string checkedInId = CheckIn_Internal(objectId, default, default, default, false, "enumVersioningState.checkedout");
                objectId = CheckOut_Internal(checkedInId);
            }

            var fileCabinet = org.GetFileCabinetsFromFilecabinetsRelation().FileCabinet.FirstOrDefault(m => m.Id == folderId);

            var indexData = new Document()
            {
                Fields = metavalues
            };

            var uploadedDocument = fileCabinet.UploadDocument(indexData, new FileInfo(path));

            return get_Object_Internal(uploadedDocument.Id.ToString());
        }

        public static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }


        protected override Exception DeleteObject(string repositoryId, string objectId, bool allVersions)
        {
            Log_Internal("DeleteObject", objectId, Conversions.ToString(allVersions));

            if (!get_Exists_Internal(objectId))
            {
                return new Exception("Object '" + objectId + "' not exists!");
            }
            Exception ex = null;

            var document = conn.GetFromDocumentForDocumentAsync(System.Convert.ToInt32(objectId), currentRepository).Result;

            document.Content.DeleteSelfRelation();

            return ex;
        }

        protected override Result<deleteTreeResponse> DeleteTree(string repositoryId, string folderId, bool allVersions, enumUnfileObject? unfileObjects, bool continueOnFailure)
        {
            Log_Internal("DeleteTree", folderId, Conversions.ToString(allVersions), Conversions.ToString(continueOnFailure));

            string path = System.IO.Path.Combine(_folder, folderId);

            System.IO.Directory.Delete(path, true);

            return new deleteTreeResponse(enumDeleteTreeResult.OK); // With {.FailedToDelete = New failedToDelete}
        }

        #endregion

        // ---------------------------------------------------------------------------------------------------------------------------

        #region Not Implemented

        protected override Result<CmisObjectModel.ServiceModel.cmisObjectType> AddObjectToFolder(string repositoryId, string objectId, string folderId, bool allVersions)
        {
            return NotSupported_Internal("AddObjectToFolder");
        }

        protected override Result<setContentStreamResponse> AppendContentStream(string repositoryId, string objectId, System.IO.Stream contentStream, string mimeType, string fileName, bool isLastChunk, string changeToken)
        {
            return NotSupported_Internal("AppendContentStream");
        }

        protected override Result<cmisAccessControlListType> ApplyACL(string repositoryId, string objectId, cmisAccessControlListType addACEs, cmisAccessControlListType removeACEs, enumACLPropagation aclPropagation)
        {
            return NotSupported_Internal("ApplyACL");
        }

        protected override Result<CmisObjectModel.ServiceModel.cmisObjectType> ApplyPolicy(string repositoryId, string objectId, string policyId)
        {
            return NotSupported_Internal("ApplyPolicy");
        }

        protected override Result<CmisObjectModel.ServiceModel.cmisObjectListType> BulkUpdateProperties(string repositoryId, cmisBulkUpdateType data)
        {
            return NotSupported_Internal("BulkUpdateProperties");
        }

        protected override Result<CmisObjectModel.ServiceModel.cmisObjectType> CreateDocumentFromSource(string repositoryId, string sourceId, cmisPropertiesType properties, string folderId, enumVersioningState? versioningState, string[] policies, cmisAccessControlListType addACEs, cmisAccessControlListType removeACEs)
        {
            return NotSupported_Internal("CreateDocumentFromSource");
        }

        protected override Result<CmisObjectModel.ServiceModel.cmisObjectType> CreateItem(string repositoryId, CmisObjectModel.Core.cmisObjectType newItem, string folderId, cmisAccessControlListType addACEs, cmisAccessControlListType removeACEs)
        {
            return NotSupported_Internal("CreateItem");
        }

        protected override Result<CmisObjectModel.ServiceModel.cmisObjectType> CreatePolicy(string repositoryId, CmisObjectModel.Core.cmisObjectType newPolicy, string folderId, cmisAccessControlListType addACEs, cmisAccessControlListType removeACEs)
        {
            return NotSupported_Internal("CreatePolicy");
        }

        protected override Result<CmisObjectModel.ServiceModel.cmisObjectType> CreateRelationship(string repositoryId, CmisObjectModel.Core.cmisObjectType newRelationship, cmisAccessControlListType addACEs, cmisAccessControlListType removeACEs)
        {
            return NotSupported_Internal("CreateRelationship");
        }

        protected override Result<cmisTypeDefinitionType> CreateType(string repositoryId, cmisTypeDefinitionType newType)
        {
            return NotSupported_Internal("CreateType");
        }

        protected override Result<deleteContentStreamResponse> DeleteContentStream(string repositoryId, string objectId, string changeToken)
        {
            return NotSupported_Internal("DeleteContentStream");
        }

        protected override Exception DeleteType(string repositoryId, string typeId)
        {
            NotSupported_Internal("DeleteType");
            throw new NotImplementedException("DeleteType");
        }

        protected override Result<cmisAccessControlListType> GetACL(string repositoryId, string objectId, bool onlyBasicPermissions)
        {
            return NotSupported_Internal("GetACL");
        }

        protected override Result<CmisObjectModel.ServiceModel.cmisObjectListType> GetAppliedPolicies(string repositoryId, string objectId, string filter)
        {
            return NotSupported_Internal("GetAppliedPolicies");
        }

        protected override enumBaseObjectTypeIds GetBaseObjectType(string repositoryId, string objectId)
        {
            NotSupported_Internal("GetBaseObjectType");
            throw new NotImplementedException("GetBaseObjectType");
        }

        protected override Result<CmisObjectModel.ServiceModel.cmisObjectListType> GetCheckedOutDocs(string repositoryId, string folderId, string filter, long? maxItems, long? skipCount, string renditionFilter, bool? includeAllowableActions, enumIncludeRelationships? includeRelationships)
        {
            return NotSupported_Internal("GetCheckedOutDocs");
        }

        protected override Result<getContentChanges> GetContentChanges(string repositoryId, string filter, long? maxItems, bool? includeACL, bool includePolicyIds, bool includeProperties, ref string changeLogToken)
        {
            return NotSupported_Internal("GetContentChanges");
        }

        protected override Result<CmisObjectModel.ServiceModel.cmisObjectInFolderContainerType> GetDescendants(string repositoryId, string folderId, string filter, long? depth, bool? includeAllowableActions, enumIncludeRelationships? includeRelationships, string renditionFilter, bool includePathSegment)
        {
            DocumentsQueryResult queryResult = conn.GetFromDocumentsForDocumentsQueryResultAsync(
                repositoryId,
                count: (int)10000)
                .Result;

            List<CmisObjectModel.ServiceModel.cmisObjectInFolderContainerType> files = new List<CmisObjectModel.ServiceModel.cmisObjectInFolderContainerType>();

            foreach (Document d in queryResult.Items)
            {
                Document doc = d.GetDocumentFromSelfRelation();

                CmisObjectModel.ServiceModel.cmisObjectType obj = get_Object_InternalFromDocuware(doc);

                files.Add(new CmisObjectModel.ServiceModel.cmisObjectInFolderContainerType()
                {
                    ObjectInFolder = new CmisObjectModel.ServiceModel.cmisObjectInFolderType()
                    {
                        Object = obj
                    }
                });
            }


            return new CmisObjectModel.ServiceModel.cmisObjectInFolderContainerType()
            {
                Children = files.ToArray()
            };
        }

        protected override Result<CmisObjectModel.ServiceModel.cmisObjectInFolderContainerType> GetFolderTree(string repositoryId, string folderId, string filter, long? depth, bool? includeAllowableActions, enumIncludeRelationships? includeRelationships, bool includePathSegment, string renditionFilter)
        {
            return NotSupported_Internal("GetFolderTree");
        }

        protected override Result<CmisObjectModel.ServiceModel.cmisObjectListType> GetObjectRelationships(string repositoryId, string objectId, bool includeSubRelationshipTypes, enumRelationshipDirection? relationshipDirection, string typeId, long? maxItems, long? skipCount, string filter, bool? includeAllowableActions)
        {
            return NotSupported_Internal("GetObjectRelationships");
        }

        protected override Result<CmisObjectModel.ServiceModel.cmisObjectListType> GetUnfiledObjects(string repositoryId, long? maxItems, long? skipCount, string filter, bool? includeAllowableActions, enumIncludeRelationships? includeRelationships, string renditionFilter, string orderBy)
        {
            return NotSupported_Internal("GetUnfiledObjects");
        }

        protected override Result<CmisObjectModel.ServiceModel.cmisObjectType> MoveObject(string repositoryId, string objectId, string targetFolderId, string sourceFolderId)
        {
            return NotSupported_Internal("MoveObject");
        }

        protected override Result<CmisObjectModel.ServiceModel.cmisObjectListType> Query(string repositoryId, string query, bool searchAllVersions, enumIncludeRelationships? includeRelationships, string renditionFilter, bool includeAllowableActions, long? maxItems, long? skipCount)
        {
            Log_Internal("Query");
            Log_Internal(query);

            if (query.ToLower().Contains("cmis:document"))
            {
                if (query.ToLower().Contains("where"))
                {
                    //MDPH51
                    //int index = query.IndexOf("mdata");

                    CmisObjectModel.ServiceModel.cmisObjectListType results = new CmisObjectModel.ServiceModel.cmisObjectListType();

                    List<CmisObjectModel.ServiceModel.cmisObjectType> files = new List<CmisObjectModel.ServiceModel.cmisObjectType>();
                    files.Add(get_Fake_Object_Internal(repositoryId, query));

                    results.Objects = files.ToArray();

                    return results;
                }
                else
                {
                    return NotSupported_Internal("Query");
                }
            }
            else if (query.ToLower().Contains("cmis:folder"))
            {
                CmisObjectModel.ServiceModel.cmisObjectListType results = new CmisObjectModel.ServiceModel.cmisObjectListType();

                return results;
            }
            else
                return NotSupported_Internal("Query");
        }

        protected override Result<CmisObjectModel.ServiceModel.cmisObjectType> RemoveObjectFromFolder(string repositoryId, string objectId, string folderId)
        {
            return NotSupported_Internal("RemoveObjectFromFolder");
        }

        protected override Exception RemovePolicy(string repositoryId, string objectId, string policyId)
        {
            return NotSupported_Internal("RemovePolicy");
        }

        protected override Result<cmisTypeDefinitionType> UpdateType(string repositoryId, cmisTypeDefinitionType modifiedType)
        {
            return NotSupported_Internal("UpdateType");
        }

        #endregion

    }
}