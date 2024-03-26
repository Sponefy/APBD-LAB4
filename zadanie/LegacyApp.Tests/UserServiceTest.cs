namespace LegacyApp.Tests;

public class UserServiceTest
{
    [Fact]
    public void AddUser_ReturnsFalseWhenFirstNameIsEmpty()
    {
        // Arange
        var userService = new UserService();

        // Act
        var result = userService.AddUser(
            null,
            "Kowalski",
            "kowaliski@gmail.com",
            DateTime.Parse("2000-01-01"),
            1
        );

        //Assert
        Assert.False(result);
    }

    [Fact]
    public void AddUser_ThrowsExceptionWhenUserDoesNotExist()
    {
        // Arange
        var userService = new UserService();

        // Act
        Action action = () => userService.AddUser(
                "Jan",
                "Kowalski",
                "kowaliski@gmail.com",
                DateTime.Parse("2000-01-01"),
                100
            );


        //Assert
        Assert.Throws<ArgumentException>(action);
    }
}