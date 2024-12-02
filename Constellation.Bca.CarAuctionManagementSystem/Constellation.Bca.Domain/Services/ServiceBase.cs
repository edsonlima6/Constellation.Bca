using Constellation.Bca.Domain.Common;
using System.Net;

namespace Constellation.Bca.Domain.Services
{
    public abstract class ServiceBase
    {
        protected virtual NotificationDomain GetNotificationDomain(HttpStatusCode httpStatusCode, List<string> messages, object data)
        {
            var notification = new NotificationDomain() { StatusCode = httpStatusCode, Data = data };
            notification.AddErrorMessages(messages);
            return notification;
        }
    }
}
