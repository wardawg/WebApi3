

using System.ComponentModel.DataAnnotations;

namespace Repos.Owin.Models
{
    public class ClientRefInfo

    {
        [Key]
        public int ClientId { get; set; }
        public string AssmPrefix { get; set; }
        public string ExtClientId { get; set; }
        public string ClientKey { get; set; }

    }
}