using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Shared.ErrorModels
{
    public  class ValidationErrorResponse // Bad req
    {
        public int StatusCode { get; set; } = (int)HttpStatusCode.BadRequest;
        public string ErrorMessage { get; set; } = "Validation Failed";
        public IEnumerable <ValidationError> ValidationError { get; set; }
    }
}public class ValidationError
{
    public string Field { get; set; } = default!;
    public IEnumerable<string> Errors { get; set; } = [];

}
