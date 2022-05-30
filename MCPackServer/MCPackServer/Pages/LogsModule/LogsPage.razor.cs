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
            List<WhereFilter> filters = new();
            DataManagerRequest request = new()
            {
                Take = state.PageSize,
                Skip = state.PageSize * state.Page,
                Where = filters
            };
            string field = state.SortLabel ?? nameof(Logs.TimeOfAction);
            string order = state.SortDirection == SortDirection.Ascending ? "ASC" : "DESC";
            var items = await _service.GetForGridAsync<Logs>(request, field, order);
            int? count = await _service.GetTotalCountAsync<Logs>(request);
            return new TableData<Logs>()
            {
                Items = items,
                TotalItems = count ?? 0
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
