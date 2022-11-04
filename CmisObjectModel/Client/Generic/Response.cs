using System;
using sn = System.Net;
using ss = System.ServiceModel;
using ssw = System.ServiceModel.Web;
using sx = System.Xml;
using sxs = System.Xml.Serialization;
using ca = CmisObjectModel.AtomPub;
using CmisObjectModel.Constants;
/* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.Client.Generic
{
    /// <summary>
   /// Response differs from void or stream
   /// </summary>
   /// <typeparam name="TResponse"></typeparam>
   /// <remarks></remarks>
    public class ResponseType<TResponse> : Response
    {

        public ResponseType(TResponse response, string contentType) : base(sn.HttpStatusCode.OK, "OK", contentType)
        {
            _response = response;
        }
        public ResponseType(TResponse response, string contentType, ss.FaultException exception) : base(exception)
        {
            _response = response;
            _contentType = contentType;
        }
        public ResponseType(sn.HttpStatusCode statusCode, string message, string contentType, TResponse response) : base(statusCode, message, contentType)
        {
            _response = response;
        }
        public ResponseType(sn.HttpWebResponse response, Func<sx.XmlReader, TResponse> responseFactory) : base(response)
        {

            // create TResponse from stream
            // evaluate response-stream
            CreateResponseFromStream(ms => { using (var sr = new System.IO.StreamReader(ms, new System.Text.UTF8Encoding(false))) { var reader = sx.XmlReader.Create(sr); _response = responseFactory.Invoke(reader); reader.Close(); } });
        }
        public ResponseType(sn.HttpWebResponse response, Func<System.IO.MemoryStream, string, TResponse> responseFactory) : base(response)
        {

            // create TResponse from stream
            // evaluate response-stream
            CreateResponseFromStream(ms => _response = responseFactory.Invoke(ms, _contentType));
        }
        public ResponseType(sn.HttpWebResponse response, Func<string, string, TResponse> responseFactory) : base(response)
        {

            // create TResponse from stream
            // evaluate response-stream
            CreateResponseFromStream(ms => _response = responseFactory.Invoke(System.Text.Encoding.UTF8.GetString(ms.ToArray()), _contentType));
        }
        private void CreateResponseFromStream(Action<System.IO.MemoryStream> responseFactory)
        {
            if (_stream is not null)
            {
                using (var ms = new System.IO.MemoryStream())
                {
                    _stream.CopyTo(ms);
                    try
                    {
                        _stream.Close();
                    }
                    catch
                    {
                    }
                    _stream = new System.IO.MemoryStream(ms.ToArray());

                    try
                    {
                        if (ms.Length != 0L)
                        {
                            ms.Position = 0L;
                            responseFactory.Invoke(ms);
                        }
                    }
                    catch (ssw.WebFaultException ex)
                    {
                        _statusCode = ex.StatusCode;
                        _message = ex.Message;
                        _exception = ex;
                    }
                    catch (ssw.WebFaultException<string> ex)
                    {
                        _statusCode = ex.StatusCode;
                        _message = ex.Detail;
                        _exception = ex;
                    }
                    catch (ssw.WebFaultException<Messaging.cmisFaultType> ex)
                    {
                        _statusCode = ex.StatusCode;
                        _message = ex.Detail.Message;
                        _exception = ex;
                    }
                    catch (ssw.WebFaultException<Exception> ex)
                    {
                        _statusCode = ex.StatusCode;
                        _message = ex.Message;
                        _exception = ex;
                    }
                    catch (ss.FaultException ex)
                    {
                        _statusCode = sn.HttpStatusCode.InternalServerError;
                        _message = ex.Message;
                        _exception = ex;
                    }
                    catch (sx.XmlException ex)
                    {
                        _statusCode = sn.HttpStatusCode.ExpectationFailed;
                        _message = ex.Message;
                        _exception = new ssw.WebFaultException<Exception>(ex, sn.HttpStatusCode.ExpectationFailed);
                    }
                    catch (Exception ex)
                    {
                        _statusCode = sn.HttpStatusCode.InternalServerError;
                        _message = ex.Message;
                    }
                    ms.Close();
                }
            }
        }
        public ResponseType(ss.FaultException exception) : base(exception)
        {
        }
        public ResponseType(sn.WebException exception) : base(exception)
        {
        }

        private TResponse _response;
        public TResponse Response
        {
            get
            {
                return _response;
            }
        }

        public static implicit operator TResponse(ResponseType<TResponse> value)
        {
            return value is null ? default : value.Response;
        }
        public static implicit operator ResponseType<TResponse>(TResponse value)
        {
            var responseType = typeof(TResponse);

            if (typeof(ca.AtomEntry).IsAssignableFrom(responseType))
            {
                return new ResponseType<TResponse>(value, MediaTypes.Entry);
            }
            else if (typeof(ca.AtomFeed).IsAssignableFrom(responseType))
            {
                return new ResponseType<TResponse>(value, MediaTypes.Feed);
            }
            else if (typeof(ca.AtomCollectionInfo).IsAssignableFrom(responseType))
            {
                return new ResponseType<TResponse>(value, MediaTypes.Xml);
            }
            else if (typeof(ca.AtomLink).IsAssignableFrom(responseType))
            {
                return new ResponseType<TResponse>(value, MediaTypes.Xml);
            }
            else if (typeof(ca.AtomWorkspace).IsAssignableFrom(responseType))
            {
                return new ResponseType<TResponse>(value, MediaTypes.Xml);
            }
            else if (ReferenceEquals(typeof(string), responseType))
            {
                return new ResponseType<TResponse>(value, MediaTypes.PlainText);
            }
            else if (typeof(Core.Security.cmisAccessControlListType).IsAssignableFrom(responseType))
            {
                return new ResponseType<TResponse>(value, MediaTypes.Acl);
            }
            else if (typeof(Core.cmisAllowableActionsType).IsAssignableFrom(responseType))
            {
                return new ResponseType<TResponse>(value, MediaTypes.AllowableActions);
            }
            else if (typeof(ca.AtomServiceDocument).IsAssignableFrom(responseType))
            {
                return new ResponseType<TResponse>(value, MediaTypes.Service);
            }
            else if (typeof(Serialization.XmlSerializable).IsAssignableFrom(typeof(TResponse)))
            {
                return new ResponseType<TResponse>(value, MediaTypes.Xml);
            }
            else if (typeof(sxs.IXmlSerializable).IsAssignableFrom(typeof(TResponse)))
            {
                return new ResponseType<TResponse>(value, MediaTypes.XmlApplication);
            }
            else
            {
                return new ResponseType<TResponse>(value, null);
            }
        }
        public static implicit operator ResponseType<TResponse>(ss.FaultException value)
        {
            return new ResponseType<TResponse>(value);
        }

    }
}