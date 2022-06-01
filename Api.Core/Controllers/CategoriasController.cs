using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Api.Core.Admin;
using Api.Core.Dtos.Common;
using Api.Core.Repositories;

namespace Api.Core.Controllers
{
    [Route("api/[controller]")]
    /*TODO: Configurar Authorize*/
    //[Authorize]
    [ApiController]
    public class CategoriasController : BaseController<CategoriaAdmin, int, Entities.Categoria, Dtos.Categoria, FilterBase>
    {
        public CategoriasController(MyContext context) : base(context)
        {
        }
    }
}