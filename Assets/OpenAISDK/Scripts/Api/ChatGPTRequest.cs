using System;
using System.Collections.Generic;

namespace ChatGPT
{
    [Serializable]
    public class ChatGPTRequest
    {
        public string model;
        public List<ChatGPTMessage> messages;
        public int max_tokens;
        public float temperature;
    }

    [Serializable]
    public class ChatGPTMessage 
    {
        public string role;
        public string content;

        public ChatGPTMessage(string role, string content)
        {
            this.role = role;
            this.content = content;
        }
    }
}