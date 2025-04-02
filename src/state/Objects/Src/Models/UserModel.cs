using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Objects.Src.Models
{
    public class UserModel
    {
        public ulong Id { get; set; }

        public bool IsAdmin { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        public double UserRating { get; set; }

        public double SuccessPercent { get; set; }

        public DateTime CreatedTime { get; set; }

        public DateTime UpdatedTime { get; set; }
    }
}
