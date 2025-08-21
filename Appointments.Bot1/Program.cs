using System;
using System.Collections.Generic;
using OpenAI.Chat;
using Azure;
using Azure.AI.OpenAI;

class SimpleChatBot
{
    static void Main()
    {
        // var respuestas = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        // {
        //     { "hola", "¡Hola! ¿En qué puedo ayudarte?" },
        //     { "¿cómo estás?", "Estoy bien, gracias por preguntar." },
        //     { "¿cuál es tu nombre?", "Soy un chatbot simple." },
        //     { "adiós", "¡Hasta luego!" }
        // };
        //
        // Console.WriteLine("Chatbot iniciado. Escribe tu pregunta (escribe 'salir' para terminar):");
        //
        // while (true)
        // {
        //     Console.Write("Tú: ");
        //     var pregunta = Console.ReadLine();
        //
        //     if (pregunta.Equals("salir", StringComparison.OrdinalIgnoreCase))
        //         break;
        //
        //     if (respuestas.TryGetValue(pregunta, out var respuesta))
        //         Console.WriteLine("Bot: " + respuesta);
        //     else
        //         Console.WriteLine("Bot: Lo siento, no entiendo la pregunta.");
        // }


        var endpoint = new Uri("https://guevararogeljj-2707-resource.cognitiveservices.azure.com/");
        var deploymentName = "gpt-4.1";
        var apiKey = "3zxDP1Ynv6A7Jsrp9YpP1WY9WP9TcZRSD5PmQ50Xiw9SyUiJzM3RJQQJ99BHACHYHv6XJ3w3AAAAACOGVMf3";

        AzureOpenAIClient azureClient = new(
            endpoint,
            new AzureKeyCredential(apiKey));
        ChatClient chatClient = azureClient.GetChatClient(deploymentName);

        List<ChatMessage> messages = new List<ChatMessage>()
        {
            new SystemChatMessage("You are a helpful assistant.")
        };

        Console.WriteLine("Chatbot iniciado. Escribe tu pregunta (escribe 'salir' para terminar):");

        while (true)
        {
            Console.Write("Tú: ");
            var pregunta = Console.ReadLine();

            if (pregunta.Equals("salir", StringComparison.OrdinalIgnoreCase))
                break;

            messages.Add(new UserChatMessage(pregunta));

            var response = chatClient.CompleteChatStreaming(messages);

            Console.Write("Bot: ");
            foreach (StreamingChatCompletionUpdate update in response)
            {
                foreach (ChatMessageContentPart updatePart in update.ContentUpdate)
                {
                    Console.Write(updatePart.Text);
                }
            }

            Console.WriteLine();

            // Opcional: agregar la respuesta del bot al historial si se desea mantener contexto
            // messages.Add(new AssistantChatMessage(respuesta));
        }
    }
}
