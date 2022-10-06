﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StructureService.Application.Services;
using StructureServiceApi.ViewModels;
using StructureService.Domain.Entities;

namespace StructureServiceApi.Controllers
{
    [Route("api/departments")]
    [Produces("application/json")]
    public class DepartmentController : Controller
    {
        private readonly IService<DepartmentEntity> _departmentsService;
        private readonly IMapper _controllerMapper;
        private readonly ILogger<DepartmentController> _logger;

        public DepartmentController(IService<DepartmentEntity> departmentsService, IMapper controllerMapper, ILogger<DepartmentController> logger)
        {
            _departmentsService = departmentsService ?? throw new ArgumentNullException(nameof(departmentsService));
            _controllerMapper = controllerMapper ?? throw new ArgumentNullException(nameof(controllerMapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>DepartmentViewModel</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /departments/{id}
        ///
        /// </remarks>
        /// <response code="200">Model ok</response>
        /// <response code="404">Model not found</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<DepartmentViewModel>> Get(Guid id)
        {
            var departmentViewModel = _controllerMapper.Map<DepartmentViewModel>(await _departmentsService.GetAsync(id));

            return Ok(departmentViewModel);
        }

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns>List of DepartmentViewModel</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /departments
        ///
        /// </remarks>
        /// <response code="200">Model ok</response>
        /// <response code="500">Internal server error</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<DepartmentViewModel>>> GetAll()
        {
            var listOfDepartmentViewModel = (await _departmentsService.GetAllAsync()).Select(dep => _controllerMapper.Map<DepartmentViewModel>(dep));

            return Ok(listOfDepartmentViewModel);
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Status code</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE /departments/{id}
        ///
        /// </remarks>
        /// <response code="204">Model saved</response>
        /// <response code="404">Model not found</response>
        /// <response code="500">Internal server error</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _departmentsService.DeleteAsync(id);

            return NoContent();
        }

        /// <summary>
        /// Posts the specified department view model.
        /// </summary>
        /// <param name="departmentViewModel">The position view model.</param>
        /// <returns>Status code</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /departments
        ///     {
        ///        "name": "qwe",
        ///        "cheifUserId": 1
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
        public async Task<IActionResult> Post(DepartmentViewModel departmentViewModel)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state {@ModelState}", ModelState);

                return BadRequest(ModelState);
            }

            var result = await _departmentsService.AddAsync(_controllerMapper.Map<DepartmentEntity>(departmentViewModel));

            return Created($"departments/{result}", result);
        }

        /// <summary>
        /// Puts the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="departmentViewModel">The position view model.</param>
        /// <returns>Status code</returns>
        /// /// <remarks>
        /// Sample request:
        ///
        ///     PUT /departments
        ///     {
        ///        "name": "qwe",
        ///        "cheifUserId": 1
        ///     }
        ///
        /// </remarks>
        /// <response code="204">Model saved</response>
        /// <response code="400">Invalid model state</response>
        /// <response code="404">Model not found</response>
        /// <response code="409">Field is duplicated</response>
        /// <response code="500">Internal server error</response>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put(Guid id, DepartmentViewModel departmentViewModel)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state {@ModelState}", ModelState);

                return BadRequest(ModelState);
            }

            var result = await _departmentsService.UpdateAsync(id, _controllerMapper.Map<DepartmentEntity>(departmentViewModel));

            return NoContent();
        }
    }
}