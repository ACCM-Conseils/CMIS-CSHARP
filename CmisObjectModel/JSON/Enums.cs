using System;
using Microsoft.VisualBasic;
/* TODO ERROR: Skipped EndIfDirectiveTrivia
#End If
*/
namespace CmisObjectModel.JSON
{
    [HideModuleName()]
    public static class Enums
    {
        public enum enumCollectionAction : int
        {
            add,
            remove
        }

        public enum enumJSONFile : int
        {
            cmisJS,
            embeddedFrameHtm,
            emptyPageHtm,
            loginPageHtm,
            loginRefPageHtm
        }

        [Flags()]
        public enum enumJSONPredefinedParameter : int
        {
            callback = minValue,
            cmisaction = callback << 1,
            cmisselector = cmisaction << 1,
            succinct = cmisselector << 1,
            suppressResponseCodes = succinct << 1,
            token = suppressResponseCodes << 1,

            minValue = 0x10000,
            maxValue = token
        }

        [Flags()]
        public enum enumRequestParameterSource : int
        {
            multipart = 1,
            queryString = 2
        }

        public enum enumValueType : int
        {
            changeToken, // only applies to bulkUpdateProperties
            objectId, // only applies to bulkUpdateProperties
            policy
        }
    }
}