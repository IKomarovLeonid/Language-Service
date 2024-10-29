namespace API.Requests
{
    public class CreateAttemptHistoryRequestModel
    {
        public ulong TotalAttempts { get; set; }

        public ulong CorrectAttempts { get; set; }

        public ulong UserId { get; set; }

        public string WordErrors { get; set; }

        public string AttemptAttributes { get; set; }
    }
}
