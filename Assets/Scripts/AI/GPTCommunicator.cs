using OpenAI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class GPTCommunicator
{
    private static OpenAIApi openAI = new OpenAIApi();

    public static bool GENERATING = false;

    public static async void Prompt(Action<ChatMessage> process, params List<ChatMessage>[] lists)
    {
        if (GENERATING) return;

        GENERATING = true;

        IEnumerable<ChatMessage> complete = null;
        foreach (List<ChatMessage> list in lists)
        {
            complete = (complete == null) ? list.AsEnumerable() : complete.Concat(list);
        }

        CreateChatCompletionRequest request = new CreateChatCompletionRequest();
        request.Model = "gpt-4o-mini";
        request.Messages = complete.ToList();

        var response = await openAI.CreateChatCompletion(request);

        if (response.Choices != null && response.Choices.Count > 0)
        {
            int idx = UnityEngine.Random.Range(0, response.Choices.Count);
            process(response.Choices[idx].Message);
        }

        GENERATING = false;
    }
}
