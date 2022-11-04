using System.Collections.Generic;
using ccdp = CmisObjectModel.Core.Definitions.Properties;
/* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.Common
{
    /// <summary>
   /// Factory of cmis predefined PropertyDefinition-instances
   /// </summary>
   /// <remarks></remarks>
    public class PredefinedPropertyDefinitionFactory
    {

        protected static Dictionary<string, bool> _predefinedRequired = new Dictionary<string, bool>() { { Constants.CmisPredefinedPropertyNames.AllowedChildObjectTypeIds, false }, { Constants.CmisPredefinedPropertyNames.ChangeToken, false }, { Constants.CmisPredefinedPropertyNames.CreatedBy, true }, { Constants.CmisPredefinedPropertyNames.CreationDate, true }, { Constants.CmisPredefinedPropertyNames.Extensions.ForeignChangeToken, false }, { Constants.CmisPredefinedPropertyNames.Extensions.ForeignObjectId, false }, { Constants.CmisPredefinedPropertyNames.Extensions.SyncRequired, false }, { Constants.CmisPredefinedPropertyNames.LastModificationDate, true }, { Constants.CmisPredefinedPropertyNames.LastModifiedBy, true }, { Constants.CmisPredefinedPropertyNames.ObjectId, true } };
        protected string _localNamespace;
        protected bool _inherited;
        protected bool _isBaseType;
        public PredefinedPropertyDefinitionFactory(string localNamespace, bool isBaseType = true)
        {
            _localNamespace = localNamespace;
            _inherited = !isBaseType;
            _isBaseType = isBaseType;
        }

        #region PropertyDefinitionFactories
        /// <summary>
      /// Returns a PropertyBooleanDefinition-object
      /// </summary>
      /// <param name="id"></param>
      /// <param name="localName"></param>
      /// <param name="displayName"></param>
      /// <param name="queryName">ignored for base types</param>
      /// <param name="required"></param>
      /// <param name="orderable"></param>
      /// <param name="cardinality"></param>
      /// <param name="updatability"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public virtual ccdp.cmisPropertyBooleanDefinitionType PropertyBooleanDefinition(string id, string localName, string displayName, string queryName, bool required, bool orderable, Core.enumCardinality cardinality, Core.enumUpdatability updatability)
        {
            return new ccdp.cmisPropertyBooleanDefinitionType(id, localName, _localNamespace, displayName, _isBaseType || string.IsNullOrEmpty(queryName) ? id : queryName, required, _inherited, _predefinedRequired.ContainsKey(id) ? _predefinedRequired[id] : !string.IsNullOrEmpty(queryName), orderable, cardinality, updatability) { Description = displayName };
        }
        /// <summary>
      /// Returns a PropertyDateTimeDefinition-object
      /// </summary>
      /// <param name="id"></param>
      /// <param name="localName"></param>
      /// <param name="displayName"></param>
      /// <param name="queryName">ignored for base types</param>
      /// <param name="required"></param>
      /// <param name="orderable"></param>
      /// <param name="cardinality"></param>
      /// <param name="updatability"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public virtual ccdp.cmisPropertyDateTimeDefinitionType PropertyDateTimeDefinition(string id, string localName, string displayName, string queryName, bool required, bool orderable, Core.enumCardinality cardinality, Core.enumUpdatability updatability)
        {
            return new ccdp.cmisPropertyDateTimeDefinitionType(id, localName, _localNamespace, displayName, _isBaseType || string.IsNullOrEmpty(queryName) ? id : queryName, required, _inherited, _predefinedRequired.ContainsKey(id) ? _predefinedRequired[id] : !string.IsNullOrEmpty(queryName), orderable, cardinality, updatability) { Description = displayName };
        }
        /// <summary>
      /// Returns a PropertyDecimalDefinition-object
      /// </summary>
      /// <param name="queryName">ignored for base types</param>
      /// <returns></returns>
      /// <remarks></remarks>
        public virtual ccdp.cmisPropertyDecimalDefinitionType PropertyDecimalDefinition(string id, string localName, string displayName, string queryName, bool required, bool orderable, Core.enumCardinality cardinality, Core.enumUpdatability updatability, decimal maxValue, decimal minValue)
        {
            return new ccdp.cmisPropertyDecimalDefinitionType(id, localName, _localNamespace, displayName, _isBaseType || string.IsNullOrEmpty(queryName) ? id : queryName, required, _inherited, _predefinedRequired.ContainsKey(id) ? _predefinedRequired[id] : !string.IsNullOrEmpty(queryName), orderable, cardinality, updatability) { Description = displayName };
        }
        /// <summary>
      /// Returns a PropertyDoubleDefinition-object
      /// </summary>
      /// <param name="queryName">ignored for base types</param>
      /// <returns></returns>
      /// <remarks></remarks>
        public virtual ccdp.cmisPropertyDoubleDefinitionType PropertyDoubleDefinition(string id, string localName, string displayName, string queryName, bool required, bool orderable, Core.enumCardinality cardinality, Core.enumUpdatability updatability, double maxValue, double minValue)
        {
            return new ccdp.cmisPropertyDoubleDefinitionType(id, localName, _localNamespace, displayName, _isBaseType || string.IsNullOrEmpty(queryName) ? id : queryName, required, _inherited, _predefinedRequired.ContainsKey(id) ? _predefinedRequired[id] : !string.IsNullOrEmpty(queryName), orderable, cardinality, updatability) { Description = displayName };
        }
        /// <summary>
      /// Returns a PropertyHtmlDefinition-object
      /// </summary>
      /// <param name="queryName">ignored for base types</param>
      /// <returns></returns>
      /// <remarks></remarks>
        public virtual ccdp.cmisPropertyHtmlDefinitionType PropertyHtmlDefinition(string id, string localName, string displayName, string queryName, bool required, bool orderable, Core.enumCardinality cardinality, Core.enumUpdatability updatability)
        {
            return new ccdp.cmisPropertyHtmlDefinitionType(id, localName, _localNamespace, displayName, _isBaseType || string.IsNullOrEmpty(queryName) ? id : queryName, required, _inherited, _predefinedRequired.ContainsKey(id) ? _predefinedRequired[id] : !string.IsNullOrEmpty(queryName), orderable, cardinality, updatability) { Description = displayName };
        }
        /// <summary>
      /// Returns a PropertyIdDefinition-object
      /// </summary>
      /// <param name="id"></param>
      /// <param name="localName"></param>
      /// <param name="displayName"></param>
      /// <param name="queryName">ignored for base types</param>
      /// <param name="required"></param>
      /// <param name="orderable"></param>
      /// <param name="cardinality"></param>
      /// <param name="updatability"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public virtual ccdp.cmisPropertyIdDefinitionType PropertyIdDefinition(string id, string localName, string displayName, string queryName, bool required, bool orderable, Core.enumCardinality cardinality, Core.enumUpdatability updatability)
        {
            return new ccdp.cmisPropertyIdDefinitionType(id, localName, _localNamespace, displayName, _isBaseType || string.IsNullOrEmpty(queryName) ? id : queryName, required, _inherited, _predefinedRequired.ContainsKey(id) ? _predefinedRequired[id] : !string.IsNullOrEmpty(queryName), orderable, cardinality, updatability) { Description = displayName };
        }
        /// <summary>
      /// Returns a PropertyIntegerDefinition-object
      /// </summary>
      /// <param name="id"></param>
      /// <param name="localName"></param>
      /// <param name="displayName"></param>
      /// <param name="queryName">ignored for base types</param>
      /// <param name="required"></param>
      /// <param name="orderable"></param>
      /// <param name="cardinality"></param>
      /// <param name="updatability"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public virtual ccdp.cmisPropertyIntegerDefinitionType PropertyIntegerDefinition(string id, string localName, string displayName, string queryName, bool required, bool orderable, Core.enumCardinality cardinality, Core.enumUpdatability updatability, long maxValue, long minValue)
        {
            return new ccdp.cmisPropertyIntegerDefinitionType(id, localName, _localNamespace, displayName, _isBaseType || string.IsNullOrEmpty(queryName) ? id : queryName, required, _inherited, _predefinedRequired.ContainsKey(id) ? _predefinedRequired[id] : !string.IsNullOrEmpty(queryName), orderable, cardinality, updatability) { Description = displayName, MaxValue = maxValue, MinValue = minValue };
        }
        /// <summary>
      /// Returns a PropertyStringDefinition-object
      /// </summary>
      /// <param name="id"></param>
      /// <param name="localName"></param>
      /// <param name="displayName"></param>
      /// <param name="queryName">ignored for base types</param>
      /// <param name="required"></param>
      /// <param name="orderable"></param>
      /// <param name="cardinality"></param>
      /// <param name="updatability"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public virtual ccdp.cmisPropertyStringDefinitionType PropertyStringDefinition(string id, string localName, string displayName, string queryName, bool required, bool orderable, Core.enumCardinality cardinality, Core.enumUpdatability updatability)
        {
            return new ccdp.cmisPropertyStringDefinitionType(id, localName, _localNamespace, displayName, _isBaseType || string.IsNullOrEmpty(queryName) ? id : queryName, required, _inherited, _predefinedRequired.ContainsKey(id) ? _predefinedRequired[id] : !string.IsNullOrEmpty(queryName), orderable, cardinality, updatability) { Description = displayName };
        }
        /// <summary>
      /// Returns a PropertyUriDefinition-object
      /// </summary>
      /// <param name="queryName">ignored for base types</param>
      /// <returns></returns>
      /// <remarks></remarks>
        public virtual ccdp.cmisPropertyUriDefinitionType PropertyUriDefinition(string id, string localName, string displayName, string queryName, bool required, bool orderable, Core.enumCardinality cardinality, Core.enumUpdatability updatability)
        {
            return new ccdp.cmisPropertyUriDefinitionType(id, localName, _localNamespace, displayName, _isBaseType || string.IsNullOrEmpty(queryName) ? id : queryName, required, _inherited, _predefinedRequired.ContainsKey(id) ? _predefinedRequired[id] : !string.IsNullOrEmpty(queryName), orderable, cardinality, updatability) { Description = displayName };
        }
        #endregion

        /// <summary>
      /// Returns PropertyIdDefinition-instance for the allowedChildObjectTypeIds-property
      /// </summary>
      /// <param name="localName"></param>
      /// <param name="queryName"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public ccdp.cmisPropertyDefinitionType AllowedChildObjectTypeIds(string localName = Constants.CmisPredefinedPropertyNames.AllowedChildObjectTypeIds, string queryName = Constants.CmisPredefinedPropertyNames.AllowedChildObjectTypeIds)
        {
            return PropertyIdDefinition(Constants.CmisPredefinedPropertyNames.AllowedChildObjectTypeIds, localName, "Id’s of the set of Object-types that can be created, moved or filed into this folder.", queryName, false, false, Core.enumCardinality.multi, Core.enumUpdatability.@readonly);
        }
        /// <summary>
      /// Returns PropertyIdDefinition-instance for the baseTypeId-property
      /// </summary>
      /// <param name="localName"></param>
      /// <param name="queryName"></param>
      /// <param name="orderable"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public ccdp.cmisPropertyDefinitionType BaseTypeId(string localName = Constants.CmisPredefinedPropertyNames.BaseTypeId, string queryName = Constants.CmisPredefinedPropertyNames.BaseTypeId, bool orderable = true)
        {
            return PropertyIdDefinition(Constants.CmisPredefinedPropertyNames.BaseTypeId, localName, "Id of the base object-type for the object", queryName, false, orderable, Core.enumCardinality.single, Core.enumUpdatability.@readonly);
        }
        /// <summary>
      /// Returns PropertyStringDefinition-instance for the changeToken-property
      /// </summary>
      /// <param name="localName"></param>
      /// <param name="queryName"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public ccdp.cmisPropertyDefinitionType ChangeToken(string localName = Constants.CmisPredefinedPropertyNames.ChangeToken, string queryName = Constants.CmisPredefinedPropertyNames.ChangeToken)
        {
            return PropertyStringDefinition(Constants.CmisPredefinedPropertyNames.ChangeToken, localName, "Opaque token used for optimistic locking & concurrency checking", queryName, false, false, Core.enumCardinality.single, Core.enumUpdatability.@readonly);
        }
        /// <summary>
      /// Returns PropertyStringDefinition-instance for the checkinComment-property
      /// </summary>
      /// <param name="localName"></param>
      /// <param name="queryName"></param>
      /// <param name="orderable"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public ccdp.cmisPropertyDefinitionType CheckinComment(string localName = Constants.CmisPredefinedPropertyNames.CheckinComment, string queryName = Constants.CmisPredefinedPropertyNames.CheckinComment, bool orderable = true)
        {
            return PropertyStringDefinition(Constants.CmisPredefinedPropertyNames.CheckinComment, localName, "Textual comment associated with the given version", queryName, false, orderable, Core.enumCardinality.single, Core.enumUpdatability.@readonly);
        }
        /// <summary>
      /// Returns PropertyStringDefinition-instance for the contentStreamFileName-property
      /// </summary>
      /// <param name="localName"></param>
      /// <param name="queryName"></param>
      /// <param name="orderable"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public ccdp.cmisPropertyDefinitionType ContentStreamFileName(string localName = Constants.CmisPredefinedPropertyNames.ContentStreamFileName, string queryName = Constants.CmisPredefinedPropertyNames.ContentStreamFileName, bool orderable = true)
        {
            return PropertyStringDefinition(Constants.CmisPredefinedPropertyNames.ContentStreamFileName, localName, "File name of the Content Stream", queryName, false, orderable, Core.enumCardinality.single, Core.enumUpdatability.@readonly);
        }
        /// <summary>
      /// Returns PropertyIdDefinition-instance for the contentStreamId-property
      /// </summary>
      /// <param name="localName"></param>
      /// <param name="queryName"></param>
      /// <param name="orderable"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public ccdp.cmisPropertyDefinitionType ContentStreamId(string localName = Constants.CmisPredefinedPropertyNames.ContentStreamId, string queryName = Constants.CmisPredefinedPropertyNames.ContentStreamId, bool orderable = true)
        {
            return PropertyIdDefinition(Constants.CmisPredefinedPropertyNames.ContentStreamId, localName, "Id of the stream", queryName, false, orderable, Core.enumCardinality.single, Core.enumUpdatability.@readonly);
        }
        /// <summary>
      /// Returns PropertyIntegerDefinition-instance for the contentStreamLength-property
      /// </summary>
      /// <param name="localName"></param>
      /// <param name="queryName"></param>
      /// <param name="orderable"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public ccdp.cmisPropertyDefinitionType ContentStreamLength(string localName = Constants.CmisPredefinedPropertyNames.ContentStreamLength, string queryName = Constants.CmisPredefinedPropertyNames.ContentStreamLength, bool orderable = true)
        {
            // the maximum is limited by the int32 type
            return PropertyIntegerDefinition(Constants.CmisPredefinedPropertyNames.ContentStreamLength, localName, "Length of the content stream (in bytes).", queryName, false, orderable, Core.enumCardinality.single, Core.enumUpdatability.@readonly, int.MaxValue, 0L);
        }
        /// <summary>
      /// Returns PropertyStringDefinition-instance for the contentStreamMimeType-property
      /// </summary>
      /// <param name="localName"></param>
      /// <param name="queryName"></param>
      /// <param name="orderable"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public ccdp.cmisPropertyDefinitionType ContentStreamMimeType(string localName = Constants.CmisPredefinedPropertyNames.ContentStreamMimeType, string queryName = Constants.CmisPredefinedPropertyNames.ContentStreamMimeType, bool orderable = true)
        {
            return PropertyStringDefinition(Constants.CmisPredefinedPropertyNames.ContentStreamMimeType, localName, "MIME type of the Content Stream", queryName, false, orderable, Core.enumCardinality.single, Core.enumUpdatability.@readonly);
        }
        /// <summary>
      /// Returns PropertyStringDefinition-instance for the createdBy-property
      /// </summary>
      /// <param name="localName"></param>
      /// <param name="queryName"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public ccdp.cmisPropertyDefinitionType CreatedBy(string localName = Constants.CmisPredefinedPropertyNames.CreatedBy, string queryName = Constants.CmisPredefinedPropertyNames.CreatedBy)
        {
            return PropertyStringDefinition(Constants.CmisPredefinedPropertyNames.CreatedBy, localName, "User who created the object.", queryName, false, true, Core.enumCardinality.single, Core.enumUpdatability.@readonly);
        }
        /// <summary>
      /// Returns PropertyDateTimeDefinition-instance for the creationDate-property
      /// </summary>
      /// <param name="localName"></param>
      /// <param name="queryName"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public ccdp.cmisPropertyDefinitionType CreationDate(string localName = Constants.CmisPredefinedPropertyNames.CreationDate, string queryName = Constants.CmisPredefinedPropertyNames.CreationDate)
        {
            return PropertyDateTimeDefinition(Constants.CmisPredefinedPropertyNames.CreationDate, localName, "DateTime when the object was created.", queryName, false, true, Core.enumCardinality.single, Core.enumUpdatability.@readonly);
        }
        /// <summary>
      /// Returns PropertyStringDefinition-instance for the description-property
      /// </summary>
      /// <param name="localName"></param>
      /// <param name="queryName"></param>
      /// <param name="orderable"></param>
      /// <param name="updatability"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public ccdp.cmisPropertyDefinitionType Description(string localName = Constants.CmisPredefinedPropertyNames.Description, string queryName = Constants.CmisPredefinedPropertyNames.Description, bool orderable = true, Core.enumUpdatability updatability = Core.enumUpdatability.readwrite)
        {
            return PropertyStringDefinition(Constants.CmisPredefinedPropertyNames.Description, localName, "Description of the object", queryName, false, orderable, Core.enumCardinality.single, updatability);
        }
        /// <summary>
      /// Returns PropertyStringDefinition-instance for the foreignChangeToken-property
      /// </summary>
      /// <param name="localName"></param>
      /// <param name="queryName"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public ccdp.cmisPropertyDefinitionType ForeignChangeToken(string localName = Constants.CmisPredefinedPropertyNames.Extensions.ForeignChangeToken, string queryName = Constants.CmisPredefinedPropertyNames.Extensions.ForeignChangeToken)
        {
            return PropertyStringDefinition(Constants.CmisPredefinedPropertyNames.Extensions.ForeignChangeToken, localName, "Opaque token used for optimistic locking & concurrency checking in coupled cmis-systems", queryName, false, false, Core.enumCardinality.single, Core.enumUpdatability.readwrite);
        }
        /// <summary>
      /// Returns PropertyIdDefinition-instance for the foreignChangeToken-property
      /// </summary>
      /// <param name="localName"></param>
      /// <param name="queryName"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public ccdp.cmisPropertyDefinitionType ForeignObjectId(string localName = Constants.CmisPredefinedPropertyNames.Extensions.ForeignObjectId, string queryName = Constants.CmisPredefinedPropertyNames.Extensions.ForeignObjectId)
        {
            return PropertyIdDefinition(Constants.CmisPredefinedPropertyNames.Extensions.ForeignObjectId, localName, "ObjectId for an object in an external cmis-system", queryName, false, false, Core.enumCardinality.single, Core.enumUpdatability.readwrite);
        }
        /// <summary>
      /// Returns PropertyBooleanDefinition-instance for the isImmutable-property
      /// </summary>
      /// <param name="localName"></param>
      /// <param name="queryName"></param>
      /// <param name="orderable"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public ccdp.cmisPropertyDefinitionType IsImmutable(string localName = Constants.CmisPredefinedPropertyNames.IsImmutable, string queryName = Constants.CmisPredefinedPropertyNames.IsImmutable, bool orderable = true)
        {
            return PropertyBooleanDefinition(Constants.CmisPredefinedPropertyNames.IsImmutable, localName, "TRUE if the repository MUST throw an error at any attempt to update or delete the object.", queryName, false, orderable, Core.enumCardinality.single, Core.enumUpdatability.@readonly);
        }
        /// <summary>
      /// Returns PropertyBooleanDefinition-instance for the IsLatestMajorVersion-property
      /// </summary>
      /// <param name="localName"></param>
      /// <param name="queryName"></param>
      /// <param name="orderable"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public ccdp.cmisPropertyDefinitionType IsLatestMajorVersion(string localName = Constants.CmisPredefinedPropertyNames.IsLatestMajorVersion, string queryName = Constants.CmisPredefinedPropertyNames.IsLatestMajorVersion, bool orderable = true)
        {
            return PropertyBooleanDefinition(Constants.CmisPredefinedPropertyNames.IsLatestMajorVersion, localName, "TRUE if the version designated as a major version and is the version, that has the most recent last modiﬁcation date", queryName, false, orderable, Core.enumCardinality.single, Core.enumUpdatability.@readonly);
        }
        /// <summary>
      /// Returns PropertyBooleanDefinition-instance for the isLatestVersion-property
      /// </summary>
      /// <param name="localName"></param>
      /// <param name="queryName"></param>
      /// <param name="orderable"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public ccdp.cmisPropertyDefinitionType IsLatestVersion(string localName = Constants.CmisPredefinedPropertyNames.IsLatestVersion, string queryName = Constants.CmisPredefinedPropertyNames.IsLatestVersion, bool orderable = true)
        {
            return PropertyBooleanDefinition(Constants.CmisPredefinedPropertyNames.IsLatestVersion, localName, "TRUE if the version that has the most recent last modiﬁcation date", queryName, false, orderable, Core.enumCardinality.single, Core.enumUpdatability.@readonly);
        }
        /// <summary>
      /// Returns PropertyBooleanDefinition-instance for the isMajorVersion-property
      /// </summary>
      /// <param name="localName"></param>
      /// <param name="queryName"></param>
      /// <param name="orderable"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public ccdp.cmisPropertyDefinitionType IsMajorVersion(string localName = Constants.CmisPredefinedPropertyNames.IsMajorVersion, string queryName = Constants.CmisPredefinedPropertyNames.IsMajorVersion, bool orderable = true)
        {
            return PropertyBooleanDefinition(Constants.CmisPredefinedPropertyNames.IsMajorVersion, localName, "TRUE if the version designated as a major version", queryName, false, orderable, Core.enumCardinality.single, Core.enumUpdatability.@readonly);
        }
        /// <summary>
      /// Returns PropertyBooleanDefinition-instance for the isPrivateWorkingCopy-property
      /// </summary>
      /// <param name="localName"></param>
      /// <param name="queryName"></param>
      /// <param name="orderable"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public ccdp.cmisPropertyDefinitionType IsPrivateWorkingCopy(string localName = Constants.CmisPredefinedPropertyNames.IsPrivateWorkingCopy, string queryName = Constants.CmisPredefinedPropertyNames.IsPrivateWorkingCopy, bool orderable = true)
        {
            return PropertyBooleanDefinition(Constants.CmisPredefinedPropertyNames.IsPrivateWorkingCopy, localName, "TRUE if the document object is a Private Working Copy", queryName, false, orderable, Core.enumCardinality.single, Core.enumUpdatability.@readonly);
        }
        /// <summary>
      /// Returns PropertyBooleanDefinition-instance for the isVersionSeriesCheckedOut-property
      /// </summary>
      /// <param name="localName"></param>
      /// <param name="queryName"></param>
      /// <param name="orderable"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public ccdp.cmisPropertyDefinitionType IsVersionSeriesCheckedOut(string localName = Constants.CmisPredefinedPropertyNames.IsVersionSeriesCheckedOut, string queryName = Constants.CmisPredefinedPropertyNames.IsVersionSeriesCheckedOut, bool orderable = true)
        {
            return PropertyBooleanDefinition(Constants.CmisPredefinedPropertyNames.IsVersionSeriesCheckedOut, localName, "TRUE if there currenly exists a Private Working Copy for this version series", queryName, false, orderable, Core.enumCardinality.single, Core.enumUpdatability.@readonly);
        }
        /// <summary>
      /// Returns PropertyDateTimeDefinition-instance for the lastModification-property
      /// </summary>
      /// <param name="localName"></param>
      /// <param name="queryName"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public ccdp.cmisPropertyDefinitionType LastModificationDate(string localName = Constants.CmisPredefinedPropertyNames.LastModificationDate, string queryName = Constants.CmisPredefinedPropertyNames.LastModificationDate)
        {
            return PropertyDateTimeDefinition(Constants.CmisPredefinedPropertyNames.LastModificationDate, localName, "DateTime when the object was last modified.", queryName, false, true, Core.enumCardinality.single, Core.enumUpdatability.@readonly);
        }
        /// <summary>
      /// Returns PropertyStringDefinition-instance for the lastModifiedBy-property
      /// </summary>
      /// <param name="localName"></param>
      /// <param name="queryName"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public ccdp.cmisPropertyDefinitionType LastModifiedBy(string localName = Constants.CmisPredefinedPropertyNames.LastModifiedBy, string queryName = Constants.CmisPredefinedPropertyNames.LastModifiedBy)
        {
            return PropertyStringDefinition(Constants.CmisPredefinedPropertyNames.LastModifiedBy, localName, "User who last modified the object.", queryName, false, true, Core.enumCardinality.single, Core.enumUpdatability.@readonly);
        }
        /// <summary>
      /// Returns PropertyStringDefinition-instance for the name-property
      /// </summary>
      /// <param name="localName"></param>
      /// <param name="queryName">ignored for base types</param>
      /// <param name="orderable"></param>
      /// <param name="updatability"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public ccdp.cmisPropertyDefinitionType Name(string localName = Constants.CmisPredefinedPropertyNames.Name, string queryName = Constants.CmisPredefinedPropertyNames.Name, bool orderable = true, Core.enumUpdatability updatability = Core.enumUpdatability.readwrite)
        {
            return PropertyStringDefinition(Constants.CmisPredefinedPropertyNames.Name, localName, "Name of the object", queryName, true, orderable, Core.enumCardinality.single, updatability);
        }
        /// <summary>
      /// Returns PropertyIdDefinition-instance for the objectId-property
      /// </summary>
      /// <param name="localName"></param>
      /// <param name="queryName"></param>
      /// <param name="orderable"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public ccdp.cmisPropertyDefinitionType ObjectId(string localName = Constants.CmisPredefinedPropertyNames.ObjectId, string queryName = Constants.CmisPredefinedPropertyNames.ObjectId, bool orderable = true)
        {
            return PropertyIdDefinition(Constants.CmisPredefinedPropertyNames.ObjectId, localName, "Id of the object", queryName, false, orderable, Core.enumCardinality.single, Core.enumUpdatability.@readonly);
        }
        /// <summary>
      /// Returns PropertyIdDefinition-instance for the objectTypeId-property
      /// </summary>
      /// <param name="localName"></param>
      /// <param name="queryName"></param>
      /// <param name="orderable"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public ccdp.cmisPropertyDefinitionType ObjectTypeId(string localName = Constants.CmisPredefinedPropertyNames.ObjectTypeId, string queryName = Constants.CmisPredefinedPropertyNames.ObjectTypeId, bool orderable = true)
        {
            return PropertyIdDefinition(Constants.CmisPredefinedPropertyNames.ObjectTypeId, localName, "Id of the object’s type", queryName, true, orderable, Core.enumCardinality.single, Core.enumUpdatability.oncreate);
        }
        /// <summary>
      /// Returns PropertyIdDefinition-instance for the parentId-property
      /// </summary>
      /// <param name="localName"></param>
      /// <param name="queryName"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public ccdp.cmisPropertyDefinitionType ParentId(string localName = Constants.CmisPredefinedPropertyNames.ParentId, string queryName = Constants.CmisPredefinedPropertyNames.ParentId)
        {
            return PropertyIdDefinition(Constants.CmisPredefinedPropertyNames.ParentId, localName, "ID of the parent folder of the folder.", queryName, false, false, Core.enumCardinality.single, Core.enumUpdatability.@readonly);
        }
        /// <summary>
      /// Returns PropertyStringDefinition-instance for the path-property
      /// </summary>
      /// <param name="localName"></param>
      /// <param name="queryName"></param>
      /// <param name="orderable"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public ccdp.cmisPropertyDefinitionType Path(string localName = Constants.CmisPredefinedPropertyNames.Path, string queryName = Constants.CmisPredefinedPropertyNames.Path, bool orderable = true)
        {
            return PropertyStringDefinition(Constants.CmisPredefinedPropertyNames.Path, localName, "The fully qualified path to this folder.", queryName, false, orderable, Core.enumCardinality.single, Core.enumUpdatability.@readonly);
        }
        /// <summary>
      /// Returns PropertyStringDefinition-instance for the policyText-property
      /// </summary>
      /// <param name="localName"></param>
      /// <param name="queryName"></param>
      /// <param name="orderable"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public ccdp.cmisPropertyDefinitionType PolicyText(string localName = Constants.CmisPredefinedPropertyNames.PolicyText, string queryName = Constants.CmisPredefinedPropertyNames.PolicyText, bool orderable = true)
        {
            return PropertyStringDefinition(Constants.CmisPredefinedPropertyNames.PolicyText, localName, "User-friendly description of the policy", queryName, false, orderable, Core.enumCardinality.single, Core.enumUpdatability.@readonly);
        }
        /// <summary>
      /// Returns the PropertyDateTimeDefinition-instance for the rm_destructionDate-property
      /// </summary>
        public ccdp.cmisPropertyDefinitionType RM_DestructionDate(bool orderable, bool queryable, string localName = Constants.CmisPredefinedPropertyNames.RM_DestructionDate, string queryName = Constants.CmisPredefinedPropertyNames.RM_DestructionDate)
        {
            ccdp.cmisPropertyDefinitionType retVal = PropertyDateTimeDefinition(Constants.CmisPredefinedPropertyNames.RM_DestructionDate, localName, "Destruction date", queryName, false, orderable, Core.enumCardinality.single, Core.enumUpdatability.readwrite);
            retVal.Queryable = queryable;
            return retVal;
        }
        /// <summary>
      /// Returns the PropertyDateTimeDefinition-instance for the rm_expirationDate-property
      /// </summary>
        public ccdp.cmisPropertyDefinitionType RM_ExpirationDate(bool orderable, string localName = Constants.CmisPredefinedPropertyNames.RM_ExpirationDate, string queryName = Constants.CmisPredefinedPropertyNames.RM_ExpirationDate)
        {
            return PropertyDateTimeDefinition(Constants.CmisPredefinedPropertyNames.RM_ExpirationDate, localName, "Expiration date", queryName, false, orderable, Core.enumCardinality.single, Core.enumUpdatability.readwrite);
        }
        /// <summary>
      /// Returns PropertyStringDefinition-instance for the rm_holdIds-property
      /// </summary>
        public ccdp.cmisPropertyDefinitionType RM_HoldIds(string localName = Constants.CmisPredefinedPropertyNames.RM_HoldIds, string queryName = Constants.CmisPredefinedPropertyNames.RM_HoldIds, bool queryable = true, Core.enumUpdatability updatability = Core.enumUpdatability.readwrite)
        {
            ccdp.cmisPropertyDefinitionType retVal = PropertyStringDefinition(Constants.CmisPredefinedPropertyNames.RM_HoldIds, localName, "Hold identifiers", queryName, false, false, Core.enumCardinality.multi, updatability);
            retVal.Queryable = queryable;
            return retVal;
        }
        /// <summary>
      /// Returns PropertyDateTimeDefinition-instance for the rm_startOfRetention-property
      /// </summary>
        public ccdp.cmisPropertyDefinitionType RM_StartOfRetention(bool orderable, bool queryable, string localName = Constants.CmisPredefinedPropertyNames.RM_StartOfRetention, string queryName = Constants.CmisPredefinedPropertyNames.RM_StartOfRetention, Core.enumUpdatability updatability = Core.enumUpdatability.readwrite)
        {
            ccdp.cmisPropertyDefinitionType retVal = PropertyDateTimeDefinition(Constants.CmisPredefinedPropertyNames.RM_StartOfRetention, localName, "Start of retention", queryName, false, orderable, Core.enumCardinality.single, updatability);
            retVal.Queryable = queryable;
            return retVal;
        }
        /// <summary>
      /// Returns PropertyIdDefinition-instance for the secondaryObjectTypeIds-property
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public ccdp.cmisPropertyDefinitionType SecondaryObjectTypeIds(string localName = Constants.CmisPredefinedPropertyNames.SecondaryObjectTypeIds, string queryName = Constants.CmisPredefinedPropertyNames.SecondaryObjectTypeIds, Core.enumUpdatability updatability = Core.enumUpdatability.@readonly)
        {
            return PropertyIdDefinition(Constants.CmisPredefinedPropertyNames.SecondaryObjectTypeIds, localName, "Ids of the object’s secondary types", queryName, false, false, Core.enumCardinality.multi, updatability);
        }
        /// <summary>
      /// Returns PropertyIdDefinition-instance for the sourceId-property
      /// </summary>
      /// <param name="localName"></param>
      /// <param name="queryName"></param>
      /// <param name="orderable"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public ccdp.cmisPropertyDefinitionType SourceId(string localName = Constants.CmisPredefinedPropertyNames.SourceId, string queryName = Constants.CmisPredefinedPropertyNames.SourceId, bool orderable = true)
        {
            return PropertyIdDefinition(Constants.CmisPredefinedPropertyNames.SourceId, localName, "ID of the source object of the relationship.", queryName, false, orderable, Core.enumCardinality.single, Core.enumUpdatability.oncreate);
        }
        /// <summary>
      /// Returns PropertyIntegerDefinition-instance for the syncRequired-property
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public ccdp.cmisPropertyDefinitionType SyncRequired(string localName = Constants.CmisPredefinedPropertyNames.Extensions.SyncRequired, string queryName = Constants.CmisPredefinedPropertyNames.Extensions.SyncRequired, long maxValue = (long)enumSyncRequired.suspendedBiDirectional, long minValue = (long)enumSyncRequired.custom)
        {
            return PropertyIntegerDefinition(Constants.CmisPredefinedPropertyNames.Extensions.SyncRequired, localName, "Pending synchronization with an external cmis-system", queryName, false, false, Core.enumCardinality.single, Core.enumUpdatability.readwrite, maxValue, minValue);
        }
        /// <summary>
      /// Returns PropertyIdDefinition-instance for the targetId-property
      /// </summary>
      /// <param name="localName"></param>
      /// <param name="queryName"></param>
      /// <param name="orderable"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public ccdp.cmisPropertyDefinitionType TargetId(string localName = Constants.CmisPredefinedPropertyNames.TargetId, string queryName = Constants.CmisPredefinedPropertyNames.TargetId, bool orderable = true)
        {
            return PropertyIdDefinition(Constants.CmisPredefinedPropertyNames.TargetId, localName, "ID of the target object of the relationship.", queryName, false, orderable, Core.enumCardinality.single, Core.enumUpdatability.oncreate);
        }
        /// <summary>
      /// Returns PropertyStringDefinition-instance for the versionLabel-property
      /// </summary>
      /// <param name="localName"></param>
      /// <param name="queryName"></param>
      /// <param name="orderable"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public ccdp.cmisPropertyDefinitionType VersionLabel(string localName = Constants.CmisPredefinedPropertyNames.VersionLabel, string queryName = Constants.CmisPredefinedPropertyNames.VersionLabel, bool orderable = true)
        {
            return PropertyStringDefinition(Constants.CmisPredefinedPropertyNames.VersionLabel, localName, "Textual description the position of an individual object with respect to the version series", queryName, false, orderable, Core.enumCardinality.single, Core.enumUpdatability.@readonly);
        }
        /// <summary>
      /// Returns PropertyStringDefinition-instance for the versionSeriesCheckedOutBy-property
      /// </summary>
      /// <param name="localName"></param>
      /// <param name="queryName"></param>
      /// <param name="orderable"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public ccdp.cmisPropertyDefinitionType VersionSeriesCheckedOutBy(string localName = Constants.CmisPredefinedPropertyNames.VersionSeriesCheckedOutBy, string queryName = Constants.CmisPredefinedPropertyNames.VersionSeriesCheckedOutBy, bool orderable = true)
        {
            return PropertyStringDefinition(Constants.CmisPredefinedPropertyNames.VersionSeriesCheckedOutBy, localName, "An identiﬁer for the user who created the Private Working Copy", queryName, false, orderable, Core.enumCardinality.single, Core.enumUpdatability.@readonly);
        }
        /// <summary>
      /// Returns PropertyIdDefinition-instance for the versionSeriesCheckedOutId-property
      /// </summary>
      /// <param name="localName"></param>
      /// <param name="queryName"></param>
      /// <param name="orderable"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public ccdp.cmisPropertyDefinitionType VersionSeriesCheckedOutId(string localName = Constants.CmisPredefinedPropertyNames.VersionSeriesCheckedOutId, string queryName = Constants.CmisPredefinedPropertyNames.VersionSeriesCheckedOutId, bool orderable = true)
        {
            return PropertyIdDefinition(Constants.CmisPredefinedPropertyNames.VersionSeriesCheckedOutId, localName, "The object id for the Private Working Copy or \"not set\"", queryName, false, orderable, Core.enumCardinality.single, Core.enumUpdatability.@readonly);
        }
        /// <summary>
      /// Returns PropertyIdDefinition-instance for the versionSeriesId-property
      /// </summary>
      /// <param name="localName"></param>
      /// <param name="queryName"></param>
      /// <param name="orderable"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public ccdp.cmisPropertyDefinitionType VersionSeriesId(string localName = Constants.CmisPredefinedPropertyNames.VersionSeriesId, string queryName = Constants.CmisPredefinedPropertyNames.VersionSeriesId, bool orderable = true)
        {
            return PropertyIdDefinition(Constants.CmisPredefinedPropertyNames.VersionSeriesId, localName, "Id of the version series for this object.", queryName, false, orderable, Core.enumCardinality.single, Core.enumUpdatability.@readonly);
        }
    }
}