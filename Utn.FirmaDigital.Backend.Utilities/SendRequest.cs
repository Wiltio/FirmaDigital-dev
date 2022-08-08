using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using RestSharp;

namespace Utn.FirmaDigital.Backend.Utilities
{
    public class SendRequest 
    {

        public static string InvokePost<T>(string url, string method, T entityParameter)
        {

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = client.PostAsJsonAsync(method, entityParameter).Result;

            if (response.IsSuccessStatusCode)
            {
                var jsonResult = response.Content.ReadAsStringAsync().Result;
                return jsonResult;
            }
            else
                throw new Exception(response.ReasonPhrase);
        }


        public static string InvokePost<T>(string url, string method, object entityParameter)
        {

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = client.PostAsJsonAsync(method, entityParameter).Result;

            if (response.IsSuccessStatusCode)
            {
                var jsonResult = response.Content.ReadAsStringAsync().Result;
                return jsonResult;
            }
            else
                throw new Exception(response.ReasonPhrase);
        }

        public static string InvokePost(string url, string method, string entityParameter)
        {

            var client = new RestClient(url);
            var request = new RestRequest("Post"); 
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("undefined", entityParameter, ParameterType.RequestBody);
            var response = client.Execute(request);
            if (response.IsSuccessful)
            {
                var jsonResult = response.Content;
                return jsonResult;
            }
            else{ 
                throw new Exception(response.ErrorMessage);
            }
        }



    }
}