using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Principal;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace Microsoft.AspNetCore.Authentication.QQConnect
{
    internal class QQConnectHandler : OAuthHandler<QQConnectOptions>
    {
        public QQConnectHandler(IOptionsMonitor<QQConnectOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        protected override string FormatScope()
        {
            return string.Join(",", Options.Scope);
        }

        protected override string BuildChallengeUrl(AuthenticationProperties properties, string redirectUri)
        {
            var url = base.BuildChallengeUrl(properties, redirectUri);
            if (Options.IsMobile) url += "&display=mobile";
            return url;
        }

        protected override async Task<OAuthTokenResponse> ExchangeCodeAsync(string code, string redirectUri)
        {
            var query = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "client_id", Options.ClientId },
                { "redirect_uri", redirectUri },
                { "client_secret", Options.ClientSecret},
                { "code", code},
                { "grant_type","authorization_code"}
            });
            var message = new HttpRequestMessage(HttpMethod.Post, Options.TokenEndpoint)
            {
                Content = query
            };
            message.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await Backchannel.SendAsync(message, Context.RequestAborted);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();

            result = "{\"" + result.Replace("=", "\":\"").Replace("&", "\",\"") + "\"}";
            return OAuthTokenResponse.Success(JObject.Parse(result));
        }

        protected override async Task<AuthenticationTicket> CreateTicketAsync(ClaimsIdentity identity, AuthenticationProperties properties, OAuthTokenResponse tokens)
        {
            var facebookHandler = this;
            var openIdEndpoint = $"{Options.OpenIdEndpoint}?access_token={tokens.AccessToken}";
            var response = await Backchannel.GetAsync(openIdEndpoint, Context.RequestAborted);
            response.EnsureSuccessStatusCode();

            var tmp = await response.Content.ReadAsStringAsync();
            var regex = new System.Text.RegularExpressions.Regex("callback\\((?<json>[ -~]+)\\);");
            var json = JObject.Parse(regex.Match(tmp).Groups["json"].Value);
            var identifier = json.Value<string>("openid");
            if (!string.IsNullOrEmpty(identifier))
            {
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, identifier, ClaimValueTypes.String, Options.ClaimsIssuer));
                identity.AddClaim(new Claim("urn:qq:id", identifier, ClaimValueTypes.String, Options.ClaimsIssuer));
            }

            var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                {"oauth_consumer_key", Options.ClientId},
                {"access_token", tokens.AccessToken},
                {"openid", identifier}
            });
            response = await Backchannel.PostAsync(Options.UserInformationEndpoint, content);
            response.EnsureSuccessStatusCode();
            var info = JObject.Parse(await response.Content.ReadAsStringAsync());
            info.Add("id", identifier);
            var name = info.Value<string>("nickname");
            if (!string.IsNullOrEmpty(name))
            {
                identity.AddClaim(new Claim(ClaimTypes.Name, name, ClaimValueTypes.String, Options.ClaimsIssuer));
                identity.AddClaim(new Claim("urn:qq:name", name, ClaimValueTypes.String, Options.ClaimsIssuer));
            }
            var figure = info.Value<string>("figureurl_qq_1");
            if (!string.IsNullOrEmpty(name))
            {
                identity.AddClaim(new Claim("urn:qq:figure", figure, ClaimValueTypes.String, Options.ClaimsIssuer));
            }
            //JObject user = JObject.Parse(await async.Content.ReadAsStringAsync());
            OAuthCreatingTicketContext context = new OAuthCreatingTicketContext(new ClaimsPrincipal(identity), properties, facebookHandler.Context, facebookHandler.Scheme, facebookHandler.Options, facebookHandler.Backchannel, tokens);
            context.RunClaimActions();
            await facebookHandler.Events.CreatingTicket(context);
            return new AuthenticationTicket(context.Principal, context.Properties, facebookHandler.Scheme.Name);
        }
    }
}