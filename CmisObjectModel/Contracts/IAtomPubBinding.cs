using ss = System.ServiceModel;
using ssw = System.ServiceModel.Web;
using sx = System.Xml;
using ca = CmisObjectModel.AtomPub;
using CmisObjectModel.Constants;

namespace CmisObjectModel.Contracts
{
    /// <summary>
   /// CMIS-AtomPub-Binding services supported in this assembly
   /// </summary>
   /// <remarks>
   /// WCF Service
   /// see http://docs.oasis-open.org/cmis/CMIS/v1.1/cs01/CMIS-v1.1-cs01.html
   /// 3.6 Resources Overview
   /// 
   /// Notice:
   /// Required parameters are received through the UriTemplate of the WebGetAttribute/WebInvokeAttribute attribute.
   /// Optional paramters are sent in the query string OR through the HttpContext class.
   /// The reason for this is that when using the same UriTemplate for both WebGet and WebInvoke, with optional parameters,
   /// the optional parameters will not be optional and they will be case sensitive. This due to an issue in WCF REST.
   /// When a service method is the only method targeting an Atom feed or entry, the optional paramters are retreived from
   /// the query string, e.g. the GetObjectParents method.
   /// When a service method is one of several methods targeting an Atom feed or entry, the optional parameters are
   /// retreived from the HttpContext class, e.g. the GetChildren method
   /// </remarks>
    [ss.ServiceContract(SessionMode = ss.SessionMode.NotAllowed, Namespace = Namespaces.cmisw)]
    [ss.XmlSerializerFormat()]
    public interface IAtomPubBinding
    {

        #region Repository
        /// <summary>
      /// Creates a new type
      /// </summary>
      /// <param name="repositoryId">The identifier for the repository</param>
      /// <param name="data">A fully populated type definition including all new property definitions</param>
      /// <returns></returns>
      /// <remarks>
      /// Success: HTTP/1.1 201 Created
      /// Media Type: application/atom+xml;type=entry (3.8.5.2 HTTP POST)</remarks>
        [ss.OperationContract()]
        [ssw.WebInvoke(Method = "POST", UriTemplate = ServiceURIs.CreateType, RequestFormat = ssw.WebMessageFormat.Xml, BodyStyle = ssw.WebMessageBodyStyle.Bare)]
        sx.XmlDocument CreateType(string repositoryId, System.IO.Stream data);

        /// <summary>
      /// Deletes a type definition
      /// </summary>
      /// <param name="repositoryId">The identifier for the repository</param>
      /// <param name="id">TypeId</param>
      /// <remarks>
      /// Success: HTTP/1.1 204 No Content
      /// </remarks>
        [ss.OperationContract()]
        [ssw.WebInvoke(Method = "DELETE", UriTemplate = ServiceURIs.DeleteType)]
        void DeleteType(string repositoryId, string id);

        /// <summary>
      /// Returns the CMIS service-documents for all available repositories
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        [ss.OperationContract(Name = "getRepositories")]
        [ssw.WebGet(UriTemplate = ServiceURIs.GetRepositories)]
        sx.XmlDocument GetRepositories();

        /// <summary>
      /// Returns the CMIS service-document for the specified repository
      /// </summary>
      /// <param name="repositoryId">The identifier for the repository</param>
      /// <returns></returns>
      /// <remarks></remarks>
        [ss.OperationContract(Name = "getRepositoryInfo")]
        [ssw.WebGet(UriTemplate = ServiceURIs.GetRepositoryInfo)]
        sx.XmlDocument GetRepositoryInfo(string repositoryId);

        /// <summary>
      /// Returns all child types of the specified type, if defined, otherwise the basetypes of the repository.
      /// </summary>
      /// <param name="repositoryId">The identifier for the repository</param>
      /// <returns></returns>
      /// <remarks>
      /// Success: HTTP/1.1 200 OK
      /// Media Type: application/atom+xml;type=feed
      /// The following CMIS Atom extension element MUST be included inside the atom entries:
      /// cmisra:type inside atom:entry
      /// The following CMIS Atom extension element MAY be included inside the atom feed:
      /// cmisra:numItems
      /// 
      /// Optional parameters:
      /// typeId, includePropertyDefinitions, maxItems, skipCount
      /// </remarks>
        [ss.OperationContract(Name = "getTypeChildren")]
        [ssw.WebGet(UriTemplate = ServiceURIs.GetTypeChildren)]
        sx.XmlDocument GetTypeChildren(string repositoryId);

        /// <summary>
      /// Returns the type-definition of the specified type
      /// </summary>
      /// <param name="repositoryId">The identifier for the repository</param>
      /// <param name="id">TypeId</param>
      /// <returns></returns>
      /// <remarks>
      /// Success: HTTP/1.1 200 OK
      /// Media Type: application/atom+xml;type=entry
      /// Atom-Extensions: cmisra:type
      /// </remarks>
        [ss.OperationContract(Name = "getTypeDefinition")]
        [ssw.WebGet(UriTemplate = ServiceURIs.GetTypeDefinition)]
        sx.XmlDocument GetTypeDefinition(string repositoryId, string id);

