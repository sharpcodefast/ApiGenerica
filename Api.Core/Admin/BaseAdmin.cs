using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Api.Core.Dtos.Common;
using Api.Core.Entities;
using Api.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Api.Core.Infrastructure;
using Microsoft.Extensions.Configuration;

namespace Api.Core.Admin
{
    public abstract class BaseAdmin<TID, TE, TD, TF>
        where TF : FilterBase
        where TE : EntityBase<TID>
    {
        public MyContext MyContext;
        public IMapper Mapper;
        public AppSettings _appSettings;
        //public Mail _mail;
        //public IConfiguration Configuration { get; }
        public string UsuarioLogged { get; set; }
        //public UserManager<ApplicationUser> _userManager;

        public BaseAdmin(MyContext context) : this()
        {
            MyContext = context;
            //_appSettings = appSettings.Value;
        }

        //public BaseAdmin(IConfiguration configuration):this()
        //{
        //    Configuration = configuration;
        //    _appSettings = new AppSettings();
        //    Configuration.Bind("App", _appSettings);
        //}

        public BaseAdmin()
        {
            Mapper = new Mapper(BootStrapper.MapperConfiguration);
        }

        public virtual TD GetById(TID id)
        {
            var query = MyContext.Set<TE>().Include(MyContext.GetIncludePaths(typeof(TE)));
            var entity = query.Where(e => e.Id.Equals(id)).FirstOrDefault();

            return Mapper.Map<TE, TD>(entity);
        }

        public virtual IList<TD> GetAll()
        {
            var entities = (IList<TE>)MyContext.Set<TE>().AsQueryable().OfType<TE>().ToList();
            return Mapper.Map<IList<TE>, IList<TD>>(entities);
        }

        public virtual PagedListResponse<TD> GetByFilter(TF filter)
        {
            var query = GetQuery(filter).OfType<TE>();
            query = query.Where(e => !e.Deleted);

            var pageSize = filter.PageSize ?? 10;
            var currentPage = filter.CurrentPage ?? 1;

            var data = query.Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();

            return new PagedListResponse<TD>
            {
                Count = query.Count(),
                Data = Mapper.Map<IList<TE>, IList<TD>>(data)
            };
        }

        public virtual TD Create(TD dto)
        {
            Validate(dto);
            var entity = ToEntity(dto);

            entity.Enabled = true;
            entity.CreateDate = DateTime.Now;
            entity.CreatedBy = "Admin";

            MyContext.Set<TE>().Add(entity);
            MyContext.SaveChanges();

            return Mapper.Map<TE, TD>(entity);
        }

        public virtual TD Update(TD dto)
        {
            Validate(dto);
            var entity = ToEntity(dto);

            entity.UpdateDate = DateTime.Now;

            MyContext.SaveChanges();

            return Mapper.Map<TE, TD>(entity);
        }

        public virtual void Delete(TID id)
        {
            var entity = (TE)MyContext.Set<TE>().Find(id);

            entity.UpdateDate = DateTime.Now;
            entity.Deleted = true;

            MyContext.SaveChanges();
        }


        public virtual object GetDataList()
        {
            return null;
        }

        public virtual object GetDataEdit()
        {
            return null;
        }

        #region Abstract Methods

        public abstract TE ToEntity(TD dto);
        public abstract void Validate(TD dto);
        public abstract IQueryable GetQuery(TF filter);
        #endregion
    }

    public static partial class CustomExtensions
    {
        public static IQueryable<T> Include<T>(this IQueryable<T> source, IEnumerable<string> navigationPropertyPaths)
            where T : class
        {
            return navigationPropertyPaths.Aggregate(source, (query, path) => query.Include(path));
        }

        public static IEnumerable<string> GetIncludePaths(this DbContext context, Type clrEntityType)
        {
            var entityType = context.Model.FindEntityType(clrEntityType);
            var includedNavigations = new HashSet<INavigation>();
            var stack = new Stack<IEnumerator<INavigation>>();

            while (true)
            {
                var entityNavigations = new List<INavigation>();

                foreach (var navigation in entityType.GetNavigations())
                {
                    if (includedNavigations.Add(navigation))
                        entityNavigations.Add(navigation);
                }

                if (entityNavigations.Count == 0)
                {
                    if (stack.Count > 0)
                        yield return string.Join(".", stack.Reverse().Select(e => e.Current.Name));
                }
                else
                {
                    foreach (var navigation in entityNavigations)
                    {
                        var inverseNavigation = navigation.FindInverse();
                        if (inverseNavigation != null)
                            includedNavigations.Add(inverseNavigation);
                    }

                    stack.Push(entityNavigations.GetEnumerator());
                }

                while (stack.Count > 0 && !stack.Peek().MoveNext())
                    stack.Pop();

                if (stack.Count == 0) break;

                entityType = stack.Peek().Current.GetTargetType();
            }
        }

    }
}
