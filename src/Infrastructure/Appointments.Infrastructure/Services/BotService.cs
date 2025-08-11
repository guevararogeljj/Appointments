using System.Text;
using Appointments.Application.Features.Bots.BotDefault;
using Appointments.Application.Features.Bots.BotDefault.Queries;
using Appointments.Domain.Common;
using Azure;
using Azure.AI.OpenAI;
using Microsoft.Extensions.Configuration;
using OpenAI.Chat;

namespace Appointments.Infrastructure.Services;

public class BotService : IBotService
{
    private readonly IConfiguration _configuration;
    public BotService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public async Task<Domain.Common.Response<BotMessagesDto>> GetConversation(string message)
    {
        var response = new Domain.Common.Response<BotMessagesDto>();
        var messageString = new StringBuilder();
        try
        {
            var endpoint = new Uri(_configuration.GetSection("Bots:DefaultBot:ApiUrl").Value!);
            var deploymentName = _configuration.GetSection("Bots:DefaultBot:Name").Value;
            var apiKey = _configuration.GetSection("Bots:DefaultBot:Token").Value;
            AzureOpenAIClient azureClient = new(endpoint, new AzureKeyCredential(apiKey));
            ChatClient chatClient = azureClient.GetChatClient(deploymentName);
            List<ChatMessage> messages = new List<ChatMessage>()
            {
                new SystemChatMessage("You are a helpful assistant.")
            };
            
            messages.Add(new UserChatMessage(message));
            var responses = chatClient.CompleteChatStreaming(messages);

            foreach (StreamingChatCompletionUpdate update in responses)
            {
                foreach (ChatMessageContentPart updatePart in update.ContentUpdate)
                {
                    messageString.Append(updatePart.Text);
                }
            }
            ///save context
            messages.Add(new AssistantChatMessage(messageString.ToString()));
            response.Result = new BotMessagesDto
            {
                Id = Guid.NewGuid(),
                Message = messageString.ToString(),
                Timestamp = DateTime.UtcNow,
                SenderName = "Bot",
                ReceiverName = "User",
                IsSenderBot = true
            };
        }

        catch (Exception ex)
        {
            response.Error = new Error(
                "Error",
                "An error occurred while processing the request.");
            return response;
        }

        return response;
    }
}