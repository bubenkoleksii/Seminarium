﻿namespace SchoolService.Api.Mappings;

public class ApiMappingProfile : Profile
{
    public ApiMappingProfile()
    {
        ConfigureGlobalMappings();

        ConfigureSchoolMapping();

        ConfigureJoiningRequestMapping();

        ConfigureSchoolProfileMappings();

        ConfigureGroupMappings();

        ConfigureStudyPeriodMappings();

        ConfigureGroupNoticeMappings();
    }

    private void ConfigureSchoolMapping()
    {
        CreateMap<CreateSchoolRequest, CreateSchoolCommand>();

        CreateMap<UpdateSchoolRequest, UpdateSchoolCommand>()
            .ForMember(command => command.Email, act => act.MapFrom(req => req.Email == null ? null : req.Email.ToLower()));

        CreateMap<SchoolModelResponse, SchoolResponse>();

        CreateMap<GetAllSchoolsParams, GetAllSchoolsQuery>();

        CreateMap<GetAllSchoolsModelResponse, GetAllSchoolsResponse>();
    }

    private void ConfigureJoiningRequestMapping()
    {
        CreateMap<CreateJoiningRequest, CreateJoiningRequestCommand>()
            .ForMember(command => command.RequesterEmail, act => act.MapFrom(req => req.RequesterEmail.ToLower()));

        CreateMap<JoiningRequestModelResponse, JoiningRequestResponse>();

        CreateMap<GetAllJoiningRequestsParams, GetAllJoiningRequestsQuery>();

        CreateMap<GetAllJoiningRequestsModelResponse, GetAllJoiningRequestsResponse>();

        CreateMap<RejectJoiningRequestModelResponse, RejectJoiningRequestResponse>();
    }

    private void ConfigureSchoolProfileMappings()
    {
        CreateMap<CreateSchoolProfileRequest, CreateSchoolProfileCommand>();

        CreateMap<UpdateSchoolProfileRequest, UpdateSchoolProfileCommand>()
            .ForMember(command => command.Email, act => act.MapFrom(req => req.Email == null ? null : req.Email.ToLower()));

        CreateMap<SchoolProfileModelResponse, SchoolProfileResponse>();

        CreateMap<GetAllSchoolProfileBySchoolParams, GetAllSchoolProfilesBySchoolQuery>();

        CreateMap<GetAllSchoolProfilesBySchoolModelResponse, GetAllSchoolProfilesBySchoolResponse>();
    }

    private void ConfigureGroupMappings()
    {
        CreateMap<CreateGroupRequest, CreateGroupCommand>();

        CreateMap<UpdateGroupRequest, UpdateGroupCommand>();

        CreateMap<GroupModelResponse, GroupResponse>();

        CreateMap<GetAllGroupsParams, GetAllGroupsQuery>();

        CreateMap<GetAllGroupsModelResponse, GetAllGroupsResponse>();

        CreateMap<OneGroupModelResponse, OneGroupResponse>();
    }

    private void ConfigureStudyPeriodMappings()
    {
        CreateMap<CreateStudyPeriodRequest, CreateStudyPeriodCommand>();

        CreateMap<UpdateStudyPeriodRequest, UpdateStudyPeriodCommand>();

        CreateMap<StudyPeriodModelResponse, StudyPeriodResponse>();
    }

    private void ConfigureGroupNoticeMappings()
    {
        CreateMap<CreateGroupNoticeRequest, CreateGroupNoticeCommand>();

        CreateMap<UpdateGroupNoticeRequest, UpdateGroupNoticeCommand>();

        CreateMap<GroupNoticeModelResponse, GroupNoticeResponse>();

        CreateMap<GetAllGroupNoticesParams, GetAllGroupNoticesQuery>();

        CreateMap<GetAllGroupNoticesModelResponse, GetAllGroupNoticesResponse>();
    }

    private void ConfigureGlobalMappings()
    {
        CreateMap<Enum, string>().ConvertUsing(e => e.ToString().ToSnakeCase());
    }
}
