using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SourceCode.SmartObjects.Services.ServiceSDK;
using SourceCode.SmartObjects.Services.ServiceSDK.Objects;
using Attr = SourceCode.SmartObjects.Services.ServiceSDK.Attributes;
using SourceCode.SmartObjects.Services.ServiceSDK.Types;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace K2Translations
{
    [Attr.ServiceObject("K2Translations", "K2Translations", "Used for translating terms and eventually CSVs in K2")]
    public class K2Translations
    {
        // Service Object Properties
        private string culture;
        private string term;
        private string translation;

        [Attr.Property("Culture", SoType.Text, "Culture", "Culture to translate to, such as en-US")]
        public string Culture { get { return culture; } set { culture = value; } }

        [Attr.Property("Term", SoType.Text, "Term", "What to translate")]
        public string Term { get { return term; } set { term = value; } }

        [Attr.Property("Translation", SoType.Text, "Translation", "The translated term")]
        public string Translation { get { return translation; } set { translation = value; } }

        // Service Object Methods
        [Attr.Method("TranslateTerm", MethodType.Read, "TranslateTerm", "Translate a term", 
            new string[]{"Culture", "Term"}, 
            new string[] {"Culture", "Term" },
            new string[] {"Translation"})]
        public K2Translations TranslateTerm() 
        {
            K2Translations k2Translations = new K2Translations();
            k2Translations.translation = TranslateTerm(Term, Culture).Result;

            return k2Translations;

        }

        private async Task<string> TranslateTerm(string term, string culture)
        {
            var requestUrl = ServiceConfiguration["ServiceURL"] + $"/translate?api-version=3.0&to={culture}";
            var body = new object[] { new { Text = term } };
            var requestBody = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");

            var client = new HttpClient();
            var request = new HttpRequestMessage
               {
                Method = HttpMethod.Post,
                Content = requestBody,
                RequestUri = new Uri( requestUrl)
            };

            request.Headers.Add("Ocp-Apim-Subscription-Key", ServiceConfiguration["APIKey"].ToString());
            request.Headers.Add("Ocp-Apim-Subscription-Region", ServiceConfiguration["Region"].ToString());

            var response = await client.SendAsync(request);
            var responseBody = await response.Content.ReadAsStringAsync();
            var result =JArray.Parse(responseBody);

            return result[0]["translations"][0]["text"].ToString();
        }

        // Adding a service Config property, so we can access it as needed
        private ServiceConfiguration serviceConfiguration;

        public ServiceConfiguration ServiceConfiguration
        {
            get { return serviceConfiguration; }
            set { serviceConfiguration = value; }
        }
    }
}
