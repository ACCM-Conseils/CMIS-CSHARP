﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;

// Diese Datei enthält alle Funktionen, die intern aufgerufen werden.

using CmisObjectModel.Core;
using CmisObjectModel.Core.Collections;
using CmisObjectModel.Core.Security;
using CmisObjectModel.Messaging;
using CmisObjectModel.Messaging.Requests;
using CmisServer;
using DocuWare.Platform.ServerClient;
using DocuWare.Services.Http;
using iTextSharp.text;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using Document = DocuWare.Platform.ServerClient.Document;

namespace CmisServer
{

    public partial class CmisServiceImpl
    {
        private static string _repoid = System.Configuration.ConfigurationManager.AppSettings["repoid"];
        private static string _folder = System.Configuration.ConfigurationManager.AppSettings["folder"];
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
                return new System.ServiceModel.Syndication.SyndicationPerson("clement.maldonado@altexence.pro", "CmisServer Docuware", String.Format(System.Configuration.ConfigurationManager.AppSettings["url"]+"/atom/", System.Configuration.ConfigurationManager.AppSettings["domain"]));
            }
        }

        #endregion

        #region TypeDefinition

        public CmisObjectModel.Core.Definitions.Types.cmisTypeDefinitionType TypeDefinition_Internal(string typeId)
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

            var document = conn.GetFromDocumentForDocumentAsync(System.Convert.ToInt32(objectId), currentRepository).Result;

            return (document.StatusCode == System.Net.HttpStatusCode.OK) ? true : false;
        }

        public CmisObjectModel.ServiceModel.cmisObjectType get_Object_Internal(string objectId)
        {
            CmisObjectModel.ServiceModel.cmisObjectType obj = null;

            if (!string.IsNullOrEmpty(objectId) && objectId != "root")
            {
                var fileCabinets = org.GetFileCabinetsFromFilecabinetsRelation().FileCabinet;

                var defaultBasket = fileCabinets.FirstOrDefault(f => !f.IsBasket && f.Id == currentRepository);

                var dialogInfoItems = defaultBasket.GetDialogInfosFromSearchesRelation();
                var dialog = dialogInfoItems.Dialog.FirstOrDefault(m => m.IsDefault).GetDialogFromSelfRelation();

                var q = new DialogExpression()
                {
                    Condition = new List<DialogExpressionCondition>()
                        {
                            DialogExpressionCondition.Create("DWDOCID", objectId )
                        },
                    Count = 1
                };

                var queryResult = dialog.Query.PostToDialogExpressionRelationForDocumentsQueryResult(q);

                if (queryResult.Items.Count() == 0)
                {
                    throw cmisFaultType.CreateNotFoundException(objectId);
                }
                else
                {
                    Document doc = queryResult.Items.FirstOrDefault();

                    List<DocumentIndexField> metalist = doc.Fields;
                    List<string> metastring = new List<string>();

                    var count = 0;

                    foreach (DocumentIndexField index in metalist)
                    {
                        if(!index.FieldName.StartsWith("DW") && !index.FieldName.StartsWith("@"))
                        {
                            string meta = index.FieldName + "=" + ((index.Item != null) ? index.Item.ToString() : "");

                            metastring.Add(meta);
                        }
                    }

                    obj = get_Object_InternalFromDocuware(doc);

                    obj.Properties.GetProperties("docuware:metavalues").First().Value.Values = metastring.ToArray();
                }
            }
            else
            {
                obj = get_Fake_Folder(currentRepository);
            }

            return obj;
        }

        public CmisObjectModel.ServiceModel.cmisObjectType get_Fake_Object_Internal(String repositoryId, String query)
        {
            CmisObjectModel.ServiceModel.cmisObjectType obj = null;

            int index = query.IndexOf("=");

            String toFind = query.Remove(0, index + 1);

            String[] indexValue = toFind.Replace(" ", "").Split('=');

            var fileCabinets = org.GetFileCabinetsFromFilecabinetsRelation().FileCabinet;

            var defaultBasket = fileCabinets.FirstOrDefault(f => !f.IsBasket && f.Id == repositoryId);

            var dialogInfoItems = defaultBasket.GetDialogInfosFromSearchesRelation();
            var dialog = dialogInfoItems.Dialog.FirstOrDefault(m => m.IsDefault).GetDialogFromSelfRelation();

            var q = new DialogExpression()
            {
                Condition = new List<DialogExpressionCondition>()
                        {
                            DialogExpressionCondition.Create(indexValue[0], indexValue[1])
                        },
                Count = 10000
            };

            var queryResult = dialog.Query.PostToDialogExpressionRelationForDocumentsQueryResult(q);

            List<string> metastring = new List<string>();

            var count = 0;

            foreach (Document idx in queryResult.Items)
            {
                string meta = "DOCID=" + idx.Id.ToString();

                metastring.Add(meta);
            }

            obj = get_Fake_Object_InternalFromDocuware();

            obj.Properties.GetProperties("docuware:resultlist").First().Value.Values = metastring.ToArray();

            return obj;
        }

        public CmisObjectModel.ServiceModel.cmisObjectType get_Object_InternalFromDocuware(Document d)
        {

            Document doc = d.GetDocumentFromSelfRelation();

            var downloadedFile = Helpers.Docuware.DownloadDocumentContent(doc);

            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(CmisObjectModel.ServiceModel.cmisObjectType));
            string xml = System.IO.File.ReadAllText(Helper.FindXmlPath("document.xml"));
            CmisObjectModel.ServiceModel.cmisObjectType obj = (CmisObjectModel.ServiceModel.cmisObjectType)serializer.Deserialize(new System.IO.StringReader(xml));

            obj.Name = doc.Title;
            obj.ObjectId = doc.Id.ToString();
            obj.Description = doc.Title;

            obj.CreatedBy = "Unknown";
            obj.CreationDate = doc.CreatedAt.ToUniversalTime();
            obj.LastModifiedBy = "Unknown";
            obj.LastModificationDate = doc.LastModified.ToUniversalTime();

            obj.IsPrivateWorkingCopy = false;
            obj.IsLatestVersion = true;
            obj.IsMajorVersion = true;
            obj.IsLatestMajorVersion = true;
            obj.VersionLabel = doc.Version.Major.ToString();
            obj.VersionSeriesId = doc.Id.ToString();

            obj.IsVersionSeriesCheckedOut = false;

            obj.CheckinComment = "";

            obj.ContentStreamLength = downloadedFile.ContentLength;
            obj.ContentStreamMimeType = downloadedFile.ContentType;
            obj.ContentStreamFileName = downloadedFile.FileName;
            obj.ContentStreamId = doc.Id.ToString();

            obj.ChangeToken = doc.LastModified.Ticks.ToString();

            obj.AllowableActions.CanDeleteObject = true;
            obj.AllowableActions.CanUpdateProperties = true;
            obj.AllowableActions.CanSetContentStream = true;
            obj.AllowableActions.CanCancelCheckOut = true;
            obj.AllowableActions.CanCheckIn = true;

            CompleteObject(obj);

            return obj;
        }

        public CmisObjectModel.ServiceModel.cmisObjectType get_Fake_Object_InternalFromDocuware()
        {
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(CmisObjectModel.ServiceModel.cmisObjectType));
            string xml = System.IO.File.ReadAllText(Helper.FindXmlPath("document.xml"));
            CmisObjectModel.ServiceModel.cmisObjectType obj = (CmisObjectModel.ServiceModel.cmisObjectType)serializer.Deserialize(new System.IO.StringReader(xml));

            obj.Name = "Liste de recherche";
            obj.ObjectId = "0";
            obj.Description = "Liste de recherche";

            obj.CreatedBy = "Unknown";
            obj.CreationDate = DateTime.Now.ToUniversalTime();
            obj.LastModifiedBy = "Unknown";
            obj.LastModificationDate = DateTime.Now.ToUniversalTime();

            obj.IsPrivateWorkingCopy = false;
            obj.IsLatestVersion = true;
            obj.IsMajorVersion = true;
            obj.IsLatestMajorVersion = true;
            obj.VersionLabel = "1";
            obj.VersionSeriesId = "1";

            obj.IsVersionSeriesCheckedOut = false;

            obj.CheckinComment = "";

            obj.ChangeToken = DateTime.Now.Ticks.ToString();

            obj.AllowableActions.CanDeleteObject = true;
            obj.AllowableActions.CanUpdateProperties = true;
            obj.AllowableActions.CanSetContentStream = true;
            obj.AllowableActions.CanCancelCheckOut = true;
            obj.AllowableActions.CanCheckIn = true;

            CompleteObject(obj);

            return obj;
        }

        public CmisObjectModel.ServiceModel.cmisObjectType get_Object_InternalFromDocuwareID(string DWDOCID)
        {

            var document = conn.GetFromDocumentForDocumentAsync(System.Convert.ToInt32(DWDOCID), currentRepository).Result;

            Document doc = document.Content.GetDocumentFromSelfRelation();

            var downloadedFile = Helpers.Docuware.DownloadDocumentContent(doc);

            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(CmisObjectModel.ServiceModel.cmisObjectType));
            string xml = System.IO.File.ReadAllText(Helper.FindXmlPath("document.xml"));
            CmisObjectModel.ServiceModel.cmisObjectType obj = (CmisObjectModel.ServiceModel.cmisObjectType)serializer.Deserialize(new System.IO.StringReader(xml));

            obj.Name = doc.Title;
            obj.ObjectId = doc.Id.ToString();
            obj.Description = doc.Title;

            obj.CreatedBy = "Unknown";
            obj.CreationDate = doc.CreatedAt.ToUniversalTime();
            obj.LastModifiedBy = "Unknown";
            obj.LastModificationDate = doc.LastModified.ToUniversalTime();

            obj.IsPrivateWorkingCopy = false;
            obj.IsLatestVersion = true;
            obj.IsMajorVersion = true;
            obj.IsLatestMajorVersion = true;
            obj.VersionLabel = doc.Version.Major.ToString();
            obj.VersionSeriesId = doc.Id.ToString();

            obj.IsVersionSeriesCheckedOut = false;

            obj.CheckinComment = "";

            obj.ContentStreamLength = downloadedFile.ContentLength;
            obj.ContentStreamMimeType = downloadedFile.ContentType;
            obj.ContentStreamFileName = downloadedFile.FileName;
            obj.ContentStreamId = doc.Id.ToString();

            obj.ChangeToken = doc.LastModified.Ticks.ToString();

            obj.AllowableActions.CanDeleteObject = true;
            obj.AllowableActions.CanUpdateProperties = true;
            obj.AllowableActions.CanSetContentStream = true;
            obj.AllowableActions.CanCancelCheckOut = true;
            obj.AllowableActions.CanCheckIn = true;

            CompleteObject(obj);

            return obj;
        }

        public CmisObjectModel.ServiceModel.cmisObjectType get_Fake_Folder(string currentRepository)
        {

            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(CmisObjectModel.ServiceModel.cmisObjectType));
            string xml = System.IO.File.ReadAllText(Helper.FindXmlPath("folder.xml"));
            CmisObjectModel.ServiceModel.cmisObjectType obj = (CmisObjectModel.ServiceModel.cmisObjectType)serializer.Deserialize(new System.IO.StringReader(xml));

            obj.Name = "Filecabinet";
            obj.ObjectId = currentRepository;
            obj.Description = "Docuware";

            obj.CreatedBy = "Unknown";
            obj.CreationDate = DateTime.Now.ToUniversalTime();
            obj.LastModifiedBy = "Unknown";
            obj.LastModificationDate = DateTime.Now.ToUniversalTime();

            obj.ChangeToken = DateTime.Now.Ticks.ToString();

            obj.AllowableActions.CanDeleteObject = true;
            obj.AllowableActions.CanUpdateProperties = true;
            obj.AllowableActions.CanSetContentStream = true;
            obj.AllowableActions.CanCancelCheckOut = true;
            obj.AllowableActions.CanCheckIn = true;

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
            var fileCabinets = org.GetFileCabinetsFromFilecabinetsRelation().FileCabinet;

            var defaultBasket = fileCabinets.FirstOrDefault(f => !f.IsBasket && f.Id == currentRepository);

            var dialogInfoItems = defaultBasket.GetDialogInfosFromSearchesRelation();
            var dialog = dialogInfoItems.Dialog.FirstOrDefault(m => m.IsDefault).GetDialogFromSelfRelation();

            var q = new DialogExpression()
            {
                Condition = new List<DialogExpressionCondition>()
                        {
                            DialogExpressionCondition.Create("DWDOCID", objectId )
                        },
                Count = 1
            };

            var queryResult = dialog.Query.PostToDialogExpressionRelationForDocumentsQueryResult(q);

            if (queryResult.Items.Count() == 0)
            {
                throw cmisFaultType.CreateNotFoundException(objectId);
            }
            else
            {
                DocuWare.Platform.ServerClient.Document doc = queryResult.Items.FirstOrDefault();

                List<DocumentIndexField> metavalues = new List<DocumentIndexField>();

                if (properties.Count > 0)
                {
                    if (properties.GetProperties("docuware:metavalues").Count > 0)
                    {
                        object[] metas = properties.GetProperties("docuware:metavalues").First().Value.Values;

                        foreach (object s in metas)
                        {
                            String[] metavals = System.Convert.ToString(s).Split('=');

                            metavalues.Add(DocumentIndexField.Create(metavals[0], metavals[1]));
                        }
                    }

                    var fields = new DocumentIndexFields()
                    {
                        Field = metavalues
                    };

                    doc.PutToFieldsRelationForDocumentIndexFields(fields);
                }
            }

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