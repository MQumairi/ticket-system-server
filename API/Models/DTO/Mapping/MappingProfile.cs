using AutoMapper;

namespace API.Models.DTO.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Ticket, TicketDto>();
            CreateMap<Product, ProductDto>();
            CreateMap<Status, StatusDto>();
            CreateMap<User, UserDto>();
            CreateMap<Comment, CommentDto>();
        }
    }
}