using MCPackServer.Entities;
using MCPackServer.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Newtonsoft.Json;

namespace MCPackServer.Pages.ArticlesModule
{
    public partial class GroupsDialog
    {
        public enum States { Add, Edit, Delete }

        #region Parameters
        [CascadingParameter]
        public MudDialogInstance Dialog { get; set; }
        [Parameter]
        public States State { get; set; }
        [Parameter]
        public ArticleGroups Model { get; set; } = new();
        #endregion

        #region Dialog variables
        private string Title;
        private string TitleIcon;
        private bool Disabled;
        private Color ButtonColor;
        private bool _processing = false;
        #endregion

        #region API elements
        private MudForm Form = new();
        #endregion

        private bool ModelHasVariablePrice = false;
        protected override void OnInitialized()
        {
            if (States.Add == State) //representing an Add dialog
            {
                Title = "Añadir nuevo grupo de artículos";
                TitleIcon = Icons.Material.Filled.Create;
                Disabled = false;
                ButtonColor = Color.Success;
            }
            else if (States.Edit == State) //representing an Edit dialog
            {
                Title = "Editar grupo seleccionado";
                TitleIcon = Icons.Material.Filled.Edit;
                Disabled = false;
                ButtonColor = Color.Primary;
            }
            else if (States.Delete == State) //representing a Delete dialog
            {
                Title = "Eliminar grupo seleccionado";
                TitleIcon = Icons.Material.Filled.Delete;
                Disabled = true;
                ButtonColor = Color.Error;
            }
            else //should not get to this option
            {
                Dialog.Cancel();
            }
        }

        private async Task Submit()
        {
            _processing = true;
            await Form.Validate();
            if (Form.IsValid)
            {
                Model.HasVariablePrice = ModelHasVariablePrice;
                if (States.Add == State)
                {
                    var response = await _service.AddAsync(Model);
                    if (response.IsSuccessful)
                        Snackbar.Add("Grupo añadido con éxito.", Severity.Success);
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
                        Snackbar.Add("Grupo editado con éxito.", Severity.Success);
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
                        Snackbar.Add("Grupo eliminado con éxito.", Severity.Success);
                    else
                    {
                        foreach (var error in response.Errors)
                        {
                            Snackbar.Add(error, Severity.Error);
                        }
                    }
                }
                _processing = false;
                Dialog.Close();
            }
            else
            {
                Snackbar.Add("Operación no válida. Revise si hay algún error en la forma.", Severity.Warning);
                _processing = false;
            }
        }
    }
}
