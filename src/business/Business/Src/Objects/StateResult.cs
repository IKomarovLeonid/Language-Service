namespace Business.Src.Objects
{
    public class StateResult
    {
        public ulong? Id { get; private set; }
        public bool IsSuccess { get; private set; }

        public string ErrorMessage { get; private set; }

        private StateResult() { }

        public static StateResult Success(ulong id) => new StateResult { IsSuccess = true, Id = id };

        public static StateResult Error(string errorMessage) => new StateResult { 
            IsSuccess = false,
            ErrorMessage = errorMessage
        };
    }
}
