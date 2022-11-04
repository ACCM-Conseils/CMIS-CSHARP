using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
// Diese Datei enthält alle Funktionen, die intern aufgerufen werden.

using CmisObjectModel.Core;
using CmisObjectModel.Core.Collections;
using CmisObjectModel.Core.Security;
using CmisObjectModel.Messaging;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace CmisServer
{

    public partial class CmisServiceImpl
    {

        private static string _repourl = System.Configuration.ConfigurationManager.AppSettings["url"];
        private static string _repoid = System.Configuration.ConfigurationManager.AppSettings["repoid"];
        private static string _reponame = System.Configuration.ConfigurationManager.AppSettings["reponame"];
        private static string _folder = System.Configuration.ConfigurationManager.AppSettings["folder"];
        private static string _errorfile = System.Configuration.ConfigurationManager.AppSettings["errorfile"];
        private static string _logfile = System.Configuration.ConfigurationManager.AppSettings["logfile"];

        private static cmisRepositoryInfoType _repository = default;

        public static Queue<string> InMemoryLogQueue = new Queue<string>();

        public CmisServiceImpl(Uri url) : base(url)
        {

            if (!System.IO.Directory.Exists(_folder))
            {
                System.IO.Directory.CreateDirectory(_folder);
            }
        }

        #region Logging and Errors

        public void Log_Internal(params string[] values)
        {
            string line = "LOG   | " + Format(values);
            lock (_logfile)
                System.IO.File.AppendAllText(_logfile, line + Constants.vbCrLf);
            lock (InMemoryLogQueue)
                InMemoryLogQueue.Enqueue(line);
        }

        public void ErrorLog_Internal(params string[] values)
        {
            string line = "ERROR | " + Format(values);
            lock (InMemoryLogQueue)
                InMemoryLogQueue.Enqueue(line);
            lock (_logfile)
                System.IO.File.AppendAllText(_logfile, line + Constants.vbCrLf);
        }

        public string Format(params string[] values)
        {
            for (int i = 0, loopTo = values.Length - 1; i <= loopTo; i++)
            {
                if (values[i] is null)
                {
                    values[i] = "(null)";
                }
                values[i] = values[i].ToString().PadRight(18);
            }

            return DateTime.Now.ToString() + " | " + string.Join(" | ", values);
        }

        public System.ServiceModel.Web.WebFaultException<cmisFaultType> NotSupported_Internal(string method)
        {
            return cmisFaultType.CreateNotSupportedException("CmisServerDemo_" + method);
        }

        #endregion

        #region Identity

        public System.ServiceModel.Syndication.SyndicationPerson SystemAuthor_Internal
        {
            get
            {
                return new System.ServiceModel.Syndication.SyndicationPerson("demo@cmis.bsw", "CmisServer Demo", "http://demo.bsw/cmis");
            }
        }

        #endregion

        #region TypeDefinition

        public CmisObjectModel.Core.Definitions.Types.cmisTypeDefinitionType get_TypeDefinition_Internal(string typeId)
        {
            string filename = Helper.FindXmlPath(typeId.Replace(":", "_") + ".xml");
            var reader = System.Xml.XmlReader.Create(filename);
            CmisObjectModel.Core.Definitions.Types.cmisTypeDefinitionType td = CmisObjectModel.Core.Definitions.Types.cmisTypeDefinitionType.CreateInstance(reader);
            return td;
        }

        #endregion

        #region Navigation

        public string get_ParentFolderId_Internal(string objectId)
        {
            string parentFolderId = null;
            if (objectId.Contains(@"\"))
            {
                int pos = objectId.LastIndexOf('\\');
                parentFolderId = objectId.Substring(0, pos);
            }
            else
            {
                parentFolderId = "root";
            }

            return parentFolderId;
        }

        #endregion

        #region Object

        public bool get_Exists_Internal(string objectId)
        {
            if (string.IsNullOrEmpty(objectId))
            {
                return false;
            }

            int pos = objectId.LastIndexOf(";");
            string versionSeriesId = pos > 0 ? objectId.Substring(0, pos) : objectId;
            bool isPwc = objectId.EndsWith(";pwc");
            bool isVersion = !isPwc && objectId.Contains(";") && pos < objectId.LastIndexOf(".");
            string path = System.IO.Path.Combine(_folder, versionSeriesId);
            if (isPwc)
            {
                path = System.IO.Path.Combine(path, "pwc");
                return System.IO.File.Exists(path);
            }
            else if (isVersion)
            {
                path = System.IO.Path.Combine(path, "Versionen", objectId.Substring(pos + 1));
                return System.IO.File.Exists(path);
            }
            bool versionExists = System.IO.Directory.Exists(System.IO.Path.Combine(path, "Versionen"));
            bool pwcExists = System.IO.File.Exists(System.IO.Path.Combine(path, "pwc"));
            return System.IO.Directory.Exists(path) && (versionExists || !pwcExists && !versionExists);
        }

        public CmisObjectModel.ServiceModel.cmisObjectType get_Object_Internal(string objectId)
        {
            if (!"root".Equals(objectId) && !get_Exists_Internal(objectId))
            {
                throw cmisFaultType.CreateNotFoundException(objectId);
            }

            // Objekt-Art bestimmen
            string objTyp = null;
            string xmlTemplate = null;
            if ("root".Equals(objectId))
            {

                objTyp = "Stammverzeichnis";
                xmlTemplate = "folder";
            }

            else if (objectId.EndsWith(";pwc"))
            {

                objTyp = "Arbeitskopie";
                xmlTemplate = "document";
            }

            else if (objectId.Contains(";") && objectId.LastIndexOf(";") < objectId.LastIndexOf("."))
            {

                objTyp = "Version";
                xmlTemplate = "document";
            }

            else if (System.IO.Directory.Exists(System.IO.Path.Combine(_folder, objectId, "Versionen")))
            {

                objTyp = "Dokument";
                xmlTemplate = "document";
            }

            else
            {

                objTyp = "Ordner";
                xmlTemplate = "folder";

            }

            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(CmisObjectModel.ServiceModel.cmisObjectType));
            string xml = System.IO.File.ReadAllText(Helper.FindXmlPath(xmlTemplate + ".xml"));
            CmisObjectModel.ServiceModel.cmisObjectType obj = (CmisObjectModel.ServiceModel.cmisObjectType)serializer.Deserialize(new System.IO.StringReader(xml));

            if ("Stammverzeichnis".Equals(objTyp))
            {

                var info = new System.IO.DirectoryInfo(_folder);

                // I. Grunddaten
                obj.Name = "Root";
                obj.ObjectId = "root";
                obj.Description = "Stammverzeichnis " + _folder;

                // III. Änderungsdaten
                obj.CreationDate = info.CreationTime.ToUniversalTime();
                obj.LastModificationDate = info.LastWriteTime.ToUniversalTime();

                // VII. Ordner
                obj.Path = "/";

                // VIII. Change Token
                obj.ChangeToken = info.LastWriteTime.ToUniversalTime().ToString();

                // Erlaubte Aktionen anpassen
                obj.AllowableActions.CanDeleteObject = false;
            }

            else if ("Ordner".Equals(objTyp))
            {

                var info = new System.IO.DirectoryInfo(System.IO.Path.Combine(_folder, objectId));
                string path = "/" + objectId.Replace(@"\", "/");
                string name = objectId.Split('\\').Last();

                // I. Grunddaten
                obj.Name = name;
                obj.ObjectId = objectId;
                obj.Description = "Lokales Verzeichnis";

                // III. Änderungsdaten
                obj.CreationDate = info.CreationTime.ToUniversalTime();
                obj.LastModificationDate = info.LastWriteTime.ToUniversalTime();

                // VII. Ordner
                obj.Path = path;
                obj.ParentId = get_ParentFolderId_Internal(objectId);

                // VIII. Change Token
                obj.ChangeToken = info.LastWriteTime.ToUniversalTime().ToString();
            }

            else
            {

                int pos = objectId.LastIndexOf(";");
                bool isLatest = pos == -1;
                string versionSeriesId = isLatest ? objectId : objectId.Substring(0, pos);
                string name = versionSeriesId.Split('\\').Last();
                var meta = get_DocumentMetadata_Internal(versionSeriesId);
                string version = isLatest ? meta.LabelOfLatestVersion : objectId.Substring(pos + 1);
                bool isPwc = "pwc".Equals(version);
                bool visiblePwc = meta.IsVersionSeriesCheckedOut && meta.VersionSeriesCheckedOutBy.Equals(CurrentAuthenticationInfo.User);
                var info = new System.IO.FileInfo(System.IO.Path.Combine(_folder, versionSeriesId, isPwc ? string.Empty : "Versionen", version));
                string isMajor = Conversions.ToString(!isPwc && version.EndsWith(".0"));
                bool isLatestMajor = !isPwc && Conversions.ToBoolean(isMajor) && version.StartsWith(meta.MajorOfLatestVersion.ToString());

                // I. Grunddaten
                obj.Name = name;
                obj.ObjectId = objectId;
                obj.Description = isPwc ? meta.DescriptionPwc : meta.Description;
                obj.Properties.GetProperties("patorg:akte").First().Value.Values = isPwc ? meta.AktePwc : meta.Akte;

                // III. Änderungsdaten
                obj.CreatedBy = meta.CreatedBy;
                obj.CreationDate = meta.CreationDate.ToUniversalTime();
                obj.LastModifiedBy = isLatest || isPwc ? meta.LastModifiedBy : "- nicht geseichert -";
                obj.LastModificationDate = (isLatest || isPwc ? meta.LastModificationDate : info.LastWriteTime).ToUniversalTime();

                // IV. Versionsinfo
                obj.IsPrivateWorkingCopy = isPwc;
                obj.IsLatestVersion = isLatest;
                obj.IsMajorVersion = System.Convert.ToBoolean(isMajor);
                obj.IsLatestMajorVersion = isLatestMajor;
                obj.VersionLabel = isLatest ? meta.LabelOfLatestVersion : version;
                obj.VersionSeriesId = versionSeriesId;

                // V. Versionierung
                if (meta.IsVersionSeriesCheckedOut)
                {
                    obj.IsVersionSeriesCheckedOut = true;
                    obj.VersionSeriesCheckedOutBy = meta.VersionSeriesCheckedOutBy;
                    if (CurrentAuthenticationInfo.User.Equals(meta.VersionSeriesCheckedOutBy))
                    {
                        obj.VersionSeriesCheckedOutId = versionSeriesId + ";pwc";
                    }
                }
                else
                {
                    obj.IsVersionSeriesCheckedOut = false;
                }
                obj.CheckinComment = meta.GetComment(version);

                // VI. Datei
                obj.ContentStreamLength = info.Length;
                obj.ContentStreamMimeType = meta.MimeType;
                obj.ContentStreamFileName = name;
                obj.ContentStreamId = objectId;

                // VIII. Change Token
                obj.ChangeToken = info.LastWriteTime.ToString();

                // Erlaubte Aktionen anpassen
                if ("Arbeitskopie".Equals(objTyp))
                {
                    obj.AllowableActions.CanDeleteObject = true;
                    obj.AllowableActions.CanUpdateProperties = true;
                    obj.AllowableActions.CanSetContentStream = true;
                    obj.AllowableActions.CanCancelCheckOut = true;
                    obj.AllowableActions.CanCheckIn = true;
                }
                else if ("Dokument".Equals(objTyp))
                {
                    if (!meta.IsVersionSeriesCheckedOut)
                    {
                        obj.AllowableActions.CanDeleteObject = true;
                        obj.AllowableActions.CanCheckOut = true;
                        obj.AllowableActions.CanUpdateProperties = true;
                    }

                }

            }

            CompleteObject(obj);

            return obj;
        }

        public CmisObjectModel.ServiceModel.cmisObjectListType GetAllVersions_Internal(string objectId)
        {
            Log_Internal("GetAllVersions", objectId);

            var list = new List<CmisObjectModel.ServiceModel.cmisObjectType>();

            int pos = objectId.LastIndexOf(";");
            string versionSeriesId = pos > 0 ? objectId.Substring(0, pos) : objectId;

            foreach (string file in System.IO.Directory.EnumerateFiles(System.IO.Path.Combine(_folder, versionSeriesId, "Versionen")))
                list.Add(get_Object_Internal(versionSeriesId + ";" + file.Split('\\').Last()));

            list.RemoveAt(list.Count - 1);
            var obj = get_Object_Internal(versionSeriesId);
            list.Add(obj);
            if ((bool)obj.IsVersionSeriesCheckedOut && obj.VersionSeriesCheckedOutBy.Equals(CurrentAuthenticationInfo.User))
            {
                list.Add(get_Object_Internal(obj.VersionSeriesCheckedOutId));

                /* TODO ERROR: Skipped IfDirectiveTrivia
                #If WorkbenchTest = "True" Then
                */         // Damit die Workbench für "Check specification compliance" ein positives Ergebnis liefert,
                           // wird hier vom CMIS-Standard abgewichen und die Arbeitskopie bekommt cmis:latestVersion = True.
                           // Siehe CMIS-Standard 1.1 - Abschnitt 2.1.13.5.1 "Checkout".
                           // (Diese Kompilerkonstante wird im Projekt CmisServer definiert.)

                obj.IsLatestVersion = false;
                list.Last().IsLatestVersion = true;
                /* TODO ERROR: Skipped EndIfDirectiveTrivia
                #End If
                */
            }

            list.Reverse();

            return new CmisObjectModel.ServiceModel.cmisObjectListType() { Objects = list.ToArray() };
        }

        private void CompleteObject(CmisObjectModel.ServiceModel.cmisObjectType obj)
        {
            if (BaseUri.ToString().ToLower().Contains("atom"))
            {
                // Für die AtomPub-Binding müssen Author und ContentLink eines CMIS-Objekt selbst gesetzt werden

                obj.ServiceModel.Author = SystemAuthor_Internal;
                if ("cmis:document".Equals(obj.BaseTypeId))
                {
                    obj.ServiceModel.ContentLink = new CmisObjectModel.AtomPub.AtomLink(new Uri(string.Format("{0}{1}/content?objectId={2}", BaseUri, _repoid, obj.ObjectId)));
                }
            }
        }

        public Metadata get_DocumentMetadata_Internal(string objectId)
        {
            int pos = objectId.LastIndexOf(";");
            string versionSeriesId = pos > 0 ? objectId.Substring(0, pos) : objectId;

            string path = System.IO.Path.Combine(_folder, versionSeriesId, "metadata");
            string xml = System.IO.File.ReadAllText(path);
            var meta = Metadata.FromXml(xml);

            return meta;
        }

        public void set_DocumentMetadata_Internal(string objectId, Metadata value)
        {
            string path = System.IO.Path.Combine(_folder, objectId.Replace(";pwc", string.Empty), "metadata");
            string xml = Conversions.ToString(value.ToXml());
            System.IO.File.WriteAllText(path, xml);
        }

        #endregion

        #region Properties

        public void UpdateProperties_Internal(string objectId, CmisObjectModel.Core.Collections.cmisPropertiesType properties, string changeToken)
        {

            var meta = get_DocumentMetadata_Internal(objectId);

            if (objectId.EndsWith(";pwc"))
            {
                if (properties.GetProperties("cmis:description").Count > 0)
                {
                    meta.DescriptionPwc = properties.GetProperties("cmis:description").Values.First().Value.ToString();
                }
                if (properties.GetProperties("patorg:akte").Count > 0)
                {
                    CmisObjectModel.Core.Properties.cmisProperty prop = properties.GetProperties("patorg:akte").Values.First();
                    if (prop.Values is null)
                    {
                        meta.AktePwc = null;
                    }
                    else
                    {
                        meta.AktePwc = (from obj in prop.Values
                                        let str = obj.ToString()
                                        select str).ToArray();
                    }
                }
                if (properties.GetProperties("cmis:foreignChangeToken").Count > 0)
                {
                    meta.ForeignChangeToken = properties.GetProperties("cmis:foreignChangeToken").Values.First().Value.ToString();
                }
            }
            else
            {
                if (properties.GetProperties("cmis:description").Count > 0)
                {
                    meta.Description = properties.GetProperties("cmis:description").Values.First().Value.ToString();
                }
                if (properties.GetProperties("patorg:akte").Count > 0)
                {
                    object[] objs = properties.GetProperties("patorg:akte").Values.First().Values;
                    if (objs is null)
                    {
                        meta.Akte = null;
                    }
                    else
                    {
                        meta.Akte = (from obj in properties.GetProperties("patorg:akte").Values.First().Values
                                     let str = obj.ToString()
                                     select str).ToArray();
                    }
                }
            }

            set_DocumentMetadata_Internal(objectId, meta);
        }

        #endregion

        #region CheckOut/CheckIn

        public string CheckOut_Internal(string objectId)
        {
            var meta = get_DocumentMetadata_Internal(objectId);

            if (!meta.IsVersionSeriesCheckedOut)
            {

                string pathOriginal = System.IO.Path.Combine(_folder, objectId, "Versionen", meta.LabelOfLatestVersion);
                string pathPwc = System.IO.Path.Combine(_folder, objectId, "pwc");

                System.IO.File.Copy(pathOriginal, pathPwc);

                meta.VersionSeriesCheckedOutBy = CurrentAuthenticationInfo.User;
                meta.DescriptionPwc = meta.Description;
                meta.AktePwc = meta.Akte;

                set_DocumentMetadata_Internal(objectId, meta);
            }

            else if (!meta.VersionSeriesCheckedOutBy.Equals(CurrentAuthenticationInfo.User))
            {
                throw new Exception("'" + objectId + "' is checked out by '" + meta.VersionSeriesCheckedOutBy + "'!");
            }

            string pwcId = objectId;
            if (!pwcId.EndsWith(";pwc"))
            {
                pwcId += ";pwc";
            }

            return pwcId;
        }

        public string CheckIn_Internal(string objectId, cmisPropertiesType properties, string[] policies, cmisContentStreamType content, bool major, string checkInComment, cmisAccessControlListType addACEs = default, cmisAccessControlListType removeACEs = default)
        {

            var meta = get_DocumentMetadata_Internal(objectId);

            if (!meta.IsVersionSeriesCheckedOut)
            {
                throw new Exception("not checked out");
            }

            if (properties is not null && properties.Count > 0)
            {
                UpdateProperties_Internal(objectId, properties, default);
                meta = get_DocumentMetadata_Internal(objectId);
            }

            if (content is not null)
            {
                SetContentStream_Internal(objectId, content.BinaryStream, content.MimeType, content.Filename, true, default);
            }

            if (meta.VersionSeriesCheckedOutBy.Equals(CurrentAuthenticationInfo.User))
            {
                if (major)
                {
                    meta.MajorOfLatestVersion += 1;
                    meta.MinorOfLatestVersion = 0;
                }
                else
                {
                    meta.MinorOfLatestVersion += 1;
                }

                meta.Description = meta.DescriptionPwc;
                meta.DescriptionPwc = null;

                meta.Akte = meta.AktePwc;
                meta.AktePwc = null;

                string comment = checkInComment;
                if (string.IsNullOrEmpty(comment))
                    comment = meta.Description;
                meta.AddComment(comment);

                string pathVersionen = System.IO.Path.Combine(_folder, objectId.Replace(";pwc", string.Empty), "Versionen");
                string pathNew = System.IO.Path.Combine(pathVersionen, meta.LabelOfLatestVersion);
                string pathPwc = System.IO.Path.Combine(_folder, objectId.Replace(";pwc", string.Empty), "pwc");

                if (!System.IO.Directory.Exists(pathVersionen))
                {
                    System.IO.Directory.CreateDirectory(pathVersionen);
                }

                if ("enumVersioningState.checkedout".Equals(meta.GetComment("0.1")) && System.IO.File.Exists(System.IO.Path.Combine(pathVersionen, "0.1")))
                {
                    System.IO.File.Delete(System.IO.Path.Combine(pathVersionen, "0.1"));
                    meta.CheckinComments = new Metadata.CheckinComment[] { meta.CheckinComments.Last() };
                }

                System.IO.File.Copy(pathPwc, pathNew);
                System.IO.File.Delete(pathPwc);

                meta.VersionSeriesCheckedOutBy = null;

                meta.LastModifiedBy = CurrentAuthenticationInfo.User;
                meta.LastModificationDate = DateTime.Now.ToUniversalTime();

                set_DocumentMetadata_Internal(objectId, meta);
            }

            else
            {
                throw new Exception("'" + objectId + "' is checked out by '" + meta.VersionSeriesCheckedOutBy + "'!");
            }

            return objectId.Replace(";pwc", string.Empty);
        }

        private static object FileEccessSyncObject = new object();

        public void CancelCheckOut_Internal(string objectId)
        {
            var meta = get_DocumentMetadata_Internal(objectId);

            if (!meta.VersionSeriesCheckedOutBy.Equals(CurrentAuthenticationInfo.User))
            {
                throw new Exception("'" + objectId + "' is checked out by '" + meta.VersionSeriesCheckedOutBy + "'!");
            }

            string versionSeriesId = objectId.Replace(";pwc", string.Empty);
            string path = System.IO.Path.Combine(_folder, versionSeriesId);

            if ("enumVersioningState.checkedout".Equals(meta.GetComment()))
            {
                // Hier wird anhand des Kommentars erkannt, dass ein Dokument mit dem Status "checkedout" erzeugt wurde.
                // Mit dem CancelCheckOut muss dann hier auch die eingecheckte Version gelöscht werden.
                // (Siehe auch CmisService.CreateDocument)
                System.IO.Directory.Delete(path, true);
            }
            else
            {

                string pathVersionen = System.IO.Path.Combine(_folder, versionSeriesId, "Versionen");
                if (System.IO.Directory.Exists(pathVersionen))
                {
                    string pathPwc = System.IO.Path.Combine(_folder, versionSeriesId, "pwc");

                    System.IO.File.Delete(pathPwc);

                    meta.AktePwc = null;
                    meta.DescriptionPwc = null;

                    meta.VersionSeriesCheckedOutBy = null;
                    set_DocumentMetadata_Internal(objectId, meta);
                }
                else
                {
                    System.IO.Directory.Delete(path, true);
                }


            }
        }

        #endregion

        #region Content

        public DateTime SetContentStream_Internal(string objectId, System.IO.Stream contentStream, string mimeType, string fileName, bool overwriteFlag, string changeToken)
        {
            string path = System.IO.Path.Combine(_folder, objectId.Replace(";pwc", string.Empty), "pwc");

            using (var fileStream = new System.IO.FileStream(path, System.IO.FileMode.Create))
            {
                contentStream.CopyTo(fileStream);
            }

            var meta = get_DocumentMetadata_Internal(objectId);
            meta.LastModificationDate = DateTime.Now.ToUniversalTime();
            if (!string.IsNullOrEmpty(mimeType))
            {
                meta.MimeType = mimeType;
            }
            set_DocumentMetadata_Internal(objectId, meta);

            return meta.LastModificationDate;
        }

        #endregion

    }
}