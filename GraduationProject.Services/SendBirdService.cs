using AutoMapper;
using ECommerce.Domain.Contracts;
using GraduationProject.Domain.Data.Entities.SpecialistModule;
using GraduationProject.Domain.Entities.ParentModule;
using GraduationProject.Services.Abstraction;
using Microsoft.EntityFrameworkCore; // لازم تضيفه عشان Include يشتغل
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GraduationProject.Services
{

    public class SendBirdService : ISendbirdService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly string _appId;

        public SendBirdService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;

            _appId = config["Sendbird:AppId"]
                ?? throw new ArgumentNullException("Sendbird:AppId is missing");

            var apiToken = config["Sendbird:ApiToken"]
                ?? throw new ArgumentNullException("Sendbird:ApiToken is missing");

            _baseUrl = $"https://api-{_appId}.sendbird.com/v3";

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Api-Token", apiToken);
        }

   
        public async Task<string> CreateUserAsync(string userId, string nickname)
        {
            var payload = new
            {
                user_id = userId,
                nickname = nickname,
                profile_url = ""
            };

            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/users", payload);

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                var errorBody = await response.Content.ReadAsStringAsync();

                // المستخدم موجود مسبقاً → مش error حقيقي
                if (errorBody.Contains("already") || errorBody.Contains("exist"))
                    return errorBody;

                throw new HttpRequestException(
                    $"SendBird CreateUser failed: {errorBody}",
                    null,
                    HttpStatusCode.BadRequest);
            }

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

       
        public async Task<string> CreateChatBetweenParentAndSpecialistAsync(
            int parentId, int specialistId)
        {
            var parentUserId = $"parent_{parentId}";
            var specialistUserId = $"specialist_{specialistId}";
            var channelName = $"chat_{parentId}_{specialistId}";

           
            await CreateUserAsync(parentUserId, $"Parent {parentId}");
            await CreateUserAsync(specialistUserId, $"Specialist {specialistId}");

            var payload = new
            {
                name = channelName,
                channel_url = channelName,
                user_ids = new[] { parentUserId, specialistUserId },
                is_distinct = true
            };

            var response = await _httpClient.PostAsJsonAsync(
                $"{_baseUrl}/group_channels", payload);

            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException(
                    $"SendBird CreateChannel failed [{response.StatusCode}]: {errorBody}",
                    null,
                    response.StatusCode);
            }

           
            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            return doc.RootElement
                      .GetProperty("channel_url")
                      .GetString() ?? channelName;
        }
    }
}