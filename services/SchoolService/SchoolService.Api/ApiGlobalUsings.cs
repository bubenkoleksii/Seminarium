﻿global using System.Security.Claims;
global using System.Security.Principal;
global using AutoMapper;
global using CaseExtensions;
global using MediatR;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Mvc.Filters;
global using Microsoft.Extensions.Options;
global using SchoolService.Api.Identity;
global using SchoolService.Api.Mappings;
global using SchoolService.Api.Models.Group;
global using SchoolService.Api.Models.JoiningRequest;
global using SchoolService.Api.Models.School;
global using SchoolService.Api.Models.SchoolProfile;
global using SchoolService.Api.Options;
global using SchoolService.Application;
global using SchoolService.Application.Common.Mappings;
global using SchoolService.Application.Group.Commands.CreateClassTeacherInvitation;
global using SchoolService.Application.Group.Commands.CreateGroup;
global using SchoolService.Application.Group.Commands.CreateStudentInvitation;
global using SchoolService.Application.Group.Commands.DeleteGroupImage;
global using SchoolService.Application.Group.Commands.SetGroupImage;
global using SchoolService.Application.Group.Models;
global using SchoolService.Application.Group.Queries.GetAllGroups;
global using SchoolService.Application.JoiningRequest.Commands.CreateJoiningRequest;
global using SchoolService.Application.JoiningRequest.Commands.RejectJoiningRequest;
global using SchoolService.Application.JoiningRequest.Models;
global using SchoolService.Application.JoiningRequest.Queries.GetAllJoiningRequests;
global using SchoolService.Application.JoiningRequest.Queries.GetOneJoiningRequest;
global using SchoolService.Application.School.Commands.CreateInvitation;
global using SchoolService.Application.School.Commands.CreateSchool;
global using SchoolService.Application.School.Commands.CreateTeacherInvitation;
global using SchoolService.Application.School.Commands.DeleteSchool;
global using SchoolService.Application.School.Commands.DeleteSchoolImage;
global using SchoolService.Application.School.Commands.SetSchoolImage;
global using SchoolService.Application.School.Commands.UpdateSchool;
global using SchoolService.Application.School.Models;
global using SchoolService.Application.School.Queries.GetAllSchools;
global using SchoolService.Application.School.Queries.GetOneSchool;
global using SchoolService.Application.SchoolProfile.Commands.CreateSchoolProfile;
global using SchoolService.Application.SchoolProfile.Commands.DeleteSchoolProfileImage;
global using SchoolService.Application.SchoolProfile.Commands.SetSchoolProfileImage;
global using SchoolService.Application.SchoolProfile.Models;
global using SchoolService.Application.SchoolProfile.Queries.GetUserSchoolProfiles;
global using SchoolService.Application.SchoolProfile.SchoolProfileManager;
global using SchoolService.Infrastructure.Persistence;
global using Serilog;
global using Serilog.Events;
global using Shared.Contracts.Errors;
global using Shared.Contracts.Errors.Invalid;
global using Shared.Contracts.Files;
global using Shared.Contracts.Options;
global using Shared.Utils.Errors;
global using Shared.Utils.File;
