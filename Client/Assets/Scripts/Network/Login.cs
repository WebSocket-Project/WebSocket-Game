using System;

namespace Shared.sdf
{
    [Serializable]
    public class LoginRequest
    {
        public ulong PlayerId { get; set; }
    }
  
    public class LoginResponse
    {
        public string Message { get; set; } = string.Empty;
    }
}