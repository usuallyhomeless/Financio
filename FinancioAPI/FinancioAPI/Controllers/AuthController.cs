using FinancioAPI.Entities;
using FinancioAPI.Models;
using FinancioAPI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancioAPI.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly financioContext _context;
        private readonly IMailService mailService;

        public object EncryptString { get; private set; }

        public AuthController(IUserService _userService, financioContext context, IMailService mailService)
        {
            userService = _userService;
            this._context = context;
            this.mailService = mailService;
        }

        // * POST : /api/auth/login 

        [HttpPost("login")]        
        public ServerJsonResponse Login(AuthenticateRequest model)
        {
            var response = this.userService.Authenticate(model);
            if (response == null) return new ServerJsonResponse { Status = 400, Message = "Username or password incorrect" };
            return new ServerJsonResponse{ Status = 200, Message = response.Token }; 
        }

        // * POST api/auth/forgot-password/rahulsanghvi18@gmail.com
        [HttpPost("forgot-password/{email}")]
        public async Task<ServerJsonResponse> ForgotPassword(string email)
        {
            try
            {
                Financiouser SubjectUser = this._context.Financiouser.Where(x => x.Email == email).FirstOrDefault();
                if (SubjectUser == null)
                {
                    return new ServerJsonResponse { Status = 404, Message = "User with email not found" };
                }

                String token = StringProcessors.EncryptString(SubjectUser.Username);
                try
                {
                    await this.mailService.SendEmailAsync(new MailRequest { ToEmail = SubjectUser.Email, Subject = "Password Reset", Body = "Password reset token : " + token });
                    return new ServerJsonResponse { Status = 200, Message = "Password reset token send. Please Check your mail" };
                }
                catch (Exception e)
                {
                    return new ServerJsonResponse { Status = 500, Message = "Something went wrong please try again" };
                }
            }catch(Exception e)
            {
                return new ServerJsonResponse { Status = 500, Message = "Something Went wrong try again!" };
            }
            
        }

        // * POST api/auth/reset-password
        [HttpPost("reset-password")]
        public ServerJsonResponse ResetPassword(ResetPassword passwordObj)
        {
            try
            {
                Financiouser SubjectUser = this._context.Financiouser.Where(x => x.Email == passwordObj.Email).FirstOrDefault();
                if (SubjectUser == null)
                {
                    return new ServerJsonResponse { Status = 404, Message = "Username not found" };
                }
                string decryptToken = StringProcessors.DecryptString(passwordObj.Token);
                if (decryptToken.Equals(SubjectUser.Username))
                {
                    SubjectUser.Password = passwordObj.Password;
                    this._context.SaveChanges();
                    return new ServerJsonResponse { Status = 200, Message = "Operation Successful" };
                }
                else
                {
                    return new ServerJsonResponse { Status = 400, Message = "Invalid Token. Please try again" };
                }
            }
            catch(Exception e)
            {
                return new ServerJsonResponse { Status = 500, Message = "Something Went wrong try again!" };
            }            
        }
    }
}