        /// <summary>
      /// Returns the descendant object-types under the specified type.
      /// </summary>
      /// <param name="repositoryId">The identifier for the repository</param>
      /// <param name="id">TypeId; optional
      /// If speciﬁed, then the repository MUST return all of descendant types of the speciﬁed type
      /// If not speciﬁed, then the Repository MUST return all types and MUST ignore the value of the depth parameter</param>
      /// <param name="includePropertyDefinitions">If TRUE, then the repository MUST return the property deﬁnitions for each object-type.
      /// If FALSE (default), the repository MUST return only the attributes for each object-type</param>
      /// <param name="depth">The number of levels of depth in the type hierarchy from which to return results. Valid values are
      /// 1:  Return only types that are children of the type. See also getTypeChildren
      /// >1: Return only types that are children of the type and descendants up to [depth] levels deep
      /// -1: Return ALL descendant types at all depth levels in the CMIS hierarchy</param>
      /// <returns></returns>
      /// <remarks>
      /// Success: HTTP/1.1 200 OK
      /// The following CMIS Atom extension element MAY be included inside the atom feed:
      /// cmisra:numItems
      /// Media Type: application/atom+xml;type=feed
      /// </remarks>
        [ss.OperationContract(Name = "getTypeDescendants")]
        [ssw.WebGet(UriTemplate = ServiceURIs.GetTypeDescendants)]
        sx.XmlDocument GetTypeDescendants(string repositoryId, string id, string includePropertyDefinitions, string depth);

        /// <summary>
      /// Updates a type definition
      /// </summary>
      /// <param name="repositoryId">The identifier for the repository</param>
      /// <param name="data">A type definition object with the property definitions that are to change.
      /// Repositories MUST ignore all fields in the type definition except for the type id and the list of properties.</param>
      /// <returns></returns>
      /// <remarks>
      /// Success: HTTP/1.1 200 OK
      /// MediaType: application/atom+xml;type=entry</remarks>
        [ss.OperationContract(Name = "updateType")]
        [ssw.WebInvoke(Method = "PUT", UriTemplate = ServiceURIs.UpdateType, RequestFormat = ssw.WebMessageFormat.Xml, BodyStyle = ssw.WebMessageBodyStyle.Bare)]
        sx.XmlDocument UpdateType(string repositoryId, System.IO.Stream data);
        #endregion

        #region Navigation
        /// <summary>
      /// Returns all children of the specified CMIS object.
      /// </summary>
      /// <param name="repositoryId">The identifier for the repository</param>
      /// <returns></returns>
      /// <remarks>
      /// Success: HTTP/1.1 200 OK
      /// Media Type: application/atom+xml;type=feed
      /// 
      /// Required parameters:
      /// folderId
      /// Optional parameters:
      /// maxItems, skipCount, filter, includeAllowableActions, includeRelationships, renditionFilter, orderBy, includePathSegment
      /// </remarks>
        [ss.OperationContract(Name = "getChildren")]
        [ssw.WebGet(UriTemplate = ServiceURIs.GetChildren)]
        sx.XmlDocument GetChildren(string repositoryId);

        /// <summary>
      /// Returns the descendant objects contained in the specified folder or any of its child-folders.
      /// </summary>
      /// <param name="repositoryId">The identifier for the repository</param>
      /// <param name="id">The identifier for the folder</param>
      /// <param name="filter"></param>
      /// <param name="depth">The number of levels of depth in the type hierarchy from which to return results. Valid values are
      /// 1:  Return only types that are children of the type. See also getTypeChildren
      /// >1: Return only types that are children of the type and descendants up to [depth] levels deep
      /// -1: Return ALL descendant types at all depth levels in the CMIS hierarchy</param>
      /// <param name="includeAllowableActions"></param>
      /// <param name="includeRelationships"></param>
      /// <param name="renditionFilter"></param>
      /// <param name="includePathSegment"></param>
      /// <returns></returns>
      /// <remarks>
      /// Success: HTTP/1.1 200 OK
      /// Media Type: application/atom+xml;type=feed 
      /// The following CMIS Atom extension element MUST be included inside the atom entries: 
      /// cmisra:object inside atom:entry 
      /// cmisra:pathSegment inside atom:entry 
      /// cmisra:children inside atom:entry
      /// The following CMIS Atom extension element MAY be included inside the atom feed:
      /// cmisra:numItems</remarks>
        [ss.OperationContract(Name = "getDescendants")]
        [ssw.WebGet(UriTemplate = ServiceURIs.GetDescendants)]
        sx.XmlDocument GetDescendants(string repositoryId, string id, string filter, string depth, string includeAllowableActions, string includeRelationships, string renditionFilter, string includePathSegment);

