using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CourseLibrary.API.Helpers
{
    public class ArrayModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            // Our binder works only on enumerable types
            if (!bindingContext.ModelMetadata.IsEnumerableType)
            {
                bindingContext.Result = ModelBindingResult.Failed();
                return Task.CompletedTask;
            }

            // Get the inputted value through the value provider
            var value = bindingContext.ValueProvider
                .GetValue(bindingContext.ModelName).ToString();

            // If that value is null or whitespace, we return null
            if (string.IsNullOrWhiteSpace(value))
            {
                bindingContext.Result = ModelBindingResult.Success(null);
                return Task.CompletedTask;
            }

            // The value isn't null or whitespace,
            // and the type of the model is enumerable.
            // Get the enumerable's type, and a converter

            // We need to get the actual enumerables type and we need a converter to convert it to the GUIDs we need.
            // So we use a bit of reflection for that.
            // We call GetTypeInfo on the model type, and on that we get the first GenericTypeArgument that will be GUIDS in our case. 
            var elementType = bindingContext.ModelType.GetTypeInfo().GenericTypeArguments[0];
            // Then we create a converter for that GUID
            var converter = TypeDescriptor.GetConverter(elementType);

            // Then convert each GUID string to an actual GUID 

            // Convert each item in the value list to the enumerable type
            var values = value.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => converter.ConvertFrom(x.Trim()))
                .ToArray();

            // and we create an array of GUIDs from it. Again we use a bit of reflection for that (Array.CreateInstance)

            // Create an array of that type, and set it as the Model value
            var typedValues = Array.CreateInstance(elementType, values.Length);
            // We copy over all our values from values to our new typed values array,
            values.CopyTo(typedValues, 0);
            // and we set the model on our bindingContext to these typedValues.
            bindingContext.Model = typedValues;

            // Finally our model is now an array of GUID 

            // return a successful result, passing in the Model
            bindingContext.Result = ModelBindingResult.Success(bindingContext.Model);
            return Task.CompletedTask;

        }
    }
}
