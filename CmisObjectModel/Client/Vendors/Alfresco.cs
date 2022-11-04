using System.Collections.Generic;
using System.Data;
using System.Linq;
using cce = CmisObjectModel.Constants.ExtendedProperties;
using ccdt = CmisObjectModel.Core.Definitions.Types;
using ce = CmisObjectModel.Extensions;
using cea = CmisObjectModel.Extensions.Alfresco;
using Microsoft.VisualBasic.CompilerServices;

namespace CmisObjectModel.Client.Vendors
{
    /// <summary>
   /// Support for Alfresco extensions like aspects
   /// </summary>
   /// <remarks></remarks>
    public class Alfresco : Vendor
    {

        #region Constructors
        /// <summary>
      /// First key: repositoryId, second key: aspect identifier
      /// </summary>
      /// <remarks></remarks>
        private Dictionary<string, Dictionary<string, ccdt.cmisTypeDefinitionType>> _aspects = new Dictionary<string, Dictionary<string, ccdt.cmisTypeDefinitionType>>();
        public Alfresco(Contracts.ICmisClient client) : base(client)
        {

            _propertyFilter = new HashSet<string>() { "cm", "exif" };
        }
        #endregion

        /// <summary>
      /// Reads the defined aspects in an Alfresco repository
      /// </summary>
      /// <remarks></remarks>
        private Dictionary<string, ccdt.cmisTypeDefinitionType> GetAspects(string repositoryId)
        {
            lock (_aspects)
            {
                if (_aspects.ContainsKey(repositoryId))
                {
                    return _aspects[repositoryId];
                }
                else
                {
                    // in Alfresco aspect types are derived from policy type 'P:cmisext:aspects'
                    var retVal = GetTypeDescendants(repositoryId, "P:cmisext:aspects");
                    _aspects.Add(repositoryId, retVal);

                    return retVal;
                }
            }
        }

        private HashSet<string> _propertyFilter;
        /// <summary>
      /// Collects all propertyDefinition of the secondary types (aspects)
      /// </summary>
      /// <param name="state"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        protected override Dictionary<string, Core.Definitions.Properties.cmisPropertyDefinitionType> GetSecondaryTypeProperties(State state)
        {
            return AddSecondaryProperties(base.GetSecondaryTypeProperties(state), GetAspects(state.RepositoryId), state.SecondaryTypeIds, _propertyFilter);
        }

        protected override State BeginRequest(State state, Core.Collections.cmisPropertiesType properties)
        {
            var currentProperties = properties is null ? null : properties.Properties;

            if (currentProperties is not null)
            {
                var setAspects = new Dictionary<string, List<Core.Properties.cmisProperty>>();
                var aspects = GetAspects(state.RepositoryId);
                var objectProperties = new List<Core.Properties.cmisProperty>();

                // find aspects if supported (since alfresco 4.2d the repository delivers aspects as secondaryObjectTypes)
                if (aspects.Count > 0)
                {
                    foreach (Core.Properties.cmisProperty cmisProperty in currentProperties)
                    {
                        var extendedProperties = cmisProperty is null ? null : cmisProperty.get_ExtendedProperties(false);
                        string declaringType = extendedProperties is null || !extendedProperties.ContainsKey(cce.DeclaringType) ? null : Conversions.ToString(extendedProperties[cce.DeclaringType]);
                        if (!string.IsNullOrEmpty(declaringType) && aspects.ContainsKey(declaringType))
                        {
                            // aspectToAdd
                            List<Core.Properties.cmisProperty> aspectProperties;

                            if (setAspects.ContainsKey(declaringType))
                            {
                                aspectProperties = setAspects[declaringType];
                            }
                            else
                            {
                                aspectProperties = new List<Core.Properties.cmisProperty>();
                                setAspects.Add(declaringType, aspectProperties);
                            }
                            aspectProperties.Add(cmisProperty);
                        }
                        else
                        {
                            objectProperties.Add(cmisProperty);
                        }
                    }
                    // aspects defined
                    if (setAspects.Count > 0)
                    {
                        var currentExtensions = properties.Extensions;
                        List<ce.Extension> extensions;

                        if (currentExtensions is null)
                        {
                            extensions = new List<ce.Extension>();
                        }
                        else
                        {
                            extensions = (from extension in currentExtensions
                                          where !(extension is null || extension is cea.Aspects || extension is cea.SetAspects)
                                          select extension).ToList();
                        }
                        // aspect-properties must be removed from property collection ...
                        properties.Properties = objectProperties.ToArray();
                        // ... and send as SetAspects extension
                        extensions.Add(new cea.SetAspects((from de in setAspects
                                                           select new cea.SetAspects.Aspect(cea.SetAspects.enumSetAspectsAction.aspectsToAdd, de.Key, de.Value.ToArray())).ToArray()));
                        properties.Extensions = extensions.ToArray();
                        // after the properties have been sent to the server the client has to restore modified values
                        state.AddRollbackAction(() =>
                              {
                                  properties.Extensions = currentExtensions;
                                  properties.Properties = currentProperties;
                              });
                    }
                }
            }

            return base.BeginRequest(state, properties);
        }

        public override void EndRequest(State state, params Core.Collections.cmisPropertiesType[] propertyCollections)
        {
            base.EndRequest(state, propertyCollections);

            if (propertyCollections is not null)
            {
                foreach (Core.Collections.cmisPropertiesType propertyCollection in propertyCollections)
                {
                    if (propertyCollection is not null && propertyCollection.Extensions is not null)
                    {
                        var properties = propertyCollection.GetProperties(true);
                        bool hasAspects = false;

                        foreach (ce.Extension extension in propertyCollection.Extensions)
                        {
                            if (extension is cea.Aspects)
                            {
                                cea.Aspects aspects = (cea.Aspects)extension;
                                if (aspects.AppliedAspects is not null)
                                {
                                    foreach (cea.Aspects.Aspect aspect in aspects.AppliedAspects)
                                    {
                                        if (aspect is not null && aspect.Properties is not null && aspect.Properties.Properties is not null)
                                        {
                                            foreach (Core.Properties.cmisProperty cmisProperty in aspect.Properties.Properties)
                                            {
                                                if (cmisProperty is not null && !properties.ContainsKey(cmisProperty.PropertyDefinitionId))
                                                {
                                                    properties.Add(cmisProperty.PropertyDefinitionId, cmisProperty);
                                                    hasAspects = true;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (hasAspects)
                            propertyCollection.Properties = properties.Values.ToArray();
                    }
                }
            }
        }
    }
}