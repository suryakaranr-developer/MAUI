using CompanyName.AppName.Core.ViewModels;
using Xunit;

namespace CompanyName.AppName.Core.Tests;

public class SystemDesignViewModelTests
{
    [Fact]
    public async Task ViewButtons_navigates_to_the_buttons_route()
    {
        var navigation = new FakeNavigationService();
        var viewModel = new SystemDesignViewModel(navigation);

        await viewModel.ViewButtonsCommand.ExecuteAsync(null);

        Assert.Equal(new[] { "Buttons" }, navigation.Routes);
    }

    [Theory]
    [InlineData("MaterialButtons")]
    [InlineData("MaterialColorRoles")]
    [InlineData("MaterialProgress")]
    public async Task ViewMaterial_navigates_to_the_route_it_is_given(string route)
    {
        var navigation = new FakeNavigationService();
        var viewModel = new SystemDesignViewModel(navigation);

        await viewModel.ViewMaterialCommand.ExecuteAsync(route);

        Assert.Equal(new[] { route }, navigation.Routes);
    }

    [Fact]
    public void SetStatus_updates_state_and_raises_change_notification()
    {
        var viewModel = new SystemDesignViewModel(new FakeNavigationService());
        var changed = new List<string?>();
        viewModel.PropertyChanged += (_, e) => changed.Add(e.PropertyName);

        viewModel.SetStatus(loading: true, status: "Working", statusMessage: "Please wait");

        Assert.True(viewModel.IsLoading);
        Assert.Equal("Working", viewModel.Status);
        Assert.Equal("Please wait", viewModel.StatusMessage);
        Assert.Contains("IsLoading", changed);
        Assert.Contains("Status", changed);
    }
}
