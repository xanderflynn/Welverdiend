using Acr.UserDialogs;
//using CloudKit;
using Newtonsoft.Json;
using RestSharp;
using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Wel.Data;


namespace Wel.Services
{
    //Wel.Data.userObject user = null;
    public static class Client
    {
       
        static JsonSerializerOptions serializerOptions;
        static HttpClient clients;
       
        public static async Task<Wel.Data.userObject> Login(string UserName, string Password)
        {
           
        clients = new HttpClient();

            Uri uri = new Uri(string.Format("https://api-test.welverdiendforestry.co.za/api/v1/Authentication/SignIn", string.Empty));



            senduser uss = new senduser();
            Wel.Data.userObject user = null;



            try
            {


                uss.password = Password;
                uss.username = UserName;

                string json = System.Text.Json.JsonSerializer.Serialize<senduser>(uss, serializerOptions);
               

                string res = await RestBase.PostReq(uri, json);
                // string tet = await responses.Content.ReadAsStringAsync();

                // string response = await RestBase.Request("http://api-test.welverdiendforestry.co.za/api/v1/Authentication/SignInNew", parameters);
                user = JsonConvert.DeserializeObject<Wel.Data.userObject>(res);
                Wel.Data.user userObject = new Wel.Data.user();

                userObject = JsonConvert.DeserializeObject<Wel.Data.user>(user.value);

                string access = userObject.AccessToken;
                Preferences.Default.Set("AccessToken", access);



                // DataTable dt = (DataTable)JsonConvert.DeserializeObject(checklists, (typeof(DataTable)));
                return user;

            }
            catch (Exception ex)
            {
                return user;
            }
        }

        public static async Task<List<Wel.Data.DyanamicData>> GetChecklists()
        {
                //bool delete = await SQLFunctions.Deletequery("Delete from DynamicData");
                Wel.Data.dynamicDataRoot data = null;
                Wel.Data.DyanamicData successData = null;

                string checklists = "";
                Uri ur = new Uri("https://api-test.welverdiendforestry.co.za/api/v1/Management/GetQuestions");


                checklists = await RestBase.GetReq(ur, Preferences.Default.Get("AccessToken", "Unknown"));
                data = JsonConvert.DeserializeObject<Wel.Data.dynamicDataRoot>(checklists);
                var successDatasss = JsonConvert.DeserializeObject<List<Wel.Data.DyanamicData>>(data.value);
                return successDatasss;
            
        }

        public static async Task<senduser> SetChecklist()
        {
            Uri uri = new Uri(string.Format("https://api-test.welverdiendforestry.co.za/api/v1/management/addquestions", string.Empty));


            senduser send = new senduser();
            try
            {


                List<TblCompleted> existingData = await Wel.Services.SQLFunctions.GetAllCompletedQuestions();
                List<object> data = new List<object>();
                //List<DyanamicData> data = new List<DyanamicData>();

                //foreach(var checkedData in existingData)
                //{
                //    //List<DynamicDataUploadTable> idCheck = await Wel.Services.SQLFunctions.FindchecklistID(checkedData.TableName);
                //    data.Add(checkedData);

                //    data.Add(checkedData.Id);
                //    data.Add(checkedData.ColumnName);
                //    data.Add(checkedData.ColumnValue);
                //    data.Add(checkedData.Error);
                //    data.Add(checkedData.TableName);
                //    data.Add(checkedData.UserId);
                //    data.Add(checkedData.Value);
                //    data.Add(checkedData.Version);
                //}

                List<addquestionsRoot> addquestions = new List<addquestionsRoot>();


                foreach (TblCompleted item in existingData)
                {
                    if (item.ColumnName == "SignatureImageId")
                    {
                        addquestionsRoot add = new addquestionsRoot();
                        add.columnName = item.ColumnName;
                        add.recordId = item.recordId;
                        add.tableName = item.TableName;
                        add.columnValue= item.ColumnValue;
                        add.userId = item.UserId;
                        add.uuid = item.uuid;
                        add.signatures = await SQLFunctions.GetSignatures(item.uuid);
                        add.value = item.Value;
                        add.version = item.Version;
                        addquestions.Add(add);
                    }
                    else
                    {
                        addquestionsRoot add = new addquestionsRoot();
                        add.columnName = item.ColumnName;
                        add.columnValue = item.ColumnValue;
                        add.recordId = item.recordId;
                        add.tableName = item.TableName;
                        add.userId = item.UserId;
                        add.uuid = item.uuid;
                        add.signatures = new List<Signature>();
                        add.value = item.Value;
                        add.version = item.Version;
                        addquestions.Add(add);
                    }
                }


                using (var progress = UserDialogs.Instance.Loading("Sending Lists..."))
                {
                    await Task.Delay(50);

                    string json = System.Text.Json.JsonSerializer.Serialize<object>(addquestions, serializerOptions);

                    //string json = System.Text.Json.JsonSerializer.Serialize<object>(existingData, serializerOptions);

                    string ret = await RestBase.PostReq(uri, json, Preferences.Default.Get("AccessToken", "Unknown"));
                    await SQLFunctions.DeleteCompletedList();
                    return send;

                }
            }
            catch (Exception ex)
            {
                return send;
            }
        }

        



