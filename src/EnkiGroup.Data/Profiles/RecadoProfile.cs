using AutoMapper;
using EnkiGroup.Core.Modelos;
using EnkiGroup.Shared.ViewModels;

namespace EnkiGroup.Data.Profiles
{
    public sealed class RecadoProfile : Profile
    {
        public RecadoProfile() 
            => CreateMap<Recado, RecadoViewModel>()
            .ForMember(r => r.TotalFilhos, c => c.MapFrom(r => r.RecadosFilhos.Count));
    }
}
