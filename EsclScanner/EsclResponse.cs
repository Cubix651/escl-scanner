using System;
using System.Net.Http;

namespace EsclScanner
{
    public class EsclResponse : IEsclResponse
    {
        HttpResponseMessage response;
        string content = null;

        public EsclResponse(HttpResponseMessage response)
        {
            this.response = response;
        }
        public bool IsSuccessStatusCode => response.IsSuccessStatusCode;

        public string Content
        {
            get
            {
                if (content == null)
                    content = response.Content.ReadAsStringAsync().Result;
                return content;
            }
        }

        public Uri LocationHeader => response.Headers.Location;
        
    }
}