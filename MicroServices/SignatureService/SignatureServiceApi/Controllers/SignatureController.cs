﻿using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SignatureService.Services.Interfaces;
using SignatureServiceApi.ViewModels;

namespace SignatureServiceApi.Controllers
{
    [ApiController]
    [Route("api/signatures")]
    [Authorize]
    public class SignatureController : Controller
    {
        private readonly ISignService _signService;

        public SignatureController(ISignService signService)
        {
            _signService = signService ?? throw new ArgumentNullException(nameof(signService));
        }

        /// <summary>
        /// Add signature for document.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="documentId">Document id</param>
        /// <param name="version">Version</param>
        /// <returns>Status code</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/signatures/{userId}/{documentId}/{version}
        ///
        /// </remarks>
        /// <response code="201">Signature created</response>
        /// <response code="500">Internal server error</response>
        [HttpPost("{userId}/{documentId}/{version}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post(Guid userId, Guid documentId, int version)
        {
            if (version < -1)
                return BadRequest("Version can't be less then -1");

            var token = Request.Headers["Authorization"].ToString();

            await _signService.AddAsync(userId, documentId, version, token);

            return Created($"/api/documents/{documentId}/{version}", new { DocumentId = documentId, Version = version });
        }

        /// <summary>
        /// Get users that signature document.
        /// </summary>
        /// <param name="documentId">Document id</param>
        /// <param name="version">Version</param>
        /// <returns>List of user id</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/documents/{documentId}/{version}
        ///
        /// </remarks>
        /// <response code="200">Users that sign this document</response>
        /// <response code="404">Not found users</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("~/api/documents/{documentId}/{version}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(Guid documentId, int version)
        {
            if (version < -1)
                return BadRequest("Version can't be less then -1");

            var users = await _signService.GetUsersByDocumentIdAsync(documentId, version);

            return Ok(users);
        }

        /// <summary>
        /// Check user that signature document.
        /// </summary>
        /// <param name="documentId">Document id</param>
        /// <param name="version">Version</param>
        /// <param name="publicKey">Puublic key</param>
        /// <returns>Status code</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/signatures/{userId}/{documentId}/{version}
        ///
        /// </remarks>
        /// <response code="200">Found matching</response>
        /// <response code="404">Not found document</response>
        /// <response code="500">Internal server error</response>
        [HttpPost("{documentId}/{version}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post([FromBody] CheckPublicKeyRequest publicKey, Guid documentId, int version)
        {
            if (version < -1)
                return BadRequest("Version can't be less then -1");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var token = Request.Headers["Authorization"].ToString();

            var result = await _signService.CheckDocumentByUserAsync(documentId, version, publicKey.Key, token);

            return result ? Ok() : NotFound();
        }
    }
}
