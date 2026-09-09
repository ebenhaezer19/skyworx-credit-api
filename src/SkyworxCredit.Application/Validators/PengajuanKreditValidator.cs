using FluentValidation;
using SkyworxCredit.Application.DTOs;

namespace SkyworxCredit.Application.Validators;

public class PengajuanKreditValidator : AbstractValidator<PengajuanKreditRequest>
{
    public PengajuanKreditValidator()
    {
        RuleFor(x => x.Plafon)
            .GreaterThan(0)
            .WithMessage("Plafon harus lebih dari 0");

        RuleFor(x => x.Bunga)
            .InclusiveBetween(0, 100)
            .WithMessage("Bunga harus antara 0 - 100 persen");

        RuleFor(x => x.Tenor)
            .GreaterThan(0)
            .WithMessage("Tenor harus lebih dari 0 bulan");
    }
}