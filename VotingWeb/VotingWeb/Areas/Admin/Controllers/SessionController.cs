using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using TableGenerator.Models;
using VotingData;
using VotingWeb.Areas.Admin.Models.Sessions;
using VotingWeb.DataContext;
using VotingWeb.Infrastructure;
using VotingWeb.Properties;

namespace VotingWeb.Areas.Admin.Controllers
{
    [Authorize(Roles = Constants.ROLE_EDITOR + "," + Constants.ROLE_ADMIN)]
    public class SessionController : BaseController
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        // GET: Admin/Session
        public ActionResult Index(int? page)
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
                    var model = new TableModel()
                    {
                        ControllerName = "Session",
                        Title = SessionAdminRes.TABLE_TITLE,
                        TableName = "Sessions",
                        DefaultWhere = "tt.[MunicipalityId] = " + claimId.ToString(),
                        IsDeleteTable = true,
                        PageIndex = page ?? 0,
                        ButtonCreateText = SessionAdminRes.BUTTON_CREATE,
                        DefaultSort = "tt.[StartDate]",
                        ColumnList = new List<ColumnModel>()
                        {
                            new ColumnModel()
                            {
                                ColumnName = "Name",
                                ColumnType = ColumnTypeEnum.String,
                                IsSortable = true,
                                Title = SessionAdminRes.COLUMN_NAME,
                            },
                            new ColumnModel()
                            {
                                ColumnName = "StartDate",
                                ColumnType = ColumnTypeEnum.Date,
                                IsSortable = true,
                                Title = SessionAdminRes.COLUMN_DATE_START,
                                DecoderDateToString = (dateTime) => {
                                    if (dateTime.HasValue)
                                    {
                                        return dateTime.Value.ToString("dd.MM.yyyy");
                                    }
                                    return null;
                                },
                            },
                            new ColumnModel()
                            {
                                ColumnType = ColumnTypeEnum.ExtraButton,
                                ButtonTitle = SessionAdminRes.BUTTON_TOPICS,
                                ButtonAction = "Index",
                                ButtonController = "Topic",
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
            }
            catch (Exception exception)
            {
                logger.Error(exception);
                ContextUtils.Instance.CreateActionStateCookie(Response, AlertTypeEnum.Danger, AdminRes.ERROR_EXCEPTION);
                return RedirectToAction("Index", "Home", new { area = "" });
            }
        }

        // GET: Admin/Session/Create
        public ActionResult Create()
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
                    var model = new UpdateModel(null);
                    model.MunicipalityId = claimId.Value;
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

        // POST: Admin/Session/Create
        [HttpPost]
        public ActionResult Create(UpdateModel model, string action)
        {
            try
            {
                using (var entities = new VotingEntities())
                {
                    if (ModelState.IsValid)
                    {
                        var claimId = CheckMunicipalityRight(entities);
                        if (claimId == null || claimId != model.MunicipalityId)
                        {
                            ContextUtils.Instance.CreateActionStateCookie(Response, AlertTypeEnum.Danger, AdminRes.ERROR_NO_RIGHT);
                            return RedirectToAction("Index", "Home", new { area = "" });
                        }

                        var itemId = SessionContext.Instance.Update(entities, null, model);
                        if (itemId.HasValue)
                        {
                            ContextUtils.Instance.CreateActionStateCookie(Response, AlertTypeEnum.Success, AdminRes.SUCCESS_CREATE);
                            if (action == SessionAdminRes.BUTTON_TOPICS)
                            {
                                return RedirectToAction("Index", "Topic", new { id = itemId.Value });
                            }
                            return RedirectToAction("Index", new { id = model.MunicipalityId });
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

        // GET: Admin/Session/Update/{id}
        public ActionResult Update(int id)
        {
            try
            {
                using (var entities = new VotingEntities())
                {
                    var entity = SessionContext.Instance.FindById(entities, id);
                    var claimId = CheckMunicipalityRight(entities);
                    if (claimId == null || claimId != entity.MunicipalityId)
                    {
                        ContextUtils.Instance.CreateActionStateCookie(Response, AlertTypeEnum.Danger, AdminRes.ERROR_NO_RIGHT);
                        return RedirectToAction("Index", "Home", new { area = "" });
                    }

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

        // POST: Admin/Session/Update
        [HttpPost]
        public ActionResult Update(UpdateModel model, string action)
        {
            try
            {
                using (var entities = new VotingEntities())
                {
                    if (ModelState.IsValid)
                    {
                        var entity = SessionContext.Instance.FindById(entities, model.Id);
                        var claimId = CheckMunicipalityRight(entities);
                        if (claimId == null || claimId != entity.MunicipalityId)
                        {
                            ContextUtils.Instance.CreateActionStateCookie(Response, AlertTypeEnum.Danger, AdminRes.ERROR_NO_RIGHT);
                            return RedirectToAction("Index", "Home", new { area = "" });
                        }

                        var itemId = SessionContext.Instance.Update(entities, entity, model);
                        if (itemId.HasValue)
                        {
                            ContextUtils.Instance.CreateActionStateCookie(Response, AlertTypeEnum.Success, AdminRes.SUCCESS_UPDATE);
                            if (action == SessionAdminRes.BUTTON_TOPICS)
                            {
                                return RedirectToAction("Index", "Topic", new { id = itemId.Value });
                            }
                            return RedirectToAction("Index", new { id = model.MunicipalityId });
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

        // GET: Admin/Session/Delete/{id}
        public ActionResult Delete(int id)
        {
            try
            {
                using (var entities = new VotingEntities())
                {
                    var entity = SessionContext.Instance.FindById(entities, id);
                    var claimId = CheckMunicipalityRight(entities);
                    if (claimId == null || claimId != entity.MunicipalityId)
                    {
                        ContextUtils.Instance.CreateActionStateCookie(Response, AlertTypeEnum.Danger, AdminRes.ERROR_NO_RIGHT);
                        return RedirectToAction("Index", "Home", new { area = "" });
                    }

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

        // POST: Admin/Session/Delete
        [HttpPost]
        public ActionResult Delete(DeleteModel model)
        {
            try
            {
                using (var entities = new VotingEntities())
                {
                    var entity = SessionContext.Instance.FindById(entities, model.Id);
                    var claimId = CheckMunicipalityRight(entities);
                    if (claimId == null || claimId != entity.MunicipalityId)
                    {
                        ContextUtils.Instance.CreateActionStateCookie(Response, AlertTypeEnum.Danger, AdminRes.ERROR_NO_RIGHT);
                        return RedirectToAction("Index", "Home", new { area = "" });
                    }

                    bool isSuccess = SessionContext.Instance.Delete(entities, entity);
                    if (isSuccess)
                    {
                        ContextUtils.Instance.CreateActionStateCookie(Response, AlertTypeEnum.Success, AdminRes.SUCCESS_DELETE);
                        return RedirectToAction("Index", new { id = model.MunicipalityId });
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