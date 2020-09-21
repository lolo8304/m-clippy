using Newtonsoft.Json;

namespace m_clippy.Models
{
    public class Error
    {
        //
        // Summary:
        //     Initializes a new instance of the Error class.
        public Error()
        {

        }
        //
        // Summary:
        //     Initializes a new instance of the Error class.
        //
        // Parameters:
        //   code:
        //     Error code
        //
        //   message:
        //     Error message
        //
        public Error(string code = null, string message = null)
        {
            Code = code;
            Message = message;
        }

        //
        // Summary:
        //     Gets or sets error code
        [JsonProperty(PropertyName = "code")]
        public string Code { get; set; }
        //
        // Summary:
        //     Gets or sets error message
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

    }
}