        /// <summary>
      /// Returns the descendant folders contained in the specified folder
      /// </summary>
      /// <param name="repositoryId">The identifier for the repository</param>
      /// <returns></returns>
      /// <remarks>
      /// Success: HTTP/1.1 200 OK
      /// Media Type: application/atom +xml;type=feed
      /// The following CMIS Atom extension element MUST be included inside the atom entries:
      /// cmisra:object inside atom:entry
      /// cmisra:pathSegment inside atom:entry
      /// cmisra:children inside atom:entry
      /// The following CMIS Atom extension element MAY be included inside the atom feed:
      /// cmisra:numItems
      /// 
      /// Required parameters:
      /// folderId
      /// Optional parameters:
      /// filter, depth, includeAllowableActions, includeRelationships, includePathSegment, renditionFilter
      /// </remarks>
        [ss.OperationContract(Name = "getFolderTree")]
        [ssw.WebGet(UriTemplate = ServiceURIs.GetFolderTree)]
        sx.XmlDocument GetFolderTree(string repositoryId);

        /// <summary>
      /// Returns the parent folder-object of the specified folder
      /// </summary>
      /// <param name="repositoryId">The identifier for the repository</param>
      /// <param name="folderId"></param>
      /// <param name="filter"></param>
      /// <returns></returns>
      /// <remarks>
      /// Success: HTTP/1.1 200 OK
      /// Media Type: application/atom +xml;type=entry
      /// 
      /// WebGet is defined for function GetObject()
      /// If GetObject() receives instead of the parameter 'objectId' the parameter 'folderId' the call is redirected
      /// from GetObject() to GetFolderParent()
      /// </remarks>
        sx.XmlDocument GetFolderParent(string repositoryId, string folderId, string filter);

        /// <summary>
      /// Returns the parent folders for the specified object
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="id"></param>
      /// <param name="filter"></param>
      /// <param name="includeAllowableActions"></param>
      /// <param name="includeRelationships"></param>
      /// <param name="renditionFilter"></param>
      /// <param name="includeRelativePathSegment"></param>
      /// <returns></returns>
      /// <remarks>
      /// Success: HTTP/1.1 200 OK
      /// Media Type: application/atom+xml;type=feed 
      /// This feed contains a set of atom entries for each parent of the object that MUST contain: 
      /// cmisra:object inside atom:entry 
      /// cmisra:relativePathSegment 
      /// </remarks>
        [ss.OperationContract(Name = "getObjectParents")]
        [ssw.WebGet(UriTemplate = ServiceURIs.GetObjectParents)]
        sx.XmlDocument GetObjectParents(string repositoryId, string id, string filter, string includeAllowableActions, string includeRelationships, string renditionFilter, string includeRelativePathSegment);

        /// <summary>
      /// Returns a list of check out object the user has access to.
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <returns></returns>
      /// <remarks>
      /// Success: HTTP/1.1 200 OK
      /// Media Type: application/atom+xml;type=feed
      /// The following CMIS Atom extension element MUST be included inside the atom entries:
      /// cmisra:object inside atom:entry
      /// The following CMIS Atom extension element MAY be included inside the atom feed:
      /// cmisra:numItems
      /// 
      /// Optional parameters:
      /// folderId, maxItems, skipCount, orderBy, filter, includeAllowableActions, includeRelationships, renditionFilter
      /// </remarks>
        [ss.OperationContract(Name = "getCheckedOutDocs")]
        [ssw.WebGet(UriTemplate = ServiceURIs.GetCheckedOutDocs)]
        sx.XmlDocument GetCheckedOutDocs(string repositoryId);

        /// <summary>
      /// Returns a list of all unfiled documents in the repository
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        [ss.OperationContract(Name = "getUnfiledObjects")]
        [ssw.WebGet(UriTemplate = ServiceURIs.GetUnfiledObjects)]
        sx.XmlDocument GetUnfiledObjects(string repositoryId);
        #endregion

        #region Object
        /// <summary>
      /// Appends to the content stream for the speciﬁed document object.
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="objectId"></param>
      /// <param name="contentStream"></param>
      /// <param name="isLastChunk"></param>
      /// <param name="changeToken"></param>
      /// <returns></returns>
      /// <remarks>
      /// Method is called by SetContentStream(), if the append-argument is specified and set to true (see 3.11.8.2 HTTP PUT)
      /// </remarks>
        sx.XmlDocument AppendContentStream(string repositoryId, string objectId, System.IO.Stream contentStream, bool isLastChunk, string changeToken);

        /// <summary>
      /// Updates properties and secondary types of one or more objects
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="data"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        [ss.OperationContract(Name = "bulkUpdateProperties")]
        [ssw.WebInvoke(Method = "POST", UriTemplate = ServiceURIs.BulkUpdateProperties, RequestFormat = ssw.WebMessageFormat.Xml, BodyStyle = ssw.WebMessageBodyStyle.Bare)]
        sx.XmlDocument BulkUpdateProperties(string repositoryId, System.IO.Stream data);

