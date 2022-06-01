using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Api.Core.Admin;
using Api.Core.Dtos.Common;
using Api.Core.Entities;
using Api.Core.Repositories;
using Microsoft.Extensions.Options;

namespace Api.Core.Controllers
{
    public abstract class BaseController<TA, TID, TE, TD, TF> : ControllerBase
         where TA : BaseAdmin<TID, TE, TD, TF>, new()
         where TF : FilterBase
         where TE : EntityBase<TID>
    {
        #region Properties

        public UserManager<ApplicationUser> _userManager;

        protected TA _admin;
        // returns the current authenticated account (null if not logged in)
        public Account Account => (Account)HttpContext.Items["Account"];

        public string UserName
        {
            get
            {
                var claims = User.Claims.Where(c => c.Type == "email");
                return claims?.FirstOrDefault().Value;
            }
        }

        #endregion

        public BaseController(MyContext context)
        {
            _admin = new TA();
            _admin.MyContext = context;
        }

        public BaseController(MyContext context, IOptions<AppSettings> appSettings)
        {
            _admin = new TA();
            _admin.MyContext = context;
            _admin._appSettings = appSettings.Value;
        }

        public BaseController(MyContext context, UserManager<ApplicationUser> userManager)
        {
            _admin = new TA();
            _admin.MyContext = context;
            this._userManager = userManager;
        }

        [HttpGet]
        public ActionResult<PagedListResponse<TD>> Get([FromQuery] TF filter)
        {
            return _admin.GetByFilter(filter);
        }

        [HttpGet("{id}")]
        public ActionResult<TD> Get(TID id)
        {
            return _admin.GetById(id);
        }

        [HttpPost]
        public TD Post([FromBody] TD dto)
        {
            return _admin.Create(dto);
        }

        public void Put(TD dto)
        {
            _admin.Update(dto);
        }

        [HttpDelete("{id}")]
        public void Delete(TID id)
        {
            _admin.Delete(id);
        }

        [HttpGet("init/dataList")]
        public ActionResult<object> DataList()
        {
            return _admin.GetDataList();
        }

        [HttpGet("init/dataEdit")]
        public ActionResult<object> DataEdit()
        {
            return _admin.GetDataEdit();
        }
    }

    public class ApplicationUser : IdentityUser
    {
    }
}