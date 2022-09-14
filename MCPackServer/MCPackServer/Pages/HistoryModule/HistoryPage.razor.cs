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
            DataManagerRequest request = new()
            {
                Take = state.PageSize,
                Skip = state.PageSize * state.Page,
            };
            string field = state.SortLabel ?? nameof(HistoryView.TimeOfAction);
            string order = state.SortDirection == SortDirection.Ascending ? "ASC" : "DESC";
            return new TableData<HistoryView>()
            {
                Items = await _service.GetForGridAsync<HistoryView>(request, field, order),
                TotalItems = await _service.GetTotalCountAsync<HistoryView>(request) ?? 0
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
