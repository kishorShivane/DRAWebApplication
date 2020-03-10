﻿using DRAWeb.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DRAWeb.Proxy
{
    public class DRAAzureServiceProxy
    {
        public static void SerializeJsonIntoStream(object value, Stream stream)
        {
            using (var sw = new StreamWriter(stream, new UTF8Encoding(false), 1024, true))
            using (var jtw = new JsonTextWriter(sw) { Formatting = Formatting.None })
            {
                var js = new JsonSerializer();
                js.Serialize(jtw, value);
                jtw.Flush();
            }
        }

        private static HttpContent CreateHttpContent(object content)
        {
            HttpContent httpContent = null;

            if (content != null)
            {
                var ms = new MemoryStream();
                SerializeJsonIntoStream(content, ms);
                ms.Seek(0, SeekOrigin.Begin);
                httpContent = new StreamContent(ms);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }

            return httpContent;
        }

        public async Task<ResponseMessage<List<UserCompetencyMatrixModel>>> GetUserCompetencyMetrix(CompetenciesReportRequest reportRequest)
        {
            ResponseMessage<List<UserCompetencyMatrixModel>> result;
            string azureBaseUrl = "https://draazurefunctionstest.azurewebsites.net/api/";
            string urlQueryStringParams = "DRAReports?code=5RQwdfaVrhbptEjWzn5ciSGP8GjdmKiOjwh9B2ccpm4kbkbwaCmK5g==";

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage(HttpMethod.Post, $"{azureBaseUrl}{urlQueryStringParams}"))
            using (var httpContent = CreateHttpContent(reportRequest))
            {
                request.Content = httpContent;

                using (var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        result = JsonConvert.DeserializeObject<ResponseMessage<List<UserCompetencyMatrixModel>>>(jsonString);
                    }
                    else
                    {
                        result = new ResponseMessage<List<UserCompetencyMatrixModel>>();
                        result = JsonConvert.DeserializeObject<ResponseMessage<List<UserCompetencyMatrixModel>>>(jsonString);
                    }
                }
            }
            return result;
        }

        public async Task<ResponseMessage<UserModel>> ValidateUserCredentials(UserModel user)
        {
            ResponseMessage<UserModel> result;
            string azureBaseUrl = "https://draazurefunctionstest.azurewebsites.net/api/";
            string urlQueryStringParams = "DRALogin?code=jb3ql/HRTcaJaeNK2RfozNwHCdG3g6WZnEeTU0WIBb47QRwLBDCuSA==";

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage(HttpMethod.Post, $"{azureBaseUrl}{urlQueryStringParams}"))
            using (var httpContent = CreateHttpContent(user))
            {
                request.Content = httpContent;

                using (var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        result = JsonConvert.DeserializeObject<ResponseMessage<UserModel>>(jsonString);
                    }
                    else
                    {
                        result = new ResponseMessage<UserModel>();
                        result = JsonConvert.DeserializeObject<ResponseMessage<UserModel>>(jsonString);
                    }
                }
            }
            return result;
        }

        public async Task<ResponseMessage<UserModel>> RegisterUser(UserModel user)
        {
            ResponseMessage<UserModel> result;
            string azureBaseUrl = "https://draazurefunctionstest.azurewebsites.net/api/";
            string urlQueryStringParams = "DRARegister?code=BTwW5nP9aKKRuISh0ahWGaiSDmEOisjec4Crxawm7arVaTQ7tCGVTw==";

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage(HttpMethod.Post, $"{azureBaseUrl}{urlQueryStringParams}"))
            using (var httpContent = CreateHttpContent(user))
            {
                request.Content = httpContent;

                using (var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode == HttpStatusCode.Created)
                    {
                        result = JsonConvert.DeserializeObject<ResponseMessage<UserModel>>(jsonString);
                    }
                    else
                    {
                        result = new ResponseMessage<UserModel>();
                        result = JsonConvert.DeserializeObject<ResponseMessage<UserModel>>(jsonString);
                    }
                }
            }
            return result;
        }

        public async Task<string> UpdatePassword(ResetPassword user)
        {
            string result;
            string azureBaseUrl = "https://draazurefunctionstest.azurewebsites.net/api/";
            string urlQueryStringParams = "DRAResetPassword?code=epixFLZiUhG4EukalBGhA6FIqRCkTQhfzhas40UaWG0evhpyCtakqQ==";

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage(HttpMethod.Post, $"{azureBaseUrl}{urlQueryStringParams}"))
            using (var httpContent = CreateHttpContent(user))
            {
                request.Content = httpContent;

                using (var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        result = "Updated";
                    }
                    else
                    {;
                        result = JsonConvert.DeserializeObject<string>(jsonString);
                    }
                }
            }
            return result;
        }

        public async Task<string> ActivateUser(int userID)
        {
            string result;
            string azureBaseUrl = "https://draazurefunctionstest.azurewebsites.net/api/";
            string urlQueryStringParams = "DRAActivateAccount?code=YrC7qctFNNaVYdz4pAtqLfJDQXidqsdvCUwH7HWMWjlMwD6O1rLJWw==";

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage(HttpMethod.Post, $"{azureBaseUrl}{urlQueryStringParams}"))
            using (var httpContent = CreateHttpContent(userID))
            {
                request.Content = httpContent;

                using (var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        result = "Updated";
                    }
                    else
                    {
                        ;
                        result = JsonConvert.DeserializeObject<string>(jsonString);
                    }
                }
            }
            return result;
        }
    }
}
