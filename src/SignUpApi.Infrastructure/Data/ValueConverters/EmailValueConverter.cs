using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SignUpApi.Domain.ValueObjects;

namespace SignUpApi.Infrastructure.Data.ValueConverters;

public class EmailValueConverter : ValueConverter<Email, string>
{
    public EmailValueConverter() : base(
        email => email.Value,
        value => Email.FromDatabase(value))
    {
    }
}
