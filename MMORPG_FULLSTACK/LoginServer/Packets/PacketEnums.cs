using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginServer.Packets
{
    /// <summary>
    /// all packet types allowed in the game
    /// </summary>
    public enum PacketType
    {
        LoginRequest,
        LoginResponse,
        Maintenance,
        Notification,
        // ... autres types
    }

    // Autres énumérations si nécessaire
}
