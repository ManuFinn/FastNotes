﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System;
using FastNotesAPI.Models;
using FastNotesAPI.Repositories;

namespace FastNotesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        public IConfiguration Configuration { get; }

        public LoginRepository repository;



        public LoginController(IConfiguration configuration)
        {
            Configuration = configuration;
        }   

        [HttpPost]
        public IActionResult Post(LoginModel model)
        {

            if(model.User == "Admin" && model.Password == "123456789")
            {
                List<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.Name, "Admin"));
                claims.Add(new Claim(ClaimTypes.Role, "1"));
                claims.Add(new Claim("id","1"));
                claims.Add(new Claim(JwtRegisteredClaimNames.Iss, Configuration["JwtAuth:Issuer"]));
                claims.Add(new Claim(JwtRegisteredClaimNames.Exp, DateTime.UtcNow.AddMinutes(5).ToString()));

                var handler = new JwtSecurityTokenHandler();

                SecurityTokenDescriptor des = new SecurityTokenDescriptor();
                des.Issuer = Configuration["JwtAuth:Issuer"];
                des.Audience = Configuration["JwtAuth:Audience"];
                des.Subject = new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme);
                des.Expires = DateTime.UtcNow.AddMinutes(5);
                des.SigningCredentials = new SigningCredentials(new SymmetricSecurityKey
                    (Encoding.UTF8.GetBytes(Configuration["JwtAuth:Key"])), SecurityAlgorithms.HmacSha256);

                var token = handler.CreateToken(des);

                return Ok(handler.WriteToken(token));
            }
            else { return Unauthorized("Datos de inicio de sesion incorrectos."); }
        }
    }
}
