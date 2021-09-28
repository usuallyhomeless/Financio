using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinancioAPI.Models;
using FinancioAPI.ViewModels;

namespace FinancioAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FinanciousersController : ControllerBase
    {
        private readonly financioContext _context;

        public FinanciousersController(financioContext context)
        {
            _context = context;
        }

        // * GET: /api/financiousers
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Financiouser>>> GetFinanciouser()
        {
            Financiouser SubjectUser = (Financiouser)HttpContext.Items["User"];
            if (SubjectUser.Isadmin == null || SubjectUser.Isadmin == false) return null;
            return await _context.Financiouser.ToListAsync();
        }       

        // * GET: api/financiousers/dashboard/

        [HttpGet("dashboard")]
        [Authorize]
        public Object GetDashboardDetails()
        {
            Financiouser SubjectUser = (Financiouser)HttpContext.Items["User"];
            var details = (from userobj in this._context.Financiouser
                           join cardobj in this._context.Card
                           on userobj.Id equals cardobj.Financiouser
                           where userobj.Id == SubjectUser.Id
                           select new
                           {
                               UserID = SubjectUser.Id,
                               UserName = userobj.Name,
                               Cardnumber = cardobj.Id,
                               Validupto = cardobj.Validupto,
                               Cardname = cardobj.Cardtype.Cardname,
                               isCardActive = cardobj.Isactive,
                               TotalCredit = cardobj.Cardtype.Creditlimit,
                               CardLimit = cardobj.Cardlimit
                           }).FirstOrDefault();

            return details;
        }
        

        // * POST: /api/financiousers/createuserandcard

        [HttpPost("createuserandcard")]
        public ServerJsonResponse CreateUserAndCard(UserAndCard userandcard)
        {
            using (var transaction = this._context.Database.BeginTransaction())
            {
                try
                {
                    Financiouser UserObj = new Financiouser { Name = userandcard.Name, Phone = userandcard.Phone, Email = userandcard.Email, Username = userandcard.Username, Password = userandcard.Password, Dob = userandcard.Dob, Address = userandcard.Address };
                    this._context.Financiouser.Add(UserObj);
                    this._context.SaveChanges();
                    Cardtype CardTypeSelected = (from x in this._context.Cardtype where x.Id == userandcard.Cardtype select x).FirstOrDefault();
                    Card Usercard = new Card { Financiouser = UserObj.Id, Registrationdate = DateTime.Today, Validupto = DateTime.Today.AddYears(6), Cardlimit = CardTypeSelected.Creditlimit, Isactive = false, Cardtypeid = CardTypeSelected.Id, Bankid = userandcard.Bank, Accountnumber = userandcard.Accountnumber, Ifscid = userandcard.Ifsc };
                    this._context.Add(Usercard);
                    this._context.SaveChanges();
                    transaction.Commit();
                    return new ServerJsonResponse { Status = 200, Message = "User Successfully Registered" };
                }
                catch (Exception e)
                {
                    return new ServerJsonResponse { Status = 400, Message = "Username or Email or Saving account number not unique internal server error. Try again!" };
                }

            }
        }

        // * GET : /api/financiousers/completedetails/34

        [HttpGet("completeDetails/{userid}")]
        [Authorize]
        public CompleteUserDetail GetCompleteDetails(int userid)
        {
            Financiouser SubjectUser = (Financiouser)HttpContext.Items["User"];
            if (SubjectUser.Isadmin == null || SubjectUser.Isadmin == false) return null;
            return (from cardobj in this._context.Card 
                    join userobj in this._context.Financiouser
                    on cardobj.Financiouser
                    equals userobj.Id
                    where userobj.Id == userid
                    select new CompleteUserDetail
                    {
                        Name = userobj.Name,
                        Dob = userobj.Dob,
                        Email = userobj.Email,
                        Phone = userobj.Phone,
                        Username = userobj.Username,
                        Address = userobj.Address,
                        CardName = cardobj.Cardtype.Cardname,
                        Bankname = cardobj.Bank.Bankname,
                        Ifcscode = cardobj.Ifsc.Ifsccode,
                        Isactive = cardobj.Isactive,
                        Accountnumber = cardobj.Accountnumber
                    }).FirstOrDefault();
        }

        // * GET api/financiousers/isloggedin

        [HttpGet("isloggedin")]
        [Authorize]
        public ServerJsonResponse CheckIsLoggedIn()
        {
            return new ServerJsonResponse() { Status = 200, Message = "logged in" };
        }


        // * GET api/financiousers/getuserdetailsbyauth

        [HttpGet("getuserdetailsbyauth")]
        [Authorize]
        public CompleteUserDetail GetCompleyeUserDetailsByAuth()
        {
            Financiouser SubjectUser = (Financiouser)HttpContext.Items["User"];
            if (SubjectUser.Isadmin == true)
            {
                return new CompleteUserDetail
                {
                    Name = SubjectUser.Name,
                    Dob = SubjectUser.Dob,
                    Email = SubjectUser.Email,
                    Phone = SubjectUser.Phone,
                    Username = SubjectUser.Username,
                    Address = SubjectUser.Address,
                    Isadmin = SubjectUser.Isadmin,
                };
            }

            return (from cardobj in this._context.Card
                    join userobj in this._context.Financiouser
                    on cardobj.Financiouser
                    equals userobj.Id
                    where userobj.Id == SubjectUser.Id
                    select new CompleteUserDetail
                    {
                        Name = userobj.Name,
                        Dob = userobj.Dob,
                        Email = userobj.Email,
                        Phone = userobj.Phone,
                        Username = userobj.Username,
                        Address = userobj.Address,
                        CardName = cardobj.Cardtype.Cardname,
                        Bankname = cardobj.Bank.Bankname,
                        Ifcscode = cardobj.Ifsc.Ifsccode,
                        Accountnumber = cardobj.Accountnumber
                    }).FirstOrDefault();
        }

        // * POST: api/financiousers/change-password
        [HttpPost]
        [Authorize]
        [Route("change-password")]
        public ServerJsonResponse ChangePassword(ChangePassword model)
        {
            var user = (Financiouser)HttpContext.Items["User"];
            if (user == null)
            {
                return new ServerJsonResponse { Status= 404, Message = "User not found"};
            }
            if (user.Password.Equals(model.currentPassword) && model.newPassword.Equals(model.confirmNewPassword))
            {
                user.Password = model.newPassword;
                _context.Financiouser.Update(user);
                this._context.SaveChanges();
                return new ServerJsonResponse { Status = 200, Message = "Operation Successful" };

            }
            else
            {
                return new ServerJsonResponse { Status = 400, Message = "Password Incorrect" };
            }
        }
    }
}
