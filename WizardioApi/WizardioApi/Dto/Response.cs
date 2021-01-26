namespace WizardioApi.Dto
{
    public class Response<T>
    {
        public string Message { get; set; }
        public bool HasErrors { get; set; }
        public T Entity { get; set; }
    }
}
