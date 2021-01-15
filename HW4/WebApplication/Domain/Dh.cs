using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Domain
{
    public class Dh
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public IdentityUser User { get; set; }
        public int moduP { get; set; }
        public int baseG { get; set; }
      
        public ulong secretIntA { get; set; }
        public ulong secretIntB { get; set; }
        [Range(0, 20)]
        public ulong secretA { get; set; }
        [Range(0, 20)]
        public ulong secretB { get; set; }
        public ulong comA { get; set; }
        public ulong comB { get; set; }

    }
}