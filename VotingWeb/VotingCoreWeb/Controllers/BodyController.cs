﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VotingCommon;
using VotingCoreData;
using VotingCoreWeb.Infrastructure;
using VotingCoreWeb.Models.Api;
using VotingCoreWeb.Properties;

namespace VotingCoreWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BodyController : ControllerBase
    {
        private readonly ILogger<BodyController> _logger;
        private readonly ContextUtils _contextUtils;
        private readonly VotingDbContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly string serverEnvironment;

        public BodyController(ILogger<BodyController> logger, ContextUtils contextUtils, VotingDbContext dbContext, IConfiguration configuration, UserManager<IdentityUser> userManager)
        {
            this._logger = logger;
            this._contextUtils = contextUtils;
            this._dbContext = dbContext;
            this._userManager = userManager;
            this.serverEnvironment = configuration.GetValue<string>("ServerEnvironment");
        }

        // GET
        [HttpGet]
        public async Task<ActionResult<List<Body>>> GetAsync(int municipalityId)
        {
            try
            {
                var user = await _contextUtils.GetApiUserAsync(Request, _dbContext, _userManager);
                if (user == null)
                {
                    return StatusCode(403);
                }
                //Reading is allowed for every APIKey, if object exists
                var municipality = await _dbContext.GetMunicipalityByIdAsync(municipalityId);
                if (municipality == null)
                {
                    return StatusCode(404);
                }

                var list = await _dbContext.GetBodyListAsync(municipalityId);
                if (list == null)
                {
                    var errorResponse = new ErrorModel()
                    {
                        Message = ApiRes.ERROR_DB,
                        Code = Constants.ERROR_CODE_DB,
                        DebugMessage = _contextUtils.GetErrorMessage("LoadBodyAsync failed", serverEnvironment),
                    };
                    return StatusCode(500, errorResponse);
                }
                var model = list.ConvertAll(item => new Body(item));
                return model;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Failed");
                var errorResponse = new ErrorModel()
                {
                    Message = ApiRes.ERROR_GENERAL,
                    Code = Constants.ERROR_CODE_GENERAL,
                    DebugMessage = _contextUtils.GetErrorMessage(exception, serverEnvironment),
                };
                return StatusCode(500, errorResponse);
            }
        }

        // POST
        [HttpPost]
        public async Task<ActionResult<int>> PostAsync([FromBody] Body model)
        {
            try
            {
                var user = await _contextUtils.GetApiUserAsync(Request, _dbContext, _userManager);
                if (user == null)
                {
                    return StatusCode(403);
                }
                //Admin or editor claim is required for editation
                var hasRight = await _contextUtils.CheckMunicipalityRightAsync(model.MunicipalityId, user, _dbContext, _userManager);
                if (!hasRight)
                {
                    return StatusCode(403);
                }

                var newId = await _dbContext.UpdateBodyAsync(model.Id, model.ToDbModel(), null);
                if (newId.HasValue)
                {
                    return newId.Value;
                }
                var errorResponse = new ErrorModel()
                {
                    Message = ApiRes.ERROR_DB,
                    Code = Constants.ERROR_CODE_DB,
                    DebugMessage = _contextUtils.GetErrorMessage("UpdateBodyAsync failed", serverEnvironment),
                };
                return StatusCode(500, errorResponse);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Failed");
                var errorResponse = new ErrorModel()
                {
                    Message = ApiRes.ERROR_GENERAL,
                    Code = Constants.ERROR_CODE_GENERAL,
                    DebugMessage = _contextUtils.GetErrorMessage(exception, serverEnvironment),
                };
                return StatusCode(500, errorResponse);
            }
        }

        // DELETE
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            try
            {
                var user = await _contextUtils.GetApiUserAsync(Request, _dbContext, _userManager);
                if (user == null)
                {
                    return StatusCode(403);
                }
                var body = await _dbContext.GetBodyByIdAsync(id);
                if (body == null)
                {
                    return StatusCode(404);
                }
                //Admin or editor claim is required for editation
                var hasRight = await _contextUtils.CheckMunicipalityRightAsync(body.MunicipalityId, user, _dbContext, _userManager);
                if (!hasRight)
                {
                    return StatusCode(403);
                }

                var isDeleted = await _dbContext.DeleteBodyAsync(id);
                if (!isDeleted)
                {
                    var errorResponse = new ErrorModel()
                    {
                        Message = ApiRes.ERROR_DB,
                        Code = Constants.ERROR_CODE_DB,
                        DebugMessage = _contextUtils.GetErrorMessage("DeleteBodyAsync failed", serverEnvironment),
                    };
                    return StatusCode(500, errorResponse);
                }
                return Ok();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Failed");
                var errorResponse = new ErrorModel()
                {
                    Message = ApiRes.ERROR_GENERAL,
                    Code = Constants.ERROR_CODE_GENERAL,
                    DebugMessage = _contextUtils.GetErrorMessage(exception, serverEnvironment),
                };
                return StatusCode(500, errorResponse);
            }
        }
    }
}
