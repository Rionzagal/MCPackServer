namespace MCPackServer.Models
{
    public enum Actions { Insert, Update, Delete }

    public class ActionResponse<T>
    {
        public ActionResponse(T entity, Actions action)
        {
            Value = entity ?? throw new ArgumentNullException(nameof(entity));
            Action = action;
            IsSuccessful = false;
            Errors = new List<string>();
        }
        public bool IsSuccessful { get; private set; }
        public Actions Action { get; private set; }
        public List<string> Errors { get; private set; }
        public string? ExceptionText { get; private set; }
        public T Value { get; private set; }

        public void Success() => IsSuccessful = true;
        public void Failure(Exception ex)
        {
            IsSuccessful = false;
            ExceptionText = ex.ToString();
            Errors.Add(ex.Message);
            if (null != ex.InnerException)
                Errors.Add(ex.InnerException.Message);
        }
        public void AttachValue(T model) => Value = model;
    }
}
