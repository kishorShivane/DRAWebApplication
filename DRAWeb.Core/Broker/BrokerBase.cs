using DRAWeb.Logger;
using DRAWeb.Proxy;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace DRAWeb.Core.Broker
{
    public class BrokerBase
    {
        public ILoggerManager logger;
        public IConfiguration config;
        public IDRAAzureServiceProxy proxy;
    }
}
