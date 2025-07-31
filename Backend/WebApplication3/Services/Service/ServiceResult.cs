namespace WebApplication3.Services.Service
{
        public class ServiceResult<T>
        {
            public bool Success { get; set; }
            public string Message { get; set; } = string.Empty;
            public T? Data { get; set; }

            public static ServiceResult<T> Fail(string message) => new() { Success = false, Message = message };
            public static ServiceResult<T> Ok(T data) => new() { Success = true, Data = data };
        }

    
}
