
//*************************************************************************************
//   !!! Generated by the fmp-cli 1.70.0.  DO NOT EDIT!
//*************************************************************************************

public abstract class BlazorUnitTestBase : IClassFixture<TestFixture>
{
    /// <summary>
    /// 测试上下文
    /// </summary>
    protected TestFixture fixture_ { get; set; }

    public BlazorUnitTestBase(TestFixture _testFixture)
    {
        fixture_ = _testFixture;
    }


    [Fact]
    public abstract Task CreateTest();

    [Fact]
    public abstract Task UpdateTest();

    [Fact]
    public abstract Task RetrieveTest();

    [Fact]
    public abstract Task DeleteTest();

    [Fact]
    public abstract Task ListTest();

    [Fact]
    public abstract Task SearchTest();

}
