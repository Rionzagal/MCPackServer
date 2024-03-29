﻿using MCPackServer.Entities;
using MCPackServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace MCPackServer.Pages.ProjectsModule
{
    public partial class ProjectsDialog
    {
        public enum States { Add, Edit, Delete }

        #region Parameters
        [CascadingParameter]
        public MudDialogInstance? Dialog { get; set; }
        [Parameter]
        public States State { get; set; }
        [Parameter]
        public ProjectsView? ModelView { get; set; }
        #endregion

        #region Dialog variables
        private string Title = string.Empty;
        private string TitleIcon = string.Empty;
        private bool Disabled;
        private Color ButtonColor;
        private bool _processing = false;
        private bool CanChangeClient = true;
        #endregion

        #region API elements
        private MudForm Form = new();
        private List<ProjectsView> ExistentProjects = new();
        private List<Clients> ClientsList = new();
        private Projects Model = new()
        {
            ProjectNumber = "01",
            Type = "Proyecto",
            AdmissionDate = DateTime.Today,
            CommitmentDate = DateTime.Today,
            DeliveryDate = DateTime.Today
        };
        #endregion

        protected override async Task OnInitializedAsync()
        {
            if (States.Add == State) //representing an Add dialog
            {
                Title = "Añadir nuevo proyecto";
                TitleIcon = Icons.Material.Filled.Create;
                Disabled = false;
                ButtonColor = Color.Success;
            }
            else if (States.Edit == State) //representing an Edit dialog
            {
                Title = "Editar proyecto seleccionado";
                TitleIcon = Icons.Material.Filled.Edit;
                Disabled = false;
                ButtonColor = Color.Primary;

                var _authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
                var user = _authState.User;
                if (user != null)
                {
                    CanChangeClient = (await _authorizationService.AuthorizeAsync(user, Constants.Permissions.ProjectSpecial.ClientChange)).Succeeded;
                }
            }
            else if (States.Delete == State) //representing a Delete dialog
            {
                Title = "Eliminar proyecto seleccionado";
                TitleIcon = Icons.Material.Filled.Delete;
                Disabled = true;
                ButtonColor = Color.Error;
            }
            else //should not get to this option
            {
                Dialog?.Cancel();
            }
            await GetExistentProjects();
            if (null != ModelView)
                Model = await _service.GetByKeyAsync<Projects>(ModelView.Id);
            else
            {
                if (ExistentProjects.Any())
                    Model.ProjectNumber = (ExistentProjects.Max(p => int.Parse(p.ProjectNumber)) + 1).ToString("d4");
            }
        }

        private async Task Submit()
        {
            bool ProjectValid = true;
            _processing = true;
            if (States.Add == State)
            {
                Model.Code = $"{Model.Id}C{Model.ClientId:4d}T{Model.Type.FirstOrDefault()}";
                if ("Refacciones" == Model.Type)
                {
                    DataManagerRequest request = new()
                    {
                        Where = new List<WhereFilter>()
                        {
                            new WhereFilter
                            {
                                Field = nameof(Projects.ClientId),
                                Value = Model.ClientId,
                                Operator = Operators.Equal,
                                Condition = Conditions.And
                            },
                            new WhereFilter
                            {
                                Field = nameof(Projects.Type),
                                Value = "Refacciones",
                                Operator = Operators.Equal,
                                Condition = Conditions.And
                            }
                        }
                    };
                    var projects = await _service.GetForGridAsync<Projects>(request);
                    if (null != projects)
                        ProjectValid = !projects.Any();
                }
            }
            await Form.Validate();
            if (Form.IsValid && ProjectValid || States.Delete == State)
            {
                if (States.Add == State)
                {
                    var response = await _service.AddAsync(Model);
                    if (response.IsSuccessful)
                        Snackbar.Add("Proyecto añadido con éxito.", Severity.Success);
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
                        Snackbar.Add("Proyecto editado con éxito.", Severity.Success);
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
                        Snackbar.Add("Proyecto eliminado con éxito.", Severity.Success);
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
                if (!Form.IsValid)
                    Snackbar.Add("Operación no válida. Revise si hay algún error en la forma.", Severity.Warning);
                if (!ProjectValid)
                    Snackbar.Add("Operación no válida. Este cliente ya cuenta con un proyecto de refacciones.", Severity.Warning);
                _processing = false;
            }
        }

        private async Task GetExistentProjects()
        {
            DataManagerRequest request = new();
            var response = await _service.GetForGridAsync<ProjectsView>(request, "ProjectNumber", "DESC");
            if (response != null)
                ExistentProjects = response.ToList();
        }

        private async Task<IEnumerable<int>> ClientsServerReload(string filter = "")
        {
            List<int> result = new();
            List<WhereFilter> filters = new()
            {
                new WhereFilter
                {
                    Field = "MarketName",
                    Value = filter,
                    Operator = Operators.Contains
                }
            };
            DataManagerRequest Dm = new()
            {
                Where = filters,
            };
            var response = await _service.GetForGridAsync<Clients>(Dm);
            if (null != response)
            {
                ClientsList = response.ToList();
                result = response.Select(c => c.Id).ToList();
            }
            return result;
        }

        private string GetClientName(int Id)
        {
            string name = "";
            if (0 != Id)
            {
                var match = ClientsList.FirstOrDefault(c => c.Id == Id);
                if (null != match)
                    name = match.MarketName;
            }
            return name;
        }
    }
}
