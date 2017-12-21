using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Cake.ProGet.Asset
{
    /// <summary>
    /// Utilities for ProGetAssets.
    /// </summary>
    internal static class ProGetAssetUtils
    {
        internal static void ConfigureAuthorizationForHttpClient(HttpClient client, ProGetConfiguration config)
        {
            if (!string.IsNullOrWhiteSpace(config.ProGetUser) && !string.IsNullOrWhiteSpace(config.ProGetPassword))
            {
                var authByteArray = Encoding.ASCII.GetBytes($"{config.ProGetUser}:{config.ProGetPassword}");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authByteArray));
            }
        }

        internal static void ConfigureAuthorizationForHttpWebRequest(HttpWebRequest client, ProGetConfiguration config)
        {
            if (!string.IsNullOrWhiteSpace(config.ProGetUser) && !string.IsNullOrWhiteSpace(config.ProGetPassword))
            {
                var encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(config.ProGetUser + ":" + config.ProGetPassword));
                client.Headers.Add("Authorization", "Basic " + encoded);
            }
        }   
    }
}