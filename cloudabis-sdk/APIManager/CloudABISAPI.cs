using cloudabis_sdk.Models;
using cloudabis_sdk.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace cloudabis_sdk.APIManager
{
    /// <summary>
    /// API Manager
    /// </summary>
    public class CloudABISAPI
    {
        /// <summary>
        /// Given App Key
        /// </summary>
        private string _appkey = string.Empty;
        /// <summary>
        /// Given Secret Key
        /// </summary>
        private string _secretKey = string.Empty;
        /// <summary>
        /// Given Base API URL
        /// </summary>
        private string _apiBaseUrl = string.Empty;

        /// <summary>
        /// Construct API Manager
        /// <br>appKey: Customer-specific app key provided by the vendor.</br>
        /// <br>secretKey: Customer-specific secret key provided by the vendor.</br>
        /// <br>apiBaseUrl: Provided by the vendor.</br>
        /// </summary>
        /// <param name="appKey"></param>
        /// <param name="secretKey"></param>
        /// <param name="apiBaseUrl"></param>
        public CloudABISAPI(string appKey, string secretKey, string apiBaseUrl)
        {
            this._appkey = appKey;
            this._secretKey = secretKey;
            this._apiBaseUrl = apiBaseUrl;

            if (string.IsNullOrEmpty(this._apiBaseUrl))
                throw new Exception("Please provide the api base url.");

            if (!this._apiBaseUrl.EndsWith("/"))
                this._apiBaseUrl = this._apiBaseUrl + "/";
        }
        /// <summary>
        /// Returns API token object if given app key, secret key is correct otherwise return the proper reason
        /// </summary>
        /// <returns></returns>
        public CloudABISToken GetToken()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    if (string.IsNullOrEmpty(this._apiBaseUrl))
                        throw new Exception("Please provide the api base url.");

                    if (!this._apiBaseUrl.EndsWith("/"))
                        this._apiBaseUrl = this._apiBaseUrl + "/";

                    //setup client
                    client.BaseAddress = new Uri(this._apiBaseUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

                    //setup login data
                    var formContent = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("grant_type", "password"),
                        new KeyValuePair<string, string>("username", this._appkey),
                        new KeyValuePair<string, string>("password", this._secretKey),
                    });

                    //send request
                    HttpResponseMessage responseMessage = client.PostAsync("token", formContent).Result;

                    if (responseMessage.StatusCode.Equals(HttpStatusCode.OK))
                    {
                        //get access token from response body
                        var responseJson = responseMessage.Content.ReadAsStringAsync().Result;
                        return JsonConvert.DeserializeObject<CloudABISToken>(responseJson);
                    }
                    else
                    {
                        return new CloudABISToken { AccessToken = "", Error = responseMessage.StatusCode.ToString(), ErrorDescription = responseMessage.RequestMessage.ToString() };
                    }
                }
            }
            catch (Exception ex)
            {
                return new CloudABISToken { AccessToken = "", Error = ex.Message };
            }
        }

        /// <summary>
        /// Determine if a member ID already has biometric data enrolled.
        /// <para>Operation-specific OperationResult values:</para>
        /// <br>IsRegistered: YES - There is biometric data enrolled with the requested Member ID.</br>
        /// <br>IsRegistered: NO - There is not any biometric data enrolled with the requested ID.</br>
        /// <para>General OperationResult values:</para>
        /// <br>CUSTOMER_INFO_NOT_FOUND: The specified CustomerKey was not found in the system. Please contat your vendor for assistance.</br>
        /// <br>INVALID_ENGINE: The specified EngineName was not valid.</br>
        /// <br>INVALID_REQUEST: The submitted request was not correctly formatted.</br>
        /// <br>LICENSE_ERROR: A system license limitation prevented your request from being fulfilled. Please contact your vendor for assistance.</br>
        /// <br>INTERNAL_ERROR: An unexpected system error was encountered. Please contact your vendor for assistance.</br>
        /// <br>CACHE_NOT_AVAILABLE: The requested record is not available in the system. Please contact your vendor for assistance.</br>
        /// </summary>
        /// <param name="CustomerKey">Customer-specific key provided by the vendor.</param>
        /// <param name="EngineName"> The engine name for fingerprint biometrics is "FPFF02". The engine name for fingervein biometrics is "FVHT01"The engine name for face biometrics is "FACE01".The engine name for iris biometrics is "IRIS01".</param>
        /// <param name="RegistrationID">The unique identifier (Member ID) of the biometric enrollment that the requested operation will be performed on.</param>
        /// <param name="Token">API authenticate token</param>
        /// <returns></returns>
        public CloudABISResponse IsRegistered(CloudABISBiometricRequest request)
        {
            return Task.Run(() => DoBiometricOperation(CloudABISConstant.CLOUDABIS_IS_REGISTERED_API_PATH, request)).Result;
        }
        /// <summary>
        /// Enroll a member's biometric data.
        /// <para>Operation-specific OperationResult values:</para>
        /// <br>Register: SUCCESS - Enrollment successful. (The Member ID and associated biometric data added to system.)</br>
        /// <br>Register: FAILED - Enrollment failed.</br>
        /// <br>IsRegistered: YES - There is biometric data enrolled with the requested Member ID.</br>
        /// <br>Register: POOR_IMAGE_QUALITY - The submitted iris image(s) were not good enough quality to fulfill the request.</br>
        /// <br>Identify: MATCH_FOUND - Match found. (The submitted biometric data matched that of an enrolled member.)</br>
        /// <para>General OperationResult values(FingerVein,Face,Iris):</para>
        /// <br>INVALID_TEMPLATE: The submitted BiometricXml was not correctly formatted.</br>
        /// <br>CUSTOMER_INFO_NOT_FOUND: The specified CustomerKey was not found in the system.Please contat your vendor for assistance.</br>
        /// <br>INVALID_ENGINE: The specified EngineName was not valid.</br>
        /// <br>INVALID_REQUEST: The submitted request was not correctly formatted.</br>
        /// <br>LICENSE_ERROR: A system license limitation prevented your request from being fulfilled. Please contact your vendor for assistance.</br>
        /// <br>INTERNAL_ERROR: An unexpected system error was encountered. Please contact your vendor for assistance.</br>
        /// <br>CACHE_NOT_AVAILABLE: The requested record is not available in the system. Please contact your vendor for assistance.</br>
        /// <para>General OperationResult values(FingerPrint):</para>
        /// <br>INVALID_TEMPLATE: The submitted BiometricXml was not correctly formatted.</br>
        /// <br>INVALID_ANSI_TEMPLATE: The submitted template in BiometricXml was not valid ANSI template.</br>
        /// <br>INVALID_ISO_TEMPLATE: The submitted template in BiometricXml was not valid ISO template.</br>
        /// <br>INVALID_ICS_TEMPLATE: The submitted template in BiometricXml was not valid ICS template.</br>
        /// <br>CUSTOMER_INFO_NOT_FOUND: The specified CustomerKey was not found in the system. Please contat your vendor for assistance.</br>
        /// <br>INVALID_ENGINE: The specified EngineName was not valid.</br>
        /// <br>INVALID_REQUEST: The submitted request was not correctly formatted.</br>
        /// <br>LICENSE_ERROR: A system license limitation prevented your request from being fulfilled. Please contact your vendor for assistance.</br>
        /// <br>INTERNAL_ERROR: An unexpected system error was encountered. Please contact your vendor for assistance.</br>
        /// <br>CACHE_NOT_AVAILABLE: The requested record is not available in the system. Please contact your vendor for assistance.</br>
        /// </summary>
        /// <param name="CustomerKey">Customer-specific key provided by the vendor.</param>
        /// <param name="EngineName"> The engine name for fingerprint biometrics is "FPFF02". The engine name for fingervein biometrics is "FVHT01"The engine name for face biometrics is "FACE01".The engine name for iris biometrics is "IRIS01".</param>
        /// <param name="RegistrationID">The unique identifier (Member ID) of the biometric enrollment that the requested operation will be performed on.</param>
        /// <param name="Format">The format of template. It might be ISO/ANSI/ICS. This parameter is need during passing the template.Required only FingerPrint engine</param>
        /// <param name="BiometricXml">The biometric template with xml formatting</param>
        /// <param name="Token">API authenticate token</param>
        /// <returns></returns>
        public CloudABISResponse Register(CloudABISBiometricRequest request)
        {
            return Task.Run(() => DoBiometricOperation(CloudABISConstant.CLOUDABIS_REGISTER_API_PATH, request)).Result;
        }
        /// <summary>
        /// Identify a member through biometric match, by comparing against all enrolled biometric records.
        /// <para>Operation-specific OperationResult values:</para>
        /// <br>Identify: MATCH_FOUND - Match found. (The submitted biometric data matched that of an enrolled member.)</br>
        /// <br>Identify: NO_MATCH_FOUND - No match found. (No enrolled members matched against the submitted biometric data.)</br>
        /// <br>Identify: POOR_IMAGE_QUALITY - The submitted face image(s) were not good enough quality to fulfill the request.</br>
        /// <para>General OperationResult values(FinverVein, Face, Iris):</para>
        /// <br>INVALID_TEMPLATE: The submitted BiometricXml was not correctly formatted.</br>
        /// <br>CUSTOMER_INFO_NOT_FOUND: The specified CustomerKey was not found in the system.Please contat your vendor for assistance.</br>
        /// <br>INVALID_ENGINE: The specified EngineName was not valid.</br>
        /// <br>INVALID_REQUEST: The submitted request was not correctly formatted.</br>
        /// <br>LICENSE_ERROR: A system license limitation prevented your request from being fulfilled. Please contact your vendor for assistance.</br>
        /// <br>INTERNAL_ERROR: An unexpected system error was encountered. Please contact your vendor for assistance.</br>
        /// <br>CACHE_NOT_AVAILABLE: The requested record is not available in the system. Please contact your vendor for assistance.</br>
        /// <para>General OperationResult values(FingerPrint):</para>
        /// <br>INVALID_TEMPLATE: The submitted BiometricXml was not correctly formatted.</br>
        /// <br>INVALID_ANSI_TEMPLATE: The submitted template in BiometricXml was not valid ANSI template.</br>
        /// <br>INVALID_ISO_TEMPLATE: The submitted template in BiometricXml was not valid ISO template.</br>
        /// <br>INVALID_ICS_TEMPLATE: The submitted template in BiometricXml was not valid ICS template.</br>
        /// <br>CUSTOMER_INFO_NOT_FOUND: The specified CustomerKey was not found in the system. Please contat your vendor for assistance.</br>
        /// <br>INVALID_ENGINE: The specified EngineName was not valid.</br>
        /// <br>INVALID_REQUEST: The submitted request was not correctly formatted.</br>
        /// <br>LICENSE_ERROR: A system license limitation prevented your request from being fulfilled. Please contact your vendor for assistance.</br>
        /// <br>INTERNAL_ERROR: An unexpected system error was encountered. Please contact your vendor for assistance.</br>
        /// <br>CACHE_NOT_AVAILABLE: The requested record is not available in the system. Please contact your vendor for assistance.</br>
        /// </summary>
        /// <param name="CustomerKey">Customer-specific key provided by the vendor.</param>
        /// <param name="EngineName"> The engine name for fingerprint biometrics is "FPFF02". The engine name for fingervein biometrics is "FVHT01"The engine name for face biometrics is "FACE01".The engine name for iris biometrics is "IRIS01".</param>
        /// <param name="RegistrationID">The unique identifier (Member ID) of the biometric enrollment that the requested operation will be performed on.</param>
        /// <param name="Format">The format of template. It might be ISO/ANSI/ICS. This parameter is need during passing the template.Required only FingerPrint engine</param>
        /// <param name="BiometricXml">The biometric template with xml formatting</param>
        /// <param name="Token">API authenticate token</param>
        /// <returns></returns>
        public CloudABISResponse Identify(CloudABISBiometricRequest request)
        {
            return Task.Run(() => DoBiometricOperation(CloudABISConstant.CLOUDABIS_IDENTIFY_API_PATH, request)).Result;
        }
        /// <summary>
        /// Verify against one member's enrolled biometric data.
        /// <para>Operation-specific OperationResult values:</para>
        /// <br>Verify: VS - Verification successful. (The submitted biometric data matched the requested member's enrolled biometric data.)</br>
        /// <br>Verify: VF - Verification failed. (The submitted biometric data did not match the requested member's enrolled biometric data.)</br>
        /// <br>Verify: ID_NOT_EXIST - The Member ID doesn't exist in the system.</br>
        /// <br>Verify: POOR_IMAGE_QUALITY - The submitted fingerprint image(s) were not good enough quality to fulfill the request.</br>
        /// <para>General OperationResult values(FinverVein, Face, Iris):</para>
        /// <br>INVALID_TEMPLATE: The submitted BiometricXml was not correctly formatted.</br>
        /// <br>CUSTOMER_INFO_NOT_FOUND: The specified CustomerKey was not found in the system.Please contat your vendor for assistance.</br>
        /// <br>INVALID_ENGINE: The specified EngineName was not valid.</br>
        /// <br>INVALID_REQUEST: The submitted request was not correctly formatted.</br>
        /// <br>LICENSE_ERROR: A system license limitation prevented your request from being fulfilled. Please contact your vendor for assistance.</br>
        /// <br>INTERNAL_ERROR: An unexpected system error was encountered. Please contact your vendor for assistance.</br>
        /// <br>CACHE_NOT_AVAILABLE: The requested record is not available in the system. Please contact your vendor for assistance.</br>
        /// <para>General OperationResult values(FingerPrint):</para>
        /// <br>INVALID_TEMPLATE: The submitted BiometricXml was not correctly formatted.</br>
        /// <br>INVALID_ANSI_TEMPLATE: The submitted template in BiometricXml was not valid ANSI template.</br>
        /// <br>INVALID_ISO_TEMPLATE: The submitted template in BiometricXml was not valid ISO template.</br>
        /// <br>INVALID_ICS_TEMPLATE: The submitted template in BiometricXml was not valid ICS template.</br>
        /// <br>CUSTOMER_INFO_NOT_FOUND: The specified CustomerKey was not found in the system. Please contat your vendor for assistance.</br>
        /// <br>INVALID_ENGINE: The specified EngineName was not valid.</br>
        /// <br>INVALID_REQUEST: The submitted request was not correctly formatted.</br>
        /// <br>LICENSE_ERROR: A system license limitation prevented your request from being fulfilled. Please contact your vendor for assistance.</br>
        /// <br>INTERNAL_ERROR: An unexpected system error was encountered. Please contact your vendor for assistance.</br>
        /// <br>CACHE_NOT_AVAILABLE: The requested record is not available in the system. Please contact your vendor for assistance.</br>
        /// </summary>
        /// <param name="CustomerKey">Customer-specific key provided by the vendor.</param>
        /// <param name="EngineName"> The engine name for fingerprint biometrics is "FPFF02". The engine name for fingervein biometrics is "FVHT01"The engine name for face biometrics is "FACE01".The engine name for iris biometrics is "IRIS01".</param>
        /// <param name="RegistrationID">The unique identifier (Member ID) of the biometric enrollment that the requested operation will be performed on.</param>
        /// <param name="Format">The format of template. It might be ISO/ANSI/ICS. This parameter is need during passing the template.Required only FingerPrint engine</param>
        /// <param name="BiometricXml">The biometric template with xml formatting</param>
        /// <param name="Token">API authenticate token</param>
        /// <returns></returns>
        public CloudABISResponse Verify(CloudABISBiometricRequest request)
        {
            return Task.Run(() => DoBiometricOperation(CloudABISConstant.CLOUDABIS_VERIFY_API_PATH, request)).Result;
        }
        /// <summary>
        /// Update the enrolled biometric data of a member.
        /// <para>Operation-specific OperationResult values:</para>
        /// <br>Update: SUCCESS - Update successful. (The biometric data associated with requested Member ID was updated in system.)</br>
        /// <br>Update: FAILED - Update Failed.</br>
        /// <br>Update: ID_NOT_EXIST - The Member ID doesn't exist in the system.</br>
        /// <br>Update: POOR_IMAGE_QUALITY - The submitted iris image(s) were not good enough quality to fulfill the request.</br>
        /// <br>Identify: MATCH_FOUND - Match found. (The submitted biometric data matched that of an enrolled member.)</br>
        /// <para>General OperationResult values(FingerVein,Face,Iris):</para>
        /// <br>INVALID_TEMPLATE: The submitted BiometricXml was not correctly formatted.</br>
        /// <br>CUSTOMER_INFO_NOT_FOUND: The specified CustomerKey was not found in the system.Please contat your vendor for assistance.</br>
        /// <br>INVALID_ENGINE: The specified EngineName was not valid.</br>
        /// <br>INVALID_REQUEST: The submitted request was not correctly formatted.</br>
        /// <br>LICENSE_ERROR: A system license limitation prevented your request from being fulfilled. Please contact your vendor for assistance.</br>
        /// <br>INTERNAL_ERROR: An unexpected system error was encountered. Please contact your vendor for assistance.</br>
        /// <br>CACHE_NOT_AVAILABLE: The requested record is not available in the system. Please contact your vendor for assistance.</br>
        /// <para>General OperationResult values(FingerPrint):</para>
        /// <br>INVALID_TEMPLATE: The submitted BiometricXml was not correctly formatted.</br>
        /// <br>INVALID_ANSI_TEMPLATE: The submitted template in BiometricXml was not valid ANSI template.</br>
        /// <br>INVALID_ISO_TEMPLATE: The submitted template in BiometricXml was not valid ISO template.</br>
        /// <br>INVALID_ICS_TEMPLATE: The submitted template in BiometricXml was not valid ICS template.</br>
        /// <br>CUSTOMER_INFO_NOT_FOUND: The specified CustomerKey was not found in the system. Please contat your vendor for assistance.</br>
        /// <br>INVALID_ENGINE: The specified EngineName was not valid.</br>
        /// <br>INVALID_REQUEST: The submitted request was not correctly formatted.</br>
        /// <br>LICENSE_ERROR: A system license limitation prevented your request from being fulfilled. Please contact your vendor for assistance.</br>
        /// <br>INTERNAL_ERROR: An unexpected system error was encountered. Please contact your vendor for assistance.</br>
        /// <br>CACHE_NOT_AVAILABLE: The requested record is not available in the system. Please contact your vendor for assistance.</br>
        /// </summary>
        /// <param name="CustomerKey">Customer-specific key provided by the vendor.</param>
        /// <param name="EngineName"> The engine name for fingerprint biometrics is "FPFF02". The engine name for fingervein biometrics is "FVHT01"The engine name for face biometrics is "FACE01".The engine name for iris biometrics is "IRIS01".</param>
        /// <param name="RegistrationID">The unique identifier (Member ID) of the biometric enrollment that the requested operation will be performed on.</param>
        /// <param name="Format">The format of template. It might be ISO/ANSI/ICS. This parameter is need during passing the template.Required only FingerPrint engine</param>
        /// <param name="BiometricXml">The biometric template with xml formatting</param>
        /// <param name="Token">API authenticate token</param>
        /// <returns></returns>
        public CloudABISResponse Update(CloudABISBiometricRequest request)
        {
            return Task.Run(() => DoBiometricOperation(CloudABISConstant.CLOUDABIS_UPDATE_API_PATH, request)).Result;
        }
        /// <summary>
        /// Change the member ID associated with an existing enrollment to a new ID.
        /// <para>Operation-specific OperationResult values:</para>
        /// <br>ChangeID: CS - Change of ID successful. (The Member ID was changed to the specified new ID.)</br>
        /// <br>ChangeID: CF - Change of ID failed.</br>
        /// <br>ChangeID: ID_NOT_EXIST - The Member ID intent for change doesn't exist in the system.</br>
        /// <br>IsRegistered: YES - There is biometric data enrolled with the requested New Member ID.</br>
        /// <para>General OperationResult values:</para>
        /// <br>CUSTOMER_INFO_NOT_FOUND: The specified CustomerKey was not found in the system. Please contat your vendor for assistance.</br>
        /// <br>INVALID_ENGINE: The specified EngineName was not valid.</br>
        /// <br>INVALID_REQUEST: The submitted request was not correctly formatted.</br>
        /// <br>LICENSE_ERROR: A system license limitation prevented your request from being fulfilled. Please contact your vendor for assistance.</br>
        /// <br>INTERNAL_ERROR: An unexpected system error was encountered. Please contact your vendor for assistance.</br>
        /// <br>CACHE_NOT_AVAILABLE: The requested record is not available in the system. Please contact your vendor for assistance.</br>
        /// </summary>
        /// <param name="CustomerKey">Customer-specific key provided by the vendor.</param>
        /// <param name="EngineName"> The engine name for fingerprint biometrics is "FPFF02". The engine name for fingervein biometrics is "FVHT01"The engine name for face biometrics is "FACE01".The engine name for iris biometrics is "IRIS01".</param>
        /// <param name="RegistrationID">The unique identifier (Member ID) of the biometric enrollment that the requested operation will be performed on.</param>
        /// <param name="NewRegistrationID">The new unique identifier (Member ID) that the existing ID will be changed to.</param>
        /// <param name="Token">API authenticate token</param>
        /// <returns></returns>
        public CloudABISResponse ChangeID(CloudABISBiometricRequest request)
        {
            return Task.Run(() => DoBiometricOperation(CloudABISConstant.CLOUDABIS_CHANGEID_API_PATH, request)).Result;
        }

        /// <summary>
        /// Delete an enrolled member ID and its associated biometric data.
        /// <para>Operation-specific OperationResult values:</para>
        /// <br>DeleteID: DS - Deletion successful. (The Member ID and associated biometric data removed from system.)</br>
        /// <br>DeleteID: DF - Deletion failed.</br>
        /// <br>DeleteID: ID_NOT_EXIST - The Member ID doesn't exist in the system.</br>
        /// <para>General OperationResult values:</para>
        /// <br>CUSTOMER_INFO_NOT_FOUND: The specified CustomerKey was not found in the system. Please contat your vendor for assistance.</br>
        /// <br>INVALID_ENGINE: The specified EngineName was not valid.</br>
        /// <br>INVALID_REQUEST: The submitted request was not correctly formatted.</br>
        /// <br>LICENSE_ERROR: A system license limitation prevented your request from being fulfilled. Please contact your vendor for assistance.</br>
        /// <br>INTERNAL_ERROR: An unexpected system error was encountered. Please contact your vendor for assistance.</br>
        /// <br>CACHE_NOT_AVAILABLE: The requested record is not available in the system. Please contact your vendor for assistance.</br>
        /// </summary>
        /// <param name="CustomerKey">Customer-specific key provided by the vendor.</param>
        /// <param name="EngineName"> The engine name for fingerprint biometrics is "FPFF02". The engine name for fingervein biometrics is "FVHT01"The engine name for face biometrics is "FACE01".The engine name for iris biometrics is "IRIS01".</param>
        /// <param name="RegistrationID">The unique identifier (Member ID) of the biometric enrollment that the requested operation will be performed on.</param>
        /// <param name="Token">API authenticate token</param>
        /// <returns></returns>
        public CloudABISResponse RemoveID(CloudABISBiometricRequest request)
        {
            return Task.Run(() => DoBiometricOperation(CloudABISConstant.CLOUDABIS_REMOVEID_API_PATH, request)).Result;
        }
        /// <summary>
        /// Return Biometric request result
        /// </summary>
        /// <param name="requestPath"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        private async Task<CloudABISResponse> DoBiometricOperation(string requestPath, CloudABISBiometricRequest request)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    //setup client
                    client.BaseAddress = new Uri(this._apiBaseUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", request.Token);

                    //make request
                    HttpResponseMessage response = await client.PostAsJsonAsync(requestPath, request);
                    if (response.StatusCode.Equals(HttpStatusCode.OK))
                    {
                        return response.Content.ReadAsAsync<CloudABISResponse>().Result;
                    }
                    else
                    {
                        return new CloudABISResponse { Status = EnumOperationStatus.NONE, OperationResult = response.StatusCode.ToString() };
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
