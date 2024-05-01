using Microsoft.AspNetCore.Authentication;
using System.Net.Http.Headers;

namespace EMStore.Services.ShoppingCartAPI.Utility
{
    public class BackendAPIAuthenticationHttpClientHandler(IHttpContextAccessor contextAccessor) : DelegatingHandler
    {
        private readonly IHttpContextAccessor _contextAccessor = contextAccessor;

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        { 
            var token = await _contextAccessor.HttpContext.GetTokenAsync("access_token") ?? string.Empty;

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
