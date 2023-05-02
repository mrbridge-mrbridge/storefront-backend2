using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain
{
    public class Token
    {
        public int Id { get; set; }

        public string ActivationCode { get; set; } = null!;

        public string Token1 { get; set; } = null!;

        public string UserId { get; set; } = null!;
    }
}
