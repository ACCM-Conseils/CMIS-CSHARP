using DocuWare.Platform.ServerClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CmisServer.Helpers
{
    public static class Docuware
    {
        static public ServiceConnection Connect(string userName, string password)
        {
            return ServiceConnection.Create(new System.Uri(ConfigurationManager.AppSettings["URLDocuware"]), userName, password, "");

        }

        public static Organization GetOrganization(ServiceConnection conn)
        {
            return conn.Organizations[0];
        }

        public static FileDownloadResult DownloadDocumentContent(this Document document)
        {
            if (document.FileDownloadRelationLink == null)
                document = document.GetDocumentFromSelfRelation();

            var downloadResponse = document.PostToFileDownloadRelationForStreamAsync(
                new FileDownload()
                {
                    TargetFileType = FileDownloadType.PDF
                }).Result;

            var contentHeaders = downloadResponse.ContentHeaders;
            return new FileDownloadResult()
            {
                Stream = downloadResponse.Content,
                ContentLength = contentHeaders.ContentLength,
                ContentType = contentHeaders.ContentType.MediaType,
                FileName = ((contentHeaders.ContentDisposition.FileName != null) ? contentHeaders.ContentDisposition.FileName : contentHeaders.ContentDisposition.FileNameStar)
            };
        }

        public static string SerializeObject<T>(this T toSerialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(toSerialize.GetType());

            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, toSerialize);
                return textWriter.ToString();
            }
        }
    }

    public class FileDownloadResult
    {
        public string ContentType { get; set; }
        public string FileName { get; set; }
        public long? ContentLength { get; set; }
        public System.IO.Stream Stream { get; set; }
    }
}
