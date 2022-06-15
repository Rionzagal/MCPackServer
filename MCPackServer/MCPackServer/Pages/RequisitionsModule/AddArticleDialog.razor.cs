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
        public MudDialogInstance? Dialog { get; set; }
        [Parameter]
        public RequisitionsView? Reference { get; set; }
        #endregion

        #region Dialog variables
        private string Title = string.Empty;
        private string TitleIcon = string.Empty;
        private bool Disabled;
        private Color ButtonColor;
        private bool _processing = false;
        #endregion

        #region API elements
        private HashSet<RequisitionArticlesView> OrderArticles = new();
        private HashSet<RequisitionArticlesView> SelectedArticles = new();
        private List<ArticleGroups> Groups = new();
        private List<ArticleFamilies> Families = new();
        private List<Projects> projects = new();
        #region Filters
        private int? FamilyFilter, GroupFilter;
        private string NameFilter = string.Empty;
        #endregion
        #endregion

        private MudTable<RequisitionArticlesView> ArticlesTable = new();
        protected override async Task OnInitializedAsync()
        {
            if (null == Reference)
                Dialog?.Cancel();

            Title = "Añadir nuevo artículo a orden de compra";
            TitleIcon = Icons.Material.Filled.Create;
            Disabled = false;
            ButtonColor = Color.Success;

            DataManagerRequest request = new()
            {
                Where = new List<WhereFilter>()
                {
                    new WhereFilter { Field = nameof(RequisitionArticles.RequisitionId), Value = Reference?.Id.ToString() ?? "" }
                }
            };
            OrderArticles = (await _service.GetForGridAsync<RequisitionArticlesView>
                (request, nameof(RequisitionArticles.ArticleId), getAll: true))
                .ToHashSet();

            #region Get groups and families
            DataManagerRequest familiesRequest = new();
            Families = (await _service.GetForGridAsync<ArticleFamilies>(familiesRequest, getAll: true)).ToList() 
                ?? new List<ArticleFamilies>();
            Groups = (await _service.GetForGridAsync<ArticleGroups>(new(), getAll: true)).ToList() 
                ?? new List<ArticleGroups>();
            #endregion

            #region Get projects
            await ProjectsServerReload();
            #endregion
        }

        private async Task Submit()
        {
            _processing = true;
            List<ActionResponse<RequisitionArticles>> SuccessResponses = new();
            List<ActionResponse<RequisitionArticles>> FailureResponses = new();
            foreach (var article in SelectedArticles)
            {
                RequisitionArticles Model = new()
                {
                    RequisitionId = article.RequisitionId,
                    ArticleId = article.ArticleId,
                    ProjectId = article.ProjectId,
                    Quantity = article.Quantity
                };
                var response = await _service.AddAsync(Model);
                if (response.IsSuccessful)
                    SuccessResponses.Add(response);
                else
                    FailureResponses.Add(response);
            }
            if (SuccessResponses.Any())
                Snackbar.Add($"{SuccessResponses.Count} de {SelectedArticles.Count} artículos han sido añadidos " +
                    $"correctamente a la requisición {Reference?.RequisitionNumber ?? "0"}.", Severity.Success);
            if (FailureResponses.Any())
            {
                Snackbar.Add($"Se ha detectado error al añadir {FailureResponses.Count} de {SelectedArticles.Count} " +
                    $"artículos a la requisición {Reference?.RequisitionNumber ?? "0"}.", Severity.Error);
                for (int i = 0; i < FailureResponses.Count; i++)
                {
                    Snackbar.Add($"Error {i + 1} de {FailureResponses.Count}: {FailureResponses[i].Errors}", Severity.Error);
                }
            }
            Dialog?.Close();
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

        private async Task<IEnumerable<int>> ProjectsServerReload(string filter = "")
        {
            List<int> result = new();
            List<WhereFilter> filters = new()
            {
                new WhereFilter { Field = nameof(Projects.ProjectNumber), Value = filter }
            };
            DataManagerRequest dm = new()
            {
                Where = filters,
            };
            var response = await _service.GetForGridAsync<Projects>(dm);
            if (null != response && response.Any())
            {
                projects = response.ToList();
                foreach (var project in projects)
                {
                    result.Add(project.Id);
                }
            }
            return result;
        }

        private string GetProjectNumber(int id)
        {
            string projectNumber = string.Empty;
            if (id != 0)
            {
                var match = projects.FirstOrDefault(f => f.Id == id);
                if (match != null)
                    projectNumber = match.ProjectNumber;
            }
            return projectNumber;
        }

        private async Task<TableData<RequisitionArticlesView>> ArticlesServerReload(TableState state)
        {
            List<RequisitionArticlesView> requestedArticles = new();
            DataManagerRequest request = new()
            {
                Take = state.PageSize,
                Skip = state.Page * state.PageSize,
                Where = new List<WhereFilter>
                {
                    new WhereFilter { Field = nameof(ArticlesView.FamilyId), Value = FamilyFilter?.ToString() ?? "" },
                    new WhereFilter { Field = "Name", Value = NameFilter }
                }
            };
            string field = state.SortLabel ?? "Id";
            string order = state.SortDirection == SortDirection.Ascending ? "ASC" : "DESC";
            
            var articles = (await _service.GetForGridAsync<ArticlesView>(request, field, order)).ToList();
            int TotalCount = await _service.GetTotalCountAsync<ArticlesView>(request) ?? 0;
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
                        new RequisitionArticlesView()
                        {
                            ArticleId = item.Id,
                            ArticleCode = item.Code,
                            ArticleName = item.Name,
                            FamilyId = item.FamilyId,
                            FamilyName = item.FamilyName,
                            Unit = item.Unit,
                            RequisitionId = Reference?.Id ?? 0
                        });
                }
                foreach (var item in OrderArticles)
                {
                    requestedArticles.Remove
                        (requestedArticles.Single(x => item.ArticleId == x.ArticleId));
                }
            }
            return new TableData<RequisitionArticlesView>() 
            { Items = requestedArticles, TotalItems = TotalCount };
        }
    }
}
