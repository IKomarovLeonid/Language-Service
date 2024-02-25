using System;

namespace Objects.Src.Dto
{
    public class UserDto : IDto
    {
        public ulong Id { get; set; }

        public string Email { get; set; }

        public bool IsAdmin { get; set; } 

        public DateTime CreatedTime { get; set; }

        public DateTime UpdatedTime { get; set; }
    }
}
