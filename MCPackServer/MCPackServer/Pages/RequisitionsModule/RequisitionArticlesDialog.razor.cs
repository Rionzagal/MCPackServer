using MCPackServer.Entities;
using MCPackServer.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using Newtonsoft.Json;

namespace MCPackServer.Pages.RequisitionsModule
{
    public partial class RequisitionArticlesDialog
    {
        public enum States { Add, Edit, Delete }

        #region Parameters
        [CascadingParameter]
        public MudDialogInstance? Dialog { get; set; }
        [Parameter]
        public States State { get; set; }
        [Parameter]
        public RequisitionsView? Reference { get; set; }
        [Parameter]
        public RequisitionArticlesView? ModelView { get; set; } = new();
        #endregion

        #region Dialog variables
        private string Title = string.Empty;
        private string TitleIcon = string.Empty;
        private bool Disabled;
        private Color ButtonColor;
        private bool _processing = false;
        #endregion

        #region API elements
        private MudForm Form;
        private List<Projects> ProjectsList = new();
        #endregion

        private RequisitionArticles Model = new();

        protected override async Task OnInitializedAsync()
        {
            if (null == Reference || null == ModelView)
                Dialog?.Cancel();
            else
            {
                DataManagerRequest request = new()
                {
                    Where = new List<WhereFilter>
                    {
                        new WhereFilter { Field = nameof(RequisitionArticles.ArticleId), Value = ModelView.ArticleId.ToString() },
                        new WhereFilter { Field = nameof(RequisitionArticles.RequisitionId), Value = Reference.Id.ToString() },
                        new WhereFilter { Field = nameof(RequisitionArticles.ProjectId), Value = ModelView.ProjectId.ToString() }
                    }
                };
                Model = (await _service.GetForGridAsync<RequisitionArticles>
                    (request, nameof(RequisitionArticles.ArticleId), getAll: true))
                    .First();
            }
            if (States.Edit == State) //representing an Edit dialog
            {
                Title = "Editar artículo seleccionado";
                TitleIcon = Icons.Material.Filled.Edit;
                Disabled = false;
                ButtonColor = Color.Primary;
            }
            else if (States.Delete == State) //representing a Delete dialog
            {
                Title = "Eliminar artículo seleccionado";
                TitleIcon = Icons.Material.Filled.Delete;
                Disabled = true;
                ButtonColor = Color.Error;
            }
            else //should not get to this option
            {
                Dialog?.Cancel();
            }

            await ProjectsServerReload();
        }

        private async Task<IEnumerable<int>> ProjectsServerReload(string filter = "")
        {
            List<int> result = new();
            DataManagerRequest request = new()
            {
                Where = new()
                {
                    new WhereFilter { Field = nameof(Projects.ProjectNumber), Value = filter }
                }
            };
            var items = await _service.GetForGridAsync<Projects>(request);
            if (null != items)
            {
                ProjectsList = items.ToList();
                ProjectsList.ForEach(p => result.Add(p.Id));
            }
            return result;
        }

        private string GetProjectNumber(int Id)
        {
            string ProjectNumber = string.Empty;
            if (Id != 0)
            {
                var match = ProjectsList.FirstOrDefault(p => p.Id == Id);
                if (match != null)
                    ProjectNumber = match.ProjectNumber;
            }
            return ProjectNumber;
        }

        private async Task Submit()
        {
            _processing = true;
            await Form.Validate();
            if (Form.IsValid)
            {
                if (States.Add == State)
                {
                    var response = await _service.AddAsync(Model);
                    if (response.IsSuccessful)
                        Snackbar.Add("Artículo añadido con éxito.", Severity.Success);
                    else
                    {
                        foreach (var error in response.Errors)
                        {
                            Snackbar.Add(error, Severity.Error);
                        }
                    }
                }
                else if (States.Edit == State)
                {
                    var response = await _service.UpdateAsync(Model);
                    if (response.IsSuccessful)
                        Snackbar.Add("Artículo añadido con éxito.", Severity.Success);
                    else
                    {
                        foreach (var error in response.Errors)
                        {
                            Snackbar.Add(error, Severity.Error);
                        }
                    }
                }
                else if (States.Delete == State)
                {
                    var response = await _service.RemoveAsync(Model);
                    if (response.IsSuccessful)
                        Snackbar.Add("Artículo añadido con éxito.", Severity.Success);
                    else
                    {
                        foreach (var error in response.Errors)
                        {
                            Snackbar.Add(error, Severity.Error);
                        }
                    }
                }
                Dialog?.Close();
            }
            else
            {
                Snackbar.Add("Operación no válida. Revise si hay algún error en la forma.", Severity.Warning);
                _processing = false;
            }
        }
    }
}
