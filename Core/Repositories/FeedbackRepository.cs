using EzeePdf.Core.DB;
using EzeePdf.Core.Model.Feedback;
using Microsoft.EntityFrameworkCore;

namespace EzeePdf.Core.Repositories
{
    public class FeedbackRepository(EzeepdfContext context) : IFeedbackRepository
    {
        private readonly EzeepdfContext context = context;

        public async Task SaveFeedback(UserFeedback request)
        {
            context.Feedbacks.Add(new Feedback
            {
                FeedbackText = request.FeedbackText,
                EmailAddress = request.EmailAddress,
                SourceDevice = request.SourceDevice,
                IpAddress = request.IpAddress,
                DateCreated = request.FeedbackDate
            });

            await context.SaveChangesAsync();
        }
        public Task<DateTime> GetThisIpAddressLastFeedbackTime(string ipAddress)
        {
            return context.Feedbacks.Where(x => x.IpAddress == ipAddress)
                .OrderByDescending(x => x.DateCreated)
                .Select(x => x.DateCreated)
                .FirstOrDefaultAsync();
        }
    }
}