        /// <summary>
      /// Creates a new document in the specified folder or as unfiled document
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="data"></param>
      /// <param name="folderId">If specified, the identifier for the folder that MUST be the parent folder for the newly-created document object.
      /// This parameter MUST be specified if the repository does NOT support the optional "unfiling" capability.</param>
      /// <param name="versioningState"></param>
      /// <param name="addACEs">A list of ACEs that MUST be added to the newly-created document object, either using the ACL from folderId if specified, or being applied if no folderId is specified.</param>
      /// <param name="removeACEs">A list of ACEs that MUST be removed from the newly-created document object, either using the ACL from folderId if specified, or being ignored if no folderId is specified.</param>
      /// <returns></returns>
      /// <remarks></remarks>
        sx.XmlDocument CreateDocument(string repositoryId, string folderId, Core.enumVersioningState? versioningState, ca.AtomEntry data, Core.Security.cmisAccessControlListType addACEs = null, Core.Security.cmisAccessControlListType removeACEs = null);

        /// <summary>
      /// Creates a document object as a copy of the given source document in the (optionally) speciﬁed location
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="sourceId"></param>
      /// <param name="properties">The property values that MUST be applied to the object. This list of properties SHOULD only contain properties whose values differ from the source document</param>
      /// <param name="folderId">If specified, the identifier for the folder that MUST be the parent folder for the newly-created document object.
      /// This parameter MUST be specified if the repository does NOT support the optional "unfiling" capability.</param>
      /// <param name="versioningState"></param>
      /// <param name="policies">A list of policy ids that MUST be applied to the newly-created document object</param>
      /// <param name="addACEs">A list of ACEs that MUST be added to the newly-created document object, either using the ACL from folderId if specified, or being applied if no folderId is specified</param>
      /// <param name="removeACEs">A list of ACEs that MUST be removed from the newly-created document object, either using the ACL from folderId if specified, or being ignored if no folderId is specified.</param>
      /// <returns></returns>
      /// <remarks></remarks>
        sx.XmlDocument CreateDocumentFromSource(string repositoryId, string sourceId, Core.Collections.cmisPropertiesType properties, string folderId, Core.enumVersioningState? versioningState, string[] policies, Core.Security.cmisAccessControlListType addACEs = null, Core.Security.cmisAccessControlListType removeACEs = null);

        /// <summary>
      /// Creates a folder object of the specified type in the specified location
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="data"></param>
      /// <param name="parentFolderId">The identifier for the folder that MUST be the parent folder for the newly-created folder object</param>
      /// <param name="addACEs">A list of ACEs that MUST be added to the newly-created folder object, either using the ACL from folderId if specified, or being applied if no folderId is specified</param>
      /// <param name="removeACEs">A list of ACEs that MUST be removed from the newly-created folder object, either using the ACL from folderId if specified, or being ignored if no folderId is specified</param>
      /// <returns></returns>
      /// <remarks></remarks>
        sx.XmlDocument CreateFolder(string repositoryId, string parentFolderId, ca.AtomEntry data, Core.Security.cmisAccessControlListType addACEs = null, Core.Security.cmisAccessControlListType removeACEs = null);

        /// <summary>
      /// Creates an item object of the specified type in the specified location
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="data"></param>
      /// <param name="folderId">The identifier for the folder that MUST be the parent folder for the newly-created folder object</param>
      /// <param name="addACEs">A list of ACEs that MUST be added to the newly-created policy object, either using the ACL from folderId if specified, or being applied if no folderId is specified</param>
      /// <param name="removeACEs">A list of ACEs that MUST be removed from the newly-created policy object, either using the ACL from folderId if specified, or being ignored if no folderId is specified</param>
      /// <returns></returns>
      /// <remarks></remarks>
        sx.XmlDocument CreateItem(string repositoryId, string folderId, ca.AtomEntry data, Core.Security.cmisAccessControlListType addACEs = null, Core.Security.cmisAccessControlListType removeACEs = null);

        /// <summary>
      /// Creates a policy object of the specified type in the specified location
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="data"></param>
      /// <param name="folderId">The identifier for the folder that MUST be the parent folder for the newly-created folder object</param>
      /// <param name="addACEs">A list of ACEs that MUST be added to the newly-created policy object, either using the ACL from folderId if specified, or being applied if no folderId is specified</param>
      /// <param name="removeACEs">A list of ACEs that MUST be removed from the newly-created policy object, either using the ACL from folderId if specified, or being ignored if no folderId is specified</param>
      /// <returns></returns>
      /// <remarks></remarks>
        sx.XmlDocument CreatePolicy(string repositoryId, string folderId, ca.AtomEntry data, Core.Security.cmisAccessControlListType addACEs = null, Core.Security.cmisAccessControlListType removeACEs = null);

        /// <summary>
      /// Creates a relationship object of the specified type
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="data"></param>
      /// <returns></returns>
      /// <remarks>
      /// Success: HTTP/1.1 200 OK
      /// Media Type: application/atom+xml;type=entry (3.9.2.2 HTTP POST)
      /// </remarks>
        [ss.OperationContract(Name = "createRelationship")]
        [ssw.WebInvoke(Method = "POST", UriTemplate = ServiceURIs.CreateRelationship, RequestFormat = ssw.WebMessageFormat.Xml, BodyStyle = ssw.WebMessageBodyStyle.Bare)]
        sx.XmlDocument CreateRelationship(string repositoryId, System.IO.Stream data);