            public static async Task<string> GetToken()
        {

            var options = new RestClientOptions("http://api-test.welverdiendforestry.co.za/");

            using var client = new RestClient(options);
            //{
            //    Authenticator = new HttpBasicAuthenticator(_clientId, _clientSecret),
            //};

            var request = new RestRequest("api-test/v1/authentication/signin").AddJsonBody(new SignInRequest()
            {
                Password = "Corei5@2",
                Username = "Tiaan"
            });

            var response = await client.PostAsync<ObjectResponse>(request);

            var accessResponse = JsonConvert.DeserializeObject<SignInResponse>(response.Value);

            return accessResponse?.AccessToken;
        }
      
    public sealed class ObjectResponse
        {
            public ObjectResponse() { }

            public ObjectResponse(int statusCode, bool success, string value = null)
            {
                StatusCode = statusCode;
                Success = success;
                Value = value;
            }

            [Required]
            public int StatusCode { get; set; }

            [Required]
            public bool Success { get; set; }

            public string Value { get; set; }
        }

        public sealed class SignInResponse
        {
            public SignInResponse() { }

            public SignInResponse(string refreshToken, string token)
            {
                RefreshToken = refreshToken;
                AccessToken = token;
            }

            public SignInResponse(string refreshToken, string token, string firstName, string lastName, int userId, List<string> userRoles, bool subscribed)
            {
                RefreshToken = refreshToken;
                AccessToken = token;
                FirstName = firstName;
                LastName = lastName;
                UserId = userId;
                UserRoles = userRoles;
                Subscribed = subscribed;
            }

            public SignInResponse(string refreshToken, string token, string firstName, string lastName, int userId, List<string> userRoles, bool subscribed, int genderId)
            {
                RefreshToken = refreshToken;
                AccessToken = token;
                FirstName = firstName;
                LastName = lastName;
                UserId = userId;
                UserRoles = userRoles;
                Subscribed = subscribed;
                GenderId = genderId;
            }

            /// <summary>
            /// Unique access token
            /// </summary>
            public string AccessToken { get; set; }

            /// <summary>
            /// Unique refresh token
            /// </summary>
            public string RefreshToken { get; set; }

            /// <summary>
            /// First Name
            /// </summary>
            public string FirstName { get; set; }

            /// <summary>
            /// Last Name
            /// </summary>
            public string LastName { get; set; }

            /// <summary>
            /// Member Id (Me)
            /// </summary>
            public int UserId { get; set; }

            /// <summary>
            /// User Role Collection
            /// </summary>
            public List<string> UserRoles { get; set; }

            /// <summary>
            /// Subscription Flag
            /// </summary>
            public bool Subscribed { get; set; }

            /// <summary>
            /// Gender Id
            /// </summary>
            public int GenderId { get; set; }
        }


    }


}
