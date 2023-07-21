namespace DistributionSystemApi.AutoMapper
{
    using AutoMapper;
    using global::DistributionSystemApi.DistributionSystemApi.Services.Models;
    using global::DistributionSystemApi.Requests;
    using global::DistributionSystemApi.Responses;
    using global::DistributionSystemApi.Services.Models;
    using global::AutoMapper;

    public class RecipientMappingProfile : Profile
    {
        public RecipientMappingProfile()
        {
            CreateMap<GetRecipient, RecipientResponse>();
            CreateMap<CreateRecipient, CreateRecipientRequest>();
            CreateMap<Guid, RecipientGroupResponse>();
            CreateMap<CreateRecipientRequest, CreateRecipient>();
            CreateMap<GetRecipientGroup, RecipientGroupResponse>();
            CreateMap<RecipientGroupResponse, GetRecipientGroup>();
            CreateMap<CreateRecipientGroup, CreateRecipientGroupRequest>();
            CreateMap<CreateRecipientGroupRequest, CreateRecipientGroup>();
            CreateMap<PaginationPage<GetRecipient>, PaginationPage<RecipientResponse>>();
            CreateMap<PaginationPage<RecipientResponse>, PaginationPage<GetRecipient>>();
            CreateMap<CreateRecipientRequest, CreateRecipient>();
            CreateMap<RecipientRecipientGroupModel, RecipientRecipientGroupResponse>();
            CreateMap<GetRecipient, RecipientResponse>()
                .ForMember(dest => dest.Groups, opt => opt.MapFrom(src => src.Groups ?? new List<RecipientRecipientGroupModel>()));
        }
    }
}