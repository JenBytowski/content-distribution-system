namespace DistributionSystemApi.Services.Mapping
{
    using AutoMapper;
    using global::DistributionSystemApi.Data.Entities;
    using global::DistributionSystemApi.DistributionSystemApi.Services.Models;

    public class ServicesMappingProfile : Profile
    {
        public ServicesMappingProfile()
        {
            CreateMap<RecipientRecipientGroupModel, RecipientRecipientGroup>();
            CreateMap<RecipientRecipientGroup, RecipientRecipientGroupModel>();
            CreateMap<Recipient, GetRecipient>();
            CreateMap<RecipientRecipientGroupModel, RecipientRecipientGroup>();
            CreateMap<RecipientRecipientGroup, RecipientRecipientGroupModel>();
            CreateMap<Guid, RecipientRecipientGroupModel>();
        }
    }
}
