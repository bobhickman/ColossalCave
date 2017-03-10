using System.Collections.Generic;
using Newtonsoft.Json;

namespace ColossalCave.Webhook.Models
{
    public class ApiAiFulfillmentRequest
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }

        [JsonProperty("lang")]
        public string Lang { get; set; }

        [JsonProperty("result")]
        public ApiAiResult Result { get; set; }

        [JsonProperty("status")]
        public ApiAiStatus Status { get; set; }

        [JsonProperty("sessionId")]
        public string SessionId { get; set; }

        [JsonProperty("originalRequest")]
        public ApiAiOriginalRequest OriginalRequest { get; set; } // original request object // NOT IN SPEC
    }

    public class ApiAiResult
    {
        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("resolvedQuery")]
        public string ResolvedQuery { get; set; }

        [JsonProperty("action")]
        public string Action { get; set; }

        [JsonProperty("actionIncomplete")]
        public bool ActionIncomplete { get; set; }

        [JsonProperty("parameters")]
        public Dictionary<string, string> Parameters { get; set; }

        [JsonProperty("contexts")]
        public ApiAiContext[] Contexts { get; set; }

        [JsonProperty("fulfillment")]
        public ApiAiFulfillment Fulfillment { get; set; }

        [JsonProperty("score")]
        public double Score { get; set; }

        [JsonProperty("metadata")]
        public ApiAiMetadata Metadata { get; set; }

        [JsonProperty("speech")]
        public string Speech { get; set; } // NOT IN SPEC
    }

    public class ApiAiStatus
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("errorType")]
        public string ErrorType { get; set; }

        [JsonProperty("errorId")]
        public string ErrorId { get; set; }

        [JsonProperty("errorDetails")]
        public string ErrorDetails { get; set; }
    }

    public class ApiAiContext
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("parameters")]
        public Dictionary<string, string> Parameters { get; set; }

        [JsonProperty("lifespan")]
        public int? Lifespan { get; set; }
    }

    public class ApiAiFulfillment
    {
        [JsonProperty("speech")]
        public string Speech { get; set; }

        [JsonProperty("messages")]
        public ApiAiMessage[] Messages { get; set; }
    }

    public class ApiAiMetadata
    {
        [JsonProperty("intentId")]
        public string IntentId { get; set; }

        [JsonProperty("webhookUsed")]
        public string WebhookUsed { get; set; }

        [JsonProperty("webhookForSlotFillingUsed")]
        public string WebhookForSlotFillingUsed { get; set; }

        [JsonProperty("intentName")]
        public string IntentName { get; set; }
    }

    public class ApiAiMessage
    {
        [JsonProperty("type")]
        public int Type { get; set; }

        // Just using type 0 always for now
        [JsonProperty("speech")]
        public string Speech { get; set; }

        // TODO: Flesh out all the rest
    }

    public class ApiAiOriginalRequest
    {
        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("data")]
        public object Data { get; set; }
    }

    public class ApiAiFulfillmentResponse
    {
        [JsonProperty("speech")]
        public string Speech { get; set; }

        [JsonProperty("displayText")]
        public string DisplayText { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public object Data { get; set; }

        [JsonProperty("contextOut", NullValueHandling = NullValueHandling.Ignore)]
        public ApiAiContext[] ContextOut { get; set; }

        [JsonProperty("followupEvent", NullValueHandling = NullValueHandling.Ignore)]
        public object FollowupEvent { get; set; }
    }
}
