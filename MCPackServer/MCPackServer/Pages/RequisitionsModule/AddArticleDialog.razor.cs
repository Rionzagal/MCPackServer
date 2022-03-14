using MCPackServer.Entities;
using MCPackServer.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace MCPackServer.Pages.RequisitionsModule
{
    public partial class AddArticleDialog
    {
        #region Parameters
        [CascadingParameter]
        public MudDialogInstance Dialog { get; set; }
        [Parameter]
        public Requisitions Reference { get; set; }
        #endregion

        #region Dialog variables
        private string Title;
        private string TitleIcon;
        private bool Disabled;
        private Color ButtonColor;
        private bool _processing = false;
        #endregion

        #region API elements
        private HashSet<RequisitionArticles> OrderArticles = new();
        private HashSet<RequisitionArticles> SelectedArticles = new();
        private List<ArticleGroups> Groups = new();
        private List<ArticleFamilies> Families = new();
        private List<Projects> projects = new();
        #region Filters
        private int? FamilyFilter, GroupFilter;
        private string NameFilter;
        #endregion
        #endregion

        private MudTable<RequisitionArticles> ArticlesTable;
        protected override async Task OnInitializedAsync()
        {
            Title = "Añadir nuevo artículo a orden de compra";
            TitleIcon = Icons.Material.Filled.Create;
            Disabled = false;
            ButtonColor = Color.Success;
            OrderArticles = Reference.RequisitionArticles.ToHashSet();

            #region Get groups and families
            DataManagerRequest familiesRequest = new();
            var familiesResponse = await _familiesService.GetForGridAsync<ArticleFamilies>(familiesRequest);
            if (null != familiesResponse)
            {
                Families = familiesResponse.ToList();
                Families.ForEach(f => Groups.Add(f.Group));
            }
            #endregion

            #region Get projects
            DataManagerRequest ProjectsDm = new();
            var projectsResponse = await _projectsService.GetForGridAsync<Projects>(ProjectsDm);
            if (null != projectsResponse) projects = projectsResponse.ToList();
            #endregion
        }

        private async Task Submit()
        {
            _processing = true;
            List<ActionResponse<RequisitionArticles>> responses = new();
            foreach (var Model in SelectedArticles)
            {
                var response = await _articlesService.AddAsync(Model);
                if (response.IsSuccessful) responses.Add(response);
            }
            Dialog.Close(DialogResult.Ok(responses));
        }

        private async Task<IEnumerable<int?>> SearchGroupFilters(string filter)
        {
            List<int?> result = new();
            await Task.Delay(1);
            if (string.IsNullOrEmpty(filter))
                foreach (var item in Groups) result.Add(item.Id);
            else
                foreach (var item in Groups.Where(g => g.Name.Contains(filter))) result.Add(item.Id);
            return result;
        }

        private string GetGroupName(int? Id)
        {
            string groupName = string.Empty;
            if (Id.HasValue && Id.Value > 0)
            {
                var match = Groups.SingleOrDefault(g => g.Id == Id);
                if (null != match) groupName = match.Name;
            }
            return groupName;
        }

        private async Task<IEnumerable<int?>> SearchFamilyFilters(string filter, int? groupId)
        {
            List<int?> result = new();
            List<ArticleFamilies> filterFamilies = Families;
            await Task.Delay(1);
            if (string.IsNullOrEmpty(filter) && groupId.HasValue)
                filterFamilies = Families.Where(x => groupId == x.GroupId).ToList();
            else if (!groupId.HasValue)
                filterFamilies = Families.Where(x => x.Name.Contains(filter)).ToList();
            else if (groupId.HasValue)
                filterFamilies = Families.Where(x => x.Name.Contains(filter) && groupId == x.GroupId).ToList();
            foreach (var item in filterFamilies) result.Add(item.Id);
            return result;
        }

        private string GetFamilyName(int? Id)
        {
            string familyName = string.Empty;
            if (Id.HasValue && Id != 0)
            {
                var match = Families.SingleOrDefault(f => f.Id == Id);
                if (match != null) familyName = match.Name;
            }
            return familyName;
        }

        private async Task<TableData<RequisitionArticles>> ArticlesServerReload(TableState state)
        {
            List<RequisitionArticles> requestedArticles = new();
            DataManagerRequest request = new()
            {
                Take = state.PageSize,
                Skip = state.Page * state.PageSize,
                Where = new List<WhereFilter>
                {
                    new WhereFilter { Field = "FamilyId", Value = FamilyFilter.HasValue ? FamilyFilter.Value.ToString() : string.Empty },
                    new WhereFilter { Field = "Name", Value = NameFilter }
                }
            };
            string field = state.SortLabel ?? "Id";
            string order = state.SortDirection == SortDirection.Ascending ? "ASC" : "DESC";
            
            var articles = (await _service.GetForGridAsync<PurchaseArticles>(request, field, order)).ToList();
            int? TotalCount = await _service.GetTotalCountAsync<PurchaseArticles>(request);
            if (null != articles)
            {
                if (GroupFilter.HasValue && 0 != GroupFilter)
                {
                    var FilterFamilies = Families.Where(f => GroupFilter != f.GroupId);
                    foreach (var family in FilterFamilies)
                    {
                        int removed = articles.RemoveAll(a => family.Id == a.FamilyId);
                        TotalCount -= removed;
                    }
                }
                foreach (var item in articles)
                {
                    requestedArticles.Add(
                        new RequisitionArticles()
                        {
                            ArticleId = item.Id,
                            Article = item,
                            RequisitionId = Reference.Id
                        });
                }
                foreach (var item in OrderArticles)
                {
                    requestedArticles.Remove
                        (requestedArticles.Single(x => item.ArticleId == x.ArticleId));
                }
            }
            return new TableData<RequisitionArticles>() 
            { Items = requestedArticles, TotalItems = TotalCount ?? 0 };
        }

        private string ProjectValidation(int input)
        {
            if (!projects.Any(p => input == p.Id)) return "Inserte un número de proyecto válido.";
            return null;
        }
    }
}
