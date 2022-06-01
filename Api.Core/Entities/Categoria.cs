using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Core.Entities
{
    public class Categoria : EntityBase<int>
    {
        public string Nombre { get; set; }
    }
}