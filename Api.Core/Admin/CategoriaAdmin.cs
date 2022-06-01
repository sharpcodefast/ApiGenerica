using Api.Core.Dtos.Common;
using Api.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Core.Admin
{
    public class CategoriaAdmin : BaseAdmin<int, Entities.Categoria, Dtos.Categoria, FilterBase>
    {
        public override IQueryable GetQuery(FilterBase filter)
        {
            var query = MyContext.Categorias.AsQueryable();

            if (!string.IsNullOrEmpty(filter.MultiColumnSearchText))
            {
                query = query.Where(e => e.Nombre.StartsWith(filter.MultiColumnSearchText, StringComparison.InvariantCultureIgnoreCase)).AsQueryable();
            }

            return query;
        }

        public override Categoria ToEntity(Dtos.Categoria dto)
        {
            var entity = new Entities.Categoria();

            if (dto.Id.HasValue)
            {
                entity = MyContext.Categorias.Single(e => e.Id == dto.Id.Value);
            }

            entity.Nombre = dto.Nombre;
            return entity;
        }

        public override void Validate(Dtos.Categoria dto)
        {
        }

    }
}