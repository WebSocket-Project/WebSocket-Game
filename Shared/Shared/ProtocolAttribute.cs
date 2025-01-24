using System;

namespace Shared
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ProtocolAttribute : Attribute
    {
        public Protocol Protocol { get; }
    
        public ProtocolAttribute(Protocol protocol)
        {
            Protocol = protocol;
        }
    }
}