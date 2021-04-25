namespace Infrastructure.Builders
{
    public class FundaRequestUriBuilder
    {
        private string _responseContentType;
        private string _accessToken;

        private string _queryParams;

        public string Build()
        {
            return $"/feeds/Aanbod.svc/{_responseContentType}/{_accessToken}/{_queryParams}";
        }

        public FundaRequestUriBuilder WithResponseContentType(string responseContentType)
        {
            _responseContentType = responseContentType;
            return this;
        }

        public FundaRequestUriBuilder WithAccessToken(string AccessToken)
        {
            _accessToken = AccessToken;
            return this;
        }

        public FundaRequestUriBuilder WithType(string type)
        {
            _queryParams += CreateNewQueryParam("type", type);

            return this;
        }

        public FundaRequestUriBuilder WithSearch(string search)
        {
            _queryParams += CreateNewQueryParam("zo", search);

            return this;
        }

        public FundaRequestUriBuilder WithPageSize(int pageSize)
        {
            _queryParams += CreateNewQueryParam("pagesize", pageSize.ToString());

            return this;
        }

        public FundaRequestUriBuilder WithPageIndex(int pageIndex)
        {
            _queryParams += CreateNewQueryParam("page", pageIndex.ToString());

            return this;
        }

        private string CreateNewQueryParam(string key, string value)
        {
            var prepend = string.IsNullOrWhiteSpace(_queryParams) ? "?" : "&";
            return $"{prepend}{key}={value}";
        }
    }
}
