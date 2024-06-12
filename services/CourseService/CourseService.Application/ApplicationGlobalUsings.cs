﻿global using Amazon;
global using Amazon.Extensions.NETCore.Setup;
global using Amazon.Runtime;
global using Amazon.S3;
global using AutoMapper;
global using CourseService.Application.Common.Behaviors;
global using CourseService.Application.Common.DataContext;
global using CourseService.Application.Common.Files;
global using CourseService.Application.Common.SchoolProfile;
global using CourseService.Application.Course.CreateCourse;
global using CourseService.Application.Course.Models;
global using CourseService.Domain.Entities;
global using FluentValidation;
global using LanguageExt;
global using MassTransit;
global using MediatR;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.Caching.Memory;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Options;
global using Serilog;
global using Shared.Contracts.Errors;
global using Shared.Contracts.Errors.Invalid;
global using Shared.Contracts.Errors.Services;
global using Shared.Contracts.Files;
global using Shared.Contracts.Group.GetGroups;
global using Shared.Contracts.Options;
global using Shared.Contracts.SchoolProfile;
global using Shared.Contracts.SchoolProfile.GetActiveSchoolProfile;
global using Shared.Contracts.StudyPeriod.GetStudyPeriods;
global using Shared.Utils.File;
global using Shared.Utils.Mail;
global using System.Reflection;
