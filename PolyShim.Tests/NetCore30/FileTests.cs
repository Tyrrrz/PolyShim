using System.IO;
using FluentAssertions;
using Xunit;

namespace PolyShim.Tests.NetCore30;

public class FileTests
{
    [Fact]
    public void Move_Overwrite_TargetDoesNotExist_Test()
    {
        // Arrange
        var sourceFileName = Path.GetTempFileName();
        var destFileName = Path.GetTempFileName();

        try
        {
            // Path.GetTempFileName() pre-creates the file, so delete it to simulate non-existence
            File.Delete(destFileName);

            // Act
            File.Move(sourceFileName, destFileName, true);

            // Assert
            File.Exists(sourceFileName).Should().BeFalse();
            File.Exists(destFileName).Should().BeTrue();
        }
        finally
        {
            try
            {
                File.Delete(sourceFileName);
                File.Delete(destFileName);
            }
            catch
            {
                // Ignore
            }
        }
    }

    [Fact]
    public void Move_Overwrite_TargetExists_Test()
    {
        // Arrange
        var sourceFileName = Path.GetTempFileName();
        var destFileName = Path.GetTempFileName();

        try
        {
            // Act
            File.Move(sourceFileName, destFileName, true);

            // Assert
            File.Exists(sourceFileName).Should().BeFalse();
            File.Exists(destFileName).Should().BeTrue();
        }
        finally
        {
            try
            {
                File.Delete(sourceFileName);
                File.Delete(destFileName);
            }
            catch
            {
                // Ignore
            }
        }
    }
}
