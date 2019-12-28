using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using VotingCoreData;
using VotingCoreWeb.Infrastructure;
using VotingCoreWeb.Properties;
using VotingImporter;

namespace VotingCoreWeb.Areas.Admin.Pages.Import
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly VotingDbContext _dbContext;
        private readonly ContextUtils _contextUtils;
        private readonly IWebHostEnvironment _environment;

        [BindProperty]
        public int MunicipalityId { get; set; }

        [Display(Name = "DETAIL_BODY", ResourceType = typeof(ImportAdminRes))]
        [Required(ErrorMessageResourceName = "VALIDATION_EMPTY", ErrorMessageResourceType = typeof(AdminRes))]
        [BindProperty]
        public int BodyId { get; set; }

        public SelectList BodyList { get; set; }

        [Display(Name = "DETAIL_IMPORTER", ResourceType = typeof(ImportAdminRes))]
        [Required(ErrorMessageResourceName = "VALIDATION_EMPTY", ErrorMessageResourceType = typeof(AdminRes))]
        [BindProperty]
        public int Importer { get; set; }

        public SelectList ImporterList { get; set; }

        [Display(Name = "DETAIL_UPLOAD", ResourceType = typeof(ImportAdminRes))]
        [Required(ErrorMessageResourceName = "VALIDATION_EMPTY", ErrorMessageResourceType = typeof(AdminRes))]
        [BindProperty]
        public IFormFile Upload { get; set; }

        [BindProperty]
        public string Filename { get; set; }

        public VotingCommon.Models.SessionModel SessionModel { get; set; }

        public AlertModel Alert { get; set; }

        public IndexModel(ILogger<IndexModel> logger, VotingDbContext dbContext, ContextUtils contextUtils, IWebHostEnvironment environment)
        {
            _logger = logger;
            _dbContext = dbContext;
            _contextUtils = contextUtils;
            _environment = environment;
        }

        private async Task PrepareSelectListsAsync()
        {
            this.ImporterList = new SelectList(ImporterConvert.GetImporterList(), "Item1", "Item2", null);
            var bodyList = await _dbContext.LoadBodiesAsync(this.MunicipalityId);
            this.BodyList = new SelectList(bodyList, "Id", "Name");
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            try
            {
                var claimId = await _contextUtils.CheckMunicipalityRightsAsync(id, User, _dbContext, TempData);
                if (claimId == null)
                {
                    _contextUtils.CreateActionStateCookie(TempData, AlertTypeEnum.Danger, AdminRes.ERROR_NO_RIGHT);
                    return RedirectToPage("/Index", new { area = "" });
                }
                this.MunicipalityId = claimId.Value;
                await PrepareSelectListsAsync();
                return Page();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Import failed");
                _contextUtils.CreateActionStateCookie(TempData, AlertTypeEnum.Danger, AdminRes.ERROR_EXCEPTION);
                return RedirectToPage("/Index", new { area = "" });
            }
        }

        public async Task<IActionResult> OnPostUploadAsync()
        {
            try
            {
                var claimId = await _contextUtils.CheckMunicipalityRightsAsync(this.MunicipalityId, User, _dbContext, TempData);
                if (claimId == null)
                {
                    _contextUtils.CreateActionStateCookie(TempData, AlertTypeEnum.Danger, AdminRes.ERROR_NO_RIGHT);
                    return RedirectToPage("/Index", new { area = "" });
                }
                if (ModelState.IsValid)
                {
                    // store the file inside ~/uploads folder
                    var guidName = Guid.NewGuid().ToString();
                    var path = Path.Combine(_environment.ContentRootPath, "uploads", guidName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await Upload.CopyToAsync(fileStream);
                    }

                    var importer = ImportFactory.GetImporter((ImporterEnum)this.Importer);
                    var sessionModel = importer.ImportFromFile(path);
                    if (sessionModel.ErrorMessage != null)
                    {
                        ModelState.AddModelError("Importer", string.Format(ImportAdminRes.VALIDATION_FILE_INVALID_FORMAT, sessionModel.ErrorMessage));
                        await PrepareSelectListsAsync();
                        return Page();
                    }
                    this.SessionModel = sessionModel;
                    this.Filename = guidName;
                }
                return Page();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Import failed");
                _contextUtils.CreateActionStateCookie(TempData, AlertTypeEnum.Danger, AdminRes.ERROR_EXCEPTION);
                return RedirectToPage("/Import/Index", new { area = "Admin" });
            }
        }

        public async Task<IActionResult> OnPostImportAsync()
        {
            try
            {
                var claimId = await _contextUtils.CheckMunicipalityRightsAsync(this.MunicipalityId, User, _dbContext, TempData);
                if (claimId == null)
                {
                    _contextUtils.CreateActionStateCookie(TempData, AlertTypeEnum.Danger, AdminRes.ERROR_NO_RIGHT);
                    return RedirectToPage("/Index", new { area = "" });
                }
                var importer = ImportFactory.GetImporter((ImporterEnum)this.Importer);
                var path = Path.Combine(_environment.ContentRootPath, "uploads", this.Filename);
                var sessionModel = importer.ImportFromFile(path);
                this.SessionModel = sessionModel;
                if (sessionModel.ErrorMessage == null)
                {
                    var isSuccess = await _dbContext.ImportSessionAsync(claimId.Value, this.BodyId, sessionModel);
                    if (isSuccess)
                    {
                        _contextUtils.CreateActionStateCookie(TempData, AlertTypeEnum.Success, ImportAdminRes.ALERT_IMPORT_SUCCESS);
                        return RedirectToPage("/Import/Index", new { area = "Admin" });
                    }
                    else
                    {
                        this.Alert = new AlertModel(AlertTypeEnum.Danger, ImportAdminRes.ALERT_IMPORT_FAILED);
                    }
                }
                return Page();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Import failed");
                _contextUtils.CreateActionStateCookie(TempData, AlertTypeEnum.Danger, AdminRes.ERROR_EXCEPTION);
                return RedirectToPage("/Import/Index", new { area = "Admin" });
            }
        }

    }
}
