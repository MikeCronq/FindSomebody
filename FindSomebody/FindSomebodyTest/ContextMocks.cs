using Moq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace FindSomebodyTest
{
    /// <summary>
    /// Contains mocks to provide web context to a controller in test.
    /// </summary>
    public class ContextMocks
    {
        /// <summary>
        /// The pages http context metadata.
        /// </summary>
        public Mock<HttpContextBase> HttpContextMock { get; set; }

        /// <summary>
        /// The pages request metadata.
        /// </summary>
        public Mock<HttpRequestBase> RequestMock { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="controller"></param>
        public ContextMocks(Controller controller)
        {
            RequestMock = new Mock<HttpRequestBase>();

            HttpContextMock = new Mock<HttpContextBase>();
            HttpContextMock.Setup(x => x.Request).Returns(RequestMock.Object);

            RequestContext rc = new RequestContext(HttpContextMock.Object, new RouteData());
            controller.ControllerContext = new ControllerContext(rc, controller);
        }
    }
}