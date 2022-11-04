using System;
using System.Collections.Generic;
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

namespace CmisObjectModel.Common.Generic
{
    /// <summary>
   /// A simple collector of entries in a tree
   /// </summary>
   /// <typeparam name="TList"></typeparam>
   /// <typeparam name="TEntry"></typeparam>
   /// <remarks></remarks>
    public class TreeEntryCollector<TList, TEntry>
    {

        #region Constructors
        protected TreeEntryCollector()
        {
        }

        public TreeEntryCollector(Func<TEntry, TList> getChildren, Func<TList, IEnumerable<TEntry>> getEntries)
        {
            _getChildren = getChildren;
            _getEntries = getEntries;
        }
        #endregion

        /// <summary>
      /// Starts collecting objects from a given entry
      /// </summary>
      /// <param name="startEntry"></param>
      /// <param name="includeStartEntry"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public List<TEntry> Collect(TEntry startEntry, bool includeStartEntry = true)
        {
            var retVal = new List<TEntry>();

            if (startEntry is not null)
            {
                if (includeStartEntry)
                    retVal.Add(startEntry);
                Append(retVal, GetChildren(startEntry));
            }

            return retVal;
        }

        /// <summary>
      /// Starts collecting objects from given entries
      /// </summary>
      /// <param name="startEntries"></param>
      /// <param name="includeStartEntries"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public List<TEntry> Collect(TEntry[] startEntries, bool includeStartEntries = true)
        {
            var retVal = new List<TEntry>();

            if (startEntries is not null)
            {
                foreach (TEntry entr in startEntries)
                {
                    if (includeStartEntries)
                        retVal.Add(entr);
                    Append(retVal, GetChildren(entr));
                }
            }

            return retVal;
        }

        /// <summary>
      /// Starts collecting object from a given list
      /// </summary>
      /// <param name="startLists"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public List<TEntry> Collect(params TList[] startLists)
        {
            var retVal = new List<TEntry>();

            if (startLists is not null)
            {
                foreach (TList startList in startLists)
                    Append(retVal, startList);
            }

            return retVal;
        }

        /// <summary>
      /// Recursive walk through the tree
      /// </summary>
      /// <param name="result"></param>
      /// <param name="list"></param>
      /// <remarks></remarks>
        protected void Append(List<TEntry> result, TList list)
        {
            var lists = new Queue<TList>();

            if (list is not null)
                lists.Enqueue(list);
            while (lists.Count > 0)
            {
                var entries = GetEntries(lists.Dequeue());

                if (entries is not null)
                {
                    foreach (TEntry entry in entries)
                    {
                        if (entry is not null)
                        {
                            result.Add(entry);
                            list = GetChildren(entry);
                            if (list is not null)
                                lists.Enqueue(list);
                        }
                    }
                }
            }
        }

        protected Func<TEntry, TList> _getChildren;
        protected virtual TList GetChildren(TEntry entry)
        {
            return entry is null || _getChildren is null ? default : _getChildren(entry);
        }

        protected Func<TList, IEnumerable<TEntry>> _getEntries;
        protected virtual IEnumerable<TEntry> GetEntries(TList list)
        {
            return list is null || _getEntries is null ? null : GetEntries(list);
        }

    }
}