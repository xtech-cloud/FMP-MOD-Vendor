
using XTC.FMP.MOD.Vendor.LIB.Proto;

public class UnityTest : UnityUnitTestBase
{
    public UnityTest(TestFixture _testFixture)
        : base(_testFixture)
    {
    }


    public override async Task CreateTest()
    {
        string uuid = "";
        {
            var request = new UnityCreateRequest();
            request.Name = "test_create";
            request.Display = "test";
            var response = await fixture_.getServiceUnity().Create(request, fixture_.context);
            Assert.Equal(0, response.Status.Code);
            uuid = response.Uuid;

            // ÷ÿ∏¥
            response = await fixture_.getServiceUnity().Create(request, fixture_.context);
            Assert.Equal(1, response.Status.Code);
        }

        {
            var request = new UuidRequest();
            request.Uuid = uuid;
            var response = await fixture_.getServiceUnity().Delete(request, fixture_.context);
            Assert.Equal(0, response.Status.Code);
        }
    }

    public override async Task UpdateTest()
    {
        string uuid = "";
        {
            var request = new UnityCreateRequest();
            request.Name = "test_update";
            request.Display = "test";
            var response = await fixture_.getServiceUnity().Create(request, fixture_.context);
            Assert.Equal(0, response.Status.Code);
            uuid = response.Uuid;
        }
        {

            var request = new UnityUpdateRequest();
            request.Uuid = uuid;
            request.Name = "test_update_2";
            request.Display = "test2";
            request.SkinSplashBackground = "b";
            request.SkinSplashSlogan = "s";
            request.GraphicsFps = 24;
            request.GraphicsQuality = 1;
            request.GraphicsPixelResolution = "1366x768";
            request.GraphicsReferenceResolutionWidth = 1366;
            request.GraphicsReferenceResolutionHeight = 768;
            request.GraphicsReferenceResolutionMatch = 0.5f;
            request.Application = "FMP";
            request.DependencyConfig = "Dependency";
            request.BootloaderConfig = "Bootloader";
            request.UpgradeConfig = "Upgrade";
            var response = await fixture_.getServiceUnity().Update(request, fixture_.context);
            Assert.Equal(0, response.Status.Code);

            var request2 = new UuidRequest();
            request2.Uuid = uuid;
            var response2 = await fixture_.getServiceUnity().Retrieve(request2, fixture_.context);
            Assert.Equal(0, response2.Status.Code);
            Assert.Equal("test_update_2", response2.Unity.Name);
            Assert.Equal("test2", response2.Unity.Display);
            Assert.Equal("b", response2.Unity.SkinSplashBackground);
            Assert.Equal("s", response2.Unity.SkinSplashSlogan);
            Assert.Equal(24, response2.Unity.GraphicsFps);
            Assert.Equal(1, response2.Unity.GraphicsQuality);
            Assert.Equal("1366x768", response2.Unity.GraphicsPixelResolution);
            Assert.Equal(1366, response2.Unity.GraphicsReferenceResolutionWidth);
            Assert.Equal(768, response2.Unity.GraphicsReferenceResolutionHeight);
            Assert.Equal(0.5f, response2.Unity.GraphicsReferenceResolutionMatch);
            Assert.Equal("FMP", response2.Unity.Application);
            Assert.Equal("Dependency", response2.Unity.DependencyConfig);
            Assert.Equal("Bootloader", response2.Unity.BootloaderConfig);
            Assert.Equal("Upgrade", response2.Unity.UpgradeConfig);
        }

        {
            var request = new UuidRequest();
            request.Uuid = uuid;
            var response = await fixture_.getServiceUnity().Delete(request, fixture_.context);
            Assert.Equal(0, response.Status.Code);
        }
    }

