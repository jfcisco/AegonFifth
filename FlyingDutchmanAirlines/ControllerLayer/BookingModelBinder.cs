using System;
using System.Buffers;
using System.IO.Pipelines;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using FlyingDutchmanAirlines.ControllerLayer.JsonData;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FlyingDutchmanAirlines.ControllerLayer
{
    public class BookingModelBinder : IModelBinder
    {
        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null) throw new ArgumentException();

            ReadResult result = await bindingContext.HttpContext.Request.BodyReader.ReadAsync();
            ReadOnlySequence<byte> buffer = result.Buffer;
            string body = Encoding.UTF8.GetString(buffer.FirstSpan);
            
            // Deserialize string body into BookingData type
            BookingData data = JsonSerializer.Deserialize<BookingData>(body);
            
            bindingContext.Result = ModelBindingResult.Success(data);
        }
    }
}