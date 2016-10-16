using Moq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace FindSomebodyTest
{
    /// <summary>
    /// Mock away the extraneous contexts for a controller.
    /// </summary>
    public class ContextMocks
    {
        public Mock<HttpContextBase> HttpContextMock { get; set; }
        public Mock<HttpRequestBase> RequestMock { get; set; }

        public ContextMocks(Controller controller)
        {
            //define context objects
            RequestMock = new Mock<HttpRequestBase>();

            HttpContextMock = new Mock<HttpContextBase>();
            HttpContextMock.Setup(x => x.Request).Returns(RequestMock.Object);

            //apply context to controller
            RequestContext rc = new RequestContext(HttpContextMock.Object, new RouteData());
            controller.ControllerContext = new ControllerContext(rc, controller);
        }
    }
}