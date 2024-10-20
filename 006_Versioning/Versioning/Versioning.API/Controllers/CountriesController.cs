using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Versioning.API.Models.DTOs;

namespace Versioning.API.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    public class CountriesController : ControllerBase
    {
        #region version 1
        [MapToApiVersion("1.0")]
        [HttpGet]
        public IActionResult GetV1()
        {
            var countriesDomainModel = CountriesData.Get();

            var response = new List<CountryDtoV1>();
            foreach (var countryDomain in countriesDomainModel)
            {
                response.Add(new CountryDtoV1
                {
                    Id = countryDomain.Id,
                    Name = countryDomain.Name,
                });
            }

            return Ok(response);
        }
        #endregion

        #region version 2
        [MapToApiVersion("2.0")]
        [HttpGet]
        public IActionResult GetV2()
        {
            var countriesDomainModel = CountriesData.Get();

            var response = new List<CountryDtoV2>();
            foreach (var countryDomain in countriesDomainModel)
            {
                response.Add(new CountryDtoV2
                {
                    Id = countryDomain.Id,
                    CountryName = countryDomain.Name,
                });
            }

            return Ok(response);
        }
        #endregion
    }
}
