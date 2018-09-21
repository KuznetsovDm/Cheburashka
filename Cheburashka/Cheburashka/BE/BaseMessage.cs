using System;

namespace Cheburashka.BE
{
    [Serializable]
    public class BaseMessage
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public string Method { get; set; }
        public string Data { get; set; }
    }
}
