namespace SMS.Net.Channel.Avochato
{
    using Newtonsoft.Json;
    using System;

    /// <summary>
    /// the Avochato message
    /// </summary>
    public partial class AvochatoMessage
    {
        /// <summary>
        /// Avochato Authentication Id.
        /// </summary>
        [JsonProperty("auth_id")]
        public string AuthId { get; set; }

        /// <summary>
        /// Avochato Authentication secret.
        /// </summary>
        [JsonProperty("auth_secret")]
        public string AuthSecret { get; set; }

        /// <summary>
        /// Phone number to send the message to
        /// </summary>
        [JsonProperty("phone")]
        public string Phone { get; set; }

        /// <summary>
        /// Text to send to the recipient.
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; set; }

        /// <summary>
        /// (E.164 format) Phone Number to use to send this message. Must be a valid Avochato number that belongs to this in-box.
        /// </summary>
        [JsonProperty("from")]
        public string From { get; set; }

        /// <summary>
        /// Mark this conversation as "addressed" (default: false).
        /// </summary>
        [JsonProperty("mark_addressed")]
        public bool MarkAddressed { get; set; }

        /// <summary>
        /// Comma-separated list of tags to apply to this contact.
        /// </summary>
        [JsonProperty("tags")]
        public string Tags { get; set; }

        /// <summary>
        /// Media attachment to send via MMS (500kb limit).
        /// </summary>
        [JsonProperty("media_url")]
        public Uri MediaUrl { get; set; }

        /// <summary>
        /// Send a callback via http POST to this domain when the delivery status is updated
        /// </summary>
        [JsonProperty("status_callback")]
        public Uri StatusCallback { get; set; }
    }
}