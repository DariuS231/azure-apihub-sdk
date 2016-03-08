﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microfoft.Azure.ApiHub.Sdk.Extensions;
using Microfoft.Azure.ApiHub.Sdk.Management.Entities;
using Newtonsoft.Json.Linq;

namespace Microfoft.Azure.ApiHub.Sdk.Management
{
    public partial class ApiHubClient
    {
        public async Task<ConnectionId[]> GetConnections(string api)
        {
            // https://management.azure.com/subscriptions/83e6374a-dfa5-428b-82ef-eab6c6bdd383/providers/Microsoft.Web/locations/brazilsouth/managedApis/dropbox/connections?api-version=2015-08-01-preview"

            UriTemplate connectionsUrl = new UriTemplate("/subscriptions/{subscriptionId}/providers/Microsoft.Web/locations/{location}/managedApis/{apiName}/connections?api-version={apiVersion}");
            var parameters = new Dictionary<string, string>();
            parameters["apiName"] =  api;
            parameters["subscriptionId"] = this.subscriptionId;
            parameters["resourceGroup"] = this.resourceGroup;
            parameters["location"] = this.location;
            parameters["apiVersion"] = ApiVersion;
            Uri url = connectionsUrl.BindByName(_managementEndpointUri, parameters);

            var json = await SendAsync<JObject>(HttpMethod.Get, url);
            var connections = json["value"].ToObject<ArmEnvelope<JToken>[]>();

            return Array.ConvertAll(connections, x => new ConnectionId
            {
                Id = x.Id,
                ApiName = api,
                ConnectionName = x.Name,
                Name = x.Name
            });
        }

        public async Task<Tuple<string, Uri>> GetConnectionKey(ConnectionId connection)
        {
            // POST https://management.azure.com/subscriptions/83e6374a-dfa5-428b-82ef-eab6c6bdd383/resourceGroups/AzureFunctions/providers/Microsoft.Web/connections/1aab7e02-73cd-408c-ba1a-cd273ae48105/listConnectionKeys?api-version=2015-08-01-preview"

            UriTemplate connectionsUrl = new UriTemplate("{armId}/listConnectionKeys?api-version={apiVersion}");
            var parameters = new Dictionary<string, string>();
            parameters["armId"] = connection.Id;
            parameters["apiVersion"] = ApiVersion;
            Uri url = connectionsUrl.BindByName(_managementEndpointUri, parameters);

            var result = await SendAsync<JObject>(HttpMethod.Post, url);

            string connectonKey = result["connectionKey"].ToString();
            string runtimeUrl = result["runtimeUrls"].ToObject<string[]>()[0];
            return new Tuple<string, Uri>(connectonKey, new Uri(runtimeUrl));
        }
    }
}
