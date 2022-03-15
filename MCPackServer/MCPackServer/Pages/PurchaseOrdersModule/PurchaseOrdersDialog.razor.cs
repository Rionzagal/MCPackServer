using MCPackServer.Entities;
using MCPackServer.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MCPackServer.Pages.PurchaseOrdersModule
{
    public partial class PurchaseOrdersDialog
    {
        public enum States { Add, Edit, Delete }

        #region Parameters
        [CascadingParameter]
        public MudDialogInstance Dialog { get; set; }
        [Parameter]
        public States State { get; set; }
        [Parameter]
        public PurchaseOrders Model { get; set; }
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
        private List<Providers> providers = new();
        private List<Projects> projects = new();
        private List<Requisitions> requisitions = new();
        #endregion

        protected override async Task OnInitializedAsync()
        {
            if (States.Add == State) //representing an Add dialog
            {
                Title = "Añadir nuevo proyecto";
                TitleIcon = Icons.Material.Filled.Create;
                Disabled = false;
                ButtonColor = Color.Success;
                Model.Status = "Pendiente";
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
                Dialog.Cancel();
            }
            await ProjectsServerReload(string.Empty);
            await ProvidersServerReload(string.Empty);
            await RequisitionsServerReload(string.Empty);
        }

        private async Task Submit()
        {
            _processing = true;
            ActionResponse<PurchaseOrders> response = new();
            await Form.Validate();
            if (Form.IsValid)
            {
                if (States.Add != State) Model.IssuedDate = DateTime.Now;
                if (States.Add == State) response = await _ordersService.AddAsync(Model);
                else if (States.Edit == State) response = await _ordersService.UpdateAsync(Model);
                else if (States.Delete == State) response = await _ordersService.RemoveAsync(Model);
                Dialog.Close(DialogResult.Ok(response));
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
            DataManagerRequest dm = new()
            {
                Take = 0,
                RequiresCounts = false,
                Where = new List<WhereFilter>
                {
                    new WhereFilter { Field = "LegalName", Value = filter }
                }
            };
            var response = await _providersService.GetForGridAsync<Providers>(dm, getAll: true);
            if (null != response)
            {
                providers = response.ToList();
                providers.ForEach(provider => result.Add(provider.Id));
            }
            return result;
        }

        private string GetProviderName(int Id)
        {
            string providerName = string.Empty;
            if (0 != Id)
            {
                var match = providers.SingleOrDefault(p => Id == p.Id);
                if (null != match) providerName = match.LegalName;
            }
            return providerName;
        }

        private float GetProviderDiscount(int Id)
        {
            float discount = 0f;
            if (0 != Id)
            {
                var match = providers.SingleOrDefault(providers => providers.Id == Id);
                if (null != match) discount = match.Discount;
            }
            return discount;
        }

        private async Task<IEnumerable<int>> ProjectsServerReload(string filter)
        {
            List<int> result = new();
            DataManagerRequest ProjectsDm = new()
            {
                Where = new List<WhereFilter>()
                {
                    new WhereFilter(){ Field = "ProjectNumber", Value = filter }
                }
            };
            var response = await _projectsService.GetForGridAsync<Projects>(ProjectsDm, getAll: true);
            if (null != response)
            {
                projects = response.ToList();
                projects.ForEach(project => result.Add(project.Id));
            }
            return result;
        }

        private string GetProjectNumber(int Id)
        {
            string number = string.Empty;
            if (Id > 0)
            {
                var match = projects.SingleOrDefault(p => p.Id == Id);
                if (null != match) number = match.ProjectNumber;
            }
            return number;
        }

        private async Task<IEnumerable<int?>> RequisitionsServerReload(string filter)
        {
            List<int?> result = new();
            DataManagerRequest dm = new()
            {
                Take = 0,
                Where = new List<WhereFilter>
                {
                    new WhereFilter { Field = "RequisitionNumber", Value = filter }
                }
            };
            var response = await _requisitionsService.GetForGridAsync<Requisitions>(dm, getAll: true);
            if (null != response)
            {
                requisitions = response.ToList();
                requisitions.ForEach(requisition => result.Add(requisition.Id));
            }
            return result;
        }

        private string GetRequisitionNumber(int? Id)
        {
            string number = string.Empty;
            if (Id.HasValue && 0 != Id.Value)
            {
                var match = requisitions.SingleOrDefault(requisition => requisition.Id == Id);
                if (null != match) number = match.RequisitionNumber;
            }
            return number;
        }

        private DateTime GetRequiredDate(int? Id)
        {
            DateTime date = DateTime.Now;
            if (Id.HasValue && 0 != Id.Value)
            {
                var match = requisitions.SingleOrDefault(requisitions => requisitions.Id == Id);
                if (null != match && match.RequiredDate.HasValue) date = match.RequiredDate.Value;
            }
            return date;
        }
    }
}
