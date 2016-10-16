using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Web;

namespace WebTools.Test
{
    [TestClass()]
    public class FilesTest
    {
        [TestMethod()]
        public void CorrectsInvalidFolderPath()
        {
            var folderPath = "/files";
            var fileName = "file.txt";

            var mockServer = new Mock<HttpServerUtilityBase>();
            mockServer.Setup(x => x.MapPath(folderPath + "/")).Returns(folderPath + "/");

            var mockFileUpload = new Mock<HttpPostedFileBase>();
            mockFileUpload.Setup(x => x.ContentLength).Returns(50);
            mockFileUpload.Setup(x => x.FileName).Returns(fileName);

            var url = Files.UploadFile(mockServer.Object, folderPath, mockFileUpload.Object);
            Assert.AreEqual(folderPath + "/" + fileName, url);
        }

        [TestMethod()]
        public void UploadFileTest()
        {
            var folderPath = "/files/";
            var fileName = "file.txt";

            var mockServer = new Mock<HttpServerUtilityBase>();
            mockServer.Setup(x => x.MapPath(folderPath)).Returns(folderPath);

            var mockFileUpload = new Mock<HttpPostedFileBase>();
            mockFileUpload.Setup(x => x.ContentLength).Returns(50);
            mockFileUpload.Setup(x => x.FileName).Returns(fileName);

            var url = Files.UploadFile(mockServer.Object, folderPath, mockFileUpload.Object);
            Assert.AreEqual(folderPath + fileName, url);
        }
    }
}