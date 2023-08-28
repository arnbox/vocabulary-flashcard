// Microsoft
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.AspNetCore.Authorization;

// System
global using System.Text.Json;

// Data models
global using VocabularyFlashCard.Core.ViewModels;
global using VocabularyFlashCard.Core.Models;
global using VocabularyFlashCard.Core.Internal;

// Extension
global using VocabularyFlashCard.Infrastructure.Extensions;

// Repository
global using VocabularyFlashCard.DataService.Repository;
global using VocabularyFlashCard.DataService.Repository.EntityFramework;
global using VocabularyFlashCard.DataService.Repository.Interfaces;

// Services
global using VocabularyFlashCard.DataService.Services;
global using VocabularyFlashCard.DataService.Services.Interfaces;
global using VocabularyFlashCard.Web.Services;
global using VocabularyFlashCard.Web.Middlewares;

// NLog
global using NLog;
global using NLog.Web;