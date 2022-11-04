using Microsoft.VisualBasic;
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
    [HideModuleName()]
    public static class BuiltInEventNames
    {
        public const string BeginCancelCheckout = "BeginCancelCheckout";
        public const string BeginCheckIn = "BeginCheckIn";
        public const string BeginDeleteObject = "BeginDeleteObject";
        public const string EndCancelCheckout = "EndCancelCheckout";
        public const string EndCheckIn = "EndCheckedIn";
        public const string EndDeleteObject = "EndDeleteObject";

        public static string GetBeginEventName(enumBuiltInEvents builtInEvent)
        {
            return GetBeginOrEndEventName(builtInEvent, true);
        }

        public static string GetEndEventName(enumBuiltInEvents builtInEvent)
        {
            return GetBeginOrEndEventName(builtInEvent, false);
        }

        private static string GetBeginOrEndEventName(enumBuiltInEvents builtInEvent, bool flgBegin)
        {
            var flgBeginOrEnd = flgBegin ? enumBuiltInEventMasks.flgEnd : enumBuiltInEventMasks.flgBegin;
            return GetEventName((enumBuiltInEvents)(((int)builtInEvent | (int)enumBuiltInEventMasks.maskBeginOrEnd) ^ (int)flgBeginOrEnd));
        }

        public static string GetEventName(enumBuiltInEvents builtInEvent)
        {
            bool flgBegin = ((int)builtInEvent & (int)enumBuiltInEventMasks.flgBegin) == (int)enumBuiltInEventMasks.flgBegin;
            bool flgEnd = ((int)builtInEvent & (int)enumBuiltInEventMasks.flgEnd) == (int)enumBuiltInEventMasks.flgEnd;
            enumBuiltInEvents eventName = (enumBuiltInEvents)((int)builtInEvent & (int)enumBuiltInEventMasks.maskEventNames);

            return (flgBegin ? "Begin" : null) + (flgEnd ? "End" : null) + eventName.ToString();
        }
    }
}