        /// <summary>
      /// Deletes the content stream of the specified object.
      /// </summary>
      /// <remarks>
      /// A repository MAY automatically create new document versions as part of this service method. Therefore, the obejctId output NEED NOT be identical to the objectId input.
      /// </remarks>
        [ss.OperationContract(Name = "deleteContentStream")]
        [ssw.WebInvoke(Method = "DELETE", UriTemplate = ServiceURIs.DeleteContentStream)]
        sx.XmlDocument DeleteContentStream(string repositoryId);

        /// <summary>
      /// Removes the submitted document
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <remarks>
      /// Success: HTTP code: 204 No Content
      /// </remarks>
        [ss.OperationContract()]
        [ssw.WebInvoke(Method = "DELETE", UriTemplate = ServiceURIs.DeleteObject)]
        void DeleteObject(string repositoryId);

        /// <summary>
      /// Deletes the specified folder object and all of its child- and descendant-objects.
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <remarks>
      /// Status Code:
      /// 200 OK if successful.  Body contains entity describing the status
      /// 202 Accepted, if accepted but deletion not yet taking place
      /// 204 No Content, if successful with no content
      /// 403 Forbidden, if permission is denied
      /// 401 Unauthorized, if not authenticated
      /// 500 Internal Server Error.  The body SHOULD contain an entity describing the status
      /// </remarks>
        [ss.OperationContract(Name = "deleteTree")]
        [ssw.WebInvoke(Method = "DELETE", UriTemplate = ServiceURIs.DeleteTree)]
        sx.XmlDocument DeleteTree(string repositoryId);

        /// <summary>
      /// Deletes the specified folder object and all of its child- and descendant-objects.
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <remarks>
      /// Status Code:
      /// 200 OK if successful.  Body contains entity describing the status
      /// 202 Accepted, if accepted but deletion not yet taking place
      /// 204 No Content, if successful with no content
      /// 403 Forbidden, if permission is denied
      /// 401 Unauthorized, if not authenticated
      /// 500 Internal Server Error.  The body SHOULD contain an entity describing the status
      /// </remarks>
        [ss.OperationContract(Name = "deleteTreeViaDescendantsFeed")]
        [ssw.WebInvoke(Method = "DELETE", UriTemplate = ServiceURIs.DeleteTreeViaDescendantsFeed)]
        sx.XmlDocument DeleteTreeViaDescendantsFeed(string repositoryId);

        /// <summary>
      /// Deletes the specified folder object and all of its child- and descendant-objects.
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <remarks>
      /// Status Code:
      /// 200 OK if successful.  Body contains entity describing the status
      /// 202 Accepted, if accepted but deletion not yet taking place
      /// 204 No Content, if successful with no content
      /// 403 Forbidden, if permission is denied
      /// 401 Unauthorized, if not authenticated
      /// 500 Internal Server Error.  The body SHOULD contain an entity describing the status
      /// </remarks>
        [ss.OperationContract(Name = "deleteTreeViaChildrenFeed")]
        [ssw.WebInvoke(Method = "DELETE", UriTemplate = ServiceURIs.DeleteTreeViaChildrenFeed)]
        sx.XmlDocument DeleteTreeViaChildrenFeed(string repositoryId);

        /// <summary>
      /// Returns the allowable actions for the specified document.
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="id"></param>
      /// <returns></returns>
      /// <remarks>
      /// Media Type: application/cmisallowableactions+xml
      /// </remarks>
        [ss.OperationContract(Name = "getAllowableActions")]
        [ssw.WebGet(UriTemplate = ServiceURIs.GetAllowableActions)]
        sx.XmlDocument GetAllowableActions(string repositoryId, string id);

        /// <summary>
      /// Returns the content stream of the specified object.
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <returns></returns>
      /// <remarks>
      /// This returns the content stream. 
      /// It is RECOMMENDED that HTTP Range requests are supported on this resource.  
      /// It is RECOMMENDED that HTTP compression is also supported. 
      /// Please see RFC2616 for more information on HTTP Range requests.
      /// </remarks>
        [ss.OperationContract(Name = "getContentStream")]
        [ssw.WebGet(UriTemplate = ServiceURIs.GetContentStream)]
        System.IO.Stream GetContentStream(string repositoryId);

        /// <summary>
      /// Gets the specified information for the object.
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <returns></returns>
      /// <remarks>
      /// Success: HTTP/1.1 200 OK
      /// Media Type: application/atom+xml;type=entry
      /// 
      /// requested parameters:
      /// objectId
      /// optional parameters:
      /// filter, includeRelationships, includePolicyIds, renditionFilter, includeACL, includeAllowableActions,
      /// returnVersion(getObjectOfLatestVersion), major(getObjectOfLatestVersion), versionSeriesId(getObjectOfLatestVersion)
      /// </remarks>
        [ss.OperationContract(Name = "getObject")]
        [ssw.WebGet(UriTemplate = ServiceURIs.GetObject)]
        sx.XmlDocument GetObject(string repositoryId);

