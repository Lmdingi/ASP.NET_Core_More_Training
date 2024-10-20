using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWorks.API.Models.Domain;
using NZWorks.API.Models.DTO;
using NZWorks.API.Repositories;

namespace NZWorks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IWalkRepository _walkRepository;

        public WalksController(IMapper mapper, IWalkRepository walkRepository)
        {
            _mapper = mapper;
            _walkRepository = walkRepository;
        }

        #region Create
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto)
        {
            if(ModelState.IsValid)
            {
                // Map DTO to Domain Model
                var walkDomainModel = _mapper.Map<Walk>(addWalkRequestDto);

                await _walkRepository.CreateAsync(walkDomainModel);

                var walkDto = _mapper.Map<WalkDto>(walkDomainModel);
                return Ok(walkDto);

            }
            else
            {
                return BadRequest(ModelState);
            }            
        }
        #endregion

        #region Get all Walks (Filter and Sort)
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? filterOn,[FromQuery] string? filterQuery, 
            [FromQuery] string? sortBy, [FromQuery] bool? isAscending, 
            [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000)
        {
            var walksDomainModel = await _walkRepository.GetAllAsync(filterOn, filterQuery, 
                sortBy, isAscending ?? true,
                pageNumber, pageSize);

            throw new Exception("This is a new Exception xxx");

            var walksDto = _mapper.Map<List<WalkDto>>(walksDomainModel);

            return Ok(walksDto);
        }
        #endregion

        #region Get Walk by id
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var walkDomainModel = await _walkRepository.GetByIdAsync(id);

            if(walkDomainModel == null)
            {
                return NotFound();
            }

            var walkDto = _mapper.Map<WalkDto>(walkDomainModel);
            return Ok(walkDto);
        }
        #endregion

        #region Update Walk by id
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, UpdateWalkRequestDto updateWalkRequestDto)
        {
            var walkDomainModel = _mapper.Map<Walk>(updateWalkRequestDto);
            walkDomainModel = await _walkRepository.UpdateAsync(id, walkDomainModel);

            if(walkDomainModel == null)
            {
                return NotFound();
            }

            var walkDto = _mapper.Map<WalkDto>(walkDomainModel);
            return Ok(walkDto);
        }
        #endregion

        #region Delete Walk by id
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        { 
            if(ModelState.IsValid)
            {
                var deletedWalkDomainModel = await _walkRepository.DeleteAsync(id);

                if (deletedWalkDomainModel == null)
                {
                    return NotFound();
                }

                var deleteWalkDto = _mapper.Map<Walk>(deletedWalkDomainModel);
                return Ok(deleteWalkDto);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        #endregion
    }
}
