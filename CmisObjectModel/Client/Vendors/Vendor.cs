using System;
using System.Collections.Generic;
using System.Linq;
using CmisObjectModel.Constants;
using ccdp = CmisObjectModel.Core.Definitions.Properties;
using ccdt = CmisObjectModel.Core.Definitions.Types;
using cmr = CmisObjectModel.Messaging.Requests;
using Microsoft.VisualBasic.CompilerServices;

namespace CmisObjectModel.Client.Vendors
{
    /// <summary>
   /// Base class for all vendor-extensions
   /// </summary>
   /// <remarks>
   /// Support 
   /// </remarks>
    public class Vendor
    {

        protected static string _objectTypeIdKey = CmisPredefinedPropertyNames.ObjectTypeId.ToLowerInvariant();
        protected Contracts.ICmisClient _client;
        public Vendor(Contracts.ICmisClient client)
        {
            _client = client;
        }

        #region Helper classes
        public class State
        {

            #region Constructors
            public State(string repositoryId)
            {
                RepositoryId = repositoryId;
            }

            public State(string repositoryId, string typeId)
            {
                RepositoryId = repositoryId;
                if (!string.IsNullOrEmpty(typeId))
                {
                    var typeIds = typeId.Split(',');
                    int length = typeIds.Length - 1;

                    TypeId = typeIds[0];
                    if (length > 0)
                    {
                        SecondaryTypeIds = (string[])Array.CreateInstance(typeof(string), length);
                        Array.Copy(typeIds, 1, SecondaryTypeIds, 0, length);
                    }
                }
            }
            #endregion

            /// <summary>
         /// Adds a new delegate to rollback changes
         /// </summary>
         /// <param name="value"></param>
         /// <remarks></remarks>
            public void AddRollbackAction(Action value)
            {
                if (value is not null)
                    _rollbackActions.Push(value);
            }

            private Stack<Action> _rollbackActions = new Stack<Action>();
            /// <summary>
         /// Executes given rollback-actions (via AddRollbackAction()) in reverse order
         /// </summary>
         /// <remarks></remarks>
            public void Rollback()
            {
                while (_rollbackActions.Count > 0)
                    _rollbackActions.Pop().Invoke();
            }

            public readonly string[] SecondaryTypeIds;
            public readonly string TypeId;

            public readonly string RepositoryId;
        }
        #endregion

