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
            CreateMap<ReaderRequest, Reader>();
            CreateMap<Reader, ReaderResponse>();
            CreateMap<ReaderUpdateRequest, Reader>()
                .ForMember(dest => dest.ReaderPassword, opt => opt.MapFrom(src => BCrypt.Net.BCrypt.HashPassword(src.ReaderPassword)))
                .ForMember(dest => dest.CreateDate, opt => opt.Ignore());

            // Mapper Author
            CreateMap<AuthorCreationRequest, Author>();
            CreateMap<Author, AuthorResponse>();
            CreateMap<AuthorUpdateRequest, Author>();

            // Mapper Role
            CreateMap<RoleRequest, Role>();
            CreateMap<Role, RoleResponse>();
        }
    }
}
