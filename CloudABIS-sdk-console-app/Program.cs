
using cloudabis_scanr.APIManager;
using cloudabis_scanr.Models;
using cloudabis_sdk.APIManager;
using cloudabis_sdk.Models;
using cloudabis_sdk.Utilities;
using System;

namespace CloudABIS_sdk_console_app
{
    public class Program
    {
        private static CloudABISAPICredentials _cloudABISAPICredentials = null;
        private static CloudABISAPI _cloudABISAPI = null;
        private static CloudABISToken _cloudABISToken = null;
        private static EnumDeviceName _deviceName = EnumDeviceName.Secugen;
        private static CloudABISScanrCapture _scanrCapture = null;
        private static CloudABISScanrAPI _cloudScanrAPI = null;
        private static string _cloudScanrBaseAPI = "http://localhost:15896/";
        static void Main(string[] args)
        {
            Console.WriteLine("Initializing CloudABIS...");
            //Init CloudABIS credentails
            if (InitCloudABIS())
            {
                Console.WriteLine("Everything looks okay!.");
                //Init CloudScanr capture and start capture call
                InitCloudScanr();
                while (true)
                {


                    try
                    {
                        args = Console.ReadLine().Split(' ');
                        string operationName = args[0];
                        //Close operation
                        if (operationName.Equals("Exit")) break;

                        string id = args.Length > 1 ? args[1] : new Guid().ToString();
                        CloudABISScanrCaptureResponse cloudScanrCaptureResponse = _cloudScanrAPI.FingerPrintCapture(_scanrCapture);
                        if (cloudScanrCaptureResponse.CloudScanrStatus.Success)
                        {
                            Console.WriteLine("Capture success within " + cloudScanrCaptureResponse.CloudScanrStatus.ElapsedTimeInSeconds + " sec");

                            if (operationName.Equals("I"))
                            {
                                Identify(cloudScanrCaptureResponse);
                            }
                            else if (operationName.Equals("R"))
                            {
                                Register(cloudScanrCaptureResponse, id);
                            }
                            else if (operationName.Equals("IR"))
                            {
                                IsRegister(id);
                            }
                        }
                        else
                        {
                            Console.WriteLine(cloudScanrCaptureResponse.CloudScanrStatus.Message);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error_Occured: " + ex.Message);
                    }

                }
            }
            Console.WriteLine("Press any key to exit!");
            Console.ReadLine();
        }

        /// <summary>
        /// Is Registered
        /// </summary>
        /// <param name="id"></param>
        private static void IsRegister(string id)
        {
            CloudABISBiometricRequest biometricRequest = new CloudABISBiometricRequest
            {
                RegistrationID = id,
                CustomerKey = _cloudABISAPICredentials.CustomerKey,
                EngineName = CloudABISConstant.FINGERPRINT_ENGINE,
                Token = _cloudABISToken.AccessToken
            };
            //IsRegistered
            CloudABISResponse matchingResponse = _cloudABISAPI.IsRegistered(biometricRequest);
            if (matchingResponse != null)
            {
                if (matchingResponse.Status.Equals(EnumOperationStatus.SUCCESS))
                {
                    Console.WriteLine("IsRegistered: " + matchingResponse.OperationResult);
                }
                else
                {
                    Console.WriteLine("IsRegistered: " + matchingResponse.OperationResult);
                }

            }
            else Console.WriteLine("Something went wrong!");
        }

        private static void Register(CloudABISScanrCaptureResponse cloudScanrCaptureResponse, string id)
        {
            CloudABISBiometricRequest biometricRequest = new CloudABISBiometricRequest
            {
                RegistrationID = id,
                BiometricXml = cloudScanrCaptureResponse.TemplateData,
                CustomerKey = _cloudABISAPICredentials.CustomerKey,
                EngineName = CloudABISConstant.FINGERPRINT_ENGINE,
                Format = CloudABISConstant.CLOUDABIS_ISO,
                Token = _cloudABISToken.AccessToken
            };
            //Register Biometric
            CloudABISResponse matchingResponse = _cloudABISAPI.Register(biometricRequest);
            if (matchingResponse != null)
            {
                if (matchingResponse.Status.Equals(EnumOperationStatus.SUCCESS))
                {
                    if (matchingResponse.OperationResult.Equals(CloudABISConstant.MATCH_FOUND))
                    {
                        Console.WriteLine(CloudABISConstant.MATCH_FOUND + ":" + matchingResponse.BestResult.ID);
                    }
                    else Console.WriteLine("IdentifyResult:" + matchingResponse.OperationResult);
                }
                else
                {
                    Console.WriteLine("IdentifyResult: " + matchingResponse.OperationResult);
                }

            }
            else Console.WriteLine("Something went wrong!");
        }

        /// <summary>
        /// Identify biometric
        /// </summary>
        /// <param name="cloudScanrCaptureResponse"></param>
        private static void Identify(CloudABISScanrCaptureResponse cloudScanrCaptureResponse)
        {
            CloudABISBiometricRequest biometricRequest = new CloudABISBiometricRequest
            {
                BiometricXml = cloudScanrCaptureResponse.TemplateData,
                CustomerKey = _cloudABISAPICredentials.CustomerKey,
                EngineName = CloudABISConstant.FINGERPRINT_ENGINE,
                Format = CloudABISConstant.CLOUDABIS_ISO,
                Token = _cloudABISToken.AccessToken
            };
            //Identify Biometric
            CloudABISResponse matchingResponse = _cloudABISAPI.Identify(biometricRequest);
            if (matchingResponse != null)
            {
                if (matchingResponse.Status.Equals(EnumOperationStatus.SUCCESS))
                {
                    if (matchingResponse.OperationResult.Equals(CloudABISConstant.MATCH_FOUND))
                    {
                        Console.WriteLine(CloudABISConstant.MATCH_FOUND + ":" + matchingResponse.BestResult.ID);
                    }
                    else Console.WriteLine("IdentifyResult:" + matchingResponse.OperationResult);
                }
                else
                {
                    Console.WriteLine("IdentifyResult: " + matchingResponse.OperationResult);
                }

            }
            else Console.WriteLine("Something went wrong!");
        }

        private static void InitCloudScanr()
        {
            _cloudScanrAPI = new CloudABISScanrAPI(_cloudScanrBaseAPI);
            _scanrCapture = new CloudABISScanrCapture
            {
                DeviceName = _deviceName,
                CaptureTimeOut = 60.0,
                CaptureOperationName = EnumCaptureOperationName.IDENTIFY,
                QuickScan = EnumFeatureMode.Enable,
                TemplateFormat = EnumTemplateFormat.ISO

            };

            CloudABISScanrStatus status = _cloudScanrAPI.GetClientInfo();
            Console.WriteLine("Client Status: " + status.Message);
        }

        private static bool InitCloudABIS()
        {

            //It could be coming from database, app configuration file or a secured source
            _cloudABISAPICredentials = new CloudABISAPICredentials
            {
                BaseAPIURL = "https://demo-fp.cloudabis.com/v1/",
                AppKey = "059d8c941ced41a4b11769908f237b31",
                SecretKey = "eNssBOV0dBxGCJDQaZrrVkzbwns=",
                CustomerKey = "1B2E60DA116D47E9B31AEFB0E5663A04"
            };
            //Init CloudABIS API
            _cloudABISAPI = new CloudABISAPI(_cloudABISAPICredentials.AppKey, _cloudABISAPICredentials.SecretKey, _cloudABISAPICredentials.BaseAPIURL);
            _cloudABISToken = _cloudABISAPI.GetToken();
            if (!string.IsNullOrEmpty(_cloudABISToken.AccessToken)) return true;
            else
            {
                Console.WriteLine("CloudABIS token getting failed: " + _cloudABISToken.ErrorDescription);
                return false;
            }
        }
    }
}
