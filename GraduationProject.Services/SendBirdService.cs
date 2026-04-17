using AutoMapper;
using GraduationProject.Services.Abstraction;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Services
{
    public class SendBirdService : ISendbirdService
    {
        private readonly HttpClient _httpClient;
        private readonly string _appId;

        public SendBirdService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _appId = config["Sendbird:AppId"];
            var apiToken = config["Sendbird:ApiToken"];

            // Correct header for Sendbird
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Api-Token", apiToken);
        }

        public async Task<string> CreateUserAsync(string userId, string nickname)
        {
            var payload = new
            {
                user_id = userId,
                nickname = nickname,
                profile_url = "https://example.com/default-avatar.png" // placeholder لازم يكون موجود
            };

            var response = await _httpClient.PostAsJsonAsync(
                $"https://api-{_appId}.sendbird.com/v3/users", payload);

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> CreateChannelAsync(string channelName, List<string> userIds)
        {
            var payload = new { name = channelName, user_ids = userIds };
            var response = await _httpClient.PostAsJsonAsync($"https://api-{_appId}.sendbird.com/v3/group_channels", payload);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> SendMessageAsync(string channelUrl, string message, string userId)
        {
            var payload = new { message = message, user_id = userId };
            var response = await _httpClient.PostAsJsonAsync($"https://api-{_appId}.sendbird.com/v3/group_channels/{channelUrl}/messages", payload);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}
