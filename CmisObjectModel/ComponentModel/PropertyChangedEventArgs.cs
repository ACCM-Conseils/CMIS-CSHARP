
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
using sc = System.ComponentModel;

namespace CmisObjectModel.ComponentModel
{
    /// <summary>
   /// PropertyChangedEventArgs with added informations for old and new value
   /// </summary>
   /// <remarks></remarks>
    public abstract class PropertyChangedEventArgs : sc.PropertyChangedEventArgs
    {

        protected PropertyChangedEventArgs(string propertyName) : base(propertyName)
        {
        }

        public object NewValue
        {
            get
            {
                return NewValueCore;
            }
        }
        protected abstract object NewValueCore { get; }

        public object OldValue
        {
            get
            {
                return OldValueCore;
            }
        }
        protected abstract object OldValueCore { get; }
    }

    namespace Generic
    {
        public class PropertyChangedEventArgs<TProperty> : PropertyChangedEventArgs
        {

            public PropertyChangedEventArgs(string propertyName, TProperty newValue, TProperty oldValue) : base(propertyName)
            {
                _newValue = newValue;
                _oldValue = oldValue;
            }

            private TProperty _newValue;
            public new TProperty NewValue
            {
                get
                {
                    return _newValue;
                }
            }
            protected override object NewValueCore
            {
                get
                {
                    return _newValue;
                }
            }

            private TProperty _oldValue;
            public new TProperty OldValue
            {
                get
                {
                    return _oldValue;
                }
            }
            protected override object OldValueCore
            {
                get
                {
                    return _oldValue;
                }
            }
        }
    }
}