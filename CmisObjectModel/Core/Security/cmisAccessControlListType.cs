using System.Collections.Generic;
using System.Data;
using System.Linq;
using sxs = System.Xml.Serialization;
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
using CmisObjectModel.Common;

namespace CmisObjectModel.Core.Security
{
    [sxs.XmlRoot("acl", Namespace = Constants.Namespaces.cmis)]
    public partial class cmisAccessControlListType
    {

        #region Helper classes
        /// <summary>
      /// Key of Map-class
      /// </summary>
      /// <remarks></remarks>
        private class Key
        {
            public Key(string principalId, bool direct, string permission)
            {
                PrincipalId = principalId;
                Direct = direct;
                Permission = permission;
            }

            public readonly bool Direct;
            public readonly string Permission;
            public readonly string PrincipalId;
        }

        /// <summary>
      /// Maps the aces of a cmisAccessControlListType-instance to enable quick transformations
      /// </summary>
      /// <remarks></remarks>
        private class Map : Dictionary<string, Dictionary<bool, Dictionary<string, int>>>
        {

            /// <summary>
         /// Adds a permission
         /// </summary>
         /// <remarks></remarks>
            public new void Add(Key key)
            {
                Dictionary<bool, Dictionary<string, int>> innerMap;
                Dictionary<string, int> permissions;

                if (key.PrincipalId is not null)
                {
                    if (ContainsKey(key.PrincipalId))
                    {
                        innerMap = this[key.PrincipalId];
                    }
                    else
                    {
                        innerMap = new Dictionary<bool, Dictionary<string, int>>();
                        Add(key.PrincipalId, innerMap);
                    }

                    if (innerMap.ContainsKey(key.Direct))
                    {
                        permissions = innerMap[key.Direct];
                    }
                    else
                    {
                        permissions = new Dictionary<string, int>();
                        innerMap.Add(key.Direct, permissions);
                    }

                    if (!(string.IsNullOrEmpty(key.Permission) || permissions.ContainsKey(key.Permission)))
                        permissions.Add(key.Permission, permissions.Count);
                }
            }

            /// <summary>
         /// Returns True, if a permission is defined for a valid principalId
         /// </summary>
         /// <returns></returns>
         /// <remarks>key.Direct-value is ignored</remarks>
            public new bool Contains(Key key)
            {
                if (key.PrincipalId is not null && !string.IsNullOrEmpty(key.Permission) && ContainsKey(key.PrincipalId))
                {
                    {
                        var withBlock = this[key.PrincipalId];
                        // ignore the key.Direct-value
                        foreach (bool direct in new bool[] { false, true })
                        {
                            if (withBlock.ContainsKey(direct) && withBlock[direct].ContainsKey(key.Permission))
                                return true;
                        }
                    }
                }

                return false;
            }

            /// <summary>
         /// Returns all defined principalId, direct, permission tuples
         /// </summary>
         /// <returns></returns>
         /// <remarks></remarks>
            public new List<Key> Keys()
            {
                var retVal = new List<Key>();

                foreach (KeyValuePair<string, Dictionary<bool, Dictionary<string, int>>> deMajor in this)
                {
                    foreach (KeyValuePair<bool, Dictionary<string, int>> deMinor in deMajor.Value)
                    {
                        var permissions = (from de in
                                               from de in deMinor.Value
                                               where de.Value >= 0
                                               select de
                                           orderby de.Value
                                           select de).ToArray();
                        if (permissions is null || permissions.Length == 0)
                        {
                            retVal.Add(new Key(deMajor.Key, deMinor.Key, null));
                        }
                        else
                        {
                            foreach (KeyValuePair<string, int> permission in permissions)
                                retVal.Add(new Key(deMajor.Key, deMinor.Key, permission.Key));
                        }
                    }
                }

                return retVal;
            }

