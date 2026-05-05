using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using FolkerKinzel.VCards;
using FolkerKinzel.VCards.Enums;

namespace PeopleIKnow.CardDavExport
{
    public class CardDavExporter : IContactExporter, IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly string _addressBookUrl;

        public CardDavExporter(string serverUrl, string addressBookPath, string username, string password)
        {
            _addressBookUrl = serverUrl.TrimEnd('/') + "/" + addressBookPath.Trim('/') + "/";

            _httpClient = new HttpClient();
            var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
        }

        public async Task ExportAsync(VCard vcard, string uid)
        {
            var vcfContent = Vcf.ToString(new[] { vcard }, VCdVersion.V3_0);
            var url = $"{_addressBookUrl}{uid}.vcf";

            var content = new StringContent(vcfContent, Encoding.UTF8, "text/vcard");
            var response = await _httpClient.PutAsync(url, content);

            switch (response.StatusCode)
            {
                case HttpStatusCode.Created:
                    Console.WriteLine($"  Created: {uid}");
                    break;
                case HttpStatusCode.NoContent:
                    Console.WriteLine($"  Updated: {uid}");
                    break;
                default:
                    var body = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"  Failed ({response.StatusCode}): {uid} - {body}");
                    throw new HttpRequestException($"CardDAV PUT failed with {response.StatusCode}");
            }
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}
