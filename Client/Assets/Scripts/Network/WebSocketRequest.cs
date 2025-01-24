using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Shared;

namespace Assets.Scripts.Network
{
    [Serializable]
    internal class WebSocketRequest
    {
        public Protocol Protocol { get; set; }
        public object Payload { get; set; }
    }
}
