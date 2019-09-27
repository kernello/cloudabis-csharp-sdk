using System;
using System.Collections.Generic;
using System.Text;

namespace cloudabis_sdk.Models
{
    /// <summary>
    /// CloudABIS API Credentials
    /// </summary>
    public class CloudABISAPICredentials
    {
        public CloudABISAPICredentials()
        {
            BaseAPIURL = string.Empty;
            AppKey = string.Empty;
            SecretKey = string.Empty;
            CustomerKey = string.Empty;
        }
        /// <summary>
        /// CloudABIS Base API URL
        /// </summary>
        public string BaseAPIURL { get; set; }
        /// <summary>
        /// Customer-specific app key provided by the vendor.
        /// </summary>
        public string AppKey { get; set; }
        /// <summary>
        /// Customer-specific secret key provided by the vendor.
        /// </summary>
        public string SecretKey { get; set; }
        /// <summary>
        /// Customer-specific key provided by the vendor.
        /// </summary>
        public string CustomerKey { get; set; }
    }
}
