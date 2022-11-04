using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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

namespace CmisObjectModel.Data
{
    public class MapperDictionary
    {

        private Dictionary<string, Mapper> _valueMapper = new Dictionary<string, Mapper>();

        /// <summary>
      /// Adds mapping information to the client
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <param name="mapper"></param>
      /// <remarks></remarks>
        public void AddMapper(string repositoryId, Mapper mapper)
        {
            if (mapper is not null)
            {
                if (string.IsNullOrEmpty(repositoryId))
                    repositoryId = "*";

                lock (_valueMapper)
                {
                    if (_valueMapper.ContainsKey(repositoryId))
                    {
                        _valueMapper[repositoryId] = mapper;
                    }
                    else
                    {
                        _valueMapper.Add(repositoryId, mapper);
                    }
                }
            }
        }

        /// <summary>
      /// Removes mapping information from the client
      /// </summary>
      /// <param name="repositoryId"></param>
      /// <remarks></remarks>
        public void RemoveMapper(string repositoryId)
        {
            _valueMapper.Remove(string.IsNullOrEmpty(repositoryId) ? "*" : repositoryId);
        }

        /// <summary>
      /// Maps the values of properties for the remote system (direction = outgoing) or current system (direction = incoming)
      /// and returns a delegate to rollback the mapping
      /// </summary>
      /// <returns></returns>
      /// <remarks></remarks>
        public Action MapProperties(string repositoryId, enumMapDirection direction, params Core.Collections.cmisPropertiesType[] propertyCollections)
        {
            if (propertyCollections is null)
            {
                return null;
            }
            else
            {
                var rollbackActions = (from propertyCollection in propertyCollections
                                       let rollbackAction = MapProperties(repositoryId, direction, propertyCollection)
                                       where rollbackAction is not null
                                       select rollbackAction).ToArray();
                switch (rollbackActions.Length)
                {
                    case 0:
                        {
                            return null;
                        }
                    case 1:
                        {
                            return rollbackActions[0];
                        }

                    default:
                        {
                            return new Action(() => { foreach (Action rollbackAction in rollbackActions) rollbackAction.Invoke(); });
                        }
                }
            }
        }
        /// <summary>
      /// Maps the values of properties for the remote system (direction = outgoing) or current system (direction = incoming)
      /// and returns a delegate to rollback the mapping
      /// </summary>
      /// <param name="properties"></param>
      /// <param name="direction"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public Action MapProperties(string repositoryId, enumMapDirection direction, Core.Collections.cmisPropertiesType properties)
        {
            if (_valueMapper.ContainsKey(repositoryId))
            {
                return _valueMapper[repositoryId].MapProperties(properties, direction);
            }
            else if (_valueMapper.ContainsKey("*"))
            {
                return _valueMapper["*"].MapProperties(properties, direction);
            }
            else
            {
                return null;
            }
        }

    }
}