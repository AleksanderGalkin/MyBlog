using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using NSubstitute;
using System.Web.Routing;
using System.Security.Principal;
using System.Collections.Specialized;
using System.Web.SessionState;
using System.IO;
using System.Reflection;

namespace PluginTextPostType.Test
{
    public class HttpContextFactory
    {
        private static HttpContextBase m_context;
        public static HttpContextBase Current
        {
            get
            {
                if (m_context != null)
                    return m_context;

                if (HttpContext.Current == null)
                    throw new InvalidOperationException("HttpContext not available");

                return new HttpContextWrapper(HttpContext.Current);
            }
        }

        public static HttpContext HttpContextCurrent
        {
            get
            {
                  return GethttpContext();
            }
        }

        public static void SetCurrentContext(HttpContextBase context)
        {
            m_context = context;
        }

        public static void SetCurrentContext()
        {
            m_context = GetMockedHttpContext();
        }

        private static HttpContextBase GetMockedHttpContext()
        {
            var context = Substitute.For<HttpContextBase>();
            var request = Substitute.For<HttpRequestBase>();
            var response = Substitute.For<HttpResponseBase>();
            var session = Substitute.For<HttpSessionStateBase>();
            var server = Substitute.For<HttpServerUtilityBase>();
            var user = Substitute.For<IPrincipal>();
            var identity = Substitute.For<IIdentity>();
            //var urlHelper = Substitute.For<UrlHelper>();

            var routes = new RouteCollection();
          //  MvcApplication.RegisterRoutes(routes);
            var requestContext = Substitute.For<RequestContext>();
            requestContext.HttpContext.Returns(context);
            context.Request.Returns(request);
            context.Response.Returns(response);
            context.Session.Returns(session);
            context.Server.Returns(server);
            context.User.Returns(user);
            user.Identity.Returns(identity);
            identity.IsAuthenticated.Returns(true);
            identity.Name.Returns("test");
            request.Url.Returns(new Uri("http://www.google.com"));
            request.RequestContext.Returns(requestContext);
            requestContext.RouteData.Returns(new RouteData());
            request.Headers.Returns(new NameValueCollection());

            return context;
        }

        private static HttpContext GethttpContext()
        {
            var httpRequest = new HttpRequest("", "http://mySomething/", "");
            var stringWriter = new StringWriter();
            var httpResponce = new HttpResponse(stringWriter);
            var httpContext = new HttpContext(httpRequest, httpResponce);

            var sessionContainer = new HttpSessionStateContainer("id", new SessionStateItemCollection(),
                                                                 new HttpStaticObjectsCollection(), 10, true,
                                                                 HttpCookieMode.AutoDetect,
                                                                 SessionStateMode.InProc, false);

            httpContext.Items["AspSession"] = typeof(HttpSessionState).GetConstructor(
                                                     BindingFlags.NonPublic | BindingFlags.Instance,
                                                     null, CallingConventions.Standard,
                                                     new[] { typeof(HttpSessionStateContainer) },
                                                     null)
                                                .Invoke(new object[] { sessionContainer });

            return httpContext;

        }

    }
}
