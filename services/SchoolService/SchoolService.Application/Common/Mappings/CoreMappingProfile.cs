﻿namespace SchoolService.Application.Common.Mappings;

public class CoreMappingProfile : Profile
{
    public CoreMappingProfile()
    {
        ConfigureGlobalMappings();

        ConfigureSchoolMapping();
    }

    private void ConfigureSchoolMapping()
    {
        CreateMap<CreateSchoolCommand, Domain.Entities.School>();

        CreateMap<UpdateSchoolCommand, Domain.Entities.School>();

        CreateMap<Domain.Entities.School, SchoolModelResponse>();
    }

    private void ConfigureGlobalMappings()
    {
        CreateMap<Stream?, string?>().ConvertUsing(_ => null);
    }
}