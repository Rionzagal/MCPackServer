using MCPackServer.Entities;
using MCPackServer.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MCPackServer.Pages.ProductsModule
{
    public partial class ProductsDialog
    {
        public enum States { Add, Edit, Delete }

        #region Parameters
        [CascadingParameter]
        public MudDialogInstance Dialog { get; set; }
        [Parameter]
        public States State { get; set; }
        [Parameter]
        public MCProducts Model { get; set; } = new();
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
        #endregion

        protected override void OnInitialized()
        {
            if (States.Add == State) //representing an Add dialog
            {
                Title = "Añadir nuevo producto";
                TitleIcon = Icons.Material.Filled.Create;
                Disabled = false;
                ButtonColor = Color.Success;
            }
            else if (States.Edit == State) //representing an Edit dialog
            {
                Title = "Editar producto seleccionado";
                TitleIcon = Icons.Material.Filled.Edit;
                Disabled = false;
                ButtonColor = Color.Primary;
            }
            else if (States.Delete == State) //representing a Delete dialog
            {
                Title = "Eliminar producto seleccionado";
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
            }
        }

        private async Task Submit()
        {
            _processing = true;
            ActionResponse<MCProducts> response = new();
            await Form.Validate();
            if (Form.IsValid)
            {
                if (States.Add == State) response = await _service.AddAsync(Model);
                else if (States.Edit == State) response = await _service.UpdateAsync(Model);
                else if (States.Delete == State) response = await _service.RemoveAsync(Model);
                _processing = false;
                Dialog.Close(DialogResult.Ok(response));
            }
            else
            {
                Snackbar.Add("Operación no válida. Revise si hay algún error en la forma.", Severity.Warning);
                _processing = false;
            }
        }
    }
}
