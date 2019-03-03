using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TableGenerator.Models;
using VotingData;
using VotingWeb.Areas.Admin.Models.Municipalities;
using VotingWeb.DataContext;
using VotingWeb.Infrastructure;
using VotingWeb.Properties;

namespace VotingWeb.Areas.Admin.Controllers
{
    [Authorize(Roles = Constants.ROLE_ADMIN)]
    public class MunicipalityController : Controller
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        // GET: Admin/Municipality
        public ActionResult Index(int? page)
        {
            try
            {
                var model = new TableModel()
                {
                    ControllerName = "Municipality",
                    Title = MunicipalityAdminRes.TABLE_TITLE,
                    TableName = "Municipalities",
                    IsDeleteTable = true,
                    PageIndex = page ?? 0,
                    ButtonCreateText = MunicipalityAdminRes.BUTTON_CREATE,
                    ColumnList = new List<ColumnModel>()
                    {
                        new ColumnModel()
                        {
                            ColumnName = "Name",
                            ColumnType = ColumnTypeEnum.String,
                            IsSortable = true,
                            Title = MunicipalityAdminRes.COLUMN_NAME,
                        },
                        new ColumnModel()
                        {
                            ColumnType = ColumnTypeEnum.ExtraButton,
                            ButtonTitle = MunicipalityAdminRes.BUTTON_SESSIONS,
                            ButtonAction = "Index",
                            ButtonController = "Session",
                        },
                        new ColumnModel()
                        {
                            ColumnType = ColumnTypeEnum.ActionButtons,
                            Title = CommonRes.COLUMN_ACTION,
                        }
                    },
                };
                return View(model);
            }
            catch (Exception exception)
            {
                logger.Error(exception);
                ContextUtils.Instance.CreateActionStateCookie(Response, AlertTypeEnum.Danger, AdminRes.ERROR_EXCEPTION);
                return RedirectToAction("Index", "Home", new { area = "" });
            }
        }

        // GET: Admin/Municipality/Create
        public ActionResult Create()
        {
            try
            {
                using (var entities = new VotingEntities())
                {
                    var model = new UpdateModel(null);
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

        // POST: Admin/Municipality/Create
        [HttpPost]
        public ActionResult Create(UpdateModel model, string action)
        {
            try
            {
                using (var entities = new VotingEntities())
                {
                    if (ModelState.IsValid)
                    {
                        var itemId = MunicipalityContext.Instance.Update(entities, null, model);
                        if (itemId.HasValue)
                        {
                            ContextUtils.Instance.CreateActionStateCookie(Response, AlertTypeEnum.Success, AdminRes.SUCCESS_CREATE);
                            if (action == MunicipalityAdminRes.BUTTON_SESSIONS)
                            {
                                return RedirectToAction("Index", "Session", new { id = itemId.Value });
                            }
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            ContextUtils.Instance.CreateActionStateCookie(Response, AlertTypeEnum.Danger, AdminRes.ERROR_FAILED_CREATE);
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

        // GET: Admin/Municipality/Update/{id}
        public ActionResult Update(int id)
        {
            try
            {
                using (var entities = new VotingEntities())
                {
                    var entity = MunicipalityContext.Instance.FindById(entities, id);
                    var model = new UpdateModel(entity);
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

        // POST: Admin/Municipality/Update
        [HttpPost]
        public ActionResult Update(UpdateModel model, string action)
        {
            try
            {
                using (var entities = new VotingEntities())
                {
                    if (ModelState.IsValid)
                    {
                        var entity = MunicipalityContext.Instance.FindById(entities, model.Id);
                        var itemId = MunicipalityContext.Instance.Update(entities, entity, model);
                        if (itemId.HasValue)
                        {
                            ContextUtils.Instance.CreateActionStateCookie(Response, AlertTypeEnum.Success, AdminRes.SUCCESS_UPDATE);
                            if (action == MunicipalityAdminRes.BUTTON_SESSIONS)
                            {
                                return RedirectToAction("Index", "Session", new { id = itemId.Value });
                            }
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            ContextUtils.Instance.CreateActionStateCookie(Response, AlertTypeEnum.Danger, AdminRes.ERROR_FAILED_UPDATE);
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

        // GET: Admin/Municipality/Delete/{id}
        public ActionResult Delete(int id)
        {
            try
            {
                using (var entities = new VotingEntities())
                {
                    var entity = MunicipalityContext.Instance.FindById(entities, id);
                    var model = new DeleteModel(entity);
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

        // POST: Admin/Municipality/Delete
        [HttpPost]
        public ActionResult Delete(DeleteModel model)
        {
            try
            {
                using (var entities = new VotingEntities())
                {
                    var entity = MunicipalityContext.Instance.FindById(entities, model.Id);
                    bool isSuccess = MunicipalityContext.Instance.Delete(entities, entity);
                    if (isSuccess)
                    {
                        ContextUtils.Instance.CreateActionStateCookie(Response, AlertTypeEnum.Success, AdminRes.SUCCESS_DELETE);
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ContextUtils.Instance.CreateActionStateCookie(Response, AlertTypeEnum.Danger, AdminRes.ERROR_FAILED_DELETE);
                        return View(new DeleteModel(entity));
                    }
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