    public override async Task RetrieveTest()
    {
        string uuid = "";
        {
            var request = new UnityCreateRequest();
            request.Name = "test_retrieve";
            request.Display = "test";
            var response = await fixture_.getServiceUnity().Create(request, fixture_.context);
            Assert.Equal(0, response.Status.Code);
            uuid = response.Uuid;
        }
        {
            var request = new UuidRequest();
            request.Uuid = uuid;
            var response = await fixture_.getServiceUnity().Retrieve(request, fixture_.context);
            Assert.Equal(0, response.Status.Code);
            // ≤ª¥Ê‘⁄
            request.Uuid = Guid.NewGuid().ToString();
            response = await fixture_.getServiceUnity().Retrieve(request, fixture_.context);
            Assert.Equal(1, response.Status.Code);
        }
        {
            var request = new UuidRequest();
            request.Uuid = uuid;
            var response = await fixture_.getServiceUnity().Delete(request, fixture_.context);
            Assert.Equal(0, response.Status.Code);
        }
    }

    public override async Task DeleteTest()
    {
    }

    public override async Task ListTest()
    {
        List<string> uuidS = new List<string>();

        {
            for (int i = 0; i < 10; i++)
            {
                var request = new UnityCreateRequest();
                request.Name = "list_" + i.ToString();
                var response = await fixture_.getServiceUnity().Create(request, fixture_.context);
                Assert.Equal(0, response.Status.Code);
            }
        }
        {
            var request = new UnityListRequest();
            request.Offset = 0;
            request.Count = 50;
            var response = await fixture_.getServiceUnity().List(request, fixture_.context);
            Assert.Equal(0, response.Status.Code);
            Assert.Equal(10, response.Total);
            Assert.Equal(10, response.UnityS.Count);

            request.Offset = 5;
            request.Count = 2;
            response = await fixture_.getServiceUnity().List(request, fixture_.context);
            Assert.Equal(0, response.Status.Code);
            Assert.Equal(10, response.Total);
            Assert.Equal(2, response.UnityS.Count);
            Assert.Equal("list_5", response.UnityS.First().Name);
        }
        {
            foreach (var uuid in uuidS)
            {
                var request = new UuidRequest();
                request.Uuid = uuid;
                var response = await fixture_.getServiceUnity().Delete(request, fixture_.context);
                Assert.Equal(0, response.Status.Code);
            }
        }
    }

    public override async Task SearchTest()
    {
        List<string> uuidS = new List<string>();

        {
            for (int i = 0; i < 10; i++)
            {
                var request = new UnityCreateRequest();
                request.Name = "search_" + (i % 2 == 0 ? "#A#" : "#B#") + "_" + i.ToString();
                request.Display = i.ToString();
                var response = await fixture_.getServiceUnity().Create(request, fixture_.context);
                Assert.Equal(0, response.Status.Code);
            }
        }

        {

            var request = new UnitySearchRequest();
            request.Count = 50;
            var response = await fixture_.getServiceUnity().Search(request, fixture_.context);
            Assert.Equal(0, response.Status.Code);
            Assert.Equal(0, response.Total);
            Assert.Empty(response.UnityS);

            request.Count = 50;
            request.Name = "#A#";
            response = await fixture_.getServiceUnity().Search(request, fixture_.context);
            Assert.Equal(0, response.Status.Code);
            Assert.Equal(5, response.Total);
            Assert.Equal(5, response.UnityS.Count);

            request.Offset = 2;
            request.Count = 2;
            request.Name = "#A#";
            response = await fixture_.getServiceUnity().Search(request, fixture_.context);
            Assert.Equal(0, response.Status.Code);
            Assert.Equal(5, response.Total);
            Assert.Equal(2, response.UnityS.Count);
            Assert.Equal("search_#A#_4", response.UnityS.First().Name);

            request.Offset = 0;
            request.Count = 2;
            request.Name = "#A#";
            request.Display = "2";
            response = await fixture_.getServiceUnity().Search(request, fixture_.context);
            Assert.Equal(0, response.Status.Code);
            Assert.Equal(1, response.Total);
            Assert.Single(response.UnityS);
            Assert.Equal("search_#A#_2", response.UnityS.First().Name);
        }
    }

}
