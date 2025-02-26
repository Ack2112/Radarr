using System;
using FluentValidation;
using NzbDrone.Core.Annotations;
using NzbDrone.Core.Parser.Model;
using NzbDrone.Core.Validation;

namespace NzbDrone.Core.CustomFormats
{
    public class IndexerFlagSpecificationValidator : AbstractValidator<IndexerFlagSpecification>
    {
        public IndexerFlagSpecificationValidator()
        {
            RuleFor(c => c.Value).NotEmpty();
            RuleFor(c => c.Value).Custom((qualityValue, context) =>
            {
                if (!Enum.IsDefined(typeof(IndexerFlags), qualityValue))
                {
                    context.AddFailure(string.Format("Invalid indexer flag condition value: {0}", qualityValue));
                }
            });
        }
    }

    public class IndexerFlagSpecification : CustomFormatSpecificationBase
    {
        private static readonly IndexerFlagSpecificationValidator Validator = new IndexerFlagSpecificationValidator();

        public override int Order => 4;
        public override string ImplementationName => "Indexer Flag";

        [FieldDefinition(1, Label = "Flag", Type = FieldType.Select, SelectOptions = typeof(IndexerFlags))]
        public int Value { get; set; }

        protected override bool IsSatisfiedByWithoutNegate(CustomFormatInput input)
        {
            return input.IndexerFlags.HasFlag((IndexerFlags)Value) == true;
        }

        public override NzbDroneValidationResult Validate()
        {
            return new NzbDroneValidationResult(Validator.Validate(this));
        }
    }
}
