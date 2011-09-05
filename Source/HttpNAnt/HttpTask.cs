using System;
using System.Collections.Generic;
using System.Net;

using Microsoft.Http;

using NAnt.Core;
using NAnt.Core.Attributes;

namespace AlexMG.NAntTasks
{
    [TaskName("http")]
    public class HttpTask : Task
    {
    	private static readonly List<HttpStatusCode> successCodes = new List<HttpStatusCode>
    	{
    		HttpStatusCode.OK,
    		HttpStatusCode.Created,
    		HttpStatusCode.Accepted,
    		HttpStatusCode.NonAuthoritativeInformation,
    		HttpStatusCode.NoContent,
    		HttpStatusCode.ResetContent,
    		HttpStatusCode.PartialContent
    	};

        [TaskAttribute("url", Required = true)]
        [StringValidator(AllowEmpty = false)]
        public string Url { get; set; }

        [TaskAttribute("method", Required = false)]
        [StringValidator(AllowEmpty = true)]
        public string Method { get; set; }

        [TaskAttribute("content", Required = false)]
        [StringValidator(AllowEmpty = true)]
        public string Content { get; set; }

        [TaskAttribute("contenttype", Required = false)]
        [StringValidator(AllowEmpty = true)]
        public string ContentType { get; set; }

		[TaskAttribute("connectiontimeout", Required = false)]
		public int ConnectionTimeout { get; set; }

        [TaskAttribute("responseproperty", Required = false)]
        [StringValidator(AllowEmpty = true)]
        public string ResponseProperty { get; set; }

		[TaskAttribute("statuscodeproperty", Required = false)]
		[StringValidator(AllowEmpty = true)]
		public string StatusCodeProperty { get; set; }

        protected override void ExecuteTask()
        {
            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage();

            if (!string.IsNullOrEmpty(Method))
            {
                request.Method = Method;
            }

			request.Uri = new Uri(Url);
            
            if (!string.IsNullOrEmpty(ContentType))
            {
            	request.Headers.ContentType = ContentType;
            }

			if (!request.Method.Equals("GET", StringComparison.OrdinalIgnoreCase))
			{
				request.Content = (string.IsNullOrEmpty(Content)) ? HttpContent.CreateEmpty() : HttpContent.Create(Content);
				request.Headers.ContentLength = request.Content.GetLength();
			}
            
			if (ConnectionTimeout != 0)
			{
				client.TransportSettings.ConnectionTimeout = TimeSpan.FromSeconds(ConnectionTimeout);
			}

			Project.Log(Level.Info, "Executing HTTP request.");
			Project.Log(Level.Info, "Url: {0}", request.Uri);
			Project.Log(Level.Info, "Method: {0}", request.Method);
			Project.Log(Level.Info, "Content Type: {0}", request.Headers.ContentType);
			Project.Log(Level.Info, "Connection Timeout: {0}", client.TransportSettings.ConnectionTimeout);

            try
            {
                HttpResponseMessage response = client.Send(request);

				if (FailOnError)
				{
					response.EnsureStatusIsSuccessful();	
				}

				if (!string.IsNullOrEmpty(StatusCodeProperty))
				{
					Project.Properties[StatusCodeProperty] = response.StatusCode.ToString();
				}

				if (successCodes.Contains(response.StatusCode) && !string.IsNullOrEmpty(ResponseProperty))
                {
                    Project.Properties[ResponseProperty] = response.Content.ReadAsString();
                }

				Project.Log(Level.Info, "Received HTTP response.");
				Project.Log(Level.Info, "Status Code: {0}", response.StatusCode);
				Project.Log(Level.Info, "Content Type: {0}", response.Headers.ContentType);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                string message = string.Format("The HTTP '{0}' request to '{1}' failed:{2}{3}", Method, Url, Environment.NewLine, ex.Message);
                throw new BuildException(message, ex);
            }
        }
    }
}