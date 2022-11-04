using System;
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
   /// This class represents the elements which can be hosted by the backbone. The given WeakListenerCallback-instance
   /// in the constructor is only stored in a weakreference. Therefor the caller is responsible for the lifetime of the
   /// callback instance.
   /// </summary>
    public sealed class WeakListener
    {

        public WeakListener(WeakListenerCallback callback, string serviceDocUri, string repositoryId, string eventName, params string[] eventParameters)
        {
            _callback = new WeakReference(callback);
            ServiceDocUri = serviceDocUri;
            RepositoryId = repositoryId;
            EventName = eventName;
            EventParameters = eventParameters;
            Backbone.Root.AddListener(this);
        }
        public static WeakListener CreateInstance(WeakListenerCallback callback, string serviceDocUri, string repositoryId, string eventName, params string[] eventParameters)
        {
            return new WeakListener(callback, serviceDocUri, repositoryId, eventName, eventParameters);
        }

        private WeakReference _callback;
        public readonly string EventName;
        public readonly string[] EventParameters;

        public enumEventBusListenerResult Invoke(EventArgs e)
        {
            WeakListenerCallback callback = (WeakListenerCallback)_callback.Target;

            return callback is null ? enumEventBusListenerResult.removeListener : callback.Invoke(e);
        }

        private static long _nextOrderIndex = 0L;
        public readonly long OrderIndex = System.Threading.Interlocked.Increment(ref _nextOrderIndex);

        public void RemoveListener()
        {
            Backbone.Root.RemoveListener(this);
        }

        public readonly string RepositoryId;
        public readonly string ServiceDocUri;

    }

    public delegate enumEventBusListenerResult WeakListenerCallback(EventArgs e);
}