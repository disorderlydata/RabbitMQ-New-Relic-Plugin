using NewRelic.Platform.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQNewRelicPlugin
{
    public class RabbitMQPluginAgentFactory : AgentFactory
    {
        public override Agent CreateAgentWithConfiguration(IDictionary<string, object> properties)
        {
            string agentName = properties["name"].ToString();
            string vhost = properties["vhost"].ToString();
            string hostName = properties["hostname"].ToString();
            string userName = properties["username"].ToString();
            string password = properties["password"].ToString();
            int port = 15672;

            if (properties.ContainsKey("port"))
            {
                port = int.Parse(properties["port"].ToString());
            }


            return new RabbitMQAgent(agentName, vhost, hostName, userName, password, port);
        }
    }
}
