using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CrossworldsModManager
{
    public class GameBananaApiResponse
    {
        [JsonPropertyName("_aRecords")]
        public List<GameBananaMod>? Records { get; set; }
    }

    public class GameBananaModProfile
    {
        [JsonPropertyName("_sText")]
        public string Description { get; set; } = "";
        [JsonPropertyName("_sName")]
        public string Name { get; set; } = "";
        [JsonPropertyName("_nLikeCount")]
        public int LikeCount { get; set; }
        [JsonPropertyName("_aSubmitter")]
        public GameBananaSubmitter? Submitter { get; set; }
        [JsonPropertyName("_aPreviewMedia")]
        public GameBananaMedia? Media { get; set; }
    }

    public class GameBananaDownloadPage
    {
        [JsonPropertyName("_aFiles")]
        public List<GameBananaFile>? Files { get; set; }
    }

    public class GameBananaFile
    {
        [JsonPropertyName("_idRow")]
        public int FileId { get; set; }
        [JsonPropertyName("_sFile")]
        public string FileName { get; set; } = "";
        [JsonPropertyName("_sDownloadUrl")]
        public string DownloadUrl { get; set; } = "";
        public override string ToString() => FileName;
    }

    public class GameBananaMod
    {
        [JsonPropertyName("_idRow")]
        public int Id { get; set; }
        [JsonPropertyName("_sModelName")]
        public string ModelName { get; set; } = "";
        [JsonPropertyName("_sName")]
        public string Name { get; set; } = "";
        [JsonPropertyName("_sVersion")]
        public string Version { get; set; } = "";
        [JsonPropertyName("_sProfileUrl")]
        public string ProfileUrl { get; set; } = "";
        [JsonPropertyName("_nLikeCount")]
        public int LikeCount { get; set; }
        [JsonPropertyName("_aSubmitter")]
        public GameBananaSubmitter? Submitter { get; set; }
        [JsonPropertyName("_aPreviewMedia")]
        public GameBananaMedia? Media { get; set; }

        public string Author => Submitter?.Name ?? "Unknown";
        public string? ThumbnailUrl => Media?.Images?.FirstOrDefault()?.BaseUrl + "/" + Media?.Images?.FirstOrDefault()?.File530;
        public string DownloadUrl => $"https://gamebanana.com/dl/{Id}";
    }

    public class GameBananaSubmitter
    {
        [JsonPropertyName("_sName")]
        public string Name { get; set; } = "";
    }

    public class GameBananaMedia
    {
        [JsonPropertyName("_aImages")]
        public List<GameBananaImage>? Images { get; set; }
    }

    public class GameBananaImage
    {
        [JsonPropertyName("_sBaseUrl")]
        public string BaseUrl { get; set; } = "";
        [JsonPropertyName("_sFile100")]
        public string File100 { get; set; } = "";
        [JsonPropertyName("_sFile220")]
        public string File220 { get; set; } = "";
        [JsonPropertyName("_sFile530")]
        public string File530 { get; set; } = "";
    }

    public static class GameBananaApiService
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private const string ApiBaseUrl = "https://gamebanana.com/apiv11";

        private static async Task<T> GetAsync<T>(string url) where T : class
        {
            try
            {
                var response = await _httpClient.GetAsync(url);
                var content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException($"Request to GameBanana API failed with status code {response.StatusCode}.\nURL: {url}\nResponse: {content}");

                return JsonSerializer.Deserialize<T>(content) ?? throw new InvalidOperationException("Deserialized null response.");
            }
            catch (HttpRequestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while contacting the GameBanana API. Please check your internet connection. Details: {ex.Message}", ex);
            }
        }

        public static async Task<List<GameBananaMod>?> SearchModsAsync(int gameId, int page = 1, string search = "")
        {
            var url = $"{ApiBaseUrl}/Game/{gameId}/Subfeed?_nPage={page}&_sSort=new";
            if (!string.IsNullOrWhiteSpace(search))
                url += $"&_sName={System.Net.WebUtility.UrlEncode(search)}";

            var apiResponse = await GetAsync<GameBananaApiResponse>(url);
            var excludedTypes = new HashSet<string> { "Wip", "Question", "Request" };
            return apiResponse?.Records?
                .Where(mod => !excludedTypes.Contains(mod.ModelName, StringComparer.OrdinalIgnoreCase))
                .ToList();
        }

        public static Task<GameBananaModProfile?> GetModDetailsAsync(GameBananaMod mod)
        {
            return GetAsync<GameBananaModProfile?>($"{ApiBaseUrl}/{mod.ModelName}/{mod.Id}/ProfilePage");
        }

        public static Task<GameBananaDownloadPage?> GetModDownloadPageAsync(GameBananaMod mod)
        {
            return GetAsync<GameBananaDownloadPage?>($"{ApiBaseUrl}/{mod.ModelName}/{mod.Id}/DownloadPage");
        }

        public static async Task<GameBananaMod?> GetModFromProfilePageAsync(string modType, int modId)
        {
            var modProfile = await GetAsync<GameBananaModProfile>($"{ApiBaseUrl}/{modType}/{modId}/ProfilePage");
            if (modProfile == null) return null;

            return new GameBananaMod
            {
                Id = modId,
                ModelName = modType,
                Name = modProfile.Name,
                LikeCount = modProfile.LikeCount,
                Submitter = modProfile.Submitter,
                Media = modProfile.Media,
                ProfileUrl = $"https://gamebanana.com/{(modType.ToLower().EndsWith('s') ? modType.ToLower() : modType.ToLower() + 's')}/{modId}"
            };
        }

        public static async Task<string?> GetLatestModUpdateCountAsync(string itemType, int itemId)
        {
            if (string.IsNullOrEmpty(itemType) || itemId <= 0) return null;

            var apiUrl = $"{ApiBaseUrl}/{itemType}/{itemId}/Updates?_nPage=1&_nPerpage=1";
            using var jsonDoc = JsonDocument.Parse(await _httpClient.GetStringAsync(apiUrl));
            jsonDoc.RootElement.TryGetProperty("_aMetadata", out var metadata);
            metadata.TryGetProperty("_nRecordCount", out var recordCount);
            return recordCount.GetInt32().ToString();
        }

        public static async Task<string?> GetLatestModVersionAsync(string itemType, int itemId)
        {
            if (string.IsNullOrEmpty(itemType) || itemId <= 0) return null;

            var apiUrl = $"{ApiBaseUrl}/{itemType}/{itemId}/Updates?_nPage=1&_nPerpage=1";
            using var jsonDoc = JsonDocument.Parse(await _httpClient.GetStringAsync(apiUrl));
            jsonDoc.RootElement.TryGetProperty("_aRecords", out var records);
            if (records.GetArrayLength() > 0)
            {
                records[0].TryGetProperty("_sVersion", out var version);
                return version.GetString();
            }
            return null;
        }
    }
}
