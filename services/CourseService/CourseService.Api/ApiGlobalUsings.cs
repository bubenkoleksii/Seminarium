﻿global using AutoMapper;
global using CaseExtensions;
global using CourseService.Api.Attachments;
global using CourseService.Api.Identity;
global using CourseService.Api.Mappings;
global using CourseService.Api.Models.Course;
global using CourseService.Api.Models.Lesson;
global using CourseService.Api.Models.LessonItem.PracticalLessonItem;
global using CourseService.Api.Models.LessonItem.TheoryLessonItem;
global using CourseService.Api.Models.LessonItems.PracticalLessonItems;
global using CourseService.Api.Models.PracticalLessonItemSubmit;
global using CourseService.Api.Options;
global using CourseService.Application;
global using CourseService.Application.Common.Attachments.Models;
global using CourseService.Application.Common.Mappings;
global using CourseService.Application.Course.Commands.CreateCourse;
global using CourseService.Application.Course.Commands.DeleteCourse;
global using CourseService.Application.Course.Commands.UpdateCourse;
global using CourseService.Application.Course.Models;
global using CourseService.Application.Lesson.Commands.CreateLesson;
global using CourseService.Application.Lesson.Commands.DeleteLesson;
global using CourseService.Application.Lesson.Commands.UpdateLesson;
global using CourseService.Application.Lesson.Models;
global using CourseService.Application.LessonItem.Commands.PracticalLessonItem.CreatePracticalLessonItem;
global using CourseService.Application.LessonItem.Commands.PracticalLessonItem.DeletePracticalLessonItem;
global using CourseService.Application.LessonItem.Commands.PracticalLessonItem.UpdatePracticalLessonItem;
global using CourseService.Application.LessonItem.Commands.TheoryLessonItem.CreateTheoryLessonItem;
global using CourseService.Application.LessonItem.Commands.TheoryLessonItem.DeleteTheoryLessonItem;
global using CourseService.Application.LessonItem.Commands.TheoryLessonItem.UpdateTheoryLessonItem;
global using CourseService.Application.LessonItem.Models;
global using CourseService.Application.PracticalLessonItemSubmit.Commands.CreatePracticalLessonItemSubmit;
global using CourseService.Application.PracticalLessonItemSubmit.Commands.DeletePracticalLessonItemSubmit;
global using CourseService.Application.PracticalLessonItemSubmit.Commands.UpdatePracticalLessonItemSubmit;
global using CourseService.Application.PracticalLessonItemSubmit.Models;
global using CourseService.Infrastructure.Persistence;
global using LanguageExt;
global using MediatR;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.Extensions.Options;
global using Serilog;
global using Serilog.Events;
global using Shared.Contracts.Errors;
global using Shared.Contracts.Errors.Invalid;
global using Shared.Contracts.Group;
global using Shared.Contracts.Options;
global using Shared.Contracts.SchoolProfile;
global using Shared.Utils.Errors;
global using Shared.Utils.File;
global using System.Security.Claims;
global using System.Security.Principal;