        /// <summary>
      /// Returns the object at the specified path
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="path"></param>
      /// <param name="filter"></param>
      /// <param name="includeAllowableActions"></param>
      /// <param name="includePolicyIds"></param>
      /// <param name="includeRelationships"></param>
      /// <param name="includeACL"></param>
      /// <param name="renditionFilter"></param>
      /// <returns></returns>
      /// <remarks>
      /// Media Type: application/atom+xml;type=entry
      /// </remarks>
        [ss.OperationContract(Name = "getObjectByPath")]
        [ssw.WebGet(UriTemplate = ServiceURIs.GetObjectByPath)]
        sx.XmlDocument GetObjectByPath(string repositoryId, string path, string filter, string includeAllowableActions, string includePolicyIds, string includeRelationships, string includeACL, string renditionFilter);

        /// <summary>
      /// Moves the specified file-able object from one folder to another
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="objectId"></param>
      /// <param name="targetFolderId"></param>
      /// <param name="sourceFolderId"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        sx.XmlDocument MoveObject(string repositoryId, string objectId, string targetFolderId, string sourceFolderId);

        /// <summary>
      /// Sets the content stream of the specified object.
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="data"></param>
      /// <remarks>
      /// Success HTTP code: 
      /// 200 (with content),  
      /// 204 (without content) or
      /// 201 if a new resource is created.  
      /// Please see the HTTP specification for more information. 
      /// Content-Location: URI for content stream 
      /// Location: URI for content stream
      /// </remarks>
        [ss.OperationContract(Name = "setContentStream")]
        [ssw.WebInvoke(Method = "PUT", UriTemplate = ServiceURIs.SetContentStream)]
        sx.XmlDocument SetContentStream(string repositoryId, System.IO.Stream data);

        /// <summary>
      /// Updates the submitted cmis-object
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="changeToken"></param>
      /// <param name="data"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        sx.XmlDocument UpdateProperties(string repositoryId, string objectId, ca.AtomEntry data, string changeToken); // Response Body containing the updated atom entry
        #endregion

        #region Multi
        /// <summary>
      /// Adds an existing fileable non-folder object to a folder.
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="objectId"></param>
      /// <param name="folderId"></param>
      /// <param name="allVersions">Add all versions of the object to the folder if the repository supports version-specific filing. Defaults to TRUE.</param>
      /// <returns></returns>
      /// <remarks></remarks>
        sx.XmlDocument AddObjectToFolder(string repositoryId, string objectId, string folderId, bool allVersions);

        /// <summary>
      /// Removes an existing fileable non-folder object from a folder.
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="folderId">The folder from which the object is to be removed.
      /// If no value is specified, then the repository MUST remove the object from all folders in which it is currently filed.</param>
      /// <remarks>
      /// Media Type: application/atom+xml;type=feed
      /// MUST support Atom Entry Documents with CMIS extensions
      /// application/atom+xml;type=entry or
      /// application/cmisatom+xml
      /// The following CMIS Atom extension element MUST be included inside the atom entries:
      /// cmisra:object inside atom:entry
      /// The following CMIS Atom extension element MAY be included inside the atom feed:
      /// cmisra:numItems
      /// HTTP Success: 201
      /// Location Header
      /// </remarks>
        sx.XmlDocument RemoveObjectFromFolder(string repositoryId, string objectId, string folderId);
        #endregion

        #region Discovery
        /// <summary>
      /// Returns a list of content changes
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="filter"></param>
      /// <param name="maxItems"></param>
      /// <param name="includeACL"></param>
      /// <param name="includePolicyIds"></param>
      /// <param name="includeProperties"></param>
      /// <param name="changeLogToken">If this parameter is specified, start the changes from the specified token. The changeLogToken is embedded in the paging link relations for normal iteration through the change list. </param>
      /// <returns></returns>
      /// <remarks>
      /// Media Type: application/atom+xml;type=feed 
      /// The following CMIS Atom extension element MUST be included inside the atom entries: 
      /// cmisra:object inside atom:entry
      /// The following CMIS Atom extension element MAY be included inside the atom feed:
      /// cmisra:numItems
      /// </remarks>
        [ss.OperationContract(Name = "getContentChanges")]
        [ssw.WebGet(UriTemplate = ServiceURIs.GetContentChanges)]
        sx.XmlDocument GetContentChanges(string repositoryId, string filter, string maxItems, string includeACL, string includePolicyIds, string includeProperties, string changeLogToken);

        /// <summary>
      /// Returns the data described by the specified CMIS query. (GET Request)
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <returns></returns>
      /// <remarks>
      /// Media Type: application/atom+xml;type=feed
      /// Status Codes:
      /// 200 Success
      /// Headers returned:
      /// Location Header
      /// Content-Location Header
      /// </remarks>
        [ss.OperationContract(Name = "queryGet")]
        [ssw.WebGet(UriTemplate = ServiceURIs.Query)]
        sx.XmlDocument Query(string repositoryId);

