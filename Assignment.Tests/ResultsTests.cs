using Assignment.Results;
using Xunit;

namespace Assignment.Tests;

public class ResultsTests
{
    [Fact]
    public void PaginatedList_UsesDefaultPageSize()
    {
        var result = new PaginatedList<string>();

        Assert.Equal(10, result.PageSize);
        Assert.Equal(1, result.PageNumber);
    }

    [Fact]
    public void PaginatedList_ClampsPageSizeToMaximum()
    {
        var result = new PaginatedList<string> { PageSize = 25 };

        Assert.Equal(20, result.PageSize);
    }

    [Fact]
    public void SuccessResult_HasSuccessStateAndNoError()
    {
        var result = Result.Success();

        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Equal(string.Empty, result.Error);
    }

    [Fact]
    public void FailureResult_HasFailureStateAndError()
    {
        var result = Result.Failure("Invalid request");

        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal("Invalid request", result.Error);
        Assert.Equal(ErrorKind.Validation, result.Kind);
    }

    [Fact]
    public void GenericResult_Success_CarriesValue()
    {
        var result = GenericResult<int>.Success(42);

        Assert.True(result.IsSuccess);
        Assert.Equal(42, result.Value);
    }

    [Fact]
    public void GenericResult_NotFound_HasNoValueAndNotFoundKind()
    {
        var result = GenericResult<string>.Failure("missing", ErrorKind.NotFound);

        Assert.True(result.IsFailure);
        Assert.Null(result.Value);
        Assert.Equal(ErrorKind.NotFound, result.Kind);
    }
}
