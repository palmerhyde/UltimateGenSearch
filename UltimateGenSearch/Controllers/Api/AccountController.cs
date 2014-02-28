using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UltimateGenSearch.Controllers.Api
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    using UltimateGenSearch.Services;
    using UltimateGenSearch.Services.Login;

    public class AccountController : ApiController
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            if (accountService == null)
                throw new ArgumentNullException("accountService");

            this._accountService = accountService;
        }

        public HttpResponseMessage Get()
        {
            try
            {
                var result = _accountService.GetAccounts();

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        public HttpResponseMessage Post(Account account)
        {
            try
            {
                var res = _accountService.UpdateAccount(account);

                return Request.CreateResponse(HttpStatusCode.OK, res);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
           
        }

    }
}
