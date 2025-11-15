using EzeePdf.Core.DB;
using EzeePdf.Core.Enums;
using EzeePdf.Core.Extensions;
using EzeePdf.Core.Model.Pdf;
using Microsoft.EntityFrameworkCore;

namespace EzeePdf.Core.Repositories
{
    public class PdfUsageRepository(EzeepdfContext context) : IPdfUsageRepository
    {
        public async Task AddUpload(PdfUsage request)
        {
            context.PdfUploads.Add(new PdfUpload
            {
                UserId = request.UserId,
                PdfFunctionId = request.Function,
                IpAddress = request.IpAddress,
                FileName = request.FileName,
                PdfSize = request.PdfSize,
                SourceDevice = request.SourceDevice,
                UploadDate = request.UsageDate,
            });
            await context.SaveChangesAsync();
        }
        public async Task AddUsage(PdfUsage request)
        {
            context.UserPdfUsages.Add(new UserPdfUsage
            {
                UserId = request.UserId,
                PdfFunctionId = request.Function,
                IpAddress = request.IpAddress,
                PdfPageCount = request.PageCount,
                PdfSize = request.PdfSize,
                PdfChangedSize = request.PdfChangedSize,
                SourceDevice = request.SourceDevice,
                UsageDate = request.UsageDate,
            });
            await context.SaveChangesAsync();
        }
        public Task<DateTime> GetThisIpAddressLastUsageTime(string ipAddress, EnumPdfFunction type)
        {
            return context.UserPdfUsages.Where(x => x.PdfFunctionId == type.Value() && x.IpAddress == ipAddress)
                .OrderByDescending(x => x.UsageDate)
                .Select(x => x.UsageDate)
                .FirstOrDefaultAsync();
        }
        public Task<double> GetDayUsage(DateTime date)
        {
            return context.PdfUploads
                        .Where(x => x.UploadDate.Date == date)
                        .SumAsync(x => x.PdfSize);
        }
    }
}
