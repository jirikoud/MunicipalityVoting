using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using VotingData;
using VotingImporter;
using VotingWeb.Areas.Admin.Models.Imports;
using VotingWeb.DataContext;
using VotingWeb.Infrastructure;
using VotingWeb.Properties;

namespace VotingWeb.Areas.Admin.Controllers
{
    [Authorize(Roles = Constants.ROLE_EDITOR + "," + Constants.ROLE_ADMIN)]
    public class ImportController : BaseController
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        // GET: Admin/Import
        public ActionResult Index()
        {
            try
            {
                using (var entities = new VotingEntities())
                {
                    var claimId = CheckMunicipalityRight(entities);
                    if (claimId == null)
                    {
                        ContextUtils.Instance.CreateActionStateCookie(Response, AlertTypeEnum.Danger, AdminRes.ERROR_NO_RIGHT);
                        return RedirectToAction("Index", "Home", new { area = "" });
                    }
                    var model = new ImportModel();
                    model.MunicipalityId = claimId.Value;
                    model.ImporterList = new SelectList(ImporterConvert.GetImporterList(), "Item1", "Item2", null);
                    return View(model);
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception);
                ContextUtils.Instance.CreateActionStateCookie(Response, AlertTypeEnum.Danger, AdminRes.ERROR_EXCEPTION);
                return RedirectToAction("Index");
            }
        }

        // POST: Admin/Import
        [HttpPost]
        public ActionResult Index(ImportModel model, HttpPostedFileBase file)
        {
            try
            {
                using (var entities = new VotingEntities())
                {
                    var claimId = CheckMunicipalityRight(entities);
                    if (claimId == null || claimId != model.MunicipalityId)
                    {
                        ContextUtils.Instance.CreateActionStateCookie(Response, AlertTypeEnum.Danger, AdminRes.ERROR_NO_RIGHT);
                        return RedirectToAction("Index", "Home", new { area = "" });
                    }

                    // Verify that the user selected a file
                    if (file != null && file.ContentLength > 0)
                    {
                        // store the file inside ~/App_Data/uploads folder
                        var guidName = Guid.NewGuid().ToString();
                        var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), guidName);
                        file.SaveAs(path);

                        var importer = ImportFactory.GetImporter((ImporterEnum)model.Importer);
                        var sessionModel = importer.ImportFromFile(path);
                        if (sessionModel.ErrorMessage != null)
                        {
                            ModelState.AddModelError("Importer", string.Format(ImportAdminRes.VALIDATION_FILE_INVALID_FORMAT, sessionModel.ErrorMessage));
                            return View(model);
                        }
                        model.SessionModel = sessionModel;
                        model.Filename = guidName;
                        return View("Import", model);
                    }
                }
                return View(model);
            }
            catch (Exception exception)
            {
                logger.Error(exception);
                ContextUtils.Instance.CreateActionStateCookie(Response, AlertTypeEnum.Danger, AdminRes.ERROR_EXCEPTION);
                return RedirectToAction("Index");
            }
        }

        // POST: Admin/Import
        [HttpPost]
        public ActionResult Import(ImportModel model)
        {
            try
            {
                using (var entities = new VotingEntities())
                {
                    var claimId = CheckMunicipalityRight(entities);
                    if (claimId == null || claimId != model.MunicipalityId)
                    {
                        ContextUtils.Instance.CreateActionStateCookie(Response, AlertTypeEnum.Danger, AdminRes.ERROR_NO_RIGHT);
                        return RedirectToAction("Index", "Home", new { area = "" });
                    }

                    var municipality = MunicipalityContext.Instance.FindById(entities, model.MunicipalityId);
                    var importer = ImportFactory.GetImporter((ImporterEnum)model.Importer);
                    var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), model.Filename);
                    var sessionModel = importer.ImportFromFile(path);
                    model.SessionModel = sessionModel;
                    if (sessionModel.ErrorMessage == null)
                    {
                        var isSuccess = MunicipalityContext.Instance.ImportSession(entities, municipality, sessionModel);
                        if (isSuccess)
                        {
                            ContextUtils.Instance.CreateActionStateCookie(Response, AlertTypeEnum.Success, ImportAdminRes.ALERT_IMPORT_SUCCESS);
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            model.SessionModel.ErrorMessage = ImportAdminRes.ALERT_IMPORT_FAILED;
                        }
                    }
                    return View(model);
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception);
                ContextUtils.Instance.CreateActionStateCookie(Response, AlertTypeEnum.Danger, AdminRes.ERROR_EXCEPTION);
                return RedirectToAction("Index");
            }
        }
    }
}