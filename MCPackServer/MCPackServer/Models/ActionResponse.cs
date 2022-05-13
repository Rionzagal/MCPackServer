namespace MCPackServer.Models
{
    public class ActionResponse<T>
    {
        public ActionResponse(string? action = null)
        {
            Action = action ?? "Waiting...";
            IsSuccessful = false;
            Errors = new List<string>();
        }
        public bool IsSuccessful { get; private set; }
        public string Action { get; private set; }
        public List<string> Errors { get; private set; }
        public T? Value { get; private set; }

        public void Success() => IsSuccessful = true;
        public void Failure(IEnumerable<string>? errors = null, string? error = null)
        {
            IsSuccessful = false;
            if (!string.IsNullOrEmpty(error))
                Errors.Add(error);
            if (null != errors && errors.Any())
            {
                foreach (var item in errors)
                {
                    Errors.Add(item);
                }
            }
        }
        public void AttachValue(T model) => Value = model;
    }
}