            /// <summary>
         /// physical remove
         /// </summary>
         /// <remarks>key.Direct-value is ignored</remarks>
            public new bool Remove(Key key)
            {
                bool retVal = false;

                if (key.PrincipalId is not null && !string.IsNullOrEmpty(key.Permission) && ContainsKey(key.PrincipalId))
                {
                    {
                        var withBlock = this[key.PrincipalId];
                        // ignore the key.Direct-value
                        foreach (bool direct in new bool[] { false, true })
                        {
                            if (withBlock.ContainsKey(direct) && withBlock[direct].Remove(key.Permission))
                                retVal = true;
                        }
                    }
                }

                return retVal;
            }

            /// <summary>
         /// Converts instance to a cmisAccessControlListType
         /// </summary>
         /// <returns></returns>
         /// <remarks></remarks>
            public cmisAccessControlListType ToACEs()
            {
                var aces = new List<cmisAccessControlEntryType>();

                foreach (KeyValuePair<string, Dictionary<bool, Dictionary<string, int>>> deMajor in this)
                {
                    foreach (KeyValuePair<bool, Dictionary<string, int>> deMinor in deMajor.Value)
                    {
                        var permissions = (from de in
                                               from de in deMinor.Value
                                               where de.Value >= 0
                                               select de
                                           orderby de.Value
                                           select de).ToArray();
                        aces.Add(new cmisAccessControlEntryType()
                        {
                            Direct = deMinor.Key,
                            Permissions = permissions is null || permissions.Length == 0 ? null : (from de in permissions
                                                                                                   select de.Key).ToArray(),
                            Principal = new cmisAccessControlPrincipalType() { PrincipalId = deMajor.Key }
                        });
                    }
                }

                return new cmisAccessControlListType() { _permissions = aces.Count == 0 ? null : aces.ToArray() };
            }

            /// <summary>
         /// Converts value to a Map-instance
         /// </summary>
         /// <param name="value"></param>
         /// <returns></returns>
         /// <remarks></remarks>
            public static implicit operator Map(cmisAccessControlListType value)
            {
                if (value is null)
                {
                    return null;
                }
                else
                {
                    var retVal = new Map();

                    if (value._permissions is not null)
                    {
                        foreach (cmisAccessControlEntryType ace in value._permissions)
                        {
                            if (ace is not null && ace.Principal is not null && ace.Principal.PrincipalId is not null)
                            {
                                if (ace.Permissions is null || ace.Permissions.Length == 0)
                                {
                                    retVal.Add(new Key(ace.Principal.PrincipalId, ace.Direct, null));
                                }
                                else
                                {
                                    foreach (string permission in ace.Permissions)
                                        retVal.Add(new Key(ace.Principal.PrincipalId, ace.Direct, permission));
                                }
                            }
                        }
                    }

                    return retVal;
                }
            }
        }
        /// <summary>
      /// Class contains the addACEs- and removeACEs-operations to create the targetACEs starting from
      /// a given cmisAccessControlListType-instance
      /// </summary>
      /// <remarks>see cmisAccessControlListType.Split()</remarks>
        public class SplitResult
        {
            public SplitResult(cmisAccessControlListType addACEs, cmisAccessControlListType removeACEs)
            {
                AddACEs = addACEs;
                RemoveACEs = removeACEs;
            }

            public readonly cmisAccessControlListType AddACEs;
            public readonly cmisAccessControlListType RemoveACEs;
        }
        #endregion

        /// <summary>
      /// Same as property Permissions; using the BrowserBinding the Permissions-parameter is called aces
      /// </summary>
        public cmisAccessControlEntryType[] ACEs
        {
            get
            {
                return _permissions;
            }
            set
            {
                Permissions = value;
            }
        }

        /// <summary>
      /// Aspect before ReadXmlCore()
      /// </summary>
        protected override void BeginReadXmlCore(System.Xml.XmlReader reader, Serialization.XmlAttributeOverrides attributeOverrides)
        {
            // unused
        }

