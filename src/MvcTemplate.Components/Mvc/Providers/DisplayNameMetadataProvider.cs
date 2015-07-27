using MvcTemplate.Resources;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MvcTemplate.Components.Mvc
{
    public class DisplayNameMetadataProvider : DataAnnotationsModelMetadataProvider
    {
        protected override ModelMetadata CreateMetadata(IEnumerable<Attribute> attributes, Type containerType, Func<Object> modelAccessor, Type modelType, String propertyName)
        {
            ModelMetadata metadata = base.CreateMetadata(attributes, containerType, modelAccessor, modelType, propertyName);
            if (containerType != null) metadata.DisplayName = ResourceProvider.GetPropertyTitle(containerType, propertyName);

            return metadata;
        }
    }
}
