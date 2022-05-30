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
        public MudDialogInstance Dialog { get; set; }
        [Parameter]
        public States State { get; set; }
        [Parameter]
        public Quotes Model { get; set; }
        [Parameter]
        public object ArticleId { get; set; }
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
        }

        private async Task Submit()
        {
            _processing = true;
            string response = string.Empty;
            await Form.Validate();
            if (Form.IsValid)
            {
                if (States.Delete != State) Model.DateUpdated = DateTime.Now;
                if (States.Add == State)
                    response = JsonConvert.SerializeObject(await _quotesService.AddAsync(Model));
                else if (States.Edit == State)
                    response = JsonConvert.SerializeObject(await _quotesService.UpdateAsync(Model));
                else if (States.Delete == State)
                    response = JsonConvert.SerializeObject(await _quotesService.RemoveAsync(Model));
                _processing = false;
                Dialog.Close(DialogResult.Ok(JsonConvert.DeserializeObject<ActionResponse<Quotes>>(response)));
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
            var items = await _providersService.GetForGridAsync<Providers>(request);
            if (null != items)
            {
                Providers = items.ToList();
                Providers.ForEach(p => result.Add(p.Id));
            }
            return result;
        }

        private string GetProviderName(int Id)
        {
            string name = "";
            if (0 != Id)
            {
                var match = Providers.FirstOrDefault(p => Id == p.Id);
                if (null != match) name = match.MarketName;
            }
            return name;
        }
    }
}
