using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VotingData;
using VotingWeb.DataContext;
using VotingWeb.Infrastructure;
using VotingWeb.Properties;

namespace VotingWeb.Areas.Admin.Models.Votings
{
    public class UpdateModel
    {
        public int Id { get; set; }
        public bool IsCreate { get; set; }
        public int TopicId { get; set; }

        [Display(Name = "DETAIL_DEPUTY", ResourceType = typeof(VotingAdminRes))]
        [Required(ErrorMessageResourceName = "VALIDATION_EMPTY", ErrorMessageResourceType = typeof(AdminRes))]
        public int DeputyId { get; set; }
        public SelectList DeputyList { get; set; }

        [Display(Name = "DETAIL_PARTY", ResourceType = typeof(VotingAdminRes))]
        public int? PartyId { get; set; }
        public SelectList PartyList { get; set; }

        [Display(Name = "DETAIL_VOTE", ResourceType = typeof(VotingAdminRes))]
        [Required(ErrorMessageResourceName = "VALIDATION_EMPTY", ErrorMessageResourceType = typeof(AdminRes))]
        public int Vote { get; set; }
        public SelectList VoteList { get; set; }

        public UpdateModel()
        {
        }

        public UpdateModel(Voting entity)
        {
            if (entity != null)
            {
                this.Id = entity.Id;
                this.TopicId = entity.TopicId;
                this.DeputyId = entity.DeputyId;
                this.PartyId = entity.PartyId;
                this.Vote = entity.Vote;
            }
            else
            {
                this.IsCreate = true;
            }
        }

        public void FillSelectLists(VotingEntities entities)
        {
            var topic = TopicContext.Instance.FindById(entities, this.TopicId);
            var deputyList = DeputyContext.Instance.LoadList(entities, topic.Session.MunicipalityId).ConvertAll(item => new Tuple<int, string>(item.Id, item.GetFullName()));
            this.DeputyList = new SelectList(deputyList, "Item1", "Item2", this.DeputyId);
            var partyList = PartyContext.Instance.LoadList(entities, topic.Session.MunicipalityId).ConvertAll(item => new Tuple<int?, string>(item.Id, item.Name));
            partyList.Insert(0, new Tuple<int?, string>(null, VotingAdminRes.ITEM_NO_PARTY));
            this.PartyList = new SelectList(partyList, "Item1", "Item2", this.PartyId);
            this.VoteList = new SelectList(VoteConvert.GetVoteList(), "Item1", "Item2", this.Vote);
        }

    }
}