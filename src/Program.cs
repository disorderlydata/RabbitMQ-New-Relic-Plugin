using NewRelic.Platform.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQNewRelicPlugin
{
    class Program
    {
        static void Main(string[] args)
        {
            Runner runner = new Runner();
            runner.Add(new RabbitMQPluginAgentFactory());
            runner.SetupAndRun();
        }
    }
}
