using System.Threading.Tasks;
using itb.Models.Api;

namespace itb.Services.Chat
{
    public interface IChatService
    {
        public Task<ChatModel> ReadOrCreateAsync(long chatId);
        public Task<ChatModel> UpdatePathAsync(long chatId, string path);
        public Task<ChatModel> UpdateUsernameAsync(long chatId, string username);
        public Task<ChatModel> UpdateStateAsync(long chatId, string state);
        public Task<ChatModel> CreateAsync(
            long chatId,
            string username = null,
            string path = null,
            string state = null
        );
        public Task<ChatModel> ReadAsync(long chatId);
        public Task<ChatModel> UpdateAsync(
            long chatId,
            string username = null,
            string path = null,
            string state = null
        );
        public Task DeleteAsync(long chatId);
    }
}