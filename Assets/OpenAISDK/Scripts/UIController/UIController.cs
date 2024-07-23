using System;
using ChatGPT;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIController : MonoBehaviour
{
    [SerializeField] private int tokens = 16;
    [SerializeField] private string chatModel = "text-davinci-003";
    [SerializeField] private TMP_Text resultText;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Button sendMessage;
    [SerializeField] private ScrollRect _scrollRect;
    [SerializeField] private RectTransform _responseContainer;
    [SerializeField] private TMP_Text _responsePrefab;
    private List<ChatGPTMessage> _history = new List<ChatGPTMessage>();

    void Start()
    {
        sendMessage.onClick.AddListener(SendMessage);
        ChatGPTApi.OnMessageReceived += ResultAction;
    }

    private void ResultAction(ChatGPTResponse resultMessage)
    {
        foreach (var choice in resultMessage.choices)
        {
            CreateTextBlock(choice.message.content, Color.white);
            CreateTextBlock("---------------", Color.yellow);
            _history.Add(new ChatGPTMessage("assistant", choice.message.content));
        }
    }

    private void CreateTextBlock(string text, Color color)
    {
        var response = Instantiate(_responsePrefab, _responseContainer);
        response.text = text;
        response.color = color;
        var size = response.preferredHeight;
        _responseContainer.sizeDelta = new Vector2(_responseContainer.sizeDelta.x, _responseContainer.sizeDelta.y + size);
        response.rectTransform.sizeDelta = new Vector2(response.rectTransform.sizeDelta.x, size);
        _scrollRect.verticalNormalizedPosition = 0;
    }

    private void SendMessage()
    {
        _history.Add(new ChatGPTMessage("user", inputField.text));
        var message = new ChatGPTRequest()
        {
            model = chatModel,
            max_tokens = tokens,
            messages = _history,
            temperature = 0.5f
        };
        CreateTextBlock(inputField.text, Color.yellow);
        ChatGPTApi.SendChatRequest(message);
    }
}
