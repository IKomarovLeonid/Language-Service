using System.Collections.Generic;

namespace Business.Src.Objects
{
    public class SelectResult<TModel>
    {
        public string Message { get; private init; }

        public ICollection<TModel> Data { get; private init; }

        private SelectResult() { }

        public static SelectResult<TModel> Fetched(ICollection<TModel> data) => new SelectResult<TModel>()
        {
            Data = data,
            Message = string.Empty
        };

        public static SelectResult<TModel> Error(string message) => new SelectResult<TModel>()
        {
            Message = message
        };
    }
}
