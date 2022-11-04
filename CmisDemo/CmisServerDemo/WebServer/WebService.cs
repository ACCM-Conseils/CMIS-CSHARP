using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace WebServer
{

    public class WebService : IWebService
    {

        private static string _folder = System.Configuration.ConfigurationManager.AppSettings["folder"];

        public System.IO.Stream ShowObject(string objectId)
        {
            if (string.IsNullOrWhiteSpace(objectId) || "{0}".Equals(objectId))
                objectId = "root";

            int pos = objectId.LastIndexOf(';');
            string versionSeriesId = pos < 0 ? objectId : objectId.Substring(0, pos);

            string details = string.Empty;
            if (System.IO.File.Exists(System.IO.Path.Combine(_folder, versionSeriesId, "metadata")))
            {
                // Document

                var versions = new List<string>();
                if (System.IO.Directory.Exists(System.IO.Path.Combine(_folder, versionSeriesId, "Versionen")))
                {
                    versions.AddRange(from path in System.IO.Directory.EnumerateFiles(System.IO.Path.Combine(_folder, versionSeriesId, "Versionen"))
                                      let name = path.Split('\\').Last()
                                      let id = versionSeriesId + ";" + name
                                      orderby double.Parse(name)
                                      select (name + " <small> <a href=\"obj?id=" + id + "\">" + id + "</a></small>"));
                }
                if (System.IO.File.Exists(System.IO.Path.Combine(_folder, versionSeriesId, "pwc")))
                {
                    versions.Add("pwc" + " <small> <a href=\"obj?id=" + versionSeriesId + ";pwc\">" + versionSeriesId + ";pwc</a></small>");
                }
                versions.Reverse();
                details = "Inhalt<ul><li><a href=\" file?id=" + objectId + "\">Content</a><li><a href=\" meta?id=" + objectId + "\">Metadaten</a></ul>" + "Versionen<ul><li>" + string.Join("<li>", versions) + "</ul>" + "Serie<ul><li><a href=\"obj?id=" + versionSeriesId + "\">" + versionSeriesId + "</a>";

            }
            else
            {
                // Folder

                if ("root".Equals(versionSeriesId))
                    versionSeriesId = string.Empty;

                var subfolders = from folder in System.IO.Directory.EnumerateDirectories(System.IO.Path.Combine(_folder, versionSeriesId))
                                 where !System.IO.File.Exists(System.IO.Path.Combine(folder, "metadata"))
                                 let id = folder.Replace(_folder + @"\", string.Empty)
                                 select ("<a href=\"obj?id=" + id + "\">" + System.IO.Path.GetFileName(folder) + "</a>");
                var files = from folder in System.IO.Directory.EnumerateDirectories(System.IO.Path.Combine(_folder, versionSeriesId))
                            where System.IO.File.Exists(System.IO.Path.Combine(folder, "metadata"))
                            let id = folder.Replace(_folder + @"\", string.Empty)
                            select ("<a href=\"obj?id=" + id + "\">" + System.IO.Path.GetFileName(folder) + "</a>");

                details = "Verzeichnisse<ul>";
                if (subfolders.Count() > 0)
                {
                    details += "<li>" + string.Join("<li>", subfolders);
                }
                else
                {
                    details += "<i>keine Unterverzeichnisse</i>";
                }
                details += "</ul>Dokumente<ul>";
                if (files.Count() > 0)
                {
                    details += "<li>" + string.Join("<li>", files);
                }
                else
                {
                    details += "<i>keine Dokumente</i>";
                }
                details += "</ul>";
            }

            return new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes("<!DOCTYPE html>" + "<html><head><meta http-equiv=\"Content-Type\" content=\"text/html; charset=iso-8859-1\" />" + "<title> CmisDemo - WebService</title>" + "</head>" + "<body>" + "<h1>" + objectId + "</h1>" + "<p>" + details + "</p>" + "</body></html>"));







        }

        public System.IO.Stream GetContent(string objectId)
        {

            int pos = objectId.LastIndexOf(';');
            string versionSeriesId;
            string filename;
            if (pos < 0)
            {
                versionSeriesId = objectId;

                string dir = System.IO.Path.Combine(_folder, versionSeriesId, "Versionen");
                double maximum = (from path in System.IO.Directory.EnumerateFiles(dir)
                                  let name = path.Split('\\').Last()
                                  select double.Parse(name.Replace(".", ","))).Max();
                filename = (from path in System.IO.Directory.EnumerateFiles(dir)
                            let name = path.Split('\\').Last()
                            where double.Parse(name.Replace(".", ",")) == maximum
                            select path).Single();
            }
            else
            {
                versionSeriesId = objectId.Substring(0, pos);
                string version = objectId.Substring(pos + 1);
                if (!version.Equals("pwc"))
                {
                    filename = System.IO.Path.Combine(_folder, versionSeriesId, "Versionen", version);
                }
                else
                {
                    filename = System.IO.Path.Combine(_folder, versionSeriesId, version);
                }
            }

            string metaXml = System.IO.File.ReadAllText(System.IO.Path.Combine(_folder, versionSeriesId, "metadata"));
            int mimeTypeStart = metaXml.IndexOf("<MimeType>") + "<MimeType>".Length;
            int mimeTypeEnd = metaXml.IndexOf("</MimeType>");
            string mimeType = metaXml.Substring(mimeTypeStart, mimeTypeEnd - mimeTypeStart);

            System.ServiceModel.Web.WebOperationContext.Current.OutgoingResponse.ContentType = mimeType;
            System.ServiceModel.Web.WebOperationContext.Current.OutgoingResponse.Headers.Add("content-disposition", "attachment; filename=\"" + System.IO.Path.GetFileName(System.IO.Path.Combine(_folder, versionSeriesId)) + "\"");
            return System.IO.File.OpenRead(filename);
        }

        private System.IO.Stream GetMetadata(string objectId)
        {
            System.ServiceModel.Web.WebOperationContext.Current.OutgoingResponse.ContentType = "text/xml";

            int pos = objectId.LastIndexOf(';');
            string versionSeriesId = pos < 0 ? objectId : objectId.Substring(0, pos);
            string filename = System.IO.Path.Combine(_folder, versionSeriesId, "metadata");
            string xml = System.IO.File.ReadAllText(filename);
            xml = xml.Replace(" encoding=\"utf-16\"", string.Empty);
            return new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(xml));
        }

        System.IO.Stream IWebService.GetMetadata(string objectId) => GetMetadata(objectId);
    }
}