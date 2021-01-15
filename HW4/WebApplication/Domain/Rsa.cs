using System;
using Microsoft.AspNetCore.Identity;

namespace Domain
{
    public class Rsa
    {
        public int Id { get; set; }
        public ulong FirstKey { get; set; }
        public ulong SecondKey { get; set; }
        public string PlainText { get; set; }
        public string CypherText { get; set; }
        public string UserId { get; set; }
        public IdentityUser User { get; set; }
    }
}