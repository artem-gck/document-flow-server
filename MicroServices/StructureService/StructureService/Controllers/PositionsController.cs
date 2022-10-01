﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StructureService.Application.Services;
using StructureService.Application.Services.Dto;
using StructureService.ViewModels;

namespace StructureService.Controllers
{
    [Route("positions")]
    [Produces("application/json")]
    public class PositionsController : Controller
    {
        private readonly IService<PositionDto> _positionsService;
        private readonly IMapper _controllerMapper;

        public PositionsController(IService<PositionDto> positionsService, IMapper controllerMapper)
        {
            _positionsService = positionsService ?? throw new ArgumentNullException(nameof(positionsService));
            _controllerMapper = controllerMapper ?? throw new ArgumentNullException(nameof(controllerMapper));
        }

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>PositionViewModel</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /positions/{id}
        ///
        /// </remarks>
        /// <response code="200">Model ok</response>
        /// <response code="404">Model not found</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PositionViewModel>> Get(int id)
        {
            var positionViewModel = _controllerMapper.Map<PositionViewModel>(await _positionsService.GetAsync(id));

            return Ok(positionViewModel);
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns>List of PositionViewModel</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /positions
        ///
        /// </remarks>
        /// <response code="200">Model ok</response>
        /// <response code="500">Internal server error</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<PositionViewModel>>> GetAll()
        {
            var listOfPositonViewModel = (await _positionsService.GetAllAsync()).Select(pos => _controllerMapper.Map<PositionViewModel>(pos));

            return Ok(listOfPositonViewModel);
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Status code</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE /positions/{id}
        ///
        /// </remarks>
        /// <response code="204">Model saved</response>
        /// <response code="404">Model not found</response>
        /// <response code="500">Internal server error</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _positionsService.DeleteAsync(id);

            return NoContent();
        }

        /// <summary>
        /// Posts the specified position view model.
        /// </summary>
        /// <param name="positionViewModel">The position view model.</param>
        /// <returns>Status code</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /positions
        ///     {
        ///        "name": "qwe"
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Model created</response>
        /// <response code="400">Invalid model state</response>
        /// <response code="409">Field is duplicated</response>
        /// <response code="500">Internal server error</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post(PositionViewModel positionViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _positionsService.AddAsync(_controllerMapper.Map<PositionDto>(positionViewModel));

            return Created($"positions/{result}", result);
        }

        /// <summary>
        /// Puts the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="positionViewModel">The position view model.</param>
        /// <returns>Status code</returns>
        /// /// <remarks>
        /// Sample request:
        ///
        ///     PUT /positions/{id}
        ///     {
        ///        "name": "qwe"
        ///     }
        ///
        /// </remarks>
        /// <response code="204">Model saved</response>
        /// <response code="400">Invalid model state</response>
        /// <response code="404">Model not found</response>
        /// <response code="409">Field is duplicated</response>
        /// <response code="500">Internal server error</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put(int id, PositionViewModel positionViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _positionsService.UpdateAsync(id, _controllerMapper.Map<PositionDto>(positionViewModel));

            return NoContent();
        }
    }
}