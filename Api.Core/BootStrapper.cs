using AutoMapper;
using Api.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Core.Dtos;

namespace Api.Core
{
    public class BootStrapper:Profile
    {
        public static MapperConfiguration MapperConfiguration { get; private set; }

        public static void BootStrap()
        {
            MapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Entities.Categoria, Dtos.Categoria>();
                cfg.CreateMap<Account, AccountResponse>();

                cfg.CreateMap<Account, AuthenticateResponse>();

                cfg.CreateMap<RegisterRequest, Account>();

                cfg.CreateMap<CreateRequest, Account>();

                cfg.CreateMap<UpdateRequest, Account>()
                    .ForAllMembers(x => x.Condition(
                        (src, dest, prop) =>
                        {
                        // ignore null & empty string properties
                        if (prop == null) return false;
                            if (prop.GetType() == typeof(string) && string.IsNullOrEmpty((string)prop)) return false;

                        // ignore null role
                        if (x.DestinationMember.Name == "Role" && src.Role == null) return false;

                            return true;
                        }
                    ));
            });

        }
    }
}
