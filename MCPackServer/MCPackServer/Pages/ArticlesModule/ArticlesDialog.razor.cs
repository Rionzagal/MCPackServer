using MCPackServer.Entities;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Text.RegularExpressions;

namespace MCPackServer.Pages.ArticlesModule
{
    public partial class ArticlesDialog
    {
        public enum States { Add, Edit, Delete }

        #region Parameters
        [CascadingParameter]
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
        public MudDialogInstance Dialog { get; set; }
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
        [Parameter]
        public States State { get; set; }
        [Parameter]
        public ArticlesView? ModelView { get; set; }
        [Parameter]
        public int? FamilyId { get; set; }
        #endregion

        #region Dialog variables
        private string Title = string.Empty;
        private string TitleIcon = string.Empty;
        private bool Disabled;
        private Color ButtonColor;
        private bool _processing = false;
        #endregion

        #region Models and elements
        private MudForm Form = new();

        private PurchaseArticles Model = new();
        private string GroupCode = string.Empty;
        private string FamilyCode = string.Empty;
        #endregion

        protected override async Task OnInitializedAsync()
        {
            if (States.Add == State) //representing an Add dialog
            {
                Title = "Añadir nuevo artículo de compra";
                TitleIcon = Icons.Material.Filled.Create;
                Disabled = false;
                ButtonColor = Color.Success;
            }
            else if (States.Edit == State) //representing an Edit dialog
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
                Dialog.Cancel();
            }
            if (null != ModelView)
            {
                Model = await _service.GetByKeyAsync<PurchaseArticles>(ModelView.Id);
                FamilyCode = (await _service.GetByKeyAsync<ArticleFamilies>(ModelView.FamilyId)).Code;
                GroupCode = (await _service.GetByKeyAsync<ArticleGroups>(ModelView.GroupId)).Code;
            }
            else if (FamilyId.HasValue)
            {
                Model.FamilyId = FamilyId.Value;
                var currentFamily = await _service.GetByKeyAsync<ArticleFamilies>(FamilyId);
                FamilyCode = currentFamily.Code;
                GroupCode = (await _service.GetByKeyAsync<ArticleGroups>(currentFamily.GroupId)).Code;
            }
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
                    if (response?.IsSuccessful ?? false)
                        Snackbar.Add("Artículo añadido con éxito.", Severity.Success);
                    else
                    {
                        foreach (var error in response?.Errors ?? new List<string>())
                        {
                            Snackbar.Add(error, Severity.Error);
                        }
                    }
                }
                else if (States.Edit == State)
                {
                    var response = await _service.UpdateAsync(Model);
                    if (response?.IsSuccessful ?? false)
                        Snackbar.Add("Artículo editado con éxito.", Severity.Success);
                    else
                    {
                        foreach (var error in (response?.Errors ?? new List<string>()))
                        {
                            Snackbar.Add(error, Severity.Error);
                        }
                    }
                }
                else if (States.Delete == State)
                {
                    var response = await _service.RemoveAsync(Model);
                    if (response?.IsSuccessful ?? false)
                        Snackbar.Add("Artículo eliminado con éxito.", Severity.Success);
                    else
                    {
                        foreach (var error in (response?.Errors ?? new List<string>()))
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
