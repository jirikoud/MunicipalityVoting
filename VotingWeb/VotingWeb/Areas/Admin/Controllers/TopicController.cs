using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TableGenerator.Models;
using VotingData;
using VotingWeb.Areas.Admin.Models.Topics;
using VotingWeb.DataContext;
using VotingWeb.Infrastructure;
using VotingWeb.Properties;

namespace VotingWeb.Areas.Admin.Controllers
{
    [Authorize(Roles = Constants.ROLE_EDITOR + "," + Constants.ROLE_ADMIN)]
    public class TopicController : BaseController
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        // GET: Admin/Topic
        public ActionResult Index(int id, int? page)
        {
            try
            {
                using (var entities = new VotingEntities())
                {
                    var session = SessionContext.Instance.FindById(entities, id);
                    var claimId = CheckMunicipalityRight(entities);
                    if (claimId == null || claimId != session.MunicipalityId)
                    {
                        ContextUtils.Instance.CreateActionStateCookie(Response, AlertTypeEnum.Danger, AdminRes.ERROR_NO_RIGHT);
                        return RedirectToAction("Index", "Home", new { area = "" });
                    }

                    var model = new TableModel()
                    {
                        ControllerName = "Topic",
                        Title = TopicAdminRes.TABLE_TITLE,
                        TableName = "Topics",
                        DefaultWhere = "tt.[SessionId] = " + id.ToString(),
                        IsDeleteTable = true,
                        PageIndex = page ?? 0,
                        ButtonCreateText = TopicAdminRes.BUTTON_CREATE,
                        ButtonCreateId = id,
                        BackController = "Session",
                        BackId = session.MunicipalityId,
                        DefaultSort = "tt.[Order]",
                        ColumnList = new List<ColumnModel>()
                        {
                            new ColumnModel()
                            {
                                ColumnName = "tt.[Order]",
                                ColumnType = ColumnTypeEnum.Integer,
                                IsSortable = true,
                                Title = TopicAdminRes.COLUMN_ORDER,
                            },
                            new ColumnModel()
                            {
                                ColumnName = "Name",
                                ColumnType = ColumnTypeEnum.String,
                                IsSortable = true,
                                Title = TopicAdminRes.COLUMN_NAME,
                            },
                            new ColumnModel()
                            {
                                ColumnName = "IsProcedural",
                                ColumnType = ColumnTypeEnum.Boolean,
                                DecoderBoolToString = (boolValue) => {
                                    if (boolValue.HasValue)
                                    {
                                        return boolValue.Value ? CommonRes.TABLE_VALUE_TRUE : CommonRes.TABLE_VALUE_FALSE;
                                    }
                                    return null;
                                },
                                IsSortable = true,
                                Title = TopicAdminRes.COLUMN_IS_PROCEDURAL,
                            },
                            new ColumnModel()
                            {
                                ColumnName = "IsSecret",
                                ColumnType = ColumnTypeEnum.Boolean,
                                DecoderBoolToString = (boolValue) => {
                                    if (boolValue.HasValue)
                                    {
                                        return boolValue.Value ? CommonRes.TABLE_VALUE_TRUE : CommonRes.TABLE_VALUE_FALSE;
                                    }
                                    return null;
                                },
                                IsSortable = true,
                                Title = TopicAdminRes.COLUMN_IS_SECRET,
                            },
                            new ColumnModel()
                            {
                                ColumnType = ColumnTypeEnum.ExtraButton,
                                ButtonTitle = TopicAdminRes.BUTTON_VOTING,
                                ButtonAction = "Index",
                                ButtonController = "Voting",
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

        // GET: Admin/Topic/Create
        public ActionResult Create(int id)
        {
            try
            {
                using (var entities = new VotingEntities())
                {
                    var session = SessionContext.Instance.FindById(entities, id);
                    var claimId = CheckMunicipalityRight(entities);
                    if (claimId == null || claimId != session.MunicipalityId)
                    {
                        ContextUtils.Instance.CreateActionStateCookie(Response, AlertTypeEnum.Danger, AdminRes.ERROR_NO_RIGHT);
                        return RedirectToAction("Index", "Home", new { area = "" });
                    }

                    var model = new UpdateModel(null);
                    var count = TopicContext.Instance.GetCountInSession(entities, id);
                    model.SessionId = session.Id;
                    model.Order = count.Value + 1;
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

        // POST: Admin/Topic/Create
        [HttpPost]
        public ActionResult Create(UpdateModel model, string action)
        {
            try
            {
                using (var entities = new VotingEntities())
                {
                    var session = SessionContext.Instance.FindById(entities, model.SessionId);
                    var claimId = CheckMunicipalityRight(entities);
                    if (claimId == null || claimId != session.MunicipalityId)
                    {
                        ContextUtils.Instance.CreateActionStateCookie(Response, AlertTypeEnum.Danger, AdminRes.ERROR_NO_RIGHT);
                        return RedirectToAction("Index", "Home", new { area = "" });
                    }

                    if (ModelState.IsValid)
                    {
                        var itemId = TopicContext.Instance.Update(entities, null, model);
                        if (itemId.HasValue)
                        {
                            ContextUtils.Instance.CreateActionStateCookie(Response, AlertTypeEnum.Success, AdminRes.SUCCESS_CREATE);
                            if (action == TopicAdminRes.BUTTON_VOTING)
                            {
                                return RedirectToAction("Index", "Voting", new { id = itemId.Value });
                            }
                            return RedirectToAction("Index", new { id = model.SessionId });
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

        // GET: Admin/Topic/Update/{id}
        public ActionResult Update(int id)
        {
            try
            {
                using (var entities = new VotingEntities())
                {
                    var entity = TopicContext.Instance.FindById(entities, id);
                    var claimId = CheckMunicipalityRight(entities);
                    if (claimId == null || claimId != entity.Session.MunicipalityId)
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

        // POST: Admin/Topic/Update
        [HttpPost]
        public ActionResult Update(UpdateModel model, string action)
        {
            try
            {
                using (var entities = new VotingEntities())
                {
                    if (ModelState.IsValid)
                    {
                        var entity = TopicContext.Instance.FindById(entities, model.Id);
                        var claimId = CheckMunicipalityRight(entities);
                        if (claimId == null || claimId != entity.Session.MunicipalityId)
                        {
                            ContextUtils.Instance.CreateActionStateCookie(Response, AlertTypeEnum.Danger, AdminRes.ERROR_NO_RIGHT);
                            return RedirectToAction("Index", "Home", new { area = "" });
                        }

                        var itemId = TopicContext.Instance.Update(entities, entity, model);
                        if (itemId.HasValue)
                        {
                            ContextUtils.Instance.CreateActionStateCookie(Response, AlertTypeEnum.Success, AdminRes.SUCCESS_UPDATE);
                            if (action == TopicAdminRes.BUTTON_VOTING)
                            {
                                return RedirectToAction("Index", "Voting", new { id = itemId.Value });
                            }
                            return RedirectToAction("Index", new { id = model.SessionId });
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

        // GET: Admin/Topic/Delete/{id}
        public ActionResult Delete(int id)
        {
            try
            {
                using (var entities = new VotingEntities())
                {
                    var entity = TopicContext.Instance.FindById(entities, id);
                    var claimId = CheckMunicipalityRight(entities);
                    if (claimId == null || claimId != entity.Session.MunicipalityId)
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

        // POST: Admin/Topic/Delete
        [HttpPost]
        public ActionResult Delete(DeleteModel model)
        {
            try
            {
                using (var entities = new VotingEntities())
                {
                    var entity = TopicContext.Instance.FindById(entities, model.Id);
                    var claimId = CheckMunicipalityRight(entities);
                    if (claimId == null || claimId != entity.Session.MunicipalityId)
                    {
                        ContextUtils.Instance.CreateActionStateCookie(Response, AlertTypeEnum.Danger, AdminRes.ERROR_NO_RIGHT);
                        return RedirectToAction("Index", "Home", new { area = "" });
                    }

                    bool isSuccess = TopicContext.Instance.Delete(entities, entity);
                    if (isSuccess)
                    {
                        ContextUtils.Instance.CreateActionStateCookie(Response, AlertTypeEnum.Success, AdminRes.SUCCESS_DELETE);
                        return RedirectToAction("Index", new { id = model.SessionId });
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