using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWorks.API.CustomActionFilters;
using NZWorks.API.Data;
using NZWorks.API.Models.Domain;
using NZWorks.API.Models.DTO;
using NZWorks.API.Repositories;
using System.Text.Json;

namespace NZWorks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDBContext _dbContext;

        private readonly IRegionRepository _RegionRepository;

        private readonly IMapper _mapper;
        private readonly ILogger<RegionsController> _logger;

        public RegionsController(NZWalksDBContext dbContext, IRegionRepository regionRepository, IMapper mapper, ILogger<RegionsController> logger)
        {
            _dbContext = dbContext;
            _RegionRepository = regionRepository;
            _mapper = mapper;
            _logger = logger;
        }

        #region GetAll
        [HttpGet]
        //[Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                //throw new Exception("Custom Exception XXXXXXXX");
                var regionsDomain = await _RegionRepository.GetAllAsync();
                _logger.LogInformation("GetAll stated =>");

                //var regionsDTO = new List<RegionDTO>();
                //foreach (var region in regionsDomain)
                //{
                //    regionsDTO.Add(new RegionDTO()
                //    {
                //        Id = region.Id,
                //        Name = region.Name,
                //        Code = region.Code,
                //        RegionImageUrl = region.RegionImageUrl,
                //    });
                //}

                var regionsDTO = _mapper.Map<List<RegionDTO>>(regionsDomain);
                _logger.LogInformation($"GetAll finished with: {JsonSerializer.Serialize(regionsDTO)}");

                return Ok(regionsDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region GetById
        [HttpGet]
        [Route("{id:Guid}")]
        //[Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetById([FromRoute] Guid  id)
        {
            var regionDomain = await _RegionRepository.GetByIdAsync(id);

            if(regionDomain == null)
            {
                return NotFound();
            }

            //var regionsDTO = new RegionDTO
            //{
            //    Id = regionDomain.Id,
            //    Name = regionDomain.Name,
            //    Code = regionDomain.Code,
            //    RegionImageUrl = regionDomain.RegionImageUrl,
            //};

            var regionsDTO = _mapper.Map<RegionDTO>(regionDomain);

            return Ok(regionsDTO);
        }
        #endregion

        #region Create
        [HttpPost]
        [ValidateModel]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDTO addRegionRequestDto)
        {
            var regionDomainModel = _mapper.Map<Region>(addRegionRequestDto);
            regionDomainModel = await _RegionRepository.CreateAsync(regionDomainModel);
            var regionDto = _mapper.Map<RegionDTO>(regionDomainModel);
            return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
            
        }
        #endregion

        #region HttpPut = Update
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionDto updateRegionDto)
        {
            var regionDomainModel = _mapper.Map<Region>(updateRegionDto);

            regionDomainModel = await _RegionRepository.UpdateAsync(id, regionDomainModel);

            if (regionDomainModel == null)
            {
                return NotFound();
            }

            var regionsDTO = _mapper.Map<RegionDTO>(regionDomainModel);

            return Ok(regionsDTO);
        }
        #endregion

        #region Delete
        [HttpDelete]
        [Route("{id:Guid}")]
        //[Authorize(Roles = "Writer,Reader")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            if(ModelState.IsValid)
            {var regionDomainModel = await _RegionRepository.DeleteAsync(id);
                if (regionDomainModel == null)
                {
                    return NotFound();
                }

            var regionsDTO = _mapper.Map<RegionDTO>(regionDomainModel);

                return Ok(regionsDTO);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        #endregion
    }
}
