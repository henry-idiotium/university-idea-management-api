namespace UIM.Core.Helpers.Mappers;

public class SubmissionProfile : Profile
{
    public SubmissionProfile()
    {
        CreateMap<Submission, SubmissionDetailsResponse>()
            .ForMember(
                dest => dest.Id,
                opt => opt.MapFrom(src => EncryptHelpers.EncodeBase64Url(src.Id))
            );

        CreateMap<Submission, SimpleSubmissionResponse>()
            .ForMember(
                dest => dest.Id,
                opt => opt.MapFrom(src => EncryptHelpers.EncodeBase64Url(src.Id))
            );

        CreateMap<CreateSubmissionRequest, Submission>()
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive ?? true))
            .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => DateTime.Now));

        CreateMap<UpdateSubmissionRequest, Submission>()
            .ForSourceMember(src => src.Id, opt => opt.DoNotValidate())
            .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => DateTime.Now));
    }
}
