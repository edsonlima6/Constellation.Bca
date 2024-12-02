
using System.Net;

namespace Constellation.Bca.Domain.Common
{
    public class NotificationDomain
    {
        public List<string> ErrorMessages { get; private set; }

        public HttpStatusCode StatusCode { get; set; }

        public object? Data { get; set; }

        public NotificationDomain()
        {
            ErrorMessages = [];
        }

        public void AddErrorMessages(List<string> errorMessages) => ErrorMessages.AddRange(errorMessages);

        public void AddErrorMessage(string errorMessages) => ErrorMessages.Add(errorMessages);

        public bool IsValid() => ErrorMessages.Count == 0;
    }
}
