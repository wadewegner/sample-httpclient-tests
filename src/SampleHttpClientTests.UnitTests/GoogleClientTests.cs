using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using SampleHttpClientTests.Services;

namespace SampleHttpClientTests.UnitTests
{
    public class GoogleClientTests
    {
        [Test]
        public async Task CheckOkResponseHandling()
        {
            const string expectedResponse = "response";

            var mockHttpMessageHandler = new MockHttpMessageHandler
            {
                Response = { 
                    StatusCode = HttpStatusCode.OK, 
                    Content = new StringContent(expectedResponse) 
                }
            };

            var googleClient = new GoogleClient(new HttpClient(mockHttpMessageHandler));
            var uri = new Uri("http://unittest");
            var response = await googleClient.GetStringFromUrl(uri);

            Assert.IsNotNullOrEmpty(response);
            Assert.AreEqual(expectedResponse, response);
        }

        [Test]
        [ExpectedException(typeof(HttpRequestException))]
        public async Task CheckNotFoundResponseHandling()
        {
            var mockHttpMessageHandler = new MockHttpMessageHandler
            {
                Response = { StatusCode = HttpStatusCode.NotFound }
            };

            var googleClient = new GoogleClient(new HttpClient(mockHttpMessageHandler));
            var uri = new Uri("http://unittest");
            
            try
            {
                await googleClient.GetStringFromUrl(uri);
            }
            catch (HttpRequestException httpRequestException)
            {
                Assert.AreEqual(httpRequestException.Message, "Response status code does not indicate success: 404 (Not Found).");

                throw;
            }
        }

        [Test]
        public async Task CheckAgainstGoogleUrl()
        {
            var googleClient = new GoogleClient();
            var uri = new Uri("http://www.google.com/");
            var response = await googleClient.GetStringFromUrl(uri);

            Assert.IsNotNullOrEmpty(response);
            StringAssert.Contains("<html", response.ToLower());
            StringAssert.Contains("google search", response.ToLower());
            StringAssert.Contains("i'm feeling lucky", response.ToLower());
        }
    }
}
