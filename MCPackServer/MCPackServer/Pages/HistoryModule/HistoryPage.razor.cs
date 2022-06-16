using MCPackServer.Entities;
using MCPackServer.Models;
using MudBlazor;

namespace MCPackServer.Pages.HistoryModule
{
    public partial class HistoryPage
    {
        #region MudComponents
        MudTable<HistoryView> LogsTable = new();
        #endregion

        #region Page variables
        private bool VisibleLog = false;
        HistoryView SelectedLog = new();
        #endregion

        #region Logs related methods
        private async Task<TableData<HistoryView>> LogsServerReload(TableState state)
        {
            List<WhereFilter> filters = new();
            DataManagerRequest request = new()
            {
                Take = state.PageSize,
                Skip = state.PageSize * state.Page,
                Where = filters
            };
            string field = state.SortLabel ?? nameof(HistoryView.TimeOfAction);
            string order = state.SortDirection == SortDirection.Ascending ? "ASC" : "DESC";
            var items = await _service.GetForGridAsync<HistoryView>(request, field, order);
            int? count = await _service.GetTotalCountAsync<HistoryView>(request);
            return new TableData<HistoryView>()
            {
                Items = items,
                TotalItems = count ?? 0
            };
        }
        private void OnSelectedLog(TableRowClickEventArgs<HistoryView> args)
        {
            SelectedLog = args.Item;
            VisibleLog = true;
        }
        #endregion
    }
}
