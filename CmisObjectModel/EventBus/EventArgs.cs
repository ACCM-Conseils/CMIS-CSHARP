using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualBasic.CompilerServices;
// ***********************************************************************************************************************
// * Project: CmisObjectModelLibrary
// * Copyright (c) 2017, Brügmann Software GmbH, Papenburg, All rights reserved
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
namespace CmisObjectModel.EventBus
{
    public class EventArgs : System.EventArgs
    {

        #region Constructors
        protected EventArgs(object sender, Dictionary<string, object> properties, string serviceDocUri, string repositoryId, string eventIdentifier, string eventName, string[] eventParameters)
        {
            Sender = sender;
            _properties = properties;
            ServiceDocUri = serviceDocUri;
            RepositoryId = repositoryId;
            EventIdentifier = eventIdentifier;
            EventName = eventName;
            EventParameters = eventParameters;
        }
        #endregion

        #region Helper classes
        public abstract class PredefinedPropertyNames
        {
            private PredefinedPropertyNames()
            {
            }

            public const string Failure = "failure";
            public static readonly Type FailureType = typeof(System.ServiceModel.FaultException);

            public const string NewObjectId = "newObjectId";
            public static readonly Type NewObjectIdType = typeof(string);

            public const string Succeeded = "succeeded";
            public static readonly Type SucceededType = typeof(bool);
        }
        #endregion

        #region DispatchEvent
        public static EventArgs DispatchBeginEvent(object sender, Dictionary<string, object> properties, string serviceDocUri, string repositoryId, enumBuiltInEvents builtInEvent, params string[] eventParameters)
        {
            var retVal = new EventArgs(sender, properties, serviceDocUri, repositoryId, Guid.NewGuid().ToString("N"), BuiltInEventNames.GetBeginEventName(builtInEvent), eventParameters) { _endEventName = BuiltInEventNames.GetEndEventName(builtInEvent) };
            Backbone.DispatchEvent(retVal);
            return retVal;
        }
        public void DispatchEndEvent(Dictionary<string, object> properties)
        {
            var e = new EventArgs(Sender, properties, ServiceDocUri, RepositoryId, EventIdentifier, "End" + (EventName.StartsWith("Begin", StringComparison.InvariantCultureIgnoreCase) ? EventName.Substring("Begin".Length) : EventName), EventParameters);
            Backbone.DispatchEvent(e);
        }
        /// <summary>
      /// Dispatches a non built-in event
      /// </summary>
        public static void DispatchEvent(object sender, Dictionary<string, object> properties, string serviceDocUri, string repositoryId, string eventName, params string[] eventParameters)
        {
            Backbone.DispatchEvent(new EventArgs(sender, properties, serviceDocUri, repositoryId, Guid.NewGuid().ToString("N"), eventName, eventParameters));
        }
        #endregion

        private string _endEventName;
        public readonly string EventIdentifier;
        public readonly string EventName;
        public readonly string[] EventParameters;

        /// <summary>
      /// Predefined Property Failure (hosted in _properties)
      /// </summary>
        public System.ServiceModel.FaultException Failure
        {
            get
            {
                return this.get_Property(PredefinedPropertyNames.Failure) as System.ServiceModel.FaultException;
            }
        }

        /// <summary>
      /// Predefined Property NewObjectId (hosted in _properties)
      /// </summary>
        public string NewObjectId
        {
            get
            {
                return this.get_Property(PredefinedPropertyNames.NewObjectId) as string;
            }
        }

        private Dictionary<string, object> _properties;
        public KeyValuePair<string, object>[] Properties
        {
            get
            {
                return _properties.ToArray();
            }
        }

        public object get_Property(string propertyName)
        {
            object retVal = null;
            try
            {
                if (_properties is not null)
                    _properties.TryGetValue(propertyName, out retVal);
            }
            catch
            {
            }
            return retVal;
        }

        public readonly string RepositoryId;
        public readonly object Sender;
        public readonly string ServiceDocUri;

        /// <summary>
      /// Predefined Property Succeeded (hosted in _properties)
      /// </summary>
        public bool Succeeded
        {
            get
            {
                object retVal = null;
                if (_properties is null || !_properties.TryGetValue(PredefinedPropertyNames.Succeeded, out retVal))
                    return true;
                try
                {
                    return Conversions.ToBoolean(retVal);
                }
                catch
                {
                    return true;
                }
            }
        }

    }
}