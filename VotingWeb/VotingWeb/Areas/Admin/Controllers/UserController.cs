using Microsoft.AspNet.Identity.Owin;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TableGenerator.Models;
using VotingData;
using VotingWeb.Areas.Admin.Models.Users;
using VotingWeb.DataContext;
using VotingWeb.Infrastructure;
using VotingWeb.Models;
using VotingWeb.Properties;

namespace VotingWeb.Areas.Admin.Controllers
{
    [Authorize(Roles = Constants.ROLE_ADMIN)]
    public class UserController : Controller
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        // GET: Admin/User
        public ActionResult Index(int? page)
        {
            try
            {
                var model = new TableModel()
                {
                    ControllerName = "User",
                    Title = UserAdminRes.TABLE_TITLE,
                    TableName = "AspNetUsers",
                    IsDeleteTable = false,
                    PageIndex = page ?? 0,
                    ButtonCreateText = UserAdminRes.BUTTON_CREATE,
                    ColumnList = new List<ColumnModel>()
                    {
                        new ColumnModel()
                        {
                            ColumnName = "UserName",
                            ColumnType = ColumnTypeEnum.String,
                            IsSortable = true,
                            Title = UserAdminRes.COLUMN_USER_NAME,
                        },
                        new ColumnModel()
                        {
                            ColumnName = @"stuff((SELECT ';' + ro.[Name] 
                                   FROM dbo.[AspNetUserRoles] ur
                                   JOIN dbo.[AspNetRoles] ro ON ur.[RoleId] = ro.[Id]
                                   WHERE ur.[UserId] = tt.[Id]
                                   ORDER BY ro.[Name]
                                   FOR XML PATH(''), TYPE).value('.', 'varchar(max)')
                                ,1,1,'')",
                            ColumnType = ColumnTypeEnum.DecodeStringToString,
                            DecoderStringToString = (stringValue) =>
                            {
                                return RoleConvert.Decode(stringValue);
                            },
                            IsSortable = true,
                            Title = UserAdminRes.COLUMN_ROLES,
                        },
                        new ColumnModel()
                        {
                            ColumnName = string.Format(@"stuff((SELECT ';' + uc.[ClaimValue]
                                   FROM dbo.[AspNetUserClaims] uc
                                   WHERE uc.[UserId] = tt.[Id] AND uc.[ClaimType] = '{0}'
                                   FOR XML PATH(''), TYPE).value('.', 'varchar(max)')
                                ,1,1,'')", Constants.CLAIM_MUNICIPALITY),
                            ColumnType = ColumnTypeEnum.DecodeStringToString,
                            DecoderStringToString = (stringValue) =>
                            {
                                return ClaimConvert.Decode(stringValue);
                            },
                            IsSortable = true,
                            Title = UserAdminRes.COLUMN_MUNICIPALITIES,
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

        // GET: Admin/User/Create
        public ActionResult Create()
        {
            try
            {
                using (var entities = new VotingEntities())
                {
                    var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                    var model = new UpdateModel(userManager, null);
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

        // POST: Admin/User/Create
        [HttpPost]
        public async Task<ActionResult> Create(UpdateModel model)
        {
            try
            {
                using (var entities = new VotingEntities())
                {
                    model.Validate(ModelState);
                    if (ModelState.IsValid)
                    {
                        var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                        var user = new ApplicationUser
                        {
                            UserName = model.UserName,
                            Email = model.UserName,
                            EmailConfirmed = true
                        };
                        var result = await userManager.CreateAsync(user);
                        if (result.Succeeded)
                        {
                            if (!string.IsNullOrWhiteSpace(model.Password))
                            {
                                await userManager.AddPasswordAsync(user.Id, model.Password);
                            }
                            await userManager.AddToRoleAsync(user.Id, model.Role);
                            if (model.Municipality != -1)
                            {
                                await userManager.AddClaimAsync(user.Id, new Claim(Constants.CLAIM_MUNICIPALITY, model.Municipality.ToString()));
                            }
                            ContextUtils.Instance.CreateActionStateCookie(Response, AlertTypeEnum.Success, AdminRes.SUCCESS_CREATE);
                            return RedirectToAction("Index");
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

        // GET: Admin/User/Update/{id}
        public ActionResult Update(string id)
        {
            try
            {
                using (var entities = new VotingEntities())
                {
                    var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                    var user = userManager.FindByIdAsync(id).Result;
                    if (user == null)
                    {
                        return RedirectToAction("Index");
                    }
                    var model = new UpdateModel(userManager, user);
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

        // POST: Admin/User/Update
        [HttpPost]
        public async Task<ActionResult> Update(UpdateModel model)
        {
            try
            {
                using (var entities = new VotingEntities())
                {
                    if (ModelState.IsValid)
                    {
                        var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                        var user = userManager.FindByIdAsync(model.Id).Result;
                        var currentRoleList = await userManager.GetRolesAsync(user.Id);
                        bool hasRole = false;
                        foreach (var role in currentRoleList)
                        {
                            if (role != model.Role)
                            {
                                await userManager.RemoveFromRoleAsync(user.Id, role);
                            }
                            else
                            {
                                hasRole = true;
                            }
                        }
                        if (!hasRole)
                        {
                            await userManager.AddToRoleAsync(user.Id, model.Role);
                        }
                        var claimsToRemove = user.Claims.ToList();
                        foreach (var claim in claimsToRemove)
                        {
                            await userManager.RemoveClaimAsync(user.Id, new Claim(claim.ClaimType, claim.ClaimValue));
                        }
                        if (model.Municipality != -1)
                        {
                            await userManager.AddClaimAsync(user.Id, new Claim(Constants.CLAIM_MUNICIPALITY, model.Municipality.ToString()));
                        }
                        var result = await userManager.UpdateAsync(user);
                        if (result.Succeeded)
                        {
                            if (!string.IsNullOrWhiteSpace(model.Password))
                            {
                                await userManager.RemovePasswordAsync(user.Id);
                                await userManager.AddPasswordAsync(user.Id, model.Password);
                            }
                            ContextUtils.Instance.CreateActionStateCookie(Response, AlertTypeEnum.Success, AdminRes.SUCCESS_UPDATE);
                            return RedirectToAction("Index");
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

        // GET: Admin/User/Delete/{id}
        public ActionResult Delete(string id)
        {
            try
            {
                using (var entities = new VotingEntities())
                {
                    var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                    var user = userManager.FindByIdAsync(id).Result;
                    var model = new DeleteModel(user);
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

        // POST: Admin/User/Delete
        [HttpPost]
        public ActionResult Delete(DeleteModel model)
        {
            try
            {
                using (var entities = new VotingEntities())
                {
                    var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                    var user = userManager.FindByIdAsync(model.Id).Result;
                    var result = userManager.DeleteAsync(user).Result;
                    if (result.Succeeded)
                    {
                        ContextUtils.Instance.CreateActionStateCookie(Response, AlertTypeEnum.Success, AdminRes.SUCCESS_DELETE);
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ContextUtils.Instance.CreateActionStateCookie(Response, AlertTypeEnum.Danger, AdminRes.ERROR_FAILED_DELETE);
                        return View(new DeleteModel(user));
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