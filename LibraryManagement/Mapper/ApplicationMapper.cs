using AutoMapper;
using LibraryManagement.Dto.Request;
using LibraryManagement.Dto.Response;
using LibraryManagement.Models;

namespace LibraryManagement.Mapper
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper()
        {
            // Mapper Reader
            CreateMap<ReaderCreationRequest, Reader>()
            .ForMember(dest => dest.Dob, opt => opt.MapFrom(src => DateTime.SpecifyKind(src.Dob, DateTimeKind.Utc)));
            CreateMap<Reader, ReaderResponse>();
            CreateMap<ReaderUpdateRequest, Reader>()
                .ForMember(dest => dest.ReaderPassword, opt => opt.MapFrom(src => BCrypt.Net.BCrypt.HashPassword(src.ReaderPassword)))
                .ForMember(dest => dest.CreateDate, opt => opt.Ignore())
                .ForMember(dest => dest.Dob, opt => opt.Ignore());


            // Mapper Author
            CreateMap<AuthorRequest, Author>();
            CreateMap<Author, AuthorResponse>();

            // Mapper TypeReader
            CreateMap<TypeReaderRequest, TypeReader>();
            CreateMap<TypeReader, TypeReaderResponse>();

            // Mapper TypeBook 
            CreateMap<TypeBookRequest, TypeBook>();
            CreateMap<TypeBook, TypeBookResponse>();

            // Mapper Role
            CreateMap<RoleRequest, Role>();
            CreateMap<Role, RoleResponse>();

            // Mapper Parameter
            CreateMap<ParameterRequest, Parameter>();
            CreateMap<Parameter, ParameterResponse>();
        }
    }
}
