using System;
using System.ComponentModel.DataAnnotations;

namespace MvcTemplate.Tests.Unit.Components.Extensions.Html
{
    public class BootstrapModel
    {
        [Required]
        public String Required { get; set; }
        public String NotRequired { get; set; }
        public Int64 RequiredValue { get; set; }
        public Int64? NotRequiredNullableValue { get; set; }
        public Decimal Number { get; set; }
        public DateTime? Date { get; set; }

        public String NotEditable { get; set; }

        [Editable(true)]
        public String EditableTrue { get; set; }

        [Editable(false)]
        public String EditableFalse { get; set; }

        public BootstrapModel Relation { get; set; }

        public BootstrapModel()
        {
            NotRequiredNullableValue = 45877;
            RequiredValue = 102055454454;
            NotRequired = "NotRequired";
            Required = "Required";
            Date = DateTime.Now;
            NotEditable = "Not";
            EditableFalse = "F";
            EditableTrue = "T";
            Number = 10.7854M;

            Relation = new BootstrapModel(true)
            {
                Date = new DateTime(2011, 01, 01, 1, 1, 1),
                NotRequired = "NotRequiredRelation",
                NotRequiredNullableValue = 5411,
                Required = "RequiredRelation",
                RequiredValue = 102055454422,
                NotEditable = "NotRelation",
                EditableFalse = "FRelation",
                EditableTrue = "TRelation",
                Number = 1.6666M
            };
        }
        private BootstrapModel(Boolean noInit)
        {
        }
    }
}
