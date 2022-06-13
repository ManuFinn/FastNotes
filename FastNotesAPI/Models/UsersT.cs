using System;
using System.Collections.Generic;

#nullable disable

namespace FastNotesAPI.Models
{
    public partial class UsersT
    {
        public int Id { get; set; }
        public string NombreUsuario { get; set; }
        public string PasswordUsuario { get; set; }
    }
}
