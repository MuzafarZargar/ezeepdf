using EzeePdf.Core.Model.Feedback;
using EzeePdf.Core.Responses;

namespace EzeePdf.Core.Services
{
    public interface IFeedbackService
    {
        Task<DataResponse> SaveFeedback(UserFeedback request, string? ipAddress);
    }
}