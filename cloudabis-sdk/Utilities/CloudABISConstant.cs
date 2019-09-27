using System;
using System.Collections.Generic;
using System.Text;

namespace cloudabis_sdk.Utilities
{
    /// <summary>
    /// CloudABIS Constant
    /// </summary>
    public class CloudABISConstant
    {
       
        #region CloudABIS response
        /// <summary>
        /// One match found
        /// </summary>
        public const string MATCH_FOUND = "MATCH_FOUND";
        /// <summary>
        /// No match found
        /// </summary>
        public const string NO_MATCH_FOUND = "NO_MATCH_FOUND";
        /// <summary>
        /// If operation was success
        /// </summary>
        public const string SUCCESS = "SUCCESS";
        /// <summary>
        /// If operation was failed
        /// </summary>
        public const string FAILED = "FAILED";
        /// <summary>
        /// Delete/Remove biometric success
        /// </summary>
        public const string DS = "DS";
        /// <summary>
        /// Delete/Remove biometric failed. Make it was not exist or else
        /// </summary>
        public const string DF = "DF";
        /// <summary>
        /// Verify success
        /// </summary>
        public const string VS = "VS";
        /// <summary>
        /// Verify failed
        /// </summary>
        public const string VF = "VF";
        /// <summary>
        /// Id not exist during IsRegistered, Verify, Delete/Remove biometric
        /// </summary>
        public const string ID_NOT_EXIST = "ID_NOT_EXIST";
        /// <summary>
        /// IsRegistered Yes
        /// </summary>
        public const string YES = "YES";
        /// <summary>
        /// ISRegistered No
        /// </summary>
        public const string NO = "NO";
        /// <summary>
        /// Given template is invalid
        /// </summary>
        public const string INVALID_TEMPLATE = "INVALID_TEMPLATE";
        /// <summary>
        /// Given ANSI template is invalid
        /// </summary>
        public const string INVALID_ANSI_TEMPLATE = "INVALID_ANSI_TEMPLATE";
        /// <summary>
        /// Given ISO template is invalid
        /// </summary>
        public const string INVALID_ISO_TEMPLATE = "INVALID_ISO_TEMPLATE";
        /// <summary>
        /// Given ICS template is invalid
        /// </summary>
        public const string INVALID_ICS_TEMPLATE = "INVALID_ICS_TEMPLATE";
        /// <summary>
        /// Given Customer info not found. May be customer key is incorrect
        /// </summary>
        public const string CUSTOMER_INFO_NOT_FOUND = "CUSTOMER_INFO_NOT_FOUND";
        /// <summary>
        /// Invalid engine
        /// </summary>
        public const string INVALID_ENGINE = "INVALID_ENGINE";
        /// <summary>
        /// Invalid request
        /// </summary>
        public const string INVALID_REQUEST = "INVALID_REQUEST";
        /// <summary>
        /// License error. This will be happened if license exceed or else
        /// </summary>
        public const string LICENSE_ERROR = "LICENSE_ERROR";
        /// <summary>
        /// Internal server error
        /// </summary>
        public const string INTERNAL_ERROR = "INTERNAL_ERROR";
        /// <summary>
        /// Cache not available
        /// </summary>
        public const string CACHE_NOT_AVAILABLE = "CACHE_NOT_AVAILABLE";
        /// <summary>
        /// Given image/tempalte was poor quality
        /// </summary>
        public const string POOR_IMAGE_QUALITY = "POOR_IMAGE_QUALITY";
        #endregion

        #region Validation
        /// <summary>
        /// Format can not be null or empty
        /// </summary>
        public const string FORMAT_CAN_NOT_BE_NULL_OR_EMPTY = "Format can not be null or empty";
        #endregion
        #region CloudABIS Template Name
        /// <summary>
        /// Standard ISO template format
        /// </summary>
        public const string CLOUDABIS_ISO = "ISO";
        /// <summary>
        /// Standard ICS template format
        /// </summary>
        public const string CLOUDABIS_ICS = "ICS";
        /// <summary>
        /// Standard ANSI template format
        /// </summary>
        public const string CLOUDABIS_ANSI = "ANSI";
        #endregion
        #region Engine Name
        /// <summary>
        /// FingerPrint engine name
        /// </summary>
        public const string FINGERPRINT_ENGINE = "FPFF02";
        /// <summary>
        /// Finger vein engine name
        /// </summary>
        public const string FINGERVEIN_ENGINE = "FVHT01";
        /// <summary>
        /// Face engine name
        /// </summary>
        public const string FACE_ENGINE = "FACE01";
        /// <summary>
        /// Iris engine name
        /// </summary>
        public const string IRIS_ENGINE = "IRIS01";

        #endregion

        #region CloudABIS API Path
        /// <summary>
        /// Is Registered API Path
        /// </summary>
        public const string CLOUDABIS_IS_REGISTERED_API_PATH = "api/Biometric/IsRegistered";
        /// <summary>
        /// Register API Path
        /// </summary>
        public const string CLOUDABIS_REGISTER_API_PATH = "api/Biometric/Register";
        /// <summary>
        /// Identify API Path
        /// </summary>
        public const string CLOUDABIS_IDENTIFY_API_PATH = "api/Biometric/Identify";
        /// <summary>
        /// Verify API Path
        /// </summary>
        public const string CLOUDABIS_VERIFY_API_PATH = "api/Biometric/Verify";

        /// <summary>
        /// Update API Path
        /// </summary>
        public const string CLOUDABIS_UPDATE_API_PATH = "api/Biometric/Update";
        /// <summary>
        /// ChangeID API Path
        /// </summary>
        public const string CLOUDABIS_CHANGEID_API_PATH = "api/Biometric/ChangeID";
        /// <summary>
        /// RemoveID API Path
        /// </summary>
        public const string CLOUDABIS_REMOVEID_API_PATH = "api/Biometric/RemoveID";

        #endregion
    }
}
