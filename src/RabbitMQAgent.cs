using EasyNetQ.Management.Client;
using NewRelic.Platform.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQNewRelicPlugin
{
    public class RabbitMQAgent : Agent
    {
        private readonly string agentName;
        private readonly string vhost;

        private readonly IManagementClient client;

        public RabbitMQAgent(string agentName, string vhost, string hostName, string userName, string password, int port)
        {
            this.agentName = agentName;
            this.vhost = vhost;
            
            this.client = new ManagementClient(hostName, userName, password, port);
        }

        public override string GetAgentName()
        {
            return this.agentName;
        }

        public override string Guid
        {
            get { return "com.disorderlydata.rabbitmqplugin2"; }
        }

        public override void PollCycle()
        {
            var overview = client.GetOverview();
            
            ReportMetric("Queued Messages/Ready", "messages", overview.QueueTotals.MessagesReady);
            ReportMetric("Queued Messages/Unacknowledged", "messages", overview.QueueTotals.MessagesUnacknowledged);
            
            var queues = client.GetQueues();

            if (!string.IsNullOrWhiteSpace(vhost))
            {
                queues = queues.Where(q => q.Vhost.Equals(vhost, StringComparison.InvariantCultureIgnoreCase));
            }

            foreach (var queue in queues)
            {
                if (queue.Name.StartsWith("amq.gen"))
                {
                    continue;
                }

                ReportMetric(string.Format("Queue/{0}/{1}/Messages/Ready", vhost, queue.Name), "message", queue.MessagesReady);
                ReportMetric(string.Format("Queue/{0}/{1}/Memory", vhost, queue.Name), "bytes", queue.Memory);
                ReportMetric(string.Format("Queue/{0}/{1}/Messages/Total", vhost, queue.Name), "message", queue.Messages);
                ReportMetric(string.Format("Queue/{0}/{1}/Consumers/Total", vhost, queue.Name), "consumers", queue.Consumers);
                ReportMetric(string.Format("Queue/{0}/{1}/Consumers/Active", vhost, queue.Name), "consumers", queue.ActiveConsumers);
            }
        }

        public override string Version
        {
            get { return "0.0.1"; }
        }
    }
}
