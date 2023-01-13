using MCPackServer.Entities;
using MCPackServer.Models;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using MudBlazor;

namespace MCPackServer.Pages.PurchaseOrdersModule
{
    public partial class PurchaseOrdersDialog
    {
        public enum States { Add, Edit, Delete }

        #region Parameters
        [CascadingParameter]
        public MudDialogInstance? Dialog { get; set; }
        [Parameter]
        public States State { get; set; }
        [Parameter]
        public PurchaseOrdersView? ModelView { get; set; }
        #endregion

        #region Dialog variables
        private string Title = string.Empty;
        private string TitleIcon = string.Empty;
        private bool Disabled;
        private Color ButtonColor;
        private bool _processing = false;
        #endregion

        #region API elements
        private MudForm Form = new();
        private List<Providers> providers = new();
        private List<ProjectsView> projects = new();
        private List<Requisitions> requisitions = new();
        private List<PurchaseOrders> existentOrders = new();
        private PurchaseOrders Model = new()
        {
            OrderNumber = "0001",
            DeliveryDate = DateTime.Today,
            Status = "Pendiente"
        };
        #endregion

        protected override async Task OnInitializedAsync()
        {
            existentOrders = (await _service.GetForGridAsync<PurchaseOrders>(new(), getAll: true)).ToList();
            string MostRecentOrderNumber = existentOrders.OrderByDescending(o => o.Id).FirstOrDefault()?
                .OrderNumber ?? "0";
            int ODdigits = (int)Math.Floor(Math.Log10(int.Parse(MostRecentOrderNumber) + 1) + 1);
            Model.OrderNumber = (int.Parse(MostRecentOrderNumber) + 1).ToString($"d{(ODdigits < 4 ? 4 : ODdigits)}");
            if (null != ModelView)
                Model = await _service.GetByKeyAsync<PurchaseOrders>(ModelView.Id);
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
                Dialog?.Cancel();
            // Load the Autocomplete elements items.
            await ProjectsServerReload(string.Empty);
            await ProvidersServerReload(string.Empty);
            await RequisitionsServerReload(string.Empty);

        }

        private async Task Submit()
        {
            _processing = true;
            await Form.Validate();
            if (Form.IsValid)
            {
                if (States.Add == State)
                {
                    Model.IssuedDate = DateTime.Now;
                    var response = await _service.AddAsync(Model);
                    if (response.IsSuccessful)
                        Snackbar.Add("Órden de compra añadida con éxito.", Severity.Success);
                    else
                        foreach (var error in response.Errors)
                        {
                            Snackbar.Add(error, Severity.Error);
                        }
                }
                else if (States.Edit == State)
                {
                    var response = await _service.UpdateAsync(Model);
                    if (response.IsSuccessful)
                        Snackbar.Add("Órden de compra editada con éxito.", Severity.Success);
                    else
                        foreach (var error in response.Errors)
                        {
                            Snackbar.Add(error, Severity.Error);
                        }
                }
                else if (States.Delete == State)
                {
                    var response = await _service.RemoveAsync(Model);
                    if (response.IsSuccessful)
                        Snackbar.Add("Órden de compra eliminada con éxito.", Severity.Success);
                    else
                        foreach (var error in response.Errors)
                        {
                            Snackbar.Add(error, Severity.Error);
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

        private async Task<IEnumerable<int>> ProvidersServerReload(string filter)
        {
            List<int> result = new();
            DataManagerRequest dm = new()
            {
                Take = 0,
                RequiresCounts = false,
                Where = new List<WhereFilter>
                {
                    new WhereFilter
                    {
                        Field = nameof(Providers.LegalName),
                        Value = filter,
                        Operator = Operators.StartsWith
                    }
                }
            };
            var response = await _service.GetForGridAsync<Providers>(dm, getAll: true);
            if (null != response)
            {
                providers = response.ToList();
                result = response.Select(x => x.Id).ToList();
            }
            return result;
        }

        private string GetProviderName(int Id)
        {
            string providerName = string.Empty;
            if (0 != Id)
            {
                var match = providers.SingleOrDefault(p => Id == p.Id);
                if (null != match)
                    providerName = match.LegalName;
            }
            return providerName;
        }

        private float GetProviderDiscount(int Id)
        {
            float discount = 0f;
            if (0 != Id)
            {
                var match = providers.SingleOrDefault(providers => providers.Id == Id);
                if (null != match)
                    discount = match.Discount;
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
                    new WhereFilter
                    {
                        Field = nameof(Projects.ProjectNumber),
                        Value = filter,
                        Operator = Operators.StartsWith
                    }
                }
            };
            var response = await _service.GetForGridAsync<ProjectsView>(ProjectsDm, getAll: true);
            if (null != response)
            {
                projects = response.ToList();
                result = response.Select(x => x.Id).ToList();
            }
            return result;
        }

        private string GetProjectNumber(int Id)
        {
            string number = string.Empty;
            if (Id > 0)
            {
                var match = projects.SingleOrDefault(p => p.Id == Id);
                if (null != match)
                    number = match.ProjectNumber;
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
                    new WhereFilter
                    {
                        Field = nameof(Requisitions.RequisitionNumber),
                        Value = filter,
                        Operator = Operators.StartsWith
                    }
                }
            };
            var response = await _service.GetForGridAsync<Requisitions>(dm, getAll: true);
            if (null != response)
            {
                requisitions = response.ToList();
                result = response.Select(x => (int?)x.Id).ToList();
            }
            return result;
        }

        private string GetRequisitionNumber(int? Id)
        {
            string number = string.Empty;
            if (Id.HasValue && 0 != Id.Value)
            {
                var match = requisitions.SingleOrDefault(requisition => requisition.Id == Id);
                if (null != match)
                    number = match.RequisitionNumber;
            }
            return number;
        }

        private DateTime GetRequiredDate(int? Id)
        {
            DateTime date = DateTime.Now;
            if (Id.HasValue && 0 != Id.Value)
            {
                var match = requisitions.SingleOrDefault(requisitions => requisitions.Id == Id);
                if (null != match && match.RequiredDate.HasValue)
                    date = match.RequiredDate.Value;
            }
            return date;
        }

        private IEnumerable<string> ValidateOrderNumber(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                yield return "Este campo es requerido.";
                yield break;
            }
            if (States.Add == State)
            {
                if (value.Any(ch => !char.IsDigit(ch)))
                    yield return "Este campo solamente acepta caracteres numéricos";
                if (int.TryParse(value, out int numValue))
                {
                    if (existentOrders.Select(o => int.Parse(o.OrderNumber)).Any(v => v == numValue))
                        yield return "Ya existe una orden de compra con este valor numérico.";
                    if (existentOrders.Any(o => o.OrderNumber == value))
                        yield return "Ya existe una orden de compra con este valor de texto.";
                }
            }
        }
    }
}
