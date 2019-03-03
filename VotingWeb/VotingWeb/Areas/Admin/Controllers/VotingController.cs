using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TableGenerator.Models;
using VotingCommon.Enumerations;
using VotingData;
using VotingWeb.Areas.Admin.Models.Votings;
using VotingWeb.DataContext;
using VotingWeb.Infrastructure;
using VotingWeb.Properties;

namespace VotingWeb.Areas.Admin.Controllers
{
    [Authorize(Roles = Constants.ROLE_EDITOR + "," + Constants.ROLE_ADMIN)]
    public class VotingController : BaseController
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        // GET: Admin/Voting
        public ActionResult Index(int id, int? page)
        {
            try
            {
                using (var entities = new VotingEntities())
                {
                    var topic = TopicContext.Instance.FindById(entities, id);
                    var claimId = CheckMunicipalityRight(entities);
                    if (claimId == null || claimId != topic.Session.MunicipalityId)
                    {
                        ContextUtils.Instance.CreateActionStateCookie(Response, AlertTypeEnum.Danger, AdminRes.ERROR_NO_RIGHT);
                        return RedirectToAction("Index", "Home", new { area = "" });
                    }

                    var model = new TableModel()
                    {
                        ControllerName = "Voting",
                        Title = VotingAdminRes.TABLE_TITLE,
                        TableName = "Votings",
                        DefaultWhere = "tt.[TopicId] = " + id.ToString(),
                        IsDeleteTable = false,
                        PageIndex = page ?? 0,
                        ButtonCreateText = VotingAdminRes.BUTTON_CREATE,
                        ButtonCreateId = id,
                        BackController = "Topic",
                        BackId = topic.SessionId,
                        JoinTable = "JOIN [Deputies] de ON tt.[DeputyId] = de.[Id] JOIN [Parties] pa ON tt.[PartyId] = pa.[Id]",
                        DefaultSort = "de.[Lastname]",
                        ColumnList = new List<ColumnModel>()
                        {
                            new ColumnModel()
                            {
                                ColumnName = "de.[Lastname]",
                                ColumnType = ColumnTypeEnum.String,
                                IsSortable = true,
                                Title = VotingAdminRes.COLUMN_DEPUTY,
                            },
                            new ColumnModel()
                            {
                                ColumnName = "pa.[Name]",
                                ColumnType = ColumnTypeEnum.String,
                                IsSortable = true,
                                Title = VotingAdminRes.COLUMN_PARTY,
                            },
                            new ColumnModel()
                            {
                                ColumnName = "tt.[Vote]",
                                ColumnType = ColumnTypeEnum.DecodeIntToString,
                                IsSortable = true,
                                Title = VotingAdminRes.COLUMN_VOTE,
                                DecoderIntToString = (vote) =>
                                {
                                    return VoteConvert.Convert((VoteEnum)vote);
                                }
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

        // GET: Admin/Voting/Create
        public ActionResult Create(int id)
        {
            try
            {
                using (var entities = new VotingEntities())
                {
                    var topic = TopicContext.Instance.FindById(entities, id);
                    var claimId = CheckMunicipalityRight(entities);
                    if (claimId == null || claimId != topic.Session.MunicipalityId)
                    {
                        ContextUtils.Instance.CreateActionStateCookie(Response, AlertTypeEnum.Danger, AdminRes.ERROR_NO_RIGHT);
                        return RedirectToAction("Index", "Home", new { area = "" });
                    }

                    var model = new UpdateModel(null);
                    model.TopicId = topic.Id;
                    model.FillSelectLists(entities);
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

        // POST: Admin/Voting/Create
        [HttpPost]
        public ActionResult Create(UpdateModel model)
        {
            try
            {
                using (var entities = new VotingEntities())
                {
                    var topic = TopicContext.Instance.FindById(entities, model.TopicId);
                    var claimId = CheckMunicipalityRight(entities);
                    if (claimId == null || claimId != topic.Session.MunicipalityId)
                    {
                        ContextUtils.Instance.CreateActionStateCookie(Response, AlertTypeEnum.Danger, AdminRes.ERROR_NO_RIGHT);
                        return RedirectToAction("Index", "Home", new { area = "" });
                    }

                    var exists = VotingContext.Instance.Exists(entities, model.TopicId, model.DeputyId);
                    if (exists)
                    {
                        ModelState.AddModelError("DeputyId", VotingAdminRes.VALIDATION_VOTING_EXISTS);
                    }
                    if (ModelState.IsValid)
                    {
                        var itemId = VotingContext.Instance.Update(entities, null, model);
                        if (itemId.HasValue)
                        {
                            ContextUtils.Instance.CreateActionStateCookie(Response, AlertTypeEnum.Success, AdminRes.SUCCESS_CREATE);
                            return RedirectToAction("Index", new { id = model.TopicId });
                        }
                        else
                        {
                            ContextUtils.Instance.CreateActionStateCookie(Response, AlertTypeEnum.Danger, AdminRes.ERROR_FAILED_CREATE);
                        }
                    }
                    model.FillSelectLists(entities);
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

        // GET: Admin/Voting/Update/{id}
        public ActionResult Update(int id)
        {
            try
            {
                using (var entities = new VotingEntities())
                {
                    var entity = VotingContext.Instance.FindById(entities, id);
                    var claimId = CheckMunicipalityRight(entities);
                    if (claimId == null || claimId != entity.Topic.Session.MunicipalityId)
                    {
                        ContextUtils.Instance.CreateActionStateCookie(Response, AlertTypeEnum.Danger, AdminRes.ERROR_NO_RIGHT);
                        return RedirectToAction("Index", "Home", new { area = "" });
                    }

                    var model = new UpdateModel(entity);
                    model.FillSelectLists(entities);
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

        // POST: Admin/Voting/Update
        [HttpPost]
        public ActionResult Update(UpdateModel model)
        {
            try
            {
                using (var entities = new VotingEntities())
                {
                    if (ModelState.IsValid)
                    {
                        var entity = VotingContext.Instance.FindById(entities, model.Id);
                        var claimId = CheckMunicipalityRight(entities);
                        if (claimId == null || claimId != entity.Topic.Session.MunicipalityId)
                        {
                            ContextUtils.Instance.CreateActionStateCookie(Response, AlertTypeEnum.Danger, AdminRes.ERROR_NO_RIGHT);
                            return RedirectToAction("Index", "Home", new { area = "" });
                        }

                        var itemId = VotingContext.Instance.Update(entities, entity, model);
                        if (itemId.HasValue)
                        {
                            ContextUtils.Instance.CreateActionStateCookie(Response, AlertTypeEnum.Success, AdminRes.SUCCESS_UPDATE);
                            return RedirectToAction("Index", new { id = model.TopicId });
                        }
                        else
                        {
                            ContextUtils.Instance.CreateActionStateCookie(Response, AlertTypeEnum.Danger, AdminRes.ERROR_FAILED_UPDATE);
                        }
                    }
                    model.FillSelectLists(entities);
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

        // GET: Admin/Voting/Delete/{id}
        public ActionResult Delete(int id)
        {
            try
            {
                using (var entities = new VotingEntities())
                {
                    var entity = VotingContext.Instance.FindById(entities, id);
                    var claimId = CheckMunicipalityRight(entities);
                    if (claimId == null || claimId != entity.Topic.Session.MunicipalityId)
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

        // POST: Admin/Voting/Delete
        [HttpPost]
        public ActionResult Delete(DeleteModel model)
        {
            try
            {
                using (var entities = new VotingEntities())
                {
                    var entity = VotingContext.Instance.FindById(entities, model.Id);
                    var claimId = CheckMunicipalityRight(entities);
                    if (claimId == null || claimId != entity.Topic.Session.MunicipalityId)
                    {
                        ContextUtils.Instance.CreateActionStateCookie(Response, AlertTypeEnum.Danger, AdminRes.ERROR_NO_RIGHT);
                        return RedirectToAction("Index", "Home", new { area = "" });
                    }

                    bool isSuccess = VotingContext.Instance.Delete(entities, entity);
                    if (isSuccess)
                    {
                        ContextUtils.Instance.CreateActionStateCookie(Response, AlertTypeEnum.Success, AdminRes.SUCCESS_DELETE);
                        return RedirectToAction("Index", new { id = model.TopicId });
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