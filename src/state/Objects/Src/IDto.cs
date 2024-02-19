using System;

namespace Objects.Src
{
    public interface IDto
    {
        public ulong Id { get; set; }
        
        public DateTime CreatedTime { get; set; }

        public DateTime UpdatedTime { get; set; }
    }
}
