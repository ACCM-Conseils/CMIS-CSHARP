using System.Collections.Generic;
using System.Data;
using System.Linq;
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
    /// <summary>
   /// Class to handle Events only known by names
   /// </summary>
   /// <remarks>
   /// The purpose of the Backbone/WeakListener is to give easy way to consume an event without knowing instances of
   /// objects that may raise them. The design prevents memoryleaks because every callback method the is connected to
   /// the backbone is stored in a weakreference. It lies in the responsibility of the calling class to keep the
   /// garbage collector from the callback method as long as its used. You find a small example how to manage lifetime
   /// of the callback method in the class EventConsumerSample.
   /// </remarks>
    public sealed class Backbone
    {

        public const string csWildcard = "*";

        private Backbone()
        {
        }
        private Backbone(Backbone parent, string childKey)
        {
            _parent = parent;
            _childKey = childKey;
            parent._children[childKey] = this;
        }

        #region Helper classes
        /// <summary>
      /// Sample to consume a named event
      /// </summary>
        private class EventConsumerSample
        {

            public EventConsumerSample()
            {

                _myEventHandler = new WeakListenerCallback(MyEventHandler);
                // Only if You want to remove the listener manually (a good idea for classes with the IDispose-interface),
                // You have to store the instance
                WeakListener.CreateInstance(_myEventHandler, "MyServiceUri", "MyRespositoryId", "MyEventName");
            }

            private WeakListenerCallback _myEventHandler;
            private enumEventBusListenerResult MyEventHandler(EventArgs e)
            {
                // Your code here

                // if You only want to execute the handler once, return removeListener to remove the listener automatically
                return enumEventBusListenerResult.dontCare;
            }
        }
        #endregion

        public void AddListener(WeakListener listener)
        {
            lock (_syncObject)
            {
                var current = this;
                Backbone child = null;

                foreach (string key in GetKeys(listener.ServiceDocUri, listener.RepositoryId, listener.EventName, listener.EventParameters))
                {
                    if (current._children.TryGetValue(key, out child))
                    {
                        current = child;
                    }
                    else
                    {
                        current = new Backbone(current, key);
                    }
                }
                current._listeners.Add(listener);
            }
        }

        private readonly string _childKey;
        private readonly Dictionary<string, Backbone> _children = new Dictionary<string, Backbone>();

        public static void DispatchEvent(EventArgs e)
        {
            Root.DispatchEventCore(e);
        }
        protected void DispatchEventCore(EventArgs e)
        {
            WeakListener[] listeners;
            var pendingRemovals = new List<WeakListener>();

            lock (_syncObject)
                listeners = (from listener in
                                 from listener in GetListeners(GetKeys(e.ServiceDocUri, e.RepositoryId, e.EventName, e.EventParameters))
                                 select listener
                             orderby listener.OrderIndex
                             select listener).ToArray();
            for (int index = 0, loopTo = listeners.Length - 1; index <= loopTo; index++)
            {
                var listener = listeners[index];
                try
                {
                    if (listener.Invoke(e) == enumEventBusListenerResult.removeListener)
                        pendingRemovals.Add(listener);
                }
                catch
                {
                }
            }
            lock (_syncObject)
            {
                for (int index = 0, loopTo1 = pendingRemovals.Count - 1; index <= loopTo1; index++)
                    RemoveListener(pendingRemovals[index]);
            }
        }

        /// <summary>
      /// Returns key or '*'-placeholder if key is not set
      /// </summary>
        private static string GetKey(string key)
        {
            return string.IsNullOrEmpty(key) ? csWildcard : key;
        }

        private static IEnumerable<string> GetKeys(string serviceDocUri, string repositoryId, string eventName, params string[] eventParameters)
        {
            yield return GetKey(serviceDocUri);
            yield return GetKey(repositoryId);
            yield return GetKey(eventName);

            int length = eventParameters is null ? 0 : eventParameters.Length;
            for (int index = 0, loopTo = length - 1; index <= loopTo; index++)
                yield return GetKey(eventParameters[index]);
        }

        /// <summary>
      /// Returns an IEnumerable-instance which contains all WeakListener-instances suitable for given keys
      /// </summary>
        private IEnumerable<WeakListener> GetListeners(IEnumerable<string> keys)
        {
            var instances = new Queue<Backbone>();
            Backbone child = null;

            // every listener stored in root belongs to the result
            foreach (WeakListener listener in _listeners)
                yield return listener;

            instances.Enqueue(this);
            // The algorithm uses the fact, that each listener in each child with a suitable key belongs to the result.
            // For example: if keys-sequence contains 'FirstKey', 'SecondKey', 'ThirdKey' then all listeners belongs
            // to the result if their key-sequence is complete contained in the keys-sequence beginning from the first
            // key.
            // No.  key-sequence sample                                    belongs to keys  remarks
            // 1.  'FirstKey','SecondKey'                                 yes              keys-sequence starts with key-sequence
            // 2.  '*', 'SecondKey'                                       yes              wildcard matches every key
            // 3.  'FirstKey','AnotherSecondKey','ThirdKey'               no               'AnotherSecondKey' does not match
            // 4.  'FirstKey','SecondKey','ThirdKey','FourthKey'          no               keys-sequence to unspecific
            // 5.  'FirstKey','SecondKey','ThirdKey','*'                  no               keys-sequence to unspecific; wildcard
            // can only match every EXISTING key
            // If the keys-sequence contains 'FirstKey','*','ThirdKey' then 3. matches as well.
            foreach (string key in keys)
            {
                int count = instances.Count;

                if (count == 0)
                {
                    // no listener registered
                    yield break;
                }
                else
                {
                    for (int index = 1, loopTo = count; index <= loopTo; index++)
                    {
                        {
                            var withBlock = instances.Dequeue();
                            if (key.Equals(csWildcard))
                            {
                                // attend every child
                                foreach (KeyValuePair<string, Backbone> de in withBlock._children)
                                {
                                    // every listener in a child belongs to the result
                                    foreach (WeakListener listener in de.Value._listeners)
                                        yield return listener;
                                    instances.Enqueue(de.Value);
                                }
                            }
                            else
                            {
                                // accept only the childs with suitable key or wildcard
                                for (int suitableKeyIndex = 0; suitableKeyIndex <= 1; suitableKeyIndex++)
                                {
                                    if (withBlock._children.TryGetValue(suitableKeyIndex == 0 ? key : csWildcard, out child))
                                    {
                                        // every listener in this child belongs to the result
                                        foreach (WeakListener listener in child._listeners)
                                            yield return listener;
                                        instances.Enqueue(child);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private readonly HashSet<WeakListener> _listeners = new HashSet<WeakListener>();
        private readonly Backbone _parent;

        public void RemoveListener(WeakListener listener)
        {
            lock (_syncObject)
            {
                var current = this;

                foreach (string key in GetKeys(listener.ServiceDocUri, listener.RepositoryId, listener.EventName, listener.EventParameters))
                {
                    if (!current._children.TryGetValue(key, out current))
                    {
                        // unknown listener
                        return;
                    }
                }
                current._listeners.Remove(listener);
                // remove empty Backbone-instances
                while (!ReferenceEquals(current, Root) && current._listeners.Count == 0 && current._children.Count == 0)
                {
                    var parent = current._parent;

                    parent._children.Remove(current._childKey);
                    current = parent;
                }
            }
        }

        public static readonly Backbone Root = new Backbone();
        private object _syncObject = new object();

    }
}