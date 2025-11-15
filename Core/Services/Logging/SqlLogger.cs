using System.Text;
using EzeePdf.Core.DB;
using EzeePdf.Core.Exceptions;
using EzeePdf.Core.Repositories;

namespace EzeePdf.Core.Services.Logging
{
    public class SqlLogger(IEzeePdfSqlRepository ezeePdfSqlRepository)
    {
        private readonly IEzeePdfSqlRepository ezeePdfSqlRepository = ezeePdfSqlRepository;
        public async Task<bool> LogException(
                Exception? error,
                string? message = null,
                string? ipAddress = null,
                string? httpUrl = null)
        {
            bool successfullyLogged = false;
            Exception? innerMostError = error;

            try
            {
                var errorMessage = new StringBuilder();
                if (message is not null)
                {
                    errorMessage.AppendLine($"Error Info: {message}");
                }

                while (error != null)
                {
                    errorMessage.AppendLine("Date: " + Utils.UtcNow);
                    errorMessage.AppendLine("Time: " + Utils.UtcNow);
                    errorMessage.AppendLine();
                    errorMessage.AppendLine("Message");
                    errorMessage.AppendLine(error.Message.ToString());
                    errorMessage.AppendLine();
                    errorMessage.AppendLine("Source");
                    errorMessage.AppendLine(error.Source);
                    errorMessage.AppendLine();
                    errorMessage.AppendLine("Target site");
                    if (error.TargetSite == null)
                    {
                        errorMessage.AppendLine(string.Empty);
                    }
                    else
                    {
                        errorMessage.AppendLine(error.TargetSite.ToString());
                    }
                    errorMessage.AppendLine();
                    errorMessage.AppendLine("Stack trace");
                    errorMessage.AppendLine(error.StackTrace);
                    errorMessage.AppendLine();
                    errorMessage.AppendLine("ToString()");
                    errorMessage.AppendLine(error.ToString());

                    // Assign the next InnerException to catch the details of that exception as well
                    error = error.InnerException;

                    // We want to capture the most inner exception.
                    // This usually contains the most relevant error information.
                    if (error != null)
                    {
                        innerMostError = error;
                    }
                }

                // Capture some environment/session data.

                string errorMessageString = errorMessage.ToString();

                // Create the ErrorLog info
                var errorInfo = new ErrorLog()
                {
                    ErrorMessage = message ?? innerMostError?.Message ?? "Unknown Error",
                    Url = httpUrl ?? ServiceLocator.HttpReferrer ?? "Unknown Url",
                    ServerName = Utils.HostName,
                    IpAddress = ipAddress ?? ServiceLocator.IpAddress ,
                    FullMessage = errorMessageString,
                    DateCreated = DateTime.UtcNow,
                    UserId = ServiceLocator.UserId, 
                };

                try
                {
                    await ezeePdfSqlRepository.ExecuteProc(ProcNames.ERROR_LOG,
                        new Param(ProcParams.URL, errorInfo.Url),
                        new Param(ProcParams.SERVER_NAME, errorInfo.ServerName),
                        new Param(ProcParams.REMOTE_ADDRESS, errorInfo.IpAddress),
                        new Param(ProcParams.ERROR_MESSAGE, errorInfo.ErrorMessage),
                        new Param(ProcParams.FULL_MESSAGE, errorInfo.FullMessage));
                }
                catch (Exception exception)
                {
                    Console.WriteLine("Error saving error information: {0}", exception.GetExceptionDetails());
                }

                successfullyLogged = true;
            }
            catch
            {
                // We are already attempting to log an exception and another exception has occured.
                // Not much else we can do at this point as we don't want to get caught in a loop
                // of log, error, log, error...
            }

            return successfullyLogged;
        }
    }
}
