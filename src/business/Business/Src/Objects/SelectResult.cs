using System.Collections.Generic;

namespace Business.Src.Objects
{
    public class SelectResult<TModel>
    {
        public string Message { get; private init; }

        public ICollection<TModel> Data { get; private init; }

        public int Count;

        private SelectResult() { }

        public static SelectResult<TModel> Fetched(ICollection<TModel> data) => new SelectResult<TModel>()
        {
            Data = data,
            Count = data.Count,
            Message = string.Empty
        };

        public static SelectResult<TModel> Error(string message) => new SelectResult<TModel>()
        {
            Message = message
        };
    }
}
