using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MvcTemplate.Components.Mvc
{
    public class DataTypeValidatorProvider : ClientDataTypeModelValidatorProvider
    {
        private HashSet<Type> NumericTypes { get; set; }

        public DataTypeValidatorProvider()
        {
            NumericTypes = new HashSet<Type>
            {
              typeof (Byte),
              typeof (SByte),
              typeof (Int16),
              typeof (UInt16),
              typeof (Int32),
              typeof (UInt32),
              typeof (Int64),
              typeof (UInt64),
              typeof (Single),
              typeof (Double),
              typeof (Decimal)
            };
        }

        public override IEnumerable<ModelValidator> GetValidators(ModelMetadata metadata, ControllerContext context)
        {
            Type type = Nullable.GetUnderlyingType(metadata.ModelType) ?? metadata.ModelType;

            if (IsDateTimeType(type, metadata))
                yield return new DateValidator(metadata, context);
            if (IsNumericType(type))
                yield return new NumberValidator(metadata, context);
        }

        private Boolean IsDateTimeType(Type type, ModelMetadata metadata)
        {
            if (type != typeof(DateTime)) return false;

            return !String.Equals(metadata.DataTypeName, "Time", StringComparison.OrdinalIgnoreCase);
        }
        private Boolean IsNumericType(Type type)
        {
            return NumericTypes.Contains(type);
        }
    }
}
