using Xunit;
using PersonalAccountOfStudent.Controllers;
using Microsoft.AspNetCore.Hosting;
using Moq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace XUnitTestProject1
{
    public class ProgressControllerTest
    {
        [Fact]
        public async Task DownloadProgressReturnsEmptyResult()
        {
            var mockRepo = new Mock<IHostingEnvironment>();
            mockRepo.Setup(repo => repo.WebRootPath).Returns("");
            var controller = new ProgressController(mockRepo.Object, null);

            var result = await controller.DownloadProgress();

            Assert.IsType<EmptyResult>(result);
        }

        [Fact]
        public async Task ViewProgressViewDataFileExist()
        {
            var mockRepo = new Mock<IHostingEnvironment>();
            mockRepo.Setup(repo => repo.WebRootPath).Returns("");
            var controller = new ProgressController(mockRepo.Object, null);

            var result = controller.ViewProgress() as ViewResult;
            Assert.False((bool)(result?.ViewData["FileExist"]));
        }
    }
}
