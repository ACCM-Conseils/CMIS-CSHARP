using DocuWare.Platform.ServerClient;
using DocuWare.Services.Http;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
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
            FileDownload dataToSend = (FileDownload)null;

                    FileDownload fileDownload1 = new FileDownload();
                    fileDownload1.TargetFileType = FileDownloadType.PDF;
                    fileDownload1.KeepAnnotations = false;
                    fileDownload1.AutoPrint = false;
                    dataToSend = fileDownload1;
                    
            DeserializedHttpResponse<Stream> result = document.PostToFileDownloadRelationForStreamAsync(dataToSend).Result;
            HttpContentHeaders contentHeaders = result.ContentHeaders;

            MemoryStream ms = new MemoryStream();
            byte[] chunk = new byte[4096];
            int bytesRead;
            while ((bytesRead = result.Content.Read(chunk, 0, chunk.Length)) > 0)
            {
                ms.Write(chunk, 0, bytesRead);
            }

            return new FileDownloadResult()
                {
                    Stream = result.Content,
                    ContentLength = ms.Length,
                    ContentType = contentHeaders.ContentType.MediaType
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
