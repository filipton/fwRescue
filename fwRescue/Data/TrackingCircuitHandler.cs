using Microsoft.AspNetCore.Components.Server.Circuits;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace fwRescue.Data
{
    public class TrackingCircuitHandler : CircuitHandler
    {
        private HashSet<Circuit> circuits = new HashSet<Circuit>();

        public override Task OnConnectionUpAsync(Circuit circuit,
            CancellationToken cancellationToken)
        {
            circuits.Add(circuit);



            return Task.CompletedTask;
        }

        public override Task OnConnectionDownAsync(Circuit circuit,
            CancellationToken cancellationToken)
        {
            circuits.Remove(circuit);

            /*foreach (var dmchat in DiscordUser.Client.PrivateChannels)
            {
                if (dmchat.Id == DiscordUser.ActiveChatId)
                {
                    dmchat.SendMessageAsync("DISPOSE");
                }
            }*/

            return Task.CompletedTask;
        }

        public int ConnectedCircuits => circuits.Count;
    }
}
