using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace WebApp.Tools
{
    public class APIRequest
    {
        static async Task<string> Get(Uri u)
        {
            var response = string.Empty;
            using (var client = new HttpClient())
            {
                HttpResponseMessage result = await client.GetAsync(u);
                if (result.IsSuccessStatusCode)
                {
                    response = await result.Content.ReadAsStringAsync();
                }
            }
            return response;
        }


        public static async Task<string> Post(Uri u, HttpContent c)
        {
            var response = string.Empty;
            using (var client = new HttpClient())
            {
                HttpResponseMessage result = await client.PostAsync(u, c);
                if (result.IsSuccessStatusCode)
                {
                    response = result.StatusCode.ToString();
                }
            }
            return response;
        }

        public static async Task<string> Delete(Uri u, int id)
        {
            var response = string.Empty;
            using (var client = new HttpClient())
            {
                HttpResponseMessage result = await client.DeleteAsync(u + "/" + id);
                if (result.IsSuccessStatusCode)
                {
                    response = result.StatusCode.ToString();
                }
            }
            return response;
        }

        public static async Task<string> Put(Uri u, HttpContent c) {
            var response = string.Empty;
            using (var client = new HttpClient()) {
                HttpResponseMessage result = await client.PutAsync(u, c);
                if (result.IsSuccessStatusCode) {
                    response = result.StatusCode.ToString();
                }
            }
            return response;
        }
    }
}