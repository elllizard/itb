using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using itb.Models.Api;
using itb.Models.Configurtations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace itb.Services.Chat
{
    public class ChatService : IChatService
    {
        private readonly ILogger<ChatService> _logger;
        private readonly ApplicationConfiguration _applicationConfig;

        public ChatService(
            ILogger<ChatService> logger,
            IOptions<ApplicationConfiguration> applicationConfig
        )
        {
            _logger = logger;
            _applicationConfig = applicationConfig.Value;
        }

        public async Task<ChatModel> ReadOrCreateAsync(long chatId)
        {
            try
            {
                return await ReadAsync(chatId);
            }
            catch (Exception _readException)
            {
                return await CreateAsync(chatId);
            }
        }

        public async Task<ChatModel> UpdatePathAsync(long chatId, string path)
        {
            try
            {
                ChatModel _chat = await ReadAsync(chatId);
                return await UpdateAsync(chatId, _chat.Username, path, _chat.State);
            }
            catch (Exception _readException)
            {
                return await CreateAsync(chatId, null, path, null);
            }
        }
        
        public async Task<ChatModel> UpdateUsernameAsync(long chatId, string username)
        {
            try
            {
                ChatModel _chat = await ReadAsync(chatId);
                return await UpdateAsync(chatId, username, _chat.Path, _chat.State);
            }
            catch (Exception _readException)
            {
                return await CreateAsync(chatId, username, null, null);
            }
        }
        
        public async Task<ChatModel> UpdateStateAsync(long chatId, string state)
        {
            try
            {
                ChatModel _chat = await ReadAsync(chatId);
                return await UpdateAsync(chatId, _chat.Username, _chat.Path, state);
            }
            catch (Exception _readException)
            {
                return await CreateAsync(chatId, null, null, state);
            }
        }

        public async Task<ChatModel> CreateAsync(long chatId, string username = null, string path = null, string state = null)
        {
            _logger.LogInformation($"Making create request for chat with id '{chatId}'.");
            
            StringBuilder _url = new StringBuilder();
            _url.Append(_applicationConfig.ApiUrl);
            _url.Append("/api/chats");
            
            using HttpClient _httpClient = new HttpClient();

            ChatModel _chat = new ChatModel()
            {
                ChatId = chatId,
                Username = username,
                Path = path,
                State = state
            };
            string _json = JsonConvert.SerializeObject(_chat);
            StringContent _content = new StringContent(_json, Encoding.UTF8, "application/json");

            HttpResponseMessage _result = await _httpClient.PostAsync(_url.ToString(), _content);

            if (_result.StatusCode == HttpStatusCode.OK)
            {
                string _response = await _result.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ChatModel>(_response);
            }

            throw new Exception(
                $"Failed making create request for chat with id '{chatId}'. Got {_result.StatusCode} status code."
            );
        }

        public async Task<ChatModel> ReadAsync(long chatId)
        {
            _logger.LogInformation($"Making read request for chat with id '{chatId}'.");
            
            StringBuilder _url = new StringBuilder();
            _url.Append(_applicationConfig.ApiUrl);
            _url.Append("/api/chats/[id]");
            _url.Replace("[id]", chatId.ToString());

            using HttpClient _httpClient = new HttpClient();

            HttpResponseMessage _result = await _httpClient.GetAsync(_url.ToString());

            if (_result.StatusCode == HttpStatusCode.OK)
            {
                string _response = await _result.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ChatModel>(_response);
            }

            throw new Exception(
                $"Failed making read request for chat with id '{chatId}'. Got {_result.StatusCode} status code."
                );
        }

        public async Task<ChatModel> UpdateAsync(long chatId, string username = null, string path = null, string state = null)
        {
            _logger.LogInformation($"Making update request for chat with id '{chatId}'.");
            
            StringBuilder _url = new StringBuilder();
            _url.Append(_applicationConfig.ApiUrl);
            _url.Append("/api/chats/[id]");
            _url.Replace("[id]", chatId.ToString());
            
            using HttpClient _httpClient = new HttpClient();

            ChatModel _chat = new ChatModel()
            {
                ChatId = chatId,
                Username = username,
                Path = path,
                State = state
            };
            string _json = JsonConvert.SerializeObject(_chat);
            StringContent _content = new StringContent(_json, Encoding.UTF8, "application/json");

            HttpResponseMessage _result = await _httpClient.PutAsync(_url.ToString(), _content);

            if (_result.StatusCode == HttpStatusCode.OK)
            {
                string _response = await _result.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ChatModel>(_response);
            }

            throw new Exception(
                $"Failed making update request for chat with id '{chatId}'. Got {_result.StatusCode} status code."
            );
        }

        public async Task DeleteAsync(long chatId)
        {
            _logger.LogInformation($"Making delete request for chat with id '{chatId}'.");
            
            StringBuilder _url = new StringBuilder();
            _url.Append(_applicationConfig.ApiUrl);
            _url.Append("/api/chats/[id]");
            _url.Replace("[id]", chatId.ToString());

            using HttpClient _httpClient = new HttpClient();

            HttpResponseMessage _result = await _httpClient.GetAsync(_url.ToString());

            if (_result.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception(
                    $"Failed making read request for chat with id '{chatId}'. Got {_result.StatusCode} status code."
                );
            }
        }
    }
}