        /// <summary>
      /// Returns the data described by the specified CMIS query. (POST Request)
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <returns></returns>
      /// <remarks>
      /// Media Type: application/atom+xml;type=feed
      /// Status Codes:
      /// 201 Success
      /// Headers returned:
      /// Location Header
      /// Content-Location Header
      /// </remarks>
        [ss.OperationContract(Name = "query")]
        [ssw.WebInvoke(Method = "POST", UriTemplate = ServiceURIs.Query, RequestFormat = ssw.WebMessageFormat.Xml, BodyStyle = ssw.WebMessageBodyStyle.Bare)]
        sx.XmlDocument Query(string repositoryId, System.IO.Stream data);
        #endregion

        #region Versioning
        /// <summary>
      /// Reverses the effect of a check-out (checkOut). Removes the Private Working Copy of the checked-out document, allowing other documents in the version series to be checked out again.
      /// If the private working copy has been created by createDocument, cancelCheckOut MUST delete the created document.
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <remarks>Handled by DeleteObject()</remarks>
        void CancelCheckOut(string repositoryId);

        /// <summary>
      /// Checks-in the Private Working Copy document.
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="data"></param>
      /// <param name="major">TRUE (default) if the checked-in document object MUST be a major version.</param>
      /// <param name="checkInComment"></param>
      /// <param name="addACEs">A list of ACEs that MUST be added to the newly-created document object.</param>
      /// <param name="removeACEs">A list of ACEs that MUST be removed from the newly-created document object.</param>
      /// <returns></returns>
      /// <remarks>
      /// For repositories that do NOT support the optional capabilityPWCUpdatable capability, the properties and contentStream input parameters MUST be
      /// provided on the checkIn service for updates to happen as part of checkIn.
      /// Each CMIS protocol binding MUST specify whether the checkin service MUST always include all updatable properties, or only those properties
      /// whose values are different than the original value of the object.
      /// </remarks>
        sx.XmlDocument CheckIn(string repositoryId, string objectId, ca.AtomEntry data, bool? major, string checkInComment, Core.Security.cmisAccessControlListType addACEs = null, Core.Security.cmisAccessControlListType removeACEs = null);

        /// <summary>
      /// Checks out the specified CMIS object.
      /// </summary>
      /// <returns></returns>
      /// <remarks>
      /// HTTP/1.1 201 Created
      /// MUST support Atom Entry Documents with CMIS extensions
      /// application/atom+xml;type=entry or
      /// application/cmisatom+xml
      /// </remarks>
        [ss.OperationContract(Name = "checkOut")]
        [ssw.WebInvoke(Method = "POST", UriTemplate = ServiceURIs.CheckOut, RequestFormat = ssw.WebMessageFormat.Xml, BodyStyle = ssw.WebMessageBodyStyle.Bare)]
        sx.XmlDocument CheckOut(string repositoryId, System.IO.Stream data);

        /// <summary>
      /// Returns all Documents in the specified version series.
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <returns></returns>
      /// <remarks>
      /// Media Type: application/atom+xml;type=feed
      /// This feed contains a set of atom entries for each version in the version series
      /// cmisra:object inside atom:entry
      /// cmisra:children inside atom:entry if atom:entry represents a CMIS Folder
      /// </remarks>
        [ss.OperationContract(Name = "getAllVersions")]
        [ssw.WebGet(UriTemplate = ServiceURIs.GetAllVersions)]
        sx.XmlDocument GetAllVersions(string repositoryId);

        /// <summary>
      /// Get the latest document object in the version series
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="versionSeriesId"></param>
      /// <param name="major"></param>
      /// <param name="filter"></param>
      /// <param name="includeRelationships"></param>
      /// <param name="includePolicyIds"></param>
      /// <param name="renditionFilter"></param>
      /// <param name="includeACL"></param>
      /// <param name="includeAllowableActions"></param>
      /// <returns></returns>
      /// <remarks>Handled by GetObject()</remarks>
        sx.XmlDocument GetObjectOfLatestVersion(string repositoryId, string objectId, string versionSeriesId, bool? major, string filter, Core.enumIncludeRelationships? includeRelationships, bool? includePolicyIds, string renditionFilter, bool? includeACL, bool? includeAllowableActions);
        #endregion

        #region Relationship
        /// <summary>
      /// Returns the relationships for the specified object.
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <returns></returns>
      /// <remarks>
      /// Media Type: application/atom+xml;type=feed
      /// The following CMIS Atom extension element MUST be included inside the atom entries:
      /// cmisra:object inside atom:entry
      /// The following CMIS Atom extension element MAY be included inside the atom feed:
      /// cmisra:numItems
      /// </remarks>
        [ss.OperationContract(Name = "getObjectRelationships")]
        [ssw.WebGet(UriTemplate = ServiceURIs.GetObjectRelationships)]
        sx.XmlDocument GetObjectRelationships(string repositoryId);
        #endregion

        #region Policy
        /// <summary>
      /// Applies a policy to the specified object.
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="data"></param>
      /// <returns></returns>
      /// <remarks>
      /// MUST support Atom Entry Documents with CMIS extensions
      /// application/atom+xml;type=entry or
      /// application/cmisatom+xml
      /// </remarks>
        [ss.OperationContract(Name = "applyPolicy")]
        [ssw.WebInvoke(Method = "POST", UriTemplate = ServiceURIs.ApplyPolicy, RequestFormat = ssw.WebMessageFormat.Xml, BodyStyle = ssw.WebMessageBodyStyle.Bare)]
        sx.XmlDocument ApplyPolicy(string repositoryId, System.IO.Stream data);

