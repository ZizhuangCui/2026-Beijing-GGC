using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace AI
{
    /// <summary>
    /// Minimal Kimi (Moonshot) ChatCompletions client for Unity.
    /// Endpoint is OpenAI-compatible: POST /v1/chat/completions
    /// Base URL examples:
    /// - https://api.moonshot.cn/v1  (CN)
    /// - https://api.moonshot.ai/v1  (Global)  // use the one your console recommends
    /// </summary>
    public class KimiTaskClient : Singleton<KimiTaskClient>
    {
        private const string API_KEY = "sk-dhF45QTl9C784oNjMHv4bN0EAmW74mou5mTcVtNDqY0P6MKZ";
        private const string BASE_URL = "https://api.moonshot.cn/v1";
        private const string MODEL = "moonshot-v1-8k";

        [Header("Kimi API Config")]
        [Tooltip("Your Kimi/Moonshot API Key (DO NOT commit to git).")]
        [SerializeField] private string apiKey = "ak-f7yu69t8e8zi11bspbf1";

        [Tooltip("Example: https://api.moonshot.cn/v1")]
        [SerializeField] private string baseUrl = "https://api.moonshot.cn/v1";

        [Tooltip("Example from official quickstart: moonshot-v1-8k")]
        [SerializeField] private string model = "moonshot-v1-8k";

        [Header("Generation Settings")]
        [Range(0f, 2f)]
        [SerializeField] private float temperature = 0.6f;

        [SerializeField] private int maxTokens = 128;

        [Header("Prompt (you provide these)")]
        [TextArea(2, 6)]
        [SerializeField]
        private string systemPrompt =
            "给我生成5个数字，不需要解释。";
        //"你是一个游戏任务生成器。只输出一行任务文本，不要解释，给我英文。";

        [TextArea(2, 6)]
        [SerializeField]
        private string userPrompt =
            "给我生成5个不同数字，不需要其他内容。";
        //"给我生成一个简短、可执行的任务（5-10字）给我英文。";

        // ---------------- Public API ----------------

        /// <summary>
        /// Generate a task text from current prompts.
        /// onSuccess returns the model's text content (trimmed).
        /// </summary>
        public void GenerateTask(Action<string> onSuccess, Action<string> onError = null)
        {
            StartCoroutine(CoGenerateTask(systemPrompt, userPrompt, onSuccess, onError));
        }

        /// <summary>
        /// Generate a task text, overriding the user prompt for this call.
        /// (Use this when you later build your own template/wordbank to feed the model.)
        /// </summary>
        public void GenerateTask(string overrideUserPrompt, Action<string> onSuccess, Action<string> onError = null)
        {
            StartCoroutine(CoGenerateTask(systemPrompt, overrideUserPrompt, onSuccess, onError));
        }

        // ---------------- Implementation ----------------

        [Serializable]
        private class ChatMessage
        {
            public string role;
            public string content;
            public ChatMessage(string role, string content) { this.role = role; this.content = content; }
        }

        [Serializable]
        private class ChatCompletionRequest
        {
            public string model;
            public ChatMessage[] messages;
            public float temperature;
            public int max_tokens;
        }

        [Serializable]
        private class ChatCompletionResponse
        {
            public Choice[] choices;
        }

        [Serializable]
        private class Choice
        {
            public Message message;
        }

        [Serializable]
        private class Message
        {
            public string role;
            public string content;
        }

        private IEnumerator CoGenerateTask(string sysPrompt, string usrPrompt, Action<string> onSuccess, Action<string> onError)
        {
            if (string.IsNullOrWhiteSpace(API_KEY))
            {
                onError?.Invoke("Kimi API Key is empty.");
                yield break;
            }

            string url = $"{BASE_URL}/chat/completions";

            var reqBody = new ChatCompletionRequest
            {
                model = MODEL,

                messages = new[]
                {
                    new ChatMessage("system", sysPrompt ?? ""),
                    new ChatMessage("user", usrPrompt ?? "")
                },
                temperature = temperature,
                max_tokens = maxTokens
            };

            string json = JsonUtility.ToJson(reqBody);

            using (var req = new UnityWebRequest(url, "POST"))
            {
                byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
                req.uploadHandler = new UploadHandlerRaw(bodyRaw);
                req.downloadHandler = new DownloadHandlerBuffer();

                req.SetRequestHeader("Content-Type", "application/json");
                req.SetRequestHeader("Authorization", $"Bearer {API_KEY.Trim()}");

                yield return req.SendWebRequest();

#if UNITY_2020_2_OR_NEWER
                bool hasError = req.result != UnityWebRequest.Result.Success;
#else
                bool hasError = req.isNetworkError || req.isHttpError;
#endif
                if (hasError)
                {
                    string msg = $"HTTP Error: {req.responseCode}\n{req.downloadHandler.text}";
                    onError?.Invoke(msg);
                    yield break;
                }

                string respText = req.downloadHandler.text;

                // Parse "choices[0].message.content" (OpenAI-compatible)
                string content = TryParseContent(respText, out string parseError);
                if (content == null)
                {
                    onError?.Invoke($"Parse error: {parseError}\nRaw:\n{respText}");
                    yield break;
                }

                onSuccess?.Invoke(content.Trim());
            }
        }

        private string TryParseContent(string respJson, out string error)
        {
            error = null;
            try
            {
                var resp = JsonUtility.FromJson<ChatCompletionResponse>(respJson);
                if (resp?.choices == null || resp.choices.Length == 0 || resp.choices[0]?.message == null)
                {
                    error = "Missing choices/message in response.";
                    return null;
                }

                return resp.choices[0].message.content;
            }
            catch (Exception e)
            {
                error = e.Message;
                return null;
            }
        }
    }
}
