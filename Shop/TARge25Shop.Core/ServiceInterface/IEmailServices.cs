

using TARge25Shop.Core.Dto;

namespace TARge25Shop.Core.ServiceInterface
{
    public interface IEmailServices
    {
        void SendEmail(EmailDto dto);
        void SendEmailToken(EmailTokenDto dto, string token);
    }
}
