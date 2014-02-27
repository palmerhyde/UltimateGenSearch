using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using UltimateGenSearch.Models;
using UltimateGenSearch.Services;

namespace UltimateGenSearch.Controllers.Api
{
    public class SearchController : ApiController
    {
        private readonly ISearchService _searchService;

        public SearchController(ISearchService searchService)
        {
            if (searchService == null)
                throw new ArgumentNullException("searchService");

            this._searchService = searchService;
        }

        public HttpResponseMessage Get(Query query)
        {
            try
            {
                var result = _searchService.Search(query);

                return Request.CreateResponse(HttpStatusCode.OK, result);
                
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }
      
    }
}
