﻿using Microsoft.AspNetCore.Identity;
using ProEventos.Domain.Enum;
using System.Collections.Generic;

namespace ProEventos.Domain.Identity
{
    public class User : IdentityUser<int>
    {
        public string PrimeiroNome { get; set; }
        public string UltimoNome { get; set; }
        public Titulo Titulo { get; set; }
        public string Descricao { get; set; }
        public Funcao Funcao { get; set; }
        public string ImagemURL { get; set; }
        public List<Role> UserRoles { get; set; }
    }
}
