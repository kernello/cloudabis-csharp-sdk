using System;
using System.Collections.Generic;
using System.Text;

namespace cloudabis_sdk.Models
{
    /// <summary>
    /// Biometric operations
    /// </summary>
    public enum EnumOperationName
    {
        /// <summary>
        /// Not a valid operation.
        /// </summary>
        None = 0,
        /// <summary>
        /// Determine if a member ID already has biometric data enrolled.
        /// </summary>
        IsRegistered = 1,
        /// <summary>
        /// Enroll a member's biometric data.
        /// </summary>
        Register = 2,
        /// <summary>
        /// Identify a member through biometric match, by comparing against all enrolled biometric records.
        /// </summary>
        Identify = 3,
        /// <summary>
        /// Verify against one member's enrolled biometric data.
        /// </summary>
        Verify = 4,
        /// <summary>
        /// Update the enrolled biometric data of a member.
        /// </summary>
        Update = 5,
        /// <summary>
        /// Change the member ID associated with an existing enrollment to a new ID.
        /// </summary>
        ChangeID = 6,
        /// <summary>
        /// Delete an enrolled member ID and its associated biometric data.
        /// </summary>
        DeleteID = 7
    }

    /// <summary>
    /// Operational status.
    /// </summary>
    public enum EnumOperationStatus
    {
        /// <summary>
        /// No status
        /// </summary>
        NONE = 0,
        /// <summary>
        /// The operation was successfully executed.
        /// </summary>
        SUCCESS = 1,
        /// <summary>
        /// The submitted BiometricXml was not correctly formatted.
        /// </summary>
        INVALID_ACCESS = 2,
        /// <summary>
        /// A system license limitation prevented your request from being fulfilled. Please contact your vendor for assistance.
        /// </summary>
        LICENSE_EXCEED = 3,
        /// <summary>
        /// An unexpected system error was encountered. Please contact your vendor for assistance.
        /// </summary>
        ENGINE_EXCEPTION = 4,
        /// <summary>
        /// An unexpected API error was encountered. Please contact your vendor for assistance.
        /// </summary>
        CLOUD_API_ERROR = 5
    }
}
