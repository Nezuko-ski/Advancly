using Advancly.Core.DTOs;
using Advancly.Domain.Entitities;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace Advancly.Core.Utilities
{
    public class AdvanclyProfile : Profile
    {
        public AdvanclyProfile()
        {
            CreateMap<IdentityUser, UserDTO>();
        }
    }
}
