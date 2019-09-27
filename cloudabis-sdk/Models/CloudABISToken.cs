using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace cloudabis_sdk.Models
{
    /// <summary>
    /// CloudABIS Token
    /// </summary>
    public class CloudABISToken
    {
        /// <summary>
        /// Contains access token
        /// </summary>
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        /// <summary>
        /// Contains token expiry time in seconds
        /// </summary>
        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }
        /// <summary>
        /// Conatins token type
        /// </summary>
        [JsonProperty("token_type")]
        public string TokenType { get; set; }
        /// <summary>
        /// Contains token error message if error occured otherwise this value should be an empty
        /// </summary>
        [JsonProperty("error")]
        public string Error { get; set; }
        /// <summary>
        /// Details description of the token error if error occured otherwise this value should be an empty
        /// </summary>
        [JsonProperty("error_description")]
        public string ErrorDescription { get; set; }
    }
}
