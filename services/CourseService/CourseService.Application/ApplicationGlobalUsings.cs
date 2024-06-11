﻿global using Amazon;
global using Amazon.Extensions.NETCore.Setup;
global using Amazon.Runtime;
global using Amazon.S3;
global using CourseService.Application.Common.Behaviors;
global using CourseService.Application.Common.Files;
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
global using Shared.Contracts.Files;
global using Shared.Contracts.Options;
global using Shared.Utils.File;
global using Shared.Utils.Mail;
global using System.Reflection;
