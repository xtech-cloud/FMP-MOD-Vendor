using XTC.FMP.MOD.Vendor.LIB.Proto;

public class BlazorTest : BlazorUnitTestBase
{
    public BlazorTest(TestFixture _testFixture)
        : base(_testFixture)
    {
    }


    public override async Task CreateTest()
    {
        string uuid = "";
        {
            var request = new BlazorCreateRequest();
            request.Name = "test_create";
            request.Display = "test";
            var response = await fixture_.getServiceBlazor().Create(request, fixture_.context);
            Assert.Equal(0, response.Status.Code);
            uuid = response.Uuid;

            // ÷ÿ∏¥
            response = await fixture_.getServiceBlazor().Create(request, fixture_.context);
            Assert.Equal(1, response.Status.Code);
        }

        {
            var request = new UuidRequest();
            request.Uuid = uuid;
            var response = await fixture_.getServiceBlazor().Delete(request, fixture_.context);
            Assert.Equal(0, response.Status.Code);
        }
    }

    public override async Task UpdateTest()
    {
        string uuid = "";
        {
            var request = new BlazorCreateRequest();
            request.Name = "test_update";
            request.Display = "test";
            var response = await fixture_.getServiceBlazor().Create(request, fixture_.context);
            Assert.Equal(0, response.Status.Code);
            uuid = response.Uuid;
        }
        {

            var request = new BlazorUpdateRequest();
            request.Uuid = uuid;
            request.Name = "test_update_2";
            request.Display = "test2";
            request.Logo = "logo";
            request.SkinConfig = "skinconfig";
            request.MenuTitle = "menutitle";
            request.MenuConfig = "menuconfig";
            request.ModulesConfig = "modulesconfig";
            var response = await fixture_.getServiceBlazor().Update(request, fixture_.context);
            Assert.Equal(0, response.Status.Code);

            var request2 = new UuidRequest();
            request2.Uuid = uuid;
            var response2 = await fixture_.getServiceBlazor().Retrieve(request2, fixture_.context);
            Assert.Equal(0, response2.Status.Code);
            Assert.Equal("test_update_2", response2.Blazor.Name);
            Assert.Equal("test2", response2.Blazor.Display);
            Assert.Equal("logo", response2.Blazor.Logo);
            Assert.Equal("skinconfig", response2.Blazor.SkinConfig);
            Assert.Equal("menutitle", response2.Blazor.MenuTitle);
            Assert.Equal("menuconfig", response2.Blazor.MenuConfig);
            Assert.Equal("modulesconfig", response2.Blazor.ModulesConfig);
        }

        {
            var request = new UuidRequest();
            request.Uuid = uuid;
            var response = await fixture_.getServiceBlazor().Delete(request, fixture_.context);
            Assert.Equal(0, response.Status.Code);
        }
    }

    public override async Task RetrieveTest()
    {
        string uuid = "";
        {
            var request = new BlazorCreateRequest();
            request.Name = "test_retrieve";
            request.Display = "test";
            var response = await fixture_.getServiceBlazor().Create(request, fixture_.context);
            Assert.Equal(0, response.Status.Code);
            uuid = response.Uuid;
        }
        {
            var request = new UuidRequest();
            request.Uuid = uuid;
            var response = await fixture_.getServiceBlazor().Retrieve(request, fixture_.context);
            Assert.Equal(0, response.Status.Code);
            // ≤ª¥Ê‘⁄
            request.Uuid = Guid.NewGuid().ToString();
            response = await fixture_.getServiceBlazor().Retrieve(request, fixture_.context);
            Assert.Equal(1, response.Status.Code);
        }
        {
            var request = new UuidRequest();
            request.Uuid = uuid;
            var response = await fixture_.getServiceBlazor().Delete(request, fixture_.context);
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
                var request = new BlazorCreateRequest();
                request.Name = "list_" + i.ToString();
                var response = await fixture_.getServiceBlazor().Create(request, fixture_.context);
                Assert.Equal(0, response.Status.Code);
            }
        }
        {
            var request = new BlazorListRequest();
            request.Offset = 0;
            request.Count = 50;
            var response = await fixture_.getServiceBlazor().List(request, fixture_.context);
            Assert.Equal(0, response.Status.Code);
            Assert.Equal(10, response.Total);
            Assert.Equal(10, response.BlazorS.Count);

            request.Offset = 5;
            request.Count = 2;
            response = await fixture_.getServiceBlazor().List(request, fixture_.context);
            Assert.Equal(0, response.Status.Code);
            Assert.Equal(10, response.Total);
            Assert.Equal(2, response.BlazorS.Count);
            Assert.Equal("list_5", response.BlazorS.First().Name);
        }
        {
            foreach (var uuid in uuidS)
            {
                var request = new UuidRequest();
                request.Uuid = uuid;
                var response = await fixture_.getServiceBlazor().Delete(request, fixture_.context);
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
                var request = new BlazorCreateRequest();
                request.Name = "search_" + (i % 2 == 0 ? "#A#" : "#B#") + "_" + i.ToString();
                request.Display = i.ToString();
                var response = await fixture_.getServiceBlazor().Create(request, fixture_.context);
                Assert.Equal(0, response.Status.Code);
            }
        }

        {

            var request = new BlazorSearchRequest();
            request.Count = 50;
            var response = await fixture_.getServiceBlazor().Search(request, fixture_.context);
            Assert.Equal(0, response.Status.Code);
            Assert.Equal(0, response.Total);
            Assert.Empty(response.BlazorS);

            request.Count = 50;
            request.Name = "#A#";
            response = await fixture_.getServiceBlazor().Search(request, fixture_.context);
            Assert.Equal(0, response.Status.Code);
            Assert.Equal(5, response.Total);
            Assert.Equal(5, response.BlazorS.Count);

            request.Offset = 2;
            request.Count = 2;
            request.Name = "#A#";
            response = await fixture_.getServiceBlazor().Search(request, fixture_.context);
            Assert.Equal(0, response.Status.Code);
            Assert.Equal(5, response.Total);
            Assert.Equal(2, response.BlazorS.Count);
            Assert.Equal("search_#A#_4", response.BlazorS.First().Name);

            request.Offset = 0;
            request.Count = 2;
            request.Name = "#A#";
            request.Display = "2";
            response = await fixture_.getServiceBlazor().Search(request, fixture_.context);
            Assert.Equal(0, response.Status.Code);
            Assert.Equal(1, response.Total);
            Assert.Single(response.BlazorS);
            Assert.Equal("search_#A#_2", response.BlazorS.First().Name);
        }
    }

}
