using System;

namespace Shop.Domain
{
    [Serializable]
    public class ItemsChangingResponse
    {
        public bool IsSuccess;
        public string ErrorText;

        public ItemsChangingResponse()
        { }

        public ItemsChangingResponse(bool isSuccess) : this(isSuccess, string.Empty)
        { }

        public ItemsChangingResponse(bool isSuccess, string errorText)
        {
            IsSuccess = isSuccess;
            ErrorText = errorText;
        }
    }
}
