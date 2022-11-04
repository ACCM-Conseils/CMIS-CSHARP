

// depends on the chosen interpretation of the xs:integer expression in a xsd-file
/* TODO ERROR: Skipped IfDirectiveTrivia
#If xs_Integer = "Int32" OrElse xs_integer = "Integer" OrElse xs_integer = "Single" Then
*//* TODO ERROR: Skipped DisabledTextTrivia
Imports xs_Integer = System.Int32
*//* TODO ERROR: Skipped ElseDirectiveTrivia
#Else
*/
/* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.Messaging.Requests
{
    public partial class RequestBase
    {

        #region Browser-Binding
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public class BrowserBindingExtensions
        {
            /// <summary>
         /// POST - calls
         /// </summary>
            internal string CmisAction { get; set; }
            /// <summary>
         /// GET - calls
         /// </summary>
            internal string CmisSelector { get; set; }
            /// <summary>
         /// If set to true properties will requested in succinct mode
         /// </summary>
            public bool Succinct { get; set; } = Client.Browser.SuccinctSupport.Current;
            /// <summary>
         /// Support for getLastRequest within browser binding
         /// </summary>
         /// <value></value>
         /// <returns></returns>
         /// <remarks>see http://docs.oasis-open.org/cmis/CMIS/v1.1/os/CMIS-v1.1-os.html, 5.2.9.2.2 Login and Tokens</remarks>
            public Client.Browser.TokenGenerator Token { get; set; } = Client.Browser.TokenGenerator.Current;
        }
        private BrowserBindingExtensions _BrowserBinding_retVal = new BrowserBindingExtensions();

        /// <summary>
      /// Returns extensions especially designed for browser binding
      /// </summary>
      /// <value></value>
      /// <returns></returns>
      /// <remarks></remarks>
        public BrowserBindingExtensions BrowserBinding
        {
            get
            {
                return _BrowserBinding_retVal;
            }
        }
        #endregion

    }
}