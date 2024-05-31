using System;
using System.Collections.Generic;

namespace CmisServer
{
    public class Metadata
    {

        /// <summary>
   /// Value of Property "cmis:description" for the latest Version
   /// </summary>
   /// <remarks></remarks>
        public string Description;

        /// <summary>
   /// Value of the Property "cmis:description" for the Private Working Copy (PWC)
   /// </summary>
   /// <remarks></remarks>
        public string DescriptionPwc;

        /// <summary>
   /// Value of Property "docuware:metavalues" for the latest Version
   /// </summary>
   /// <remarks></remarks>
        public string[] Akte;

        /// <summary>
   /// Value of the Property "docuware:metavalues" for the Private Working Copy (PWC)
   /// </summary>
   /// <remarks></remarks>
        public string[] AktePwc;

        public int MajorOfLatestVersion;
        public int MinorOfLatestVersion;

        public string MimeType;

        public string CreatedBy;
        public DateTime CreationDate;
        public string LastModifiedBy;
        public DateTime LastModificationDate;

        public string ForeignChangeToken;

        public string VersionSeriesCheckedOutBy;

        public bool IsVersionSeriesCheckedOut
        {
            get
            {
                return !string.IsNullOrEmpty(VersionSeriesCheckedOutBy);
            }
        }

        public string LabelOfLatestVersion
        {
            get
            {
                return MajorOfLatestVersion + "." + MinorOfLatestVersion;
            }
        }

        #region Comments

        public CheckinComment[] CheckinComments;

        public class CheckinComment
        {
            public string VersionLabel;
            public string Comment;
        }

        public void AddComment(string text)
        {
            var list = new List<CheckinComment>();
            if (CheckinComments is not null)
            {
                list.AddRange(CheckinComments);
            }
            list.Add(new CheckinComment() { VersionLabel = LabelOfLatestVersion, Comment = text });
            CheckinComments = list.ToArray();
        }

        public string GetComment(string versionLabel = null)
        {
            if (string.IsNullOrEmpty(versionLabel))
                versionLabel = LabelOfLatestVersion;
            if (CheckinComments is not null)
            {
                foreach (CheckinComment comment in CheckinComments)
                {
                    if (comment.VersionLabel.Equals(versionLabel))
                    {
                        return comment.Comment;
                    }
                }
            }
            return null;
        }

        #endregion

        #region Xml

        public static Metadata FromXml(string xml)
        {
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(Metadata));
            Metadata meta = (Metadata)serializer.Deserialize(new System.IO.StringReader(xml));
            return meta;
        }

        public object ToXml()
        {
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(Metadata));
            var writer = new System.IO.StringWriter();
            serializer.Serialize(writer, this);
            string xml = writer.ToString();
            return xml;
        }

        #endregion

    }
}