        /// <summary>
      /// Returns a list of policies applied to the specified object.
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="id"></param>
      /// <returns></returns>
      /// <remarks>
      /// Media Type: application/atom+xml;type=feed
      /// The following CMIS Atom extension element MUST be included inside the atom entries:
      /// cmisra:object inside atom:entry
      /// The following CMIS Atom extension element MAY be included inside the atom feed:
      /// cmisra:numItems
      /// </remarks>
        [ss.OperationContract(Name = "getAppliedPolicies")]
        [ssw.WebGet(UriTemplate = ServiceURIs.GetAppliedPolicies)]
        sx.XmlDocument GetAppliedPolicies(string repositoryId, string id);

        /// <summary>
      /// Removes a policy from the specified object.
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="id"></param>
      /// <remarks></remarks>
        [ss.OperationContract(Name = "removePolicy")]
        [ssw.WebInvoke(Method = "DELETE", UriTemplate = ServiceURIs.RemovePolicy)]
        void RemovePolicy(string repositoryId, string id, string policyId);
        #endregion

        #region ACL
        /// <summary>
      /// Adds or removes the given ACEs to or from the ACL of document or folder object.
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="id"></param>
      /// <param name="data"></param>
      /// <remarks>
      /// Keine weiteren Infos in Standardbeschreibung http://docs.oasis-open.org/cmis/CMIS/v1.0/cmis-spec-v1.0.html
      /// </remarks>
        [ss.OperationContract(Name = "applyACL")]
        [ssw.WebInvoke(Method = "PUT", UriTemplate = ServiceURIs.ApplyAcl, RequestFormat = ssw.WebMessageFormat.Xml, BodyStyle = ssw.WebMessageBodyStyle.Bare)]
        sx.XmlDocument ApplyACL(string repositoryId, string id, System.IO.Stream data);

        /// <summary>
      /// Get the ACL currently applied to the specified document or folder object.
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="id"></param>
      /// <returns></returns>
      /// <remarks>
      /// Media Type: application/cmisacl+xml
      /// </remarks>
        [ss.OperationContract(Name = "getACL")]
        [ssw.WebGet(UriTemplate = ServiceURIs.GetAcl)]
        sx.XmlDocument GetACL(string repositoryId, string id);
        #endregion

        #region Miscellaneous
        /// <summary>
      /// Handles every POST on object resource.
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="data"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        [ss.OperationContract(Name = "checkInOrUpdateProperties")]
        [ssw.WebInvoke(Method = "PUT", UriTemplate = ServiceURIs.UpdateProperties, RequestFormat = ssw.WebMessageFormat.Xml, BodyStyle = ssw.WebMessageBodyStyle.Bare)]
        sx.XmlDocument CheckInOrUpdateProperties(string repositoryId, System.IO.Stream data); // Response Body containing the updated atom entry

        /// <summary>
      /// Handles every POST on the folder children collection. As defined in 3.9.2.2 HTTP POST the function has to return in the AtomPub-Binding
      /// an ca.AtomEntry-Object (MediaType: application/atom+xml;type=entry)
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="data"></param>
      /// <returns></returns>
      /// <remarks>
      /// Success: HTTP/1.1 200 OK
      /// Handled cmis services:
      /// addObjectToFolder, createDocument, createDocumentFromSource, createFolder, createPolicy, moveObject
      /// The function supports data as a serialized ca.AtomEntry-instance or as a serialized request-Object
      /// out of the Namespace CmisObjectModel.Messaging.Requests
      /// Media Type: application/atom+xml;type=entry (3.9.2.2 HTTP POST)
      /// </remarks>
        [ss.OperationContract(Name = "createOrMoveChildObject")]
        [ssw.WebInvoke(Method = "POST", UriTemplate = ServiceURIs.CreateOrMoveChildObject, RequestFormat = ssw.WebMessageFormat.Xml, BodyStyle = ssw.WebMessageBodyStyle.Bare)]
        sx.XmlDocument CreateOrMoveChildObject(string repositoryId, System.IO.Stream data);

        /// <summary>
      /// Returns the new object created in the unfiled-resource
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <returns></returns>
      /// <remarks>
      /// Success: HTTP/1.1 201 Created
      /// Handles the cmis services:
      /// createDocument, createPolicy, removeObjectFromFolder
      /// </remarks>
        [ss.OperationContract(Name = "createUnfiledObjectOrRemoveObjectFromFolder")]
        [ssw.WebInvoke(Method = "POST", UriTemplate = ServiceURIs.CreateUnfiledObjectOrRemoveObjectFromFolder, RequestFormat = ssw.WebMessageFormat.Xml, BodyStyle = ssw.WebMessageBodyStyle.Bare)]
        sx.XmlDocument CreateUnfiledObjectOrRemoveObjectFromFolder(string repositoryId, System.IO.Stream data);
        #endregion

    }
}