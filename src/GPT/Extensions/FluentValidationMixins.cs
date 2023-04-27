namespace FluentValidation;

public static class FluentValidationMixins
{
    public static IRuleBuilder<T, string> ValidEmail<T>(this IRuleBuilder<T, string> builder)
        => builder
        .NotEmpty()
        .EmailAddress()
        .Length(3, 25)
        .WithName("Email");

    public static IRuleBuilder<T, string> ValidPassword<T>(this IRuleBuilder<T, string> builder)
        => builder
        .NotEmpty()
        .MinimumLength(10)
        .WithName("Password");

    public static IRuleBuilder<T, string> ValidName<T>(this IRuleBuilder<T, string> builder)
        => builder
        .NotEmpty()
        .Length(3, 25)
        .WithName("Name");

    public static IRuleBuilder<T, string> ValidDescription<T>(this IRuleBuilder<T, string> builder)
        => builder
        .NotEmpty()
        .Length(3, 25)
        .WithName("Description");
}
