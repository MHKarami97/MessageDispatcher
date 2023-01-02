using DataAccess.Context;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Util.Common;
using Util.Queue.Consumer;
using Util.Queue.Publisher;

namespace DataAccess
{
    public class ConfigurationRepository : IConfigurationRepository
    {
        private readonly ApplicationDbContext _context;

        public ConfigurationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<Configuration>> GetConfigurationAsync()
        {
            var configurations = await _context.Consumers
                .Include(c => c.Publishers)
                .ToListAsync();

            return configurations.Select(c => new Configuration
            {
                Consumer = Convert(c),
                Publishers = c.Publishers.Select(Convert).ToList(),
            }).ToList();
        }

        private QueueConsumerConfig Convert(Consumer consumer)
        {
            return new QueueConsumerConfig
            {
                HostNames = ConvertHostNames(consumer.HostNames),
                Exchange = consumer.Exchange,
                ExchangeType = consumer.ExchangeType,
                ExchangeAutoDelete = consumer.ExchangeAutoDelete,
                Durable = consumer.Durable,
                RouteingKey = consumer.RouteingKey,
                UserName = consumer.UserName,
                Password = consumer.Password,
                Queue = consumer.Queue,
                RequeueMessageRetryCount = consumer.RequeueMessageRetryCount,
                AutomaticRecoveryEnabled = consumer.AutomaticRecoveryEnabled,
            };
        }

        private QueuePublisherConfig Convert(Publisher publisher)
        {
            return new QueuePublisherConfig
            {
                HostNames = ConvertHostNames(publisher.HostNames),
                Exchange = publisher.Exchange,
                RouteingKey = publisher.RouteingKey,
                UserName = publisher.UserName,
                Password = publisher.Password,
                AutomaticRecoveryEnabled = publisher.AutomaticRecoveryEnabled,
                HasReProcess = publisher.HasReProcess,
            };
        }

        private ICollection<Endpoint> ConvertHostNames(string hostNames)
        {
            var hosts = hostNames.Split(',');
            return hosts.Select(h => new Endpoint(h)).ToList();
        }
    }
}