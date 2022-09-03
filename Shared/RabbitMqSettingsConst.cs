using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class RabbitMqSettingsConst
    {
        public const string StockOrderCreatedEventQueueName = "stock-order-created-queue";
        public const string StockReservedEventQueueName = "stock-reserved-queue";
        public const string OrderPaymentCompletedEventQueueName = "order-payment-complated-queue";
        public const string OrderPaymentFailedEventQueueName = "order-payment-failede-queue";


    }
}
