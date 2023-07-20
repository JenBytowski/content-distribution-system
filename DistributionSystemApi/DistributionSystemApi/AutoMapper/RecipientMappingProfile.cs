using AutoMapper;
using DistributionSystemApi.Data.Entities;
using DistributionSystemApi.DistributionSystemApi.Services.Models;
using DistributionSystemApi.Requests;
using DistributionSystemApi.Responses;
using DistributionSystemApi.Services.Models;

namespace DistributionSystemApi.AutoMapper
{
    public class RecipientMappingProfile : Profile
    {
        public RecipientMappingProfile()
        {
            CreateMap<RecipientRecipientGroupModel, RecipientRecipientGroup>();
            CreateMap<RecipientRecipientGroup, RecipientRecipientGroupModel>();
            CreateMap<Recipient, GetRecipient>();
            CreateMap<GetRecipient, RecipientResponse>();
            CreateMap<CreateRecipient, CreateRecipientRequest>();
            CreateMap<CreateRecipientRequest, CreateRecipient>();
            CreateMap<RecipientRecipientGroupModel, RecipientRecipientGroup>();
            CreateMap<RecipientRecipientGroup, RecipientRecipientGroupModel>();
            CreateMap<GetRecipientGroup, RecipientGroupResponse>();
            CreateMap<RecipientGroupResponse, GetRecipientGroup>();
            CreateMap<CreateRecipientGroup, CreateRecipientGroupRequest>();
            CreateMap<CreateRecipientGroupRequest, CreateRecipientGroup>();
            CreateMap<PaginationPage<GetRecipient>, PaginationPage<RecipientResponse>>();
            CreateMap<PaginationPage<RecipientResponse>, PaginationPage<GetRecipient>>();
            CreateMap<PaginationPage<GetRecipient>, PaginationPage<RecipientResponse>>();
            CreateMap<CreateRecipientRequest, CreateRecipient>();
            CreateMap<Guid, RecipientRecipientGroupModel>();
            CreateMap<RecipientRecipientGroupModel, RecipientRecipientGroupResponse>();
            CreateMap<GetRecipient, RecipientResponse>()
                .ForMember(dest => dest.Groups, opt => opt.MapFrom(src => src.Groups ?? new List<RecipientRecipientGroupModel>()));
        }
    }
}