﻿global using System.Reflection;
global using System.Security.Cryptography;
global using System.Text;
global using Amazon;
global using Amazon.Extensions.NETCore.Setup;
global using Amazon.Runtime;
global using Amazon.S3;
global using AutoMapper;
global using FluentValidation;
global using LanguageExt;
global using MassTransit;
global using MediatR;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.Caching.Memory;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Options;
global using Newtonsoft.Json;
global using SchoolService.Application.Common.Behaviors;
global using SchoolService.Application.Common.Cryptography.Aes;
global using SchoolService.Application.Common.DataContext;
global using SchoolService.Application.Common.Email;
global using SchoolService.Application.Common.Files;
global using SchoolService.Application.Group.Commands.CreateGroup;
global using SchoolService.Application.Group.Models;
global using SchoolService.Application.JoiningRequest.Commands.CreateJoiningRequest;
global using SchoolService.Application.JoiningRequest.Models;
global using SchoolService.Application.School.Commands.CreateSchool;
global using SchoolService.Application.School.Commands.UpdateSchool;
global using SchoolService.Application.School.Common;
global using SchoolService.Application.School.Models;
global using SchoolService.Application.SchoolProfile.Commands.CreateSchoolProfile;
global using SchoolService.Application.SchoolProfile.Common.Invitation;
global using SchoolService.Application.SchoolProfile.Models;
global using SchoolService.Application.SchoolProfile.SchoolProfileManager;
global using SchoolService.Domain.Enums.JoiningRequest;
global using SchoolService.Domain.Enums.School;
global using SchoolService.Domain.Enums.SchoolProfile;
global using Serilog;
global using Shared.Contracts.Errors;
global using Shared.Contracts.Errors.AlreadyExists;
global using Shared.Contracts.Errors.AlreadyExists.School;
global using Shared.Contracts.Errors.Invalid;
global using Shared.Contracts.Errors.NotFound;
global using Shared.Contracts.Files;
global using Shared.Contracts.Options;
global using Shared.Utils.File;
global using Shared.Utils.Mail;
