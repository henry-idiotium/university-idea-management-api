global using System.ComponentModel.DataAnnotations;
global using System.ComponentModel.DataAnnotations.Schema;
global using System.IdentityModel.Tokens.Jwt;
global using System.Net;
global using System.Security.Claims;
global using System.Security.Cryptography;
global using System.Text;
global using AutoMapper;
global using Google.Apis.Auth;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Identity;
global using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Mvc.Filters;
global using Microsoft.AspNetCore.WebUtilities;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Infrastructure;
global using Microsoft.EntityFrameworkCore.Migrations;
global using Microsoft.Extensions.Options;
global using Microsoft.IdentityModel.Tokens;
global using Microsoft.OpenApi.Models;
global using Newtonsoft.Json;
global using Newtonsoft.Json.Serialization;
global using Sieve.Models;
global using Sieve.Services;
global using Swashbuckle.AspNetCore.SwaggerGen;
// In-Project namespaces
global using UIM.Core.Common;
global using UIM.Core.Common.Controller;
global using UIM.Core.Common.Entity;
global using UIM.Core.Common.Repository;
global using UIM.Core.Common.UnitOfWork;
global using UIM.Core.Data;
global using UIM.Core.Data.Repositories;
global using UIM.Core.Data.Repositories.Interfaces;
global using UIM.Core.Helpers;
global using UIM.Core.Helpers.Attributes;
global using UIM.Core.Helpers.Mappers;
global using UIM.Core.Helpers.SieveExtensions;
global using UIM.Core.Helpers.SieveExtensions.Configurations;
global using UIM.Core.Middlewares;
global using UIM.Core.Models.Dtos;
global using UIM.Core.Models.Dtos.Attachment;
global using UIM.Core.Models.Dtos.Auth;
global using UIM.Core.Models.Dtos.Comment;
global using UIM.Core.Models.Dtos.Department;
global using UIM.Core.Models.Dtos.Idea;
global using UIM.Core.Models.Dtos.Role;
global using UIM.Core.Models.Dtos.Submission;
global using UIM.Core.Models.Dtos.Tag;
global using UIM.Core.Models.Dtos.Token;
global using UIM.Core.Models.Dtos.User;
global using UIM.Core.Models.Entities;
global using UIM.Core.ResponseMessages;
global using UIM.Core.Services;
global using UIM.Core.Services.Interfaces;