        #region SecondaryObjectType-Support
        /// <summary>
      /// Selects from secondaryTypes the typeIds defined in secondaryTypeFilter and adds their propertyDefinitions
      /// to the result parameter, respecting the optional given propertyFilter
      /// </summary>
      /// <param name="secondaryTypes"></param>
      /// <param name="secondaryTypeFilter"></param>
      /// <param name="propertyFilter">Set of supported propertyDefinitionId-namespaces.
      /// If filter is set to null or contains '*' all namespaces are accepted.</param>
      /// <remarks></remarks>
        protected Dictionary<string, ccdp.cmisPropertyDefinitionType> AddSecondaryProperties(Dictionary<string, ccdp.cmisPropertyDefinitionType> result, Dictionary<string, ccdt.cmisTypeDefinitionType> secondaryTypes, string[] secondaryTypeFilter, HashSet<string> propertyFilter = null)
        {
            // collect propertyDefinitions from selected types
            foreach (string typeId in secondaryTypeFilter)
            {
                if (!string.IsNullOrEmpty(typeId) && secondaryTypes.ContainsKey(typeId))
                {
                    var propertyDefinitions = secondaryTypes[typeId].PropertyDefinitions;

                    if (propertyDefinitions is not null)
                    {
                        foreach (ccdp.cmisPropertyDefinitionType propertyDefinition in propertyDefinitions)
                        {
                            string key = propertyDefinition.Id.ToLowerInvariant();

                            if (!result.ContainsKey(key))
                            {
                                if (propertyFilter is null || propertyFilter.Contains("*"))
                                {
                                    // all prefixes allowed
                                    result.Add(key, propertyDefinition);
                                }
                                else
                                {
                                    // check for defined prefixes
                                    int indexOf = key.IndexOf(":");
                                    if (propertyFilter.Contains(indexOf <= 0 ? "" : key.Substring(0, indexOf)))
                                    {
                                        result.Add(key, propertyDefinition);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
      /// First key: repositoryId, second key: secondaryObjectType identifier
      /// </summary>
      /// <remarks></remarks>
        protected Dictionary<string, Dictionary<string, ccdt.cmisTypeDefinitionType>> _secondaryObjectTypes = new Dictionary<string, Dictionary<string, ccdt.cmisTypeDefinitionType>>();
        protected Dictionary<string, ccdt.cmisTypeDefinitionType> GetSecondaryObjectTypes(string repositoryId)
        {
            lock (_secondaryObjectTypes)
            {
                if (_secondaryObjectTypes.ContainsKey(repositoryId))
                {
                    return _secondaryObjectTypes[repositoryId];
                }
                else
                {
                    var retVal = GetTypeDescendants(repositoryId, "cmis:secondary");
                    _secondaryObjectTypes.Add(repositoryId, retVal);

                    return retVal;
                }
            }
        }

        /// <summary>
      /// Collects all propertyDefinition of the secondary types (secondaryObjectTypes)
      /// </summary>
      /// <param name="state"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        protected virtual Dictionary<string, ccdp.cmisPropertyDefinitionType> GetSecondaryTypeProperties(State state)
        {
            return AddSecondaryProperties(new Dictionary<string, ccdp.cmisPropertyDefinitionType>(), GetSecondaryObjectTypes(state.RepositoryId), state.SecondaryTypeIds);
        }

        /// <summary>
      /// Returns type definitions derived (directly or indirectly) from parentTypeId
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="parentTypeId"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        protected Dictionary<string, ccdt.cmisTypeDefinitionType> GetTypeDescendants(string repositoryId, string parentTypeId)
        {
            var retVal = new Dictionary<string, ccdt.cmisTypeDefinitionType>();

            {
                var withBlock = _client.GetTypeDescendants(new cmr.getTypeDescendants() { RepositoryId = repositoryId, TypeId = parentTypeId, IncludePropertyDefinitions = true });
                if (withBlock.Exception is null && withBlock.Response.Types is not null)
                {
                    var types = new Queue<Messaging.cmisTypeContainer[]>();

                    // first level
                    types.Enqueue(withBlock.Response.Types);
                    while (types.Count > 0)
                    {
                        foreach (Messaging.cmisTypeContainer cmisTypeContainer in types.Dequeue())
                        {
                            if (cmisTypeContainer is not null)
                            {
                                var cmisType = cmisTypeContainer.Type;
                                var children = cmisTypeContainer.Children;

                                if (cmisType is not null)
                                {
                                    string id = cmisType.Id;

                                    if (!string.IsNullOrEmpty(id))
                                    {
                                        var keys = new string[] { id, id.ToLowerInvariant(), id.ToUpperInvariant() };

                                        foreach (string key in keys)
                                        {
                                            if (!retVal.ContainsKey(key))
                                                retVal.Add(key, cmisType);
                                        }
                                    }
                                    // children must be registered as well (next level)
                                    if (children is not null)
                                        types.Enqueue(children);
                                }
                            }
                        }
                    }
                }
            }

            return retVal;
        }
        #endregion

        #region Vendor specific for cmisPropertyCollectionType
        public State BeginRequest(string repositoryId)
        {
            return BeginRequest(new State(repositoryId, null), null);
        }
        /// <summary>
      /// Allows vendor specific action before request dealing with cmisObjects
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="properties"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public State BeginRequest(string repositoryId, Core.Collections.cmisPropertiesType properties, Action rollbackAction)
        {
            var objectTypeIdProperty = properties is null ? null : properties.FindProperty(CmisPredefinedPropertyNames.ObjectTypeId);
            string typeId = objectTypeIdProperty is null ? null : Conversions.ToString(objectTypeIdProperty.Value);
            var state = new State(repositoryId, typeId);

            state.AddRollbackAction(rollbackAction);
            // remove secondaryTypeIds
            if (state.SecondaryTypeIds is not null)
            {
                var oldTypeId = objectTypeIdProperty.SetValueSilent(state.TypeId);
                state.AddRollbackAction(() => objectTypeIdProperty.SetValueSilent(oldTypeId));
            }
            return BeginRequest(state, properties);
        }
        protected virtual State BeginRequest(State state, Core.Collections.cmisPropertiesType properties)
        {
            return state;
        }

        /// <summary>
      /// Allows vendor specific action to process the response dealing with cmisObjects
      /// </summary>
      /// <param name="state"></param>
      /// <param name="propertyCollections"></param>
      /// <remarks></remarks>
        public virtual void EndRequest(State state, params Core.Collections.cmisPropertiesType[] propertyCollections)
        {
        }
        #endregion

        #region Vendor specific for cmisTypeDefinitionType
        /// <summary>
      /// Allows vendor specific action before request dealing with cmisTypes
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="typeId"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public virtual State BeginRequest(string repositoryId, ref string typeId)
        {
            var retVal = new State(repositoryId, typeId);

            // the given typeId is a composite typeId, starting with the original typeId followed by secondaryTypeIds
            if (retVal.SecondaryTypeIds is not null)
                typeId = retVal.TypeId;

            return retVal;
        }

        /// <summary>
      /// Allows vendor specific action to process the response dealing with cmisTypes
      /// </summary>
      /// <param name="state"></param>
      /// <param name="type"></param>
      /// <remarks></remarks>
        public virtual void EndRequest(State state, ccdt.cmisTypeDefinitionType type)
        {
            var secondaryTypeProperties = state.SecondaryTypeIds is null ? null : GetSecondaryTypeProperties(state);

            if (secondaryTypeProperties is not null)
            {
                var propertyDefinitions = type.GetPropertyDefinitions(true);
                int count = propertyDefinitions.Count;

                foreach (KeyValuePair<string, ccdp.cmisPropertyDefinitionType> de in secondaryTypeProperties)
                {
                    if (!propertyDefinitions.ContainsKey(de.Key))
                    {
                        propertyDefinitions.Add(de.Key, de.Value);
                    }
                }
                // at least one aspect property has been added
                if (count != propertyDefinitions.Count)
                {
                    type.PropertyDefinitions = propertyDefinitions.Values.ToArray();
                }
            }
        }
        #endregion

        #region Patches
        /// <summary>
      /// Allows vendor specific action to patch values within the propertyCollection
      /// </summary>
      /// <remarks>
      /// For example: in Agorum the cmis:versionSeriesId of a pwc differs from the cmis:versionSeriesId
      /// of the checkedin-versions. High level code in this assembly (CmisObjectModel.Client.CmisDataModelObject)
      /// uses this method to make sure the cmis:versionSeriesId of both are the same.
      /// </remarks>
        public virtual void PatchProperties(Core.cmisRepositoryInfoType repositoryInfo, Core.Collections.cmisPropertiesType properties)
        {
        }

        public virtual void PatchProperties(Core.cmisRepositoryInfoType repositoryInfo, Core.cmisObjectType cmisObject)
        {
            if (cmisObject is not null)
                PatchProperties(repositoryInfo, cmisObject.Properties);
        }
        #endregion

    }
}