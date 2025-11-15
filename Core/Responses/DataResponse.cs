using System.Text.Json.Serialization;
using EzeePdf.Core.Extensions;

namespace EzeePdf.Core.Responses
{

    public class DataResponse
    {
        private object? info;
        private string? errorMessage;
        public EnumResponseCode Code { get; set; }
        public bool Success => Code == EnumResponseCode.Success;
        public string? ErrorMessage
        {
            get
            {
                if (errorMessage is null && Code != EnumResponseCode.Success)
                {
                    return Code.EnumDesc(false);
                }
                return errorMessage;
            }
            set
            {
                errorMessage = value;
            }
        }
        public object? Response { get; set; }
        public object? ErrorContext { get; set; }
        public object? Info
        {
            get
            {
                if (info is null && SendDescription)
                {
                    return Code.EnumDesc(false);
                }
                return info;
            }
            set
            {
                info = value;
            }
        }
#if !PROD
        public TimeSpan? TimeTaken { get; set; }
        public object? AttemptedValue { get; set; }
#endif

#if DEBUG
        public string? Exception { get; set; }
#endif
        public DataResponse()
        {
        }
        public DataResponse(EnumResponseCode code)
        {
            Code = code;
        }
        public static DataResponse OK => new DataResponse(EnumResponseCode.Success);
        public static DataResponse From(EnumResponseCode code) => new DataResponse(code);
        public static DataResponse FromError(EnumResponseCode code, string? errorMessage) => new DataResponse(code) { ErrorMessage = errorMessage };
        public static DataResponse<T> From<T>(T response) => DataResponse<T>.From(EnumResponseCode.Success, response);
        public static DataResponse<T> From<T>(EnumResponseCode code, T response) => DataResponse<T>.From(code, response);
        public static DataResponse<T> From<T>(EnumResponseCode code, T response, object info) => DataResponse<T>.From(code, response, info);
        public static DataResponse<T?> FromNull<T>(EnumResponseCode code, T? response) => DataResponse<T?>.From(code, response);
        public static DataResponse<T?> FromNull<T>(EnumResponseCode code, T? response, object info) => DataResponse<T?>.From(code, response, info);
        public static DataResponse<T?> FromNull<T>(EnumResponseCode code, object info) => DataResponse<T?>.From(code, default, info);
        public static DataResponse Format<T>(EnumResponseCode code, params object[] args)
        {
            var msg = code.EnumDesc(false);
            if (msg is not null)
            {
                msg = string.Format(msg, args);
            }
            else
            {
                msg = string.Empty;
            }
            return new DataResponse(code) { ErrorMessage = msg };
        }
        public static bool SendDescription
        {
            get; set;
        }
    }
    public class DataResponse<T> : DataResponse
    {
        public DataResponse() { }
        public DataResponse(EnumResponseCode code)
        {
            Code = code;
        }
        public DataResponse(EnumResponseCode code, T? response)
        {
            Code = code;
            base.Response = response;
            Response = response;
        }
        public DataResponse(EnumResponseCode code, T? response, object info)
        {
            Code = code;
            base.Response = response;
            Response = response;
            Info = info;
        }
        public new T? Response { get; set; }
        public static DataResponse<T> From(EnumResponseCode code, T? response) => new(code, response);
        public static DataResponse<T> From(EnumResponseCode code, T? response, object info) => new(code, response, info);

        public static implicit operator DataResponse<T>(EnumResponseCode code) => new(code);

        public static implicit operator DataResponse<T>(T response) => new(EnumResponseCode.Success, response);
    }
}
