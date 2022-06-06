using MCPackServer.Entities;
using MCPackServer.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Newtonsoft.Json;

namespace MCPackServer.Pages.ArticlesModule
{
    public partial class QuotesDialog
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
        public QuotesView? ModelView { get; set; }
        [Parameter]
        public int? ArticleId { get; set; }
        #endregion

        #region Dialog variables
        private string Title = string.Empty;
        private string TitleIcon = string.Empty;
        private bool Disabled;
        private Color ButtonColor;
        private bool _processing = false;
        private Quotes Model = new();
        #endregion

        #region API elements
        private MudForm Form;
        private List<Providers> Providers = new();
        #endregion

        protected override async Task OnInitializedAsync()
        {
            if (States.Add == State) //representing an Add dialog
            {
                Title = "Añadir nueva cotización";
                TitleIcon = Icons.Material.Filled.Create;
                Disabled = false;
                ButtonColor = Color.Success;
            }
            else if (States.Edit == State) //representing an Edit dialog
            {
                Title = "Editar cotización seleccionada";
                TitleIcon = Icons.Material.Filled.Edit;
                Disabled = false;
                ButtonColor = Color.Primary;
            }
            else if (States.Delete == State) //representing a Delete dialog
            {
                Title = "Eliminar cotización seleccionada";
                TitleIcon = Icons.Material.Filled.Delete;
                Disabled = true;
                ButtonColor = Color.Error;
            }
            else //should not get to this option
            {
                Dialog.Cancel();
            }
            _ = await ProvidersServerReload(string.Empty);
            if (null != ModelView)
                Model = await _service.GetByKeyAsync<Quotes>(ModelView.Id);
            else
                Model.ArticleId = ArticleId ?? 0;
        }

        private async Task Submit()
        {
            _processing = true;
            await Form.Validate();
            if (Form.IsValid)
            {
                if (States.Delete != State) Model.DateUpdated = DateTime.Now;
                if (States.Add == State)
                {
                    var response = await _service.AddAsync(Model);
                    if (response.IsSuccessful)
                        Snackbar.Add("Cotización añadida con éxito.", Severity.Success);
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
                        Snackbar.Add("Cotización editada con éxito.", Severity.Success);
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
                        Snackbar.Add("Cotización eliminada con éxito.", Severity.Success);
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

        private async Task<IEnumerable<int>> ProvidersServerReload(string filter)
        {
            List<int> result = new();
            List<WhereFilter> filters = new()
            {
                new WhereFilter { Field = "MarketName", Value = filter }
            };
            DataManagerRequest request = new()
            {
                Where = filters
            };
            var items = await _service.GetForGridAsync<Providers>(request);
            if (null != items)
            {
                Providers = items.ToList();
                Providers.ForEach(p => result.Add(p.Id));
            }
            return result;
        }

        private string GetProviderName(int Id)
        {
            string name = string.Empty;
            if (0 != Id)
            {
                var match = Providers.FirstOrDefault(p => Id == p.Id);
                if (null != match) name = match.MarketName;
            }
            return name;
        }
    }
}
