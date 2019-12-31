using Microsoft.AspNetCore.Identity;
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
    public class MunicipalityController : ControllerBase
    {
        private readonly ILogger<MunicipalityController> _logger;
        private readonly ContextUtils _contextUtils;
        private readonly VotingDbContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly string serverEnvironment;

        public MunicipalityController(ILogger<MunicipalityController> logger, ContextUtils contextUtils, VotingDbContext dbContext, IConfiguration configuration, UserManager<IdentityUser> userManager)
        {
            this._logger = logger;
            this._contextUtils = contextUtils;
            this._dbContext = dbContext;
            this._userManager = userManager;
            this.serverEnvironment = configuration.GetValue<string>("ServerEnvironment");
        }

        // GET
        [HttpGet]
        public async Task<ActionResult<List<Municipality>>> GetAsync()
        {
            try
            {
                var user = await _contextUtils.GetApiUserAsync(Request, _dbContext, _userManager);
                if (user == null)
                {
                    return StatusCode(403);
                }

                var list = await _dbContext.LoadMunicipalitiesAsync();
                if (list == null)
                {
                    var errorResponse = new ErrorModel()
                    {
                        Message = ApiRes.ERROR_DB,
                        Code = Constants.ERROR_CODE_DB,
                        DebugMessage = _contextUtils.GetErrorMessage("LoadMunicipalitiesAsync failed", serverEnvironment),
                    };
                    return StatusCode(500, errorResponse);
                }
                var model = list.ConvertAll(item => new Municipality(item));
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
        public async Task<ActionResult<int>> PostAsync([FromBody] Municipality model)
        {
            try
            {
                var user = await _contextUtils.GetApiUserAsync(Request, _dbContext, _userManager);
                if (user == null)
                {
                    return StatusCode(403);
                }
                //Admin is required
                var isInRole = await _userManager.IsInRoleAsync(user, Constants.ROLE_ADMIN);
                if (!isInRole)
                {
                    return StatusCode(403);
                }

                var newId = await _dbContext.UpdateMunicipalityAsync(model.Id, model.ToDbModel());
                if (newId.HasValue)
                {
                    return newId.Value;
                }
                var errorResponse = new ErrorModel()
                {
                    Message = ApiRes.ERROR_DB,
                    Code = Constants.ERROR_CODE_DB,
                    DebugMessage = _contextUtils.GetErrorMessage("UpdateMunicipalityAsync failed", serverEnvironment),
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
                //Admin is required
                var isInRole = await _userManager.IsInRoleAsync(user, Constants.ROLE_ADMIN);
                if (!isInRole)
                {
                    return StatusCode(403);
                }

                var isDeleted = await _dbContext.DeleteMunicipalityAsync(id);
                if (!isDeleted)
                {
                    var errorResponse = new ErrorModel()
                    {
                        Message = ApiRes.ERROR_DB,
                        Code = Constants.ERROR_CODE_DB,
                        DebugMessage = _contextUtils.GetErrorMessage("DeleteMunicipalityAsync failed", serverEnvironment),
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
