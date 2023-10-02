namespace State.Src
{
    public class CheckResult
    {
        public string ErrorMessage { get; private set; }

        public bool IsSuccess { get; private set; }

        private CheckResult(string error, bool isSuccess) {
            this.ErrorMessage = error;
            this.IsSuccess = isSuccess;
        }

        public static CheckResult Error(string error) => new CheckResult(error, false);

        public static CheckResult Success() => new CheckResult(null, true);
    }
}
