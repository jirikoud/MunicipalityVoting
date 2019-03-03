using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using TableGenerator.Models;
using VotingData;
using VotingWeb.Areas.Admin.Models.Parties;
using VotingWeb.DataContext;
using VotingWeb.Infrastructure;
using VotingWeb.Properties;

namespace VotingWeb.Areas.Admin.Controllers
{
    [Authorize(Roles = Constants.ROLE_EDITOR + "," + Constants.ROLE_ADMIN)]
    public class PartyController : BaseController
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        // GET: Admin/Party
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
                        ControllerName = "Party",
                        Title = PartyAdminRes.TABLE_TITLE,
                        TableName = "Parties",
                        DefaultWhere = "tt.[MunicipalityId] = " + claimId.ToString(),
                        IsDeleteTable = true,
                        PageIndex = page ?? 0,
                        ButtonCreateText = PartyAdminRes.BUTTON_CREATE,
                        DefaultSort = "tt.[Name]",
                        ColumnList = new List<ColumnModel>()
                        {
                            new ColumnModel()
                            {
                                ColumnName = "tt.[Name]",
                                ColumnType = ColumnTypeEnum.String,
                                IsSortable = true,
                                Title = PartyAdminRes.COLUMN_NAME,
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

        // GET: Admin/Party/Create
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

        // POST: Admin/Party/Create
        [HttpPost]
        public ActionResult Create(UpdateModel model)
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

                        var itemId = PartyContext.Instance.Update(entities, null, model);
                        if (itemId.HasValue)
                        {
                            ContextUtils.Instance.CreateActionStateCookie(Response, AlertTypeEnum.Success, AdminRes.SUCCESS_CREATE);
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

        // GET: Admin/Party/Update/{id}
        public ActionResult Update(int id)
        {
            try
            {
                using (var entities = new VotingEntities())
                {
                    var entity = PartyContext.Instance.FindById(entities, id);
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

        // POST: Admin/Party/Update
        [HttpPost]
        public ActionResult Update(UpdateModel model)
        {
            try
            {
                using (var entities = new VotingEntities())
                {
                    if (ModelState.IsValid)
                    {
                        var entity = PartyContext.Instance.FindById(entities, model.Id);
                        var claimId = CheckMunicipalityRight(entities);
                        if (claimId == null || claimId != entity.MunicipalityId)
                        {
                            ContextUtils.Instance.CreateActionStateCookie(Response, AlertTypeEnum.Danger, AdminRes.ERROR_NO_RIGHT);
                            return RedirectToAction("Index", "Home", new { area = "" });
                        }

                        var itemId = PartyContext.Instance.Update(entities, entity, model);
                        if (itemId.HasValue)
                        {
                            ContextUtils.Instance.CreateActionStateCookie(Response, AlertTypeEnum.Success, AdminRes.SUCCESS_UPDATE);
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

        // GET: Admin/Party/Delete/{id}
        public ActionResult Delete(int id)
        {
            try
            {
                using (var entities = new VotingEntities())
                {
                    var entity = PartyContext.Instance.FindById(entities, id);
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

        // POST: Admin/Party/Delete
        [HttpPost]
        public ActionResult Delete(DeleteModel model)
        {
            try
            {
                using (var entities = new VotingEntities())
                {
                    var entity = PartyContext.Instance.FindById(entities, model.Id);
                    var claimId = CheckMunicipalityRight(entities);
                    if (claimId == null || claimId != entity.MunicipalityId)
                    {
                        ContextUtils.Instance.CreateActionStateCookie(Response, AlertTypeEnum.Danger, AdminRes.ERROR_NO_RIGHT);
                        return RedirectToAction("Index", "Home", new { area = "" });
                    }

                    bool isSuccess = PartyContext.Instance.Delete(entities, entity);
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