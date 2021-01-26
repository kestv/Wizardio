using System;
namespace Assets.Scripts.Api.Dto
{
    [Serializable]
    public class Response<T>
    {
        public string message;
        public bool hasErrors;
        public T entity;
    }
}
