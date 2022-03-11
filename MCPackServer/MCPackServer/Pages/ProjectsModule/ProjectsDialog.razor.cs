﻿using MCPackServer.Entities;
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

namespace MCPackServer.Pages.ProjectsModule
{
    public partial class ProjectsDialog
    {
        public enum States { Add, Edit, Delete }

        #region Parameters
        [CascadingParameter]
        public MudDialogInstance Dialog { get; set; }
        [Parameter]
        public States State { get; set; }
        [Parameter]
        public Projects Model { get; set; }
        #endregion

        #region Dialog variables
        private string Title;
        private string TitleIcon;
        private bool Disabled;
        private Color ButtonColor;
        private bool _processing = false;
        #endregion

        #region API elements
        private MudForm Form;
        private List<Clients> ClientsList = new();
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
                Title = null;
                TitleIcon = null;
                Disabled = true;
                ButtonColor = Color.Default;
                Dialog.Cancel();
            }
        }

        private async Task Submit()
        {
            bool ProjectValid = true;
            _processing = true;
            ActionResponse<Projects> response = new();
            if (States.Add == State)
            {
                Model.Code = $"{Model.Id}C{Model.ClientId:4d}T{Model.Type.FirstOrDefault()}";
                DataManagerRequest request = new()
                {
                    Where = new List<WhereFilter>()
                    {
                        new WhereFilter { Field = "ClientId", Value = Model.ClientId.ToString() },
                        new WhereFilter { Field = "Type", Value = Model.Type }
                    }
                };
                var projects = await _service.GetForGridAsync<Projects>(request);
                if (null != projects) ProjectValid = !projects.Any();
            }
            await Form.Validate();
            if (Form.IsValid && ProjectValid)
            {
                var properties = typeof(Projects).GetProperties()
                    .Where(x => !x.GetAccessors()[0].IsFinal && x.GetAccessors()[0].IsVirtual).ToList();
                properties.ForEach(x => x.SetValue(Model, null));
                if (States.Add == State) response = await _projectsService.AddAsync(Model);
                else if (States.Edit == State) response = await _projectsService.UpdateAsync(Model);
                else if (States.Edit == State) response = await _projectsService.RemoveAsync(Model);
                Dialog.Close(DialogResult.Ok(response));
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

        private async Task<IEnumerable<int>> ClientsServerReload(string filter = "")
        {
            List<int> result = new();
            List<WhereFilter> filters = new()
            {
                new WhereFilter { Field = "MarketName", Value = filter }
            };
            DataManagerRequest Dm = new()
            {
                Where = filters,
            };
            var response = await _service.GetForGridAsync<Clients>(Dm);
            if (null != response)
            {
                ClientsList = response.ToList();
                ClientsList.ForEach(c => result.Add(c.Id));
            }
            return result;
        }

        private string GetClientName(int Id)
        {
            string name = "";
            if (0 != Id)
            {
                var match = ClientsList.FirstOrDefault(c => c.Id == Id);
                if (null != match) name = match.MarketName;
            }
            return name;
        }
    }
}