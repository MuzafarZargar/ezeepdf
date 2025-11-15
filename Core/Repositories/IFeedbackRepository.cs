using EzeePdf.Core.Model.Feedback;

namespace EzeePdf.Core.Repositories
{
    public interface IFeedbackRepository
    {
        Task SaveFeedback(UserFeedback request);
        Task<DateTime> GetThisIpAddressLastFeedbackTime(string ipAddress);
    }
}