using MCPackServer.Entities;
using MCPackServer.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using MudBlazor;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace MCPackServer.Pages.RolesModule
{
    //public partial class RolesDialog
    //{
    //    public enum States { Add, Edit, Delete }

    //    #region Parameters
    //    [CascadingParameter]
    //    public MudDialogInstance Dialog { get; set; }
    //    [Parameter]
    //    public States State { get; set; }
    //    [Parameter]
    //    public AspNetRoles Model { get; set; }
    //    #endregion

    //    #region Dialog variables
    //    private string Title;
    //    private string TitleIcon;
    //    private bool Disabled;
    //    private Color ButtonColor;
    //    private bool _processing = false;
    //    #endregion

    //    #region API elements
    //    private MudForm Form;
    //    #endregion

    //    protected override async Task OnInitializedAsync()
    //    {
    //        if (States.Add == State) //representing an Add dialog
    //        {
    //            Title = "Añadir nuevo rol de usuario";
    //            TitleIcon = Icons.Material.Filled.Create;
    //            Disabled = false;
    //            ButtonColor = Color.Success;
    //        }
    //        else if (States.Edit == State) //representing an Edit dialog
    //        {
    //            Title = $"Editar rol : {Model.Name}";
    //            TitleIcon = Icons.Material.Filled.Edit;
    //            Disabled = false;
    //            ButtonColor = Color.Primary;
    //        }
    //        else if (States.Delete == State) //representing a Delete dialog
    //        {
    //            Title = $"Eliminar rol: {Model.Name}";
    //            TitleIcon = Icons.Material.Filled.Delete;
    //            Disabled = true;
    //            ButtonColor = Color.Error;
    //        }
    //        else //should not get to this option
    //        {
    //            Dialog.Cancel();
    //        }
    //    }

    //    private async Task Submit()
    //    {
    //        _processing = true;
    //        AspNetRoles item = new() { Id = Model.Id, Name = Model.Name };
    //        await Form.Validate();
    //        if (Form.IsValid)
    //        {
    //            AspNetRoles ResultElement = new();
    //            bool success = false;
    //            if (States.Add == State)
    //            {
    //                IdentityRole role = new() { Name = Model.Name };
    //                var response = await _roleManager.CreateAsync(role);
    //                success = response.Succeeded;
    //            }
    //            else if (States.Edit == State)
    //            {
    //                var role = await _roleManager.FindByIdAsync(Model.Id);
    //                role.Name = item.Name;
    //                var response = await _roleManager.UpdateAsync(role);
    //                success = response.Succeeded;
    //                if (success && States.Edit == State)
    //                    ResultElement = await _service.GetByKeyAsync<AspNetRoles>(Model.Id);
    //            }
    //            else if (States.Delete == State)
    //            {
    //                var role = await _roleManager.FindByIdAsync(Model.Id);
    //                var response = await _roleManager.DeleteAsync(role);
    //                success = response.Succeeded;
    //            }
    //            var result = new
    //            {
    //                Success = success,
    //                Value = ResultElement
    //            };
    //            _processing = false;
    //            Dialog.Close(DialogResult.Ok(JsonSerializer.Serialize(result)));
    //        }
    //        else
    //        {
    //            if (!Form.IsValid)
    //                Snackbar.Add("Operación no válida. Revise si hay algún error en la forma.", Severity.Warning);
    //            _processing = false;
    //        }
    //    }
    //}
}
