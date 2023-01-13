using MCPackServer.Entities;
using MCPackServer.Models;
using MudBlazor;

namespace MCPackServer.Pages.LogsModule
{
    public partial class LogsPage
    {
        #region MudComponents
        MudTable<Logs> LogsTable = new();
        #endregion

        #region Page variables
        private bool VisibleLog = false;
        Logs SelectedLog = new();
        #endregion

        #region Logs related methods
        private async Task<TableData<Logs>> LogsServerReload(TableState state)
        {
            DataManagerRequest request = new()
            {
                Take = state.PageSize,
                Skip = state.PageSize * state.Page
            };
            string field = state.SortLabel ?? nameof(Logs.TimeOfAction);
            string order = state.SortDirection == SortDirection.Ascending ? "ASC" : "DESC";
            return new TableData<Logs>()
            {
                Items = await _service.GetForGridAsync<Logs>(request, field, order),
                TotalItems = await _service.GetTotalCountAsync<Logs>(request) ?? 0
            };
        }
        private void OnSelectedLog(TableRowClickEventArgs<Logs> args)
        {
            SelectedLog = args.Item;
            VisibleLog = true;
        }
        #endregion
    }
}
