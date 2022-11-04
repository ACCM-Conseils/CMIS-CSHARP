using System;
using System.Collections.Generic;
using System.Data;
// depends on the chosen interpretation of the xs:integer expression in a xsd-file
/* TODO ERROR: Skipped IfDirectiveTrivia
#If xs_Integer = "Int32" OrElse xs_integer = "Integer" OrElse xs_integer = "Single" Then
*//* TODO ERROR: Skipped DisabledTextTrivia
Imports xs_Integer = System.Int32
*//* TODO ERROR: Skipped ElseDirectiveTrivia
#Else
*/
using xs_Integer = System.Int64;
using System.Linq;
using sss = System.ServiceModel.Syndication;
using sx = System.Xml;
using sxs = System.Xml.Serialization;
/* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.AtomPub
{
    /// <summary>
   /// Represents a list of child elements
   /// </summary>
   /// <remarks></remarks>
    public class AtomFeed : sss.SyndicationFeed
    {

        #region Constants
        private const string csHasMoreItems = "hasMoreItems";
        private const string csNumItems = "numItems";
        #endregion

        #region Constructors
        public AtomFeed() : base()
        {

            // define prefixes for used namespaces
            foreach (KeyValuePair<sx.XmlQualifiedName, string> de in CmisObjectModel.Common.CommonFunctions.CmisNamespaces)
                AttributeExtensions.Add(de.Key, de.Value);
        }

        public AtomFeed(AtomEntry parent, List<AtomEntry> entries, bool hasMoreItems, long? numItems, List<AtomLink> links) : this(parent.Id, parent.Title is null ? null : parent.Title.Text, parent.LastUpdatedTime, entries, hasMoreItems, numItems, links, parent.Authors.ToArray())
        {
        }

        public AtomFeed(string id, string title, DateTimeOffset lastUpdatedTime, List<AtomEntry> entries, bool hasMoreItems, long? numItems, List<AtomLink> links, params sss.SyndicationPerson[] authors) : this()
        {

            // conform to guideline (see 3.5.1 Feeds)
            // atom:updated SHOULD be the latest time the folder or its contents was updated. If unknown by the underlying repository, it MUST be the current time
            if (lastUpdatedTime == DateTimeOffset.MinValue)
                lastUpdatedTime = DateTimeOffset.UtcNow;
            Id = id;
            Title = new sss.TextSyndicationContent(title);
            LastUpdatedTime = lastUpdatedTime;
            ElementExtensions.Add("edited", CmisObjectModel.Constants.Namespaces.app, sx.XmlConvert.ToString(lastUpdatedTime));
            if (authors is not null)
            {
                foreach (sss.SyndicationPerson author in authors)
                    Authors.Add(author);
            }
            if (links is not null)
            {
                foreach (AtomLink link in links)
                    Links.Add(link);
            }
            if (entries is not null)
            {
                // omit duplicate namespace definitions
                foreach (AtomEntry entry in entries)
                {
                    foreach (KeyValuePair<sx.XmlQualifiedName, string> de in CmisObjectModel.Common.CommonFunctions.CmisNamespaces)
                        entry.AttributeExtensions.Remove(de.Key);
                }
                Items = entries;
            }
            HasMoreItems = hasMoreItems;
            NumItems = numItems;
        }

        /// <summary>
      /// Creates a new instance (similar to ReadXml() in IXmlSerializable-classes)
      /// </summary>
      /// <param name="reader"></param>
      /// <returns></returns>
      /// <remarks></remarks>
        public static AtomFeed CreateInstance(sx.XmlReader reader)
        {
            var authors = new List<sss.SyndicationPerson>();
            bool hasMoreItems = false;
            var entries = new List<AtomEntry>();
            string id = null;
            var lastUpdatedTime = default(DateTimeOffset);
            var links = new List<AtomLink>();
            long? numItems = default;
            string title = null;
            bool isEmptyElement;

            reader.MoveToContent();
            isEmptyElement = reader.IsEmptyElement;
            reader.ReadStartElement();
            if (!isEmptyElement)
            {
                reader.MoveToContent();
                while (reader.IsStartElement())
                {
                    switch (reader.NamespaceURI ?? "")
                    {
                        case CmisObjectModel.Constants.Namespaces.atom:
                            {
                                switch (reader.LocalName ?? "")
                                {
                                    case "author":
                                    case "contributor":
                                        {
                                            authors.Add(Factory.CreateAuthor(reader));
                                            break;
                                        }
                                    case "entry":
                                        {
                                            entries.Add(AtomEntry.CreateInstance(reader));
                                            break;
                                        }
                                    case "id":
                                        {
                                            id = reader.ReadElementString();
                                            break;
                                        }
                                    case "link":
                                        {
                                            links.Add(Factory.CreateLink(reader));
                                            break;
                                        }
                                    case "title":
                                        {
                                            title = reader.ReadElementString();
                                            break;
                                        }
                                    case "updated":
                                        {
                                            lastUpdatedTime = Factory.CreateDateTimeOffset(reader);
                                            break;
                                        }

                                    default:
                                        {
                                            // ignore node
                                            reader.ReadOuterXml();
                                            break;
                                        }
                                }

                                break;
                            }
                        case CmisObjectModel.Constants.Namespaces.cmisra:
                            {
                                switch (reader.LocalName ?? "")
                                {
                                    case csHasMoreItems:
                                        {
                                            bool boolValue;
                                            hasMoreItems = bool.TryParse(reader.ReadElementString(), out boolValue) && boolValue;
                                            break;
                                        }
                                    case csNumItems:
                                        {
                                            long intValue;
                                            if (long.TryParse(reader.ReadElementString(), out intValue))
                                            {
                                                numItems = intValue;
                                            }

                                            break;
                                        }

                                    default:
                                        {
                                            // ignore node
                                            reader.ReadOuterXml();
                                            break;
                                        }
                                }

                                break;
                            }

                        default:
                            {
                                // ignore node
                                reader.ReadOuterXml();
                                break;
                            }
                    }
                    reader.MoveToContent();
                }

                reader.ReadEndElement();
            }

            return new AtomFeed(id, title, lastUpdatedTime, entries.Count > 0 ? entries : null, hasMoreItems, numItems, links.Count > 0 ? links : null, authors.Count > 0 ? authors.ToArray() : null);
        }
        #endregion

        #region AtomPub-extensions
        private sss.SyndicationElementExtension _hasMoreItems;
        /// <summary>
      /// AtomPub extension HasMoreItems
      /// </summary>
      /// <value></value>
      /// <returns></returns>
      /// <remarks></remarks>
        [sxs.XmlIgnore()]
        public bool HasMoreItems
        {
            get
            {
                return _hasMoreItems is not null && _hasMoreItems.GetObject<bool>();
            }
            private set
            {
                if (_hasMoreItems is not null)
                    ElementExtensions.Remove(_hasMoreItems);
                _hasMoreItems = new sss.SyndicationElementExtension(csHasMoreItems, CmisObjectModel.Constants.Namespaces.cmisra, (object)value);
                ElementExtensions.Add(_hasMoreItems);
            }
        } // HasMoreItems

        private sss.SyndicationElementExtension _numItems;
        /// <summary>
      /// AtomPub extension NumItems
      /// </summary>
      /// <value></value>
      /// <returns></returns>
      /// <remarks></remarks>
        [sxs.XmlIgnore()]
        public long? NumItems
        {
            get
            {
                if (_numItems is null)
                {
                    return default;
                }
                else
                {
                    return _numItems.GetObject<xs_Integer>();
                }
            }
            private set
            {
                if (_numItems is not null)
                    ElementExtensions.Remove(_numItems);
                if (value.HasValue)
                {
                    _numItems = new sss.SyndicationElementExtension(csNumItems, CmisObjectModel.Constants.Namespaces.cmisra, (object)value.Value);
                }
                else
                {
                    _numItems = null;
                }
            }
        } // NumItems
        #endregion

        [sxs.XmlIgnore()]
        public List<AtomEntry> Entries
        {
            get
            {
                if (Items is null)
                {
                    return null;
                }
                else if (Items is List<AtomEntry>)
                {
                    return (List<AtomEntry>)Items;
                }
                else
                {
                    return (from item in Items
                            where item is AtomEntry
                            select ((AtomEntry)item)).ToList();
                }
            }
        }

        /// <summary>
      /// Returns the first AtomEntry if objectId is not specified (null or empty), otherwise
      /// the first AtomEntry-instance matching the specified objectId
      /// </summary>
      /// <param name="objectId"></param>
      /// <value></value>
      /// <returns></returns>
      /// <remarks></remarks>
        public AtomEntry get_Entry(string objectId)
        {
            if (Items is not null)
            {
                foreach (sss.SyndicationItem item in Items)
                {
                    if (item is AtomEntry)
                    {
                        AtomEntry retVal = (AtomEntry)item;

                        if (string.IsNullOrEmpty(objectId) || string.Compare(objectId, retVal.ObjectId ?? "") == 0)
                        {
                            return retVal;
                        }
                    }
                }
            }

            // not found
            return null;
        }

        /// <summary>
      /// Returns the number of items of this feed
      /// </summary>
      /// <returns></returns>
      /// <remarks>if numitems is set to -1 then the number of items in the item-collection is returned</remarks>
        public long? GetNumItems()
        {
            var retVal = NumItems;

            if (retVal.HasValue && retVal.Value == -1)
            {
                /* TODO ERROR: Skipped IfDirectiveTrivia
                #If xs_Integer = "Int32" OrElse xs_Integer = "Integer" OrElse xs_Integer = "Single" Then
                *//* TODO ERROR: Skipped DisabledTextTrivia
                            Return Items.Count()
                *//* TODO ERROR: Skipped ElseDirectiveTrivia
                #Else
                */
                return Items.LongCount();
            }
            /* TODO ERROR: Skipped EndIfDirectiveTrivia
            #End If
            */
            else
            {
                return retVal;
            }
        }

    }
}