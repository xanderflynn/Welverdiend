using ModernHttpClient;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

namespace Wel.Services
{
    public class RestBase
    {
        public static async Task<string> Request(string URL, List<KeyValuePair<string, string>> Parameters, string accesstoken = "")
        {
            try
            {
                using (HttpClient client = new HttpClient(new NativeMessageHandler()))
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Wel.SharedLibrary.Const.REQUEST_HEADER_ACCEPT));
                    //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("multipart/form-data"));
                    if (!string.IsNullOrEmpty(accesstoken))
                    {
                        AuthenticationHeaderValue authenticationHeaderValue = new AuthenticationHeaderValue("Bearer", accesstoken);
                        client.DefaultRequestHeaders.Authorization = authenticationHeaderValue;
                    }
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, URL);
                    if (Parameters != null)
                        request.Content = new FormUrlEncodedContent(Parameters);

                    using (HttpResponseMessage response = await client.SendAsync(request))
                    {
                        if ((int)response.StatusCode == 200)
                        {
                            using (HttpContent content = response.Content)
                            {
                                string result = await content.ReadAsStringAsync();
                                if (result != null)
                                    return result;
                                else
                                    Wel.SharedLibrary.Runtime.LastErrorMessage = "Unable to communicate";
                            }
                        }
                        else
                        {
                            Wel.SharedLibrary.Runtime.LastErrorMessage = "Unable to communicate";
                        }
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                Wel.SharedLibrary.Runtime.LastErrorMessage = ex.Message;
                return "";
            }
        }
        static HttpClient clients;
        public static async Task<string> PostReq(Uri uri, string json, string accesstoken = "")
        {
            clients = new HttpClient();

            if (!string.IsNullOrEmpty(accesstoken))
            {
                AuthenticationHeaderValue authenticationHeaderValue = new AuthenticationHeaderValue("Bearer", accesstoken);
                clients.DefaultRequestHeaders.Authorization = authenticationHeaderValue;
            }
            
            string tet = "";
            try
            {
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage responses = await clients.PostAsync(uri, content);
                tet = await responses.Content.ReadAsStringAsync();
            }
            catch(Exception ex)
            {

            }

            return tet;
        }

        public static async Task<string> GetReq(Uri uri, string accesstoken = "")
        {
            clients = new HttpClient();

            //StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            if (!string.IsNullOrEmpty(accesstoken))
            {
                AuthenticationHeaderValue authenticationHeaderValue = new AuthenticationHeaderValue("Bearer", accesstoken);
                clients.DefaultRequestHeaders.Authorization = authenticationHeaderValue;
            }
            string tet = "";
            try
            {
                HttpResponseMessage responses = await clients.GetAsync(uri);

                tet = await responses.Content.ReadAsStringAsync();
            }
            catch(Exception ex)
            {

            }
           

            
            return tet;
        }

        public static async Task<string> PutReq(Uri uri, string json, string accesstoken = "")
        {
            clients = new HttpClient();

            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage responses = await clients.PutAsync(uri, content);

            string tet = await responses.Content.ReadAsStringAsync();

            if (!string.IsNullOrEmpty(accesstoken))
            {
                AuthenticationHeaderValue authenticationHeaderValue = new AuthenticationHeaderValue("Bearer", accesstoken);
                clients.DefaultRequestHeaders.Authorization = authenticationHeaderValue;
            }

            return tet;
        }

        public static async Task<string> DeleteReq(Uri uri, string json, string accesstoken = "")
        {
            clients = new HttpClient();

            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage responses = await clients.DeleteAsync(uri);

            string tet = await responses.Content.ReadAsStringAsync();

            if (!string.IsNullOrEmpty(accesstoken))
            {
                AuthenticationHeaderValue authenticationHeaderValue = new AuthenticationHeaderValue("Bearer", accesstoken);
                clients.DefaultRequestHeaders.Authorization = authenticationHeaderValue;
            }

            return tet;
        }

        public static async Task<string> Requests(string URL, List<KeyValuePair<string, string>> Parameters)
        {
            try
            {
                using (HttpClient client = new HttpClient(new NativeMessageHandler()))
                {
                    client.Timeout = TimeSpan.FromSeconds(45);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Wel.SharedLibrary.Const.REQUEST_HEADER_ACCEPT));

                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, URL);
                    if (Parameters != null)
                        request.Content = new FormUrlEncodedContent(Parameters);

                    using (HttpResponseMessage response = await client.SendAsync(request))
                    {
                        if ((int)response.StatusCode == 200)
                        {
                            using (HttpContent content = response.Content)
                            {
                                string result = await content.ReadAsStringAsync();
                                if (result != null)
                                    return result;
                                else
                                    Wel.SharedLibrary.Runtime.LastErrorMessage = "Unable to communicate";
                            }
                        }
                        else
                        {
                            Wel.SharedLibrary.Runtime.LastErrorMessage = "Unable to communicate";
                        }
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                Wel.SharedLibrary.Runtime.LastErrorMessage = ex.Message;
                return "";
            }
        }

        public static async Task<string> RequestMail(string URL, List<KeyValuePair<string, string>> Parameters)
        {
            try
            {
                using (HttpClient client = new HttpClient(new NativeMessageHandler()))
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Wel.SharedLibrary.Const.REQUEST_HEADER_ACCEPT));

                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, URL);
                    if (Parameters != null)
                        request.Content = new FormUrlEncodedContent(Parameters);

                    using (HttpResponseMessage response = await client.SendAsync(request))
                    {
                        if ((int)response.StatusCode == 200)
                        {
                            using (HttpContent content = response.Content)
                            {
                                string result = await content.ReadAsStringAsync();
                                if (result != null)
                                    return result;
                                else
                                    Wel.SharedLibrary.Runtime.LastErrorMessage = "Unable to communicate";
                            }
                        }
                        else
                        {
                            Wel.SharedLibrary.Runtime.LastErrorMessage = "Unable to communicate";
                        }
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                Wel.SharedLibrary.Runtime.LastErrorMessage = ex.Message;
                return "";
            }
        }



    }
}