        /// <summary>
      /// Aspect before WriteXmlCore()
      /// </summary>
        protected override void BeginWriteXmlCore(System.Xml.XmlWriter writer, Serialization.XmlAttributeOverrides attributeOverrides)
        {
            // unused
        }

        /// <summary>
      /// Aspect after ReadXmlCore()
      /// </summary>
        protected override void EndReadXmlCore(System.Xml.XmlReader reader, Serialization.XmlAttributeOverrides attributeOverrides)
        {
            _exact = Read(reader, attributeOverrides, "exact", Constants.Namespaces.cmis, _exact);
        }

        /// <summary>
      /// Aspect after WriteXmlCore()
      /// </summary>
        protected override void EndWriteXmlCore(System.Xml.XmlWriter writer, Serialization.XmlAttributeOverrides attributeOverrides)
        {
            if (_exact.HasValue)
                WriteElement(writer, attributeOverrides, "exact", Constants.Namespaces.cmis, CommonFunctions.Convert(_exact));
        }

        protected bool? _exact;
        /// <summary>
      /// Property not defined in CMIS-definition (CMIS-Core.xsd), but in chapter 2.2.10.2.2 Outputs is a property exact defined.
      /// </summary>
      /// <value></value>
      /// <returns></returns>
      /// <remarks>
      /// The sample given in http://docs.oasis-open.org/cmis/CMIS/v1.1/os/examples/atompub/getAcl-response.log shows, that the
      /// Core.cmisAccessControlListType is used for transmission. A second class definition contains the exact-property
      /// (Messaging.cmisACLType). Unfortunately the ACL-property is of type cmisAccessControlListType and not defined as an
      /// array of cmisAccessControlEntry.
      /// </remarks>
        public virtual bool? Exact
        {
            get
            {
                return _exact;
            }
            set
            {
                if (!_exact.Equals(value))
                {
                    var oldValue = _exact;
                    _exact = value;
                    OnPropertyChanged("Exact", value, oldValue);
                    OnPropertyChanged("IsExact", value, oldValue);
                }
            }
        } // Exact

        /// <summary>
      /// Same as property Exact; using the BrowserBinding the Exact-parameter is called IsExact
      /// </summary>
        public bool? IsExact
        {
            get
            {
                return _exact;
            }
            set
            {
                Exact = value;
            }
        } // IsExact

        /// <summary>
      /// Adds the aces specified in addACEs and removes the aces specified in removeACEs from the current instance
      /// </summary>
      /// <param name="addACEs"></param>
      /// <param name="removeACEs"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public cmisAccessControlListType Join(cmisAccessControlListType addACEs, cmisAccessControlListType removeACEs)
        {
            {
                var withBlock = (Map)this;
                if (addACEs is not null)
                {
                    foreach (Key key in ((Map)addACEs).Keys())
                        withBlock.Add(new Key(key.PrincipalId, true, key.Permission));
                }
                if (removeACEs is not null)
                {
                    foreach (Key key in ((Map)removeACEs).Keys())
                        withBlock.Remove(key);
                }

                return withBlock.ToACEs();
            }
        }

        /// <summary>
      /// Calculates the addACEs- and removeACEs-operation to transform the current instance into targetACEs
      /// </summary>
      /// <param name="targetACEs"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public SplitResult Split(cmisAccessControlListType targetACEs)
        {
            if (targetACEs is null || targetACEs._permissions is null || targetACEs._permissions.Length == 0)
            {
                // remove all aces
                return new SplitResult(null, this);
            }
            else
            {
                Map addACEs = targetACEs;
                var removeACEs = new Map();

                {
                    var withBlock = (Map)this;
                    foreach (Key key in withBlock.Keys())
                    {
                        // this ace is not defined within the targetACEs instance
                        if (!addACEs.Remove(key))
                        {
                            removeACEs.Add(new Key(key.PrincipalId, true, key.Permission));
                        }
                    }
                }

                return new SplitResult(addACEs.ToACEs(), removeACEs.ToACEs());
            }
        }

    }
}